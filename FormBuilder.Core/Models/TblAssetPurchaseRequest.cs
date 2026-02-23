using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblAssetPurchaseRequest
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public int IdTechncian { get; set; }

    public int? IdApprovalStatus { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdStoreStatus { get; set; }

    public string? StoreRemarks { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblUser IdTechncianNavigation { get; set; } = null!;

    public virtual ICollection<TblAssetPurchaseRequestAsset> TblAssetPurchaseRequestAssets { get; set; } = new List<TblAssetPurchaseRequestAsset>();
}
