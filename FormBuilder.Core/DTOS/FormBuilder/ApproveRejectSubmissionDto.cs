using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.DTOS.FormBuilder
{
    // ==========================
    // DTO للموافقة أو الرفض على Submission
    // ==========================
    public class ApproveSubmissionDto
    {
        [Required]
        public int SubmissionId { get; set; }

        [Required]
        public int StageId { get; set; }

        [Required]
        public string ActionByUserId { get; set; } = string.Empty;

        public string? Comments { get; set; }
    }

    public class RejectSubmissionDto
    {
        [Required]
        public int SubmissionId { get; set; }

        [Required]
        public int StageId { get; set; }

        [Required]
        public string ActionByUserId { get; set; } = string.Empty;

        public string? Comments { get; set; }
    }
}

