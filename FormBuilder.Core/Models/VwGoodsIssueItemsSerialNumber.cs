using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwGoodsIssueItemsSerialNumber
{
    public int? IdLegalEntity { get; set; }

    public int? IdGoodsIssueItem { get; set; }

    public int TransferItemQuantity { get; set; }

    public int? TransferItemIssuedQuantity { get; set; }

    public DateTime? TransferItemIssueDate { get; set; }

    public int IdGoodsIssue { get; set; }

    public string GoodsIssueCode { get; set; } = null!;

    public DateTime GoodsIssueCreatedDate { get; set; }

    public int IdItems { get; set; }

    public string? ItemCode { get; set; }

    public string ItemName { get; set; } = null!;

    public string? ItemForeignName { get; set; }

    public bool? ItemIsSerialManaged { get; set; }

    public int? IdMaintenanceType { get; set; }

    public string? MaintenanceTypeCode { get; set; }

    public string? MaintenanceTypeName { get; set; }

    public string? MaintenanceTypeForeignName { get; set; }

    public int? IdAsset { get; set; }

    public string? AssetCode { get; set; }

    public string? AssetName { get; set; }

    public string? AssetForeignName { get; set; }

    public int? IdItemSerialNumber { get; set; }

    public string? SerialNumberCode { get; set; }
}
