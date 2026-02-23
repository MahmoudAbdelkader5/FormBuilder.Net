using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblWorkOrderSparePart
{
    public int Id { get; set; }

    public int IdWorkOrder { get; set; }

    public int IdSparePart { get; set; }

    public decimal? EstimatedQuantity { get; set; }

    public decimal? ActualQuantity { get; set; }

    public int? IdMaintenanceType { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int IdAsset { get; set; }

    public int? IdWarehouse { get; set; }

    public decimal? QuantityToIssue { get; set; }

    public int? IdWorkOrderMaintenanceType { get; set; }

    public decimal? Cost { get; set; }

    public decimal? WarehouseQuantity { get; set; }

    public virtual TblAsset IdAssetNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblMaintenanceType? IdMaintenanceTypeNavigation { get; set; }

    public virtual TblItem IdSparePartNavigation { get; set; } = null!;

    public virtual TblWarehouse? IdWarehouseNavigation { get; set; }

    public virtual TblWorkOrder IdWorkOrderNavigation { get; set; } = null!;
}
