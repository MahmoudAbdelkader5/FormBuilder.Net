using formBuilder.Domian.Entitys;
using FormBuilder.Domian.Entitys.FormBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Domian.Entitys.FromBuilder
{


    [Table("FORM_ATTACHMENT_TYPES")]
    public class FORM_ATTACHMENT_TYPES : BaseEntity
    {
   

        [ForeignKey("FORM_BUILDER")]
        public int FormBuilderId { get; set; }
        public virtual FORM_BUILDER FORM_BUILDER { get; set; }

        [ForeignKey("ATTACHMENT_TYPES")]
        public int AttachmentTypeId { get; set; }
        public virtual ATTACHMENT_TYPES ATTACHMENT_TYPES { get; set; }

        public bool IsMandatory { get; set; }
        public new bool IsActive { get; set; }
    }


}
