using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwWarehouseBinLocation
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? Quantity { get; set; }

    public bool? IsDefault { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdWarehouse { get; set; }

    public string WarehouseName { get; set; } = null!;

    public string? WarehouseForeignName { get; set; }

    public string WarehouseCode { get; set; } = null!;
}
