using AutoMapper;
using FormBuilder.API.DTOs;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Domian.Entitys.FormBuilder;
using formBuilder.Domian.Interfaces;
using FormBuilder.Services.Services.Base;
using FormBuilder.Services.Services.Common;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FormBuilder.Services
{
    public class FieldTypesService
        : BaseService<FIELD_TYPES, FieldTypeDto, CreateFieldTypeDto, UpdateFieldTypeDto>,
          IFieldTypesService
    {
        private readonly IStringLocalizer<FieldTypesService>? _localizer;
        private readonly ValidationMessageService _validationMessageService;

        public FieldTypesService(
            IunitOfwork unitOfWork, 
            IMapper mapper, 
            ValidationMessageService validationMessageService,
            IStringLocalizer<FieldTypesService>? localizer = null)
            : base(unitOfWork, mapper, null)
        {
            _localizer = localizer;
            _validationMessageService = validationMessageService ?? throw new ArgumentNullException(nameof(validationMessageService));
        }

        protected override IBaseRepository<FIELD_TYPES> Repository => _unitOfWork.FieldTypesRepository;

        public async Task<ServiceResult<IEnumerable<FieldTypeDto>>> GetAllAsync(Expression<Func<FIELD_TYPES, bool>>? filter = null)
        {
            var list = await _unitOfWork.FieldTypesRepository.GetAllAsync(filter);
            return ServiceResult<IEnumerable<FieldTypeDto>>.Ok(_mapper.Map<IEnumerable<FieldTypeDto>>(list));
        }

        public async Task<ServiceResult<FieldTypeDto>> GetByIdAsync(int id, bool asNoTracking = false)
        {
            var entity = await _unitOfWork.FieldTypesRepository.GetByIdAsync(id);
            if (entity == null)
            {
                var message = _validationMessageService.GetMessage(ValidationErrorCodes.FieldTypeNotFound);
                return ServiceResult<FieldTypeDto>.NotFound(message);
            }
            return ServiceResult<FieldTypeDto>.Ok(_mapper.Map<FieldTypeDto>(entity));
        }

        public async Task<ServiceResult<FieldTypeDto>> GetByTypeNameAsync(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                var message = _validationMessageService.GetMessage(ValidationErrorCodes.FieldTypeNameRequired);
                return ServiceResult<FieldTypeDto>.BadRequest(message);
            }

            var entity = await _unitOfWork.FieldTypesRepository.GetByTypeNameAsync(typeName.Trim());
            if (entity == null)
            {
                var message = _validationMessageService.GetMessage(ValidationErrorCodes.FieldTypeNotFound);
                return ServiceResult<FieldTypeDto>.NotFound(message);
            }

            return ServiceResult<FieldTypeDto>.Ok(_mapper.Map<FieldTypeDto>(entity));
        }

        public async Task<ServiceResult<IEnumerable<FieldTypeDto>>> GetActiveAsync()
        {
            var list = await _unitOfWork.FieldTypesRepository.GetActiveAsync();
            return ServiceResult<IEnumerable<FieldTypeDto>>.Ok(_mapper.Map<IEnumerable<FieldTypeDto>>(list));
        }

        // ================================
        // VALIDATION OVERRIDES
        // ================================

        protected override async Task<ValidationResult> ValidateCreateAsync(CreateFieldTypeDto dto)
        {
            var errors = new ValidationErrorCollection();

            // Validate TypeName is required
            if (string.IsNullOrWhiteSpace(dto.TypeName))
            {
                errors.AddError(
                    nameof(dto.TypeName),
                    ValidationErrorCodes.FieldTypeNameRequired,
                    _validationMessageService.GetMessage(ValidationErrorCodes.FieldTypeNameRequired),
                    "Required"
                );
            }
            else
            {
                // Validate TypeName length
                if (dto.TypeName.Length > 100)
                {
                    errors.AddError(
                        nameof(dto.TypeName),
                        ValidationErrorCodes.InvalidLength,
                        _validationMessageService.GetMessage(ValidationErrorCodes.InvalidLength, nameof(dto.TypeName), 0, 100),
                        "Length"
                    );
                }

                // Validate TypeName is unique
                if (await _unitOfWork.FieldTypesRepository.TypeNameExistsAsync(dto.TypeName.Trim()))
                {
                    errors.AddError(
                        nameof(dto.TypeName),
                        ValidationErrorCodes.FieldTypeNameExists,
                        _validationMessageService.GetMessage(ValidationErrorCodes.FieldTypeNameExists, dto.TypeName),
                        "Duplicate"
                    );
                }
            }

            // Validate DataType is required
            if (string.IsNullOrWhiteSpace(dto.DataType))
            {
                errors.AddError(
                    nameof(dto.DataType),
                    ValidationErrorCodes.FieldTypeDataTypeRequired,
                    _validationMessageService.GetMessage(ValidationErrorCodes.FieldTypeDataTypeRequired),
                    "Required"
                );
            }
            else
            {
                // Validate DataType length
                if (dto.DataType.Length > 50)
                {
                    errors.AddError(
                        nameof(dto.DataType),
                        ValidationErrorCodes.InvalidLength,
                        _validationMessageService.GetMessage(ValidationErrorCodes.InvalidLength, nameof(dto.DataType), 0, 50),
                        "Length"
                    );
                }

                // Validate DataType value (common data types)
                var validDataTypes = new[] { "string", "int", "decimal", "bool", "date", "datetime", "time", "text", "email", "url", "phone" };
                if (!validDataTypes.Contains(dto.DataType.ToLower()))
                {
                    errors.AddError(
                        nameof(dto.DataType),
                        ValidationErrorCodes.FieldTypeInvalidDataType,
                        _validationMessageService.GetMessage(ValidationErrorCodes.FieldTypeInvalidDataType, string.Join(", ", validDataTypes)),
                        "InvalidValue"
                    );
                }
            }

            // Validate MaxLength if provided
            if (dto.MaxLength.HasValue && dto.MaxLength.Value < 0)
            {
                errors.AddError(
                    nameof(dto.MaxLength),
                    ValidationErrorCodes.InvalidRange,
                    _validationMessageService.GetMessage(ValidationErrorCodes.InvalidRange, nameof(dto.MaxLength), 0, int.MaxValue),
                    "Range"
                );
            }

            if (errors.HasErrors)
            {
                // Combine all error messages into a single message for the simple ValidationResult
                var combinedMessage = errors.GeneralMessage 
                    ?? string.Join("; ", errors.Errors.Select(e => e.ErrorMessage));

                return ValidationResult.Failure(combinedMessage);
            }

            return ValidationResult.Success();
        }

        public override async Task<ServiceResult<FieldTypeDto>> CreateAsync(CreateFieldTypeDto createDto)
        {
            var validation = await ValidateCreateAsync(createDto);
            if (!validation.IsValid)
            {
                var errorMessage = validation.ErrorMessage ?? "Validation failed";
                return ServiceResult<FieldTypeDto>.BadRequest(errorMessage);
            }

            var result = await base.CreateAsync(createDto);
            return result;
        }

        protected override async Task<ValidationResult> ValidateUpdateAsync(int id, UpdateFieldTypeDto dto, FIELD_TYPES entity)
        {
            var errors = new ValidationErrorCollection();

            // Validate TypeName if provided and changed
            if (!string.IsNullOrWhiteSpace(dto.TypeName))
            {
                var dtoTypeName = dto.TypeName.Trim();
                var entityTypeName = entity.TypeName?.Trim() ?? string.Empty;

                if (dtoTypeName != entityTypeName)
                {
                    // Validate length
                    if (dtoTypeName.Length > 100)
                    {
                        errors.AddError(
                            nameof(dto.TypeName),
                            ValidationErrorCodes.InvalidLength,
                            _validationMessageService.GetMessage(ValidationErrorCodes.InvalidLength, nameof(dto.TypeName), 0, 100),
                            "Length"
                        );
                    }

                    // Validate uniqueness
                    if (await _unitOfWork.FieldTypesRepository.TypeNameExistsAsync(dtoTypeName, id))
                    {
                        errors.AddError(
                            nameof(dto.TypeName),
                            ValidationErrorCodes.FieldTypeNameExists,
                            _validationMessageService.GetMessage(ValidationErrorCodes.FieldTypeNameExists, dtoTypeName),
                            "Duplicate"
                        );
                    }
                }
            }

            // Validate DataType if provided
            if (!string.IsNullOrWhiteSpace(dto.DataType))
            {
                // Validate length
                if (dto.DataType.Length > 50)
                {
                    errors.AddError(
                        nameof(dto.DataType),
                        ValidationErrorCodes.InvalidLength,
                        _validationMessageService.GetMessage(ValidationErrorCodes.InvalidLength, nameof(dto.DataType), 0, 50),
                        "Length"
                    );
                }

                // Validate DataType value
                var validDataTypes = new[] { "string", "int", "decimal", "bool", "date", "datetime", "time", "text", "email", "url", "phone" };
                if (!validDataTypes.Contains(dto.DataType.ToLower()))
                {
                    errors.AddError(
                        nameof(dto.DataType),
                        ValidationErrorCodes.FieldTypeInvalidDataType,
                        _validationMessageService.GetMessage(ValidationErrorCodes.FieldTypeInvalidDataType, string.Join(", ", validDataTypes)),
                        "InvalidValue"
                    );
                }
            }

            // Validate MaxLength if provided
            if (dto.MaxLength.HasValue && dto.MaxLength.Value < 0)
            {
                errors.AddError(
                    nameof(dto.MaxLength),
                    ValidationErrorCodes.InvalidRange,
                    _validationMessageService.GetMessage(ValidationErrorCodes.InvalidRange, nameof(dto.MaxLength), 0, int.MaxValue),
                    "Range"
                );
            }

            if (errors.HasErrors)
            {
                var combinedMessage = errors.GeneralMessage 
                    ?? string.Join("; ", errors.Errors.Select(e => e.ErrorMessage));

                return ValidationResult.Failure(combinedMessage);
            }

            return ValidationResult.Success();
        }

        public override async Task<ServiceResult<FieldTypeDto>> UpdateAsync(int id, UpdateFieldTypeDto updateDto)
        {
            var entity = await _unitOfWork.FieldTypesRepository.GetByIdAsync(id);
            if (entity == null)
            {
                var message = _validationMessageService.GetMessage(ValidationErrorCodes.FieldTypeNotFound);
                return ServiceResult<FieldTypeDto>.NotFound(message);
            }

            var validation = await ValidateUpdateAsync(id, updateDto, entity);
            if (!validation.IsValid)
            {
                var errorMessage = validation.ErrorMessage ?? "Validation failed";
                return ServiceResult<FieldTypeDto>.BadRequest(errorMessage);
            }

            var result = await base.UpdateAsync(id, updateDto);
            return result;
        }

        public override async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.FieldTypesRepository.GetByIdAsync(id);
            if (entity == null)
            {
                var message = _validationMessageService.GetMessage(ValidationErrorCodes.FieldTypeNotFound);
                return ServiceResult<bool>.NotFound(message);
            }

            // Check if field type is being used
            var usageCount = await _unitOfWork.FieldTypesRepository.GetUsageCountAsync(id);
            if (usageCount > 0)
            {
                var message = _validationMessageService.GetMessage(ValidationErrorCodes.FieldTypeInUse, usageCount.ToString());
                return ServiceResult<bool>.BadRequest(message);
            }

            return await base.DeleteAsync(id);
        }

        public async Task<ServiceResult<bool>> ToggleActiveAsync(int id, bool isActive)
        {
            var result = await base.ToggleActiveAsync(id, isActive);
            if (result.Success)
            {
                return ServiceResult<bool>.Ok(true);
            }
            return ServiceResult<bool>.Error(result.ErrorMessage ?? "Error toggling active status", result.StatusCode);
        }

        public async Task<ServiceResult<bool>> ExistsAsync(int id)
        {
            var exists = await Repository.AnyAsync(e => e.Id == id && !e.IsDeleted);
            return ServiceResult<bool>.Ok(exists);
        }

        public async Task<ServiceResult<bool>> TypeNameExistsAsync(string typeName, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                return ServiceResult<bool>.Ok(false);
            }

            var exists = await _unitOfWork.FieldTypesRepository.TypeNameExistsAsync(typeName.Trim(), excludeId);
            return ServiceResult<bool>.Ok(exists);
        }

        public async Task<ServiceResult<int>> GetUsageCountAsync(int fieldTypeId)
        {
            try
            {
                var count = await _unitOfWork.FieldTypesRepository.GetUsageCountAsync(fieldTypeId);
                return ServiceResult<int>.Ok(count);
            }
            catch (Exception ex)
            {
                var message = _localizer?["FieldTypes_UsageCheckFailed"] ?? $"Error checking field type usage: {ex.Message}";
                return ServiceResult<int>.Error(message);
            }
        }
    }
}

