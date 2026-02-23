using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblIncomingPaymentInstallment
{
    public int Id { get; set; }

    public decimal? Amount { get; set; }

    public decimal? TaxAmount { get; set; }

    public decimal? TotalAmount { get; set; }

    public int? IdPaymentStatus { get; set; }

    public int? IdIncomingPayment { get; set; }

    public int? IdSalesInvoiceInstallment { get; set; }

    public int? IdIncomingCash { get; set; }

    public int? IdIncomingTransfer { get; set; }

    public int? IdIncomingAccount { get; set; }

    public int? IdIncomingCheque { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual TblIncomingPaymentAccount? IdIncomingAccountNavigation { get; set; }

    public virtual TblIncomingPaymentCash? IdIncomingCashNavigation { get; set; }

    public virtual TblIncomingPaymentCheque? IdIncomingChequeNavigation { get; set; }

    public virtual TblIncomingPayment? IdIncomingPaymentNavigation { get; set; }

    public virtual TblIncomingPaymentTransfer? IdIncomingTransferNavigation { get; set; }

    public virtual TblSalesInvoiceInstallment? IdSalesInvoiceInstallmentNavigation { get; set; }
}
