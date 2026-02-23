using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwApplicationMenu
{
    public int IdApplication { get; set; }

    public string ApplicationName { get; set; } = null!;

    public int IdMenu { get; set; }

    public string MenuName { get; set; } = null!;

    public string MenuDescription { get; set; } = null!;

    public string MenuForeignDescription { get; set; } = null!;
}
