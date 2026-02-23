using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwUnitSalesContract
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public DateTime? HandOverDate { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public DateTime? ActualToDate { get; set; }

    public DateTime? ActualFromDate { get; set; }

    public string? Remarks { get; set; }

    public decimal NetAmount { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal? TaxAmount { get; set; }

    public decimal? DiscountPercent { get; set; }

    public decimal? TaxPercent { get; set; }

    public decimal? ParticularDiscountAmount { get; set; }

    public decimal? ParticularTaxAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal? ParticularAmount { get; set; }

    public int? IdLegalEntity { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int IdCreatedBy { get; set; }

    public int? IdUpdatedBy { get; set; }

    public string? CustomerInfoRef { get; set; }

    public int? IdSalesQuotation { get; set; }

    public string? QuotationCode { get; set; }

    public int? IdSalesOrder { get; set; }

    public string? SalesOrderCode { get; set; }

    public int IdUnit { get; set; }

    public string? UnitName { get; set; }

    public string? UnitForeignName { get; set; }

    public string? UnitCode { get; set; }

    public int? IdProperty { get; set; }

    public string? PropertyName { get; set; }

    public string? PropertyForeignName { get; set; }

    public string? PropertyCode { get; set; }

    public int IdCustomer { get; set; }

    public string? CustomerFirstName { get; set; }

    public string? CustomerForeignFirstName { get; set; }

    public string? CustomerLastName { get; set; }

    public string? CustomerForeignLastName { get; set; }

    public string? CustomerCode { get; set; }

    public string? CustomerMiddleName { get; set; }

    public string? CustomerForeignMiddleName { get; set; }

    public bool? CustomerIsBlackList { get; set; }

    public int? CustomerIdContractType { get; set; }

    public int IdTransactionType { get; set; }

    public string? TransactionTypeName { get; set; }

    public string? TransactionTypeForeignName { get; set; }

    public int? IdUnitRate { get; set; }

    public DateTime? UnitRateEffectiveFrom { get; set; }

    public DateTime? UnitRateEffectiveTo { get; set; }

    public decimal? UnitRateRate { get; set; }

    public int? IdTransactionSource { get; set; }

    public string? TransactionSourceName { get; set; }

    public string? TransactionSourceForeignName { get; set; }

    public int? IdSalesCalculationBase { get; set; }

    public string? CalculationBaseName { get; set; }

    public string? CalculationBaseForeignName { get; set; }

    public int? IdApprovalStatus { get; set; }

    public string? ApprovalStatusName { get; set; }

    public string? ApprovalStatusForeignName { get; set; }

    public int IdDocumentStatus { get; set; }

    public string? DocumentStatusName { get; set; }

    public string? DocumentStatusForeignName { get; set; }

    public int? IdPaymentCalculationBase { get; set; }

    public string? PaymentCalculationBaseName { get; set; }

    public string? PaymentCalculationBaseForeignName { get; set; }

    public int IdPaymentTerm { get; set; }

    public string? PaymentTermName { get; set; }

    public string? PaymentTermForeignName { get; set; }

    public string? PaymentTermCode { get; set; }

    public string? LegalStatus { get; set; }

    public int? InvId { get; set; }

    public string? InvCode { get; set; }

    public int? Ipid { get; set; }

    public string? Ipcode { get; set; }
}
