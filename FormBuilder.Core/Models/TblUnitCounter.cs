using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblUnitCounter
{
    public int Id { get; set; }

    public int? IdUnit { get; set; }

    public string? Count { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? CounterDate { get; set; }

    public int? IdExpenseType { get; set; }

    public string? Remarks { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblUnit? IdUnitNavigation { get; set; }
}
