using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class Item
{
    public string? Code { get; set; }

    public string? Name { get; set; }

    public double? Type { get; set; }

    public string? ItemGroup { get; set; }

    public string? InventoryUnitOfMeasure { get; set; }
}
