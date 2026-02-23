using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwWorkOrderExpense
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public DateTime? Date { get; set; }

    public int Quantity { get; set; }

    public int Cost { get; set; }

    public int IdLegalEntity { get; set; }

    public string? Code { get; set; }

    public int IdWorkOrder { get; set; }

    public string WorkOrderName { get; set; } = null!;

    public string? WorkOrderForeignName { get; set; }

    public string? WorkOrderDocumentNumber { get; set; }

    public int IdAsset { get; set; }

    public string AssetCode { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public string? AssetForeignName { get; set; }

    public int? AssetType { get; set; }

    public int? AssetIdGroup { get; set; }

    public int? AssetIdZone { get; set; }

    public int? AssetIdCostCenter1 { get; set; }

    public string? CostCenter1Name { get; set; }

    public string? CostCenter1ForeignName { get; set; }

    public int? AssetIdCostCenter2 { get; set; }

    public string? CostCenter2Name { get; set; }

    public string? CostCenter2ForeignName { get; set; }

    public int? AssetIdCostCenter3 { get; set; }

    public string? CostCenter3Name { get; set; }

    public string? CostCenter3ForeignName { get; set; }

    public int? AssetIdCostCenter4 { get; set; }

    public string? CostCenter4Name { get; set; }

    public string? CostCenter4ForeignName { get; set; }

    public int? AssetIdCostCenter5 { get; set; }

    public string? CostCenter5Name { get; set; }

    public string? CostCenter5ForeignName { get; set; }

    public string? AssetBarcode { get; set; }

    public string? AssetDescription { get; set; }

    public string? AssetForeignDescription { get; set; }

    public string? AssetSerialNumber { get; set; }

    public int? AssetIdManufacturer { get; set; }

    public int? WorkOrderIdApprovalStatus { get; set; }

    public string WorkOrderApprovalStatusName { get; set; } = null!;

    public string? WorkOrderApprovalStatusForeignName { get; set; }
}
