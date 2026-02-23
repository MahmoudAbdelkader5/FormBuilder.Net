using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwUnitCounter
{
    public int Id { get; set; }

    public string? Count { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? CounterDate { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public string? Remarks { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdUnit { get; set; }

    public string UnitName { get; set; } = null!;

    public string? UnitForeignName { get; set; }

    public string? UnitCode { get; set; }

    public int? IdExpenseType { get; set; }

    public string ExpenseTypeName { get; set; } = null!;

    public string? ExpenseTypeForeignName { get; set; }
}
