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
    [Table("APPROVAL_STAGE_ASSIGNEES")]
    public class APPROVAL_STAGE_ASSIGNEES : BaseEntity
    {

        [ForeignKey("APPROVAL_STAGES")]
        public int StageId { get; set; }
        public virtual APPROVAL_STAGES APPROVAL_STAGES { get; set; }

        [StringLength(450)]
        public string? RoleId { get; set; }

        public string? UserId { get; set; }
    }
}
