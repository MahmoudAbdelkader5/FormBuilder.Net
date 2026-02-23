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

    [Table("CRYSTAL_LAYOUTS")]
    public class CRYSTAL_LAYOUTS : BaseEntity
    {
       

        [ForeignKey("DOCUMENT_TYPES")]
        public int DocumentTypeId { get; set; }
        public virtual DOCUMENT_TYPES DOCUMENT_TYPES { get; set; }

        [Required, StringLength(200)]
        public string LayoutName { get; set; }

        [Required, StringLength(500)]
        public string LayoutPath { get; set; }

        public bool IsDefault { get; set; }
        public new bool IsActive { get; set; }
    }
}
