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
    [Table("ALERT_RULES")]
    public class ALERT_RULES : BaseEntity
    {
       

        [ForeignKey("DOCUMENT_TYPES")]
        public int DocumentTypeId { get; set; }
        public virtual DOCUMENT_TYPES DOCUMENT_TYPES { get; set; }

        [Required, StringLength(200)]
        public string RuleName { get; set; }

        [Required, StringLength(50)]
        public string TriggerType { get; set; }

        public string ConditionJson { get; set; }

        [ForeignKey("EMAIL_TEMPLATES")]
        public int? EmailTemplateId { get; set; }
        public virtual EMAIL_TEMPLATES EMAIL_TEMPLATES { get; set; }

        [Required, StringLength(20)]
        public string NotificationType { get; set; }

        [StringLength(450)]
        public string TargetRoleId { get; set; }

       
        public string TargetUserId { get; set; }

        public new bool IsActive { get; set; }
    }
}
