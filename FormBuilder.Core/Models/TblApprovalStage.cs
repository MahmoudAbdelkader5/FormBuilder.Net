using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblApprovalStage
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ForeignName { get; set; } = null!;

    public string? Description { get; set; }

    public int NumberOfApprovalRequired { get; set; }

    public int NumberOfRejectionRequired { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblApprovalProcess> TblApprovalProcesses { get; set; } = new List<TblApprovalProcess>();

    public virtual ICollection<TblApprovalStageUser> TblApprovalStageUsers { get; set; } = new List<TblApprovalStageUser>();

    public virtual ICollection<TblApprovalTemplate> TblApprovalTemplates { get; set; } = new List<TblApprovalTemplate>();
}
