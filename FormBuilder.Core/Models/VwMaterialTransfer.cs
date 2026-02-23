using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwMaterialTransfer
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public int? IdWorkOrder { get; set; }

    public int IdTechncian { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsIntegrated { get; set; }

    public DateTime? IssuedDate { get; set; }

    public string? IntegrationStatus { get; set; }

    public int? IdSap { get; set; }

    public int? IdApprovalStatus { get; set; }

    public string? ApprovalStatusName { get; set; }

    public string? ApprovalStatusForeignName { get; set; }

    public string? WorkOrderDocumentNumber { get; set; }

    public string? WorkOrderName { get; set; }

    public string? WorkOrderForeignName { get; set; }

    public DateTime? DocumentDate { get; set; }

    public DateTime? RequiredDate { get; set; }

    public DateTime? ClosingDate { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public int? IdRequester { get; set; }

    public string? RequesterName { get; set; }

    public string? RequesterForeignName { get; set; }

    public int? IdObjectType { get; set; }

    public string? ObjectTypeName { get; set; }

    public string? ObjectTypeForeignName { get; set; }

    public int? IdRequest { get; set; }

    public string? RequestCode { get; set; }
}
