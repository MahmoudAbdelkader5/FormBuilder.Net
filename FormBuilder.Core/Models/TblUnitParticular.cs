using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblUnitParticular
{
    public int Id { get; set; }

    public int IdUnit { get; set; }

    public int IdParticular { get; set; }

    public int? IdTransactionType { get; set; }

    public int? IdPeriodBase { get; set; }

    public int? IdType { get; set; }

    public decimal? Amount { get; set; }

    public decimal? Tax { get; set; }

    public bool IsTaxPercentage { get; set; }

    public decimal? Discount { get; set; }

    public bool IsDiscountPercentage { get; set; }

    public bool IsApplyOninstallment { get; set; }

    public bool IsRefundable { get; set; }

    public decimal? RefundableAmount { get; set; }

    public bool IsRefundablePercenatge { get; set; }

    public DateTime? EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdAmountType { get; set; }

    public bool? IsPaidByLandLord { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblParticular IdParticularNavigation { get; set; } = null!;

    public virtual TblUnit IdUnitNavigation { get; set; } = null!;
}
