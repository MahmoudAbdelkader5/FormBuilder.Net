using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwRemark
{
    public int Id { get; set; }

    public int? IdObject { get; set; }

    public string? Description { get; set; }

    public int? IdLegalEntity { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public int? IdWorkOrder { get; set; }

    public int IdCreatedBy { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public string UserUsername { get; set; } = null!;

    public string? UserEmail { get; set; }

    public string UserPassword { get; set; } = null!;

    public string? UserPhone { get; set; }

    public int? IdObjectType { get; set; }

    public string ObjectTypeName { get; set; } = null!;

    public string? ObjectTypeForeignName { get; set; }

    public int? IdAsset { get; set; }

    public string? AssetCode { get; set; }

    public string? AssetName { get; set; }

    public string? AssetForeignName { get; set; }

    public int? IdMaintenanceType { get; set; }

    public string? MaintenanceTypeCode { get; set; }

    public string? MaintenanceTypeName { get; set; }

    public string? MaintenanceTypeForeignName { get; set; }
}
