using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblDynamicReport
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public string ReportQuery { get; set; } = null!;

    public string? ReportQueryXml { get; set; }

    public string? Category { get; set; }

    public string? ForeignCategory { get; set; }

    public string ForeignReportQuery { get; set; } = null!;

    public string? ForeignReportQueryXml { get; set; }

    public int? IdMenu { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblMenu? IdMenuNavigation { get; set; }

    public virtual ICollection<TblDashboard> TblDashboards { get; set; } = new List<TblDashboard>();
}
