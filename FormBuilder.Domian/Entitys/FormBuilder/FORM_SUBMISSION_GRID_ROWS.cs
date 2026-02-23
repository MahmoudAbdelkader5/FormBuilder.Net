using formBuilder.Domian.Entitys;
using FormBuilder.Domian.Entitys.FormBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Domian.Entitys.FormBuilder
{
    [Table("FORM_SUBMISSION_GRID_ROWS")]
    public class FORM_SUBMISSION_GRID_ROWS : BaseEntity
    {
       

        [ForeignKey("FORM_SUBMISSIONS")]
        public int SubmissionId { get; set; }
        public virtual FORM_SUBMISSIONS FORM_SUBMISSIONS { get; set; }

        [ForeignKey("FORM_GRIDS")]
        public int GridId { get; set; }
        public virtual FORM_GRIDS FORM_GRIDS { get; set; }

        public int RowIndex { get; set; }
        public new bool IsActive { get; set; }

        public virtual ICollection<FORM_SUBMISSION_GRID_CELLS> FORM_SUBMISSION_GRID_CELLS { get; set; }
    }

}
