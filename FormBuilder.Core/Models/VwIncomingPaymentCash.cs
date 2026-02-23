using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwIncomingPaymentCash
{
    public int Id { get; set; }

    public decimal? Amount { get; set; }

    public string? BillNumber { get; set; }

    public DateTime? ReceiptDate { get; set; }

    public DateTime? TransactionDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdIncomingPayment { get; set; }

    public decimal IncomingPaymentAmount { get; set; }

    public decimal? IncomingPaymentTaxAmount { get; set; }

    public decimal? IncomingPaymentTotalAmount { get; set; }

    public string IncomingPaymentCode { get; set; } = null!;

    public int? IdLegalEntity { get; set; }

    public int? IdSalesOrder { get; set; }

    public string? SalesOrderCode { get; set; }

    public int? IdSalesContract { get; set; }

    public string? SalesContractCode { get; set; }

    public int? IdGlaccount { get; set; }

    public string? GlaccountCode { get; set; }

    public string? GlaccountName { get; set; }

    public string? GlaccountForeignName { get; set; }

    public bool? GlaccountIsPostable { get; set; }

    public int? IdCashStatus { get; set; }

    public string? CashStatusName { get; set; }

    public string? CashStatusForeignName { get; set; }

    public int IdCreatedBy { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }
}
