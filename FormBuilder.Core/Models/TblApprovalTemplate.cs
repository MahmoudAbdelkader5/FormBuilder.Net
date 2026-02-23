using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblApprovalTemplate
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public string? Description { get; set; }

    public string? Query { get; set; }

    public int IdApprovalStage { get; set; }

    public int IdApprovalType { get; set; }

    public bool IsCustom { get; set; }

    public bool IsSequential { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual TblApprovalStage IdApprovalStageNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblApprovalAlert> TblApprovalAlerts { get; set; } = new List<TblApprovalAlert>();

    public virtual ICollection<TblApprovalProcess> TblApprovalProcesses { get; set; } = new List<TblApprovalProcess>();

    public virtual ICollection<TblApprovalTemplateTerm> TblApprovalTemplateTerms { get; set; } = new List<TblApprovalTemplateTerm>();

    public virtual ICollection<TblApprovalTemplateUser> TblApprovalTemplateUsers { get; set; } = new List<TblApprovalTemplateUser>();
}
