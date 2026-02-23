using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblDashboard
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public string? DashboardXml { get; set; }

    public string? ForeignDashboardXml { get; set; }

    public int? Height { get; set; }

    public int? IdDynamicReport { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblDynamicReport? IdDynamicReportNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblDynamicDashboardDetail> TblDynamicDashboardDetails { get; set; } = new List<TblDynamicDashboardDetail>();
}
