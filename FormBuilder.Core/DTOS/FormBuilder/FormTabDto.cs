using System;
using System.Collections.Generic;
using FormBuilder.API.Models;
using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.DTOS.FormTabs
{
    public class FormTabDto
    {
        public int Id { get; set; }
        public int FormBuilderId { get; set; }
        public string TabName { get; set; }
        public string? ForeignTabName { get; set; }
        public string TabCode { get; set; }
        public int TabOrder { get; set; }
        public bool IsActive { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }

        // Computed properties for task requirements (name_ar / name_en pattern)
        public string name_en => TabName;
        public string? name_ar => ForeignTabName;
        public int order => TabOrder;
        public bool is_active => IsActive;

        // Nested fields for public form rendering
        public List<FormFieldDto> Fields { get; set; } = new List<FormFieldDto>();
    }

    public class UpdateFormTabDto
    {
        [Required]
        [StringLength(100)]
        public string TabName { get; set; }

        [StringLength(100)]
        public string? ForeignTabName { get; set; }

        [Required]
        [StringLength(50)]
        public string TabCode { get; set; }

        public int TabOrder { get; set; }

        public bool IsActive { get; set; }
    }

    public class CreateFormTabDto
    {
        [Required]
        public int FormBuilderId { get; set; }

        [Required]
        [StringLength(100)]
        public string TabName { get; set; }

        [StringLength(100)]
        public string? ForeignTabName { get; set; }

        [Required]
        [StringLength(50)]
        public string TabCode { get; set; }

        public int TabOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public string? CreatedByUserId { get; set; }
    }
}
