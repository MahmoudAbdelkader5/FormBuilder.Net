using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwAssetTransfer
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public int? IdLegalEntity { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdApprovalStatus { get; set; }

    public string? ApprovalStatusName { get; set; }

    public string? ApprovalStatusForeignName { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }
}
