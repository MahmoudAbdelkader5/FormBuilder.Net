using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblRevenueRecognitionMonthly
{
    public int Id { get; set; }

    public int IdSalesContract { get; set; }

    public string InvoiceCode { get; set; } = null!;

    public string ContractCode { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public decimal UnitRate { get; set; }

    public decimal DailyRate { get; set; }

    public DateOnly MonthStart { get; set; }

    public DateOnly MonthEnd { get; set; }

    public decimal MonthlyAmount { get; set; }

    public decimal? RemainingFromUnitRate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool IsPosted { get; set; }

    public DateTime? PostingDate { get; set; }
}
