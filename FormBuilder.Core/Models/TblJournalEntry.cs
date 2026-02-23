using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblJournalEntry
{
    public int Id { get; set; }

    public string TransactionCode { get; set; } = null!;

    public int IdUnit { get; set; }

    public int IdCustomer { get; set; }

    public DateTime? Date { get; set; }

    public DateTime? DueDate { get; set; }

    public string? Remarks { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdReference { get; set; }

    public bool IsPosted { get; set; }

    public virtual TblCustomer IdCustomerNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblUnit IdUnitNavigation { get; set; } = null!;

    public virtual ICollection<TblJournalEntryDetail> TblJournalEntryDetails { get; set; } = new List<TblJournalEntryDetail>();
}
