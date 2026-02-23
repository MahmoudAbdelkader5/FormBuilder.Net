using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwCancellationTypeChecklist
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public string? ForeignDescription { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool? IsFixedAmount { get; set; }

    public bool? IsDeductedFromPaidTo { get; set; }

    public decimal? Value { get; set; }

    public string? Remark { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdValueType { get; set; }

    public string? ValueTypeName { get; set; }

    public string? ValueTypeForeignName { get; set; }

    public int IdCancellationType { get; set; }

    public string CancellationTypeName { get; set; } = null!;

    public string? CancellationTypeForeignName { get; set; }

    public int? IdCategory { get; set; }

    public string? CategoryName { get; set; }

    public string? CategoryForeignName { get; set; }
}
