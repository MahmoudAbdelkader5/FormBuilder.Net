using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwApprovalAlert
{
    public int Id { get; set; }

    public string? Subject { get; set; }

    public string? Description { get; set; }

    public string? ForeignDescription { get; set; }

    public DateTime AlertDate { get; set; }

    public int IdApprovalTemplate { get; set; }

    public int? IdApprovalObject { get; set; }

    public int? IdApprovalProcess { get; set; }

    public int? IdObjectType { get; set; }

    public int? IdUser { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public DateTime? StatusDate { get; set; }

    public string? Note { get; set; }

    public int? IdLegalEntity { get; set; }

    public string? ForeignSubject { get; set; }

    public int IdApprovalType { get; set; }

    public string TypeName { get; set; } = null!;

    public string? TypeForeignName { get; set; }

    public int IdApprovalStatus { get; set; }

    public string StatusName { get; set; } = null!;

    public string? StatusForeignName { get; set; }

    public string? CreatorName { get; set; }

    public string? CreatorForeignName { get; set; }

    public string? UnitCodeName { get; set; }
}
