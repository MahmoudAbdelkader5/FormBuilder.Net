using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwDocumentParking
{
    public int? IdLegalEntity { get; set; }

    public DateTime CreatedDate { get; set; }

    public int IdDocument { get; set; }

    public int IdCreatedBy { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public int IdParking { get; set; }

    public string? ParkingName { get; set; }

    public string? ParkingForeignName { get; set; }

    public string? Code { get; set; }

    public string? Floor { get; set; }

    public string? SpaceNumber { get; set; }

    public string? LevelNumber { get; set; }

    public int? IdProperty { get; set; }

    public string? PropertyName { get; set; }

    public string? PropertyForeignName { get; set; }

    public string? PropertyCode { get; set; }

    public string? AvailabilityName { get; set; }

    public string? AvailabilityForeignName { get; set; }

    public int IdObjectType { get; set; }

    public string ObjectTypeName { get; set; } = null!;

    public string? ObjectTypeForeignName { get; set; }
}
