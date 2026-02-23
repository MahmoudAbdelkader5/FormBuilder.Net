using AutoMapper;
using FormBuilder.API.Models.DTOs;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Domian.Entitys.FromBuilder;
using formBuilder.Domian.Interfaces;
using FormBuilder.Services.Services.Base;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FormBuilder.Services
{
    public class ProjectService
        : BaseService<PROJECTS, ProjectDto, CreateProjectDto, UpdateProjectDto>,
          IProjectService
    {
        private readonly IStringLocalizer<ProjectService>? _localizer;

        public ProjectService(IunitOfwork unitOfWork, IMapper mapper, IStringLocalizer<ProjectService>? localizer = null)
            : base(unitOfWork, mapper, null)
        {
            _localizer = localizer;
        }

        protected override IBaseRepository<PROJECTS> Repository => _unitOfWork.ProjectRepository;

        public async Task<ServiceResult<IEnumerable<ProjectDto>>> GetAllAsync(Expression<Func<PROJECTS, bool>>? filter = null)
            => await base.GetAllAsync(filter);

        public async Task<ServiceResult<PagedResult<ProjectDto>>> GetPagedAsync(int page = 1, int pageSize = 20, Expression<Func<PROJECTS, bool>>? filter = null)
            => await base.GetPagedAsync(page, pageSize, filter);

        public async Task<ServiceResult<ProjectDto>> GetByIdAsync(int id, bool asNoTracking = false)
            => await base.GetByIdAsync(id, asNoTracking);

        public async Task<ServiceResult<ProjectDto>> GetByCodeAsync(string code, bool asNoTracking = false)
        {
            if (string.IsNullOrWhiteSpace(code))
                return ServiceResult<ProjectDto>.BadRequest("Project code is required");

            var entity = await Repository.SingleOrDefaultAsync(p => p.Code == code.Trim(), asNoTracking);
            if (entity == null) return ServiceResult<ProjectDto>.NotFound();

            return ServiceResult<ProjectDto>.Ok(_mapper.Map<ProjectDto>(entity));
        }

        public async Task<ServiceResult<IEnumerable<ProjectDto>>> GetActiveAsync()
        {
            return await base.GetAllAsync(p => p.IsActive);
        }

        public override async Task<ServiceResult<ProjectDto>> CreateAsync(CreateProjectDto createDto)
        {
            return await base.CreateAsync(createDto);
        }

        public override async Task<ServiceResult<ProjectDto>> UpdateAsync(int id, UpdateProjectDto updateDto)
        {
            return await base.UpdateAsync(id, updateDto);
        }

        public override async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id, asNoTracking: false);
            if (entity == null)
            {
                var message = _localizer?["Project_NotFound"] ?? "Project not found";
                return ServiceResult<bool>.NotFound(message);
            }

            // Check if there are document series using this project
            var documentSeriesCount = await _unitOfWork.DocumentSeriesRepository.CountAsync(ds => ds.ProjectId == id);

            // If there are related data, use soft delete instead of hard delete
            if (documentSeriesCount > 0)
            {
                entity.IsActive = false;
                entity.UpdatedDate = DateTime.UtcNow;
                Repository.Update(entity);
                await _unitOfWork.CompleteAsyn();
                
                return ServiceResult<bool>.Ok(true);
            }

            try
            {
                // Soft Delete
                entity.IsDeleted = true;
                entity.DeletedDate = DateTime.UtcNow;
                entity.IsActive = false;
                Repository.Update(entity);
                await _unitOfWork.CompleteAsyn();
                return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                // Check if it's a foreign key constraint violation
                if (ex.Message.Contains("REFERENCE constraint") || 
                    ex.Message.Contains("FK_DOCUMENT_SERIES") ||
                    ex.InnerException?.Message?.Contains("REFERENCE constraint") == true)
                {
                    // Try soft delete as fallback
                    try
                    {
                        entity.IsDeleted = true;
                        entity.DeletedDate = DateTime.UtcNow;
                        entity.IsActive = false;
                        Repository.Update(entity);
                        await _unitOfWork.CompleteAsyn();
                        
                        return ServiceResult<bool>.Ok(true);
                    }
                    catch
                    {
                        var finalSeriesCount = await _unitOfWork.DocumentSeriesRepository.CountAsync(ds => ds.ProjectId == id);
                        var errorMessage = _localizer?["Project_CannotDelete"] 
                            ?? $"Cannot delete project: There are {finalSeriesCount} document series associated with it. The project has been deactivated instead.";
                        return ServiceResult<bool>.BadRequest(errorMessage);
                    }
                }
                
                var message = _localizer?["Project_DeleteError"] ?? $"Error deleting project: {ex.Message}";
                return ServiceResult<bool>.Error(message);
            }
        }

        public async Task<ServiceResult<bool>> ToggleActiveAsync(int id, bool isActive)
        {
            var entity = await Repository.SingleOrDefaultAsync(p => p.Id == id);
            if (entity == null) return ServiceResult<bool>.NotFound();

            entity.IsActive = isActive;
            entity.UpdatedDate = DateTime.UtcNow;
            Repository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            return ServiceResult<bool>.Ok(true);
        }

        public async Task<ServiceResult<bool>> ExistsAsync(int id)
        {
            var exists = await Repository.AnyAsync(p => p.Id == id);
            return ServiceResult<bool>.Ok(exists);
        }

        public async Task<ServiceResult<bool>> CodeExistsAsync(string code, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                var message = _localizer?["Project_CodeRequired"] ?? "Project code is required";
                return ServiceResult<bool>.BadRequest(message);
            }

            var exists = await _unitOfWork.ProjectRepository.CodeExistsAsync(code.Trim(), excludeId);
            return ServiceResult<bool>.Ok(exists);
        }

        protected override async Task<ValidationResult> ValidateCreateAsync(CreateProjectDto dto)
        {
            if (dto == null)
            {
                var message = _localizer?["Common_PayloadRequired"] ?? "Payload is required";
                return ValidationResult.Failure(message);
            }

            var exists = await _unitOfWork.ProjectRepository.CodeExistsAsync(dto.Code);
            if (exists)
            {
                var message = _localizer?["Project_CodeExists", dto.Code] ?? $"Project code '{dto.Code}' already exists.";
                return ValidationResult.Failure(message);
            }

            return ValidationResult.Success();
        }

        protected override async Task<ValidationResult> ValidateUpdateAsync(int id, UpdateProjectDto dto, PROJECTS entity)
        {
            if (dto == null)
            {
                var message = _localizer?["Common_PayloadRequired"] ?? "Payload is required";
                return ValidationResult.Failure(message);
            }

            if (!string.IsNullOrWhiteSpace(dto.Code) && !string.Equals(dto.Code, entity.Code, StringComparison.OrdinalIgnoreCase))
            {
                var exists = await _unitOfWork.ProjectRepository.CodeExistsAsync(dto.Code, id);
                if (exists)
                {
                    var message = _localizer?["Project_CodeExists", dto.Code] ?? $"Project code '{dto.Code}' already exists.";
                    return ValidationResult.Failure(message);
                }
            }

            return ValidationResult.Success();
        }
    }
}