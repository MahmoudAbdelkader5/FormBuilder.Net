using formBuilder.Domian.Entitys;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Domian.Entitys.froms;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormBuilder.Domian.Entitys.FormBuilder
{
    

    [Table("FORM_BUILDER")]
    public class FORM_BUILDER :BaseEntity
    {
       

        [Required, StringLength(200)]
        public string FormName { get; set; }

        [StringLength(200)]
        public string? ForeignFormName { get; set; }

        [Required, StringLength(100)]
        public string FormCode { get; set; }

        public string ? Description { get; set; }

        public string? ForeignDescription { get; set; }
        public int ? Version { get; set; }
        public bool IsPublished { get; set; }
        public bool? IsSapEnabled { get; set; } = false;
        [StringLength(50)]
        public string? SapExecutionMode { get; set; } = "OnSubmit";
        public new bool IsActive { get; set; }

       

        public virtual ICollection<FORM_TABS> FORM_TABS { get; set; }
        public virtual ICollection<FORM_SUBMISSIONS> FORM_SUBMISSIONS { get; set; }
        public virtual ICollection<FORM_GRIDS> FORM_GRIDS { get; set; }
        public virtual ICollection<FORM_RULES> FORM_RULES { get; set; }
        public virtual ICollection<FORM_ATTACHMENT_TYPES> FORM_ATTACHMENT_TYPES { get; set; }
        public virtual ICollection<FORMULAS> FORMULAS { get; set; }
        public virtual ICollection<FORM_VALIDATION_RULES> FORM_VALIDATION_RULES { get; set; }
        public virtual ICollection<FORM_BUTTONS> FORM_BUTTONS { get; set; }
    }



}
