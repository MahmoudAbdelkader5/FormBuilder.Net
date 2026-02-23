using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwLookup
{
    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdLookupType { get; set; }

    public int LookupKey { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public string LookUpTypeName { get; set; } = null!;

    public string? LookUpTypeForeignName { get; set; }
}
