using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblDynamicDashboard
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public string? Category { get; set; }

    public string? ForeignCategory { get; set; }

    public int? Sort { get; set; }

    public int? IdMenu { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblMenu? IdMenuNavigation { get; set; }

    public virtual ICollection<TblDynamicDashboardDetail> TblDynamicDashboardDetails { get; set; } = new List<TblDynamicDashboardDetail>();
}
