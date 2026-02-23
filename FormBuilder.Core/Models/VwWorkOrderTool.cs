using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwWorkOrderTool
{
    public string? ToolCode { get; set; }

    public string ToolName { get; set; } = null!;

    public string? ToolForeignName { get; set; }

    public int IdTool { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdWorkOrder { get; set; }

    public string WorkOrderName { get; set; } = null!;

    public string? WorkOrderForeignName { get; set; }

    public string? WorkOrderDocumentNumber { get; set; }

    public int WorkOrderIdWorkOrderCategory { get; set; }

    public string WorkOrderCategoryName { get; set; } = null!;

    public string? WorkOrderCategoryForeignName { get; set; }

    public long WorkOrderReferenceNumber { get; set; }

    public int? WorkOrderIdApprovalStatus { get; set; }

    public string WorkOrderApprovalStatusName { get; set; } = null!;

    public string? WorkOrderApprovalStatusForeignName { get; set; }
}
