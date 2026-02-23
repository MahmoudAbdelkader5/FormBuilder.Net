using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwDocumentAttestationType
{
    public DateTime? AttestationDate { get; set; }

    public bool IsChecked { get; set; }

    public DateTime? CheckDate { get; set; }

    public bool? IsActive { get; set; }

    public int IdDocument { get; set; }

    public int? IdLegalEntity { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int IdCreatedBy { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public int? IdCheckedBy { get; set; }

    public string? CheckedByName { get; set; }

    public string? CheckedByForeignName { get; set; }

    public int IdAttestationType { get; set; }

    public string AttestationTypeName { get; set; } = null!;

    public string? AttestationTypeForeignName { get; set; }

    public int IdObjectType { get; set; }

    public string ObjectTypeName { get; set; } = null!;

    public string? ObjectTypeForeignName { get; set; }
}
