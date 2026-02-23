using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwParticular
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public decimal? Amount { get; set; }

    public decimal? Tax { get; set; }

    public bool IsTaxPercentage { get; set; }

    public decimal? Discount { get; set; }

    public bool IsDiscountPercentage { get; set; }

    public bool IsApplyOninstallment { get; set; }

    public bool IsRefundable { get; set; }

    public decimal? RefundableAmount { get; set; }

    public bool IsRefundablePercenatge { get; set; }

    public bool? IsPaidByLandLord { get; set; }

    public int? IdTransactionType { get; set; }

    public string? TransactionTypeName { get; set; }

    public string? TransactionTypeForeignName { get; set; }

    public int? IdPeriodBase { get; set; }

    public string? PeriodBaseName { get; set; }

    public string? PeriodBaseForeignName { get; set; }

    public int? IdType { get; set; }

    public string? TypeName { get; set; }

    public string? TypeForeignName { get; set; }

    public int? IdAmountType { get; set; }

    public string? AmountTypeName { get; set; }

    public string? AmountTypeForeignName { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool IsSeparateParticularInstallment { get; set; }
}
