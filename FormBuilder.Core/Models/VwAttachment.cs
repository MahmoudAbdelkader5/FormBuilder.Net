using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwAttachment
{
    public int? IdWorkOrder { get; set; }

    public string? WorkOrderName { get; set; }

    public string? WorkOrderForeignName { get; set; }

    public string? WorkOrderDocumentNumber { get; set; }

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? IdObject { get; set; }

    public string? Path { get; set; }

    public string? Description { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdAttachmentsType { get; set; }

    public string? AttachmentsName { get; set; }

    public string? AttachmentsTypeForeign { get; set; }

    public int? IdObjectType { get; set; }

    public string? ObjectTypeName { get; set; }

    public string? ObjectTypeForeignName { get; set; }

    public int IdCreatedBy { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public int? IdAsset { get; set; }

    public string? AssetCode { get; set; }

    public string? AssetName { get; set; }

    public string? AssetForeignName { get; set; }

    public int? IdMaintenanceType { get; set; }

    public string? MaintenanceTypeCode { get; set; }

    public string? MaintenanceTypeName { get; set; }

    public string? MaintenanceTypeForeignName { get; set; }
}
