using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.DTOS.FormFields
{
    public class CreateFormFieldDto
    {
        [Required(ErrorMessage = "TabId is required")]
        public int TabId { get; set; }

        public int? FieldTypeId { get; set; }

        [Required(ErrorMessage = "FieldName is required")]
        [StringLength(200, ErrorMessage = "FieldName cannot exceed 200 characters")]
        public string FieldName { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "ForeignFieldName cannot exceed 200 characters")]
        public string? ForeignFieldName { get; set; }

        [Required(ErrorMessage = "FieldCode is required")]
        [StringLength(100, ErrorMessage = "FieldCode cannot exceed 100 characters")]
        public string FieldCode { get; set; } = string.Empty;

        public int FieldOrder { get; set; } = 0;

        [StringLength(200, ErrorMessage = "Placeholder cannot exceed 200 characters")]
        public string? Placeholder { get; set; }

        [StringLength(200, ErrorMessage = "ForeignPlaceholder cannot exceed 200 characters")]
        public string? ForeignPlaceholder { get; set; }

        [StringLength(500, ErrorMessage = "HintText cannot exceed 500 characters")]
        public string? HintText { get; set; }

        [StringLength(500, ErrorMessage = "ForeignHintText cannot exceed 500 characters")]
        public string? ForeignHintText { get; set; }

        public bool IsMandatory { get; set; } = false;

        public bool IsEditable { get; set; } = true;

        public bool IsVisible { get; set; } = true;

        public string? DefaultValueJson { get; set; }

        public int? MaxLength { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "MinValue must be a positive number")]
        public decimal? MinValue { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "MaxValue must be a positive number")]
        public decimal? MaxValue { get; set; }

        [StringLength(500, ErrorMessage = "RegexPattern cannot exceed 500 characters")]
        public string? RegexPattern { get; set; }

        [StringLength(500, ErrorMessage = "ValidationMessage cannot exceed 500 characters")]
        public string? ValidationMessage { get; set; }

        [StringLength(500, ErrorMessage = "ForeignValidationMessage cannot exceed 500 characters")]
        public string? ForeignValidationMessage { get; set; }

        public bool IsActive { get; set; } = true;

        // خصائص إضافية للتحكم في السلوك
        public bool ShowInList { get; set; } = true;

        public bool IsSearchable { get; set; } = false;

        public string? CssClass { get; set; }

        public string? Style { get; set; }

        // خيارات الحقل (لـ Dropdown, Radio, Checkbox)
        public List<FieldOptionDto> FieldOptions { get; set; } = new List<FieldOptionDto>();

        // Grid support (for Grid field type)
        public int? GridId { get; set; }

        // Calculation Fields Properties
        [StringLength(int.MaxValue, ErrorMessage = "ExpressionText cannot exceed maximum length")]
        public string? ExpressionText { get; set; }

        [StringLength(50, ErrorMessage = "CalculationMode cannot exceed 50 characters")]
        public string? CalculationMode { get; set; }

        [StringLength(50, ErrorMessage = "RecalculateOn cannot exceed 50 characters")]
        public string? RecalculateOn { get; set; }

        [StringLength(50, ErrorMessage = "ResultType cannot exceed 50 characters")]
        public string? ResultType { get; set; }
    }

    public class FieldOptionDto
    {
        [Required(ErrorMessage = "OptionText is required")]
        [StringLength(200, ErrorMessage = "OptionText cannot exceed 200 characters")]
        public string OptionText { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "ForeignOptionText cannot exceed 200 characters")]
        public string? ForeignOptionText { get; set; }

        [StringLength(100, ErrorMessage = "OptionValue cannot exceed 100 characters")]
        public string? OptionValue { get; set; }

        public int SortOrder { get; set; } = 0;

        public bool IsDefault { get; set; } = false;

        public bool IsActive { get; set; } = true;

        // خصائص إضافية للخيارات
        public string? Description { get; set; }

        public string? IconClass { get; set; }

        public string? Color { get; set; }
    }
}