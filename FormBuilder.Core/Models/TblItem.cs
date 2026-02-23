using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblItem
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public int? IdItemsType { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public decimal Cost { get; set; }

    public int? IdItemGroup { get; set; }

    public int? IdInventoryUnitMeasure { get; set; }

    public bool? IsSerialManaged { get; set; }

    public decimal? QuantityInStock { get; set; }

    public string? Tax { get; set; }

    public int? IdPreferrableVendor { get; set; }

    public virtual TblInventoryUnitMeasure? IdInventoryUnitMeasureNavigation { get; set; }

    public virtual TblItemGroup? IdItemGroupNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblGoodsIssueItem> TblGoodsIssueItems { get; set; } = new List<TblGoodsIssueItem>();

    public virtual ICollection<TblItemSerialNumber> TblItemSerialNumbers { get; set; } = new List<TblItemSerialNumber>();

    public virtual ICollection<TblMaintenanceTypeItem> TblMaintenanceTypeItems { get; set; } = new List<TblMaintenanceTypeItem>();

    public virtual ICollection<TblMaterialRequestItem> TblMaterialRequestItems { get; set; } = new List<TblMaterialRequestItem>();

    public virtual ICollection<TblMaterialTransferItem> TblMaterialTransferItems { get; set; } = new List<TblMaterialTransferItem>();

    public virtual ICollection<TblSparePartRepairTransferDetail> TblSparePartRepairTransferDetails { get; set; } = new List<TblSparePartRepairTransferDetail>();

    public virtual ICollection<TblWarehouseItem> TblWarehouseItems { get; set; } = new List<TblWarehouseItem>();

    public virtual ICollection<TblWorkOrderGoodsIssue> TblWorkOrderGoodsIssues { get; set; } = new List<TblWorkOrderGoodsIssue>();

    public virtual ICollection<TblWorkOrderSparePart> TblWorkOrderSpareParts { get; set; } = new List<TblWorkOrderSparePart>();
}
