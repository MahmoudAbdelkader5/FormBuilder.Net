using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.DTOS.FormRules
{
    /// <summary>
    /// Configuration DTO for CopyToDocument action
    /// This is stored in ActionDataDto.Value as JSON string
    /// </summary>
    public class CopyToDocumentActionDto
    {
        /// <summary>
        /// Source Document Type ID (required - explicitly defines source document type)
        /// </summary>
        [Required(ErrorMessage = "SourceDocumentTypeId is required")]
        public int SourceDocumentTypeId { get; set; }

        /// <summary>
        /// Source Form Builder ID (required - explicitly defines source form)
        /// </summary>
        [Required(ErrorMessage = "SourceFormId is required")]
        public int SourceFormId { get; set; }

        /// <summary>
        /// Source Submission ID (optional - defaults to current submission)
        /// If not provided, uses the submission that triggered the action
        /// </summary>
        public int? SourceSubmissionId { get; set; }

        /// <summary>
        /// Target Document Type ID (required)
        /// </summary>
        [Required(ErrorMessage = "TargetDocumentTypeId is required")]
        public int TargetDocumentTypeId { get; set; }

        /// <summary>
        /// Target Form Builder ID (required)
        /// </summary>
        [Required(ErrorMessage = "TargetFormId is required")]
        public int TargetFormId { get; set; }

        /// <summary>
        /// Create new document if true, update existing if false
        /// </summary>
        public bool CreateNewDocument { get; set; } = true;

        /// <summary>
        /// Initial status for new target document (Draft / Submitted)
        /// Default: Draft
        /// </summary>
        public string InitialStatus { get; set; } = "Draft";

        /// <summary>
        /// Target document ID to update when CreateNewDocument is false
        /// </summary>
        public int? TargetDocumentId { get; set; }

        /// <summary>
        /// Field mapping: SourceFieldCode -> TargetFieldCode
        /// Example: {"TOTAL_AMOUNT": "CONTRACT_VALUE", "CUSTOMER_NAME": "PARTY_NAME"}
        /// </summary>
        public Dictionary<string, string> FieldMapping { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Grid mapping: SourceGridCode -> TargetGridCode
        /// Example: {"ITEMS": "CONTRACT_ITEMS"}
        /// </summary>
        public Dictionary<string, string> GridMapping { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Copy calculated fields (Yes/No)
        /// </summary>
        public bool CopyCalculatedFields { get; set; } = true;

        /// <summary>
        /// Copy grid rows (Yes/No)
        /// </summary>
        public bool CopyGridRows { get; set; } = true;

        /// <summary>
        /// Start workflow for target document (Yes/No)
        /// </summary>
        public bool StartWorkflow { get; set; } = false;

        /// <summary>
        /// Link source and target documents (set ParentDocumentId)
        /// </summary>
        public bool LinkDocuments { get; set; } = true;

        /// <summary>
        /// Copy metadata (submission date, document number, etc.)
        /// </summary>
        public bool CopyMetadata { get; set; } = false;

        /// <summary>
        /// Copy attachments (Yes/No)
        /// </summary>
        public bool CopyAttachments { get; set; } = false;

        /// <summary>
        /// Override target default values with source values (Yes/No)
        /// If true, source values overwrite defaults. If false, defaults are preserved if source is empty.
        /// </summary>
        public bool OverrideTargetDefaults { get; set; } = false;

        /// <summary>
        /// Metadata fields to copy (if CopyMetadata = true)
        /// Example: ["DocumentNumber", "SubmittedDate", "SubmittedByUserId"]
        /// </summary>
        public List<string> MetadataFields { get; set; } = new List<string>();
    }
}

