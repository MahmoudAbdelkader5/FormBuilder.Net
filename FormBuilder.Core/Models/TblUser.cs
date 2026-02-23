using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblUser
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public int IdUserType { get; set; }

    public int IdPreferableLanguage { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdApprovalStatus { get; set; }

    public bool? EmployeeLiscence { get; set; }

    public int? IdWarehouse { get; set; }

    public int? IdEmployee { get; set; }

    public decimal? RatePerHour { get; set; }

    public string? LoginKey { get; set; }

    public virtual TblEmployee? IdEmployeeNavigation { get; set; }

    public virtual ICollection<TblAlertMessageUser> TblAlertMessageUsers { get; set; } = new List<TblAlertMessageUser>();

    public virtual ICollection<TblApprovalAlert> TblApprovalAlerts { get; set; } = new List<TblApprovalAlert>();

    public virtual ICollection<TblApprovalProcess> TblApprovalProcesses { get; set; } = new List<TblApprovalProcess>();

    public virtual ICollection<TblApprovalStageUser> TblApprovalStageUsers { get; set; } = new List<TblApprovalStageUser>();

    public virtual ICollection<TblApprovalTemplateUser> TblApprovalTemplateUsers { get; set; } = new List<TblApprovalTemplateUser>();

    public virtual ICollection<TblAssetPurchaseRequest> TblAssetPurchaseRequests { get; set; } = new List<TblAssetPurchaseRequest>();

    public virtual ICollection<TblAssetRepair> TblAssetRepairs { get; set; } = new List<TblAssetRepair>();

    public virtual ICollection<TblAssetTransferAsset> TblAssetTransferAssets { get; set; } = new List<TblAssetTransferAsset>();

    public virtual ICollection<TblCertification> TblCertifications { get; set; } = new List<TblCertification>();

    public virtual ICollection<TblDocumentAttestationType> TblDocumentAttestationTypes { get; set; } = new List<TblDocumentAttestationType>();

    public virtual ICollection<TblGlaccountDeterminationDetail> TblGlaccountDeterminationDetails { get; set; } = new List<TblGlaccountDeterminationDetail>();

    public virtual ICollection<TblGoodsIssueItem> TblGoodsIssueItems { get; set; } = new List<TblGoodsIssueItem>();

    public virtual ICollection<TblGoodsIssue> TblGoodsIssues { get; set; } = new List<TblGoodsIssue>();

    public virtual ICollection<TblLegalEntityUser> TblLegalEntityUsers { get; set; } = new List<TblLegalEntityUser>();

    public virtual ICollection<TblMaintenanceTypeTechncian> TblMaintenanceTypeTechncians { get; set; } = new List<TblMaintenanceTypeTechncian>();

    public virtual ICollection<TblMaterialRequestItem> TblMaterialRequestItems { get; set; } = new List<TblMaterialRequestItem>();

    public virtual ICollection<TblMaterialRequest> TblMaterialRequests { get; set; } = new List<TblMaterialRequest>();

    public virtual ICollection<TblMaterialTransferItem> TblMaterialTransferItems { get; set; } = new List<TblMaterialTransferItem>();

    public virtual ICollection<TblMaterialTransfer> TblMaterialTransfers { get; set; } = new List<TblMaterialTransfer>();

    public virtual ICollection<TblRemark> TblRemarks { get; set; } = new List<TblRemark>();

    public virtual ICollection<TblSparePartRepairRequest> TblSparePartRepairRequests { get; set; } = new List<TblSparePartRepairRequest>();

    public virtual ICollection<TblSparePartRepairTransfer> TblSparePartRepairTransfers { get; set; } = new List<TblSparePartRepairTransfer>();

    public virtual ICollection<TblSystemAlert> TblSystemAlerts { get; set; } = new List<TblSystemAlert>();

    public virtual ICollection<TblTimeSheet> TblTimeSheets { get; set; } = new List<TblTimeSheet>();

    public virtual ICollection<TblToolTransferTool> TblToolTransferTools { get; set; } = new List<TblToolTransferTool>();

    public virtual ICollection<TblUserGroupUser> TblUserGroupUsers { get; set; } = new List<TblUserGroupUser>();

    public virtual ICollection<TblWorkOrderTechnician> TblWorkOrderTechnicians { get; set; } = new List<TblWorkOrderTechnician>();
}
