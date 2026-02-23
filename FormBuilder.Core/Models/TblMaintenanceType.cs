using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblMaintenanceType
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public int? IdWorkType { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Description { get; set; }

    public string? ForeignDescription { get; set; }

    public decimal? Hours { get; set; }

    public bool? IsRequireShutDown { get; set; }

    public int? IdWorkOrderCategory { get; set; }

    public bool? IsBreakDown { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblWorkOrderCategory? IdWorkOrderCategoryNavigation { get; set; }

    public virtual TblWorkType? IdWorkTypeNavigation { get; set; }

    public virtual ICollection<TblAssetMaintenanceSchedule> TblAssetMaintenanceSchedules { get; set; } = new List<TblAssetMaintenanceSchedule>();

    public virtual ICollection<TblGoodsIssueItem> TblGoodsIssueItems { get; set; } = new List<TblGoodsIssueItem>();

    public virtual ICollection<TblMaintenanceTypeItem> TblMaintenanceTypeItems { get; set; } = new List<TblMaintenanceTypeItem>();

    public virtual ICollection<TblMaintenanceTypeTechncian> TblMaintenanceTypeTechncians { get; set; } = new List<TblMaintenanceTypeTechncian>();

    public virtual ICollection<TblMaintenanceTypeTool> TblMaintenanceTypeTools { get; set; } = new List<TblMaintenanceTypeTool>();

    public virtual ICollection<TblMaterialRequestItem> TblMaterialRequestItems { get; set; } = new List<TblMaterialRequestItem>();

    public virtual ICollection<TblMaterialTransferItem> TblMaterialTransferItems { get; set; } = new List<TblMaterialTransferItem>();

    public virtual ICollection<TblMeterReadingDetail> TblMeterReadingDetails { get; set; } = new List<TblMeterReadingDetail>();

    public virtual ICollection<TblTimeSheet> TblTimeSheets { get; set; } = new List<TblTimeSheet>();

    public virtual ICollection<TblWorkOrderGoodsIssue> TblWorkOrderGoodsIssues { get; set; } = new List<TblWorkOrderGoodsIssue>();

    public virtual ICollection<TblWorkOrderMaintenanceRequest> TblWorkOrderMaintenanceRequests { get; set; } = new List<TblWorkOrderMaintenanceRequest>();

    public virtual ICollection<TblWorkOrderMaintenanceType> TblWorkOrderMaintenanceTypes { get; set; } = new List<TblWorkOrderMaintenanceType>();

    public virtual ICollection<TblWorkOrderSparePart> TblWorkOrderSpareParts { get; set; } = new List<TblWorkOrderSparePart>();

    public virtual ICollection<TblWorkOrderTechnician> TblWorkOrderTechnicians { get; set; } = new List<TblWorkOrderTechnician>();
}
