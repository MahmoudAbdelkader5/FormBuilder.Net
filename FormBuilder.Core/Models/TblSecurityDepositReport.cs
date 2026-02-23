using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblSecurityDepositReport
{
    public int Id { get; set; }

    public string? CustomerName { get; set; }

    public string? UnitName { get; set; }

    public decimal? SecurityDepositAmount { get; set; }

    public DateTime? ContractCreatedDate { get; set; }

    public decimal? ContractAmount { get; set; }

    public string? ContractCode { get; set; }

    public string? InvoiceCode { get; set; }

    public string? SalesQuotationCode { get; set; }

    public string? SalesOrderCode { get; set; }

    public string? ParticularName { get; set; }

    public string? InstallmentType { get; set; }

    public DateTime? CreatedAt { get; set; }
}
