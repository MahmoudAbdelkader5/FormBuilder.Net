using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblRequiredAttachmentType
{
    public int IdAttachmentType { get; set; }

    public int IdTransactionType { get; set; }

    public bool? IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int Id { get; set; }

    public virtual TblAttachmentsType IdAttachmentTypeNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }
}
