using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwPaymentTermInstallment
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public decimal Percentage { get; set; }

    public int Months { get; set; }

    public int Days { get; set; }

    public int IdPaymentTerm { get; set; }

    public string PaymentTermName { get; set; } = null!;

    public string? PaymentTermForeignName { get; set; }

    public bool PaymentTermIsActive { get; set; }

    public string? PaymentTermCode { get; set; }

    public int PaymentTermMonths { get; set; }

    public int PaymentTermDays { get; set; }

    public bool PaymentTermIsTaxOnFirstInstallment { get; set; }

    public int PaymentTermNumberOfInstallments { get; set; }
}
