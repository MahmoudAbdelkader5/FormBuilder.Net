using AutoMapper;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using FormBuilder.Core.DTOS.FormTabs;
using FormBuilder.Domian.Entitys.FormBuilder;
using formBuilder.Domian.Interfaces;
using FormBuilder.Services.Services.Base;
using FormBuilder.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FormBuilder.Services.Services
{
    public class FormTabService
        : BaseService<FORM_TABS, FormTabDto, CreateFormTabDto, UpdateFormTabDto>,
          IFormTabService
    {
        private readonly IStringLocalizer<FormTabService>? _localizer;
        private readonly ILogger<FormTabService>? _logger;

        public FormTabService(IunitOfwork unitOfWork, IMapper mapper, IStringLocalizer<FormTabService>? localizer = null, ILogger<FormTabService>? logger = null)
            : base(unitOfWork, mapper, null)
        {
            _localizer = localizer;
            _logger = logger;
        }

        protected override IBaseRepository<FORM_TABS> Repository => _unitOfWork.FormTabRepository;

        public async Task<ServiceResult<IEnumerable<FormTabDto>>> GetAllAsync(Expression<Func<FORM_TABS, bool>>? filter = null)
            => await base.GetAllAsync(filter);

        public async Task<ServiceResult<PagedResult<FormTabDto>>> GetPagedAsync(int page = 1, int pageSize = 20, Expression<Func<FORM_TABS, bool>>? filter = null)
            => await base.GetPagedAsync(page, pageSize, filter);

        public async Task<ServiceResult<FormTabDto>> GetByIdAsync(int id, bool asNoTracking = false)
            => await base.GetByIdAsync(id, asNoTracking);

        public async Task<ServiceResult<FormTabDto>> GetByCodeAsync(string tabCode, bool asNoTracking = false)
        {
            if (string.IsNullOrWhiteSpace(tabCode))
            {
                var message = _localizer?["FormTab_TabCodeRequired"] ?? "Tab code is required";
                return ServiceResult<FormTabDto>.BadRequest(message);
            }

            var entity = await Repository.SingleOrDefaultAsync(t => t.TabCode == tabCode.Trim(), asNoTracking);
            if (entity == null) return ServiceResult<FormTabDto>.NotFound();

            return ServiceResult<FormTabDto>.Ok(_mapper.Map<FormTabDto>(entity));
        }

        public async Task<ServiceResult<IEnumerable<FormTabDto>>> GetByFormIdAsync(int formBuilderId)
        {
            var tabs = await _unitOfWork.FormTabRepository.GetTabsByFormIdAsync(formBuilderId);
            var dtos = _mapper.Map<IEnumerable<FormTabDto>>(tabs);
            return ServiceResult<IEnumerable<FormTabDto>>.Ok(dtos);
        }

        /// <summary>
        /// Get all deleted (soft deleted) tabs
        /// </summary>
        public async Task<ServiceResult<IEnumerable<FormTabDto>>> GetAllDeletedAsync()
        {
            var query = Repository.GetAll().Where(e => e.IsDeleted);
            var data = await query.ToListAsync();
            var mapped = _mapper.Map<IEnumerable<FormTabDto>>(data);
            return ServiceResult<IEnumerable<FormTabDto>>.Ok(mapped);
        }

        /// <summary>
        /// Get paged deleted (soft deleted) tabs
        /// </summary>
        public async Task<ServiceResult<PagedResult<FormTabDto>>> GetPagedDeletedAsync(int page = 1, int pageSize = 20)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 20;

            var query = Repository.GetAll().Where(e => e.IsDeleted);
            var totalCount = await query.CountAsync();
            var entities = await query
                .OrderByDescending(e => e.DeletedDate ?? e.UpdatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mapped = _mapper.Map<IEnumerable<FormTabDto>>(entities);
            var paged = new PagedResult<FormTabDto>(mapped, totalCount, page, pageSize);

            return ServiceResult<PagedResult<FormTabDto>>.Ok(paged);
        }

        /// <summary>
        /// Get all tabs including deleted ones
        /// </summary>
        public async Task<ServiceResult<IEnumerable<FormTabDto>>> GetAllIncludingDeletedAsync(Expression<Func<FORM_TABS, bool>>? filter = null)
        {
            var query = Repository.GetAll();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            var data = await query.ToListAsync();
            var mapped = _mapper.Map<IEnumerable<FormTabDto>>(data);
            return ServiceResult<IEnumerable<FormTabDto>>.Ok(mapped);
        }

        public override async Task<ServiceResult<FormTabDto>> CreateAsync(CreateFormTabDto dto)
        {
            return await base.CreateAsync(dto);
        }

        public override async Task<ServiceResult<FormTabDto>> UpdateAsync(int id, UpdateFormTabDto dto)
        {
            return await base.UpdateAsync(id, dto);
        }

        public override async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id && !e.IsDeleted, asNoTracking: false);
            if (entity == null)
            {
                var message = _localizer?["FormTab_NotFound"] ?? "Tab not found";
                return ServiceResult<bool>.NotFound(message);
            }

            // Always use soft delete for tabs using IsDeleted flag
            entity.IsDeleted = true;
            entity.DeletedDate = DateTime.UtcNow;
            entity.UpdatedDate = DateTime.UtcNow;
            Repository.Update(entity);
            await _unitOfWork.CompleteAsyn();
            
            return ServiceResult<bool>.Ok(true);
        }

        public async Task<ServiceResult<bool>> ToggleActiveAsync(int id, bool isActive)
        {
            var entity = await Repository.SingleOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
            if (entity == null) return ServiceResult<bool>.NotFound();

            entity.IsActive = isActive;
            entity.UpdatedDate = DateTime.UtcNow;
            Repository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            return ServiceResult<bool>.Ok(true);
        }

        public async Task<ServiceResult<FormTabDto>> RestoreAsync(int id)
        {
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id && e.IsDeleted, asNoTracking: false);
            if (entity == null)
            {
                var message = _localizer?["FormTab_NotFound"] ?? "Deleted tab not found";
                return ServiceResult<FormTabDto>.NotFound(message);
            }

            entity.IsDeleted = false;
            entity.DeletedDate = null;
            entity.DeletedByUserId = null;
            entity.UpdatedDate = DateTime.UtcNow;
            Repository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            return ServiceResult<FormTabDto>.Ok(_mapper.Map<FormTabDto>(entity));
        }

        public async Task<ServiceResult<bool>> ExistsAsync(int id)
        {
            var exists = await Repository.AnyAsync(t => t.Id == id);
            return ServiceResult<bool>.Ok(exists);
        }

        public async Task<ServiceResult<bool>> CodeExistsAsync(int formBuilderId, string tabCode, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(tabCode))
            {
                var message = _localizer?["FormTab_TabCodeRequired"] ?? "Tab code is required";
                return ServiceResult<bool>.BadRequest(message);
            }

            var isUnique = await _unitOfWork.FormTabRepository.IsTabCodeUniqueAsync(formBuilderId, tabCode.Trim(), excludeId);
            // IsTabCodeUniqueAsync returns true if unique (doesn't exist), so exists = !isUnique
            return ServiceResult<bool>.Ok(!isUnique);
        }

        protected override async Task<ValidationResult> ValidateCreateAsync(CreateFormTabDto dto)
        {
            if (dto == null) return ValidationResult.Failure("Payload is required");

            // Validate FormBuilder exists
            var formBuilderExists = await _unitOfWork.FormBuilderRepository.AnyAsync(f => f.Id == dto.FormBuilderId);
            if (!formBuilderExists)
                return ValidationResult.Failure($"FormBuilder with ID '{dto.FormBuilderId}' does not exist.");

            // Validate TabCode uniqueness
            var isUnique = await _unitOfWork.FormTabRepository.IsTabCodeUniqueAsync(dto.FormBuilderId, dto.TabCode);
            if (!isUnique)
            {
                // Log duplicate warning
                var existingTab = await Repository.SingleOrDefaultAsync(t => t.TabCode == dto.TabCode && !t.IsDeleted);
                if (existingTab != null)
                {
                    DuplicateValidationHelper.LogDuplicateDetection(
                        _logger,
                        "FormTab",
                        "TabCode",
                        dto.TabCode,
                        existingTab.Id,
                        null,
                        existingTab.IsDeleted
                    );
                }
                else
                {
                    DuplicateValidationHelper.LogDuplicateWarning(_logger, "FormTab", "TabCode", dto.TabCode);
                }

                var message = DuplicateValidationHelper.FormatDuplicateErrorMessage("Tab", "code", dto.TabCode);
                return ValidationResult.Failure(message);
            }

            return ValidationResult.Success();
        }

        protected override async Task<ValidationResult> ValidateUpdateAsync(int id, UpdateFormTabDto dto, FORM_TABS entity)
        {
            if (dto == null) return ValidationResult.Failure("Payload is required");

            // Validate TabCode uniqueness (excluding current tab)
            if (!string.IsNullOrWhiteSpace(dto.TabCode) && !string.Equals(dto.TabCode, entity.TabCode, StringComparison.OrdinalIgnoreCase))
            {
                var isUnique = await _unitOfWork.FormTabRepository.IsTabCodeUniqueAsync(entity.FormBuilderId, dto.TabCode, id);
                if (!isUnique)
                {
                    // Log duplicate warning
                    var conflictingTab = await Repository.SingleOrDefaultAsync(t => t.TabCode == dto.TabCode && t.Id != id && !t.IsDeleted);
                    if (conflictingTab != null)
                    {
                        DuplicateValidationHelper.LogDuplicateDetection(
                            _logger,
                            "FormTab",
                            "TabCode",
                            dto.TabCode,
                            conflictingTab.Id,
                            null,
                            conflictingTab.IsDeleted
                        );
                    }

                    var message = DuplicateValidationHelper.FormatDuplicateErrorMessage("Tab", "code", dto.TabCode);
                    return ValidationResult.Failure(message);
                }
            }

            return ValidationResult.Success();
        }
    }
}
