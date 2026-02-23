using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblMeterReadingDetail
{
    public int Id { get; set; }

    public int? IdMeterReading { get; set; }

    public int? IdAsset { get; set; }

    public int? IdMeterType { get; set; }

    public decimal? ReadingValue { get; set; }

    public string? Remark { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdMaintenanceType { get; set; }

    public virtual TblAsset? IdAssetNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblMaintenanceType? IdMaintenanceTypeNavigation { get; set; }

    public virtual TblMeterReading? IdMeterReadingNavigation { get; set; }

    public virtual TblMeterType? IdMeterTypeNavigation { get; set; }
}
