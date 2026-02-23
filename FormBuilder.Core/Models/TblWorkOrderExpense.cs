using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblWorkOrderExpense
{
    public int Id { get; set; }

    public int IdWorkOrder { get; set; }

    public int IdAsset { get; set; }

    public string Description { get; set; } = null!;

    public DateTime? Date { get; set; }

    public int Quantity { get; set; }

    public int Cost { get; set; }

    public int IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Code { get; set; }

    public virtual TblAsset IdAssetNavigation { get; set; } = null!;

    public virtual TblLegalEntity IdLegalEntityNavigation { get; set; } = null!;

    public virtual TblWorkOrder IdWorkOrderNavigation { get; set; } = null!;
}
