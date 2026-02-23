using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwApplicationDynamicReport
{
    public int IdApplication { get; set; }

    public string ApplicationName { get; set; } = null!;

    public int IdDynamicReport { get; set; }

    public string DynamicReportName { get; set; } = null!;

    public string? DynamicReportForeignName { get; set; }

    public string? DynamicReportCategory { get; set; }
}
