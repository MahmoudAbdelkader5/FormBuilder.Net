using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblApprovalProcess
{
    public int Id { get; set; }

    public int IdApprovalType { get; set; }

    public int IdApprovalStatus { get; set; }

    public int? IdObjectType { get; set; }

    public int? IdObject { get; set; }

    public int? IdNewObject { get; set; }

    public int IdApprovalStage { get; set; }

    public int IdCurrentUser { get; set; }

    public int IdApprovalTemplate { get; set; }

    public int? IdApprovalTemplateTerm { get; set; }

    public int? IdApprovalTransaction { get; set; }

    public int? IdLegalEntity { get; set; }

    public int NumberOfApprovalRequired { get; set; }

    public int NumberOfRejectionRequired { get; set; }

    public int? CurrentNumberOfApproval { get; set; }

    public int? CurrentNumberOfRejection { get; set; }

    public bool IsSequential { get; set; }

    public decimal? Value { get; set; }

    public DateTime? LastUpdatedDate { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual TblApprovalStage IdApprovalStageNavigation { get; set; } = null!;

    public virtual TblApprovalTemplate IdApprovalTemplateNavigation { get; set; } = null!;

    public virtual TblUser IdCurrentUserNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblApprovalAlert> TblApprovalAlerts { get; set; } = new List<TblApprovalAlert>();
}
