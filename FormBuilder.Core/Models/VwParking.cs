using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwParking
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public string SpaceNumber { get; set; } = null!;

    public string? Floor { get; set; }

    public string LevelNumber { get; set; } = null!;

    public int? IdLegalEntity { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int IdCreatedBy { get; set; }

    public int? IdUpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public int? IdAvailability { get; set; }

    public string? AvailabilityName { get; set; }

    public string? AvailabilityForeignName { get; set; }

    public int? IdProperty { get; set; }

    public string? PropertyName { get; set; }

    public string? PropertyForeignName { get; set; }

    public string? PropertyCode { get; set; }
}
