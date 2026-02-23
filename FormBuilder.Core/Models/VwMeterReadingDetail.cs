using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwMeterReadingDetail
{
    public int Id { get; set; }

    public decimal? ReadingValue { get; set; }

    public string? Remark { get; set; }

    public bool IsActive { get; set; }

    public int? IdMeterReading { get; set; }

    public string? MeterReadingDocumentNumber { get; set; }

    public DateTime MeterReadingPostingDate { get; set; }

    public int? IdAsset { get; set; }

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

    public int? IdMeterType { get; set; }

    public string? MeterTypeCode { get; set; }

    public string MeterTypeName { get; set; } = null!;

    public string? MeterTypeForeignName { get; set; }

    public bool MeterTypeIsActive { get; set; }

    public int? IdMaintenanceType { get; set; }

    public string? MaintenanceTypeCode { get; set; }

    public string? MaintenanceTypeName { get; set; }

    public string? MaintenanceTypeForeignName { get; set; }

    public int IdCreatedBy { get; set; }

    public string? CreatedByName { get; set; }

    public string? CreatedByForeignName { get; set; }
}
