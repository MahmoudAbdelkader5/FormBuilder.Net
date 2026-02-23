using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.DTOS.FormBuilder
{
    public class CreateFormBuilderDto
    {
        [Required]
        [StringLength(200)]
        public string FormName { get; set; } = string.Empty;

        [StringLength(200)]
        public string? ForeignFormName { get; set; }

        [Required]
        [StringLength(100)]
        public string FormCode { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? ForeignDescription { get; set; }

        public bool IsPublished { get; set; } = false;

        public bool? IsSapEnabled { get; set; } = false;

        [StringLength(50)]
        public string? SapExecutionMode { get; set; } = "OnSubmit";

        public bool IsActive { get; set; } = true;

        // optional: will be set from authenticated user in controller
        public string? CreatedByUserId { get; set; }
    }
}
