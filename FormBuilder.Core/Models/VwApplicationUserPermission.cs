using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwApplicationUserPermission
{
    public int IdApplication { get; set; }

    public string ApplicationName { get; set; } = null!;

    public string UserPermissionName { get; set; } = null!;

    public string? UserPermissionDescription { get; set; }

    public string? UserPermissionForeignDescription { get; set; }

    public string? UserPermissionScreenName { get; set; }

    public string? UserPermissionForeignScreenName { get; set; }
}
