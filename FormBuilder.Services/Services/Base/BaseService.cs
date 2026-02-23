using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using formBuilder.Domian.Entitys;
using formBuilder.Domian.Interfaces;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FormBuilder.Services.Services.Base
{
    /// <summary>
    /// Generic base service to reduce duplicated CRUD logic.
    /// </summary>
    public abstract class BaseService<TEntity, TDto, TCreateDto, TUpdateDto>
        where TEntity : BaseEntity
    {
        protected readonly IunitOfwork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly IStringLocalizer<BaseService<TEntity, TDto, TCreateDto, TUpdateDto>>? _localizer;

        protected BaseService(IunitOfwork unitOfWork, IMapper mapper, IStringLocalizer<BaseService<TEntity, TDto, TCreateDto, TUpdateDto>>? localizer = null)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _localizer = localizer;
        }

        protected abstract IBaseRepository<TEntity> Repository { get; }

        public virtual async Task<ServiceResult<IEnumerable<TDto>>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            // Use repository's GetAll and apply filters with IsDeleted exclusion
            var query = Repository.GetAll().Where(e => !e.IsDeleted);
            if (filter != null)
            {
                query = query.Where(filter);
            }

            var data = await query.ToListAsync();
            var mapped = _mapper.Map<IEnumerable<TDto>>(data);
            return ServiceResult<IEnumerable<TDto>>.Ok(mapped);
        }

        public virtual async Task<ServiceResult<PagedResult<TDto>>> GetPagedAsync(int page = 1, int pageSize = 20, Expression<Func<TEntity, bool>>? filter = null)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 20;

            var query = Repository.GetAll().Where(e => !e.IsDeleted); // Exclude soft-deleted entities
            if (filter != null) query = query.Where(filter);

            var totalCount = await query.CountAsync();
            var entities = await query
                .OrderBy(e => e.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mapped = _mapper.Map<IEnumerable<TDto>>(entities);
            var paged = new PagedResult<TDto>(mapped, totalCount, page, pageSize);

            return ServiceResult<PagedResult<TDto>>.Ok(paged);
        }

        public virtual async Task<ServiceResult<TDto>> GetByIdAsync(int id, bool asNoTracking = true)
        {
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id && !e.IsDeleted, asNoTracking);
            if (entity == null)
            {
                var message = _localizer?["Common_ResourceNotFound"] ?? "Resource not found";
                return ServiceResult<TDto>.NotFound(message);
            }

            return ServiceResult<TDto>.Ok(_mapper.Map<TDto>(entity));
        }

        public virtual async Task<ServiceResult<TDto>> CreateAsync(TCreateDto dto)
        {
            if (dto == null)
            {
                var message = _localizer?["Common_PayloadRequired"] ?? "Payload is required";
                return ServiceResult<TDto>.BadRequest(message);
            }

            var validation = await ValidateCreateAsync(dto);
            if (!validation.IsValid)
            {
                var message = validation.ErrorMessage ?? (_localizer?["Common_ValidationFailed"] ?? "Validation failed");
                return ServiceResult<TDto>.BadRequest(message);
            }

            var entity = _mapper.Map<TEntity>(dto);
            entity.CreatedDate = entity.CreatedDate == default ? DateTime.UtcNow : entity.CreatedDate;
            entity.IsActive = true;
            entity.IsDeleted = false; // Ensure IsDeleted is set to false for new entities

            Repository.Add(entity);
            await _unitOfWork.CompleteAsyn();

            return ServiceResult<TDto>.Ok(_mapper.Map<TDto>(entity));
        }

        public virtual async Task<ServiceResult<TDto>> UpdateAsync(int id, TUpdateDto dto)
        {
            if (dto == null)
            {
                var message = _localizer?["Common_PayloadRequired"] ?? "Payload is required";
                return ServiceResult<TDto>.BadRequest(message);
            }

            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id && !e.IsDeleted, asNoTracking: false);
            if (entity == null)
            {
                var message = _localizer?["Common_ResourceNotFound"] ?? "Resource not found or has been deleted";
                return ServiceResult<TDto>.NotFound(message);
            }

            var validation = await ValidateUpdateAsync(id, dto, entity);
            if (!validation.IsValid)
            {
                var message = validation.ErrorMessage ?? (_localizer?["Common_ValidationFailed"] ?? "Validation failed");
                return ServiceResult<TDto>.BadRequest(message);
            }

            _mapper.Map(dto, entity);
            entity.UpdatedDate = DateTime.UtcNow;

            Repository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            return ServiceResult<TDto>.Ok(_mapper.Map<TDto>(entity));
        }

        public virtual async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id && !e.IsDeleted, asNoTracking: false);
            if (entity == null)
            {
                var message = _localizer?["Common_ResourceNotFound"] ?? "Resource not found";
                return ServiceResult<bool>.NotFound(message);
            }

            // Soft Delete - لا نحذف فعلياً
            entity.IsDeleted = true;
            entity.DeletedDate = DateTime.UtcNow;
            entity.IsActive = false;
            Repository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            return ServiceResult<bool>.Ok(true);
        }

        protected virtual Task<ValidationResult> ValidateCreateAsync(TCreateDto dto) => Task.FromResult(ValidationResult.Success());

        protected virtual Task<ValidationResult> ValidateUpdateAsync(int id, TUpdateDto dto, TEntity entity) =>
            Task.FromResult(ValidationResult.Success());

        // Helper method for code validation (can be overridden in derived classes)
        protected virtual Task<bool> CodeExistsAsync(string code, int? excludeId = null)
        {
            // Default implementation - should be overridden in derived classes if needed
            return Task.FromResult(false);
        }

        // Helper method for soft delete using IsDeleted flag
        public virtual async Task<ServiceResult<bool>> SoftDeleteAsync(int id, string? deletedByUserId = null)
        {
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id && !e.IsDeleted, asNoTracking: false);
            if (entity == null)
            {
                var message = _localizer?["Common_ResourceNotFound"] ?? "Resource not found";
                return ServiceResult<bool>.NotFound(message);
            }

            // Use IsDeleted flag for soft delete (clear distinction from IsActive)
            entity.IsDeleted = true;
            entity.DeletedDate = DateTime.UtcNow;
            entity.DeletedByUserId = deletedByUserId;
            entity.UpdatedDate = DateTime.UtcNow;
            Repository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            return ServiceResult<bool>.Ok(true);
        }

        // Helper method for restore soft-deleted entity
        public virtual async Task<ServiceResult<TDto>> RestoreAsync(int id)
        {
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id && e.IsDeleted, asNoTracking: false);
            if (entity == null)
            {
                var message = _localizer?["Common_ResourceNotFound"] ?? "Deleted resource not found";
                return ServiceResult<TDto>.NotFound(message);
            }

            entity.IsDeleted = false;
            entity.DeletedDate = null;
            entity.DeletedByUserId = null;
            entity.UpdatedDate = DateTime.UtcNow;
            Repository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            return ServiceResult<TDto>.Ok(_mapper.Map<TDto>(entity));
        }

        // Helper method for toggle active - only works on non-deleted entities
        public virtual async Task<ServiceResult<TDto>> ToggleActiveAsync(int id, bool isActive)
        {
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id && !e.IsDeleted, asNoTracking: false);
            if (entity == null)
            {
                var message = _localizer?["Common_ResourceNotFound"] ?? "Resource not found or has been deleted";
                return ServiceResult<TDto>.NotFound(message);
            }

            entity.IsActive = isActive;
            entity.UpdatedDate = DateTime.UtcNow;
            Repository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            return ServiceResult<TDto>.Ok(_mapper.Map<TDto>(entity));
        }

        // Helper method for GetActive - excludes soft-deleted entities
        public virtual async Task<ServiceResult<IEnumerable<TDto>>> GetActiveAsync()
        {
            var query = Repository.GetAll().Where(e => e.IsActive && !e.IsDeleted);
            var data = await query.ToListAsync();
            var mapped = _mapper.Map<IEnumerable<TDto>>(data);
            return ServiceResult<IEnumerable<TDto>>.Ok(mapped);
        }
    }
}

