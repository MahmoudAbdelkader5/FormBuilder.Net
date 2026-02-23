using AutoMapper;
using formBuilder.Domian.Interfaces;
using FormBuilder.Application.DTOS;
using FormBuilder.Application.DTOs.Formula;
using FormBuilder.Core.DTOS.Common;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Core.DTOS.FormFields;
using FormBuilder.Core.IServices.FormBuilder;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.API.Models;
using FormBuilder.Services.Services.Base;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Services.Helpers;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CreateFormFieldDto = FormBuilder.Core.DTOS.FormFields.CreateFormFieldDto;
using UpdateFormFieldDto = FormBuilder.API.Models.UpdateFormFieldDto;
using FormBuilder.Domian.Entitys.froms;

namespace FormBuilder.Services.Services
{
    public class FormFieldService : BaseService<FORM_FIELDS, FormFieldDto, CreateFormFieldDto, UpdateFormFieldDto>, IFormFieldService
    {
        private readonly IStringLocalizer<FormFieldService>? _localizer;
        private readonly IFormulaService _formulaService;
        private readonly ILogger<FormFieldService>? _logger;

        public FormFieldService(IunitOfwork unitOfWork, IMapper mapper, IFormulaService formulaService, IStringLocalizer<FormFieldService>? localizer = null, ILogger<FormFieldService>? logger = null)
            : base(unitOfWork, mapper, null)
        {
            _localizer = localizer;
            _formulaService = formulaService;
            _logger = logger;
        }

        protected override IBaseRepository<FORM_FIELDS> Repository => _unitOfWork.FormFieldRepository;

        // ================================
        // CUSTOM OPERATIONS
        // ================================

        public async Task<ServiceResult<IEnumerable<FormFieldDto>>> GetActiveAsync()
        {
            var entities = await Repository.GetAllAsync(e => e.IsActive);
            var dtos = _mapper.Map<IEnumerable<FormFieldDto>>(entities);
            return ServiceResult<IEnumerable<FormFieldDto>>.Ok(dtos);
        }

        public async Task<ServiceResult<IEnumerable<FormFieldDto>>> GetByTabIdAsync(int tabId)
        {
            var entities = await _unitOfWork.FormFieldRepository.GetFieldsByTabIdAsync(tabId);
            var dtos = _mapper.Map<IEnumerable<FormFieldDto>>(entities);
            return ServiceResult<IEnumerable<FormFieldDto>>.Ok(dtos);
        }

        public async Task<ServiceResult<IEnumerable<FormFieldDto>>> GetByFormIdAsync(int formBuilderId)
        {
            var entities = await _unitOfWork.FormFieldRepository.GetFieldsByFormIdAsync(formBuilderId);
            var dtos = _mapper.Map<IEnumerable<FormFieldDto>>(entities);
            return ServiceResult<IEnumerable<FormFieldDto>>.Ok(dtos);
        }

        public async Task<ServiceResult<IEnumerable<FormFieldDto>>> GetMandatoryFieldsAsync(int tabId)
        {
            var entities = await _unitOfWork.FormFieldRepository.GetMandatoryFieldsAsync(tabId);
            var dtos = _mapper.Map<IEnumerable<FormFieldDto>>(entities);
            return ServiceResult<IEnumerable<FormFieldDto>>.Ok(dtos);
        }

        public async Task<ServiceResult<IEnumerable<FormFieldDto>>> GetVisibleFieldsAsync(int tabId)
        {
            var entities = await _unitOfWork.FormFieldRepository.GetVisibleFieldsAsync(tabId);
            var dtos = _mapper.Map<IEnumerable<FormFieldDto>>(entities);
            return ServiceResult<IEnumerable<FormFieldDto>>.Ok(dtos);
        }

        public async Task<ServiceResult<FormFieldDto>> GetByFieldCodeAsync(string fieldCode)
        {
            if (string.IsNullOrWhiteSpace(fieldCode))
            {
                var message = _localizer?["FormField_FieldCodeRequired"] ?? "Field code is required";
                return ServiceResult<FormFieldDto>.BadRequest(message);
            }

            var entities = await Repository.GetAllAsync(e => e.FieldCode == fieldCode && e.IsActive);
            var entity = entities.FirstOrDefault();
            
            if (entity == null)
                return ServiceResult<FormFieldDto>.NotFound();

            var dto = _mapper.Map<FormFieldDto>(entity);
            return ServiceResult<FormFieldDto>.Ok(dto);
        }

        public async Task<ServiceResult<IEnumerable<FormFieldDto>>> GetEditableFieldsAsync(int tabId)
        {
            var entities = await _unitOfWork.FormFieldRepository.GetFieldsByTabIdAsync(tabId);
            var editableEntities = entities.Where(e => e.IsEditable ?? false);
            var dtos = _mapper.Map<IEnumerable<FormFieldDto>>(editableEntities);
            return ServiceResult<IEnumerable<FormFieldDto>>.Ok(dtos);
        }

        public async Task<ServiceResult<IEnumerable<FormFieldDto>>> GetFieldsByGridIdAsync(int gridId)
        {
            var entities = await _unitOfWork.FormFieldRepository.GetFieldsByGridIdAsync(gridId);
            var dtos = _mapper.Map<IEnumerable<FormFieldDto>>(entities);
            return ServiceResult<IEnumerable<FormFieldDto>>.Ok(dtos);
        }

        public async Task<ServiceResult<bool>> SoftDeleteAsync(int id)
        {
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
            if (entity == null)
                return ServiceResult<bool>.NotFound();

            // Proper soft delete - set IsDeleted flag to allow code reuse
            entity.IsDeleted = true;
            entity.DeletedDate = DateTime.UtcNow;
            entity.IsActive = false;
            entity.UpdatedDate = DateTime.UtcNow;
            Repository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            return ServiceResult<bool>.Ok(true);
        }

        // ================================
        // VALIDATION
        // ================================

        public async Task<bool> IsFieldCodeUniqueAsync(string fieldCode, int? ignoreId = null)
        {
            return await _unitOfWork.FormFieldRepository.IsFieldCodeUniqueAsync(fieldCode, ignoreId);
        }

        public async Task<bool> IsFieldNameUniqueAsync(string fieldName, int? ignoreId = null, int? tabId = null)
        {
            return await _unitOfWork.FormFieldRepository.IsFieldNameUniqueAsync(fieldName, ignoreId, tabId);
        }

        // ================================
        // UTILITY
        // ================================

        public async Task<bool> ExistsAsync(int id)
        {
            return await Repository.AnyAsync(e => e.Id == id);
        }

        public async Task<ServiceResult<int>> GetUsageCountAsync(int fieldId)
        {
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == fieldId);
            if (entity == null)
                return ServiceResult<int>.NotFound();

            // Add logic to check usage in related entities
            // For example: FORM_SUBMISSION_VALUES, FORMULA_VARIABLES, etc.
            int count = 0;

            // Example: count form submission values
            // count += entity.FORM_SUBMISSION_VALUES?.Count(x => x.IsActive) ?? 0;

            return ServiceResult<int>.Ok(count);
        }

        public async Task<ServiceResult<int>> GetFieldsCountByTabAsync(int tabId)
        {
            var fields = await _unitOfWork.FormFieldRepository.GetFieldsByTabIdAsync(tabId);
            return ServiceResult<int>.Ok(fields.Count());
        }

        public async Task<ServiceResult<int>> GetFieldsCountByFormAsync(int formBuilderId)
        {
            var fields = await _unitOfWork.FormFieldRepository.GetFieldsByFormIdAsync(formBuilderId);
            return ServiceResult<int>.Ok(fields.Count());
        }

        // ================================
        // CREATE OVERRIDE - Support optional FieldOptions
        // ================================

        public override async Task<ServiceResult<FormFieldDto>> CreateAsync(CreateFormFieldDto dto)
        {
            if (dto == null)
            {
                var message = _localizer?["Common_PayloadRequired"] ?? "Payload is required";
                return ServiceResult<FormFieldDto>.BadRequest(message);
            }

            var validation = await ValidateCreateAsync(dto);
            if (!validation.IsValid)
            {
                var message = validation.ErrorMessage ?? (_localizer?["Common_ValidationFailed"] ?? "Validation failed");
                return ServiceResult<FormFieldDto>.BadRequest(message);
            }

            var entity = _mapper.Map<FORM_FIELDS>(dto);
            entity.CreatedDate = entity.CreatedDate == default ? DateTime.UtcNow : entity.CreatedDate;
            entity.IsActive = true;
            
            // Ensure HintText is not null (database constraint)
            if (string.IsNullOrEmpty(entity.HintText))
            {
                entity.HintText = string.Empty;
            }

            // Handle calculation fields - check if ExpressionText is provided
            bool isCalculatedField = !string.IsNullOrWhiteSpace(dto.ExpressionText);

            if (isCalculatedField)
            {
                // Enforce calculation field rules
                entity.IsEditable = false; // Calculated fields are never editable
                entity.IsMandatory = false; // Calculated fields are never mandatory
                entity.IsVisible = dto.IsVisible; // Visible can be controlled

                // Validate expression if provided
                if (!string.IsNullOrWhiteSpace(dto.ExpressionText))
                {
                    var tab = await _unitOfWork.FormTabRepository.SingleOrDefaultAsync(t => t.Id == dto.TabId);
                    if (tab != null)
                    {
                        var validationResult = await _formulaService.ValidateExpressionAsync(new ValidateExpressionDto
                        {
                            ExpressionText = dto.ExpressionText,
                            FormBuilderId = tab.FormBuilderId
                        });

                        if (!validationResult.Success)
                        {
                            var message = validationResult.ErrorMessage ?? (_localizer?["FormField_InvalidExpression"] ?? "Invalid expression");
                            return ServiceResult<FormFieldDto>.BadRequest(message);
                        }
                    }
                }
            }

            Repository.Add(entity);
            await _unitOfWork.CompleteAsyn();

            // Handle calculation fields after field is created
            if (isCalculatedField && !string.IsNullOrWhiteSpace(dto.ExpressionText))
            {
                var tab = await _unitOfWork.FormTabRepository.SingleOrDefaultAsync(t => t.Id == dto.TabId);
                if (tab != null)
                {
                    // Create FORMULA entry for this calculated field
                    var formula = new FORMULAS
                    {
                        FormBuilderId = tab.FormBuilderId,
                        Name = $"{dto.FieldName} Formula",
                        Code = $"{dto.FieldCode}_FORMULA",
                        ExpressionText = dto.ExpressionText,
                        ResultFieldId = entity.Id,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow
                    };

                    _unitOfWork.FormulasRepository.Add(formula);
                    await _unitOfWork.CompleteAsyn();

                    // Get dependent fields from validation result
                    var validationDetails = await _formulaService.ValidateExpressionWithDetailsAsync(new ValidateExpressionDto
                    {
                        ExpressionText = dto.ExpressionText,
                        FormBuilderId = tab.FormBuilderId
                    });

                    if (validationDetails.Success && validationDetails.Data?.FieldDetails != null)
                    {
                        // Create FORMULA_VARIABLES entries for dependent fields
                        foreach (var fieldDetail in validationDetails.Data.FieldDetails)
                        {
                            var variable = new FORMULA_VARIABLES
                            {
                                FormulaId = formula.Id,
                                VariableName = fieldDetail.FieldCode,
                                SourceFieldId = fieldDetail.FieldId,
                                IsActive = true,
                                CreatedDate = DateTime.UtcNow
                            };
                            _unitOfWork.FormulaVariablesRepository.Add(variable);
                        }
                        await _unitOfWork.CompleteAsyn();
                    }
                }
            }

            // Create field options if provided (optional)
            if (dto.FieldOptions != null && dto.FieldOptions.Any())
            {
                var fieldOptions = dto.FieldOptions.Select((option, index) => new FIELD_OPTIONS
                {
                    FieldId = entity.Id,
                    OptionText = option.OptionText,
                    OptionValue = option.OptionValue ?? option.OptionText,
                    OptionOrder = option.SortOrder != 0 ? option.SortOrder : index + 1,
                    IsDefault = option.IsDefault,
                    IsActive = option.IsActive,
                    CreatedDate = DateTime.UtcNow
                }).ToList();

                foreach (var option in fieldOptions)
                {
                    _unitOfWork.FieldOptionsRepository.Add(option);
                }

                await _unitOfWork.CompleteAsyn();
            }

            return ServiceResult<FormFieldDto>.Ok(_mapper.Map<FormFieldDto>(entity));
        }

        // ================================
        // UPDATE OVERRIDE
        // ================================
        public override async Task<ServiceResult<FormFieldDto>> UpdateAsync(int id, UpdateFormFieldDto dto)
        {
            if (dto == null)
            {
                var message = _localizer?["Common_PayloadRequired"] ?? "Payload is required";
                return ServiceResult<FormFieldDto>.BadRequest(message);
            }

            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id && !e.IsDeleted, asNoTracking: false);
            if (entity == null)
            {
                var message = _localizer?["Common_ResourceNotFound"] ?? "Resource not found or has been deleted";
                return ServiceResult<FormFieldDto>.NotFound(message);
            }

            var validation = await ValidateUpdateAsync(id, dto, entity);
            if (!validation.IsValid)
            {
                var message = validation.ErrorMessage ?? (_localizer?["Common_ValidationFailed"] ?? "Validation failed");
                return ServiceResult<FormFieldDto>.BadRequest(message);
            }

            _mapper.Map(dto, entity);
            entity.UpdatedDate = DateTime.UtcNow;

            // Ensure HintText is not null (database constraint)
            if (string.IsNullOrEmpty(entity.HintText))
            {
                entity.HintText = string.Empty;
            }

            Repository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            return ServiceResult<FormFieldDto>.Ok(_mapper.Map<FormFieldDto>(entity));
        }

        // ================================
        // VALIDATION OVERRIDES
        // ================================

        protected override async Task<ValidationResult> ValidateCreateAsync(CreateFormFieldDto dto)
        {
            // Validate field code uniqueness - only check non-deleted records
            // This allows reusing FieldCode from soft-deleted records
            var isUnique = await _unitOfWork.FormFieldRepository.IsFieldCodeUniqueAsync(dto.FieldCode);
            
            if (!isUnique)
            {
                // Double-check: Make sure we're only checking non-deleted records
                var existingField = await Repository.SingleOrDefaultAsync(
                    f => f.FieldCode == dto.FieldCode && !f.IsDeleted, 
                    asNoTracking: true);
                    
                if (existingField != null)
                {
                    // Found a non-deleted record with same FieldCode - this is a real duplicate
                    DuplicateValidationHelper.LogDuplicateDetection(
                        _logger,
                        "FormField",
                        "FieldCode",
                        dto.FieldCode,
                        existingField.Id,
                        null,
                        existingField.IsDeleted
                    );

                    var message = _localizer?["FormField_FieldCodeExists", dto.FieldCode] 
                        ?? DuplicateValidationHelper.FormatDuplicateErrorMessage("Field", "code", dto.FieldCode);
                    return ValidationResult.Failure(message);
                }
                else
                {
                    // IsFieldCodeUniqueAsync returned false but no non-deleted record found
                    // This shouldn't happen, but log a warning and allow creation
                    DuplicateValidationHelper.LogDuplicateWarning(_logger, "FormField", "FieldCode", dto.FieldCode);
                    // Allow creation to proceed (may be a race condition or soft-deleted record)
                }
            }

            // Field name can be duplicated - no uniqueness validation needed

            // Validate GridId if provided (for Grid field type)
            if (dto.GridId.HasValue)
            {
                // التحقق من وجود Grid
                var grid = await _unitOfWork.FormGridRepository.GetByIdAsync(dto.GridId.Value);
                if (grid == null)
                {
                    var message = _localizer?["FormField_GridNotFound"] ?? "Grid not found";
                    return ValidationResult.Failure(message);
                }
                
                // التحقق من أن Grid ينتمي لنفس Tab
                if (grid.TabId.HasValue && grid.TabId != dto.TabId)
                {
                    var message = _localizer?["FormField_GridNotInTab"] ?? "Grid does not belong to this tab";
                    return ValidationResult.Failure(message);
                }
            }

            // Validate calculation fields - check if ExpressionText is provided
            bool isCalculatedField = !string.IsNullOrWhiteSpace(dto.ExpressionText);

            if (isCalculatedField)
            {
                // Expression is required for calculated fields
                if (string.IsNullOrWhiteSpace(dto.ExpressionText))
                {
                    var message = _localizer?["FormField_ExpressionRequired"] ?? "Expression is required for calculated fields";
                    return ValidationResult.Failure(message);
                }

                // Check for circular reference - expression cannot reference itself
                var fieldCodePattern = @"\[([A-Za-z_][A-Za-z0-9_]*)\]";
                var matches = Regex.Matches(dto.ExpressionText, fieldCodePattern);
                var referencedFieldCodes = matches.Cast<Match>()
                    .Select(m => m.Groups[1].Value.ToUpper())
                    .Distinct()
                    .ToList();

                if (referencedFieldCodes.Contains(dto.FieldCode.ToUpper()))
                {
                    var message = _localizer?["FormField_CircularReference"] ?? $"Expression cannot reference the same field '{dto.FieldCode}'. This would create a circular reference.";
                    return ValidationResult.Failure(message);
                }

                // Validate expression syntax and field references
                var tab = await _unitOfWork.FormTabRepository.SingleOrDefaultAsync(t => t.Id == dto.TabId);
                if (tab != null)
                {
                    var validationResult = await _formulaService.ValidateExpressionAsync(new ValidateExpressionDto
                    {
                        ExpressionText = dto.ExpressionText,
                        FormBuilderId = tab.FormBuilderId
                    });
                    
                    if (!validationResult.Success)
                    {
                        return ValidationResult.Failure(validationResult.ErrorMessage ?? "Invalid expression");
                    }
                }

                // Validate ResultType
                if (!string.IsNullOrWhiteSpace(dto.ResultType))
                {
                    var validResultTypes = new[] { "decimal", "integer", "int", "text", "string" };
                    if (!validResultTypes.Contains(dto.ResultType.ToLower()))
                    {
                        var message = _localizer?["FormField_InvalidResultType"] ?? $"Invalid result type. Must be one of: {string.Join(", ", validResultTypes)}";
                        return ValidationResult.Failure(message);
                    }
                }
            }
            else
            {
                // Non-calculated fields should not have calculation properties
                if (!string.IsNullOrWhiteSpace(dto.ExpressionText))
                {
                    var message = _localizer?["FormField_ExpressionNotAllowed"] ?? "Expression can only be set for Calculated field type";
                    return ValidationResult.Failure(message);
                }
            }

            // Validate MinValue < MaxValue when both are provided
            if (dto.MinValue.HasValue && dto.MaxValue.HasValue && dto.MinValue.Value >= dto.MaxValue.Value)
            {
                var message = _localizer?["FormField_MinValueMustBeLessThanMaxValue"] ?? "Min Value must be less than Max Value";
                return ValidationResult.Failure(message);
            }

            // Validate MinValue < MaxValue when both are provided
            if (dto.MinValue.HasValue && dto.MaxValue.HasValue && dto.MinValue.Value >= dto.MaxValue.Value)
            {
                var message = _localizer?["FormField_MinValueMustBeLessThanMaxValue"] ?? "Min Value must be less than Max Value";
                return ValidationResult.Failure(message);
            }

            // FieldOptions are optional - no validation needed
            // If provided, they will be created after the field is created

            return ValidationResult.Success();
        }

        protected override async Task<ValidationResult> ValidateUpdateAsync(int id, UpdateFormFieldDto dto, FORM_FIELDS entity)
        {
            // Validate field code uniqueness only if FieldCode has changed
            // Compare trimmed values to handle whitespace differences
            var dtoFieldCode = dto.FieldCode?.Trim() ?? string.Empty;
            var entityFieldCode = entity.FieldCode?.Trim() ?? string.Empty;
            
            if (!string.IsNullOrEmpty(dtoFieldCode) && dtoFieldCode != entityFieldCode)
            {
                // Validate field code uniqueness - only check non-deleted records
                // This allows reusing FieldCode from soft-deleted records
                var isUnique = await _unitOfWork.FormFieldRepository.IsFieldCodeUniqueAsync(dtoFieldCode, id);
                
                if (!isUnique)
                {
                    // Double-check: Make sure we're only checking non-deleted records
                    var conflictingField = await Repository.SingleOrDefaultAsync(
                        f => f.FieldCode == dtoFieldCode && f.Id != id && !f.IsDeleted,
                        asNoTracking: true);
                        
                    if (conflictingField != null)
                    {
                        // Found a non-deleted record with same FieldCode - this is a real duplicate
                        DuplicateValidationHelper.LogDuplicateDetection(
                            _logger,
                            "FormField",
                            "FieldCode",
                            dtoFieldCode,
                            conflictingField.Id,
                            null,
                            conflictingField.IsDeleted
                        );

                        var message = _localizer?["FormField_FieldCodeExists", dtoFieldCode] 
                            ?? DuplicateValidationHelper.FormatDuplicateErrorMessage("Field", "code", dtoFieldCode);
                        return ValidationResult.Failure(message);
                    }
                    else
                    {
                        // IsFieldCodeUniqueAsync returned false but no non-deleted record found
                        // This shouldn't happen, but log a warning and allow update
                        DuplicateValidationHelper.LogDuplicateWarning(_logger, "FormField", "FieldCode", dtoFieldCode);
                        // Allow update to proceed (may be a race condition or soft-deleted record)
                    }
                }
            }

            // Field name can be duplicated - no uniqueness validation needed

            // Validate GridId if provided (for Grid field type)
            if (dto.GridId.HasValue)
            {
                // التحقق من وجود Grid
                var grid = await _unitOfWork.FormGridRepository.GetByIdAsync(dto.GridId.Value);
                if (grid == null)
                {
                    var message = _localizer?["FormField_GridNotFound"] ?? "Grid not found";
                    return ValidationResult.Failure(message);
                }
                
                // التحقق من أن Grid ينتمي لنفس Tab
                if (grid.TabId.HasValue && grid.TabId != dto.TabId)
                {
                    var message = _localizer?["FormField_GridNotInTab"] ?? "Grid does not belong to this tab";
                    return ValidationResult.Failure(message);
                }
            }

            // Validate calculation fields - check if ExpressionText is provided
            bool isCalculatedField = !string.IsNullOrWhiteSpace(dto.ExpressionText);

            if (isCalculatedField)
            {
                // Expression is required for calculated fields
                if (string.IsNullOrWhiteSpace(dto.ExpressionText))
                {
                    var message = _localizer?["FormField_ExpressionRequired"] ?? "Expression is required for calculated fields";
                    return ValidationResult.Failure(message);
                }

                // Check for circular reference - expression cannot reference itself
                var fieldCodePattern = @"\[([A-Za-z_][A-Za-z0-9_]*)\]";
                var matches = Regex.Matches(dto.ExpressionText, fieldCodePattern);
                var referencedFieldCodes = matches.Cast<Match>()
                    .Select(m => m.Groups[1].Value.ToUpper())
                    .Distinct()
                    .ToList();

                if (referencedFieldCodes.Contains(dto.FieldCode.ToUpper()))
                {
                    var message = _localizer?["FormField_CircularReference"] ?? $"Expression cannot reference the same field '{dto.FieldCode}'. This would create a circular reference.";
                    return ValidationResult.Failure(message);
                }

                // Validate expression syntax and field references
                var tab = await _unitOfWork.FormTabRepository.SingleOrDefaultAsync(t => t.Id == dto.TabId);
                if (tab != null)
                {
                    var validationResult = await _formulaService.ValidateExpressionAsync(new ValidateExpressionDto
                    {
                        ExpressionText = dto.ExpressionText,
                        FormBuilderId = tab.FormBuilderId
                    });
                    
                    if (!validationResult.Success)
                    {
                        return ValidationResult.Failure(validationResult.ErrorMessage ?? "Invalid expression");
                    }
                }

                // Validate ResultType
                if (!string.IsNullOrWhiteSpace(dto.ResultType))
                {
                    var validResultTypes = new[] { "decimal", "integer", "int", "text", "string" };
                    if (!validResultTypes.Contains(dto.ResultType.ToLower()))
                    {
                        var message = _localizer?["FormField_InvalidResultType"] ?? $"Invalid result type. Must be one of: {string.Join(", ", validResultTypes)}";
                        return ValidationResult.Failure(message);
                    }
                }
            }
            else
            {
                // Non-calculated fields should not have calculation properties
                if (!string.IsNullOrWhiteSpace(dto.ExpressionText))
                {
                    var message = _localizer?["FormField_ExpressionNotAllowed"] ?? "Expression can only be set for Calculated field type";
                    return ValidationResult.Failure(message);
                }
            }

            // Validate MinValue < MaxValue when both are provided
            var minValue = dto.MinValue ?? entity.MinValue;
            var maxValue = dto.MaxValue ?? entity.MaxValue;
            if (minValue.HasValue && maxValue.HasValue && minValue.Value >= maxValue.Value)
            {
                var message = _localizer?["FormField_MinValueMustBeLessThanMaxValue"] ?? "Min Value must be less than Max Value";
                return ValidationResult.Failure(message);
            }

            return ValidationResult.Success();
        }

        public override async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id && !e.IsDeleted, asNoTracking: false);
            if (entity == null)
            {
                var message = _localizer?["FormField_NotFound"] ?? "Field not found";
                return ServiceResult<bool>.NotFound(message);
            }

            // Always use soft delete for fields using IsDeleted flag
            entity.IsDeleted = true;
            entity.DeletedDate = DateTime.UtcNow;
            entity.UpdatedDate = DateTime.UtcNow;
            Repository.Update(entity);
            await _unitOfWork.CompleteAsyn();
            
            return ServiceResult<bool>.Ok(true);
        }

        public override async Task<ServiceResult<FormFieldDto>> RestoreAsync(int id)
        {
            return await base.RestoreAsync(id);
        }
    }
}
