using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwWorkOrderHseatmosphericMonitoring
{
    public int? ActualReading { get; set; }

    public DateTime? Time { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int IdAtmosphericMonitoring { get; set; }

    public string HseatmosphericMonitoringName { get; set; } = null!;

    public string? HseatmosphericMonitoringForeignName { get; set; }

    public string? HseatmosphericMonitoringRemark { get; set; }

    public bool HseatmosphericMonitoringIsActive { get; set; }

    public int IdSafeLimit { get; set; }

    public string SafeLimitName { get; set; } = null!;

    public string? SafeLimitForeignName { get; set; }

    public bool SafeLimitIsActive { get; set; }

    public int IdWorkOrder { get; set; }

    public string WorkOrderName { get; set; } = null!;

    public string? WorkOrderForeignName { get; set; }

    public string? WorkOrderDocumentNumber { get; set; }
}
