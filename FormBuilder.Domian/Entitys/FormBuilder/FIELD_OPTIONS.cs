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
    [Table("FIELD_OPTIONS")]
    public class FIELD_OPTIONS :BaseEntity
    {
       

        [ForeignKey("FORM_FIELDS")]
        public int FieldId { get; set; }
        public virtual FORM_FIELDS FORM_FIELDS { get; set; }

        [Required, StringLength(200)]
        public string OptionText { get; set; }

        [StringLength(200)]
        public string? ForeignOptionText { get; set; }

        [Required, StringLength(200)]
        public string OptionValue { get; set; }

        public int OptionOrder { get; set; }
        public bool IsDefault { get; set; }
        public new bool IsActive { get; set; }
    }



}
