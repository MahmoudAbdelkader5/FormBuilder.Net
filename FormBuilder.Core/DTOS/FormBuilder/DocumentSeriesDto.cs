

using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.DTOS.FormBuilder
{
    public class DocumentSeriesDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string SeriesCode { get; set; }
        public string SeriesName { get; set; }
        public string Template { get; set; }
        public int SequenceStart { get; set; }
        public int SequencePadding { get; set; }
        public string ResetPolicy { get; set; }
        public string GenerateOn { get; set; }
        public int NextNumber { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateDocumentSeriesDto
    {
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

    public class UpdateDocumentSeriesDto
    {
        public int? ProjectId { get; set; }
        [StringLength(50)]
        public string SeriesCode { get; set; }
        [StringLength(200)]
        public string? SeriesName { get; set; }
        [StringLength(500)]
        public string? Template { get; set; }
        public int? SequenceStart { get; set; }
        public int? SequencePadding { get; set; }
        [StringLength(20)]
        public string? ResetPolicy { get; set; }
        [StringLength(20)]
        public string? GenerateOn { get; set; }
        public int? NextNumber { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsActive { get; set; }
    }

    public class DocumentSeriesNumberDto
    {
        public int SeriesId { get; set; }
        public string SeriesCode { get; set; }
        public int NextNumber { get; set; }
        public string FullNumber { get; set; }
    }
}
