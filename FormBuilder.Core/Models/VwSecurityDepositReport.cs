using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwSecurityDepositReport
{
    public string CustomerName { get; set; } = null!;

    public string UnitName { get; set; } = null!;

    public string? UnitCode { get; set; }

    public decimal SecurityDepositAmount { get; set; }

    public DateTime ContractCreatedDate { get; set; }

    public decimal ContractAmount { get; set; }

    public string ContractCode { get; set; } = null!;

    public string? InvoiceCode { get; set; }

    public string? SalesQuotationCode { get; set; }

    public string SalesOrderCode { get; set; } = null!;

    public string ParticularName { get; set; } = null!;

    public string InstallmentType { get; set; } = null!;
}
