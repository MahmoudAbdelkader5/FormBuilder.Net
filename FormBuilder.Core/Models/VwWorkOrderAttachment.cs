using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwWorkOrderAttachment
{
    public int IdWorkOrder { get; set; }

    public string WorkOrderName { get; set; } = null!;

    public string? WorkOrderForeignName { get; set; }

    public string? WorkOrderDocumentNumber { get; set; }

    public int? WorkOrderIdApprovalStatus { get; set; }

    public string WorkOrderApprovalStatusName { get; set; } = null!;

    public string? WorkOrderApprovalStatusForeignName { get; set; }

    public int IdAttachment { get; set; }

    public string AttachmentName { get; set; } = null!;

    public int? AttachmentIdObject { get; set; }

    public string? AttachmentPath { get; set; }

    public string? AttachmentDescription { get; set; }

    public bool AttachmentIsActive { get; set; }

    public DateTime AttachmentCreatedDate { get; set; }

    public int? AttachmentIdUpdatedBy { get; set; }

    public DateTime? AttachmentUpdatedDate { get; set; }

    public int? AttachmentIdAttachmentsType { get; set; }

    public string AttachmentTypeName { get; set; } = null!;

    public string? AttachmentTypeForeign { get; set; }

    public int? AttachmentIdObjectType { get; set; }

    public string ObjectTypeName { get; set; } = null!;

    public string? ObjectTypeForeignName { get; set; }

    public int AttachmentIdCreatedBy { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }
}
