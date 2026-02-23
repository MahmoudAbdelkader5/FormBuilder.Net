using formBuilder.Domian.Entitys;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormBuilder.Domian.Entitys.FromBuilder
{
    [Table("SAP_INTEGRATION_LOGS")]
    public class SAP_INTEGRATION_LOGS : BaseEntity
    {
        [Required]
        public int FormId { get; set; }

        [Required]
        public int SubmissionId { get; set; }

        [Required]
        public int SapConfigId { get; set; }

        [Required, StringLength(200)]
        public string Endpoint { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string EventType { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string Status { get; set; } = "Failed";

        public string? RequestPayloadJson { get; set; }
        public string? ResponsePayloadJson { get; set; }
        public string? ErrorMessage { get; set; }

        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    }
}

