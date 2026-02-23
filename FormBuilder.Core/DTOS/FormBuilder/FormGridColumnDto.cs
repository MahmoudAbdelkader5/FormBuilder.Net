using System;
using System.ComponentModel.DataAnnotations;

namespace FormBuilder.API.DTOs
{
    public class FormGridColumnDto
    {
        public int Id { get; set; }
        public int GridId { get; set; }
        public string GridName { get; set; } = string.Empty;
        public string FormBuilderName { get; set; } = string.Empty;
        public int FieldTypeId { get; set; }
        public string FieldTypeName { get; set; } = string.Empty;
        public string ColumnName { get; set; } = string.Empty;
        public string ColumnCode { get; set; } = string.Empty;
        public int ColumnOrder { get; set; }
        public bool IsMandatory { get; set; }
        public string DataType { get; set; } = string.Empty;
        public int? MaxLength { get; set; }
        public string? DefaultValueJson { get; set; }
        public string? ValidationRuleJson { get; set; }
        public bool IsActive { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsVisible { get; set; }
        public string? VisibilityRuleJson { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class CreateFormGridColumnDto
    {
        [Required(ErrorMessage = "GridId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "GridId must be greater than 0")]
        public int GridId { get; set; }

        [Required(ErrorMessage = "FieldTypeId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "FieldTypeId must be greater than 0")]
        public int FieldTypeId { get; set; }

        [Required(ErrorMessage = "ColumnName is required")]
        [StringLength(200, ErrorMessage = "ColumnName cannot exceed 200 characters")]
        public string ColumnName { get; set; } = string.Empty;

        [Required(ErrorMessage = "ColumnCode is required")]
        [StringLength(100, ErrorMessage = "ColumnCode cannot exceed 100 characters")]
        public string ColumnCode { get; set; } = string.Empty;

        public int? ColumnOrder { get; set; }
        public bool IsMandatory { get; set; }

        [Required(ErrorMessage = "DataType is required")]
        public string DataType { get; set; } = string.Empty;

        public int? MaxLength { get; set; }
        public string? DefaultValueJson { get; set; }
        public string? ValidationRuleJson { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsReadOnly { get; set; } = false;
        public bool IsVisible { get; set; } = true;
        public string? VisibilityRuleJson { get; set; }
    }

    public class UpdateFormGridColumnDto
    {
        /// <summary>
        /// GridId cannot be changed after creation. This property is ignored during updates.
        /// </summary>
        public int? GridId { get; set; }
        public int? FieldTypeId { get; set; }
        public string? ColumnName { get; set; }
        public string? ColumnCode { get; set; }
        public int? ColumnOrder { get; set; }
        public bool? IsMandatory { get; set; }
        public string? DataType { get; set; }
        public int? MaxLength { get; set; }
        public string? DefaultValueJson { get; set; }
        public string? ValidationRuleJson { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsReadOnly { get; set; }
        public bool? IsVisible { get; set; }
        public string? VisibilityRuleJson { get; set; }
    }
}