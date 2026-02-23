using System;
using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Application.DTOs.ApprovalWorkflow
{
    // ==========================
    // DTO لعرض البيانات
    // ==========================
    public class ApprovalDelegationDto
    {
        public int Id { get; set; }
        public string FromUserId { get; set; }
        public string FromUserName { get; set; }
        public string ToUserId { get; set; }
        public string ToUserName { get; set; }
        public string ScopeType { get; set; }  // "Global" / "Workflow" / "Document"
        public int? ScopeId { get; set; }      // NULL / WorkflowId / SubmissionId
        public string ScopeName { get; set; }   // اسم النطاق (للعرض)
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    // ==========================
    // DTO لإنشاء Delegation جديد
    // ==========================
    public class ApprovalDelegationCreateDto
    {
        [Required]
        public string FromUserId { get; set; }
        
        [Required]
        public string ToUserId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string ScopeType { get; set; } = "Global";  // "Global" / "Workflow" / "Document"
        
        public int? ScopeId { get; set; }  // NULL (Global) / WorkflowId / SubmissionId
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        public bool IsActive { get; set; } = true;
    }

    // ==========================
    // DTO لتحديث Delegation موجود
    // ==========================
    public class ApprovalDelegationUpdateDto
    {
        public string ToUserId { get; set; }
        public string ScopeType { get; set; }
        public int? ScopeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}

