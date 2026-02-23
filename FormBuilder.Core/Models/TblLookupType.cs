using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblLookupType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblLookup> TblLookups { get; set; } = new List<TblLookup>();
}
