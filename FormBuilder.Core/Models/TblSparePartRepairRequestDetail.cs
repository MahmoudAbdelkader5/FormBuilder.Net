using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblSparePartRepairRequestDetail
{
    public int Id { get; set; }

    public int? IdSparePartRepairRequest { get; set; }

    public string? SparePartNumber { get; set; }

    public string? SerialNumber { get; set; }

    public int? Quantity { get; set; }

    public string? Remark { get; set; }

    public int? IdFromWarehouse { get; set; }

    public int? IdToWarehouse { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblWarehouse? IdFromWarehouseNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblSparePartRepairRequest? IdSparePartRepairRequestNavigation { get; set; }

    public virtual TblWarehouse? IdToWarehouseNavigation { get; set; }
}
