using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblJournalEntryDetail
{
    public int Id { get; set; }

    public int IdJournalEntry { get; set; }

    public int IdGlaccount { get; set; }

    public decimal? Debit { get; set; }

    public decimal? Credit { get; set; }

    public DateTime? DueDate { get; set; }

    public string? Remarks { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdReferenceDetail { get; set; }

    public virtual TblGlaccount IdGlaccountNavigation { get; set; } = null!;

    public virtual TblJournalEntry IdJournalEntryNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }
}
