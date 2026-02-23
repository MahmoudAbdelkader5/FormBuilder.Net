using System;
using System.ComponentModel.DataAnnotations;

namespace FormBuilder.API.DTOs
{
    public class GridColumnOptionDto
    {
        public int Id { get; set; }
        public int ColumnId { get; set; }
        public string ColumnName { get; set; } = string.Empty;
        public string ColumnCode { get; set; } = string.Empty;
        public string GridName { get; set; } = string.Empty;
        public string OptionText { get; set; } = string.Empty;
        public string? ForeignOptionText { get; set; }
        public string OptionValue { get; set; } = string.Empty;
        public int OptionOrder { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class CreateGridColumnOptionDto
    {
        [Required]
        public int ColumnId { get; set; }

        [Required, StringLength(200)]
        public string OptionText { get; set; } = string.Empty;

        [StringLength(200)]
        public string? ForeignOptionText { get; set; }

        [Required, StringLength(200)]
        public string OptionValue { get; set; } = string.Empty;

        [Required]
        public int OptionOrder { get; set; }

        public bool IsDefault { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }

    public class UpdateGridColumnOptionDto
    {
        [Required, StringLength(200)]
        public string OptionText { get; set; } = string.Empty;

        [StringLength(200)]
        public string? ForeignOptionText { get; set; }

        [Required, StringLength(200)]
        public string OptionValue { get; set; } = string.Empty;

        [Required]
        public int OptionOrder { get; set; }

        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }
}

