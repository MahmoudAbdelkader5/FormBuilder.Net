using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwApplicationAlertTag
{
    public int IdApplication { get; set; }

    public string ApplicationName { get; set; } = null!;

    public int IdAlertTag { get; set; }

    public string TagName { get; set; } = null!;

    public string? TagDescription { get; set; }

    public string? TagForeignDescription { get; set; }
}
