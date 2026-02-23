using System.ComponentModel.DataAnnotations;

namespace FormBuilder.API.Models.DTOs
{
    public class EmailTemplateDto
    {
        public int Id { get; set; }
        public int DocumentTypeId { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string TemplateCode { get; set; } = string.Empty;
        public string SubjectTemplate { get; set; } = string.Empty;
        public string BodyTemplateHtml { get; set; } = string.Empty;
        public int SmtpConfigId { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateEmailTemplateDto
    {
        [Range(1, int.MaxValue)]
        public int DocumentTypeId { get; set; }

        [Required, StringLength(200)]
        public string TemplateName { get; set; } = string.Empty;

        /// <summary>
        /// Code used by system (e.g. SubmissionConfirmation, ApprovalRequired, ApprovalResult)
        /// </summary>
        [Required, StringLength(100)]
        public string TemplateCode { get; set; } = string.Empty;

        [Required]
        public string SubjectTemplate { get; set; } = string.Empty;

        [Required]
        public string BodyTemplateHtml { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int SmtpConfigId { get; set; }

        public bool IsDefault { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }

    public class UpdateEmailTemplateDto
    {
        public int? DocumentTypeId { get; set; }

        [StringLength(200)]
        public string? TemplateName { get; set; }

        [StringLength(100)]
        public string? TemplateCode { get; set; }

        public string? SubjectTemplate { get; set; }
        public string? BodyTemplateHtml { get; set; }

        public int? SmtpConfigId { get; set; }

        public bool? IsDefault { get; set; }
        public bool? IsActive { get; set; }
    }
}


