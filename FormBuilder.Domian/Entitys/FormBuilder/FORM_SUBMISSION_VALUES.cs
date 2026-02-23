using formBuilder.Domian.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Domian.Entitys.froms
{

    [Table("FORM_SUBMISSION_VALUES")]
    public class FORM_SUBMISSION_VALUES : BaseEntity
    {
       

        [ForeignKey("FORM_SUBMISSIONS")]
        public int SubmissionId { get; set; }
        public virtual FORM_SUBMISSIONS FORM_SUBMISSIONS { get; set; }

        [ForeignKey("FORM_FIELDS")]
        public int FieldId { get; set; }
        public virtual FORM_FIELDS FORM_FIELDS { get; set; }

        [Required, StringLength(100)]
        public string FieldCode { get; set; }

        public string ValueString { get; set; }
        public decimal? ValueNumber { get; set; }
        public DateTime? ValueDate { get; set; }
        public bool? ValueBool { get; set; }
        public string ValueJson { get; set; }
    }
}
