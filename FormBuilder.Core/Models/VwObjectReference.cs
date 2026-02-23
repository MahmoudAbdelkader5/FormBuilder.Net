using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwObjectReference
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public long? FirstReference { get; set; }

    public long? LastReference { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? NumberofDigits { get; set; }

    public string? ZeroNumber { get; set; }

    public int? IdObjectType { get; set; }

    public string ObjectTypeName { get; set; } = null!;

    public string? ObjectTypeForeignName { get; set; }
}
