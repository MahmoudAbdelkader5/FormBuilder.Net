using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwCertification
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public DateTime? Date { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public int IdTechncian { get; set; }

    public int? IdLegalEntity { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int IdCreatedBy { get; set; }

    public int? IdUpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public int AboutToExpire { get; set; }

    public string? TechnicianName { get; set; }

    public string? TechnicianForeignName { get; set; }

    public int? IdAsset { get; set; }

    public string? AssetCode { get; set; }

    public string? AssetName { get; set; }

    public string? AssetForeignName { get; set; }
}
