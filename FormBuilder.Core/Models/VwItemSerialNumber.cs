using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwItemSerialNumber
{
    public int Id { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public string? Code { get; set; }

    public decimal? Cost { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdWarehouse { get; set; }

    public string WarehouseName { get; set; } = null!;

    public string? WarehouseForeignName { get; set; }

    public string WarehouseCode { get; set; } = null!;

    public int? IdItem { get; set; }

    public string ItemName { get; set; } = null!;

    public string? ItemForeignName { get; set; }

    public string? ItemCode { get; set; }

    public int? IdBinLocation { get; set; }

    public string? BinLocationCode { get; set; }

    public int? BinLocationQuantity { get; set; }

    public bool? BinLocationIsDefault { get; set; }
}
