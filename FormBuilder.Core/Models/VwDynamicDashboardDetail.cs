using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwDynamicDashboardDetail
{
    public int Id { get; set; }

    public int? Sort { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IdDynamicDashboard { get; set; }

    public string? DynamicDashboardName { get; set; }

    public string? DynamicDashboardForeignName { get; set; }

    public int? IdDashboard { get; set; }

    public string? DashboardName { get; set; }

    public string? DashboardXml { get; set; }

    public string? ForeignDashboardXml { get; set; }

    public int Height { get; set; }

    public int? IdDynamicReport { get; set; }

    public string ReportName { get; set; } = null!;

    public string? ReportForeignName { get; set; }

    public string ReportQuery { get; set; } = null!;

    public string? ReportQueryXml { get; set; }

    public string? ReportCategory { get; set; }

    public string? ReportForeignCategory { get; set; }

    public string ForeignReportQuery { get; set; } = null!;

    public string? ForeignReportQueryXml { get; set; }
}
