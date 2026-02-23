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
    [Table("DOCUMENT_SERIES")]
    public class DOCUMENT_SERIES : BaseEntity
    {
       

        //[ForeignKey("DOCUMENT_TYPES")]
        //public int DocumentTypeId { get; set; }
        //public virtual DOCUMENT_TYPES DOCUMENT_TYPES { get; set; }

        [ForeignKey("PROJECTS")]
        public int ProjectId { get; set; }
        public virtual PROJECTS PROJECTS { get; set; }

        [Required, StringLength(50)]
        public string SeriesCode { get; set; }

        public int NextNumber { get; set; }
        public bool IsDefault { get; set; }
        public new bool IsActive { get; set; }


        [Required, StringLength(200)]
        public string SeriesName { get; set; }

        [Required, StringLength(500)]
        public string Template { get; set; } = string.Empty;

        public int SequenceStart { get; set; } = 1;

        public int SequencePadding { get; set; } = 3;

        [Required, StringLength(20)]
        public string ResetPolicy { get; set; } = "None"; // None/Yearly/Monthly/Daily

        [Required, StringLength(20)]
        public string GenerateOn { get; set; } = "Submit"; // Submit/Approval

        public virtual ICollection<FORM_SUBMISSIONS> FORM_SUBMISSIONS { get; set; }
    }

}
