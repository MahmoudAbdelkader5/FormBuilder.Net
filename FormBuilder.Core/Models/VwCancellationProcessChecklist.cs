using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwCancellationProcessChecklist
{
    public int Id { get; set; }

    public bool IsChecked { get; set; }

    public DateTime? CheckingDate { get; set; }

    public bool IsFixedAmount { get; set; }

    public decimal Amount { get; set; }

    public string? Remarks { get; set; }

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsDeductedFromPaidTo { get; set; }

    public int IdCancellationProcess { get; set; }

    public string CancellationProcessCode { get; set; } = null!;

    public int? IdCategory { get; set; }

    public string? CategoryName { get; set; }

    public string? CategoryForeignName { get; set; }

    public int? IdCancellationTypeChecklist { get; set; }

    public string? CancellationTypeChecklistDescription { get; set; }

    public string? CancellationTypeChecklistForeignDescription { get; set; }
}
