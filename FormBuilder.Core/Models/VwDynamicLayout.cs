using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwDynamicLayout
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public string Description { get; set; } = null!;

    public string? ForeignDescription { get; set; }

    public string? LayoutPath { get; set; }

    public bool? IsSystem { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdLayoutType { get; set; }

    public string LayoutTypeName { get; set; } = null!;

    public string? LayoutTypeForeignName { get; set; }
}
