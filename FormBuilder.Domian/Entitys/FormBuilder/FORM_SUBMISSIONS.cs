using formBuilder.Domian.Entitys;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Domian.Entitys.froms;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("FORM_SUBMISSIONS")]
public class FORM_SUBMISSIONS : BaseEntity
{



    [ForeignKey("FORM_BUILDER")]
    public int FormBuilderId { get; set; }
    public virtual FORM_BUILDER FORM_BUILDER { get; set; } = null!;

    public int Version { get; set; }

    [ForeignKey("DOCUMENT_TYPES")]
    public int DocumentTypeId { get; set; }
    public virtual DOCUMENT_TYPES DOCUMENT_TYPES { get; set; } = null!;

    [ForeignKey("DOCUMENT_SERIES")]
    public int SeriesId { get; set; }
    public virtual DOCUMENT_SERIES DOCUMENT_SERIES { get; set; } = null!;

    [Required, StringLength(50)]
    public string DocumentNumber { get; set; } = string.Empty;

    public string SubmittedByUserId { get; set; } = string.Empty;

    public DateTime SubmittedDate { get; set; }

    [Required, StringLength(50)]
    public string Status { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string SignatureStatus { get; set; } = "not_required";

    [StringLength(64)]
    public string? DocuSignEnvelopeId { get; set; }

    public DateTime? SignedAt { get; set; }

    [ForeignKey("APPROVAL_STAGES")]
    public int? StageId { get; set; }
    public virtual APPROVAL_STAGES APPROVAL_STAGES { get; set; }

    [ForeignKey("ParentDocument")]
    public int? ParentDocumentId { get; set; }
    public virtual FORM_SUBMISSIONS? ParentDocument { get; set; }

    public virtual ICollection<FORM_SUBMISSION_VALUES> FORM_SUBMISSION_VALUES { get; set; } = null!;
    public virtual ICollection<FORM_SUBMISSION_ATTACHMENTS> FORM_SUBMISSION_ATTACHMENTS { get; set; } = null!;
    public virtual ICollection<FORM_SUBMISSION_GRID_ROWS> FORM_SUBMISSION_GRID_ROWS { get; set; } = null!;
    public virtual ICollection<DOCUMENT_APPROVAL_HISTORY> DOCUMENT_APPROVAL_HISTORY { get; set; } = null!;
}
