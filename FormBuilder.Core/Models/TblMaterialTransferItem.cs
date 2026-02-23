using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblMaterialTransferItem
{
    public int Id { get; set; }

    public int IdMaterialTransfer { get; set; }

    public int? IdAsset { get; set; }

    public int? IdMaintenanceType { get; set; }

    public int IdItems { get; set; }

    public int IdTechnician { get; set; }

    public int? IdFromWarehouse { get; set; }

    public int? IdToWarehouse { get; set; }

    public int Quantity { get; set; }

    public int? IssuedQuantity { get; set; }

    public DateTime? IssueDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IdStatus { get; set; }

    public int? IdStatusBy { get; set; }

    public DateTime? StatusDate { get; set; }

    public int? IdWorkOrder { get; set; }

    public int? IdMaterialRequestItem { get; set; }

    public int? IdFromBinLocation { get; set; }

    public int? IdToBinLocation { get; set; }

    public virtual TblAsset? IdAssetNavigation { get; set; }

    public virtual TblWarehouse? IdFromWarehouseNavigation { get; set; }

    public virtual TblItem IdItemsNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblMaintenanceType? IdMaintenanceTypeNavigation { get; set; }

    public virtual TblMaterialRequestItem? IdMaterialRequestItemNavigation { get; set; }

    public virtual TblMaterialTransfer IdMaterialTransferNavigation { get; set; } = null!;

    public virtual TblUser IdTechnicianNavigation { get; set; } = null!;

    public virtual TblWarehouse? IdToWarehouseNavigation { get; set; }

    public virtual TblWorkOrder? IdWorkOrderNavigation { get; set; }

    public virtual ICollection<TblMaterialTransferItemSerialNumber> TblMaterialTransferItemSerialNumbers { get; set; } = new List<TblMaterialTransferItemSerialNumber>();
}
