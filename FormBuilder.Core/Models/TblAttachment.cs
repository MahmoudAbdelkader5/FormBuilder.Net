using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblAttachment
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public string? Path { get; set; }

    public string? Description { get; set; }

    public int? IdAttachmentsType { get; set; }

    public int? IdObject { get; set; }

    public int? IdObjectType { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdWorkOrder { get; set; }

    public virtual TblAttachmentsType? IdAttachmentsTypeNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblWorkOrder? IdWorkOrderNavigation { get; set; }

    public virtual ICollection<TblWorkOrderAttachment> TblWorkOrderAttachments { get; set; } = new List<TblWorkOrderAttachment>();
}
