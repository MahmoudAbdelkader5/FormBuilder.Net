using System;
using System.Collections.Generic;

namespace FormBuilder.Application.DTOs.ApprovalWorkflow
{
    // DTO للـ Stage داخل الـ Workflow
   

    // DTO للـ Workflow لعمليات القراءة
    public class ApprovalWorkflowDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; } = string.Empty; // للعرض
        public bool IsActive { get; set; }
        public List<ApprovalStageDto> Stages { get; set; } = new();
    }

    // DTO لإنشاء Workflow جديد
    public class ApprovalWorkflowCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public int DocumentTypeId { get; set; }
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// When true, the backend will auto-create a default first stage (StageOrder=1) for the new workflow.
        /// Set to false to prevent creating any default stage.
        /// </summary>
        public bool CreateDefaultStage { get; set; } = false;
    }

    // DTO لتحديث Workflow موجود
    public class ApprovalWorkflowUpdateDto
    {
        public string? Name { get; set; }
        public int? DocumentTypeId { get; set; }
        public bool? IsActive { get; set; }
    }
}
