using formBuilder.Domian.Entitys;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domian.Entitys.froms;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormBuilder.Domian.Entitys.FormBuilder
{
    [Table("FORM_TABS")]
    public class FORM_TABS : BaseEntity
    {
        [ForeignKey("FORM_BUILDER")]
        public int FormBuilderId { get; set; }
        public virtual FORM_BUILDER FORM_BUILDER { get; set; }

        [Required, StringLength(200)]
        public string TabName { get; set; }

        [StringLength(200)]
        public string? ForeignTabName { get; set; }

        [StringLength(100)]
        public string TabCode { get; set; }

        public int TabOrder { get; set; }

        public virtual ICollection<FORM_FIELDS> FORM_FIELDS { get; set; }
        public virtual ICollection<FORM_GRIDS> FORM_GRIDS { get; set; }
        
        // constructor
        public FORM_TABS()
        {
            FORM_FIELDS = new HashSet<FORM_FIELDS>();
            FORM_GRIDS = new HashSet<FORM_GRIDS>();
        }
    }
}
