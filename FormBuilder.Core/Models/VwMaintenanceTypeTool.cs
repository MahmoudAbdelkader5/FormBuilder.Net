using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwMaintenanceTypeTool
{
    public string? ToolCode { get; set; }

    public string ToolName { get; set; } = null!;

    public string? ToolForeignName { get; set; }

    public int IdTool { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdMaintenanceType { get; set; }

    public string? MaintenanceTypeCode { get; set; }

    public string MaintenanceTypeName { get; set; } = null!;

    public string? MaintenanceTypeForeignName { get; set; }

    public int? IdWorkOrderCategory { get; set; }

    public string WorkOrderCategoryName { get; set; } = null!;

    public string? WorkOrderCategoryForeignName { get; set; }
}
