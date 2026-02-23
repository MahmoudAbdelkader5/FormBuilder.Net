using formBuilder.Domian.Entitys;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormBuilder.Domian.Entitys.FormBuilder
{
    [Table("APPROVAL_DELEGATIONS")]
    public class APPROVAL_DELEGATIONS : BaseEntity
    {
        [Required]
        [StringLength(450)]
        public string FromUserId { get; set; } = string.Empty;  // الموافق الأصلي

        [Required]
        [StringLength(450)]
        public string ToUserId { get; set; } = string.Empty;    // الموافق المفوض

        [Required]
        [StringLength(50)]
        public string ScopeType { get; set; } = "Global";  // "Global" / "Workflow" / "Document"

        public int? ScopeId { get; set; }  // NULL (Global) / WorkflowId / SubmissionId

        [Required]
        public DateTime StartDate { get; set; }  // تاريخ بداية التفويض

        [Required]
        public DateTime EndDate { get; set; }    // تاريخ نهاية التفويض

        public new bool IsActive { get; set; } = true;  // هل التفويض نشط؟
    }
}
