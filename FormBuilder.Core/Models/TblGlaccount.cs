using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblGlaccount
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public int? IdParentAccount { get; set; }

    public bool? IsPostable { get; set; }

    public bool? IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IdType { get; set; }

    public bool? IsControlAccount { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblGlaccount? IdParentAccountNavigation { get; set; }

    public virtual ICollection<TblGlaccount> InverseIdParentAccountNavigation { get; set; } = new List<TblGlaccount>();

    public virtual ICollection<TblBank> TblBankIdDefaultAccountNavigations { get; set; } = new List<TblBank>();

    public virtual ICollection<TblBank> TblBankIdPdcaccountNavigations { get; set; } = new List<TblBank>();

    public virtual ICollection<TblCustomer> TblCustomers { get; set; } = new List<TblCustomer>();

    public virtual ICollection<TblGlaccountDeterminationDetail> TblGlaccountDeterminationDetails { get; set; } = new List<TblGlaccountDeterminationDetail>();

    public virtual ICollection<TblGlaccountDetermination> TblGlaccountDeterminations { get; set; } = new List<TblGlaccountDetermination>();

    public virtual ICollection<TblIncomingPaymentAccount> TblIncomingPaymentAccounts { get; set; } = new List<TblIncomingPaymentAccount>();

    public virtual ICollection<TblIncomingPaymentCash> TblIncomingPaymentCashes { get; set; } = new List<TblIncomingPaymentCash>();

    public virtual ICollection<TblIncomingPaymentCheque> TblIncomingPaymentCheques { get; set; } = new List<TblIncomingPaymentCheque>();

    public virtual ICollection<TblIncomingPaymentTransfer> TblIncomingPaymentTransfers { get; set; } = new List<TblIncomingPaymentTransfer>();

    public virtual ICollection<TblJournalEntryDetail> TblJournalEntryDetails { get; set; } = new List<TblJournalEntryDetail>();
}
