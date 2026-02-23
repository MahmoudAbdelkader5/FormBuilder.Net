using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblWorkOrderMaintenanceRequest
{
    public int Id { get; set; }

    public int IdAsset { get; set; }

    public int? IdMaintenanceType { get; set; }

    public int IdStatus { get; set; }

    public DateTime? DeffectDate { get; set; }

    public int? IdShift { get; set; }

    public int? IdDepartment { get; set; }

    public int? IdCustomer { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdWorkOrder { get; set; }

    public string? Remark { get; set; }

    public DateTime? PostingDate { get; set; }

    public int? IdRequesterStatus { get; set; }

    public string? Code { get; set; }

    public virtual TblAsset IdAssetNavigation { get; set; } = null!;

    public virtual TblCustomer? IdCustomerNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblMaintenanceType? IdMaintenanceTypeNavigation { get; set; }

    public virtual TblWorkOrder? IdWorkOrderNavigation { get; set; }
}
