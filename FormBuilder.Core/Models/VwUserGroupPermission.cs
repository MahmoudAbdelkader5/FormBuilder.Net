using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwUserGroupPermission
{
    public int IdUserGroup { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? ForeignDescription { get; set; }

    public string? ScreenName { get; set; }

    public string? ForeignScreenName { get; set; }

    public bool IsActive { get; set; }
}
