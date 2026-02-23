using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwPropertyNeighborhood
{
    public int Id { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public string? Note { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int IdNeighborhood { get; set; }

    public string? NeighborhoodName { get; set; }

    public string? NeighborhoodForeignName { get; set; }

    public int IdProperty { get; set; }

    public string? PropertyName { get; set; }

    public string? PropertyForeignName { get; set; }

    public string? PropertyCode { get; set; }

    public string? PropertyNumber { get; set; }
}
