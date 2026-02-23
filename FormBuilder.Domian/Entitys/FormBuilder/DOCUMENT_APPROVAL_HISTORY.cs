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
    [Table("DOCUMENT_APPROVAL_HISTORY")]
    public class DOCUMENT_APPROVAL_HISTORY : BaseEntity
    {
        

        [ForeignKey("FORM_SUBMISSIONS")]
        public int SubmissionId { get; set; }
        public virtual FORM_SUBMISSIONS FORM_SUBMISSIONS { get; set; }

        [ForeignKey("APPROVAL_STAGES")]
        public int StageId { get; set; }
        public virtual APPROVAL_STAGES APPROVAL_STAGES { get; set; }


        public DateTime ActionDate { get; set; }

        [Required, StringLength(50)]
        public string ActionType { get; set; }

        [StringLength(450)]
        public string ActionByUserId { get; set; }  // الموافق المفوض (من قام بالموافقة)

        [StringLength(450)]
        public string? OriginalApproverUserId { get; set; }  // الموافق الأصلي (المرجع)

        [ForeignKey("APPROVAL_DELEGATIONS")]
        public int? DelegationId { get; set; }  // ID الـ Delegation إذا تم استخدام تفويض
        public virtual APPROVAL_DELEGATIONS? APPROVAL_DELEGATIONS { get; set; }

        // Optional comment, but DB column is NOT NULL.
        public string Comments { get; set; } = string.Empty;
    }
}
