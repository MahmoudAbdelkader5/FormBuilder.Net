using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwWarehouseItem
{
    public int Id { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? Quantity { get; set; }

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

    public decimal ItemCost { get; set; }

    public int? IdItemsType { get; set; }

    public string ItemTypeName { get; set; } = null!;

    public string? ItemTypeForeignName { get; set; }

    public int? IdItemGroup { get; set; }

    public string? ItemGroupCode { get; set; }

    public string ItemGroupName { get; set; } = null!;

    public string? ItemGroupForeignName { get; set; }

    public int? IdBinLocation { get; set; }

    public string? BinLocationCode { get; set; }

    public int? BinLocationQuantity { get; set; }

    public bool? BinLocationIsDefault { get; set; }
}
