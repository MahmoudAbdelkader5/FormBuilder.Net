using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwWorkOrderAsset
{
    public int IdAsset { get; set; }

    public string AssetCode { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public string? AssetForeignName { get; set; }

    public int? AssetType { get; set; }

    public int? AssetIdGroup { get; set; }

    public int? AssetIdZone { get; set; }

    public int? AssetIdCostCenter1 { get; set; }

    public int? AssetIdCostCenter2 { get; set; }

    public int? AssetIdCostCenter3 { get; set; }

    public int? AssetIdCostCenter4 { get; set; }

    public int? AssetIdCostCenter5 { get; set; }

    public string? AssetBarcode { get; set; }

    public string? AssetDescription { get; set; }

    public string? AssetForeignDescription { get; set; }

    public string? AssetSerialNumber { get; set; }

    public int? AssetIdManufacturer { get; set; }

    public int IdWorkOrder { get; set; }

    public string WorkOrderName { get; set; } = null!;

    public string? WorkOrderForeignName { get; set; }

    public string? WorkOrderDocumentNumber { get; set; }

    public int? WorkOrderIdWocategory { get; set; }

    public int? WorkOrderIdSource { get; set; }

    public int? WorkOrderIdPriority { get; set; }

    public int WorkOrderIdZone { get; set; }

    public string? WorkOrderManholeNumber { get; set; }

    public int? WorkOrderIdSafetyAssessment { get; set; }

    public DateTime? WorkOrderDocumentDate { get; set; }

    public DateTime? WorkOrderRequiredDate { get; set; }

    public DateTime? WorkOrderClosingDate { get; set; }

    public DateTime? WorkOrderStartDate { get; set; }

    public bool WorkOrderIsActive { get; set; }

    public long WorkOrderReferenceNumber { get; set; }

    public int? WorkOrderIdApprovalStatus { get; set; }

    public string WorkOrderApprovalStatusName { get; set; } = null!;

    public string? WorkOrderApprovalStatusForeignName { get; set; }

    public int WorkOrderIdWorkOrderCategory { get; set; }

    public string WorkOrderCategoryName { get; set; } = null!;

    public string? WorkOrderCategoryForeignName { get; set; }

    public int? WorkOrderIdJobType { get; set; }

    public string JobTypeName { get; set; } = null!;

    public string? JobTypeForeignName { get; set; }

    public int? IdUnit { get; set; }

    public string? UnitName { get; set; }

    public string? UnitForeignName { get; set; }

    public string? UnitCode { get; set; }

    public int IdCreatedBy { get; set; }

    public string? CreatedByName { get; set; }

    public string? CreatedByForeignName { get; set; }
}
