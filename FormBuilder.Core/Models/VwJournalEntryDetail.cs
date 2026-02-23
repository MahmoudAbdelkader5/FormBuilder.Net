using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwJournalEntryDetail
{
    public int Id { get; set; }

    public decimal? Debit { get; set; }

    public decimal? Credit { get; set; }

    public DateTime? DueDate { get; set; }

    public string? Remarks { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int IdJournalEntry { get; set; }

    public string JournalEntryTransactionCode { get; set; } = null!;

    public DateTime? JournalEntryDate { get; set; }

    public DateTime? JournalEntryDueDate { get; set; }

    public int IdGlaccount { get; set; }

    public string? GlaccountName { get; set; }

    public string? GlaccountForeignName { get; set; }

    public string? GlaccountCode { get; set; }

    public string? CustomerCode { get; set; }

    public string FirstName { get; set; } = null!;

    public string? ForeignFirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? ForeignMiddleName { get; set; }

    public string? LastName { get; set; }

    public string? ForeignLastName { get; set; }

    public int? IdReference { get; set; }

    public int? IdReferenceDetail { get; set; }
}
