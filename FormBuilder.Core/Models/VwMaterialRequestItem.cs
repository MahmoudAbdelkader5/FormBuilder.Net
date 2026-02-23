using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwMaterialRequestItem
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public int IdTechnician { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IssuedQuantity { get; set; }

    public DateTime? IssueDate { get; set; }

    public bool? IsPurchaseRequestInitiated { get; set; }

    public int? PurchaseRequestQuantity { get; set; }

    public int IdMaterialRequest { get; set; }

    public string MaterialRequestCode { get; set; } = null!;

    public DateTime MaterialRequestCreatedDate { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public int IdItems { get; set; }

    public string? ItemCode { get; set; }

    public string ItemName { get; set; } = null!;

    public string? ItemForeignName { get; set; }

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

    public int? IdFromWarehouse { get; set; }

    public string? FromWarehouseCode { get; set; }

    public string? FromWarehouseName { get; set; }

    public string? FromWarehouseForeignName { get; set; }

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
