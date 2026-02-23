using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblCancellationType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? NoticePeriod { get; set; }

    public int? MaxDaysAllowed { get; set; }

    public int? IdPeriodBase { get; set; }

    public int? IdTransactionType { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsReturnSecurityDeposit { get; set; }

    public int? IdContractType { get; set; }

    public virtual TblContractType? IdContractTypeNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblCancellationProcess> TblCancellationProcesses { get; set; } = new List<TblCancellationProcess>();

    public virtual ICollection<TblCancellationTypeChecklist> TblCancellationTypeChecklists { get; set; } = new List<TblCancellationTypeChecklist>();

    public virtual ICollection<TblGlaccountDeterminationDetail> TblGlaccountDeterminationDetails { get; set; } = new List<TblGlaccountDeterminationDetail>();
}
