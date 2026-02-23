using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwRequiredAttachmentType
{
    public int Id { get; set; }

    public bool? IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int IdAttachmentType { get; set; }

    public string AttachmentTypeName { get; set; } = null!;

    public string? AttachmentTypeForeignName { get; set; }

    public int IdTransactionType { get; set; }

    public string TransactionTypeName { get; set; } = null!;

    public string? TransactionTypeForeignName { get; set; }
}
