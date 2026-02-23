using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwWorkOrderSparePart
{
    public int Id { get; set; }

    public decimal? EstimatedQuantity { get; set; }

    public decimal? ActualQuantity { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public decimal? QuantityToIssue { get; set; }

    public decimal? WarehouseQuantity { get; set; }

    public decimal? Cost { get; set; }

    public int? IdMaintenanceType { get; set; }

    public string? MTypeCode { get; set; }

    public string? MTypeName { get; set; }

    public string? MTypeForeignName { get; set; }

    public string? MTypeDescription { get; set; }

    public string? MTypeForeignDescription { get; set; }

    public bool? MTypeIsActive { get; set; }

    public int IdWorkOrder { get; set; }

    public string WorkOrderName { get; set; } = null!;

    public string? WorkOrderForeignName { get; set; }

    public string? WorkOrderDocumentNumber { get; set; }

    public int WorkOrderIdWorkOrderCategory { get; set; }

    public string? WorkOrderCategoryName { get; set; }

    public string? WorkOrderCategoryForeignName { get; set; }

    public int IdSparePart { get; set; }

    public string? ItemsCode { get; set; }

    public string ItemsName { get; set; } = null!;

    public string? ItemsForeignName { get; set; }

    public int? ItemsIdItemsType { get; set; }

    public decimal ItemsCost { get; set; }

    public bool ItemsIsActive { get; set; }

    public string? ItemTypeName { get; set; }

    public string? ItemTypeForeignName { get; set; }

    public bool? ItemIsSerialManaged { get; set; }

    public int IdAsset { get; set; }

    public string AssetCode { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public string? AssetForeignName { get; set; }

    public int? AssetType { get; set; }

    public int? AssetIdGroup { get; set; }

    public int? AssetIdZone { get; set; }

    public string? AssetBarcode { get; set; }

    public string? AssetDescription { get; set; }

    public string? AssetForeignDescription { get; set; }

    public string? AssetSerialNumber { get; set; }

    public int? AssetIdManufacturer { get; set; }

    public string? CostCenter1Name { get; set; }

    public string? CostCenter2Name { get; set; }

    public string? CostCenter3Name { get; set; }

    public string? CostCenter4Name { get; set; }

    public string? CostCenter5Name { get; set; }

    public string? CostCenter1ForeignName { get; set; }

    public string? CostCenter2ForeignName { get; set; }

    public string? CostCenter3ForeignName { get; set; }

    public string? CostCenter4ForeignName { get; set; }

    public string? CostCenter5ForeignName { get; set; }

    public int? WorkOrderIdApprovalStatus { get; set; }

    public string? WorkOrderApprovalStatusName { get; set; }

    public string? WorkOrderApprovalStatusForeignName { get; set; }

    public int IdCreatedBy { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public int? IdUserWarehouse { get; set; }

    public string? UserWarehouseName { get; set; }

    public string? UserWarehouseForeignName { get; set; }

    public string? UserWarehouseCode { get; set; }

    public int? IdWarehouse { get; set; }

    public string? WarehouseName { get; set; }

    public string? WarehouseForeignName { get; set; }

    public string? WarehouseCode { get; set; }
}
