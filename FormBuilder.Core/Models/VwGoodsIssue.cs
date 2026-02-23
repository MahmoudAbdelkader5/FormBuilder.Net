using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwGoodsIssue
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

    public int? IdApprovalStatus { get; set; }

    public string? StatusName { get; set; }

    public string? StatusForeignName { get; set; }

    public string? WorkOrderName { get; set; }

    public string? WorkOrderForeignName { get; set; }

    public DateTime? DocumentDate { get; set; }

    public DateTime? RequiredDate { get; set; }

    public DateTime? ClosingDate { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }
}
