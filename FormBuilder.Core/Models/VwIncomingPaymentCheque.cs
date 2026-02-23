using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwIncomingPaymentCheque
{
    public int Id { get; set; }

    public decimal? Amount { get; set; }

    public string? ChequeNumber { get; set; }

    public DateTime? DueDate { get; set; }

    public string? HolderName { get; set; }

    public decimal? StatusFee { get; set; }

    public DateTime? TransactionDate { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Remark { get; set; }

    public int? IdIncomingPayment { get; set; }

    public decimal IncomingPaymentAmount { get; set; }

    public decimal? IncomingPaymentTaxAmount { get; set; }

    public decimal? IncomingPaymentTotalAmount { get; set; }

    public string IncomingPaymentCode { get; set; } = null!;

    public int? IdLegalEntity { get; set; }

    public int? IdSalesOrder { get; set; }

    public string? SalesOrderCode { get; set; }

    public int? IdUnit { get; set; }

    public string? UnitCode { get; set; }

    public string? UnitName { get; set; }

    public string? UnitForeignName { get; set; }

    public int? IdProperty { get; set; }

    public string? PropertyCode { get; set; }

    public string? PropertyName { get; set; }

    public string? PropertyForeignName { get; set; }

    public int? IdSalesContract { get; set; }

    public string? SalesContractCode { get; set; }

    public int IdBusinessPartner { get; set; }

    public string? CustomerFirstName { get; set; }

    public string? CustomerForeignFirstName { get; set; }

    public string? CustomerLastName { get; set; }

    public string? CustomerForeignLastName { get; set; }

    public string? CustomerCode { get; set; }

    public string? CustomerMiddleName { get; set; }

    public string? CustomerForeignMiddleName { get; set; }

    public bool? CustomerIsBlackList { get; set; }

    public int IdBank { get; set; }

    public string? BankCode { get; set; }

    public string? BankName { get; set; }

    public string? BankForeignName { get; set; }

    public bool? BankIsPostOffice { get; set; }

    public string? Branch { get; set; }

    public int IdGlaccount { get; set; }

    public string? GlaccountCode { get; set; }

    public string? GlaccountName { get; set; }

    public string? GlaccountForeignName { get; set; }

    public bool? GlaccountIsPostable { get; set; }

    public int IdChequeStatus { get; set; }

    public string? ChequeStatusName { get; set; }

    public string? ChequeStatusForeignName { get; set; }
}
