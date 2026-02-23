using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblAssetPurchaseRequestAsset
{
    public int Id { get; set; }

    public string? AssetDescription { get; set; }

    public string? SerialNumber { get; set; }

    public int? IdSupplier { get; set; }

    public int? IdManufacturer { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IdAssetPurchaseRequest { get; set; }

    public string? UnitOfMeasure { get; set; }

    public string? Remarks { get; set; }

    public int? IdItem { get; set; }

    public decimal? Quantity { get; set; }

    public virtual TblAssetPurchaseRequest? IdAssetPurchaseRequestNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblManufacturer? IdManufacturerNavigation { get; set; }

    public virtual TblSupplier? IdSupplierNavigation { get; set; }
}
