using System;

namespace FormBuilder.Application.DTOs.ApprovalWorkflow
{
    // ==========================
    // DTO لعرض البيانات
    // ==========================
    public class DocumentApprovalHistoryDto
    {
        public int Id { get; set; }
        public int SubmissionId { get; set; }
        public string DocumentNumber { get; set; }
        public string FormName { get; set; }
        public string DocumentTypeName { get; set; }
        public string SubmissionStatus { get; set; }
        public int StageId { get; set; }
        public string StageName { get; set; }
        public string ActionType { get; set; }
        public string ActionByUserId { get; set; }
        public string ActionByUserName { get; set; }
        public DateTime ActionDate { get; set; }
        public string Comments { get; set; }
    }

    // ==========================
    // DTO لإنشاء History record جديد
    // ==========================
    public class DocumentApprovalHistoryCreateDto
    {
        public int SubmissionId { get; set; }
        public int StageId { get; set; }
        public string ActionType { get; set; } // Approved, Rejected, Returned, Approved (Delegated)
        public string ActionByUserId { get; set; }
        public string Comments { get; set; }
    }
}

