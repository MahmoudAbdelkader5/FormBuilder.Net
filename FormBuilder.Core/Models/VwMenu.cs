using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwMenu
{
    public int Id { get; set; }

    public int IdApplicationType { get; set; }

    public int? IdParent { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ForeignDescription { get; set; } = null!;

    public int Sort { get; set; }

    public string? Url { get; set; }

    public string? Cssclass { get; set; }

    public bool IsActive { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public string? ParentName { get; set; }

    public string? ParentDescription { get; set; }

    public string? ParentForeignDescription { get; set; }

    public int? ParentSort { get; set; }

    public bool? ParentIsActive { get; set; }
}
