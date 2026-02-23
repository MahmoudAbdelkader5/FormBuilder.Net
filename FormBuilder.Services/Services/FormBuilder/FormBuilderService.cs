using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using formBuilder.Domian.Interfaces;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domian.Entitys.froms;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Services.Services.Base;
using FormBuilder.Core.DTOS.FormTabs;
using FormBuilder.API.Models;
using FormBuilder.Services.Helpers;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace FormBuilder.Services.Services
{
    public class FormBuilderService
        : BaseService<FORM_BUILDER, FormBuilderDto, CreateFormBuilderDto, UpdateFormBuilderDto>,
          IFormBuilderService
    {
        private readonly IStringLocalizer<FormBuilderService>? _localizer;
        private readonly ILogger<FormBuilderService>? _logger;

        public FormBuilderService(IunitOfwork unitOfWork, IMapper mapper, IStringLocalizer<FormBuilderService>? localizer = null, ILogger<FormBuilderService>? logger = null)
            : base(unitOfWork, mapper, null)
        {
            _localizer = localizer;
            _logger = logger;
        }

        protected override IBaseRepository<FORM_BUILDER> Repository => _unitOfWork.FormBuilderRepository;

        public async Task<ServiceResult<FormBuilderDto>> GetByCodeAsync(string formCode, bool asNoTracking = false)
        {
            if (string.IsNullOrWhiteSpace(formCode))
            {
                var message = _localizer?["FormBuilder_FormCodeRequired"] ?? "Form code is required";
                return ServiceResult<FormBuilderDto>.BadRequest(message);
            }

            // Load the form with its tabs and fields for public/anonymous usage
            var entity = await _unitOfWork.FormBuilderRepository.GetFormWithTabsAndFieldsByCodeAsync(formCode.Trim());
            if (entity == null) return ServiceResult<FormBuilderDto>.NotFound();

            // Map the basic form data
            var dto = _mapper.Map<FormBuilderDto>(entity);

            // Manually map tabs and fields for the public form view
            dto.Tabs = entity.FORM_TABS
                .Where(t => t.IsActive)
                .OrderBy(t => t.TabOrder)
                .Select(t => new FormTabDto
                {
                    Id = t.Id,
                    FormBuilderId = t.FormBuilderId,
                    TabName = t.TabName,
                    ForeignTabName = t.ForeignTabName,
                    TabCode = t.TabCode,
                    TabOrder = t.TabOrder,
                    IsActive = t.IsActive,
                    CreatedByUserId = t.CreatedByUserId,
                    CreatedDate = t.CreatedDate,
                    Fields = t.FORM_FIELDS
                        .Where(f => f.IsActive)
                        .OrderBy(f => f.FieldOrder)
                        .Select(f => new FormFieldDto
                        {
                            Id = f.Id,
                            TabId = f.TabId,
                            FieldTypeId = f.FieldTypeId,
                            FieldTypeName = null,
                            FieldName = f.FieldName,
                            ForeignFieldName = f.ForeignFieldName,
                            FieldCode = f.FieldCode,
                            FieldOrder = f.FieldOrder,
                            Placeholder = f.Placeholder,
                            ForeignPlaceholder = f.ForeignPlaceholder,
                            HintText = f.HintText,
                            ForeignHintText = f.ForeignHintText,
                            IsMandatory = f.IsMandatory ?? false,
                            IsEditable = f.IsEditable ?? false,
                            IsVisible = f.IsVisible,
                            DefaultValueJson = f.DefaultValueJson,
                            MinValue = f.MinValue,
                            MaxValue = f.MaxValue,
                            RegexPattern = f.RegexPattern,
                            ValidationMessage = f.ValidationMessage,
                            ForeignValidationMessage = f.ForeignValidationMessage,
                            // Calculation Fields Properties
                            ExpressionText = f.ExpressionText,
                            CalculationMode = f.CalculationMode,
                            RecalculateOn = f.RecalculateOn,
                            ResultType = f.ResultType,
                            CreatedDate = f.CreatedDate,
                            CreatedByUserId = f.CreatedByUserId,
                            IsActive = f.IsActive,
                            // FieldType removed
                            // For public view we only need basic option data
                            FieldOptions = f.FIELD_OPTIONS?
                                .Where(fo => fo.IsActive)
                                .Select(fo => new FieldOptionDto
                                {
                                    Id = fo.Id,
                                    FieldId = fo.FieldId,
                                    OptionText = fo.OptionText,
                                    ForeignOptionText = fo.ForeignOptionText,
                                    OptionValue = fo.OptionValue,
                                    OptionOrder = fo.OptionOrder,
                                    IsActive = fo.IsActive
                                }).ToList() ?? new System.Collections.Generic.List<FieldOptionDto>(),
                            // Map Field Data Source - tells frontend where to load options from
                            FieldDataSource = f.FIELD_DATA_SOURCES?
                                .Where(fds => fds.IsActive)
                                .Select(fds => new FieldDataSourceDto
                                {
                                    Id = fds.Id,
                                    FieldId = fds.FieldId,
                                    SourceType = fds.SourceType,
                                    ApiUrl = fds.ApiUrl,
                                    ApiPath = fds.ApiPath,
                                    HttpMethod = fds.HttpMethod,
                                    RequestBodyJson = fds.RequestBodyJson,
                                    ValuePath = fds.ValuePath,
                                    TextPath = fds.TextPath,
                                    ConfigurationJson = fds.ConfigurationJson,
                                    IsActive = fds.IsActive
                                }).FirstOrDefault()
                        }).ToList()
                })
                .ToList();

            return ServiceResult<FormBuilderDto>.Ok(dto);
        }

        public async Task<ServiceResult<IEnumerable<FormBuilderDto>>> GetAllAsync(Expression<Func<FORM_BUILDER, bool>>? filter = null)
        {
            return await base.GetAllAsync(filter);
        }

        public async Task<ServiceResult<PagedResult<FormBuilderDto>>> GetPagedAsync(int page = 1, int pageSize = 20)
        {
            return await base.GetPagedAsync(page, pageSize);
        }

        public async Task<ServiceResult<bool>> IsFormCodeExistsAsync(string formCode, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(formCode))
            {
                var message = _localizer?["FormBuilder_FormCodeRequired"] ?? "Form code is required";
                return ServiceResult<bool>.BadRequest(message);
            }

            var exists = await _unitOfWork.FormBuilderRepository.IsFormCodeExistsAsync(formCode.Trim(), excludeId);
            return ServiceResult<bool>.Ok(exists);
        }

        protected override async Task<ValidationResult> ValidateCreateAsync(CreateFormBuilderDto dto)
        {
            var exists = await _unitOfWork.FormBuilderRepository.IsFormCodeExistsAsync(dto.FormCode);
            if (exists)
            {
                // Log duplicate warning
                var existingForm = await Repository.SingleOrDefaultAsync(f => f.FormCode == dto.FormCode && !f.IsDeleted);
                if (existingForm != null)
                {
                    DuplicateValidationHelper.LogDuplicateDetection(
                        _logger,
                        "FormBuilder",
                        "FormCode",
                        dto.FormCode,
                        existingForm.Id,
                        null,
                        existingForm.IsDeleted
                    );
                }
                else
                {
                    DuplicateValidationHelper.LogDuplicateWarning(_logger, "FormBuilder", "FormCode", dto.FormCode);
                }

                var message = DuplicateValidationHelper.FormatDuplicateErrorMessage("Form", "code", dto.FormCode);
                return ValidationResult.Failure(message);
            }

            return ValidationResult.Success();
        }

        protected override async Task<ValidationResult> ValidateUpdateAsync(int id, UpdateFormBuilderDto dto, FORM_BUILDER entity)
        {
            var exists = await _unitOfWork.FormBuilderRepository.IsFormCodeExistsAsync(dto.FormCode, id);
            if (exists)
            {
                // Log duplicate warning
                var conflictingForm = await Repository.SingleOrDefaultAsync(f => f.FormCode == dto.FormCode && f.Id != id && !f.IsDeleted);
                if (conflictingForm != null)
                {
                    DuplicateValidationHelper.LogDuplicateDetection(
                        _logger,
                        "FormBuilder",
                        "FormCode",
                        dto.FormCode,
                        conflictingForm.Id,
                        null,
                        conflictingForm.IsDeleted
                    );
                }

                var message = DuplicateValidationHelper.FormatDuplicateErrorMessage("Form", "code", dto.FormCode);
                return ValidationResult.Failure(message);
            }

            return ValidationResult.Success();
        }

        public override async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id && !e.IsDeleted, asNoTracking: false);
            if (entity == null)
            {
                var message = _localizer?["FormBuilder_NotFound"] ?? "Form not found";
                return ServiceResult<bool>.NotFound(message);
            }

            // Always use soft delete for forms using IsDeleted flag
            entity.IsDeleted = true;
            entity.IsPublished = false; // ✅ Unpublish the form so users cannot access it
            entity.DeletedDate = DateTime.UtcNow;
            entity.UpdatedDate = DateTime.UtcNow;
            Repository.Update(entity);
            await _unitOfWork.CompleteAsyn();
            
            return ServiceResult<bool>.Ok(true);
        }

        public async Task<ServiceResult<FormBuilderDto>> ToggleActiveAsync(int id, bool isActive)
        {
            var entity = await Repository.SingleOrDefaultAsync(e => e.Id == id && !e.IsDeleted, asNoTracking: false);
            if (entity == null)
            {
                var message = _localizer?["FormBuilder_NotFound"] ?? "Form not found or has been deleted";
                return ServiceResult<FormBuilderDto>.NotFound(message);
            }

            entity.IsActive = isActive;
            entity.UpdatedDate = DateTime.UtcNow;
            Repository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            var dto = _mapper.Map<FormBuilderDto>(entity);
            return ServiceResult<FormBuilderDto>.Ok(dto);
        }

        public override async Task<ServiceResult<FormBuilderDto>> RestoreAsync(int id)
        {
            return await base.RestoreAsync(id);
        }

        public async Task<ServiceResult<FormBuilderDto>> DuplicateAsync(int id, string? newFormCode = null, string? newFormName = null)
        {
            try
            {
                // Load the original form with all related data
                var originalForm = await _unitOfWork.FormBuilderRepository.GetFormWithAllDataForDuplicateAsync(id);
                if (originalForm == null)
                {
                    var message = _localizer?["FormBuilder_NotFound"] ?? "Form not found";
                    return ServiceResult<FormBuilderDto>.NotFound(message);
                }

                // Generate new form code if not provided
                var generatedFormCode = newFormCode ?? $"{originalForm.FormCode}_COPY_{DateTime.UtcNow:yyyyMMddHHmmss}";
                
                // Check if the new form code already exists
                var codeExists = await _unitOfWork.FormBuilderRepository.IsFormCodeExistsAsync(generatedFormCode);
                if (codeExists)
                {
                    var message = _localizer?["FormBuilder_FormCodeExists"] ?? $"Form code '{generatedFormCode}' already exists";
                    return ServiceResult<FormBuilderDto>.BadRequest(message);
                }

                // Generate new form name if not provided
                var generatedFormName = newFormName ?? $"{originalForm.FormName} (Copy)";

                // Create new Form Builder
                var newForm = new FORM_BUILDER
                {
                    FormName = generatedFormName,
                    ForeignFormName = originalForm.ForeignFormName,
                    FormCode = generatedFormCode,
                    Description = originalForm.Description,
                    ForeignDescription = originalForm.ForeignDescription,
                    Version = 1,
                    IsPublished = originalForm.IsPublished, // Preserve the original form's published status
                    IsSapEnabled = originalForm.IsSapEnabled,
                    SapExecutionMode = originalForm.SapExecutionMode,
                    IsActive = true,
                    CreatedByUserId = originalForm.CreatedByUserId,
                    CreatedDate = DateTime.UtcNow,
                    FORM_TABS = new List<FORM_TABS>(),
                    FORMULAS = new List<FORMULAS>(),
                    FORM_RULES = new List<FORM_RULES>(),
                    FORM_GRIDS = new List<FORM_GRIDS>(),
                    FORM_ATTACHMENT_TYPES = new List<FORM_ATTACHMENT_TYPES>(),
                    FORM_BUTTONS = new List<FORM_BUTTONS>(),
                    FORM_VALIDATION_RULES = new List<FORM_VALIDATION_RULES>()
                };

                // Dictionary to map old tab IDs to new tab IDs
                var tabIdMapping = new Dictionary<int, int>();
                
                // Dictionary to map old FieldCodes to new FieldCodes (for updating Rules/Formulas)
                var fieldCodeMapping = new Dictionary<string, string>();

                // Dictionary to map old TabCodes to new TabCodes
                var tabCodeMapping = new Dictionary<string, string>();
                
                // Copy Tabs and Fields
                foreach (var originalTab in originalForm.FORM_TABS.OrderBy(t => t.TabOrder))
                {
                    // توليد TabCode فريد للتاب الجديد
                    var newTabCode = await GenerateUniqueTabCodeAsync(originalTab.TabCode);
                    
                    // حفظ mapping بين TabCode القديم والجديد
                    if (!string.IsNullOrEmpty(originalTab.TabCode))
                    {
                        tabCodeMapping[originalTab.TabCode] = newTabCode;
                    }
                    
                    var newTab = new FORM_TABS
                    {
                        FormBuilderId = 0, // Will be set after saving
                        TabName = originalTab.TabName,
                        ForeignTabName = originalTab.ForeignTabName,
                        TabCode = newTabCode,
                        TabOrder = originalTab.TabOrder,
                        IsActive = originalTab.IsActive,
                        CreatedByUserId = originalTab.CreatedByUserId,
                        CreatedDate = DateTime.UtcNow,
                        FORM_FIELDS = new List<FORM_FIELDS>()
                    };

                    newForm.FORM_TABS.Add(newTab);
                }

                // Save the form first to get the new FormBuilderId
                Repository.Add(newForm);
                await _unitOfWork.CompleteAsyn();

                // Update tab FormBuilderId and copy fields
                int tabIndex = 0;
                foreach (var originalTab in originalForm.FORM_TABS.OrderBy(t => t.TabOrder))
                {
                    var newTab = newForm.FORM_TABS.ElementAt(tabIndex);
                    newTab.FormBuilderId = newForm.Id;
                    tabIdMapping[originalTab.Id] = newTab.Id;

                    // Copy Fields
                    foreach (var originalField in originalTab.FORM_FIELDS.OrderBy(f => f.FieldOrder))
                    {
                        // توليد FieldCode فريد للحقل الجديد
                        var newFieldCode = await GenerateUniqueFieldCodeAsync(originalField.FieldCode);
                        
                        // حفظ mapping بين FieldCode القديم والجديد (فقط إذا كان الكود الأصلي غير فارغ)
                        if (!string.IsNullOrEmpty(originalField.FieldCode))
                        {
                            fieldCodeMapping[originalField.FieldCode] = newFieldCode;
                        }
                        
                        var newField = new FORM_FIELDS
                        {
                            TabId = newTab.Id,
                            FieldTypeId = originalField.FieldTypeId,
                            GridId = originalField.GridId, // Will be updated after grids are copied
                            FieldName = originalField.FieldName,
                            ForeignFieldName = originalField.ForeignFieldName,
                            FieldCode = newFieldCode,
                            FieldOrder = originalField.FieldOrder,
                            Placeholder = originalField.Placeholder,
                            ForeignPlaceholder = originalField.ForeignPlaceholder,
                            HintText = originalField.HintText,
                            ForeignHintText = originalField.ForeignHintText,
                            IsMandatory = originalField.IsMandatory,
                            IsEditable = originalField.IsEditable,
                            IsVisible = originalField.IsVisible,
                            DefaultValueJson = originalField.DefaultValueJson,
                            MinValue = originalField.MinValue,
                            MaxValue = originalField.MaxValue,
                            RegexPattern = originalField.RegexPattern,
                            ValidationMessage = originalField.ValidationMessage,
                            ForeignValidationMessage = originalField.ForeignValidationMessage,
                            ExpressionText = originalField.ExpressionText,
                            CalculationMode = originalField.CalculationMode,
                            RecalculateOn = originalField.RecalculateOn,
                            ResultType = originalField.ResultType,
                            IsActive = originalField.IsActive,
                            CreatedByUserId = originalField.CreatedByUserId,
                            CreatedDate = DateTime.UtcNow,
                            FIELD_OPTIONS = new List<FIELD_OPTIONS>(),
                            FIELD_DATA_SOURCES = new List<FIELD_DATA_SOURCES>()
                        };

                        // Copy Field Options
                        foreach (var originalOption in originalField.FIELD_OPTIONS)
                        {
                            var newOption = new FIELD_OPTIONS
                            {
                                FieldId = 0, // Will be set after saving
                                OptionText = originalOption.OptionText,
                                ForeignOptionText = originalOption.ForeignOptionText,
                                OptionValue = originalOption.OptionValue,
                                OptionOrder = originalOption.OptionOrder,
                                IsDefault = originalOption.IsDefault,
                                IsActive = originalOption.IsActive,
                                CreatedByUserId = originalOption.CreatedByUserId,
                                CreatedDate = DateTime.UtcNow
                            };
                            newField.FIELD_OPTIONS.Add(newOption);
                        }

                        // Copy Field Data Sources
                        foreach (var originalDataSource in originalField.FIELD_DATA_SOURCES)
                        {
                            var newDataSource = new FIELD_DATA_SOURCES
                            {
                                FieldId = 0, // Will be set after saving
                                SourceType = originalDataSource.SourceType,
                                ApiUrl = originalDataSource.ApiUrl,
                                ApiPath = originalDataSource.ApiPath,
                                HttpMethod = originalDataSource.HttpMethod,
                                RequestBodyJson = originalDataSource.RequestBodyJson,
                                ValuePath = originalDataSource.ValuePath,
                                TextPath = originalDataSource.TextPath,
                                ConfigurationJson = originalDataSource.ConfigurationJson,
                                IsActive = originalDataSource.IsActive,
                                CreatedByUserId = originalDataSource.CreatedByUserId,
                                CreatedDate = DateTime.UtcNow
                            };
                            newField.FIELD_DATA_SOURCES.Add(newDataSource);
                        }

                        newTab.FORM_FIELDS.Add(newField);
                    }

                    tabIndex++;
                }

                // Save tabs and fields
                await _unitOfWork.CompleteAsyn();

                // Dictionary to map old field IDs to new field IDs
                var fieldIdMapping = new Dictionary<int, int>();
                tabIndex = 0;
                foreach (var originalTab in originalForm.FORM_TABS.OrderBy(t => t.TabOrder))
                {
                    var newTab = newForm.FORM_TABS.ElementAt(tabIndex);
                    int fieldIndex = 0;
                    foreach (var originalField in originalTab.FORM_FIELDS.OrderBy(f => f.FieldOrder))
                    {
                        var newField = newTab.FORM_FIELDS.ElementAt(fieldIndex);
                        fieldIdMapping[originalField.Id] = newField.Id;

                        // Update FieldId in Options and DataSources
                        foreach (var option in newField.FIELD_OPTIONS)
                        {
                            option.FieldId = newField.Id;
                        }
                        foreach (var dataSource in newField.FIELD_DATA_SOURCES)
                        {
                            dataSource.FieldId = newField.Id;
                        }

                        fieldIndex++;
                    }
                    tabIndex++;
                }

                // تحديث ExpressionText في الـ Fields بالـ FieldCodes الجديدة
                foreach (var newTab in newForm.FORM_TABS)
                {
                    foreach (var newField in newTab.FORM_FIELDS)
                    {
                        if (!string.IsNullOrEmpty(newField.ExpressionText))
                        {
                            newField.ExpressionText = ReplaceFieldCodesInText(newField.ExpressionText, fieldCodeMapping);
                        }
                    }
                }

                // Copy Formulas
                foreach (var originalFormula in originalForm.FORMULAS)
                {
                    // تحديث ExpressionText بالـ FieldCodes الجديدة
                    var updatedExpressionText = ReplaceFieldCodesInText(originalFormula.ExpressionText, fieldCodeMapping);
                    
                    var newFormula = new FORMULAS
                    {
                        FormBuilderId = newForm.Id,
                        Name = originalFormula.Name,
                        Code = originalFormula.Code,
                        ExpressionText = updatedExpressionText,
                        ResultFieldId = originalFormula.ResultFieldId.HasValue && fieldIdMapping.ContainsKey(originalFormula.ResultFieldId.Value)
                            ? fieldIdMapping[originalFormula.ResultFieldId.Value]
                            : null,
                        IsActive = originalFormula.IsActive,
                        CreatedByUserId = originalFormula.CreatedByUserId,
                        CreatedDate = DateTime.UtcNow,
                        FORMULA_VARIABLES = new List<FORMULA_VARIABLES>()
                    };

                    // Copy Formula Variables
                    foreach (var originalVariable in originalFormula.FORMULA_VARIABLES)
                    {
                        var newVariable = new FORMULA_VARIABLES
                        {
                            FormulaId = 0, // Will be set after saving
                            VariableName = originalVariable.VariableName,
                            SourceFieldId = fieldIdMapping.ContainsKey(originalVariable.SourceFieldId)
                                ? fieldIdMapping[originalVariable.SourceFieldId]
                                : originalVariable.SourceFieldId,
                            IsActive = originalVariable.IsActive,
                            CreatedByUserId = originalVariable.CreatedByUserId,
                            CreatedDate = DateTime.UtcNow
                        };
                        newFormula.FORMULA_VARIABLES.Add(newVariable);
                    }

                    newForm.FORMULAS.Add(newFormula);
                }

                // Copy Rules
                foreach (var originalRule in originalForm.FORM_RULES)
                {
                    // تحديث ConditionField بالـ FieldCode الجديد
                    var updatedConditionField = !string.IsNullOrEmpty(originalRule.ConditionField) && fieldCodeMapping.ContainsKey(originalRule.ConditionField)
                        ? fieldCodeMapping[originalRule.ConditionField]
                        : originalRule.ConditionField;
                    
                    // تحديث RuleJson بالـ FieldCodes الجديدة
                    var updatedRuleJson = ReplaceFieldCodesInText(originalRule.RuleJson, fieldCodeMapping);
                    
                    var newRule = new FORM_RULES
                    {
                        FormBuilderId = newForm.Id,
                        RuleName = originalRule.RuleName,
                        ConditionField = updatedConditionField,
                        ConditionOperator = originalRule.ConditionOperator,
                        ConditionValue = originalRule.ConditionValue,
                        ConditionValueType = originalRule.ConditionValueType,
                        RuleJson = updatedRuleJson,
                        IsActive = originalRule.IsActive,
                        ExecutionOrder = originalRule.ExecutionOrder,
                        CreatedByUserId = originalRule.CreatedByUserId,
                        CreatedDate = DateTime.UtcNow,
                        FORM_RULE_ACTIONS = new List<FORM_RULE_ACTIONS>()
                    };

                    // Copy Rule Actions
                    foreach (var originalAction in originalRule.FORM_RULE_ACTIONS)
                    {
                        // تحديث FieldCode بالـ FieldCode الجديد
                        var updatedActionFieldCode = !string.IsNullOrEmpty(originalAction.FieldCode) && fieldCodeMapping.ContainsKey(originalAction.FieldCode)
                            ? fieldCodeMapping[originalAction.FieldCode]
                            : originalAction.FieldCode;
                        
                        // تحديث Expression بالـ FieldCodes الجديدة
                        var updatedActionExpression = ReplaceFieldCodesInText(originalAction.Expression, fieldCodeMapping);
                        
                        var newAction = new FORM_RULE_ACTIONS
                        {
                            RuleId = 0, // Will be set after saving
                            ActionType = originalAction.ActionType,
                            FieldCode = updatedActionFieldCode,
                            Value = originalAction.Value,
                            Expression = updatedActionExpression,
                            IsElseAction = originalAction.IsElseAction,
                            ActionOrder = originalAction.ActionOrder,
                            IsActive = originalAction.IsActive,
                            CreatedByUserId = originalAction.CreatedByUserId,
                            CreatedDate = DateTime.UtcNow
                        };
                        newRule.FORM_RULE_ACTIONS.Add(newAction);
                    }

                    newForm.FORM_RULES.Add(newRule);
                }

                // Copy Grids (if any)
                var gridIdMapping = new Dictionary<int, int>();
                foreach (var originalGrid in originalForm.FORM_GRIDS)
                {
                    // تحديث GridRulesJson بالـ FieldCodes الجديدة
                    var updatedGridRulesJson = ReplaceFieldCodesInText(originalGrid.GridRulesJson, fieldCodeMapping);
                    
                    var newGrid = new FORM_GRIDS
                    {
                        FormBuilderId = newForm.Id,
                        GridName = originalGrid.GridName,
                        GridCode = originalGrid.GridCode,
                        TabId = originalGrid.TabId.HasValue && tabIdMapping.ContainsKey(originalGrid.TabId.Value)
                            ? tabIdMapping[originalGrid.TabId.Value]
                            : null,
                        GridOrder = originalGrid.GridOrder,
                        MinRows = originalGrid.MinRows,
                        MaxRows = originalGrid.MaxRows,
                        GridRulesJson = updatedGridRulesJson,
                        IsActive = originalGrid.IsActive,
                        CreatedByUserId = originalGrid.CreatedByUserId,
                        CreatedDate = DateTime.UtcNow,
                        FORM_GRID_COLUMNS = new List<FORM_GRID_COLUMNS>()
                    };

                    // Copy Grid Columns
                    foreach (var originalColumn in originalGrid.FORM_GRID_COLUMNS)
                    {
                        var newColumn = new FORM_GRID_COLUMNS
                        {
                            GridId = 0, // Will be set after saving
                            ColumnName = originalColumn.ColumnName,
                            ColumnCode = originalColumn.ColumnCode,
                            ColumnOrder = originalColumn.ColumnOrder,
                            FieldTypeId = originalColumn.FieldTypeId,
                            IsMandatory = originalColumn.IsMandatory,
                            DataType = originalColumn.DataType,
                            MaxLength = originalColumn.MaxLength,
                            DefaultValueJson = originalColumn.DefaultValueJson,
                            ValidationRuleJson = originalColumn.ValidationRuleJson,
                            IsReadOnly = originalColumn.IsReadOnly,
                            IsVisible = originalColumn.IsVisible,
                            VisibilityRuleJson = originalColumn.VisibilityRuleJson,
                            IsActive = originalColumn.IsActive,
                            CreatedByUserId = originalColumn.CreatedByUserId,
                            CreatedDate = DateTime.UtcNow,
                            GRID_COLUMN_OPTIONS = new List<GRID_COLUMN_OPTIONS>(),
                            GRID_COLUMN_DATA_SOURCES = new List<GRID_COLUMN_DATA_SOURCES>()
                        };

                        // Copy Grid Column Options
                        foreach (var originalOption in originalColumn.GRID_COLUMN_OPTIONS)
                        {
                            var newOption = new GRID_COLUMN_OPTIONS
                            {
                                ColumnId = 0, // Will be set after saving
                                OptionText = originalOption.OptionText,
                                ForeignOptionText = originalOption.ForeignOptionText,
                                OptionValue = originalOption.OptionValue,
                                OptionOrder = originalOption.OptionOrder,
                                IsDefault = originalOption.IsDefault,
                                IsActive = originalOption.IsActive,
                                CreatedByUserId = originalOption.CreatedByUserId,
                                CreatedDate = DateTime.UtcNow
                            };
                            newColumn.GRID_COLUMN_OPTIONS.Add(newOption);
                        }

                        // Copy Grid Column Data Sources
                        foreach (var originalDataSource in originalColumn.GRID_COLUMN_DATA_SOURCES)
                        {
                            var newDataSource = new GRID_COLUMN_DATA_SOURCES
                            {
                                ColumnId = 0, // Will be set after saving
                                SourceType = originalDataSource.SourceType,
                                ApiUrl = originalDataSource.ApiUrl,
                                ApiPath = originalDataSource.ApiPath,
                                HttpMethod = originalDataSource.HttpMethod,
                                RequestBodyJson = originalDataSource.RequestBodyJson,
                                ValuePath = originalDataSource.ValuePath,
                                TextPath = originalDataSource.TextPath,
                                ConfigurationJson = originalDataSource.ConfigurationJson,
                                ArrayPropertyNames = originalDataSource.ArrayPropertyNames,
                                IsActive = originalDataSource.IsActive,
                                CreatedByUserId = originalDataSource.CreatedByUserId,
                                CreatedDate = DateTime.UtcNow
                            };
                            newColumn.GRID_COLUMN_DATA_SOURCES.Add(newDataSource);
                        }

                        newGrid.FORM_GRID_COLUMNS.Add(newColumn);
                    }

                    newForm.FORM_GRIDS.Add(newGrid);
                }

                // Save all changes
                await _unitOfWork.CompleteAsyn();

                // Update GridId in fields that reference grids
                var gridIndex = 0;
                foreach (var originalGrid in originalForm.FORM_GRIDS)
                {
                    var newGrid = newForm.FORM_GRIDS.ElementAt(gridIndex);
                    gridIdMapping[originalGrid.Id] = newGrid.Id;

                    // Update GridId in Grid Columns and ColumnId in Options/DataSources
                    foreach (var column in newGrid.FORM_GRID_COLUMNS)
                    {
                        column.GridId = newGrid.Id;

                        // Update ColumnId in Grid Column Options
                        foreach (var option in column.GRID_COLUMN_OPTIONS)
                        {
                            option.ColumnId = column.Id;
                        }

                        // Update ColumnId in Grid Column Data Sources
                        foreach (var dataSource in column.GRID_COLUMN_DATA_SOURCES)
                        {
                            dataSource.ColumnId = column.Id;
                        }
                    }

                    gridIndex++;
                }

                // Update GridId in fields
                foreach (var tab in newForm.FORM_TABS)
                {
                    foreach (var field in tab.FORM_FIELDS)
                    {
                        if (field.GridId.HasValue && gridIdMapping.ContainsKey(field.GridId.Value))
                        {
                            field.GridId = gridIdMapping[field.GridId.Value];
                        }
                    }
                }

                // Update FormulaId in Formula Variables
                var formulaIndex = 0;
                foreach (var originalFormula in originalForm.FORMULAS)
                {
                    var newFormula = newForm.FORMULAS.ElementAt(formulaIndex);
                    foreach (var variable in newFormula.FORMULA_VARIABLES)
                    {
                        variable.FormulaId = newFormula.Id;
                    }
                    formulaIndex++;
                }

                // Update RuleId in Rule Actions
                var ruleIndex = 0;
                foreach (var originalRule in originalForm.FORM_RULES)
                {
                    var newRule = newForm.FORM_RULES.ElementAt(ruleIndex);
                    foreach (var action in newRule.FORM_RULE_ACTIONS)
                    {
                        action.RuleId = newRule.Id;
                    }
                    ruleIndex++;
                }

                // Copy Form Attachment Types
                foreach (var originalAttachmentType in originalForm.FORM_ATTACHMENT_TYPES)
                {
                    var newAttachmentType = new FORM_ATTACHMENT_TYPES
                    {
                        FormBuilderId = newForm.Id,
                        AttachmentTypeId = originalAttachmentType.AttachmentTypeId,
                        IsMandatory = originalAttachmentType.IsMandatory,
                        IsActive = originalAttachmentType.IsActive,
                        CreatedByUserId = originalAttachmentType.CreatedByUserId,
                        CreatedDate = DateTime.UtcNow
                    };
                    newForm.FORM_ATTACHMENT_TYPES.Add(newAttachmentType);
                }

                // Copy Form Buttons
                foreach (var originalButton in originalForm.FORM_BUTTONS)
                {
                    var newButton = new FORM_BUTTONS
                    {
                        FormBuilderId = newForm.Id,
                        ButtonName = originalButton.ButtonName,
                        ButtonCode = originalButton.ButtonCode,
                        ButtonOrder = originalButton.ButtonOrder,
                        Icon = originalButton.Icon,
                        ActionType = originalButton.ActionType,
                        ActionConfigJson = originalButton.ActionConfigJson,
                        IsVisibleDefault = originalButton.IsVisibleDefault,
                        IsActive = originalButton.IsActive,
                        CreatedByUserId = originalButton.CreatedByUserId,
                        CreatedDate = DateTime.UtcNow
                    };
                    newForm.FORM_BUTTONS.Add(newButton);
                }

                // Copy Form Validation Rules (with FieldId mapping)
                foreach (var originalValidationRule in originalForm.FORM_VALIDATION_RULES)
                {
                    // تحديث ExpressionText بالـ FieldCodes الجديدة
                    var updatedValidationExpressionText = ReplaceFieldCodesInText(originalValidationRule.ExpressionText, fieldCodeMapping);
                    
                    var newValidationRule = new FORM_VALIDATION_RULES
                    {
                        FormBuilderId = newForm.Id,
                        Level = originalValidationRule.Level,
                        FieldId = originalValidationRule.FieldId.HasValue && fieldIdMapping.ContainsKey(originalValidationRule.FieldId.Value)
                            ? fieldIdMapping[originalValidationRule.FieldId.Value]
                            : null,
                        ExpressionText = updatedValidationExpressionText,
                        ErrorMessage = originalValidationRule.ErrorMessage,
                        IsActive = originalValidationRule.IsActive,
                        CreatedByUserId = originalValidationRule.CreatedByUserId,
                        CreatedDate = DateTime.UtcNow
                    };
                    newForm.FORM_VALIDATION_RULES.Add(newValidationRule);
                }

                // Final save
                await _unitOfWork.CompleteAsyn();

                // Copy Document Types linked to the original form
                var originalDocumentTypes = await _unitOfWork.DocumentTypeRepository.GetByFormBuilderIdAsync(id);
                foreach (var originalDocType in originalDocumentTypes)
                {
                    // توليد Code فريد للـ Document Type الجديد
                    var newDocTypeCode = $"{originalDocType.Code}_CPY_{DateTime.UtcNow:yyyyMMddHHmmssfff}";
                    
                    var newDocumentType = new DOCUMENT_TYPES
                    {
                        FormBuilderId = newForm.Id,
                        Name = $"{originalDocType.Name} (Copy)",
                        Code = newDocTypeCode,
                        MenuCaption = originalDocType.MenuCaption,
                        MenuOrder = originalDocType.MenuOrder,
                        ParentMenuId = originalDocType.ParentMenuId,
                        IsActive = originalDocType.IsActive,
                        CreatedByUserId = originalDocType.CreatedByUserId,
                        CreatedDate = DateTime.UtcNow
                    };
                    
                    _unitOfWork.DocumentTypeRepository.Add(newDocumentType);
                }
                
                // Save Document Types
                await _unitOfWork.CompleteAsyn();

                // Return the new form
                var result = await GetByIdAsync(newForm.Id, asNoTracking: true);
                return result;
            }
            catch (Exception ex)
            {
                // Log the actual error for debugging
                _logger?.LogError(ex, "Error duplicating form. Inner exception: {InnerException}", ex.InnerException?.Message);
                
                // إرجاع رسالة الخطأ الفعلية للتشخيص
                var errorDetails = ex.InnerException != null 
                    ? $"{ex.Message} | Inner: {ex.InnerException.Message}" 
                    : ex.Message;
                    
                // إرجاع الخطأ الفعلي بدلاً من رسالة عامة
                return ServiceResult<FormBuilderDto>.Error($"Error duplicating form: {errorDetails}");
            }
        }

        /// <summary>
        /// استبدال الـ FieldCodes القديمة بالجديدة في النص
        /// </summary>
        private string? ReplaceFieldCodesInText(string? text, Dictionary<string, string> fieldCodeMapping)
        {
            if (string.IsNullOrEmpty(text) || fieldCodeMapping == null || !fieldCodeMapping.Any())
            {
                return text;
            }

            var result = text;
            // ترتيب الـ FieldCodes من الأطول للأقصر لتجنب استبدال جزئي
            foreach (var mapping in fieldCodeMapping.OrderByDescending(m => m.Key.Length))
            {
                // استبدال الـ FieldCode في النص
                result = result.Replace(mapping.Key, mapping.Value);
            }

            return result;
        }

        /// <summary>
        /// توليد FieldCode فريد بناءً على الكود الأصلي
        /// </summary>
        private async Task<string> GenerateUniqueFieldCodeAsync(string? originalFieldCode)
        {
            // حماية من القيم الفارغة
            if (string.IsNullOrEmpty(originalFieldCode))
            {
                originalFieldCode = "FIELD";
            }
            
            // إضافة timestamp لضمان التفرد
            var baseCode = $"{originalFieldCode}_CPY";
            var newCode = $"{baseCode}_{DateTime.UtcNow:yyyyMMddHHmmssfff}";
            
            // التحقق من أن الكود فريد
            var isUnique = await _unitOfWork.FormFieldRepository.IsFieldCodeUniqueAsync(newCode);
            if (isUnique)
            {
                return newCode;
            }
            
            // إذا لم يكن فريد (نادر جداً)، أضف رقم عشوائي
            var counter = 1;
            while (!await _unitOfWork.FormFieldRepository.IsFieldCodeUniqueAsync(newCode))
            {
                newCode = $"{baseCode}_{DateTime.UtcNow:yyyyMMddHHmmssfff}_{counter}";
                counter++;
                if (counter > 100) // حماية من الحلقة اللانهائية
                {
                    throw new InvalidOperationException($"Unable to generate unique field code for: {originalFieldCode}");
                }
            }
            
            return newCode;
        }

        /// <summary>
        /// توليد TabCode فريد بناءً على الكود الأصلي
        /// </summary>
        private async Task<string> GenerateUniqueTabCodeAsync(string? originalTabCode)
        {
            // حماية من القيم الفارغة
            if (string.IsNullOrEmpty(originalTabCode))
            {
                originalTabCode = "TAB";
            }
            
            // إضافة timestamp لضمان التفرد
            var baseCode = $"{originalTabCode}_CPY";
            var newCode = $"{baseCode}_{DateTime.UtcNow:yyyyMMddHHmmssfff}";
            
            // التحقق من أن الكود فريد
            var exists = await _unitOfWork.FormTabRepository.TabCodeExistsAsync(newCode);
            if (!exists)
            {
                return newCode;
            }
            
            // إذا كان موجود (نادر جداً)، أضف رقم عشوائي
            var counter = 1;
            while (await _unitOfWork.FormTabRepository.TabCodeExistsAsync(newCode))
            {
                newCode = $"{baseCode}_{DateTime.UtcNow:yyyyMMddHHmmssfff}_{counter}";
                counter++;
                if (counter > 100) // حماية من الحلقة اللانهائية
                {
                    throw new InvalidOperationException($"Unable to generate unique tab code for: {originalTabCode}");
                }
            }
            
            return newCode;
        }
    }
}
