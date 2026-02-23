using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblWarehouse
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public string? Location { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsMain { get; set; }

    public bool? IsBinEnabled { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblAssetRepairAsset> TblAssetRepairAssetIdFromWarehouseNavigations { get; set; } = new List<TblAssetRepairAsset>();

    public virtual ICollection<TblAssetRepairAsset> TblAssetRepairAssetIdToWarehouseNavigations { get; set; } = new List<TblAssetRepairAsset>();

    public virtual ICollection<TblGoodsIssueItem> TblGoodsIssueItems { get; set; } = new List<TblGoodsIssueItem>();

    public virtual ICollection<TblItemSerialNumber> TblItemSerialNumbers { get; set; } = new List<TblItemSerialNumber>();

    public virtual ICollection<TblMaterialRequestItem> TblMaterialRequestItemIdFromWarehouseNavigations { get; set; } = new List<TblMaterialRequestItem>();

    public virtual ICollection<TblMaterialRequestItem> TblMaterialRequestItemIdToWarehouseNavigations { get; set; } = new List<TblMaterialRequestItem>();

    public virtual ICollection<TblMaterialTransferItem> TblMaterialTransferItemIdFromWarehouseNavigations { get; set; } = new List<TblMaterialTransferItem>();

    public virtual ICollection<TblMaterialTransferItem> TblMaterialTransferItemIdToWarehouseNavigations { get; set; } = new List<TblMaterialTransferItem>();

    public virtual ICollection<TblSparePartRepairRequestDetail> TblSparePartRepairRequestDetailIdFromWarehouseNavigations { get; set; } = new List<TblSparePartRepairRequestDetail>();

    public virtual ICollection<TblSparePartRepairRequestDetail> TblSparePartRepairRequestDetailIdToWarehouseNavigations { get; set; } = new List<TblSparePartRepairRequestDetail>();

    public virtual ICollection<TblSparePartRepairTransferDetail> TblSparePartRepairTransferDetailIdFromWarehouseNavigations { get; set; } = new List<TblSparePartRepairTransferDetail>();

    public virtual ICollection<TblSparePartRepairTransferDetail> TblSparePartRepairTransferDetailIdToWarehouseNavigations { get; set; } = new List<TblSparePartRepairTransferDetail>();

    public virtual ICollection<TblToolTransferTool> TblToolTransferToolIdFromWarehouseNavigations { get; set; } = new List<TblToolTransferTool>();

    public virtual ICollection<TblToolTransferTool> TblToolTransferToolIdToWarehouseNavigations { get; set; } = new List<TblToolTransferTool>();

    public virtual ICollection<TblWarehouseBinLocation> TblWarehouseBinLocations { get; set; } = new List<TblWarehouseBinLocation>();

    public virtual ICollection<TblWarehouseItem> TblWarehouseItems { get; set; } = new List<TblWarehouseItem>();

    public virtual ICollection<TblWorkOrderGoodsIssue> TblWorkOrderGoodsIssues { get; set; } = new List<TblWorkOrderGoodsIssue>();

    public virtual ICollection<TblWorkOrderSparePart> TblWorkOrderSpareParts { get; set; } = new List<TblWorkOrderSparePart>();
}
