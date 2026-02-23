using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblDocumentParticular
{
    public int IdObjectType { get; set; }

    public int IdDocument { get; set; }

    public int IdParticular { get; set; }

    public bool? IsAmountPercent { get; set; }

    public decimal? Amount { get; set; }

    public decimal? AmountPercent { get; set; }

    public bool? IsTaxPercent { get; set; }

    public decimal? TaxAmount { get; set; }

    public decimal? TaxPercent { get; set; }

    public bool? IsDiscountPercent { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal? DiscountPercent { get; set; }

    public decimal? Quantity { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsPaidByLandLord { get; set; }

    public bool IsSeparateParticularInstallment { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblParticular IdParticularNavigation { get; set; } = null!;
}
