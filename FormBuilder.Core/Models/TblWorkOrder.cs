using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblWorkOrder
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public string? DocumentNumber { get; set; }

    public int? IdWocategory { get; set; }

    public int? IdWorkOrderCategory { get; set; }

    public int? IdSource { get; set; }

    public int? IdPriority { get; set; }

    public int? IdZone { get; set; }

    public string? ManholeNumber { get; set; }

    public int? IdSafetyAssessment { get; set; }

    public int? IdApprovalStatus { get; set; }

    public DateTime? DocumentDate { get; set; }

    public DateTime? RequiredDate { get; set; }

    public DateTime? ClosingDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? InductionExplanation { get; set; }

    public string? EquipmentDetails { get; set; }

    public string? Inspection { get; set; }

    public string? Findings { get; set; }

    public string? CorrectiveAction { get; set; }

    public long ReferenceNumber { get; set; }

    public int? IdJobType { get; set; }

    public int? IdCustomer { get; set; }

    public DateTime? StartDate { get; set; }

    public string? Remarks { get; set; }

    public bool? IsCompleted { get; set; }

    public DateTime? MachineStartingTime { get; set; }

    public DateTime? MachineStoppingTime { get; set; }

    public decimal? MachineDownTime { get; set; }

    public DateTime? CompleteDate { get; set; }

    public virtual TblCustomer? IdCustomerNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblWorkOrderCategory? IdWorkOrderCategoryNavigation { get; set; }

    public virtual TblZone? IdZoneNavigation { get; set; }

    public virtual ICollection<TblAssetMaintenanceSchedule> TblAssetMaintenanceSchedules { get; set; } = new List<TblAssetMaintenanceSchedule>();

    public virtual ICollection<TblAssetRepairAsset> TblAssetRepairAssets { get; set; } = new List<TblAssetRepairAsset>();

    public virtual ICollection<TblAssetRepair> TblAssetRepairs { get; set; } = new List<TblAssetRepair>();

    public virtual ICollection<TblAttachment> TblAttachments { get; set; } = new List<TblAttachment>();

    public virtual ICollection<TblGoodsIssue> TblGoodsIssues { get; set; } = new List<TblGoodsIssue>();

    public virtual ICollection<TblMaterialRequest> TblMaterialRequests { get; set; } = new List<TblMaterialRequest>();

    public virtual ICollection<TblMaterialTransferItem> TblMaterialTransferItems { get; set; } = new List<TblMaterialTransferItem>();

    public virtual ICollection<TblMaterialTransfer> TblMaterialTransfers { get; set; } = new List<TblMaterialTransfer>();

    public virtual ICollection<TblRemark> TblRemarks { get; set; } = new List<TblRemark>();

    public virtual ICollection<TblTimeSheet> TblTimeSheets { get; set; } = new List<TblTimeSheet>();

    public virtual ICollection<TblWorkOrderAsset> TblWorkOrderAssets { get; set; } = new List<TblWorkOrderAsset>();

    public virtual ICollection<TblWorkOrderAttachment> TblWorkOrderAttachments { get; set; } = new List<TblWorkOrderAttachment>();

    public virtual ICollection<TblWorkOrderExpense> TblWorkOrderExpenses { get; set; } = new List<TblWorkOrderExpense>();

    public virtual ICollection<TblWorkOrderGoodsIssue> TblWorkOrderGoodsIssues { get; set; } = new List<TblWorkOrderGoodsIssue>();

    public virtual ICollection<TblWorkOrderMaintenanceRequest> TblWorkOrderMaintenanceRequests { get; set; } = new List<TblWorkOrderMaintenanceRequest>();

    public virtual ICollection<TblWorkOrderMaintenanceType> TblWorkOrderMaintenanceTypes { get; set; } = new List<TblWorkOrderMaintenanceType>();

    public virtual ICollection<TblWorkOrderRemark> TblWorkOrderRemarks { get; set; } = new List<TblWorkOrderRemark>();

    public virtual ICollection<TblWorkOrderSparePart> TblWorkOrderSpareParts { get; set; } = new List<TblWorkOrderSparePart>();

    public virtual ICollection<TblWorkOrderTechnician> TblWorkOrderTechnicians { get; set; } = new List<TblWorkOrderTechnician>();

    public virtual ICollection<TblWorkOrderTool> TblWorkOrderTools { get; set; } = new List<TblWorkOrderTool>();
}
