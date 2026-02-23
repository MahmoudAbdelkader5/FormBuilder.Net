using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblApprovalAlert
{
    public int Id { get; set; }

    public string? Subject { get; set; }

    public string? Description { get; set; }

    public DateTime AlertDate { get; set; }

    public int? IdUser { get; set; }

    public int IdApprovalTemplate { get; set; }

    public int? IdApprovalObject { get; set; }

    public int? IdApprovalProcess { get; set; }

    public int IdApprovalStatus { get; set; }

    public int IdApprovalType { get; set; }

    public DateTime? StatusDate { get; set; }

    public string? Note { get; set; }

    public int? IdLegalEntity { get; set; }

    public string? ForeignDescription { get; set; }

    public string? ForeignSubject { get; set; }

    public int? IdObjectType { get; set; }

    public virtual TblApprovalProcess? IdApprovalProcessNavigation { get; set; }

    public virtual TblApprovalTemplate IdApprovalTemplateNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblUser? IdUserNavigation { get; set; }
}
