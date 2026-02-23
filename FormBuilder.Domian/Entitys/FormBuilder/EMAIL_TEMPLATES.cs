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

    [Table("EMAIL_TEMPLATES")]
    public class EMAIL_TEMPLATES : BaseEntity
    {
       

        [ForeignKey("DOCUMENT_TYPES")]
        public int DocumentTypeId { get; set; }
        public virtual DOCUMENT_TYPES DOCUMENT_TYPES { get; set; }

        [Required, StringLength(200)]
        public string TemplateName { get; set; }

        [Required, StringLength(100)]
        public string TemplateCode { get; set; }

        public string SubjectTemplate { get; set; }
        public string BodyTemplateHtml { get; set; }

        [ForeignKey("SMTP_CONFIGS")]
        public int SmtpConfigId { get; set; }
        public virtual SMTP_CONFIGS SMTP_CONFIGS { get; set; }

        public bool IsDefault { get; set; }
        public new bool IsActive { get; set; }

        public virtual ICollection<ALERT_RULES> ALERT_RULES { get; set; }
    }

}
