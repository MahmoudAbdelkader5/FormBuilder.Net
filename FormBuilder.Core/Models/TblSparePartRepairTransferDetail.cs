using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblSparePartRepairTransferDetail
{
    public int Id { get; set; }

    public int? IdSparePartRepairTransfer { get; set; }

    public int? IdItem { get; set; }

    public string? SparePartNumber { get; set; }

    public int? Quantity { get; set; }

    public string? Remark { get; set; }

    public int? IdFromWarehouse { get; set; }

    public int? IdToWarehouse { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IdSerialNumber { get; set; }

    public virtual TblWarehouse? IdFromWarehouseNavigation { get; set; }

    public virtual TblItem? IdItemNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblItemSerialNumber? IdSerialNumberNavigation { get; set; }

    public virtual TblSparePartRepairTransfer? IdSparePartRepairTransferNavigation { get; set; }

    public virtual TblWarehouse? IdToWarehouseNavigation { get; set; }
}
