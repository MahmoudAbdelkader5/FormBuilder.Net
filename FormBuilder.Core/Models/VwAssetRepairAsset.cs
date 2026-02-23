using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwAssetRepairAsset
{
    public int Id { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? Quantity { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? PurchaseRequestCode { get; set; }

    public int? IdAssetRepair { get; set; }

    public string AssetRepairCode { get; set; } = null!;

    public string? AssetRepairDescription { get; set; }

    public int? IdAsset { get; set; }

    public string? AssetCode { get; set; }

    public string? AssetName { get; set; }

    public string? AssetForeignName { get; set; }

    public int? IdWorkOrder { get; set; }

    public string? WorkOrderDocumentNumber { get; set; }

    public string? WorkOrderName { get; set; }

    public string? WorkOrderForeignName { get; set; }

    public int? IdToWarehouse { get; set; }

    public string? ToWarehouseCode { get; set; }

    public string? ToWarehouseName { get; set; }

    public string? ToWarehouseForeignName { get; set; }

    public int? IdFromWarehouse { get; set; }

    public string? FromWarehouseCode { get; set; }

    public string? FromWarehouseName { get; set; }

    public string? FromWarehouseForeignName { get; set; }
}
