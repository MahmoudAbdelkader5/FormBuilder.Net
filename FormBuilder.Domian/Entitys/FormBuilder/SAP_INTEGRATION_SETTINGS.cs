using formBuilder.Domian.Entitys;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormBuilder.Domian.Entitys.FromBuilder
{
    [Table("SAP_INTEGRATION_SETTINGS")]
    public class SAP_INTEGRATION_SETTINGS : BaseEntity
    {
        [Required]
        public int DocumentTypeId { get; set; }

        [Required]
        public int SapConfigId { get; set; }

        [Required, StringLength(200)]
        public string TargetEndpoint { get; set; } = string.Empty;

        [Required, StringLength(10)]
        public string HttpMethod { get; set; } = "POST";

        [StringLength(200)]
        public string? TargetObject { get; set; }

        // OnSubmit | OnFinalApproval | OnSpecificWorkflowStage
        [Required, StringLength(50)]
        public string ExecutionMode { get; set; } = "OnSubmit";

        public int? TriggerStageId { get; set; }

        public bool BlockWorkflowOnError { get; set; } = false;
    }
}
