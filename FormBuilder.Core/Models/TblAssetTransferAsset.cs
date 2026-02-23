using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblAssetTransferAsset
{
    public int Id { get; set; }

    public int IdAsset { get; set; }

    public int IdTechnician { get; set; }

    public int? IdFromZone { get; set; }

    public int? IdToZone { get; set; }

    public int Quantity { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IdStatus { get; set; }

    public int? IdStatusBy { get; set; }

    public DateTime? StatusDate { get; set; }

    public string? Remarks { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? IdCreatedBy { get; set; }

    public int? IdAssetTransfer { get; set; }

    public virtual TblAsset IdAssetNavigation { get; set; } = null!;

    public virtual TblAssetTransfer? IdAssetTransferNavigation { get; set; }

    public virtual TblZone? IdFromZoneNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblUser IdTechnicianNavigation { get; set; } = null!;

    public virtual TblZone? IdToZoneNavigation { get; set; }
}
