using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwDashboard
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public string? DashboardXml { get; set; }

    public string? ForeignDashboardXml { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? Height { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdDynamicReport { get; set; }

    public string? ReportName { get; set; }

    public string? ReportForeignName { get; set; }

    public string? ReportQuery { get; set; }

    public string? ReportQueryXml { get; set; }

    public string? ReportCategory { get; set; }

    public string? ReportForeignCategory { get; set; }

    public string? ForeignReportQuery { get; set; }

    public string? ForeignReportQueryXml { get; set; }
}
