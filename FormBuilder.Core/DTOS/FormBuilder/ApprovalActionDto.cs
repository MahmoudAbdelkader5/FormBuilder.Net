namespace FormBuilder.Application.DTOs.ApprovalWorkflow
{
    // ==========================
    // DTO لتنفيذ إجراءات الموافقة
    // ==========================
    public class ApprovalActionDto
    {
        public int SubmissionId { get; set; }
        public int StageId { get; set; }
        public string ActionType { get; set; } // Approved, Rejected, Returned
        public string ActionByUserId { get; set; }
        // Optional approval comment
        public string? Comments { get; set; }
    }

    public class RequestStageSignatureDto
    {
        public int SubmissionId { get; set; }
        public int StageId { get; set; }
        public string? RequestedByUserId { get; set; }
    }

    // ==========================
    // DTO للحصول على المستندات في صندوق الوارد للموافقة
    // ==========================
    public class ApprovalInboxDto
    {
        public int SubmissionId { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentTypeName { get; set; }
        public int StageId { get; set; }
        public string StageName { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string SubmittedByUserId { get; set; }
        public string SubmittedByUserName { get; set; }
        public string Status { get; set; }
        public bool IsAssigned { get; set; }
        public string[] Approvers { get; set; }
        public int StageOrder { get; set; }
        public bool CanApprove { get; set; } // Indicates if the current user can approve/reject this item
    }

    // ==========================
    // DTO لحل المستخدمين من الأدوار
    // ==========================
    public class ResolvedApproversDto
    {
        public int StageId { get; set; }
        public string StageName { get; set; }
        public string[] UserIds { get; set; }
        public string[] UserNames { get; set; }
    }
}
