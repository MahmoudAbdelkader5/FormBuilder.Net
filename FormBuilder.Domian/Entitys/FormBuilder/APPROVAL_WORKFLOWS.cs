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
    [Table("APPROVAL_WORKFLOWS")]
    public class APPROVAL_WORKFLOWS : BaseEntity
    {
       

        [ForeignKey("DOCUMENT_TYPES")]
        public int DocumentTypeId { get; set; }
        public virtual DOCUMENT_TYPES DOCUMENT_TYPES { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; }

        public new bool IsActive { get; set; }

        public virtual ICollection<APPROVAL_STAGES> APPROVAL_STAGES { get; set; }
    }


}
