using formBuilder.Domian.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Domian.Entitys.FormBuilder
{
    [Table("GRID_COLUMN_OPTIONS")]
    public class GRID_COLUMN_OPTIONS : BaseEntity
    {
        [ForeignKey("FORM_GRID_COLUMNS")]
        public int ColumnId { get; set; }
        public virtual FORM_GRID_COLUMNS FORM_GRID_COLUMNS { get; set; }

        [Required, StringLength(200)]
        public string OptionText { get; set; } = string.Empty;

        [StringLength(200)]
        public string? ForeignOptionText { get; set; }

        [Required, StringLength(200)]
        public string OptionValue { get; set; } = string.Empty;

        public int OptionOrder { get; set; }
        public bool IsDefault { get; set; }
        public new bool IsActive { get; set; } = true;
    }
}

