using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwUserGroupMenu
{
    public int IdUserGroup { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ForeignDescription { get; set; } = null!;

    public bool IsActive { get; set; }
}
