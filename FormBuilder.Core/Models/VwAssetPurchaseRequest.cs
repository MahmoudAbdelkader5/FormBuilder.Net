using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwAssetPurchaseRequest
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public string? StoreRemarks { get; set; }

    public int? IdApprovalStatus { get; set; }

    public string? ApprovalStatusName { get; set; }

    public string? ApprovalStatusForeignName { get; set; }

    public int? IdStoreStatus { get; set; }

    public string? StoreStatusName { get; set; }

    public string? StoreStatusForeignName { get; set; }

    public int IdTechncian { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }
}
