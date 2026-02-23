using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblSalesContract
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public int IdUnit { get; set; }

    public int IdCustomer { get; set; }

    public int IdTransactionType { get; set; }

    public int? IdUnitRate { get; set; }

    public int? IdTransactionSource { get; set; }

    public int? IdSalesCalculationBase { get; set; }

    public int IdPaymentTerm { get; set; }

    public int? IdApprovalStatus { get; set; }

    public int? IdSalesQuotation { get; set; }

    public int? IdSalesOrder { get; set; }

    public DateTime? HandOverDate { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public string? Remarks { get; set; }

    public decimal NetAmount { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal? TaxAmount { get; set; }

    public decimal? DiscountPercent { get; set; }

    public decimal? TaxPercent { get; set; }

    public decimal? ParticularAmount { get; set; }

    public decimal? ParticularDiscountAmount { get; set; }

    public decimal? ParticularTaxAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public DateTime? ActualFromDate { get; set; }

    public int? IdPaymentCalculationBase { get; set; }

    public DateTime? ActualToDate { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public int IdDocumentStatus { get; set; }

    public int? IdTempDocStatus { get; set; }

    public string? CustomerInfoRef { get; set; }

    public bool IsConfidential { get; set; }

    public string? LegalStatus { get; set; }

    public virtual TblCustomer IdCustomerNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblPaymentTerm IdPaymentTermNavigation { get; set; } = null!;

    public virtual TblSalesOrder? IdSalesOrderNavigation { get; set; }

    public virtual TblSalesQuotation? IdSalesQuotationNavigation { get; set; }

    public virtual TblTransactionSource? IdTransactionSourceNavigation { get; set; }

    public virtual TblUnit IdUnitNavigation { get; set; } = null!;

    public virtual TblUnitRate? IdUnitRateNavigation { get; set; }

    public virtual ICollection<TblAccessCard> TblAccessCards { get; set; } = new List<TblAccessCard>();

    public virtual ICollection<TblCancellationProcess> TblCancellationProcesses { get; set; } = new List<TblCancellationProcess>();

    public virtual ICollection<TblCommission> TblCommissions { get; set; } = new List<TblCommission>();

    public virtual ICollection<TblCustomerRequest> TblCustomerRequests { get; set; } = new List<TblCustomerRequest>();

    public virtual ICollection<TblIncomingPayment> TblIncomingPayments { get; set; } = new List<TblIncomingPayment>();

    public virtual ICollection<TblSalesInvoiceInstallment> TblSalesInvoiceInstallments { get; set; } = new List<TblSalesInvoiceInstallment>();

    public virtual ICollection<TblSalesInvoice> TblSalesInvoices { get; set; } = new List<TblSalesInvoice>();

    public virtual ICollection<TblSalesOrder> TblSalesOrders { get; set; } = new List<TblSalesOrder>();
}
