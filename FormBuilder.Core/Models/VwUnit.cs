using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwUnit
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public string Number { get; set; } = null!;

    public string? FlatNumber { get; set; }

    public bool IsAllowSale { get; set; }

    public bool IsAllowRent { get; set; }

    public bool IsAllowLease { get; set; }

    public string? Floor { get; set; }

    public int? NumberOfBedrooms { get; set; }

    public int? NumberOfBathrooms { get; set; }

    public int? NumberOfParkings { get; set; }

    public string? SquareFeet { get; set; }

    public string? Width { get; set; }

    public string? Depth { get; set; }

    public int? IdLegalEntity { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int IdCreatedBy { get; set; }

    public int? IdUpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public int? IdLastMergedBy { get; set; }

    public DateTime? LastMergedDate { get; set; }

    public string? UnitLink { get; set; }

    public int? IdSalesEmployee { get; set; }

    public string? SalesEmployeeName { get; set; }

    public string? SalesEmployeeForeignName { get; set; }

    public int? IdUnitStatus { get; set; }

    public string? UnitStatusName { get; set; }

    public string? UnitStatusForeignName { get; set; }

    public int? IdAvailability { get; set; }

    public string? AvailabilityName { get; set; }

    public string? AvailabilityForeignName { get; set; }

    public int? IdUnitView { get; set; }

    public string? UnitViewName { get; set; }

    public string? UnitViewForeignName { get; set; }

    public int? IdUnitUse { get; set; }

    public string? UnitUseName { get; set; }

    public string? UnitUseForeignName { get; set; }

    public int? IdUnitClass { get; set; }

    public string? UnitClassName { get; set; }

    public string? UnitClassForeignName { get; set; }

    public int? IdSalesCalculationBase { get; set; }

    public string? CalculationBaseName { get; set; }

    public string? CalculationBaseForeignName { get; set; }

    public int? IdProperty { get; set; }

    public string? PropertyName { get; set; }

    public string? PropertyForeignName { get; set; }

    public string? PropertyCode { get; set; }

    public int? IdMergeType { get; set; }

    public string? MergeTypeName { get; set; }

    public string? MergeTypeForeignName { get; set; }

    public int? IdParent { get; set; }

    public string? ParentName { get; set; }

    public string? ParentForeignName { get; set; }
}
