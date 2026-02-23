using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwApprovalTemplateTerm
{
    public int IdApprovalTemplate { get; set; }

    public DateTime CreatedDate { get; set; }

    public int IdCreatedBy { get; set; }

    public int? IdLegalEntity { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public decimal? FromValue { get; set; }

    public decimal? ToValue { get; set; }

    public int IdApprovalRatio { get; set; }

    public string RatioName { get; set; } = null!;

    public string? RatioForeignName { get; set; }

    public int IdApprovalTerm { get; set; }

    public string TermName { get; set; } = null!;

    public string? TermForeignName { get; set; }
}
