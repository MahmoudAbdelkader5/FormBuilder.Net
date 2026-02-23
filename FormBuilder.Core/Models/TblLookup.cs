using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblLookup
{
    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public int IdLookupType { get; set; }

    public int LookupKey { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblLookupType IdLookupTypeNavigation { get; set; } = null!;
}
