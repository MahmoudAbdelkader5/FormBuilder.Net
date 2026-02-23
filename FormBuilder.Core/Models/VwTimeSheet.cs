using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwTimeSheet
{
    public int Id { get; set; }

    public DateOnly TimeSheetDate { get; set; }

    public decimal Hours { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdCreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public int IdTechncian { get; set; }

    public string UserUsername { get; set; } = null!;

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public string? TypeName { get; set; }

    public string? TypeForeignName { get; set; }

    public int? IdWorkOrder { get; set; }

    public string? WorkOrderName { get; set; }

    public string? WorkOrderForeignName { get; set; }

    public string? DocumentNumber { get; set; }

    public int? IdAsset { get; set; }

    public string? AssetCode { get; set; }

    public string? AssetName { get; set; }

    public string? AssetForeignName { get; set; }

    public int? IdMaintenanceType { get; set; }

    public string? MaintenanceTypeCode { get; set; }

    public string? MaintenanceTypeName { get; set; }

    public string? MaintenanceTypeForeignName { get; set; }

    public int? IdWorkOrderCategory { get; set; }

    public decimal? EstimatedHours { get; set; }

    public int? IdUnit { get; set; }

    public string? UnitName { get; set; }

    public string? UnitForeignName { get; set; }

    public string? UnitCode { get; set; }
}
