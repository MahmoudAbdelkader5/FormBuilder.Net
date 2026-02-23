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
    [Table("FORMULA_VARIABLES")]
    public class FORMULA_VARIABLES : BaseEntity
    {
   
        [ForeignKey("FORMULAS")]
        public int FormulaId { get; set; }
        public virtual FORMULAS FORMULAS { get; set; }

        [Required, StringLength(100)]
        public string VariableName { get; set; }

        [ForeignKey("FORM_FIELDS")]
        public int SourceFieldId { get; set; }
        public virtual FORM_FIELDS FORM_FIELDS { get; set; }
    }

}
