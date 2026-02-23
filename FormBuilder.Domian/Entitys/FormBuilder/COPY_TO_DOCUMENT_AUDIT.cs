using formBuilder.Domian.Entitys;
using FormBuilder.Domian.Entitys.froms;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormBuilder.Domian.Entitys.FormBuilder
{
    /// <summary>
    /// Audit table for CopyToDocument action executions
    /// Records each execution of CopyToDocument action for traceability
    /// </summary>
    [Table("COPY_TO_DOCUMENT_AUDIT")]
    public class COPY_TO_DOCUMENT_AUDIT : BaseEntity
    {
        /// <summary>
        /// Target Document ID (the created/updated document)
        /// </summary>
        public int? TargetDocumentId { get; set; }
        public virtual FORM_SUBMISSIONS? TargetDocument { get; set; }

        /// <summary>
        /// Action ID that triggered this copy (from FORM_RULE_ACTIONS)
        /// </summary>
        public int? ActionId { get; set; }
        public virtual FORM_RULE_ACTIONS? Action { get; set; }

        /// <summary>
        /// Rule ID that contains the action (from FORM_RULES)
        /// </summary>
        public int? RuleId { get; set; }
        public virtual FORM_RULES? Rule { get; set; }

        /// <summary>
        /// Source Form Builder ID
        /// </summary>
        [Required]
        public int SourceFormId { get; set; }

        /// <summary>
        /// Target Form Builder ID
        /// </summary>
        [Required]
        public int TargetFormId { get; set; }

        /// <summary>
        /// Target Document Type ID
        /// </summary>
        [Required]
        public int TargetDocumentTypeId { get; set; }

        /// <summary>
        /// Whether operation was successful
        /// </summary>
        [Required]
        public bool Success { get; set; }

        /// <summary>
        /// Error message if operation failed
        /// </summary>
        [StringLength(2000)]
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Number of fields copied
        /// </summary>
        public int FieldsCopied { get; set; }

        /// <summary>
        /// Number of grid rows copied
        /// </summary>
        public int GridRowsCopied { get; set; }

        /// <summary>
        /// Target document number (if created successfully)
        /// </summary>
        [StringLength(100)]
        public string? TargetDocumentNumber { get; set; }

        /// <summary>
        /// Execution timestamp (same as CreatedDate, but kept for clarity)
        /// </summary>
        [Required]
        public DateTime ExecutionDate { get; set; } = DateTime.UtcNow;
    }
}

