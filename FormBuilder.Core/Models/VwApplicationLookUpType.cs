using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwApplicationLookUpType
{
    public int IdApplication { get; set; }

    public string ApplicationName { get; set; } = null!;

    public int IdLookUpType { get; set; }

    public string LookUpTypeName { get; set; } = null!;

    public string? LookUpTypeForeignName { get; set; }
}
