using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblSupplier
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public string? Group { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Description { get; set; }

    public string? Remark { get; set; }

    public int? QualityRating { get; set; }

    public int? DeliveryRating { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblAssetPurchaseRequestAsset> TblAssetPurchaseRequestAssets { get; set; } = new List<TblAssetPurchaseRequestAsset>();

    public virtual ICollection<TblItemPurchaseRequestItem> TblItemPurchaseRequestItems { get; set; } = new List<TblItemPurchaseRequestItem>();
}
