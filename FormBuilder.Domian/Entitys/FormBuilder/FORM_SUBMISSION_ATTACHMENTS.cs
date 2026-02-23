using formBuilder.Domian.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Domian.Entitys.FromBuilder
{

    [Table("FORM_SUBMISSION_ATTACHMENTS")]
    public class FORM_SUBMISSION_ATTACHMENTS : BaseEntity
    {
       

        [ForeignKey("FORM_SUBMISSIONS")]
        public int SubmissionId { get; set; }
        public virtual FORM_SUBMISSIONS FORM_SUBMISSIONS { get; set; }

        [ForeignKey("FORM_FIELDS")]
        public int FieldId { get; set; }
        public virtual FORM_FIELDS FORM_FIELDS { get; set; }

        [Required, StringLength(260)]
        public string FileName { get; set; }

        [Required, StringLength(500)]
        public string FilePath { get; set; }

        public long FileSize { get; set; }

        [Required, StringLength(100)]
        public string ContentType { get; set; }

        public DateTime UploadedDate { get; set; }
    }
}
