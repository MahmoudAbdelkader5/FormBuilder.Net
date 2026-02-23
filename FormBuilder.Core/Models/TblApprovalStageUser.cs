using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblApprovalStageUser
{
    public int IdApprovalStage { get; set; }

    public int IdUser { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int Sort { get; set; }

    public virtual TblApprovalStage IdApprovalStageNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblUser IdUserNavigation { get; set; } = null!;
}
