using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwWorkOrderRemark
{
    public int IdWorkOrder { get; set; }

    public string WorkOrderName { get; set; } = null!;

    public string? WorkOrderForeignName { get; set; }

    public int? WorkOrderIdApprovalStatus { get; set; }

    public string WorkOrderApprovalStatusName { get; set; } = null!;

    public string? WorkOrderApprovalStatusForeignName { get; set; }

    public int WorkOrderIdWorkOrderCategory { get; set; }

    public string WorkOrderCategoryName { get; set; } = null!;

    public string? WorkOrderCategoryForeignName { get; set; }

    public int IdRemark { get; set; }

    public int? IdObject { get; set; }

    public string? Description { get; set; }

    public int? IdLegalEntity { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public int? RemarkIdObjectType { get; set; }

    public string ObjectTypeName { get; set; } = null!;

    public string? ObjectTypeForeignName { get; set; }

    public int RemarkIdCreatedBy { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public string UserUsername { get; set; } = null!;

    public string? UserEmail { get; set; }

    public string UserPassword { get; set; } = null!;

    public string? UserPhone { get; set; }
}
