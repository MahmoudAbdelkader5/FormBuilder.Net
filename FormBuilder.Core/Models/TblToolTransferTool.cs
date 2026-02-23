using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblToolTransferTool
{
    public int Id { get; set; }

    public int IdTool { get; set; }

    public int IdTechnician { get; set; }

    public int? IdFromWarehouse { get; set; }

    public int? IdToWarehouse { get; set; }

    public int Quantity { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IdStatus { get; set; }

    public int? IdStatusBy { get; set; }

    public DateTime? StatusDate { get; set; }

    public string? Remarks { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? IdCreatedBy { get; set; }

    public int? IdToolTransfer { get; set; }

    public virtual TblWarehouse? IdFromWarehouseNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblUser IdTechnicianNavigation { get; set; } = null!;

    public virtual TblWarehouse? IdToWarehouseNavigation { get; set; }

    public virtual TblTool IdToolNavigation { get; set; } = null!;

    public virtual TblToolTransfer? IdToolTransferNavigation { get; set; }
}
