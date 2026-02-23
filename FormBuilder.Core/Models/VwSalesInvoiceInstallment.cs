using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwSalesInvoiceInstallment
{
    public int Id { get; set; }

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

    public int? IdLegalEntity { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int IdCreatedBy { get; set; }

    public int? IdUpdatedBy { get; set; }

    public string? Remarks { get; set; }

    public bool? IsRevenueGenerated { get; set; }

    public DateTime? ReveueGeneratedDate { get; set; }

    public int? IdSalesQuotation { get; set; }

    public string? QuotationCode { get; set; }

    public int? IdSalesInvoice { get; set; }

    public string? SalesInvoiceCode { get; set; }

    public decimal? SalesInvoiceTotalAmount { get; set; }

    public int? IdSalesContract { get; set; }

    public string? SalesContractCode { get; set; }

    public int? SalesContractIdApprovalStatus { get; set; }

    public int? IdSalesOrder { get; set; }

    public string? SalesOrderCode { get; set; }

    public int? SalesOrderIdApprovalStatus { get; set; }

    public int? IdParticular { get; set; }

    public string? ParticularName { get; set; }

    public string? ParticularForeignName { get; set; }

    public string? ParticularCode { get; set; }

    public int IdInstallmentType { get; set; }

    public string? InstallmentTypeName { get; set; }

    public string? InstallmentTypeForeignName { get; set; }

    public int IdInvoiceStatus { get; set; }

    public string? InvoiceStatusName { get; set; }

    public string? InvoiceStatusForeignName { get; set; }

    public int? IdCustomer { get; set; }

    public string? CustomerFirstName { get; set; }

    public string? CustomerForeignFirstName { get; set; }

    public string? CustomerLastName { get; set; }

    public string? CustomerForeignLastName { get; set; }

    public string? CustomerCode { get; set; }

    public string? CustomerMiddleName { get; set; }

    public string? CustomerForeignMiddleName { get; set; }

    public bool OnIncomingPayment { get; set; }
}
