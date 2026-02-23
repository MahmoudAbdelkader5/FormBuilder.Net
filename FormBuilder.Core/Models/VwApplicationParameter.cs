using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwApplicationParameter
{
    public int IdApplication { get; set; }

    public string ApplicationName { get; set; } = null!;

    public int IdParameter { get; set; }

    public string ParameterName { get; set; } = null!;

    public string? ParameterForeignName { get; set; }

    public string? ParameterValue { get; set; }
}
