using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwParameter
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public string? Value { get; set; }

    public int IdParameterType { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLookupType { get; set; }

    public string? LookUpTypeName { get; set; }

    public string? LookUpTypeForeignName { get; set; }

    public int? LookupKey { get; set; }

    public string? LookupName { get; set; }

    public string? LookupForeignName { get; set; }
}
