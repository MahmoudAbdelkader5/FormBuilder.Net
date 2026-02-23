using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblAsset
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public int? IdGroup { get; set; }

    public int? IdZone { get; set; }

    public int? IdCostCenter1 { get; set; }

    public int? IdCostCenter2 { get; set; }

    public int? IdCostCenter3 { get; set; }

    public int? IdCostCenter4 { get; set; }

    public int? IdCostCenter5 { get; set; }

    public int? IdType { get; set; }

    public string? Barcode { get; set; }

    public string? Description { get; set; }

    public string? ForeignDescription { get; set; }

    public string? SerialNumber { get; set; }

    public int? IdManufacturer { get; set; }

    public int? IdAssetStatus { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsWarranty { get; set; }

    public DateTime? WarrantyExpiryDate { get; set; }

    public string? WarrantyRemark { get; set; }

    public bool? IsServiceContract { get; set; }

    public DateTime? ServiceContractExpiryDate { get; set; }

    public string? ServiceContractRemark { get; set; }

    public int? ManufactureYear { get; set; }

    public string? Vender { get; set; }

    public int? IdUnit { get; set; }

    public decimal? Cost { get; set; }

    public int? IdMainGroup { get; set; }

    public int? IdSubGroup1 { get; set; }

    public int? IdSubGroup2 { get; set; }

    public int? IdAssetType { get; set; }

    public virtual TblCostCenter? IdCostCenter1Navigation { get; set; }

    public virtual TblCostCenter? IdCostCenter2Navigation { get; set; }

    public virtual TblCostCenter? IdCostCenter3Navigation { get; set; }

    public virtual TblCostCenter? IdCostCenter4Navigation { get; set; }

    public virtual TblCostCenter? IdCostCenter5Navigation { get; set; }

    public virtual TblAssetGroup? IdGroupNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblManufacturer? IdManufacturerNavigation { get; set; }

    public virtual TblMeterType? IdTypeNavigation { get; set; }

    public virtual TblZone? IdZoneNavigation { get; set; }

    public virtual ICollection<TblAssetMaintenanceSchedule> TblAssetMaintenanceSchedules { get; set; } = new List<TblAssetMaintenanceSchedule>();

    public virtual ICollection<TblAssetRepairAsset> TblAssetRepairAssets { get; set; } = new List<TblAssetRepairAsset>();

    public virtual ICollection<TblAssetTransferAsset> TblAssetTransferAssets { get; set; } = new List<TblAssetTransferAsset>();

    public virtual ICollection<TblCertification> TblCertifications { get; set; } = new List<TblCertification>();

    public virtual ICollection<TblGoodsIssueItem> TblGoodsIssueItems { get; set; } = new List<TblGoodsIssueItem>();

    public virtual ICollection<TblMaterialRequestItem> TblMaterialRequestItems { get; set; } = new List<TblMaterialRequestItem>();

    public virtual ICollection<TblMaterialTransferItem> TblMaterialTransferItems { get; set; } = new List<TblMaterialTransferItem>();

    public virtual ICollection<TblMeterReadingDetail> TblMeterReadingDetails { get; set; } = new List<TblMeterReadingDetail>();

    public virtual ICollection<TblTimeSheet> TblTimeSheets { get; set; } = new List<TblTimeSheet>();

    public virtual ICollection<TblWorkOrderAsset> TblWorkOrderAssets { get; set; } = new List<TblWorkOrderAsset>();

    public virtual ICollection<TblWorkOrderExpense> TblWorkOrderExpenses { get; set; } = new List<TblWorkOrderExpense>();

    public virtual ICollection<TblWorkOrderGoodsIssue> TblWorkOrderGoodsIssues { get; set; } = new List<TblWorkOrderGoodsIssue>();

    public virtual ICollection<TblWorkOrderMaintenanceRequest> TblWorkOrderMaintenanceRequests { get; set; } = new List<TblWorkOrderMaintenanceRequest>();

    public virtual ICollection<TblWorkOrderSparePart> TblWorkOrderSpareParts { get; set; } = new List<TblWorkOrderSparePart>();

    public virtual ICollection<TblWorkOrderTechnician> TblWorkOrderTechnicians { get; set; } = new List<TblWorkOrderTechnician>();
}
