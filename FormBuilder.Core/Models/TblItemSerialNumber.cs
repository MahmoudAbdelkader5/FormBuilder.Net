using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblItemSerialNumber
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public int? IdItem { get; set; }

    public int? IdWarehouse { get; set; }

    public int? IdBinLocation { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public decimal? Cost { get; set; }

    public virtual TblWarehouseBinLocation? IdBinLocationNavigation { get; set; }

    public virtual TblItem? IdItemNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblWarehouse? IdWarehouseNavigation { get; set; }

    public virtual ICollection<TblGoodsIssueItemSerialNumber> TblGoodsIssueItemSerialNumbers { get; set; } = new List<TblGoodsIssueItemSerialNumber>();

    public virtual ICollection<TblMaterialTransferItemSerialNumber> TblMaterialTransferItemSerialNumbers { get; set; } = new List<TblMaterialTransferItemSerialNumber>();

    public virtual ICollection<TblSparePartRepairTransferDetail> TblSparePartRepairTransferDetails { get; set; } = new List<TblSparePartRepairTransferDetail>();
}
