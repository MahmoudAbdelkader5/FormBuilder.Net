using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwSalesInvoice
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public decimal TotalAmount { get; set; }

    public int? IdLegalEntity { get; set; }

    public DateTime CreatedDate { get; set; }

    public int IdCreatedBy { get; set; }

    public int? IdSalesContract { get; set; }

    public string ContractCode { get; set; } = null!;

    public string? ContractUnitName { get; set; }

    public int ContractIdUnit { get; set; }

    public string? ContractUnitForeignName { get; set; }

    public string? ContractUnitCode { get; set; }

    public string? ContractTransactionTypeName { get; set; }

    public int ContractIdTransactionType { get; set; }

    public string? ContractTransactionTypeForeignName { get; set; }

    public int? IdSalesOrder { get; set; }

    public string OrderCode { get; set; } = null!;

    public string? OrderUnitName { get; set; }

    public string? OrderUnitForeignName { get; set; }

    public int OrderIdUnit { get; set; }

    public string? OrderUnitCode { get; set; }

    public int OrderIdTransactionType { get; set; }

    public string? OrderTransactionTypeName { get; set; }

    public string? OrderTransactionTypeForeignName { get; set; }

    public string? CustomerName { get; set; }
}
