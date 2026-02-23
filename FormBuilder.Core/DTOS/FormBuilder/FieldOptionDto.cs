using System.ComponentModel.DataAnnotations;

namespace FormBuilder.API.Models
{
    public class FieldOptionDto
    {
        public int Id { get; set; }
        public int FieldId { get; set; }
        public string OptionText { get; set; } = string.Empty;
        public string? ForeignOptionText { get; set; }
        public string OptionValue { get; set; } = string.Empty;
        public int OptionOrder { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateFieldOptionDto
    {
        [Required]
        public int FieldId { get; set; }

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

    public class UpdateFieldOptionDto
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