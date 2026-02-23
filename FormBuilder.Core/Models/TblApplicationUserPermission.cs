using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblApplicationUserPermission
{
    public int IdApplication { get; set; }

    public string UserPermissionName { get; set; } = null!;
}
