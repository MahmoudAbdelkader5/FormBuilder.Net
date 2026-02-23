using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwJournalEntry
{
    public int Id { get; set; }

    public string TransactionCode { get; set; } = null!;

    public DateTime? Date { get; set; }

    public DateTime? DueDate { get; set; }

    public string? Remarks { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int IdUnit { get; set; }

    public string? UnitCode { get; set; }

    public string UnitName { get; set; } = null!;

    public string? UnitForeignName { get; set; }

    public int IdCustomer { get; set; }

    public string CustomerFirstName { get; set; } = null!;

    public string? CustomerForeignFirstName { get; set; }

    public string? CustomerLastName { get; set; }

    public string? CustomerForeignLastName { get; set; }

    public string? CustomerCode { get; set; }

    public string? CustomerMiddleName { get; set; }

    public string? CustomerForeignMiddleName { get; set; }

    public bool? CustomerIsBlackList { get; set; }

    public bool IsPosted { get; set; }

    public decimal? Jeamount { get; set; }
}
