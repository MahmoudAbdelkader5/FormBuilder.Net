using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblMaintenanceTypeItem
{
    public int Id { get; set; }

    public int IdMaintenanceType { get; set; }

    public int IdItem { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public decimal? Quantity { get; set; }

    public string? MaintenanceType { get; set; }

    public string? Item { get; set; }

    public virtual TblItem IdItemNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblMaintenanceType IdMaintenanceTypeNavigation { get; set; } = null!;
}
