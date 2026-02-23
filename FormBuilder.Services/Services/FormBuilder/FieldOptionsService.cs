using AutoMapper;
using formBuilder.Domian.Interfaces;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using FormBuilder.Core.IServices.FormBuilder;
using FormBuilder.Domain.Interfaces;
using FormBuilder.Domian.Entitys.froms;
using FormBuilder.API.Models;
using FormBuilder.Services.Services.Base;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CreateFieldOptionDto = FormBuilder.API.Models.CreateFieldOptionDto;
using UpdateFieldOptionDto = FormBuilder.API.Models.UpdateFieldOptionDto;

namespace FormBuilder.Services.Services
{
    public class FieldOptionsService : BaseService<FIELD_OPTIONS, FieldOptionDto, CreateFieldOptionDto, UpdateFieldOptionDto>, IFieldOptionsService
    {
        private readonly IStringLocalizer<FieldOptionsService>? _localizer;

        public FieldOptionsService(IunitOfwork unitOfWork, IMapper mapper, IStringLocalizer<FieldOptionsService>? localizer = null)
            : base(unitOfWork, mapper, null)
        {
            _localizer = localizer;
        }

        protected override IBaseRepository<FIELD_OPTIONS> Repository => _unitOfWork.FieldOptionsRepository;

        /// <summary>
        /// Checks if field has Api or LookupTable DataSource (options should not be saved in database)
        /// </summary>
        private async Task<bool> HasExternalDataSourceAsync(int fieldId)
        {
            try
            {
                var dataSources = await _unitOfWork.FieldDataSourcesRepository.GetActiveByFieldIdAsync(fieldId);
                foreach (var dataSource in dataSources)
                {
                    if (string.Equals(dataSource.SourceType, "Api", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(dataSource.SourceType, "LookupTable", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                // If we can't check, assume it's safe to proceed
            }

            return false;
        }

        // ================================
        // CUSTOM OPERATIONS
        // ================================

        public async Task<ServiceResult<IEnumerable<FieldOptionDto>>> GetByFieldIdAsync(int fieldId)
        {
            // If field has Api or LookupTable DataSource, return empty list (options are not stored in database)
            if (await HasExternalDataSourceAsync(fieldId))
            {
                return ServiceResult<IEnumerable<FieldOptionDto>>.Ok(new List<FieldOptionDto>());
            }

            var options = await _unitOfWork.FieldOptionsRepository.GetByFieldIdAsync(fieldId);
            var dtos = _mapper.Map<IEnumerable<FieldOptionDto>>(options);
            return ServiceResult<IEnumerable<FieldOptionDto>>.Ok(dtos);
        }

        public async Task<ServiceResult<IEnumerable<FieldOptionDto>>> GetActiveByFieldIdAsync(int fieldId)
        {
            // If field has Api or LookupTable DataSource, return empty list (options are not stored in database)
            if (await HasExternalDataSourceAsync(fieldId))
            {
                return ServiceResult<IEnumerable<FieldOptionDto>>.Ok(new List<FieldOptionDto>());
            }

            var options = await _unitOfWork.FieldOptionsRepository.GetActiveByFieldIdAsync(fieldId);
            var dtos = _mapper.Map<IEnumerable<FieldOptionDto>>(options);
            return ServiceResult<IEnumerable<FieldOptionDto>>.Ok(dtos);
        }

        public async Task<ServiceResult<IEnumerable<FieldOptionDto>>> CreateBulkAsync(List<CreateFieldOptionDto> createDtos)
        {
            if (createDtos == null || !createDtos.Any())
            {
                var message = _localizer?["FieldOptions_NoOptionsProvided"] ?? "No field options provided";
                return ServiceResult<IEnumerable<FieldOptionDto>>.BadRequest(message);
            }

            // Validate all field IDs exist
            var fieldIds = createDtos.Select(d => d.FieldId).Distinct().ToList();
            foreach (var fieldId in fieldIds)
            {
                var fieldExists = await _unitOfWork.FormFieldRepository.AnyAsync(f => f.Id == fieldId);
                if (!fieldExists)
                {
                    var message = _localizer?["FieldOptions_InvalidFieldId", fieldId] ?? $"Invalid field ID: {fieldId}";
                    return ServiceResult<IEnumerable<FieldOptionDto>>.BadRequest(message);
                }

                // Check if field has Api or LookupTable DataSource - options should not be saved for these
                if (await HasExternalDataSourceAsync(fieldId))
                {
                    var message = _localizer?["FieldOptions_CannotSaveForExternalDataSource"] ?? 
                        "Cannot save options for Api/LookupTable DataSource. Options are loaded from external source.";
                    return ServiceResult<IEnumerable<FieldOptionDto>>.BadRequest(message);
                }
            }

            var entities = _mapper.Map<List<FIELD_OPTIONS>>(createDtos);
            foreach (var entity in entities)
            {
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsActive = true;
            }

            Repository.AddRange(entities);
            await _unitOfWork.CompleteAsyn();

            var dtos = _mapper.Map<IEnumerable<FieldOptionDto>>(entities);
            return ServiceResult<IEnumerable<FieldOptionDto>>.Ok(dtos);
        }

        public override async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id, asNoTracking: false);
            if (entity == null)
            {
                var message = _localizer?["Common_ResourceNotFound"] ?? "Resource not found";
                return ServiceResult<bool>.NotFound(message);
            }

            // Check if field has Api or LookupTable DataSource - options should not be deleted for these
            if (await HasExternalDataSourceAsync(entity.FieldId))
            {
                var message = _localizer?["FieldOptions_CannotDeleteForExternalDataSource"] ?? 
                    "Cannot delete options for Api/LookupTable DataSource. Options are loaded from external source.";
                return ServiceResult<bool>.BadRequest(message);
            }

            // Soft Delete
            entity.IsDeleted = true;
            entity.DeletedDate = DateTime.UtcNow;
            entity.IsActive = false;
            Repository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            return ServiceResult<bool>.Ok(true);
        }

        public override async Task<ServiceResult<bool>> SoftDeleteAsync(int id, string? deletedByUserId = null)
        {
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id && !e.IsDeleted, asNoTracking: false);
            if (entity == null)
            {
                var message = _localizer?["Common_ResourceNotFound"] ?? "Resource not found";
                return ServiceResult<bool>.NotFound(message);
            }

            // Check if field has Api or LookupTable DataSource - options should not be deleted for these
            if (await HasExternalDataSourceAsync(entity.FieldId))
            {
                var message = _localizer?["FieldOptions_CannotDeleteForExternalDataSource"] ?? 
                    "Cannot delete options for Api/LookupTable DataSource. Options are loaded from external source.";
                return ServiceResult<bool>.BadRequest(message);
            }

            // For Field Options, use IsActive = false (as per business logic - they use hard delete normally)
            // But still mark as deleted using IsDeleted flag for consistency
            entity.IsDeleted = true;
            entity.IsActive = false; // Also deactivate
            entity.DeletedDate = DateTime.UtcNow;
            entity.DeletedByUserId = deletedByUserId;
            entity.UpdatedDate = DateTime.UtcNow;
            Repository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            return ServiceResult<bool>.Ok(true);
        }

        public async Task<ServiceResult<FieldOptionDto>> GetDefaultOptionAsync(int fieldId)
        {
            var defaultOption = await _unitOfWork.FieldOptionsRepository.GetDefaultOptionAsync(fieldId);
            if (defaultOption == null)
            {
                var message = _localizer?["FieldOptions_NoDefaultFound"] ?? "No default option found for this field";
                return ServiceResult<FieldOptionDto>.NotFound(message);
            }

            var dto = _mapper.Map<FieldOptionDto>(defaultOption);
            return ServiceResult<FieldOptionDto>.Ok(dto);
        }

        public async Task<ServiceResult<int>> GetOptionsCountAsync(int fieldId)
        {
            var count = await _unitOfWork.FieldOptionsRepository.GetOptionsCountAsync(fieldId);
            return ServiceResult<int>.Ok(count);
        }

        public async Task<bool> FieldHasOptionsAsync(int fieldId)
        {
            return await _unitOfWork.FieldOptionsRepository.FieldHasOptionsAsync(fieldId);
        }

        // ================================
        // VALIDATION OVERRIDES
        // ================================

        protected override async Task<ValidationResult> ValidateCreateAsync(CreateFieldOptionDto dto)
        {
            // Validate if field exists
            var fieldExists = await _unitOfWork.FormFieldRepository.AnyAsync(f => f.Id == dto.FieldId);
            if (!fieldExists)
            {
                var message = _localizer?["FieldOptions_InvalidFieldId", dto.FieldId] ?? $"Invalid field ID: {dto.FieldId}";
                return ValidationResult.Failure(message);
            }

            // Check if field has Api or LookupTable DataSource - options should not be saved for these
            if (await HasExternalDataSourceAsync(dto.FieldId))
            {
                var message = _localizer?["FieldOptions_CannotSaveForExternalDataSource"] ?? 
                    "Cannot save options for Api/LookupTable DataSource. Options are loaded from external source.";
                return ValidationResult.Failure(message);
            }

            return ValidationResult.Success();
        }

        protected override async Task<ValidationResult> ValidateUpdateAsync(int id, UpdateFieldOptionDto dto, FIELD_OPTIONS entity)
        {
            // Check if field has Api or LookupTable DataSource - options should not be updated for these
            if (await HasExternalDataSourceAsync(entity.FieldId))
            {
                var message = _localizer?["FieldOptions_CannotUpdateForExternalDataSource"] ?? 
                    "Cannot update options for Api/LookupTable DataSource. Options are loaded from external source.";
                return ValidationResult.Failure(message);
            }

            return ValidationResult.Success();
        }
    }
}