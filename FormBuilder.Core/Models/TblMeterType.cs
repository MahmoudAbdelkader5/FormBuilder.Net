using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblMeterType
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblAssetMaintenanceSchedule> TblAssetMaintenanceSchedules { get; set; } = new List<TblAssetMaintenanceSchedule>();

    public virtual ICollection<TblAsset> TblAssets { get; set; } = new List<TblAsset>();

    public virtual ICollection<TblMeterReadingDetail> TblMeterReadingDetails { get; set; } = new List<TblMeterReadingDetail>();
}
