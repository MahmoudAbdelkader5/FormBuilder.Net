using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwMaintenanceTypeItem
{
    public string? ItemCode { get; set; }

    public string ItemName { get; set; } = null!;

    public string? ItemForeignName { get; set; }

    public int Id { get; set; }

    public int IdItem { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdMaintenanceType { get; set; }

    public string? MaintenanceTypeCode { get; set; }

    public string MaintenanceTypeName { get; set; } = null!;

    public string? MaintenanceTypeForeignName { get; set; }

    public int? IdWorkOrderCategory { get; set; }

    public string WorkOrderCategoryName { get; set; } = null!;

    public string? WorkOrderCategoryForeignName { get; set; }

    public decimal? Quantity { get; set; }

    public decimal ItemCost { get; set; }

    public int? IdItemsType { get; set; }

    public string ItemTypeName { get; set; } = null!;

    public string? ItemTypeForeignName { get; set; }

    public int? IdItemGroup { get; set; }

    public string? ItemGroupCode { get; set; }

    public string ItemGroupName { get; set; } = null!;

    public string? ItemGroupForeignName { get; set; }

    public int? IdInventoryUnitMeasure { get; set; }

    public string? InventoryUnitMeasureCode { get; set; }

    public string InventoryUnitMeasureName { get; set; } = null!;

    public string? InventoryUnitMeasureForeignName { get; set; }
}
