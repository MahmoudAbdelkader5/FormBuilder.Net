using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwDocumentParticular
{
    public bool? IsAmountPercent { get; set; }

    public decimal? Amount { get; set; }

    public decimal? AmountPercent { get; set; }

    public bool? IsTaxPercent { get; set; }

    public decimal? TaxAmount { get; set; }

    public decimal? TaxPercent { get; set; }

    public bool? IsDiscountPercent { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal? DiscountPercent { get; set; }

    public int? IdLegalEntity { get; set; }

    public DateTime CreatedDate { get; set; }

    public int IdDocument { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsPaidByLandLord { get; set; }

    public int IdCreatedBy { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public int IdParticular { get; set; }

    public string ParticularName { get; set; } = null!;

    public string? ParticularForeignName { get; set; }

    public string? ParticularCode { get; set; }

    public int? IdType { get; set; }

    public string? TypeName { get; set; }

    public string? TypeForeignName { get; set; }

    public decimal? ParticularAmount { get; set; }

    public bool? ParticularIsPaidByLandLord { get; set; }

    public decimal? ParticularTax { get; set; }

    public bool ParticularIsTaxPercentage { get; set; }

    public decimal? ParticularDiscount { get; set; }

    public bool ParticularIsDiscountPercentage { get; set; }

    public bool ParticularIsApplyOninstallment { get; set; }

    public bool ParticularIsRefundable { get; set; }

    public decimal? ParticularRefundableAmount { get; set; }

    public bool ParticularIsRefundablePercenatge { get; set; }

    public string? PeriodBaseName { get; set; }

    public string? PeriodBaseForeignName { get; set; }

    public string? TransactionTypeName { get; set; }

    public string? TransactionTypeForeignName { get; set; }

    public int IdObjectType { get; set; }

    public string ObjectTypeName { get; set; } = null!;

    public string? ObjectTypeForeignName { get; set; }
}
