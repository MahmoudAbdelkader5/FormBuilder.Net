using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblTimeSheet
{
    public int Id { get; set; }

    public int? IdMaintenanceType { get; set; }

    public int IdTechncian { get; set; }

    public DateOnly Date { get; set; }

    public decimal Hours { get; set; }

    public int? IdLegalEntity { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Description { get; set; }

    public int? IdWorkOrder { get; set; }

    public int? IdAsset { get; set; }

    public int? IdCreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public virtual TblAsset? IdAssetNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblMaintenanceType? IdMaintenanceTypeNavigation { get; set; }

    public virtual TblUser IdTechncianNavigation { get; set; } = null!;

    public virtual TblWorkOrder? IdWorkOrderNavigation { get; set; }
}
