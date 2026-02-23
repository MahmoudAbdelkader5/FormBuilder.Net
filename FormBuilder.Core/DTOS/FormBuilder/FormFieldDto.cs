using FormBuilder.Core.DTOS.FormFields;
using FormBuilder.Core.DTOS.FormTabs;
using FormBuilder.API.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FormBuilder.API.Models
{
    public class FormFieldDto
    {
        public int Id { get; set; }

        [Required]
        public int TabId { get; set; }

        public int? FieldTypeId { get; set; }
        public string? FieldTypeName { get; set; }

        [Required, StringLength(200)]
        public string FieldName { get; set; } = string.Empty;

        [StringLength(200)]
        public string? ForeignFieldName { get; set; }

        [Required, StringLength(100)]
        public string FieldCode { get; set; } = string.Empty;

        [Required]
        public int FieldOrder { get; set; }

        public string? Placeholder { get; set; }
        public string? ForeignPlaceholder { get; set; }
        public string HintText { get; set; }
        public string? ForeignHintText { get; set; }

        [Required]
        public bool ?IsMandatory { get; set; }

        [Required]
        public bool ?IsEditable { get; set; }

        [Required]
        public bool ?IsVisible { get; set; }

        public string? DefaultValueJson { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
        public string? RegexPattern { get; set; }
        public string? ValidationMessage { get; set; }
        public string? ForeignValidationMessage { get; set; }

        // Calculation Fields Properties
        public string? ExpressionText { get; set; }
        public string? CalculationMode { get; set; }
        public string? RecalculateOn { get; set; }
        public string? ResultType { get; set; }

        public DateTime CreatedDate { get; set; }
        public string? CreatedByUserId { get; set; }
        public string? CreatedByUserName { get; set; }

        [Required]
        public bool IsActive { get; set; }

        // Computed properties for task requirements (label_ar / label_en pattern)
        public string label_en => FieldName;
        public string? label_ar => ForeignFieldName;
        public string? placeholder_en => Placeholder;
        public string? placeholder_ar => ForeignPlaceholder;
        public string? type => FieldTypeName;
        public bool is_required => IsMandatory ?? false;
        public int tab_id => TabId;

        // Navigation properties as DTOs
        // Ignored in JSON to prevent circular reference with FormTabDto.Fields (helps Swagger schema generation)
        [JsonIgnore]
        public FormTabDto? Tab { get; set; }
        public List<FieldOptionDto> FieldOptions { get; set; } = new List<FieldOptionDto>();
        
        // Field Data Source - tells frontend where to load options from
        public FieldDataSourceDto? FieldDataSource { get; set; }
        
        // Grid support
        public int? GridId { get; set; }
        public FormGridDto? Grid { get; set; }
    }

    public class CreateFormFieldDto
    {
        [Required]
        public int TabId { get; set; }

        public int? FieldTypeId { get; set; }

        [Required, StringLength(200)]
        public string FieldName { get; set; } = string.Empty;

        [StringLength(200)]
        public string? ForeignFieldName { get; set; }

        [Required, StringLength(100)]
        public string FieldCode { get; set; } = string.Empty;

        [Required]
        public int FieldOrder { get; set; }

        public string? Placeholder { get; set; }
        public string? ForeignPlaceholder { get; set; }
        public string HintText { get; set; }
        public string? ForeignHintText { get; set; }

        public bool ?IsMandatory { get; set; } = true;
        public bool ?IsEditable { get; set; } = true;
        public bool ?IsVisible { get; set; } = true;

        public string? DefaultValueJson { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
        public string? RegexPattern { get; set; }
        public string? ValidationMessage { get; set; }
        public string? ForeignValidationMessage { get; set; }

        // Calculation Fields Properties
        public string? ExpressionText { get; set; }
        public string? CalculationMode { get; set; }
        public string? RecalculateOn { get; set; }
        public string? ResultType { get; set; }

        public string? CreatedByUserId { get; set; }
        
        // Grid support
        public int? GridId { get; set; }
    }

    public class UpdateFormFieldDto
    {
        [Required]
        public int TabId { get; set; }

        public int? FieldTypeId { get; set; }

        [Required, StringLength(200)]
        public string FieldName { get; set; } = string.Empty;

        [StringLength(200)]
        public string? ForeignFieldName { get; set; }

        [Required, StringLength(100)]
        public string FieldCode { get; set; } = string.Empty;

        [Required]
        public int FieldOrder { get; set; }

        public string? Placeholder { get; set; }
        public string? ForeignPlaceholder { get; set; }
        public string HintText { get; set; }
        public string? ForeignHintText { get; set; }

        public bool? IsMandatory { get; set; }
        public bool? IsEditable { get; set; }
        public bool? IsVisible { get; set; }

        public string? DefaultValueJson { get; set; }
        public int? MaxLength { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
        public string? RegexPattern { get; set; }
        public string? ValidationMessage { get; set; }
        public string? ForeignValidationMessage { get; set; }

        // Calculation Fields Properties
        public string? ExpressionText { get; set; }
        public string? CalculationMode { get; set; }
        public string? RecalculateOn { get; set; }
        public string? ResultType { get; set; }
        
        // Grid support
        public int? GridId { get; set; }
    }
}