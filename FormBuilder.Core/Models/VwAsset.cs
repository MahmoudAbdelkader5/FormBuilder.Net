using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwAsset
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public string? Description { get; set; }

    public string? Barcode { get; set; }

    public string? ForeignDescription { get; set; }

    public string? SerialNumber { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsWarranty { get; set; }

    public DateTime? WarrantyExpiryDate { get; set; }

    public string? WarrantyRemark { get; set; }

    public bool? IsServiceContract { get; set; }

    public DateTime? ServiceContractExpiryDate { get; set; }

    public string? ServiceContractRemark { get; set; }

    public int? ManufactureYear { get; set; }

    public string? Vender { get; set; }

    public decimal? Cost { get; set; }

    public int? IdGroup { get; set; }

    public string? GroupCode { get; set; }

    public string? GroupName { get; set; }

    public string? GroupForeignName { get; set; }

    public int? IdZone { get; set; }

    public string? ZoneCode { get; set; }

    public string? ZoneName { get; set; }

    public string? ZoneForeignName { get; set; }

    public int? IdType { get; set; }

    public string? MeterTypeName { get; set; }

    public string? MeterTypeForeignName { get; set; }

    public int? IdManufacturer { get; set; }

    public string? ManufacturerCode { get; set; }

    public string? ManufacturerName { get; set; }

    public string? ManufacturerForeignName { get; set; }

    public int? IdCostCenter1 { get; set; }

    public string? CostCenter1Name { get; set; }

    public string? CostCenter1ForeignName { get; set; }

    public int? IdCostCenter2 { get; set; }

    public string? CostCenter2Name { get; set; }

    public string? CostCenter2ForeignName { get; set; }

    public int? IdCostCenter3 { get; set; }

    public string? CostCenter3Name { get; set; }

    public string? CostCenter3ForeignName { get; set; }

    public int? IdCostCenter4 { get; set; }

    public string? CostCenter4Name { get; set; }

    public string? CostCenter4ForeignName { get; set; }

    public int? IdCostCenter5 { get; set; }

    public string? CostCenter5Name { get; set; }

    public string? CostCenter5ForeignName { get; set; }

    public int? IdAssetStatus { get; set; }

    public string? StatusName { get; set; }

    public string? StatusForeignName { get; set; }

    public int? IdUnit { get; set; }

    public string? UnitName { get; set; }

    public string? UnitForeignName { get; set; }

    public string? UnitCode { get; set; }

    public int? IdMainGroup { get; set; }

    public string? MainGroupCode { get; set; }

    public string? MainGroupName { get; set; }

    public string? MainGroupForeignName { get; set; }

    public int? IdSubGroup1 { get; set; }

    public string? SubGroup1Code { get; set; }

    public string? SubGroup1Name { get; set; }

    public string? SubGroup1ForeignName { get; set; }

    public int? IdSubGroup2 { get; set; }

    public string? SubGroup2Code { get; set; }

    public string? SubGroup2Name { get; set; }

    public string? SubGroup2ForeignName { get; set; }

    public int? IdAssetType { get; set; }

    public string? AssetTypeName { get; set; }

    public string? AssetTypeForeignName { get; set; }

    public int? IdParent { get; set; }

    public string? ParentCode { get; set; }

    public string? ParentName { get; set; }

    public string? ParentForeignName { get; set; }
}
