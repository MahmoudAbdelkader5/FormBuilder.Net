using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwIncomingPaymentInstallment
{
    public int Id { get; set; }

    public decimal? Amount { get; set; }

    public decimal? TaxAmount { get; set; }

    public decimal? TotalAmount { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdIncomingPayment { get; set; }

    public decimal IncomingPaymentAmount { get; set; }

    public decimal? IncomingPaymentTaxAmount { get; set; }

    public decimal? IncomingPaymentTotalAmount { get; set; }

    public string IncomingPaymentCode { get; set; } = null!;

    public int? IdLegalEntity { get; set; }

    public int? IdSalesInvoiceInstallment { get; set; }

    public decimal? InvoiceInstallmentAmount { get; set; }

    public decimal? InvoiceInstallmentTaxAmount { get; set; }

    public decimal? InvoiceInstallmentTotalAmount { get; set; }

    public int? IdIncomingCash { get; set; }

    public decimal? CashAmount { get; set; }

    public string? CashBillNumber { get; set; }

    public DateTime? CashReceiptDate { get; set; }

    public int? IdIncomingTransfer { get; set; }

    public decimal? TransferAmount { get; set; }

    public string? TransferNumber { get; set; }

    public DateTime? TransferDate { get; set; }

    public string? TransferReceivedFrom { get; set; }

    public int? IdIncomingCheque { get; set; }

    public decimal? ChequeAmount { get; set; }

    public string? ChequeChequeNumber { get; set; }

    public DateTime? ChequeDueDate { get; set; }

    public DateTime? ChequeTransactionDate { get; set; }

    public int? IdPaymentStatus { get; set; }

    public string? PaymentStatusName { get; set; }

    public string? PaymentStatusForeignName { get; set; }
}
