using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwIncomingPayment
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public decimal? TaxAmount { get; set; }

    public decimal? TotalAmount { get; set; }

    public string Code { get; set; } = null!;

    public string? Remarks { get; set; }

    public DateTime PostingDate { get; set; }

    public DateTime? DueDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdSalesOrder { get; set; }

    public string? SalesOrderCode { get; set; }

    public int? IdSalesContract { get; set; }

    public string? SalesContractCode { get; set; }

    public int IdBusinessPartner { get; set; }

    public string CustomerFirstName { get; set; } = null!;

    public string? CustomerForeignFirstName { get; set; }

    public string? CustomerLastName { get; set; }

    public string? CustomerForeignLastName { get; set; }

    public string? CustomerCode { get; set; }

    public string? CustomerMiddleName { get; set; }

    public string? CustomerForeignMiddleName { get; set; }

    public bool? CustomerIsBlackList { get; set; }

    public int? IdPaidTo { get; set; }

    public string? PaidToName { get; set; }

    public string? PaidToForeignName { get; set; }
}
