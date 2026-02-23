using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblDocumentAttestationType
{
    public int IdObjectType { get; set; }

    public int IdDocument { get; set; }

    public int IdAttestationType { get; set; }

    public DateTime? AttestationDate { get; set; }

    public bool IsChecked { get; set; }

    public int? IdCheckedBy { get; set; }

    public DateTime? CheckDate { get; set; }

    public bool? IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual TblAttestationType IdAttestationTypeNavigation { get; set; } = null!;

    public virtual TblUser? IdCheckedByNavigation { get; set; }
}
