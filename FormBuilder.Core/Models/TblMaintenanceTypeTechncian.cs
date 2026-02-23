using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblMaintenanceTypeTechncian
{
    public int IdMaintenanceType { get; set; }

    public int IdTechncian { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsDefault { get; set; }

    public decimal? Hours { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblMaintenanceType IdMaintenanceTypeNavigation { get; set; } = null!;

    public virtual TblUser IdTechncianNavigation { get; set; } = null!;
}
