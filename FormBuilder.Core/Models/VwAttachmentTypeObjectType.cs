using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwAttachmentTypeObjectType
{
    public int Id { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IdObjectType { get; set; }

    public string ObjectTypeName { get; set; } = null!;

    public string? ObjectTypeForeignName { get; set; }

    public int? IdAttachmentType { get; set; }

    public string AttachmentTypeName { get; set; } = null!;

    public string? AttachmentTypeForeignName { get; set; }

    public bool IsActive { get; set; }
}
