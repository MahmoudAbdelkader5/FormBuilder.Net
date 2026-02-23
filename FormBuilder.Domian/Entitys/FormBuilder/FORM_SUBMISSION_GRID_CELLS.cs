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
    [Table("FORM_SUBMISSION_GRID_CELLS")]
    public class FORM_SUBMISSION_GRID_CELLS : BaseEntity
    {
       

        [ForeignKey("FORM_SUBMISSION_GRID_ROWS")]
        public int RowId { get; set; }
        public virtual FORM_SUBMISSION_GRID_ROWS FORM_SUBMISSION_GRID_ROWS { get; set; }

        [ForeignKey("FORM_GRID_COLUMNS")]
        public int ColumnId { get; set; }
        public virtual FORM_GRID_COLUMNS FORM_GRID_COLUMNS { get; set; }

        public string ValueString { get; set; }
        public decimal? ValueNumber { get; set; }
        public DateTime? ValueDate { get; set; }
        public bool? ValueBool { get; set; }
        public string ValueJson { get; set; }
    }

}
