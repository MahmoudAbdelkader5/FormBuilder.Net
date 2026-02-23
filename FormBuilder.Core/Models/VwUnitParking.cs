using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwUnitParking
{
    public int Id { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int IdParking { get; set; }

    public string ParkingName { get; set; } = null!;

    public string? ParkingForeignName { get; set; }

    public int IdUnit { get; set; }

    public string UnitName { get; set; } = null!;

    public string? UnitForeignName { get; set; }

    public string? UnitCode { get; set; }

    public string UnitNumber { get; set; } = null!;

    public int? UnitIdParent { get; set; }
}
