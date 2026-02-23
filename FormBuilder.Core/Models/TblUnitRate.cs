using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblUnitRate
{
    public int Id { get; set; }

    public DateTime? EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public decimal? MaxDiscountPercent { get; set; }

    public decimal? TaxPercentage { get; set; }

    public decimal? MinimumRate { get; set; }

    public decimal? Rate { get; set; }

    public decimal? MaximumRate { get; set; }

    public string? Remark { get; set; }

    public int? IdPeriodBase { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsRenew { get; set; }

    public int IdUnit { get; set; }

    public int? IdTransactionType { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblSalesContract> TblSalesContracts { get; set; } = new List<TblSalesContract>();

    public virtual ICollection<TblSalesOrder> TblSalesOrders { get; set; } = new List<TblSalesOrder>();

    public virtual ICollection<TblSalesQuotation> TblSalesQuotations { get; set; } = new List<TblSalesQuotation>();
}
