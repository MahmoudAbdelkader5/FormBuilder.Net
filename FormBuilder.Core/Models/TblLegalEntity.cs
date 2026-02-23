using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblLegalEntity
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public string? DatabaseServer { get; set; }

    public string? DatabaseName { get; set; }

    public string? DatabaseUsername { get; set; }

    public string? DatabasePassword { get; set; }

    public string? SapuserUsername { get; set; }

    public string? SapuserPassword { get; set; }

    public string? LiscenceServer { get; set; }

    public string? Sldserver { get; set; }

    public int? IdConnectionType { get; set; }

    public bool? AllowMultiBranches { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Sapserver { get; set; }

    public virtual ICollection<TblAccessCardCar> TblAccessCardCars { get; set; } = new List<TblAccessCardCar>();

    public virtual ICollection<TblAccessCard> TblAccessCards { get; set; } = new List<TblAccessCard>();

    public virtual ICollection<TblAlertMessageTag> TblAlertMessageTags { get; set; } = new List<TblAlertMessageTag>();

    public virtual ICollection<TblAlertMessageUser> TblAlertMessageUsers { get; set; } = new List<TblAlertMessageUser>();

    public virtual ICollection<TblAlertMessage> TblAlertMessages { get; set; } = new List<TblAlertMessage>();

    public virtual ICollection<TblAlertTag> TblAlertTags { get; set; } = new List<TblAlertTag>();

    public virtual ICollection<TblApprovalAlert> TblApprovalAlerts { get; set; } = new List<TblApprovalAlert>();

    public virtual ICollection<TblApprovalProcess> TblApprovalProcesses { get; set; } = new List<TblApprovalProcess>();

    public virtual ICollection<TblApprovalStageUser> TblApprovalStageUsers { get; set; } = new List<TblApprovalStageUser>();

    public virtual ICollection<TblApprovalStage> TblApprovalStages { get; set; } = new List<TblApprovalStage>();

    public virtual ICollection<TblApprovalTemplateTerm> TblApprovalTemplateTerms { get; set; } = new List<TblApprovalTemplateTerm>();

    public virtual ICollection<TblApprovalTemplateUser> TblApprovalTemplateUsers { get; set; } = new List<TblApprovalTemplateUser>();

    public virtual ICollection<TblApprovalTemplate> TblApprovalTemplates { get; set; } = new List<TblApprovalTemplate>();

    public virtual ICollection<TblArea> TblAreas { get; set; } = new List<TblArea>();

    public virtual ICollection<TblAssetAsset> TblAssetAssets { get; set; } = new List<TblAssetAsset>();

    public virtual ICollection<TblAssetAttachment> TblAssetAttachments { get; set; } = new List<TblAssetAttachment>();

    public virtual ICollection<TblAssetGroup> TblAssetGroups { get; set; } = new List<TblAssetGroup>();

    public virtual ICollection<TblAssetMaintenanceSchedule> TblAssetMaintenanceSchedules { get; set; } = new List<TblAssetMaintenanceSchedule>();

    public virtual ICollection<TblAssetPurchaseRequestAsset> TblAssetPurchaseRequestAssets { get; set; } = new List<TblAssetPurchaseRequestAsset>();

    public virtual ICollection<TblAssetPurchaseRequest> TblAssetPurchaseRequests { get; set; } = new List<TblAssetPurchaseRequest>();

    public virtual ICollection<TblAssetRepairAsset> TblAssetRepairAssets { get; set; } = new List<TblAssetRepairAsset>();

    public virtual ICollection<TblAssetRepair> TblAssetRepairs { get; set; } = new List<TblAssetRepair>();

    public virtual ICollection<TblAssetTransferAsset> TblAssetTransferAssets { get; set; } = new List<TblAssetTransferAsset>();

    public virtual ICollection<TblAssetTransfer> TblAssetTransfers { get; set; } = new List<TblAssetTransfer>();

    public virtual ICollection<TblAsset> TblAssets { get; set; } = new List<TblAsset>();

    public virtual ICollection<TblAttachmentTypeObjectType> TblAttachmentTypeObjectTypes { get; set; } = new List<TblAttachmentTypeObjectType>();

    public virtual ICollection<TblAttachment> TblAttachments { get; set; } = new List<TblAttachment>();

    public virtual ICollection<TblAttachmentsType> TblAttachmentsTypes { get; set; } = new List<TblAttachmentsType>();

    public virtual ICollection<TblAttestationType> TblAttestationTypes { get; set; } = new List<TblAttestationType>();

    public virtual ICollection<TblBank> TblBanks { get; set; } = new List<TblBank>();

    public virtual ICollection<TblCancellationProcessChecklist> TblCancellationProcessChecklists { get; set; } = new List<TblCancellationProcessChecklist>();

    public virtual ICollection<TblCancellationProcess> TblCancellationProcesses { get; set; } = new List<TblCancellationProcess>();

    public virtual ICollection<TblCancellationTypeChecklist> TblCancellationTypeChecklists { get; set; } = new List<TblCancellationTypeChecklist>();

    public virtual ICollection<TblCancellationType> TblCancellationTypes { get; set; } = new List<TblCancellationType>();

    public virtual ICollection<TblCertification> TblCertifications { get; set; } = new List<TblCertification>();

    public virtual ICollection<TblCity> TblCities { get; set; } = new List<TblCity>();

    public virtual ICollection<TblClass> TblClasses { get; set; } = new List<TblClass>();

    public virtual ICollection<TblCommission> TblCommissions { get; set; } = new List<TblCommission>();

    public virtual ICollection<TblContractType> TblContractTypes { get; set; } = new List<TblContractType>();

    public virtual ICollection<TblCostCenter> TblCostCenters { get; set; } = new List<TblCostCenter>();

    public virtual ICollection<TblCountry> TblCountries { get; set; } = new List<TblCountry>();

    public virtual ICollection<TblCustomerDependent> TblCustomerDependents { get; set; } = new List<TblCustomerDependent>();

    public virtual ICollection<TblCustomerRequest> TblCustomerRequests { get; set; } = new List<TblCustomerRequest>();

    public virtual ICollection<TblCustomerSponsor> TblCustomerSponsors { get; set; } = new List<TblCustomerSponsor>();

    public virtual ICollection<TblCustomer> TblCustomers { get; set; } = new List<TblCustomer>();

    public virtual ICollection<TblDashboard> TblDashboards { get; set; } = new List<TblDashboard>();

    public virtual ICollection<TblDimension> TblDimensions { get; set; } = new List<TblDimension>();

    public virtual ICollection<TblDocumentEmployee> TblDocumentEmployees { get; set; } = new List<TblDocumentEmployee>();

    public virtual ICollection<TblDocumentParking> TblDocumentParkings { get; set; } = new List<TblDocumentParking>();

    public virtual ICollection<TblDocumentParticular> TblDocumentParticulars { get; set; } = new List<TblDocumentParticular>();

    public virtual ICollection<TblDynamicDashboardDetail> TblDynamicDashboardDetails { get; set; } = new List<TblDynamicDashboardDetail>();

    public virtual ICollection<TblDynamicDashboard> TblDynamicDashboards { get; set; } = new List<TblDynamicDashboard>();

    public virtual ICollection<TblDynamicLayout> TblDynamicLayouts { get; set; } = new List<TblDynamicLayout>();

    public virtual ICollection<TblDynamicReport> TblDynamicReports { get; set; } = new List<TblDynamicReport>();

    public virtual ICollection<TblEmail> TblEmails { get; set; } = new List<TblEmail>();

    public virtual ICollection<TblEmployee> TblEmployees { get; set; } = new List<TblEmployee>();

    public virtual ICollection<TblFacility> TblFacilities { get; set; } = new List<TblFacility>();

    public virtual ICollection<TblGlaccountDeterminationDetail> TblGlaccountDeterminationDetails { get; set; } = new List<TblGlaccountDeterminationDetail>();

    public virtual ICollection<TblGlaccountDeterminationPriority> TblGlaccountDeterminationPriorities { get; set; } = new List<TblGlaccountDeterminationPriority>();

    public virtual ICollection<TblGlaccountDetermination> TblGlaccountDeterminations { get; set; } = new List<TblGlaccountDetermination>();

    public virtual ICollection<TblGlaccount> TblGlaccounts { get; set; } = new List<TblGlaccount>();

    public virtual ICollection<TblGoodsIssueItemSerialNumber> TblGoodsIssueItemSerialNumbers { get; set; } = new List<TblGoodsIssueItemSerialNumber>();

    public virtual ICollection<TblGoodsIssueItem> TblGoodsIssueItems { get; set; } = new List<TblGoodsIssueItem>();

    public virtual ICollection<TblGoodsIssue> TblGoodsIssues { get; set; } = new List<TblGoodsIssue>();

    public virtual ICollection<TblHseatmosphericMonitoring> TblHseatmosphericMonitorings { get; set; } = new List<TblHseatmosphericMonitoring>();

    public virtual ICollection<TblHsecondition> TblHseconditions { get; set; } = new List<TblHsecondition>();

    public virtual ICollection<TblHseexcavationWork> TblHseexcavationWorks { get; set; } = new List<TblHseexcavationWork>();

    public virtual ICollection<TblHseprocedure> TblHseprocedures { get; set; } = new List<TblHseprocedure>();

    public virtual ICollection<TblHsesafetyEquipment> TblHsesafetyEquipments { get; set; } = new List<TblHsesafetyEquipment>();

    public virtual ICollection<TblHseworkDescription> TblHseworkDescriptions { get; set; } = new List<TblHseworkDescription>();

    public virtual ICollection<TblIncomingPayment> TblIncomingPayments { get; set; } = new List<TblIncomingPayment>();

    public virtual ICollection<TblInventoryUnitMeasure> TblInventoryUnitMeasures { get; set; } = new List<TblInventoryUnitMeasure>();

    public virtual ICollection<TblItemGroup> TblItemGroups { get; set; } = new List<TblItemGroup>();

    public virtual ICollection<TblItemPurchaseRequestItem> TblItemPurchaseRequestItems { get; set; } = new List<TblItemPurchaseRequestItem>();

    public virtual ICollection<TblItemPurchaseRequest> TblItemPurchaseRequests { get; set; } = new List<TblItemPurchaseRequest>();

    public virtual ICollection<TblItemSerialNumber> TblItemSerialNumbers { get; set; } = new List<TblItemSerialNumber>();

    public virtual ICollection<TblItem> TblItems { get; set; } = new List<TblItem>();

    public virtual ICollection<TblJobTitle> TblJobTitles { get; set; } = new List<TblJobTitle>();

    public virtual ICollection<TblJournalEntry> TblJournalEntries { get; set; } = new List<TblJournalEntry>();

    public virtual ICollection<TblJournalEntryDetail> TblJournalEntryDetails { get; set; } = new List<TblJournalEntryDetail>();

    public virtual ICollection<TblKnowU> TblKnowUs { get; set; } = new List<TblKnowU>();

    public virtual ICollection<TblLegalEntityUser> TblLegalEntityUsers { get; set; } = new List<TblLegalEntityUser>();

    public virtual ICollection<TblLegend> TblLegends { get; set; } = new List<TblLegend>();

    public virtual ICollection<TblLookupType> TblLookupTypes { get; set; } = new List<TblLookupType>();

    public virtual ICollection<TblLookup> TblLookups { get; set; } = new List<TblLookup>();

    public virtual ICollection<TblMaintenanceTypeItem> TblMaintenanceTypeItems { get; set; } = new List<TblMaintenanceTypeItem>();

    public virtual ICollection<TblMaintenanceTypeTechncian> TblMaintenanceTypeTechncians { get; set; } = new List<TblMaintenanceTypeTechncian>();

    public virtual ICollection<TblMaintenanceTypeTool> TblMaintenanceTypeTools { get; set; } = new List<TblMaintenanceTypeTool>();

    public virtual ICollection<TblMaintenanceType> TblMaintenanceTypes { get; set; } = new List<TblMaintenanceType>();

    public virtual ICollection<TblManufacturer> TblManufacturers { get; set; } = new List<TblManufacturer>();

    public virtual ICollection<TblMaterialRequestItem> TblMaterialRequestItems { get; set; } = new List<TblMaterialRequestItem>();

    public virtual ICollection<TblMaterialRequest> TblMaterialRequests { get; set; } = new List<TblMaterialRequest>();

    public virtual ICollection<TblMaterialTransferItemSerialNumber> TblMaterialTransferItemSerialNumbers { get; set; } = new List<TblMaterialTransferItemSerialNumber>();

    public virtual ICollection<TblMaterialTransferItem> TblMaterialTransferItems { get; set; } = new List<TblMaterialTransferItem>();

    public virtual ICollection<TblMaterialTransfer> TblMaterialTransfers { get; set; } = new List<TblMaterialTransfer>();

    public virtual ICollection<TblMenu> TblMenus { get; set; } = new List<TblMenu>();

    public virtual ICollection<TblMeterReadingDetail> TblMeterReadingDetails { get; set; } = new List<TblMeterReadingDetail>();

    public virtual ICollection<TblMeterReading> TblMeterReadings { get; set; } = new List<TblMeterReading>();

    public virtual ICollection<TblMeterType> TblMeterTypes { get; set; } = new List<TblMeterType>();

    public virtual ICollection<TblMobileMessageMobileUser> TblMobileMessageMobileUsers { get; set; } = new List<TblMobileMessageMobileUser>();

    public virtual ICollection<TblMobileMessage> TblMobileMessages { get; set; } = new List<TblMobileMessage>();

    public virtual ICollection<TblMobileNews> TblMobileNews { get; set; } = new List<TblMobileNews>();

    public virtual ICollection<TblMobileUser> TblMobileUsers { get; set; } = new List<TblMobileUser>();

    public virtual ICollection<TblNeighborhood> TblNeighborhoods { get; set; } = new List<TblNeighborhood>();

    public virtual ICollection<TblObjectReference> TblObjectReferences { get; set; } = new List<TblObjectReference>();

    public virtual ICollection<TblParameter> TblParameters { get; set; } = new List<TblParameter>();

    public virtual ICollection<TblParking> TblParkings { get; set; } = new List<TblParking>();

    public virtual ICollection<TblParticular> TblParticulars { get; set; } = new List<TblParticular>();

    public virtual ICollection<TblPaymentTermInstallment> TblPaymentTermInstallments { get; set; } = new List<TblPaymentTermInstallment>();

    public virtual ICollection<TblPaymentTerm> TblPaymentTerms { get; set; } = new List<TblPaymentTerm>();

    public virtual ICollection<TblPortalResource> TblPortalResources { get; set; } = new List<TblPortalResource>();

    public virtual ICollection<TblPosition> TblPositions { get; set; } = new List<TblPosition>();

    public virtual ICollection<TblProperty> TblProperties { get; set; } = new List<TblProperty>();

    public virtual ICollection<TblPropertyAddress> TblPropertyAddresses { get; set; } = new List<TblPropertyAddress>();

    public virtual ICollection<TblPropertyFacility> TblPropertyFacilities { get; set; } = new List<TblPropertyFacility>();

    public virtual ICollection<TblPropertyGroup> TblPropertyGroups { get; set; } = new List<TblPropertyGroup>();

    public virtual ICollection<TblPropertyNeighborhood> TblPropertyNeighborhoods { get; set; } = new List<TblPropertyNeighborhood>();

    public virtual ICollection<TblPropertyProgress> TblPropertyProgresses { get; set; } = new List<TblPropertyProgress>();

    public virtual ICollection<TblRemark> TblRemarks { get; set; } = new List<TblRemark>();

    public virtual ICollection<TblRequiredAttachmentType> TblRequiredAttachmentTypes { get; set; } = new List<TblRequiredAttachmentType>();

    public virtual ICollection<TblRequiredAttestationType> TblRequiredAttestationTypes { get; set; } = new List<TblRequiredAttestationType>();

    public virtual ICollection<TblSalesContract> TblSalesContracts { get; set; } = new List<TblSalesContract>();

    public virtual ICollection<TblSalesInvoiceInstallment> TblSalesInvoiceInstallments { get; set; } = new List<TblSalesInvoiceInstallment>();

    public virtual ICollection<TblSalesInvoice> TblSalesInvoices { get; set; } = new List<TblSalesInvoice>();

    public virtual ICollection<TblSalesOrder> TblSalesOrders { get; set; } = new List<TblSalesOrder>();

    public virtual ICollection<TblSalesQuotation> TblSalesQuotations { get; set; } = new List<TblSalesQuotation>();

    public virtual ICollection<TblSm> TblSms { get; set; } = new List<TblSm>();

    public virtual ICollection<TblSmsResponse> TblSmsResponses { get; set; } = new List<TblSmsResponse>();

    public virtual ICollection<TblSparePartRepairRequestDetail> TblSparePartRepairRequestDetails { get; set; } = new List<TblSparePartRepairRequestDetail>();

    public virtual ICollection<TblSparePartRepairRequest> TblSparePartRepairRequests { get; set; } = new List<TblSparePartRepairRequest>();

    public virtual ICollection<TblSparePartRepairTransferDetail> TblSparePartRepairTransferDetails { get; set; } = new List<TblSparePartRepairTransferDetail>();

    public virtual ICollection<TblSparePartRepairTransfer> TblSparePartRepairTransfers { get; set; } = new List<TblSparePartRepairTransfer>();

    public virtual ICollection<TblSponsor> TblSponsors { get; set; } = new List<TblSponsor>();

    public virtual ICollection<TblState> TblStates { get; set; } = new List<TblState>();

    public virtual ICollection<TblSupplier> TblSuppliers { get; set; } = new List<TblSupplier>();

    public virtual ICollection<TblSystemAlert> TblSystemAlerts { get; set; } = new List<TblSystemAlert>();

    public virtual ICollection<TblTimeSheet> TblTimeSheets { get; set; } = new List<TblTimeSheet>();

    public virtual ICollection<TblToolTransferTool> TblToolTransferTools { get; set; } = new List<TblToolTransferTool>();

    public virtual ICollection<TblToolTransfer> TblToolTransfers { get; set; } = new List<TblToolTransfer>();

    public virtual ICollection<TblTool> TblTools { get; set; } = new List<TblTool>();

    public virtual ICollection<TblTransactionSource> TblTransactionSources { get; set; } = new List<TblTransactionSource>();

    public virtual ICollection<TblUnitClassParticular> TblUnitClassParticulars { get; set; } = new List<TblUnitClassParticular>();

    public virtual ICollection<TblUnitCounter> TblUnitCounters { get; set; } = new List<TblUnitCounter>();

    public virtual ICollection<TblUnitParking> TblUnitParkings { get; set; } = new List<TblUnitParking>();

    public virtual ICollection<TblUnitParticular> TblUnitParticulars { get; set; } = new List<TblUnitParticular>();

    public virtual ICollection<TblUnitRate> TblUnitRates { get; set; } = new List<TblUnitRate>();

    public virtual ICollection<TblUnitStatus> TblUnitStatuses { get; set; } = new List<TblUnitStatus>();

    public virtual ICollection<TblUnitUse> TblUnitUses { get; set; } = new List<TblUnitUse>();

    public virtual ICollection<TblUnitView> TblUnitViews { get; set; } = new List<TblUnitView>();

    public virtual ICollection<TblUnitVisit> TblUnitVisits { get; set; } = new List<TblUnitVisit>();

    public virtual ICollection<TblUnit> TblUnits { get; set; } = new List<TblUnit>();

    public virtual ICollection<TblUserGroupMenu> TblUserGroupMenus { get; set; } = new List<TblUserGroupMenu>();

    public virtual ICollection<TblUserGroupPermission> TblUserGroupPermissions { get; set; } = new List<TblUserGroupPermission>();

    public virtual ICollection<TblUserGroupUser> TblUserGroupUsers { get; set; } = new List<TblUserGroupUser>();

    public virtual ICollection<TblUserGroup> TblUserGroups { get; set; } = new List<TblUserGroup>();

    public virtual ICollection<TblUserPermission> TblUserPermissions { get; set; } = new List<TblUserPermission>();

    public virtual ICollection<TblWarehouseBinLocation> TblWarehouseBinLocations { get; set; } = new List<TblWarehouseBinLocation>();

    public virtual ICollection<TblWarehouseItem> TblWarehouseItems { get; set; } = new List<TblWarehouseItem>();

    public virtual ICollection<TblWarehouse> TblWarehouses { get; set; } = new List<TblWarehouse>();

    public virtual ICollection<TblWorkOrderAsset> TblWorkOrderAssets { get; set; } = new List<TblWorkOrderAsset>();

    public virtual ICollection<TblWorkOrderAttachment> TblWorkOrderAttachments { get; set; } = new List<TblWorkOrderAttachment>();

    public virtual ICollection<TblWorkOrderCategory> TblWorkOrderCategories { get; set; } = new List<TblWorkOrderCategory>();

    public virtual ICollection<TblWorkOrderExpense> TblWorkOrderExpenses { get; set; } = new List<TblWorkOrderExpense>();

    public virtual ICollection<TblWorkOrderGoodsIssue> TblWorkOrderGoodsIssues { get; set; } = new List<TblWorkOrderGoodsIssue>();

    public virtual ICollection<TblWorkOrderHseAtmosphericMonitoring> TblWorkOrderHseAtmosphericMonitorings { get; set; } = new List<TblWorkOrderHseAtmosphericMonitoring>();

    public virtual ICollection<TblWorkOrderHseCondition> TblWorkOrderHseConditions { get; set; } = new List<TblWorkOrderHseCondition>();

    public virtual ICollection<TblWorkOrderHseDeepExcavationControl> TblWorkOrderHseDeepExcavationControls { get; set; } = new List<TblWorkOrderHseDeepExcavationControl>();

    public virtual ICollection<TblWorkOrderHseExcavationSafetyEquipment> TblWorkOrderHseExcavationSafetyEquipments { get; set; } = new List<TblWorkOrderHseExcavationSafetyEquipment>();

    public virtual ICollection<TblWorkOrderHseExcavationWork> TblWorkOrderHseExcavationWorks { get; set; } = new List<TblWorkOrderHseExcavationWork>();

    public virtual ICollection<TblWorkOrderHseSafetyEquipment> TblWorkOrderHseSafetyEquipments { get; set; } = new List<TblWorkOrderHseSafetyEquipment>();

    public virtual ICollection<TblWorkOrderHseprocedure> TblWorkOrderHseprocedures { get; set; } = new List<TblWorkOrderHseprocedure>();

    public virtual ICollection<TblWorkOrderMaintenanceRequest> TblWorkOrderMaintenanceRequests { get; set; } = new List<TblWorkOrderMaintenanceRequest>();

    public virtual ICollection<TblWorkOrderMaintenanceType> TblWorkOrderMaintenanceTypes { get; set; } = new List<TblWorkOrderMaintenanceType>();

    public virtual ICollection<TblWorkOrderRemark> TblWorkOrderRemarks { get; set; } = new List<TblWorkOrderRemark>();

    public virtual ICollection<TblWorkOrderSparePart> TblWorkOrderSpareParts { get; set; } = new List<TblWorkOrderSparePart>();

    public virtual ICollection<TblWorkOrderTechnician> TblWorkOrderTechnicians { get; set; } = new List<TblWorkOrderTechnician>();

    public virtual ICollection<TblWorkOrderTool> TblWorkOrderTools { get; set; } = new List<TblWorkOrderTool>();

    public virtual ICollection<TblWorkOrder> TblWorkOrders { get; set; } = new List<TblWorkOrder>();

    public virtual ICollection<TblWorkType> TblWorkTypes { get; set; } = new List<TblWorkType>();

    public virtual ICollection<TblZone> TblZones { get; set; } = new List<TblZone>();
}
