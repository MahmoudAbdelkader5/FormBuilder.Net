using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwAssetTransferAsset
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public int? IdLegalEntity { get; set; }

    public DateTime? StatusDate { get; set; }

    public string? Remarks { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? IdCreatedBy { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? IdAssetTransfer { get; set; }

    public string AssetTransferCode { get; set; } = null!;

    public int IdTechnician { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public int IdAsset { get; set; }

    public string AssetCode { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public string? AssetForeignName { get; set; }

    public int? IdToZone { get; set; }

    public string? ToZoneCode { get; set; }

    public string? ToZoneName { get; set; }

    public string? ToZoneForeignName { get; set; }

    public int? IdFromZone { get; set; }

    public string? FromZoneCode { get; set; }

    public string? FromZoneName { get; set; }

    public string? FromZoneForeignName { get; set; }

    public int? IdStatus { get; set; }

    public string? AssetStatusName { get; set; }

    public string? AssetStatusForeignName { get; set; }

    public int IdStatusBy { get; set; }

    public string? UserStatusName { get; set; }

    public string? UserStatusForeignName { get; set; }
}
