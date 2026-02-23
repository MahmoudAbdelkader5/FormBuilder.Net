using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblAssetMaintenanceSchedule
{
    public int Id { get; set; }

    public int IdAsset { get; set; }

    public int IdMaintenanceType { get; set; }

    public int IdStatus { get; set; }

    public int IdFrequency { get; set; }

    public int FrequencyInterval { get; set; }

    public int? IdMeterType { get; set; }

    public DateTime? LastMaintenanceDate { get; set; }

    public DateTime? NextMaintenanceDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public decimal? LastMaintenanceReading { get; set; }

    public decimal? LastMeterReading { get; set; }

    public bool? IsScheduled { get; set; }

    public int? IdWorkOrder { get; set; }

    public decimal? SchedulerLimit { get; set; }

    public int? AlertInterval { get; set; }

    public virtual TblAsset IdAssetNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblMaintenanceType IdMaintenanceTypeNavigation { get; set; } = null!;

    public virtual TblMeterType? IdMeterTypeNavigation { get; set; }

    public virtual TblWorkOrder? IdWorkOrderNavigation { get; set; }
}
