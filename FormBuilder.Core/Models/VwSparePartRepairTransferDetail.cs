using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwSparePartRepairTransferDetail
{
    public int Id { get; set; }

    public string? SparePartNumber { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? Quantity { get; set; }

    public string? Remark { get; set; }

    public int? IdSparePartRepairTransfer { get; set; }

    public string? SparePartRepairCode { get; set; }

    public DateTime SparePartRepairCreatedDate { get; set; }

    public int? IdItem { get; set; }

    public string? ItemCode { get; set; }

    public string? ItemName { get; set; }

    public string? ItemForeignName { get; set; }

    public bool? ItemIsSerialManaged { get; set; }

    public int? IdToWarehouse { get; set; }

    public string? ToWarehouseCode { get; set; }

    public string? ToWarehouseName { get; set; }

    public string? ToWarehouseForeignName { get; set; }

    public int? IdFromWarehouse { get; set; }

    public string? FromWarehouseCode { get; set; }

    public string? FromWarehouseName { get; set; }

    public string? FromWarehouseForeignName { get; set; }

    public int? IdApprovalStatus { get; set; }

    public string? StatusName { get; set; }

    public string? StatusForeignName { get; set; }

    public int? IdSerialNumber { get; set; }

    public string? SerialNumberCode { get; set; }
}
