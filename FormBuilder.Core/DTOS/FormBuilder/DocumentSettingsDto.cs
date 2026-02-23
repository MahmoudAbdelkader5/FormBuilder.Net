using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.DTOS.FormBuilder
{
    /// <summary>
    /// DTO for Document Settings (Document Type + Series) within Form Builder context
    /// </summary>
    public class DocumentSettingsDto
    {
        public int FormBuilderId { get; set; }
        public string FormBuilderName { get; set; }

        // Document Type Information
        public int? DocumentTypeId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentCode { get; set; }
        public string MenuCaption { get; set; }
        public int MenuOrder { get; set; }
        public int? ParentMenuId { get; set; }
        public bool IsActive { get; set; }

        // Document Series List
        public List<DocumentSeriesDto> DocumentSeries { get; set; } = new List<DocumentSeriesDto>();
    }

    /// <summary>
    /// DTO for saving Document Settings (Document Type + Series) for a Form Builder
    /// </summary>
    public class SaveDocumentSettingsDto
    {
        [Required]
        public int FormBuilderId { get; set; }

        // Document Type Fields
        [Required, StringLength(200)]
        public string DocumentName { get; set; }

        [Required, StringLength(100)]
        public string DocumentCode { get; set; }

        [Required, StringLength(200)]
        public string MenuCaption { get; set; }

        public int MenuOrder { get; set; } = 0;

        public int? ParentMenuId { get; set; }

        public bool IsActive { get; set; } = true;

        // Document Series List (optional - can be managed separately)
        public List<SaveDocumentSeriesDto> DocumentSeries { get; set; } = new List<SaveDocumentSeriesDto>();
    }

    /// <summary>
    /// DTO for saving a single Document Series
    /// </summary>
    public class SaveDocumentSeriesDto
    {
        public int? Id { get; set; } // If provided, update existing; otherwise create new

        [Required]
        public int ProjectId { get; set; }

        [Required, StringLength(50)]
        public string SeriesCode { get; set; }

        [StringLength(200)]
        public string? SeriesName { get; set; }

        [StringLength(500)]
        public string? Template { get; set; }

        public int SequenceStart { get; set; } = 1;

        public int SequencePadding { get; set; } = 3;

        [StringLength(20)]
        public string ResetPolicy { get; set; } = "None";

        [StringLength(20)]
        public string GenerateOn { get; set; } = "Submit";

        public int NextNumber { get; set; } = 1;

        public bool IsDefault { get; set; } = false;

        public bool IsActive { get; set; } = true;
    }
}
