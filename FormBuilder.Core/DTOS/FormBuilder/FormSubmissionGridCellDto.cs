using System;
using System.ComponentModel.DataAnnotations;

namespace FormBuilder.API.DTOs
{
    // DTOs للـ CRUD فقط
    public class FormSubmissionGridCellDto
    {
        public int Id { get; set; }
        public int RowId { get; set; }
        public int ColumnId { get; set; }
        public string ColumnCode { get; set; } = string.Empty;
        public string ColumnName { get; set; } = string.Empty;
        public int? FieldTypeId { get; set; }
        public string FieldTypeName { get; set; } = string.Empty;
        public string DisplayValue { get; set; } = string.Empty;
        public string ValueString { get; set; }
        public decimal? ValueNumber { get; set; }
        public DateTime? ValueDate { get; set; }
        public bool? ValueBool { get; set; }
        public string ValueJson { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class CreateFormSubmissionGridCellDto
    {
        [Required]
        public int RowId { get; set; }

        [Required]
        public int ColumnId { get; set; }

        // Value fields - حسب نوع العمود
        public string ValueString { get; set; }
        public decimal? ValueNumber { get; set; }
        public DateTime? ValueDate { get; set; }
        public bool? ValueBool { get; set; }
        public string ValueJson { get; set; }
    }

    public class UpdateFormSubmissionGridCellDto
    {
        // Value fields فقط
        public string ValueString { get; set; }
        public decimal? ValueNumber { get; set; }
        public DateTime? ValueDate { get; set; }
        public bool? ValueBool { get; set; }
        public string ValueJson { get; set; }
    }
}