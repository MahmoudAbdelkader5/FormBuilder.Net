using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblCancellationTypeChecklist
{
    public int Id { get; set; }

    public int IdCancellationType { get; set; }

    public string Description { get; set; } = null!;

    public string? ForeignDescription { get; set; }

    public bool? IsFixedAmount { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public decimal? Value { get; set; }

    public int? IdValueType { get; set; }

    public string? Remark { get; set; }

    public int? IdCategory { get; set; }

    public bool? IsDeductedFromPaidTo { get; set; }

    public virtual TblCancellationType IdCancellationTypeNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblCancellationProcessChecklist> TblCancellationProcessChecklists { get; set; } = new List<TblCancellationProcessChecklist>();

    public virtual ICollection<TblGlaccountDeterminationDetail> TblGlaccountDeterminationDetails { get; set; } = new List<TblGlaccountDeterminationDetail>();
}
