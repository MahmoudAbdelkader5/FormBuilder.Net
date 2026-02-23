using formBuilder.Domian.Entitys;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domian.Entitys.FromBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Domian.Entitys.FormBuilder
{
    [Table("APPROVAL_STAGES")]
    public class APPROVAL_STAGES : BaseEntity
    {
        

        [ForeignKey("APPROVAL_WORKFLOWS")]
        public int WorkflowId { get; set; }
        public virtual APPROVAL_WORKFLOWS APPROVAL_WORKFLOWS { get; set; }

        public int StageOrder { get; set; }

        [Required, StringLength(200)]
        public string StageName { get; set; }

        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public bool IsFinalStage { get; set; }
        public int? MinimumRequiredAssignees { get; set; }
        
        /// <summary>
        /// FieldCode of the form field to validate against MinAmount and MaxAmount
        /// If null, validation will apply to all numeric fields
        /// </summary>
        [StringLength(100)]
        public string? AmountFieldCode { get; set; }

        /// <summary>
        /// If true, this stage requires e-signature (DocuSign integration) before approval can proceed
        /// </summary>
        public bool RequiresAdobeSign { get; set; } = false;

        public virtual ICollection<APPROVAL_STAGE_ASSIGNEES> APPROVAL_STAGE_ASSIGNEES { get; set; }
        public virtual ICollection<DOCUMENT_APPROVAL_HISTORY> DOCUMENT_APPROVAL_HISTORY { get; set; }
    }
}
