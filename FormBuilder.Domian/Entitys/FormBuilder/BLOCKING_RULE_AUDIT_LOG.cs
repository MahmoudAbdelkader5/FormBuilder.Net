using formBuilder.Domian.Entitys;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormBuilder.Domian.Entitys.FormBuilder
{
    /// <summary>
    /// Audit log for blocking rule evaluations
    /// </summary>
    [Table("BLOCKING_RULE_AUDIT_LOG")]
    public class BLOCKING_RULE_AUDIT_LOG : BaseEntity
    {
        /// <summary>
        /// Form Builder ID
        /// </summary>
        [Required]
        public int FormBuilderId { get; set; }

        /// <summary>
        /// Submission ID (nullable for Pre-Open rules)
        /// </summary>
        public int? SubmissionId { get; set; }

        /// <summary>
        /// Evaluation Phase: PreOpen or PreSubmit
        /// </summary>
        [Required, StringLength(20)]
        public string EvaluationPhase { get; set; } = string.Empty;

        /// <summary>
        /// ID of the rule that caused the block (if any)
        /// </summary>
        public int? RuleId { get; set; }

        /// <summary>
        /// Name of the rule that caused the block
        /// </summary>
        [StringLength(200)]
        public string? RuleName { get; set; }

        /// <summary>
        /// Whether the action was blocked
        /// </summary>
        [Required]
        public bool IsBlocked { get; set; }

        /// <summary>
        /// Block message displayed to user
        /// </summary>
        [StringLength(1000)]
        public string? BlockMessage { get; set; }

        /// <summary>
        /// User ID who attempted the action (if available)
        /// </summary>
        [StringLength(450)]
        public string? UserId { get; set; }

        /// <summary>
        /// Additional context or metadata (JSON)
        /// </summary>
        public string? ContextJson { get; set; }
    }
}

