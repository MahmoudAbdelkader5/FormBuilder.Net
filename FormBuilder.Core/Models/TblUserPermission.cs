using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblUserPermission
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? ForeignDescription { get; set; }

    public string? ScreenName { get; set; }

    public string? ForeignScreenName { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }
}
