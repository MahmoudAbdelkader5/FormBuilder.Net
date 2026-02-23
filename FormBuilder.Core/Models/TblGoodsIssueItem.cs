using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblGoodsIssueItem
{
    public int Id { get; set; }

    public int IdGoodsIssue { get; set; }

    public int? IdAsset { get; set; }

    public int? IdMaintenanceType { get; set; }

    public int IdItems { get; set; }

    public int? IdWarehouse { get; set; }

    public int? IssuedQuantity { get; set; }

    public DateTime? IssueDate { get; set; }

    public int Quantity { get; set; }

    public int IdTechnician { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IdWorkOrderSparePart { get; set; }

    public decimal? Cost { get; set; }

    public int? IdBinLocation { get; set; }

    public virtual TblAsset? IdAssetNavigation { get; set; }

    public virtual TblGoodsIssue IdGoodsIssueNavigation { get; set; } = null!;

    public virtual TblItem IdItemsNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblMaintenanceType? IdMaintenanceTypeNavigation { get; set; }

    public virtual TblUser IdTechnicianNavigation { get; set; } = null!;

    public virtual TblWarehouse? IdWarehouseNavigation { get; set; }

    public virtual ICollection<TblGoodsIssueItemSerialNumber> TblGoodsIssueItemSerialNumbers { get; set; } = new List<TblGoodsIssueItemSerialNumber>();
}
