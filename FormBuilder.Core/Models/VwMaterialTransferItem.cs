using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwMaterialTransferItem
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public int IdTechnician { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IssuedQuantity { get; set; }

    public DateTime? IssueDate { get; set; }

    public DateTime? StatusDate { get; set; }

    public int? IdMaterialRequestItem { get; set; }

    public int IdMaterialTransfer { get; set; }

    public string MaterialTransferCode { get; set; } = null!;

    public DateTime MaterialTransferCreatedDate { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public int IdItems { get; set; }

    public string? ItemCode { get; set; }

    public string ItemName { get; set; } = null!;

    public string? ItemForeignName { get; set; }

    public bool? ItemIsSerialManaged { get; set; }

    public int? IdInventoryUnitMeasure { get; set; }

    public string? InventoryUnitMeasureCode { get; set; }

    public string? InventoryUnitMeasureName { get; set; }

    public string? InventoryUnitMeasureForeignName { get; set; }

    public int? IdMaintenanceType { get; set; }

    public string? MaintenanceTypeCode { get; set; }

    public string? MaintenanceTypeName { get; set; }

    public string? MaintenanceTypeForeignName { get; set; }

    public int? IdAsset { get; set; }

    public string? AssetCode { get; set; }

    public string? AssetName { get; set; }

    public string? AssetForeignName { get; set; }

    public int? IdToWarehouse { get; set; }

    public string? ToWarehouseCode { get; set; }

    public string? ToWarehouseName { get; set; }

    public string? ToWarehouseForeignName { get; set; }

    public bool? ToWarehouseIsBinEnabled { get; set; }

    public int? IdFromWarehouse { get; set; }

    public string? FromWarehouseCode { get; set; }

    public string? FromWarehouseName { get; set; }

    public string? FromWarehouseForeignName { get; set; }

    public bool? FromWarehouseIsBinEnabled { get; set; }

    public int? IdToBinLocation { get; set; }

    public string? ToBinLocationCode { get; set; }

    public int? IdFromBinLocation { get; set; }

    public string? FromBinLocationCode { get; set; }

    public int? IdStatus { get; set; }

    public string? ItemStatusName { get; set; }

    public string? ItemStatusForeignName { get; set; }

    public int IdStatusBy { get; set; }

    public string? UserStatusName { get; set; }

    public string? UserStatusForeignName { get; set; }

    public int? IdWorkOrder { get; set; }

    public string? WorkOrderName { get; set; }

    public string? WorkOrderForeignName { get; set; }

    public DateTime? DocumentDate { get; set; }

    public DateTime? RequiredDate { get; set; }

    public DateTime? ClosingDate { get; set; }

    public int? MaterialTransferIdApprovalStatus { get; set; }

    public string? StatusName { get; set; }

    public string? StatusForeignName { get; set; }

    public int? IdUnit { get; set; }

    public string? UnitName { get; set; }

    public string? UnitForeignName { get; set; }

    public string? UnitCode { get; set; }
}
