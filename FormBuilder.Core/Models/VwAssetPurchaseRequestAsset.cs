using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwAssetPurchaseRequestAsset
{
    public int Id { get; set; }

    public string? AssetDescription { get; set; }

    public string? SerialNumber { get; set; }

    public decimal? Quantity { get; set; }

    public string? UnitOfMeasure { get; set; }

    public string? Remarks { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IdAssetPurchaseRequest { get; set; }

    public string AssetPurchaseRequestCode { get; set; } = null!;

    public int? IdSupplier { get; set; }

    public string? SupplierName { get; set; }

    public string? SupplierForeignName { get; set; }

    public string? SupplierCode { get; set; }

    public int? IdItem { get; set; }

    public string? ItemName { get; set; }

    public string? ItemForeignName { get; set; }

    public string? ItemCode { get; set; }

    public int? IdManufacturer { get; set; }

    public string? ManufacturerName { get; set; }

    public string? ManufacturerForeignName { get; set; }

    public string? ManufacturerCode { get; set; }
}
