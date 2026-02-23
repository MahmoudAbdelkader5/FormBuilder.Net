using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblMaterialRequestItem
{
    public int Id { get; set; }

    public int IdMaterialRequest { get; set; }

    public int IdItems { get; set; }

    public int Quantity { get; set; }

    public int IdTechnician { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IssuedQuantity { get; set; }

    public DateTime? IssueDate { get; set; }

    public int? IdMaintenanceType { get; set; }

    public int? IdAsset { get; set; }

    public int? IdFromWarehouse { get; set; }

    public int? IdToWarehouse { get; set; }

    public int? IdWorkOrder { get; set; }

    public bool? IsPurchaseRequestInitiated { get; set; }

    public int? PurchaseRequestQuantity { get; set; }

    public virtual TblAsset? IdAssetNavigation { get; set; }

    public virtual TblWarehouse? IdFromWarehouseNavigation { get; set; }

    public virtual TblItem IdItemsNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblMaintenanceType? IdMaintenanceTypeNavigation { get; set; }

    public virtual TblMaterialRequest IdMaterialRequestNavigation { get; set; } = null!;

    public virtual TblUser IdTechnicianNavigation { get; set; } = null!;

    public virtual TblWarehouse? IdToWarehouseNavigation { get; set; }

    public virtual ICollection<TblMaterialTransferItem> TblMaterialTransferItems { get; set; } = new List<TblMaterialTransferItem>();
}
