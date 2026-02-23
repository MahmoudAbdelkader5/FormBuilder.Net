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
    [Table("SAP_OBJECT_MAPPINGS")]
    public class SAP_OBJECT_MAPPINGS : BaseEntity
    {
       
        [ForeignKey("DOCUMENT_TYPES")]
        public int DocumentTypeId { get; set; }
        public virtual DOCUMENT_TYPES DOCUMENT_TYPES { get; set; }

        [Required, StringLength(200)]
        public string SapObjectName { get; set; }

        public bool IsDraftOnly { get; set; }
        public new bool IsActive { get; set; }
    }
}