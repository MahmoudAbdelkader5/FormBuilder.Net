using formBuilder.Domian.Entitys;
using FormBuilder.Domian.Entitys.FormBuilder;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormBuilder.Domian.Entitys.FromBuilder
{
    [Table("DOCUMENT_TYPES")]
    public class DOCUMENT_TYPES : BaseEntity
    {
        [Required]
        [StringLength(200)]
        [Column("Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [Column("Code")]
        public string Code { get; set; }

        [Required]
        [ForeignKey("FORM_BUILDER")]
        [Column("FormBuilderId")]
        public int ? FormBuilderId { get; set; }

        [Required]
        [StringLength(200)]
        [Column("MenuCaption")]
        public string MenuCaption { get; set; }

        [Required]
        [Column("MenuOrder")]
        public int MenuOrder { get; set; }

        [Column("ParentMenuId")]
        public int? ParentMenuId { get; set; }

        [Required]
        [Column("IsActive")]
        public new bool IsActive { get; set; } = true;

        [Column("ApprovalWorkflowId")]
        public int? ApprovalWorkflowId { get; set; }

        // Navigation properties
        public virtual FORM_BUILDER FORM_BUILDER { get; set; }

        [ForeignKey("ApprovalWorkflowId")]
        public virtual APPROVAL_WORKFLOWS ApprovalWorkflow { get; set; }

        [ForeignKey("ParentMenuId")]
        public virtual DOCUMENT_TYPES ParentMenu { get; set; }

        public virtual ICollection<DOCUMENT_TYPES> Children { get; set; }
        public virtual ICollection<FORM_SUBMISSIONS> FORM_SUBMISSIONS { get; set; }
        // Document Series is now independent from Document Type.
        // public virtual ICollection<DOCUMENT_SERIES> DOCUMENT_SERIES { get; set; }
        public virtual ICollection<APPROVAL_WORKFLOWS> APPROVAL_WORKFLOWS { get; set; }
        public virtual ICollection<EMAIL_TEMPLATES> EMAIL_TEMPLATES { get; set; }
        public virtual ICollection<ALERT_RULES> ALERT_RULES { get; set; }
        public virtual ICollection<CRYSTAL_LAYOUTS> CRYSTAL_LAYOUTS { get; set; }
        public virtual ICollection<OUTLOOK_APPROVAL_CONFIG> OUTLOOK_APPROVAL_CONFIG { get; set; }
        public virtual ICollection<SAP_OBJECT_MAPPINGS> SAP_OBJECT_MAPPINGS { get; set; }
    }
}
