using System;
using System.ComponentModel.DataAnnotations;

namespace FormBuilder.API.DTOs
{
    public class FieldTypeDto
    {
        public int Id { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public int? MaxLength { get; set; }
        public bool HasOptions { get; set; }
        public bool AllowMultiple { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class CreateFieldTypeDto
    {
        [Required(ErrorMessage = "TypeName is required")]
        [StringLength(100, ErrorMessage = "TypeName cannot exceed 100 characters")]
        public string TypeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "DataType is required")]
        [StringLength(50, ErrorMessage = "DataType cannot exceed 50 characters")]
        public string DataType { get; set; } = string.Empty;

        public int? MaxLength { get; set; }

        public bool HasOptions { get; set; } = false;

        public bool AllowMultiple { get; set; } = false;

        public bool IsActive { get; set; } = true;
    }

    public class UpdateFieldTypeDto
    {
        [StringLength(100, ErrorMessage = "TypeName cannot exceed 100 characters")]
        public string? TypeName { get; set; }

        [StringLength(50, ErrorMessage = "DataType cannot exceed 50 characters")]
        public string? DataType { get; set; }

        public int? MaxLength { get; set; }

        public bool? HasOptions { get; set; }

        public bool? AllowMultiple { get; set; }

        public bool? IsActive { get; set; }
    }
}

