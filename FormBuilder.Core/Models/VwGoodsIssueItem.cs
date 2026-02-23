using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwGoodsIssueItem
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public int IdTechnician { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IssuedQuantity { get; set; }

    public DateTime? IssueDate { get; set; }

    public int? IdWorkOrderSparePart { get; set; }

    public int IdGoodsIssue { get; set; }

    public string GoodsIssueCode { get; set; } = null!;

    public DateTime GoodsIssueCreatedDate { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public int IdItems { get; set; }

    public string? ItemCode { get; set; }

    public string ItemName { get; set; } = null!;

    public string? ItemForeignName { get; set; }

    public bool? ItemIsSerialManaged { get; set; }

    public int? IdInventoryUnitMeasure { get; set; }

    public string? InventoryUnitMeasureCode { get; set; }

    public string InventoryUnitMeasureName { get; set; } = null!;

    public string? InventoryUnitMeasureForeignName { get; set; }

    public int? IdMaintenanceType { get; set; }

    public string? MaintenanceTypeCode { get; set; }

    public string? MaintenanceTypeName { get; set; }

    public string? MaintenanceTypeForeignName { get; set; }

    public int? IdAsset { get; set; }

    public string? AssetCode { get; set; }

    public string? AssetName { get; set; }

    public string? AssetForeignName { get; set; }

    public int? IdCostCenter1 { get; set; }

    public string? CostCenterCode1 { get; set; }

    public string? CostCenter1Name { get; set; }

    public string? CostCenter1ForeignName { get; set; }

    public int? IdCostCenter2 { get; set; }

    public string? CostCenterCode2 { get; set; }

    public string? CostCenter2Name { get; set; }

    public string? CostCenter2ForeignName { get; set; }

    public int? IdCostCenter3 { get; set; }

    public string? CostCenterCode3 { get; set; }

    public string? CostCenter3Name { get; set; }

    public string? CostCenter3ForeignName { get; set; }

    public int? IdCostCenter4 { get; set; }

    public string? CostCenterCode4 { get; set; }

    public string? CostCenter4Name { get; set; }

    public string? CostCenter4ForeignName { get; set; }

    public int? IdCostCenter5 { get; set; }

    public string? CostCenterCode5 { get; set; }

    public string? CostCenter5Name { get; set; }

    public string? CostCenter5ForeignName { get; set; }

    public int? IdWarehouse { get; set; }

    public string? WarehouseCode { get; set; }

    public string? WarehouseName { get; set; }

    public string? WarehouseForeignName { get; set; }

    public int? IdBinLocation { get; set; }

    public string? BinLocationCode { get; set; }

    public int? IdWorkOrder { get; set; }

    public string? WorkOrderName { get; set; }

    public string? WorkOrderForeignName { get; set; }

    public DateTime? DocumentDate { get; set; }

    public DateTime? RequiredDate { get; set; }

    public DateTime? ClosingDate { get; set; }

    public int? IdApprovalStatus { get; set; }

    public string? StatusName { get; set; }

    public string? StatusForeignName { get; set; }
}
