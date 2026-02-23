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

    [Table("SAP_FIELD_MAPPINGS")]
    public class SAP_FIELD_MAPPINGS : BaseEntity
    {
       

        [ForeignKey("FORM_FIELDS")]
        public int FormFieldId { get; set; }
        public virtual FORM_FIELDS FORM_FIELDS { get; set; }

        [Required, StringLength(200)]
        public string SapFieldName { get; set; }

        [Required, StringLength(20)]
        public string Direction { get; set; }

        public new bool IsActive { get; set; }
    }
}
