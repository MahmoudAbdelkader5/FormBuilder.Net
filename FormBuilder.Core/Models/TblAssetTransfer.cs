using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblAssetTransfer
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public int? IdApprovalStatus { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblAssetTransferAsset> TblAssetTransferAssets { get; set; } = new List<TblAssetTransferAsset>();
}
