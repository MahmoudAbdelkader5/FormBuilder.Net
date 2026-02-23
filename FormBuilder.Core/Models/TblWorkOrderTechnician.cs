using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblWorkOrderTechnician
{
    public int Id { get; set; }

    public int IdWorkOrder { get; set; }

    public int IdTechnician { get; set; }

    public int? IdMaintenanceType { get; set; }

    public int IdAsset { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateOnly? Date { get; set; }

    public TimeOnly? FromTime { get; set; }

    public TimeOnly? ToTime { get; set; }

    public decimal? ActualHours { get; set; }

    public virtual TblAsset IdAssetNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblMaintenanceType? IdMaintenanceTypeNavigation { get; set; }

    public virtual TblUser IdTechnicianNavigation { get; set; } = null!;

    public virtual TblWorkOrder IdWorkOrderNavigation { get; set; } = null!;
}
