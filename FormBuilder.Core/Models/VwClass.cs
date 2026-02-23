using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwClass
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public int? IdParent { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? ParentName { get; set; }

    public string? ForeignParentName { get; set; }
}
