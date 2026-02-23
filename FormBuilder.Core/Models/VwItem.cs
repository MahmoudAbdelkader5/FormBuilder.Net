using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwItem
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public int? IdLegalEntity { get; set; }

    public DateTime CreatedDate { get; set; }

    public decimal? QuantityInStock { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int IdCreatedBy { get; set; }

    public int? IdUpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public decimal Cost { get; set; }

    public bool? IsSerialManaged { get; set; }

    public string? Tax { get; set; }

    public int? IdItemsType { get; set; }

    public string ItemTypeName { get; set; } = null!;

    public string? ItemTypeForeignName { get; set; }

    public int? IdPreferrableVendor { get; set; }

    public string? PreferrableVendorCode { get; set; }

    public string? PreferrableVendorName { get; set; }

    public string? PreferrableVendorForeignName { get; set; }

    public int? IdItemGroup { get; set; }

    public string? ItemGroupCode { get; set; }

    public string ItemGroupName { get; set; } = null!;

    public string? ItemGroupForeignName { get; set; }

    public int? IdInventoryUnitMeasure { get; set; }

    public string? InventoryUnitMeasureCode { get; set; }

    public string InventoryUnitMeasureName { get; set; } = null!;

    public string? InventoryUnitMeasureForeignName { get; set; }
}
