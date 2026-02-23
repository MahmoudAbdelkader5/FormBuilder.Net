using System;
using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.DTOS.FormBuilder
{
    public class AlertRuleDto
    {
        public int Id { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; }
        public string RuleName { get; set; }
        public string TriggerType { get; set; }
        public string ConditionJson { get; set; }
        public int? EmailTemplateId { get; set; }
        public string EmailTemplateName { get; set; }
        public string NotificationType { get; set; }
        public string TargetRoleId { get; set; }
        public string TargetUserId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class CreateAlertRuleDto
    {
        [Required]
        public int DocumentTypeId { get; set; }

        [Required, StringLength(200)]
        public string RuleName { get; set; }

        [Required, StringLength(50)]
        public string TriggerType { get; set; } // FormSubmitted, ApprovalRequired, ApprovalApproved, ApprovalRejected, ApprovalReturned

        public string ConditionJson { get; set; }

        public int? EmailTemplateId { get; set; }

        [Required, StringLength(20)]
        public string NotificationType { get; set; } // Email, Internal, Both

        [StringLength(450)]
        public string TargetRoleId { get; set; }

        public string TargetUserId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpdateAlertRuleDto
    {
        [StringLength(200)]
        public string RuleName { get; set; }

        [StringLength(50)]
        public string TriggerType { get; set; }

        public string ConditionJson { get; set; }

        public int? EmailTemplateId { get; set; }

        [StringLength(20)]
        public string NotificationType { get; set; }

        [StringLength(450)]
        public string TargetRoleId { get; set; }

        public string TargetUserId { get; set; }

        public bool? IsActive { get; set; }
    }
}

