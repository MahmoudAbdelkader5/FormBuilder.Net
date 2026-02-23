namespace FormBuilder.Core.DTOS.FormRules
{
    /// <summary>
    /// Result DTO for CopyToDocument action execution
    /// </summary>
    public class CopyToDocumentResultDto
    {
        /// <summary>
        /// Whether the copy operation was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Target document ID (if created/updated successfully)
        /// </summary>
        public int? TargetDocumentId { get; set; }

        /// <summary>
        /// Target document number (if created successfully)
        /// </summary>
        public string? TargetDocumentNumber { get; set; }

        /// <summary>
        /// Error message (if failed)
        /// </summary>
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
        /// Action ID that triggered this copy
        /// </summary>
        public int? ActionId { get; set; }

        /// <summary>
        /// Source submission ID
        /// </summary>
        public int SourceSubmissionId { get; set; }
    }
}

