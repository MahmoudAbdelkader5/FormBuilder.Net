using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwAssetMaintenanceSchedule
{
    public int Id { get; set; }

    public int IdStatus { get; set; }

    public string? StatusName { get; set; }

    public string? ForeignStatusName { get; set; }

    public int IdFrequency { get; set; }

    public int FrequencyInterval { get; set; }

    public int? IdMeterType { get; set; }

    public DateTime? LastMaintenanceDate { get; set; }

    public DateTime? NextMaintenanceDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public decimal? LastMaintenanceReading { get; set; }

    public decimal? LastMeterReading { get; set; }

    public bool? IsScheduled { get; set; }

    public decimal? SchedulerLimit { get; set; }

    public int? AlertInterval { get; set; }

    public int AboutToNeedMaintenance { get; set; }

    public int ReadingNearAlert { get; set; }

    public decimal? NextMeterReading { get; set; }

    public int IdMaintenanceType { get; set; }

    public string? MTypeCode { get; set; }

    public string MTypeName { get; set; } = null!;

    public string? MTypeForeignName { get; set; }

    public string? MTypeDescription { get; set; }

    public string? MTypeForeignDescription { get; set; }

    public bool MTypeIsActive { get; set; }

    public int? AssetIdWorkOrder { get; set; }

    public string? WorkOrderDocumentNumber { get; set; }

    public string? WorkOrderName { get; set; }

    public string? WorkOrderForeignName { get; set; }

    public int IdAsset { get; set; }

    public string AssetCode { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public string? AssetForeignName { get; set; }

    public int? AssetType { get; set; }

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

    public int? AssetIdGroup { get; set; }

    public string? AssetGroupName { get; set; }

    public string? AssetGroupForeignName { get; set; }

    public int? AssetIdZone { get; set; }

    public string? ZoneCode { get; set; }

    public string? ZoneName { get; set; }

    public string? ZoneForeignName { get; set; }

    public int? MTypeIdWorkOrderCategory { get; set; }

    public string? WorkOrderCategoryName { get; set; }

    public string? WorkOrderCategoryForeignName { get; set; }

    public string? FrequencyName { get; set; }

    public string? FrequencyForeignName { get; set; }

    public string? MeterTypeName { get; set; }

    public string? MeterTypeForeignName { get; set; }

    public int? IdUnit { get; set; }

    public string? UnitName { get; set; }

    public string? UnitForeignName { get; set; }

    public string? UnitCode { get; set; }

    public int? IdParent { get; set; }

    public string? ParentCode { get; set; }

    public string? ParentName { get; set; }

    public string? ParentForeignName { get; set; }
}
