using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblPaymentTermInstallment
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public int IdPaymentTerm { get; set; }

    public int Months { get; set; }

    public int Days { get; set; }

    public decimal Percentage { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblPaymentTerm IdPaymentTermNavigation { get; set; } = null!;
}
