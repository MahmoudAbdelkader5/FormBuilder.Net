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

    [Table("FORM_VALIDATION_RULES")]
    public class FORM_VALIDATION_RULES : BaseEntity
    {
      

        [ForeignKey("FORM_BUILDER")]
        public int FormBuilderId { get; set; }
        public virtual FORM_BUILDER FORM_BUILDER { get; set; }

        [Required, StringLength(20)]
        public string Level { get; set; }

        [ForeignKey("FORM_FIELDS")]
        public int? FieldId { get; set; }
        public virtual FORM_FIELDS FORM_FIELDS { get; set; }

        public string ExpressionText { get; set; }

        [Required, StringLength(500)]
        public string ErrorMessage { get; set; }

        public new bool IsActive { get; set; }
    }
}
