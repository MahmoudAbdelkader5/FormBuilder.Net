using formBuilder.Domian.Entitys;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormBuilder.Domian.Entitys.FormBuilder
{
    [Table("FIELD_TYPES")]
    public class FIELD_TYPES : BaseEntity
    {
        [Required, StringLength(100)]
        public string TypeName { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string DataType { get; set; } = string.Empty;

        public int? MaxLength { get; set; }

        public bool HasOptions { get; set; } = false;

        public bool AllowMultiple { get; set; } = false;

        public new bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<FORM_FIELDS> FORM_FIELDS { get; set; } = new List<FORM_FIELDS>();
        public virtual ICollection<FORM_GRID_COLUMNS> FORM_GRID_COLUMNS { get; set; } = new List<FORM_GRID_COLUMNS>();
    }
}

