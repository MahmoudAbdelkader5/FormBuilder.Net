using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblCancellationProcess
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public int IdSalesContract { get; set; }

    public int IdCancellationType { get; set; }

    public DateTime CancellationDate { get; set; }

    public int? IdApprovalStatus { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public int NoticePeriodDays { get; set; }

    public int? IdAdjustmentType { get; set; }

    public string? Reason { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public decimal? AdjustmentAmount { get; set; }

    public decimal? IdPeriodBase { get; set; }

    public decimal? PenaltyPropertyProgress { get; set; }

    public int? PenaltyNumberOfDays { get; set; }

    public decimal? PaidAmount { get; set; }

    public decimal? ContractAmount { get; set; }

    public int IdDocumentStatus { get; set; }

    public decimal? SecurityDeposit { get; set; }

    public decimal? PenaltyTotal { get; set; }

    public virtual TblCancellationType IdCancellationTypeNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblSalesContract IdSalesContractNavigation { get; set; } = null!;

    public virtual ICollection<TblCancellationProcessChecklist> TblCancellationProcessChecklists { get; set; } = new List<TblCancellationProcessChecklist>();
}
