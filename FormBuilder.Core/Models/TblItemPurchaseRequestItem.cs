using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblItemPurchaseRequestItem
{
    public int Id { get; set; }

    public int? IdItemPurchaseRequest { get; set; }

    public int? IdItem { get; set; }

    public decimal? Quantity { get; set; }

    public string? ItemTax { get; set; }

    public int? IdInventoryUnitOfMeasure { get; set; }

    public int? IdSupplier { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblInventoryUnitMeasure? IdInventoryUnitOfMeasureNavigation { get; set; }

    public virtual TblItemPurchaseRequest? IdItemPurchaseRequestNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblSupplier? IdSupplierNavigation { get; set; }
}
