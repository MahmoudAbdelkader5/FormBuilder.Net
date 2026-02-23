using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblWarehouseBinLocation
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public int? IdWarehouse { get; set; }

    public int? Quantity { get; set; }

    public bool? IsDefault { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblWarehouse? IdWarehouseNavigation { get; set; }

    public virtual ICollection<TblItemSerialNumber> TblItemSerialNumbers { get; set; } = new List<TblItemSerialNumber>();

    public virtual ICollection<TblWarehouseItem> TblWarehouseItems { get; set; } = new List<TblWarehouseItem>();
}
