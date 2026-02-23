using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblMenu
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

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public string? UserPermissionName { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblMenu? IdParentNavigation { get; set; }

    public virtual ICollection<TblMenu> InverseIdParentNavigation { get; set; } = new List<TblMenu>();

    public virtual ICollection<TblDynamicDashboard> TblDynamicDashboards { get; set; } = new List<TblDynamicDashboard>();

    public virtual ICollection<TblDynamicReport> TblDynamicReports { get; set; } = new List<TblDynamicReport>();

    public virtual ICollection<TblUserGroupMenu> TblUserGroupMenus { get; set; } = new List<TblUserGroupMenu>();
}
