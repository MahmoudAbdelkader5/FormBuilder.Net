using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblSalesInvoiceInstallment
{
    public int Id { get; set; }

    public int? IdSalesInvoice { get; set; }

    public int? IdSalesContract { get; set; }

    public int? IdSalesOrder { get; set; }

    public int? IdSalesQuotation { get; set; }

    public decimal Amount { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal RemainingAmount { get; set; }

    public decimal RemainingTaxAmount { get; set; }

    public decimal TotalRemainingAmount { get; set; }

    public int? InstallmentMonths { get; set; }

    public int? InstallmentDays { get; set; }

    public decimal? InstallmentPercentage { get; set; }

    public DateTime PostingDate { get; set; }

    public DateTime DueDate { get; set; }

    public int IdInstallmentType { get; set; }

    public int IdInvoiceStatus { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IdParticular { get; set; }

    public bool? IsPosted { get; set; }

    public string? Remarks { get; set; }

    public bool? IsReplacedCheque { get; set; }

    public bool? IsBouncedCheque { get; set; }

    public bool? IsRevenueGenerated { get; set; }

    public DateTime? ReveueGeneratedDate { get; set; }

    public bool OnIncomingPayment { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblParticular? IdParticularNavigation { get; set; }

    public virtual TblSalesContract? IdSalesContractNavigation { get; set; }

    public virtual TblSalesInvoice? IdSalesInvoiceNavigation { get; set; }

    public virtual TblSalesOrder? IdSalesOrderNavigation { get; set; }

    public virtual TblSalesQuotation? IdSalesQuotationNavigation { get; set; }

    public virtual ICollection<TblIncomingPaymentInstallment> TblIncomingPaymentInstallments { get; set; } = new List<TblIncomingPaymentInstallment>();
}
