using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwItemPurchaseRequest
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public DateTime DocumentDate { get; set; }

    public DateTime ValidUntilDate { get; set; }

    public DateTime RequiredDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public string? Remarks { get; set; }

    public bool? IsIntegrated { get; set; }

    public string? IntegrationStatus { get; set; }

    public int? IdApprovalStatus { get; set; }

    public string? ApprovalStatusName { get; set; }

    public string? ApprovalStatusForeignName { get; set; }

    public int IdCreatedBy { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }
}
