using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblIncomingPayment
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public decimal? TaxAmount { get; set; }

    public decimal? TotalAmount { get; set; }

    public string Code { get; set; } = null!;

    public string? Remarks { get; set; }

    public DateTime PostingDate { get; set; }

    public DateTime? DueDate { get; set; }

    public int IdBusinessPartner { get; set; }

    public int? IdSalesOrder { get; set; }

    public int? IdSalesContract { get; set; }

    public int? IdPaidTo { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool? IsPosted { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblSalesContract? IdSalesContractNavigation { get; set; }

    public virtual TblSalesOrder? IdSalesOrderNavigation { get; set; }

    public virtual ICollection<TblIncomingPaymentAccount> TblIncomingPaymentAccounts { get; set; } = new List<TblIncomingPaymentAccount>();

    public virtual ICollection<TblIncomingPaymentCash> TblIncomingPaymentCashes { get; set; } = new List<TblIncomingPaymentCash>();

    public virtual ICollection<TblIncomingPaymentCheque> TblIncomingPaymentCheques { get; set; } = new List<TblIncomingPaymentCheque>();

    public virtual ICollection<TblIncomingPaymentInstallment> TblIncomingPaymentInstallments { get; set; } = new List<TblIncomingPaymentInstallment>();

    public virtual ICollection<TblIncomingPaymentTransfer> TblIncomingPaymentTransfers { get; set; } = new List<TblIncomingPaymentTransfer>();
}
