using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwWorkOrderTechnician
{
    public int Id { get; set; }

    public decimal? ActualHours { get; set; }

    public int? IdMaintenanceType { get; set; }

    public string? MTypeCode { get; set; }

    public string MTypeName { get; set; } = null!;

    public string? MTypeForeignName { get; set; }

    public string? MTypeDescription { get; set; }

    public string? MTypeForeignDescription { get; set; }

    public bool MTypeIsActive { get; set; }

    public decimal? EstimatedHours { get; set; }

    public int IdTechnician { get; set; }

    public string UserUsername { get; set; } = null!;

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public bool UserIsActive { get; set; }

    public decimal? UserRatePerHour { get; set; }

    public int IdWorkOrder { get; set; }

    public string WorkOrderName { get; set; } = null!;

    public string? WorkOrderForeignName { get; set; }

    public string? WorkOrderDocumentNumber { get; set; }

    public int IdAsset { get; set; }

    public string AssetCode { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public string? AssetForeignName { get; set; }

    public int? AssetType { get; set; }

    public int? AssetIdGroup { get; set; }

    public int? AssetIdZone { get; set; }

    public int? AssetIdCostCenter1 { get; set; }

    public int? AssetIdCostCenter2 { get; set; }

    public int? AssetIdCostCenter3 { get; set; }

    public int? AssetIdCostCenter4 { get; set; }

    public int? AssetIdCostCenter5 { get; set; }

    public string? AssetBarcode { get; set; }

    public string? AssetDescription { get; set; }

    public string? AssetForeignDescription { get; set; }

    public string? AssetSerialNumber { get; set; }

    public int? AssetIdManufacturer { get; set; }

    public int WorkOrderIdWorkOrderCategory { get; set; }

    public string WorkOrderCategoryName { get; set; } = null!;

    public string? WorkOrderCategoryForeignName { get; set; }

    public int? MTypeIdWorkOrderCategory { get; set; }

    public int? WorkOrderIdApprovalStatus { get; set; }

    public string WorkOrderApprovalStatusName { get; set; } = null!;

    public string? WorkOrderApprovalStatusForeignName { get; set; }

    public DateOnly? TimesheetDate { get; set; }

    public TimeOnly? TimesheetFromTime { get; set; }

    public TimeOnly? TimesheetToTime { get; set; }
}
