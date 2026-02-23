using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblAssetRepair
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public int? IdWorkOrder { get; set; }

    public int IdTechncian { get; set; }

    public int? IdApprovalStatus { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Description { get; set; }

    public int? IdStoreStatus { get; set; }

    public string? StoreRemarks { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblUser IdTechncianNavigation { get; set; } = null!;

    public virtual TblWorkOrder? IdWorkOrderNavigation { get; set; }

    public virtual ICollection<TblAssetRepairAsset> TblAssetRepairAssets { get; set; } = new List<TblAssetRepairAsset>();
}
