using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblApprovalTemplateTerm
{
    public int IdApprovalTemplate { get; set; }

    public int IdApprovalTerm { get; set; }

    public int IdApprovalRatio { get; set; }

    public decimal? FromValue { get; set; }

    public decimal? ToValue { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual TblApprovalTemplate IdApprovalTemplateNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }
}
