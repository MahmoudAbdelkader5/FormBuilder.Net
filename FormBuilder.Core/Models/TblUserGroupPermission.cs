using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblUserGroupPermission
{
    public int IdUserGroup { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string UserPermissionName { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblUserGroup IdUserGroupNavigation { get; set; } = null!;
}
