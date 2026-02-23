using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblMaterialTransferItemSerialNumber
{
    public int Id { get; set; }

    public int IdMaterialTransfer { get; set; }

    public int? IdMaterialTransferItem { get; set; }

    public int? IdItemSerialNumber { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblItemSerialNumber? IdItemSerialNumberNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblMaterialTransferItem? IdMaterialTransferItemNavigation { get; set; }

    public virtual TblMaterialTransfer IdMaterialTransferNavigation { get; set; } = null!;
}
