using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblZone
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblAssetTransferAsset> TblAssetTransferAssetIdFromZoneNavigations { get; set; } = new List<TblAssetTransferAsset>();

    public virtual ICollection<TblAssetTransferAsset> TblAssetTransferAssetIdToZoneNavigations { get; set; } = new List<TblAssetTransferAsset>();

    public virtual ICollection<TblAsset> TblAssets { get; set; } = new List<TblAsset>();

    public virtual ICollection<TblWorkOrder> TblWorkOrders { get; set; } = new List<TblWorkOrder>();
}
