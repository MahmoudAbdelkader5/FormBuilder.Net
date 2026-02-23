using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwMaintenanceTypeTechnician
{
    public string UserUsername { get; set; } = null!;

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public int UserIdUserType { get; set; }

    public bool UserIsActive { get; set; }

    public string TypeName { get; set; } = null!;

    public string? TypeForeignName { get; set; }

    public int IdTechncian { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdMaintenanceType { get; set; }

    public bool IsDefault { get; set; }

    public decimal? Hours { get; set; }

    public string? MaintenanceTypeCode { get; set; }

    public string MaintenanceTypeName { get; set; } = null!;

    public string? MaintenanceTypeForeignName { get; set; }

    public int? IdWorkOrderCategory { get; set; }

    public bool MaintenanceTypeIsActive { get; set; }
}
