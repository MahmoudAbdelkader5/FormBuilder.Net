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
    [Table("FORM_BUTTONS")]
    public class FORM_BUTTONS : BaseEntity
    {
      

        [ForeignKey("FORM_BUILDER")]
        public int FormBuilderId { get; set; }
        public virtual FORM_BUILDER FORM_BUILDER { get; set; }

        [Required, StringLength(200)]
        public string ButtonName { get; set; }

        [Required, StringLength(100)]
        public string ButtonCode { get; set; }

        public int ButtonOrder { get; set; }

        [StringLength(100)]
        public string Icon { get; set; }

        [Required, StringLength(50)]
        public string ActionType { get; set; }

        public string ActionConfigJson { get; set; }
        public bool IsVisibleDefault { get; set; }
        public new bool IsActive { get; set; }
    }
}
