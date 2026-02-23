using System;
using System.Collections.Generic;

namespace FormBuilder.Application.DTOs.ApprovalWorkflow
{
    // ==========================
    // DTO لعرض البيانات
    // ==========================
    public class ApprovalStageDto
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public string StageName { get; set; }
        public int StageOrder { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public bool IsFinalStage { get; set; }
        public bool IsActive { get; set; }
        public int? MinimumRequiredAssignees { get; set; }
        
        /// <summary>
        /// FieldCode of the form field to validate against MinAmount and MaxAmount
        /// </summary>
        public string? AmountFieldCode { get; set; }

        /// <summary>
        /// If true, this stage requires e-signature (DocuSign integration) before approval can proceed
        /// </summary>
        public bool RequiresAdobeSign { get; set; }

        // يمكن إضافة معلومات عن الـ Workflow المرتبط
        public string WorkflowName { get; set; }
    }

    // ==========================
    // DTO لإنشاء Stage جديد
    // ==========================
    public class ApprovalStageCreateDto
    {
        public int WorkflowId { get; set; }
        public string StageName { get; set; }
        public int StageOrder { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public bool IsFinalStage { get; set; }
        public bool IsActive { get; set; } = true;
        public int? MinimumRequiredAssignees { get; set; }
        
        /// <summary>
        /// FieldCode of the form field to validate against MinAmount and MaxAmount
        /// If null, validation will apply to all numeric fields
        /// </summary>
        public string? AmountFieldCode { get; set; }

        /// <summary>
        /// If true, this stage requires e-signature (DocuSign integration) before approval can proceed
        /// </summary>
        public bool RequiresAdobeSign { get; set; } = false;
    }

    // ==========================
    // DTO لتحديث Stage موجود
    // ==========================
    public class ApprovalStageUpdateDto
    {
        public int? WorkflowId { get; set; }
        public string StageName { get; set; }
        public int? StageOrder { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public bool? IsFinalStage { get; set; }
        public bool? IsActive { get; set; }
        public int? MinimumRequiredAssignees { get; set; }
        
        /// <summary>
        /// FieldCode of the form field to validate against MinAmount and MaxAmount
        /// </summary>
        public string? AmountFieldCode { get; set; }

        /// <summary>
        /// If true, this stage requires e-signature (DocuSign integration) before approval can proceed
        /// </summary>
        public bool? RequiresAdobeSign { get; set; }
    }
}
