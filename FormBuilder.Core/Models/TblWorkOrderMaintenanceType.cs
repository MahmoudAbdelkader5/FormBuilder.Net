using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblWorkOrderMaintenanceType
{
    public int IdWorkOrder { get; set; }

    public int? IdMaintenanceType { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int IdAsset { get; set; }

    public int Id { get; set; }

    public bool? IsRiskAcknowledgement { get; set; }

    public bool? IsInductionAcknowledgement { get; set; }

    public string? Remarks { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblMaintenanceType? IdMaintenanceTypeNavigation { get; set; }

    public virtual TblWorkOrder IdWorkOrderNavigation { get; set; } = null!;
}
