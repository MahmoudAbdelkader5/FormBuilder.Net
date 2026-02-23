using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwPropertyFacility
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Note { get; set; }

    public int IdFacility { get; set; }

    public string? FacilityName { get; set; }

    public string? FacilityForeignName { get; set; }

    public int IdProperty { get; set; }

    public string? PropertyName { get; set; }

    public string? PropertyForeignName { get; set; }

    public string? PropertyCode { get; set; }

    public string? PropertyNumber { get; set; }
}
