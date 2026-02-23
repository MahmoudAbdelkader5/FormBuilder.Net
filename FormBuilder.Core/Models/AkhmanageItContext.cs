using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using formBuilder.Domian.Entitys;
using formBuilder.Domian.Entitys;

namespace FormBuilder.Core.Models;

public partial class AkhmanageItContext : DbContext
{
    public AkhmanageItContext()
    {
    }

    public AkhmanageItContext(DbContextOptions<AkhmanageItContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<TblAccessCard> TblAccessCards { get; set; }

    public virtual DbSet<TblAccessCardCar> TblAccessCardCars { get; set; }

    public virtual DbSet<TblAlertMessage> TblAlertMessages { get; set; }

    public virtual DbSet<TblAlertMessageTag> TblAlertMessageTags { get; set; }

    public virtual DbSet<TblAlertMessageUser> TblAlertMessageUsers { get; set; }

    public virtual DbSet<TblAlertTag> TblAlertTags { get; set; }

    public virtual DbSet<TblApplication> TblApplications { get; set; }

    public virtual DbSet<TblApplicationAlertMessage> TblApplicationAlertMessages { get; set; }

    public virtual DbSet<TblApplicationAlertTag> TblApplicationAlertTags { get; set; }

    public virtual DbSet<TblApplicationApprovalType> TblApplicationApprovalTypes { get; set; }

    public virtual DbSet<TblApplicationDynamicReport> TblApplicationDynamicReports { get; set; }

    public virtual DbSet<TblApplicationLookUp> TblApplicationLookUps { get; set; }

    public virtual DbSet<TblApplicationLookUpType> TblApplicationLookUpTypes { get; set; }

    public virtual DbSet<TblApplicationMenu> TblApplicationMenus { get; set; }

    public virtual DbSet<TblApplicationParameter> TblApplicationParameters { get; set; }

    public virtual DbSet<TblApplicationUserPermission> TblApplicationUserPermissions { get; set; }

    public virtual DbSet<TblApprovalAlert> TblApprovalAlerts { get; set; }

    public virtual DbSet<TblApprovalProcess> TblApprovalProcesses { get; set; }

    public virtual DbSet<TblApprovalStage> TblApprovalStages { get; set; }

    public virtual DbSet<TblApprovalStageUser> TblApprovalStageUsers { get; set; }

    public virtual DbSet<TblApprovalTemplate> TblApprovalTemplates { get; set; }

    public virtual DbSet<TblApprovalTemplateTerm> TblApprovalTemplateTerms { get; set; }

    public virtual DbSet<TblApprovalTemplateUser> TblApprovalTemplateUsers { get; set; }

    public virtual DbSet<TblArea> TblAreas { get; set; }

    public virtual DbSet<TblAsset> TblAssets { get; set; }

    public virtual DbSet<TblAssetAsset> TblAssetAssets { get; set; }

    public virtual DbSet<TblAssetAttachment> TblAssetAttachments { get; set; }

    public virtual DbSet<TblAssetGroup> TblAssetGroups { get; set; }

    public virtual DbSet<TblAssetMaintenanceSchedule> TblAssetMaintenanceSchedules { get; set; }

    public virtual DbSet<TblAssetPurchaseRequest> TblAssetPurchaseRequests { get; set; }

    public virtual DbSet<TblAssetPurchaseRequestAsset> TblAssetPurchaseRequestAssets { get; set; }

    public virtual DbSet<TblAssetRepair> TblAssetRepairs { get; set; }

    public virtual DbSet<TblAssetRepairAsset> TblAssetRepairAssets { get; set; }

    public virtual DbSet<TblAssetTransfer> TblAssetTransfers { get; set; }

    public virtual DbSet<TblAssetTransferAsset> TblAssetTransferAssets { get; set; }

    public virtual DbSet<TblAttachment> TblAttachments { get; set; }

    public virtual DbSet<TblAttachmentTypeObjectType> TblAttachmentTypeObjectTypes { get; set; }

    public virtual DbSet<TblAttachmentsType> TblAttachmentsTypes { get; set; }

    public virtual DbSet<TblAttestationType> TblAttestationTypes { get; set; }

    public virtual DbSet<TblBank> TblBanks { get; set; }

    public virtual DbSet<TblCancellationProcess> TblCancellationProcesses { get; set; }

    public virtual DbSet<TblCancellationProcessChecklist> TblCancellationProcessChecklists { get; set; }

    public virtual DbSet<TblCancellationType> TblCancellationTypes { get; set; }

    public virtual DbSet<TblCancellationTypeChecklist> TblCancellationTypeChecklists { get; set; }

    public virtual DbSet<TblCertification> TblCertifications { get; set; }

    public virtual DbSet<TblCity> TblCities { get; set; }

    public virtual DbSet<TblClass> TblClasses { get; set; }

    public virtual DbSet<TblCommission> TblCommissions { get; set; }

    public virtual DbSet<TblContractType> TblContractTypes { get; set; }

    public virtual DbSet<TblCostCenter> TblCostCenters { get; set; }

    public virtual DbSet<TblCountry> TblCountries { get; set; }

    public virtual DbSet<TblCustomer> TblCustomers { get; set; }

    public virtual DbSet<TblCustomerDependent> TblCustomerDependents { get; set; }

    public virtual DbSet<TblCustomerRequest> TblCustomerRequests { get; set; }

    public virtual DbSet<TblCustomerSponsor> TblCustomerSponsors { get; set; }

    public virtual DbSet<TblDashboard> TblDashboards { get; set; }

    public virtual DbSet<TblDimension> TblDimensions { get; set; }

    public virtual DbSet<TblDocumentAttestationType> TblDocumentAttestationTypes { get; set; }

    public virtual DbSet<TblDocumentEmployee> TblDocumentEmployees { get; set; }

    public virtual DbSet<TblDocumentParking> TblDocumentParkings { get; set; }

    public virtual DbSet<TblDocumentParticular> TblDocumentParticulars { get; set; }

    public virtual DbSet<TblDynamicDashboard> TblDynamicDashboards { get; set; }

    public virtual DbSet<TblDynamicDashboardDetail> TblDynamicDashboardDetails { get; set; }

    public virtual DbSet<TblDynamicLayout> TblDynamicLayouts { get; set; }

    public virtual DbSet<TblDynamicReport> TblDynamicReports { get; set; }

    public virtual DbSet<TblEmail> TblEmails { get; set; }

    public virtual DbSet<TblEmployee> TblEmployees { get; set; }

    public virtual DbSet<TblFacility> TblFacilities { get; set; }

    public virtual DbSet<TblGlaccount> TblGlaccounts { get; set; }

    public virtual DbSet<TblGlaccountDetermination> TblGlaccountDeterminations { get; set; }

    public virtual DbSet<TblGlaccountDeterminationDetail> TblGlaccountDeterminationDetails { get; set; }

    public virtual DbSet<TblGlaccountDeterminationPriority> TblGlaccountDeterminationPriorities { get; set; }

    public virtual DbSet<TblGoodsIssue> TblGoodsIssues { get; set; }

    public virtual DbSet<TblGoodsIssueItem> TblGoodsIssueItems { get; set; }

    public virtual DbSet<TblGoodsIssueItemSerialNumber> TblGoodsIssueItemSerialNumbers { get; set; }

    public virtual DbSet<TblHseatmosphericMonitoring> TblHseatmosphericMonitorings { get; set; }

    public virtual DbSet<TblHsecondition> TblHseconditions { get; set; }

    public virtual DbSet<TblHseexcavationWork> TblHseexcavationWorks { get; set; }

    public virtual DbSet<TblHseprocedure> TblHseprocedures { get; set; }

    public virtual DbSet<TblHsesafetyEquipment> TblHsesafetyEquipments { get; set; }

    public virtual DbSet<TblHseworkDescription> TblHseworkDescriptions { get; set; }

    public virtual DbSet<TblIncomingPayment> TblIncomingPayments { get; set; }

    public virtual DbSet<TblIncomingPaymentAccount> TblIncomingPaymentAccounts { get; set; }

    public virtual DbSet<TblIncomingPaymentCash> TblIncomingPaymentCashes { get; set; }

    public virtual DbSet<TblIncomingPaymentCheque> TblIncomingPaymentCheques { get; set; }

    public virtual DbSet<TblIncomingPaymentInstallment> TblIncomingPaymentInstallments { get; set; }

    public virtual DbSet<TblIncomingPaymentTransfer> TblIncomingPaymentTransfers { get; set; }

    public virtual DbSet<TblInventoryUnitMeasure> TblInventoryUnitMeasures { get; set; }

    public virtual DbSet<TblItem> TblItems { get; set; }

    public virtual DbSet<TblItemGroup> TblItemGroups { get; set; }

    public virtual DbSet<TblItemPurchaseRequest> TblItemPurchaseRequests { get; set; }

    public virtual DbSet<TblItemPurchaseRequestItem> TblItemPurchaseRequestItems { get; set; }

    public virtual DbSet<TblItemSerialNumber> TblItemSerialNumbers { get; set; }

    public virtual DbSet<TblJobTitle> TblJobTitles { get; set; }

    public virtual DbSet<TblJournalEntry> TblJournalEntries { get; set; }

    public virtual DbSet<TblJournalEntryDetail> TblJournalEntryDetails { get; set; }

    public virtual DbSet<TblKnowU> TblKnowUs { get; set; }

    public virtual DbSet<TblLegalEntity> TblLegalEntities { get; set; }

    public virtual DbSet<TblLegalEntityUser> TblLegalEntityUsers { get; set; }

    public virtual DbSet<TblLegend> TblLegends { get; set; }

    public virtual DbSet<TblLookup> TblLookups { get; set; }

    public virtual DbSet<TblLookupType> TblLookupTypes { get; set; }

    public virtual DbSet<TblMaintenanceType> TblMaintenanceTypes { get; set; }

    public virtual DbSet<TblMaintenanceTypeImport> TblMaintenanceTypeImports { get; set; }

    public virtual DbSet<TblMaintenanceTypeItem> TblMaintenanceTypeItems { get; set; }

    public virtual DbSet<TblMaintenanceTypeTechncian> TblMaintenanceTypeTechncians { get; set; }

    public virtual DbSet<TblMaintenanceTypeTool> TblMaintenanceTypeTools { get; set; }

    public virtual DbSet<TblManufacturer> TblManufacturers { get; set; }

    public virtual DbSet<TblMaterialRequest> TblMaterialRequests { get; set; }

    public virtual DbSet<TblMaterialRequestItem> TblMaterialRequestItems { get; set; }

    public virtual DbSet<TblMaterialTransfer> TblMaterialTransfers { get; set; }

    public virtual DbSet<TblMaterialTransferItem> TblMaterialTransferItems { get; set; }

    public virtual DbSet<TblMaterialTransferItemSerialNumber> TblMaterialTransferItemSerialNumbers { get; set; }

    public virtual DbSet<TblMenu> TblMenus { get; set; }

    public virtual DbSet<TblMeterReading> TblMeterReadings { get; set; }

    public virtual DbSet<TblMeterReadingDetail> TblMeterReadingDetails { get; set; }

    public virtual DbSet<TblMeterType> TblMeterTypes { get; set; }

    public virtual DbSet<TblMobileMessage> TblMobileMessages { get; set; }

    public virtual DbSet<TblMobileMessageMobileUser> TblMobileMessageMobileUsers { get; set; }

    public virtual DbSet<TblMobileNews> TblMobileNews { get; set; }

    public virtual DbSet<TblMobileUser> TblMobileUsers { get; set; }

    public virtual DbSet<TblNeighborhood> TblNeighborhoods { get; set; }

    public virtual DbSet<TblObjectReference> TblObjectReferences { get; set; }

    public virtual DbSet<TblParameter> TblParameters { get; set; }

    public virtual DbSet<TblParking> TblParkings { get; set; }

    public virtual DbSet<TblParticular> TblParticulars { get; set; }

    public virtual DbSet<TblPaymentTerm> TblPaymentTerms { get; set; }

    public virtual DbSet<TblPaymentTermInstallment> TblPaymentTermInstallments { get; set; }

    public virtual DbSet<TblPortalResource> TblPortalResources { get; set; }

    public virtual DbSet<TblPosition> TblPositions { get; set; }

    public virtual DbSet<TblProperty> TblProperties { get; set; }

    public virtual DbSet<TblPropertyAddress> TblPropertyAddresses { get; set; }

    public virtual DbSet<TblPropertyFacility> TblPropertyFacilities { get; set; }

    public virtual DbSet<TblPropertyGroup> TblPropertyGroups { get; set; }

    public virtual DbSet<TblPropertyNeighborhood> TblPropertyNeighborhoods { get; set; }

    public virtual DbSet<TblPropertyProgress> TblPropertyProgresses { get; set; }

    public virtual DbSet<TblRemark> TblRemarks { get; set; }

    public virtual DbSet<TblRequiredAttachmentType> TblRequiredAttachmentTypes { get; set; }

    public virtual DbSet<TblRequiredAttestationType> TblRequiredAttestationTypes { get; set; }

    public virtual DbSet<TblRevenueRecognitionMonthly> TblRevenueRecognitionMonthlies { get; set; }

    public virtual DbSet<TblSalesContract> TblSalesContracts { get; set; }

    public virtual DbSet<TblSalesInvoice> TblSalesInvoices { get; set; }

    public virtual DbSet<TblSalesInvoiceInstallment> TblSalesInvoiceInstallments { get; set; }

    public virtual DbSet<TblSalesOrder> TblSalesOrders { get; set; }

    public virtual DbSet<TblSalesQuotation> TblSalesQuotations { get; set; }

    public virtual DbSet<TblSecurityDepositReport> TblSecurityDepositReports { get; set; }

    public virtual DbSet<TblSm> TblSms { get; set; }

    public virtual DbSet<TblSmsResponse> TblSmsResponses { get; set; }

    public virtual DbSet<TblSparePartRepairRequest> TblSparePartRepairRequests { get; set; }

    public virtual DbSet<TblSparePartRepairRequestDetail> TblSparePartRepairRequestDetails { get; set; }

    public virtual DbSet<TblSparePartRepairTransfer> TblSparePartRepairTransfers { get; set; }

    public virtual DbSet<TblSparePartRepairTransferDetail> TblSparePartRepairTransferDetails { get; set; }

    public virtual DbSet<TblSponsor> TblSponsors { get; set; }

    public virtual DbSet<TblState> TblStates { get; set; }

    public virtual DbSet<TblSupplier> TblSuppliers { get; set; }

    public virtual DbSet<TblSystemAlert> TblSystemAlerts { get; set; }

    public virtual DbSet<TblTimeSheet> TblTimeSheets { get; set; }

    public virtual DbSet<TblTool> TblTools { get; set; }

    public virtual DbSet<TblToolTransfer> TblToolTransfers { get; set; }

    public virtual DbSet<TblToolTransferTool> TblToolTransferTools { get; set; }

    public virtual DbSet<TblTransactionSource> TblTransactionSources { get; set; }

    public virtual DbSet<TblUnit> TblUnits { get; set; }

    public virtual DbSet<TblUnitClassParticular> TblUnitClassParticulars { get; set; }

    public virtual DbSet<TblUnitCounter> TblUnitCounters { get; set; }

    public virtual DbSet<TblUnitParking> TblUnitParkings { get; set; }

    public virtual DbSet<TblUnitParticular> TblUnitParticulars { get; set; }

    public virtual DbSet<TblUnitRate> TblUnitRates { get; set; }

    public virtual DbSet<TblUnitStatus> TblUnitStatuses { get; set; }

    public virtual DbSet<TblUnitUse> TblUnitUses { get; set; }

    public virtual DbSet<TblUnitView> TblUnitViews { get; set; }

    public virtual DbSet<TblUnitVisit> TblUnitVisits { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblUserGroup> TblUserGroups { get; set; }

    public virtual DbSet<REFRESH_TOKENS> REFRESH_TOKENS { get; set; }

    public virtual DbSet<TblUserGroupMenu> TblUserGroupMenus { get; set; }

    public virtual DbSet<TblUserGroupPermission> TblUserGroupPermissions { get; set; }

    public virtual DbSet<TblUserGroupUser> TblUserGroupUsers { get; set; }

    public virtual DbSet<TblUserPermission> TblUserPermissions { get; set; }

    public virtual DbSet<TblWarehouse> TblWarehouses { get; set; }

    public virtual DbSet<TblWarehouseBinLocation> TblWarehouseBinLocations { get; set; }

    public virtual DbSet<TblWarehouseItem> TblWarehouseItems { get; set; }

    public virtual DbSet<TblWorkOrder> TblWorkOrders { get; set; }

    public virtual DbSet<TblWorkOrderAsset> TblWorkOrderAssets { get; set; }

    public virtual DbSet<TblWorkOrderAttachment> TblWorkOrderAttachments { get; set; }

    public virtual DbSet<TblWorkOrderCategory> TblWorkOrderCategories { get; set; }

    public virtual DbSet<TblWorkOrderExpense> TblWorkOrderExpenses { get; set; }

    public virtual DbSet<TblWorkOrderGoodsIssue> TblWorkOrderGoodsIssues { get; set; }

    public virtual DbSet<TblWorkOrderHseAtmosphericMonitoring> TblWorkOrderHseAtmosphericMonitorings { get; set; }

    public virtual DbSet<TblWorkOrderHseCondition> TblWorkOrderHseConditions { get; set; }

    public virtual DbSet<TblWorkOrderHseDeepExcavationControl> TblWorkOrderHseDeepExcavationControls { get; set; }

    public virtual DbSet<TblWorkOrderHseExcavationSafetyEquipment> TblWorkOrderHseExcavationSafetyEquipments { get; set; }

    public virtual DbSet<TblWorkOrderHseExcavationWork> TblWorkOrderHseExcavationWorks { get; set; }

    public virtual DbSet<TblWorkOrderHseSafetyEquipment> TblWorkOrderHseSafetyEquipments { get; set; }

    public virtual DbSet<TblWorkOrderHseprocedure> TblWorkOrderHseprocedures { get; set; }

    public virtual DbSet<TblWorkOrderMaintenanceRequest> TblWorkOrderMaintenanceRequests { get; set; }

    public virtual DbSet<TblWorkOrderMaintenanceType> TblWorkOrderMaintenanceTypes { get; set; }

    public virtual DbSet<TblWorkOrderRemark> TblWorkOrderRemarks { get; set; }

    public virtual DbSet<TblWorkOrderSparePart> TblWorkOrderSpareParts { get; set; }

    public virtual DbSet<TblWorkOrderTechnician> TblWorkOrderTechnicians { get; set; }

    public virtual DbSet<TblWorkOrderTool> TblWorkOrderTools { get; set; }

    public virtual DbSet<TblWorkType> TblWorkTypes { get; set; }

    public virtual DbSet<TblZone> TblZones { get; set; }

    public virtual DbSet<VwAccessCard> VwAccessCards { get; set; }

    public virtual DbSet<VwAccessCardCar> VwAccessCardCars { get; set; }

    public virtual DbSet<VwAlertMessage> VwAlertMessages { get; set; }

    public virtual DbSet<VwAlertMessageTag> VwAlertMessageTags { get; set; }

    public virtual DbSet<VwAlertMessageUser> VwAlertMessageUsers { get; set; }

    public virtual DbSet<VwApplicationAlertMessage> VwApplicationAlertMessages { get; set; }

    public virtual DbSet<VwApplicationAlertTag> VwApplicationAlertTags { get; set; }

    public virtual DbSet<VwApplicationApprovalType> VwApplicationApprovalTypes { get; set; }

    public virtual DbSet<VwApplicationDynamicReport> VwApplicationDynamicReports { get; set; }

    public virtual DbSet<VwApplicationLookUp> VwApplicationLookUps { get; set; }

    public virtual DbSet<VwApplicationLookUpType> VwApplicationLookUpTypes { get; set; }

    public virtual DbSet<VwApplicationMenu> VwApplicationMenus { get; set; }

    public virtual DbSet<VwApplicationParameter> VwApplicationParameters { get; set; }

    public virtual DbSet<VwApplicationUserPermission> VwApplicationUserPermissions { get; set; }

    public virtual DbSet<VwApprovalAlert> VwApprovalAlerts { get; set; }

    public virtual DbSet<VwApprovalStageUser> VwApprovalStageUsers { get; set; }

    public virtual DbSet<VwApprovalTemplate> VwApprovalTemplates { get; set; }

    public virtual DbSet<VwApprovalTemplateTerm> VwApprovalTemplateTerms { get; set; }

    public virtual DbSet<VwApprovalTemplateUser> VwApprovalTemplateUsers { get; set; }

    public virtual DbSet<VwArea> VwAreas { get; set; }

    public virtual DbSet<VwAsset> VwAssets { get; set; }

    public virtual DbSet<VwAssetGroup> VwAssetGroups { get; set; }

    public virtual DbSet<VwAssetMaintenanceSchedule> VwAssetMaintenanceSchedules { get; set; }

    public virtual DbSet<VwAssetPurchaseRequest> VwAssetPurchaseRequests { get; set; }

    public virtual DbSet<VwAssetPurchaseRequestAsset> VwAssetPurchaseRequestAssets { get; set; }

    public virtual DbSet<VwAssetRepair> VwAssetRepairs { get; set; }

    public virtual DbSet<VwAssetRepairAsset> VwAssetRepairAssets { get; set; }

    public virtual DbSet<VwAssetTransfer> VwAssetTransfers { get; set; }

    public virtual DbSet<VwAssetTransferAsset> VwAssetTransferAssets { get; set; }

    public virtual DbSet<VwAttachment> VwAttachments { get; set; }

    public virtual DbSet<VwAttachmentTypeObjectType> VwAttachmentTypeObjectTypes { get; set; }

    public virtual DbSet<VwBank> VwBanks { get; set; }

    public virtual DbSet<VwCancellationProcess> VwCancellationProcesses { get; set; }

    public virtual DbSet<VwCancellationProcessChecklist> VwCancellationProcessChecklists { get; set; }

    public virtual DbSet<VwCancellationType> VwCancellationTypes { get; set; }

    public virtual DbSet<VwCancellationTypeChecklist> VwCancellationTypeChecklists { get; set; }

    public virtual DbSet<VwCertification> VwCertifications { get; set; }

    public virtual DbSet<VwCity> VwCities { get; set; }

    public virtual DbSet<VwClass> VwClasses { get; set; }

    public virtual DbSet<VwCommission> VwCommissions { get; set; }

    public virtual DbSet<VwContractType> VwContractTypes { get; set; }

    public virtual DbSet<VwCostCenter> VwCostCenters { get; set; }

    public virtual DbSet<VwCustomer> VwCustomers { get; set; }

    public virtual DbSet<VwCustomerDependent> VwCustomerDependents { get; set; }

    public virtual DbSet<VwCustomerRequest> VwCustomerRequests { get; set; }

    public virtual DbSet<VwCustomerSponsor> VwCustomerSponsors { get; set; }

    public virtual DbSet<VwDashboard> VwDashboards { get; set; }

    public virtual DbSet<VwDocumentAttestationType> VwDocumentAttestationTypes { get; set; }

    public virtual DbSet<VwDocumentEmployee> VwDocumentEmployees { get; set; }

    public virtual DbSet<VwDocumentParking> VwDocumentParkings { get; set; }

    public virtual DbSet<VwDocumentParticular> VwDocumentParticulars { get; set; }

    public virtual DbSet<VwDynamicDashboardDetail> VwDynamicDashboardDetails { get; set; }

    public virtual DbSet<VwDynamicLayout> VwDynamicLayouts { get; set; }

    public virtual DbSet<VwEmployee> VwEmployees { get; set; }

    public virtual DbSet<VwGlaccount> VwGlaccounts { get; set; }

    public virtual DbSet<VwGlaccountDetermination> VwGlaccountDeterminations { get; set; }

    public virtual DbSet<VwGlaccountDeterminationDetail> VwGlaccountDeterminationDetails { get; set; }

    public virtual DbSet<VwGoodsIssue> VwGoodsIssues { get; set; }

    public virtual DbSet<VwGoodsIssueItem> VwGoodsIssueItems { get; set; }

    public virtual DbSet<VwGoodsIssueItemsSerialNumber> VwGoodsIssueItemsSerialNumbers { get; set; }

    public virtual DbSet<VwHseatmosphericMonitoring> VwHseatmosphericMonitorings { get; set; }

    public virtual DbSet<VwHsecondition> VwHseconditions { get; set; }

    public virtual DbSet<VwHseprocedure> VwHseprocedures { get; set; }

    public virtual DbSet<VwHsesafetyEquipment> VwHsesafetyEquipments { get; set; }

    public virtual DbSet<VwHseworkDescription> VwHseworkDescriptions { get; set; }

    public virtual DbSet<VwIncomingPayment> VwIncomingPayments { get; set; }

    public virtual DbSet<VwIncomingPaymentAccount> VwIncomingPaymentAccounts { get; set; }

    public virtual DbSet<VwIncomingPaymentCash> VwIncomingPaymentCashes { get; set; }

    public virtual DbSet<VwIncomingPaymentCheque> VwIncomingPaymentCheques { get; set; }

    public virtual DbSet<VwIncomingPaymentInstallment> VwIncomingPaymentInstallments { get; set; }

    public virtual DbSet<VwIncomingPaymentTransfer> VwIncomingPaymentTransfers { get; set; }

    public virtual DbSet<VwItem> VwItems { get; set; }

    public virtual DbSet<VwItemPurchaseRequest> VwItemPurchaseRequests { get; set; }

    public virtual DbSet<VwItemPurchaseRequestItem> VwItemPurchaseRequestItems { get; set; }

    public virtual DbSet<VwItemSerialNumber> VwItemSerialNumbers { get; set; }

    public virtual DbSet<VwJobTitle> VwJobTitles { get; set; }

    public virtual DbSet<VwJournalEntry> VwJournalEntries { get; set; }

    public virtual DbSet<VwJournalEntryDetail> VwJournalEntryDetails { get; set; }

    public virtual DbSet<VwLegalEntity> VwLegalEntities { get; set; }

    public virtual DbSet<VwLegalEntityUser> VwLegalEntityUsers { get; set; }

    public virtual DbSet<VwLookup> VwLookups { get; set; }

    public virtual DbSet<VwMaintenanceType> VwMaintenanceTypes { get; set; }

    public virtual DbSet<VwMaintenanceTypeItem> VwMaintenanceTypeItems { get; set; }

    public virtual DbSet<VwMaintenanceTypeTechnician> VwMaintenanceTypeTechnicians { get; set; }

    public virtual DbSet<VwMaintenanceTypeTool> VwMaintenanceTypeTools { get; set; }

    public virtual DbSet<VwMaterialRequest> VwMaterialRequests { get; set; }

    public virtual DbSet<VwMaterialRequestItem> VwMaterialRequestItems { get; set; }

    public virtual DbSet<VwMaterialTransfer> VwMaterialTransfers { get; set; }

    public virtual DbSet<VwMaterialTransferItem> VwMaterialTransferItems { get; set; }

    public virtual DbSet<VwMaterialTransferItemsSerialNumber> VwMaterialTransferItemsSerialNumbers { get; set; }

    public virtual DbSet<VwMenu> VwMenus { get; set; }

    public virtual DbSet<VwMeterReading> VwMeterReadings { get; set; }

    public virtual DbSet<VwMeterReadingDetail> VwMeterReadingDetails { get; set; }

    public virtual DbSet<VwMobileUser> VwMobileUsers { get; set; }

    public virtual DbSet<VwObjectReference> VwObjectReferences { get; set; }

    public virtual DbSet<VwParameter> VwParameters { get; set; }

    public virtual DbSet<VwParking> VwParkings { get; set; }

    public virtual DbSet<VwParticular> VwParticulars { get; set; }

    public virtual DbSet<VwPaymentTermInstallment> VwPaymentTermInstallments { get; set; }

    public virtual DbSet<VwPermissionUser> VwPermissionUsers { get; set; }

    public virtual DbSet<VwProperty> VwProperties { get; set; }

    public virtual DbSet<VwPropertyAddress> VwPropertyAddresses { get; set; }

    public virtual DbSet<VwPropertyFacility> VwPropertyFacilities { get; set; }

    public virtual DbSet<VwPropertyGroup> VwPropertyGroups { get; set; }

    public virtual DbSet<VwPropertyNeighborhood> VwPropertyNeighborhoods { get; set; }

    public virtual DbSet<VwPropertyProgress> VwPropertyProgresses { get; set; }

    public virtual DbSet<VwRemark> VwRemarks { get; set; }

    public virtual DbSet<VwRequiredAttachmentType> VwRequiredAttachmentTypes { get; set; }

    public virtual DbSet<VwRequiredAttestationType> VwRequiredAttestationTypes { get; set; }

    public virtual DbSet<VwRevenueRecognitionMonthly> VwRevenueRecognitionMonthlies { get; set; }

    public virtual DbSet<VwSalesContract> VwSalesContracts { get; set; }

    public virtual DbSet<VwSalesInvoice> VwSalesInvoices { get; set; }

    public virtual DbSet<VwSalesInvoiceInstallment> VwSalesInvoiceInstallments { get; set; }

    public virtual DbSet<VwSalesOrder> VwSalesOrders { get; set; }

    public virtual DbSet<VwSalesQuotation> VwSalesQuotations { get; set; }

    public virtual DbSet<VwSecurityDepositReport> VwSecurityDepositReports { get; set; }

    public virtual DbSet<VwSparePartRepairRequest> VwSparePartRepairRequests { get; set; }

    public virtual DbSet<VwSparePartRepairRequestDetail> VwSparePartRepairRequestDetails { get; set; }

    public virtual DbSet<VwSparePartRepairTransfer> VwSparePartRepairTransfers { get; set; }

    public virtual DbSet<VwSparePartRepairTransferDetail> VwSparePartRepairTransferDetails { get; set; }

    public virtual DbSet<VwSponsor> VwSponsors { get; set; }

    public virtual DbSet<VwState> VwStates { get; set; }

    public virtual DbSet<VwSystemAlert> VwSystemAlerts { get; set; }

    public virtual DbSet<VwTimeSheet> VwTimeSheets { get; set; }

    public virtual DbSet<VwToolTransfer> VwToolTransfers { get; set; }

    public virtual DbSet<VwToolTransferTool> VwToolTransferTools { get; set; }

    public virtual DbSet<VwUnit> VwUnits { get; set; }

    public virtual DbSet<VwUnitClassParticular> VwUnitClassParticulars { get; set; }

    public virtual DbSet<VwUnitCounter> VwUnitCounters { get; set; }

    public virtual DbSet<VwUnitParking> VwUnitParkings { get; set; }

    public virtual DbSet<VwUnitParticular> VwUnitParticulars { get; set; }

    public virtual DbSet<VwUnitRate> VwUnitRates { get; set; }

    public virtual DbSet<VwUnitSalesContract> VwUnitSalesContracts { get; set; }

    public virtual DbSet<VwUnitVisit> VwUnitVisits { get; set; }

    public virtual DbSet<VwUser> VwUsers { get; set; }

    public virtual DbSet<VwUserGroupMenu> VwUserGroupMenus { get; set; }

    public virtual DbSet<VwUserGroupPermission> VwUserGroupPermissions { get; set; }

    public virtual DbSet<VwUserGroupUser> VwUserGroupUsers { get; set; }

    public virtual DbSet<VwUserMenu> VwUserMenus { get; set; }

    public virtual DbSet<VwWarehouseBinLocation> VwWarehouseBinLocations { get; set; }

    public virtual DbSet<VwWarehouseItem> VwWarehouseItems { get; set; }

    public virtual DbSet<VwWorkOrder> VwWorkOrders { get; set; }

    public virtual DbSet<VwWorkOrderAsset> VwWorkOrderAssets { get; set; }

    public virtual DbSet<VwWorkOrderAttachment> VwWorkOrderAttachments { get; set; }

    public virtual DbSet<VwWorkOrderCategory> VwWorkOrderCategories { get; set; }

    public virtual DbSet<VwWorkOrderExpense> VwWorkOrderExpenses { get; set; }

    public virtual DbSet<VwWorkOrderGoodsIssue> VwWorkOrderGoodsIssues { get; set; }

    public virtual DbSet<VwWorkOrderHseatmosphericMonitoring> VwWorkOrderHseatmosphericMonitorings { get; set; }

    public virtual DbSet<VwWorkOrderHsecondition> VwWorkOrderHseconditions { get; set; }

    public virtual DbSet<VwWorkOrderHsedeepExcavationControl> VwWorkOrderHsedeepExcavationControls { get; set; }

    public virtual DbSet<VwWorkOrderHseexcavationSafetyEquipment> VwWorkOrderHseexcavationSafetyEquipments { get; set; }

    public virtual DbSet<VwWorkOrderHseexcavationWork> VwWorkOrderHseexcavationWorks { get; set; }

    public virtual DbSet<VwWorkOrderHseprocedure> VwWorkOrderHseprocedures { get; set; }

    public virtual DbSet<VwWorkOrderHsesafetyEquipment> VwWorkOrderHsesafetyEquipments { get; set; }

    public virtual DbSet<VwWorkOrderLog> VwWorkOrderLogs { get; set; }

    public virtual DbSet<VwWorkOrderMaintenanceRequest> VwWorkOrderMaintenanceRequests { get; set; }

    public virtual DbSet<VwWorkOrderMaintenanceType> VwWorkOrderMaintenanceTypes { get; set; }

    public virtual DbSet<VwWorkOrderRemark> VwWorkOrderRemarks { get; set; }

    public virtual DbSet<VwWorkOrderSparePart> VwWorkOrderSpareParts { get; set; }

    public virtual DbSet<VwWorkOrderTechnician> VwWorkOrderTechnicians { get; set; }

    public virtual DbSet<VwWorkOrderTool> VwWorkOrderTools { get; set; }

  

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("Code ");
            entity.Property(e => e.InventoryUnitOfMeasure)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("Inventory Unit of Measure");
            entity.Property(e => e.ItemGroup)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("Item Group");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<TblAccessCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AccessCard");

            entity.ToTable("Tbl_AccessCard");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdContractNavigation).WithMany(p => p.TblAccessCards)
                .HasForeignKey(d => d.IdContract)
                .HasConstraintName("FK_AccessCard_Contract");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.TblAccessCards)
                .HasForeignKey(d => d.IdCustomer)
                .HasConstraintName("FK_AccessCard_Customer");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAccessCards)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_AccessCard_LegalEntity");

            entity.HasOne(d => d.IdPropertyNavigation).WithMany(p => p.TblAccessCards)
                .HasForeignKey(d => d.IdProperty)
                .HasConstraintName("FK_AccessCard_Property");

            entity.HasOne(d => d.IdUnitNavigation).WithMany(p => p.TblAccessCards)
                .HasForeignKey(d => d.IdUnit)
                .HasConstraintName("FK_AccessCard_Unit");
        });

        modelBuilder.Entity<TblAccessCardCar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AccessCard_Car");

            entity.ToTable("Tbl_AccessCard_Car");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Color)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Make)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Model)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParkingLevel)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PlateNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RegisteredIn)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RegistrationNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SpaceNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdAccessCardNavigation).WithMany(p => p.TblAccessCardCars)
                .HasForeignKey(d => d.IdAccessCard)
                .HasConstraintName("FK_AccessCard_Car_AccessCard");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAccessCardCars)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_AccessCard_Car_LegalEntity");
        });

        modelBuilder.Entity<TblAlertMessage>(entity =>
        {
            entity.ToTable("Tbl_AlertMessage");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Body).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignBody).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignScreen)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignSubject)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Screen)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Subject)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAlertMessages)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_AlertMessage_LegalEntity");
        });

        modelBuilder.Entity<TblAlertMessageTag>(entity =>
        {
            entity.HasKey(e => new { e.IdAlertMessage, e.IdAlertTag });

            entity.ToTable("Tbl_AlertMessage_Tag");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdAlertMessageNavigation).WithMany(p => p.TblAlertMessageTags)
                .HasForeignKey(d => d.IdAlertMessage)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AlertMessage_Tag_AlertMessage");

            entity.HasOne(d => d.IdAlertTagNavigation).WithMany(p => p.TblAlertMessageTags)
                .HasForeignKey(d => d.IdAlertTag)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AlertMessage_Tag_AlertTag");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAlertMessageTags)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_AlertMessage_Tag_LegalEntity");
        });

        modelBuilder.Entity<TblAlertMessageUser>(entity =>
        {
            entity.HasKey(e => new { e.IdAlertMessage, e.IdUser });

            entity.ToTable("Tbl_AlertMessage_User");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdAlertMessageNavigation).WithMany(p => p.TblAlertMessageUsers)
                .HasForeignKey(d => d.IdAlertMessage)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AlertMessage_User_AlertMessage");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAlertMessageUsers)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_AlertMessage_User_LegalEntity");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TblAlertMessageUsers)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AlertMessage_User_User");
        });

        modelBuilder.Entity<TblAlertTag>(entity =>
        {
            entity.ToTable("Tbl_AlertTag");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDescription)
                .HasMaxLength(1000)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAlertTags)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_AlertTag_LegalEntity");
        });

        modelBuilder.Entity<TblApplication>(entity =>
        {
            entity.ToTable("Tbl_Application");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<TblApplicationAlertMessage>(entity =>
        {
            entity.HasKey(e => new { e.IdApplication, e.IdAlertMessage });

            entity.ToTable("Tbl_Application_AlertMessage");
        });

        modelBuilder.Entity<TblApplicationAlertTag>(entity =>
        {
            entity.HasKey(e => new { e.IdApplication, e.IdAlertTag }).HasName("PK_Tbl_Application_AlertMessageTag");

            entity.ToTable("Tbl_Application_AlertTag");
        });

        modelBuilder.Entity<TblApplicationApprovalType>(entity =>
        {
            entity.HasKey(e => new { e.IdApplication, e.IdApprovalType });

            entity.ToTable("Tbl_Application_ApprovalType");
        });

        modelBuilder.Entity<TblApplicationDynamicReport>(entity =>
        {
            entity.HasKey(e => new { e.IdApplication, e.IdDynamicReport });

            entity.ToTable("Tbl_Application_DynamicReport");
        });

        modelBuilder.Entity<TblApplicationLookUp>(entity =>
        {
            entity.HasKey(e => new { e.IdApplication, e.IdLookUp, e.IdLookUpType });

            entity.ToTable("Tbl_Application_LookUp");
        });

        modelBuilder.Entity<TblApplicationLookUpType>(entity =>
        {
            entity.HasKey(e => new { e.IdApplication, e.IdLookUpType });

            entity.ToTable("Tbl_Application_LookUpType");
        });

        modelBuilder.Entity<TblApplicationMenu>(entity =>
        {
            entity.HasKey(e => new { e.IdApplication, e.IdMenu });

            entity.ToTable("Tbl_Application_Menu");
        });

        modelBuilder.Entity<TblApplicationParameter>(entity =>
        {
            entity.HasKey(e => new { e.IdApplication, e.IdParameter });

            entity.ToTable("Tbl_Application_Parameter");
        });

        modelBuilder.Entity<TblApplicationUserPermission>(entity =>
        {
            entity.HasKey(e => new { e.IdApplication, e.UserPermissionName });

            entity.ToTable("Tbl_Application_UserPermission");

            entity.Property(e => e.UserPermissionName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<TblApprovalAlert>(entity =>
        {
            entity.ToTable("Tbl_ApprovalAlert");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AlertDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDescription).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignSubject)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Note).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusDate).HasColumnType("datetime");
            entity.Property(e => e.Subject)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");

            entity.HasOne(d => d.IdApprovalProcessNavigation).WithMany(p => p.TblApprovalAlerts)
                .HasForeignKey(d => d.IdApprovalProcess)
                .HasConstraintName("FK_ApprovalAlert_ApprovalProcess");

            entity.HasOne(d => d.IdApprovalTemplateNavigation).WithMany(p => p.TblApprovalAlerts)
                .HasForeignKey(d => d.IdApprovalTemplate)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApprovalAlert_ApprovalTemplate");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblApprovalAlerts)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_ApprovalAlert_LegalEntity");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TblApprovalAlerts)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_ApprovalAlert_User");
        });

        modelBuilder.Entity<TblApprovalProcess>(entity =>
        {
            entity.ToTable("Tbl_ApprovalProcess");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Value).HasColumnType("decimal(18, 6)");

            entity.HasOne(d => d.IdApprovalStageNavigation).WithMany(p => p.TblApprovalProcesses)
                .HasForeignKey(d => d.IdApprovalStage)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApprovalProcess_ApprovalStage");

            entity.HasOne(d => d.IdApprovalTemplateNavigation).WithMany(p => p.TblApprovalProcesses)
                .HasForeignKey(d => d.IdApprovalTemplate)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApprovalProcess_ApprovalTemplate");

            entity.HasOne(d => d.IdCurrentUserNavigation).WithMany(p => p.TblApprovalProcesses)
                .HasForeignKey(d => d.IdCurrentUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApprovalProcess_CurrentUser");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblApprovalProcesses)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_ApprovalProcess_LegalEntity");
        });

        modelBuilder.Entity<TblApprovalStage>(entity =>
        {
            entity.ToTable("Tbl_ApprovalStage");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblApprovalStages)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_ApprovalStage_LegalEntity");
        });

        modelBuilder.Entity<TblApprovalStageUser>(entity =>
        {
            entity.HasKey(e => new { e.IdApprovalStage, e.IdUser }).HasName("PK_ApprovalStage_User");

            entity.ToTable("Tbl_ApprovalStage_User");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdApprovalStageNavigation).WithMany(p => p.TblApprovalStageUsers)
                .HasForeignKey(d => d.IdApprovalStage)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApprovalStage_User_ApprovalStage");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblApprovalStageUsers)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_ApprovalStage_User_LegalEntity");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TblApprovalStageUsers)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApprovalStage_User_User");
        });

        modelBuilder.Entity<TblApprovalTemplate>(entity =>
        {
            entity.ToTable("Tbl_ApprovalTemplate");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Query).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdApprovalStageNavigation).WithMany(p => p.TblApprovalTemplates)
                .HasForeignKey(d => d.IdApprovalStage)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApprovalTemplate_ApprovalStage");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblApprovalTemplates)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_ApprovalTemplate_LegalEntity");
        });

        modelBuilder.Entity<TblApprovalTemplateTerm>(entity =>
        {
            entity.HasKey(e => new { e.IdApprovalTemplate, e.IdApprovalTerm });

            entity.ToTable("Tbl_ApprovalTemplate_Term");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FromValue).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ToValue).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdApprovalTemplateNavigation).WithMany(p => p.TblApprovalTemplateTerms)
                .HasForeignKey(d => d.IdApprovalTemplate)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApprovalTemplate_Term_ApprovalTemplate");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblApprovalTemplateTerms)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_ApprovalTemplate_Term_LegalEntity");
        });

        modelBuilder.Entity<TblApprovalTemplateUser>(entity =>
        {
            entity.HasKey(e => new { e.IdApprovalTemplate, e.IdUser });

            entity.ToTable("Tbl_ApprovalTemplate_User");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdApprovalTemplateNavigation).WithMany(p => p.TblApprovalTemplateUsers)
                .HasForeignKey(d => d.IdApprovalTemplate)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApprovalTemplate_User_ApprovalTemplate");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblApprovalTemplateUsers)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_ApprovalTemplate_User_LegalEntity");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TblApprovalTemplateUsers)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApprovalTemplate_User_User");
        });

        modelBuilder.Entity<TblArea>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Area");

            entity.ToTable("Tbl_Area");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdCityNavigation).WithMany(p => p.TblAreas)
                .HasForeignKey(d => d.IdCity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Area_City");

            entity.HasOne(d => d.IdCountryNavigation).WithMany(p => p.TblAreas)
                .HasForeignKey(d => d.IdCountry)
                .HasConstraintName("FK_Area_Country");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAreas)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Area_LegalEntity");

            entity.HasOne(d => d.IdStateNavigation).WithMany(p => p.TblAreas)
                .HasForeignKey(d => d.IdState)
                .HasConstraintName("FK_Area_State");
        });

        modelBuilder.Entity<TblAsset>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Asse__3214EC2778A885AB");

            entity.ToTable("Tbl_Asset");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Barcode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SerialNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ServiceContractExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.ServiceContractRemark).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Vender)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WarrantyExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.WarrantyRemark).UseCollation("SQL_Latin1_General_CP1256_CS_AS");

            entity.HasOne(d => d.IdCostCenter1Navigation).WithMany(p => p.TblAssetIdCostCenter1Navigations)
                .HasForeignKey(d => d.IdCostCenter1)
                .HasConstraintName("FK_Asset_CostCenter1");

            entity.HasOne(d => d.IdCostCenter2Navigation).WithMany(p => p.TblAssetIdCostCenter2Navigations)
                .HasForeignKey(d => d.IdCostCenter2)
                .HasConstraintName("FK_Asset_CostCenter2");

            entity.HasOne(d => d.IdCostCenter3Navigation).WithMany(p => p.TblAssetIdCostCenter3Navigations)
                .HasForeignKey(d => d.IdCostCenter3)
                .HasConstraintName("FK_Asset_CostCenter3");

            entity.HasOne(d => d.IdCostCenter4Navigation).WithMany(p => p.TblAssetIdCostCenter4Navigations)
                .HasForeignKey(d => d.IdCostCenter4)
                .HasConstraintName("FK_Asset_CostCenter4");

            entity.HasOne(d => d.IdCostCenter5Navigation).WithMany(p => p.TblAssetIdCostCenter5Navigations)
                .HasForeignKey(d => d.IdCostCenter5)
                .HasConstraintName("FK_Asset_CostCenter5");

            entity.HasOne(d => d.IdGroupNavigation).WithMany(p => p.TblAssets)
                .HasForeignKey(d => d.IdGroup)
                .HasConstraintName("FK_Tbl_Asset_AssetGroup");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAssets)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Asset_LegalEntity");

            entity.HasOne(d => d.IdManufacturerNavigation).WithMany(p => p.TblAssets)
                .HasForeignKey(d => d.IdManufacturer)
                .HasConstraintName("FK_Asset_Manufacturer");

            entity.HasOne(d => d.IdTypeNavigation).WithMany(p => p.TblAssets)
                .HasForeignKey(d => d.IdType)
                .HasConstraintName("FK_Asset_Type");

            entity.HasOne(d => d.IdZoneNavigation).WithMany(p => p.TblAssets)
                .HasForeignKey(d => d.IdZone)
                .HasConstraintName("FK_Asset_Zone");
        });

        modelBuilder.Entity<TblAssetAsset>(entity =>
        {
            entity.HasKey(e => new { e.IdAsset, e.IdParent }).HasName("PK__Tbl_Asse__1A031226ABC38CF1");

            entity.ToTable("Tbl_Asset_Asset");

            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAssetAssets)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Asset_Asset_LegalEntity");
        });

        modelBuilder.Entity<TblAssetAttachment>(entity =>
        {
            entity.HasKey(e => new { e.IdAsset, e.IdDocument }).HasName("PK_Asset_Attachment");

            entity.ToTable("Tbl_Asset_Attachment");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAssetAttachments)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Asset_Attachment_LegalEntity");
        });

        modelBuilder.Entity<TblAssetGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AssetGroup");

            entity.ToTable("Tbl_AssetGroup");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAssetGroups)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_AssetGroup_LegalEntity");
        });

        modelBuilder.Entity<TblAssetMaintenanceSchedule>(entity =>
        {
            entity.ToTable("Tbl_Asset_MaintenanceSchedule");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastMaintenanceDate).HasColumnType("datetime");
            entity.Property(e => e.LastMaintenanceReading).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LastMeterReading).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NextMaintenanceDate).HasColumnType("datetime");
            entity.Property(e => e.SchedulerLimit).HasColumnType("decimal(18, 6)");

            entity.HasOne(d => d.IdAssetNavigation).WithMany(p => p.TblAssetMaintenanceSchedules)
                .HasForeignKey(d => d.IdAsset)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_Asset_MaintenanceSchedule_Asset");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAssetMaintenanceSchedules)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Asset_MaintenanceSchedule_LegalEntity");

            entity.HasOne(d => d.IdMaintenanceTypeNavigation).WithMany(p => p.TblAssetMaintenanceSchedules)
                .HasForeignKey(d => d.IdMaintenanceType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_Asset_MaintenanceSchedule_MaintenanceType");

            entity.HasOne(d => d.IdMeterTypeNavigation).WithMany(p => p.TblAssetMaintenanceSchedules)
                .HasForeignKey(d => d.IdMeterType)
                .HasConstraintName("FK_Asset_MaintenanceSchedule_MeterType");

            entity.HasOne(d => d.IdWorkOrderNavigation).WithMany(p => p.TblAssetMaintenanceSchedules)
                .HasForeignKey(d => d.IdWorkOrder)
                .HasConstraintName("FK_Asset_MaintenanceSchedule_WorkOrder");
        });

        modelBuilder.Entity<TblAssetPurchaseRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AssetPurchaseRequest");

            entity.ToTable("Tbl_AssetPurchaseRequest");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.StoreRemarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAssetPurchaseRequests)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_AssetPurchaseRequest_LegalEntity");

            entity.HasOne(d => d.IdTechncianNavigation).WithMany(p => p.TblAssetPurchaseRequests)
                .HasForeignKey(d => d.IdTechncian)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_AssetPurchaseRequest_User");
        });

        modelBuilder.Entity<TblAssetPurchaseRequestAsset>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AssetPurchaseRequest_Assets");

            entity.ToTable("Tbl_AssetPurchaseRequest_Assets");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AssetDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SerialNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitOfMeasure)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdAssetPurchaseRequestNavigation).WithMany(p => p.TblAssetPurchaseRequestAssets)
                .HasForeignKey(d => d.IdAssetPurchaseRequest)
                .HasConstraintName("FK_AssetPurchaseRequest_Assets_AssetPurchaseRequest");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAssetPurchaseRequestAssets)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_AssetPurchaseRequest_Assets_LegalEntity");

            entity.HasOne(d => d.IdManufacturerNavigation).WithMany(p => p.TblAssetPurchaseRequestAssets)
                .HasForeignKey(d => d.IdManufacturer)
                .HasConstraintName("FK_AssetPurchaseRequest_Assets_Manufacturer");

            entity.HasOne(d => d.IdSupplierNavigation).WithMany(p => p.TblAssetPurchaseRequestAssets)
                .HasForeignKey(d => d.IdSupplier)
                .HasConstraintName("FK_AssetPurchaseRequest_Assets_Supplier");
        });

        modelBuilder.Entity<TblAssetRepair>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AssetRepair");

            entity.ToTable("Tbl_AssetRepair");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StoreRemarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAssetRepairs)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_AssetRepair_LegalEntity");

            entity.HasOne(d => d.IdTechncianNavigation).WithMany(p => p.TblAssetRepairs)
                .HasForeignKey(d => d.IdTechncian)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_AssetRepair_User");

            entity.HasOne(d => d.IdWorkOrderNavigation).WithMany(p => p.TblAssetRepairs)
                .HasForeignKey(d => d.IdWorkOrder)
                .HasConstraintName("FK_Tbl_AssetRepair_WorkOrder");
        });

        modelBuilder.Entity<TblAssetRepairAsset>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AssetRepair_Assets");

            entity.ToTable("Tbl_AssetRepair_Assets");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PurchaseRequestCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdAssetNavigation).WithMany(p => p.TblAssetRepairAssets)
                .HasForeignKey(d => d.IdAsset)
                .HasConstraintName("FK_AssetRepair_Assets_Asset");

            entity.HasOne(d => d.IdAssetRepairNavigation).WithMany(p => p.TblAssetRepairAssets)
                .HasForeignKey(d => d.IdAssetRepair)
                .HasConstraintName("FK_AssetRepair_Assets_AssetRepair");

            entity.HasOne(d => d.IdFromWarehouseNavigation).WithMany(p => p.TblAssetRepairAssetIdFromWarehouseNavigations)
                .HasForeignKey(d => d.IdFromWarehouse)
                .HasConstraintName("FK_AssetRepair_FromWarehouse_Assets");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAssetRepairAssets)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_AssetRepair_Assets_LegalEntity");

            entity.HasOne(d => d.IdToWarehouseNavigation).WithMany(p => p.TblAssetRepairAssetIdToWarehouseNavigations)
                .HasForeignKey(d => d.IdToWarehouse)
                .HasConstraintName("FK_AssetRepair_Assets_ToWarehouse");

            entity.HasOne(d => d.IdWorkOrderNavigation).WithMany(p => p.TblAssetRepairAssets)
                .HasForeignKey(d => d.IdWorkOrder)
                .HasConstraintName("FK_AssetRepair_Assets_WorkOrder");
        });

        modelBuilder.Entity<TblAssetTransfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AssetTransfer");

            entity.ToTable("Tbl_AssetTransfer");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAssetTransfers)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_AssetTransfer_LegalEntity");
        });

        modelBuilder.Entity<TblAssetTransferAsset>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Tbl_AssetTransfer_Items");

            entity.ToTable("Tbl_AssetTransfer_Assets");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdAssetNavigation).WithMany(p => p.TblAssetTransferAssets)
                .HasForeignKey(d => d.IdAsset)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_AssetTransfer_Items_Asset");

            entity.HasOne(d => d.IdAssetTransferNavigation).WithMany(p => p.TblAssetTransferAssets)
                .HasForeignKey(d => d.IdAssetTransfer)
                .HasConstraintName("FK_AssetTransfer_Assets_AssetTransfer");

            entity.HasOne(d => d.IdFromZoneNavigation).WithMany(p => p.TblAssetTransferAssetIdFromZoneNavigations)
                .HasForeignKey(d => d.IdFromZone)
                .HasConstraintName("FK_AssetTransfer_Assets_FromZone");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAssetTransferAssets)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_AssetTransfer_Items_LegalEntity");

            entity.HasOne(d => d.IdTechnicianNavigation).WithMany(p => p.TblAssetTransferAssets)
                .HasForeignKey(d => d.IdTechnician)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_AssetTransfer_Items_Users");

            entity.HasOne(d => d.IdToZoneNavigation).WithMany(p => p.TblAssetTransferAssetIdToZoneNavigations)
                .HasForeignKey(d => d.IdToZone)
                .HasConstraintName("FK_AssetTransfer_Items_ToZone");
        });

        modelBuilder.Entity<TblAttachment>(entity =>
        {
            entity.ToTable("Tbl_Attachments");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Path)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdAttachmentsTypeNavigation).WithMany(p => p.TblAttachments)
                .HasForeignKey(d => d.IdAttachmentsType)
                .HasConstraintName("FK_Attachments_AttachmentsType");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAttachments)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Attachments_LegalEntity");

            entity.HasOne(d => d.IdWorkOrderNavigation).WithMany(p => p.TblAttachments)
                .HasForeignKey(d => d.IdWorkOrder)
                .HasConstraintName("FK_Attachments_WorkOrder");
        });

        modelBuilder.Entity<TblAttachmentTypeObjectType>(entity =>
        {
            entity.ToTable("Tbl_AttachmentType_ObjectType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAttachmentTypeObjectTypes)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Tbl_AttachmentType_ObjectType_LegalEntity");
        });

        modelBuilder.Entity<TblAttachmentsType>(entity =>
        {
            entity.ToTable("Tbl_AttachmentsType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAttachmentsTypes)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_AttachmentsType_LegalEntity");
        });

        modelBuilder.Entity<TblAttestationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AttestationType");

            entity.ToTable("Tbl_AttestationType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblAttestationTypes)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_AttestationType_LegalEntity");
        });

        modelBuilder.Entity<TblBank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Bank");

            entity.ToTable("Tbl_Bank");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Branch)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IdPdcaccount).HasColumnName("IdPDCAccount");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remark).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdCountryNavigation).WithMany(p => p.TblBanks)
                .HasForeignKey(d => d.IdCountry)
                .HasConstraintName("FK_Bank_Country");

            entity.HasOne(d => d.IdDefaultAccountNavigation).WithMany(p => p.TblBankIdDefaultAccountNavigations)
                .HasForeignKey(d => d.IdDefaultAccount)
                .HasConstraintName("FK_Bank_DefaultAccount");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblBanks)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Bank_LegalEntity");

            entity.HasOne(d => d.IdPdcaccountNavigation).WithMany(p => p.TblBankIdPdcaccountNavigations)
                .HasForeignKey(d => d.IdPdcaccount)
                .HasConstraintName("FK_Bank_PDCAccount");
        });

        modelBuilder.Entity<TblCancellationProcess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CancellationProcess");

            entity.ToTable("Tbl_CancellationProcess");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AdjustmentAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CancellationDate).HasColumnType("datetime");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ContractAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdDocumentStatus).HasDefaultValue(1);
            entity.Property(e => e.IdPeriodBase).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PaidAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.PenaltyPropertyProgress).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.PenaltyTotal).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Reason).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ReleaseDate).HasColumnType("datetime");
            entity.Property(e => e.SecurityDeposit).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdCancellationTypeNavigation).WithMany(p => p.TblCancellationProcesses)
                .HasForeignKey(d => d.IdCancellationType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CancellationProcess_CancellationType");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblCancellationProcesses)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_CancellationProcess_LegalEntity");

            entity.HasOne(d => d.IdSalesContractNavigation).WithMany(p => p.TblCancellationProcesses)
                .HasForeignKey(d => d.IdSalesContract)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CancellationProcess_SalesContract");
        });

        modelBuilder.Entity<TblCancellationProcessChecklist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CancellationProcess_Checklist");

            entity.ToTable("Tbl_CancellationProcess_Checklist");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CheckingDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdCancellationProcessNavigation).WithMany(p => p.TblCancellationProcessChecklists)
                .HasForeignKey(d => d.IdCancellationProcess)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CancellationProcess_Checklist_CancellationProcess");

            entity.HasOne(d => d.IdCancellationTypeChecklistNavigation).WithMany(p => p.TblCancellationProcessChecklists)
                .HasForeignKey(d => d.IdCancellationTypeChecklist)
                .HasConstraintName("FK_CancellationProcess_Checklist_CancellationType_Checklist");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblCancellationProcessChecklists)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_CancellationProcess_Checklist_LegalEntity");
        });

        modelBuilder.Entity<TblCancellationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CancellationType");

            entity.ToTable("Tbl_CancellationType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdContractTypeNavigation).WithMany(p => p.TblCancellationTypes)
                .HasForeignKey(d => d.IdContractType)
                .HasConstraintName("FK_CancellationType_ContractType");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblCancellationTypes)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_CancellationType_LegalEntity");
        });

        modelBuilder.Entity<TblCancellationTypeChecklist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CancellationType_Checklist");

            entity.ToTable("Tbl_CancellationType_Checklist");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remark).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Value).HasColumnType("decimal(18, 6)");

            entity.HasOne(d => d.IdCancellationTypeNavigation).WithMany(p => p.TblCancellationTypeChecklists)
                .HasForeignKey(d => d.IdCancellationType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CancellationType_Checklist_CancellationType");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblCancellationTypeChecklists)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_CancellationType_Checklist_LegalEntity");
        });

        modelBuilder.Entity<TblCertification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Certifications");

            entity.ToTable("Tbl_Certification");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdAssetNavigation).WithMany(p => p.TblCertifications)
                .HasForeignKey(d => d.IdAsset)
                .HasConstraintName("FK_Certifications_Asset");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblCertifications)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Certifications_LegalEntity");

            entity.HasOne(d => d.IdTechncianNavigation).WithMany(p => p.TblCertifications)
                .HasForeignKey(d => d.IdTechncian)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Certifications_User");
        });

        modelBuilder.Entity<TblCity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_City");

            entity.ToTable("Tbl_City");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdCountryNavigation).WithMany(p => p.TblCities)
                .HasForeignKey(d => d.IdCountry)
                .HasConstraintName("FK_City_Country");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblCities)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_City_LegalEntity");

            entity.HasOne(d => d.IdStateNavigation).WithMany(p => p.TblCities)
                .HasForeignKey(d => d.IdState)
                .HasConstraintName("FK_City_State");
        });

        modelBuilder.Entity<TblClass>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Class");

            entity.ToTable("Tbl_Class");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(25)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblClasses)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Class_LegalEntity");
        });

        modelBuilder.Entity<TblCommission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Commission");

            entity.ToTable("Tbl_Commission");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Percentage).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Remark).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.TblCommissions)
                .HasForeignKey(d => d.IdEmployee)
                .HasConstraintName("FK_Commission_Employee");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblCommissions)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Commission_LegalEntity");

            entity.HasOne(d => d.IdSalesContractNavigation).WithMany(p => p.TblCommissions)
                .HasForeignKey(d => d.IdSalesContract)
                .HasConstraintName("FK_Commission_SalesContract");
        });

        modelBuilder.Entity<TblContractType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ContractType");

            entity.ToTable("Tbl_ContractType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IdGlaccount).HasColumnName("IdGLAccount");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblContractTypes)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_ContractType_LegalEntity");
        });

        modelBuilder.Entity<TblCostCenter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CostCenters");

            entity.ToTable("Tbl_CostCenter");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.ValidFrom).HasColumnType("datetime");
            entity.Property(e => e.ValidTo).HasColumnType("datetime");

            entity.HasOne(d => d.IdDimensionNavigation).WithMany(p => p.TblCostCenters)
                .HasForeignKey(d => d.IdDimension)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CostCenters_Dimension");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblCostCenters)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_CostCenters_LegalEntity");
        });

        modelBuilder.Entity<TblCountry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Country");

            entity.ToTable("Tbl_Country");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignNationality)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Nationality)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblCountries)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Country_LegalEntity");
        });

        modelBuilder.Entity<TblCustomer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Customer");

            entity.ToTable("Tbl_Customer");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BlackListDate).HasColumnType("datetime");
            entity.Property(e => e.BlackListReason).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CardCode)
                .HasMaxLength(15)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Contact)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Fax)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignMotherName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IdGlaccount).HasColumnName("IdGLAccount");
            entity.Property(e => e.Idexpiry)
                .HasColumnType("datetime")
                .HasColumnName("IDExpiry");
            entity.Property(e => e.Idnumber)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("IDNumber");
            entity.Property(e => e.LastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LicenseNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MotherName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PassportExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.PassportIssueDate).HasColumnType("datetime");
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Pobox)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("PObox");
            entity.Property(e => e.PrimaryEmail)
                .HasMaxLength(150)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PrimaryPhone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SecondaryEmail)
                .HasMaxLength(150)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SecondaryPhone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.VatNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Vatcertificate)
                .HasMaxLength(200)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("VATCertificate");

            entity.HasOne(d => d.IdContractTypeNavigation).WithMany(p => p.TblCustomers)
                .HasForeignKey(d => d.IdContractType)
                .HasConstraintName("FK_Customer_ContractType");

            entity.HasOne(d => d.IdGlaccountNavigation).WithMany(p => p.TblCustomers)
                .HasForeignKey(d => d.IdGlaccount)
                .HasConstraintName("FK_Customer_GLAccount");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblCustomers)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Customer_LegalEntity");

            entity.HasOne(d => d.IdNationalityNavigation).WithMany(p => p.TblCustomers)
                .HasForeignKey(d => d.IdNationality)
                .HasConstraintName("FK_Customer_Country");
        });

        modelBuilder.Entity<TblCustomerDependent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CustomerDependent");

            entity.ToTable("Tbl_CustomerDependent");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Idnumber)
                .HasMaxLength(150)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("IDNumber");
            entity.Property(e => e.MarriageCertificationNumber)
                .HasMaxLength(150)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(150)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.TblCustomerDependents)
                .HasForeignKey(d => d.IdCustomer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerDependent_Customer");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblCustomerDependents)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_CustomerDependent_LegalEntity");
        });

        modelBuilder.Entity<TblCustomerRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CustomerRequest");

            entity.ToTable("Tbl_CustomerRequest");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MaintenanceEndDate).HasColumnType("datetime");
            entity.Property(e => e.MaintenanceStartDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.TblCustomerRequests)
                .HasForeignKey(d => d.IdCustomer)
                .HasConstraintName("FK_CustomerRequest_Customer");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblCustomerRequests)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_CustomerRequest_LegalEntity");

            entity.HasOne(d => d.IdPropertyNavigation).WithMany(p => p.TblCustomerRequests)
                .HasForeignKey(d => d.IdProperty)
                .HasConstraintName("FK_CustomerRequest_Property");

            entity.HasOne(d => d.IdSalesContractNavigation).WithMany(p => p.TblCustomerRequests)
                .HasForeignKey(d => d.IdSalesContract)
                .HasConstraintName("FK_CustomerRequest_SalesContract");

            entity.HasOne(d => d.IdUnitNavigation).WithMany(p => p.TblCustomerRequests)
                .HasForeignKey(d => d.IdUnit)
                .HasConstraintName("FK_CustomerRequest_Unit");
        });

        modelBuilder.Entity<TblCustomerSponsor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CustomerSponsor");

            entity.ToTable("Tbl_CustomerSponsor");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.TblCustomerSponsors)
                .HasForeignKey(d => d.IdCustomer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerSponsor_Customer");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblCustomerSponsors)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_CustomerSponsor_LegalEntity");

            entity.HasOne(d => d.IdSponsorNavigation).WithMany(p => p.TblCustomerSponsors)
                .HasForeignKey(d => d.IdSponsor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerSponsor_Sponsor");
        });

        modelBuilder.Entity<TblDashboard>(entity =>
        {
            entity.ToTable("Tbl_Dashboard");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DashboardXml)
                .HasColumnType("xml")
                .HasColumnName("DashboardXML");
            entity.Property(e => e.ForeignDashboardXml)
                .HasColumnType("xml")
                .HasColumnName("ForeignDashboardXML");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdDynamicReportNavigation).WithMany(p => p.TblDashboards)
                .HasForeignKey(d => d.IdDynamicReport)
                .HasConstraintName("FK_Dashboard_DynamicReport");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblDashboards)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Dashboard_LegalEntity");
        });

        modelBuilder.Entity<TblDimension>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Dimensions");

            entity.ToTable("Tbl_Dimension");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblDimensions)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Dimensions_LegalEntity");
        });

        modelBuilder.Entity<TblDocumentAttestationType>(entity =>
        {
            entity.HasKey(e => new { e.IdObjectType, e.IdDocument, e.IdAttestationType }).HasName("PK_Document_AttestationType");

            entity.ToTable("Tbl_Document_AttestationType");

            entity.Property(e => e.AttestationDate).HasColumnType("datetime");
            entity.Property(e => e.CheckDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdAttestationTypeNavigation).WithMany(p => p.TblDocumentAttestationTypes)
                .HasForeignKey(d => d.IdAttestationType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Document_AttestationType_AttestationType");

            entity.HasOne(d => d.IdCheckedByNavigation).WithMany(p => p.TblDocumentAttestationTypes)
                .HasForeignKey(d => d.IdCheckedBy)
                .HasConstraintName("FK_Document_AttestationType_CheckedBy");
        });

        modelBuilder.Entity<TblDocumentEmployee>(entity =>
        {
            entity.HasKey(e => new { e.IdObjectType, e.IdDocument, e.IdEmployee });

            entity.ToTable("Tbl_Document_Employee");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.TblDocumentEmployees)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Document_Employee_Employee");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblDocumentEmployees)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Document_Employee_LegalEntity");
        });

        modelBuilder.Entity<TblDocumentParking>(entity =>
        {
            entity.HasKey(e => new { e.IdObjectType, e.IdDocument, e.IdParking });

            entity.ToTable("Tbl_Document_Parking");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblDocumentParkings)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Document_Parking_LegalEntity");

            entity.HasOne(d => d.IdParkingNavigation).WithMany(p => p.TblDocumentParkings)
                .HasForeignKey(d => d.IdParking)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Document_Parking_Parking");
        });

        modelBuilder.Entity<TblDocumentParticular>(entity =>
        {
            entity.HasKey(e => new { e.IdObjectType, e.IdDocument, e.IdParticular });

            entity.ToTable("Tbl_Document_Particular");

            entity.Property(e => e.Amount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.AmountPercent)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.DiscountPercent)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.IsAmountPercent).HasDefaultValue(false);
            entity.Property(e => e.IsDiscountPercent).HasDefaultValue(false);
            entity.Property(e => e.IsTaxPercent).HasDefaultValue(false);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TaxAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TaxPercent)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblDocumentParticulars)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Tbl_Document_Particular_LegalEntity");

            entity.HasOne(d => d.IdParticularNavigation).WithMany(p => p.TblDocumentParticulars)
                .HasForeignKey(d => d.IdParticular)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_Document_Particular_Particular");
        });

        modelBuilder.Entity<TblDynamicDashboard>(entity =>
        {
            entity.ToTable("Tbl_DynamicDashboard");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Category)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignCategory)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblDynamicDashboards)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_DynamicDashboard_LegalEntity");

            entity.HasOne(d => d.IdMenuNavigation).WithMany(p => p.TblDynamicDashboards)
                .HasForeignKey(d => d.IdMenu)
                .HasConstraintName("FK_DynamicDashboard_Menu");
        });

        modelBuilder.Entity<TblDynamicDashboardDetail>(entity =>
        {
            entity.ToTable("Tbl_DynamicDashboardDetails");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdDashboardNavigation).WithMany(p => p.TblDynamicDashboardDetails)
                .HasForeignKey(d => d.IdDashboard)
                .HasConstraintName("FK_DynamicDashboardDetails_Dashboard");

            entity.HasOne(d => d.IdDynamicDashboardNavigation).WithMany(p => p.TblDynamicDashboardDetails)
                .HasForeignKey(d => d.IdDynamicDashboard)
                .HasConstraintName("FK_DynamicDashboardDetails_DynamicDashboard");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblDynamicDashboardDetails)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_DynamicDashboardDetails_LegalEntity");
        });

        modelBuilder.Entity<TblDynamicLayout>(entity =>
        {
            entity.ToTable("Tbl_DynamicLayout");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDescription).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsSystem).HasDefaultValue(false);
            entity.Property(e => e.LayoutPath)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblDynamicLayouts)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Tbl_DynamicLayout_LegalEntity");
        });

        modelBuilder.Entity<TblDynamicReport>(entity =>
        {
            entity.ToTable("Tbl_DynamicReport");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Category)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignCategory)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignReportQuery).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignReportQueryXml)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("ForeignReportQueryXML");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ReportQuery).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ReportQueryXml)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("ReportQueryXML");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblDynamicReports)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_DynamicReport_LegalEntity");

            entity.HasOne(d => d.IdMenuNavigation).WithMany(p => p.TblDynamicReports)
                .HasForeignKey(d => d.IdMenu)
                .HasConstraintName("FK_DynamicReport_Menu");
        });

        modelBuilder.Entity<TblEmail>(entity =>
        {
            entity.ToTable("Tbl_Email");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Body).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Ccemail)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("CCEmail");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RecipientEmail).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ResponseCode)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ResponseMessage).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SendDate).HasColumnType("datetime");
            entity.Property(e => e.SenderEmail).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Subject).UseCollation("SQL_Latin1_General_CP1256_CS_AS");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblEmails)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Email_LegalEntity");
        });

        modelBuilder.Entity<TblEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Employee");

            entity.ToTable("Tbl_Employee");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(25)
                .HasDefaultValue("Emp")
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CommissionPercentage).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IdGlaccount).HasColumnName("IdGLAccount");
            entity.Property(e => e.JoiningDate).HasColumnType("datetime");
            entity.Property(e => e.LastCommissionDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ResignationDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblEmployees)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Employee_LegalEntity");
        });

        modelBuilder.Entity<TblFacility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Facility");

            entity.ToTable("Tbl_Facility");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblFacilities)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Facility_LegalEntity");
        });

        modelBuilder.Entity<TblGlaccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_GLAccount");

            entity.ToTable("Tbl_GLAccount");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblGlaccounts)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_GLAccount_LegalEntity");

            entity.HasOne(d => d.IdParentAccountNavigation).WithMany(p => p.InverseIdParentAccountNavigation)
                .HasForeignKey(d => d.IdParentAccount)
                .HasConstraintName("FK_GLAccount_ParentAccount");
        });

        modelBuilder.Entity<TblGlaccountDetermination>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_GLAccountDetermination");

            entity.ToTable("Tbl_GLAccountDetermination");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdGlaccount).HasColumnName("IdGLAccount");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdGlaccountNavigation).WithMany(p => p.TblGlaccountDeterminations)
                .HasForeignKey(d => d.IdGlaccount)
                .HasConstraintName("FK_GLAccountDetermination_GLAccount");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblGlaccountDeterminations)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_GLAccountDetermination_LegalEntity");
        });

        modelBuilder.Entity<TblGlaccountDeterminationDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_GLAccountDeterminationDetail");

            entity.ToTable("Tbl_GLAccountDeterminationDetail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdGlaccount).HasColumnName("IdGLAccount");
            entity.Property(e => e.IdGlaccountDetermination).HasColumnName("IdGLAccountDetermination");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdCancellationTypeNavigation).WithMany(p => p.TblGlaccountDeterminationDetails)
                .HasForeignKey(d => d.IdCancellationType)
                .HasConstraintName("FK_GLAccountDeterminationDetail_CancellationType");

            entity.HasOne(d => d.IdCancellationTypeChecklistNavigation).WithMany(p => p.TblGlaccountDeterminationDetails)
                .HasForeignKey(d => d.IdCancellationTypeChecklist)
                .HasConstraintName("FK_GLAccountDeterminationDetail_CancellationTypeChecklist");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.TblGlaccountDeterminationDetails)
                .HasForeignKey(d => d.IdCustomer)
                .HasConstraintName("FK_GLAccountDeterminationDetail_Customer");

            entity.HasOne(d => d.IdGlaccountNavigation).WithMany(p => p.TblGlaccountDeterminationDetails)
                .HasForeignKey(d => d.IdGlaccount)
                .HasConstraintName("FK_GLAccountDeterminationDetail_GLAccount");

            entity.HasOne(d => d.IdGlaccountDeterminationNavigation).WithMany(p => p.TblGlaccountDeterminationDetails)
                .HasForeignKey(d => d.IdGlaccountDetermination)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GLAccountDeterminationDetail_GLAccountDetermination");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblGlaccountDeterminationDetails)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_GLAccountDeterminationDetail_LegalEntity");

            entity.HasOne(d => d.IdParticularNavigation).WithMany(p => p.TblGlaccountDeterminationDetails)
                .HasForeignKey(d => d.IdParticular)
                .HasConstraintName("FK_GLAccountDeterminationDetail_Particular");

            entity.HasOne(d => d.IdPropertyNavigation).WithMany(p => p.TblGlaccountDeterminationDetails)
                .HasForeignKey(d => d.IdProperty)
                .HasConstraintName("FK_GLAccountDeterminationDetail_Property");

            entity.HasOne(d => d.IdStateNavigation).WithMany(p => p.TblGlaccountDeterminationDetails)
                .HasForeignKey(d => d.IdState)
                .HasConstraintName("FK_GLAccountDeterminationDetail_State");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TblGlaccountDeterminationDetails)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_GLAccountDeterminationDetail_User");
        });

        modelBuilder.Entity<TblGlaccountDeterminationPriority>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_GLAccountDeterminationPriority");

            entity.ToTable("Tbl_GLAccountDeterminationPriority");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblGlaccountDeterminationPriorities)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_GLAccountDeterminationPriority_LegalEntity");
        });

        modelBuilder.Entity<TblGoodsIssue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_GoodsIssue");

            entity.ToTable("Tbl_GoodsIssue");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IntegrationStatus).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IssuedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblGoodsIssues)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_GoodsIssue_LegalEntity");

            entity.HasOne(d => d.IdTechncianNavigation).WithMany(p => p.TblGoodsIssues)
                .HasForeignKey(d => d.IdTechncian)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GoodsIssue_User");

            entity.HasOne(d => d.IdWorkOrderNavigation).WithMany(p => p.TblGoodsIssues)
                .HasForeignKey(d => d.IdWorkOrder)
                .HasConstraintName("FK_GoodsIssue_WorkOrder");
        });

        modelBuilder.Entity<TblGoodsIssueItem>(entity =>
        {
            entity.ToTable("Tbl_GoodsIssue_Items");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.IssueDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdAssetNavigation).WithMany(p => p.TblGoodsIssueItems)
                .HasForeignKey(d => d.IdAsset)
                .HasConstraintName("FK_GoodsIssue_Items_Asset");

            entity.HasOne(d => d.IdGoodsIssueNavigation).WithMany(p => p.TblGoodsIssueItems)
                .HasForeignKey(d => d.IdGoodsIssue)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GoodsIssue_Items");

            entity.HasOne(d => d.IdItemsNavigation).WithMany(p => p.TblGoodsIssueItems)
                .HasForeignKey(d => d.IdItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GoodsIssue_Items_Items");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblGoodsIssueItems)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_GoodsIssue_Items_LegalEntity");

            entity.HasOne(d => d.IdMaintenanceTypeNavigation).WithMany(p => p.TblGoodsIssueItems)
                .HasForeignKey(d => d.IdMaintenanceType)
                .HasConstraintName("FK_GoodsIssue_Items_MaintenanceType");

            entity.HasOne(d => d.IdTechnicianNavigation).WithMany(p => p.TblGoodsIssueItems)
                .HasForeignKey(d => d.IdTechnician)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GoodsIssue_Items_Users");

            entity.HasOne(d => d.IdWarehouseNavigation).WithMany(p => p.TblGoodsIssueItems)
                .HasForeignKey(d => d.IdWarehouse)
                .HasConstraintName("FK_GoodsIssue_Items_Warehouse");
        });

        modelBuilder.Entity<TblGoodsIssueItemSerialNumber>(entity =>
        {
            entity.ToTable("Tbl_GoodsIssue_ItemSerialNumber");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 6)");

            entity.HasOne(d => d.IdGoodsIssueNavigation).WithMany(p => p.TblGoodsIssueItemSerialNumbers)
                .HasForeignKey(d => d.IdGoodsIssue)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_GoodsIssue_ItemSerialNumber_GoodsIssue");

            entity.HasOne(d => d.IdGoodsIssueItemNavigation).WithMany(p => p.TblGoodsIssueItemSerialNumbers)
                .HasForeignKey(d => d.IdGoodsIssueItem)
                .HasConstraintName("FK_GoodsIssue_ItemSerialNumber_GoodsIssueItem");

            entity.HasOne(d => d.IdItemSerialNumberNavigation).WithMany(p => p.TblGoodsIssueItemSerialNumbers)
                .HasForeignKey(d => d.IdItemSerialNumber)
                .HasConstraintName("FK_GoodsIssue_ItemSerialNumber_ItemSerialNumber");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblGoodsIssueItemSerialNumbers)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_GoodsIssue_ItemSerialNumber_LegalEntity");
        });

        modelBuilder.Entity<TblHseatmosphericMonitoring>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HSEAtmosphericMonitoring");

            entity.ToTable("Tbl_HSEAtmosphericMonitoring");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IdHsetype).HasColumnName("IdHSEType");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remark)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblHseatmosphericMonitorings)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_HSEAtmosphericMonitoring_LegalEntity");
        });

        modelBuilder.Entity<TblHsecondition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HSEConditions");

            entity.ToTable("Tbl_HSEConditions");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IdHsetype).HasColumnName("IdHSEType");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remark)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblHseconditions)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_HSEConditions_LegalEntity");
        });

        modelBuilder.Entity<TblHseexcavationWork>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HSEExcavationWork");

            entity.ToTable("Tbl_HSEExcavationWork");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remark)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblHseexcavationWorks)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_HSEExcavationWork_LegalEntity");
        });

        modelBuilder.Entity<TblHseprocedure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HSEProcedure");

            entity.ToTable("Tbl_HSEProcedure");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Area)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.IdHsetype).HasColumnName("IdHSEType");
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblHseprocedures)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_HSEProcedure_LegalEntity");
        });

        modelBuilder.Entity<TblHsesafetyEquipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HSE_SafetyEquipment");

            entity.ToTable("Tbl_HSESafetyEquipment");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IdHsetype).HasColumnName("IdHSEType");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remark)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblHsesafetyEquipments)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_HSESafetyEquipment_LegalEntity");
        });

        modelBuilder.Entity<TblHseworkDescription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HSEWorkDescription");

            entity.ToTable("Tbl_HSEWorkDescription");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IdHsetype).HasColumnName("IdHSEType");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remark)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblHseworkDescriptions)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_HSEWorkDescription_LegalEntity");
        });

        modelBuilder.Entity<TblIncomingPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_IncomingPayment");

            entity.ToTable("Tbl_IncomingPayment");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .HasDefaultValue("")
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.IsPosted).HasDefaultValue(false);
            entity.Property(e => e.PostingDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 6)");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblIncomingPayments)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_IncomingPayment_LegalEntity");

            entity.HasOne(d => d.IdSalesContractNavigation).WithMany(p => p.TblIncomingPayments)
                .HasForeignKey(d => d.IdSalesContract)
                .HasConstraintName("FK_IncomingPayment_SalesContract");

            entity.HasOne(d => d.IdSalesOrderNavigation).WithMany(p => p.TblIncomingPayments)
                .HasForeignKey(d => d.IdSalesOrder)
                .HasConstraintName("FK_IncomingPayment_SalesOrder");
        });

        modelBuilder.Entity<TblIncomingPaymentAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_IncomingPaymentAccount");

            entity.ToTable("Tbl_IncomingPaymentAccount");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdGlaccount).HasColumnName("IdGLAccount");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdGlaccountNavigation).WithMany(p => p.TblIncomingPaymentAccounts)
                .HasForeignKey(d => d.IdGlaccount)
                .HasConstraintName("FK_IncomingPaymentAccount_GLAccount");

            entity.HasOne(d => d.IdIncomingPaymentNavigation).WithMany(p => p.TblIncomingPaymentAccounts)
                .HasForeignKey(d => d.IdIncomingPayment)
                .HasConstraintName("FK_IncomingPaymentAccount_IncomingPayment");
        });

        modelBuilder.Entity<TblIncomingPaymentCash>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_IncomingPaymentCash");

            entity.ToTable("Tbl_IncomingPaymentCash");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.BillNumber)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdGlaccount).HasColumnName("IdGLAccount");
            entity.Property(e => e.ReceiptDate).HasColumnType("datetime");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdGlaccountNavigation).WithMany(p => p.TblIncomingPaymentCashes)
                .HasForeignKey(d => d.IdGlaccount)
                .HasConstraintName("FK_IncomingPaymentCash_GLAccount");

            entity.HasOne(d => d.IdIncomingPaymentNavigation).WithMany(p => p.TblIncomingPaymentCashes)
                .HasForeignKey(d => d.IdIncomingPayment)
                .HasConstraintName("FK_IncomingPaymentCash_IncomingPayment");
        });

        modelBuilder.Entity<TblIncomingPaymentCheque>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_IncomingPaymentCheque");

            entity.ToTable("Tbl_IncomingPaymentCheque");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ChequeNumber)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.HolderName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IdGlaccount).HasColumnName("IdGLAccount");
            entity.Property(e => e.Remark).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusFee).HasColumnType("decimal(19, 6)");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdBankNavigation).WithMany(p => p.TblIncomingPaymentCheques)
                .HasForeignKey(d => d.IdBank)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IncomingPaymentCheque_Bank");

            entity.HasOne(d => d.IdGlaccountNavigation).WithMany(p => p.TblIncomingPaymentCheques)
                .HasForeignKey(d => d.IdGlaccount)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IncomingPaymentCheque_GLAccount");

            entity.HasOne(d => d.IdIncomingPaymentNavigation).WithMany(p => p.TblIncomingPaymentCheques)
                .HasForeignKey(d => d.IdIncomingPayment)
                .HasConstraintName("FK_IncomingPaymentCheque_IncomingPayment");
        });

        modelBuilder.Entity<TblIncomingPaymentInstallment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_IncomingPayment_Installment");

            entity.ToTable("Tbl_IncomingPayment_Installment");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdPaymentStatus).HasDefaultValue(1);
            entity.Property(e => e.IdUpdatedBy).HasDefaultValue(0);
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdIncomingAccountNavigation).WithMany(p => p.TblIncomingPaymentInstallments)
                .HasForeignKey(d => d.IdIncomingAccount)
                .HasConstraintName("FK_IncomingPayment_Installment_Account");

            entity.HasOne(d => d.IdIncomingCashNavigation).WithMany(p => p.TblIncomingPaymentInstallments)
                .HasForeignKey(d => d.IdIncomingCash)
                .HasConstraintName("FK_IncomingPayment_Installment_Cash");

            entity.HasOne(d => d.IdIncomingChequeNavigation).WithMany(p => p.TblIncomingPaymentInstallments)
                .HasForeignKey(d => d.IdIncomingCheque)
                .HasConstraintName("FK_IncomingPayment_Installment_Cheque");

            entity.HasOne(d => d.IdIncomingPaymentNavigation).WithMany(p => p.TblIncomingPaymentInstallments)
                .HasForeignKey(d => d.IdIncomingPayment)
                .HasConstraintName("FK_IncomingPayment_Installment_IncomingPayment");

            entity.HasOne(d => d.IdIncomingTransferNavigation).WithMany(p => p.TblIncomingPaymentInstallments)
                .HasForeignKey(d => d.IdIncomingTransfer)
                .HasConstraintName("FK_IncomingPayment_Installment_Transfer");

            entity.HasOne(d => d.IdSalesInvoiceInstallmentNavigation).WithMany(p => p.TblIncomingPaymentInstallments)
                .HasForeignKey(d => d.IdSalesInvoiceInstallment)
                .HasConstraintName("FK_IncomingPayment_Installment_InvoiceInstallment");
        });

        modelBuilder.Entity<TblIncomingPaymentTransfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_IncomingPaymentTransfer");

            entity.ToTable("Tbl_IncomingPaymentTransfer");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerReferenceNumber)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IdGlaccount).HasColumnName("IdGLAccount");
            entity.Property(e => e.ReceivedFrom)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.TransferDate).HasColumnType("datetime");
            entity.Property(e => e.TransferNumber)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdGlaccountNavigation).WithMany(p => p.TblIncomingPaymentTransfers)
                .HasForeignKey(d => d.IdGlaccount)
                .HasConstraintName("FK_IncomingPaymentTransfer_GLAccount");

            entity.HasOne(d => d.IdIncomingPaymentNavigation).WithMany(p => p.TblIncomingPaymentTransfers)
                .HasForeignKey(d => d.IdIncomingPayment)
                .HasConstraintName("FK_IncomingPaymentTransfer_IncomingPayment");
        });

        modelBuilder.Entity<TblInventoryUnitMeasure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_InventoryUnitMeasure");

            entity.ToTable("Tbl_InventoryUnitMeasure");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblInventoryUnitMeasures)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_InventoryUnitMeasure_LegalEntity");
        });

        modelBuilder.Entity<TblItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Items");

            entity.ToTable("Tbl_Items");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.QuantityInStock).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Tax)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdInventoryUnitMeasureNavigation).WithMany(p => p.TblItems)
                .HasForeignKey(d => d.IdInventoryUnitMeasure)
                .HasConstraintName("FK_Items_InventoryUnitMeasure");

            entity.HasOne(d => d.IdItemGroupNavigation).WithMany(p => p.TblItems)
                .HasForeignKey(d => d.IdItemGroup)
                .HasConstraintName("FK_Items_ItemGroup");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblItems)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Items_LegalEntity");
        });

        modelBuilder.Entity<TblItemGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ItemGroup");

            entity.ToTable("Tbl_ItemGroup");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblItemGroups)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_ItemGroup_LegalEntity");
        });

        modelBuilder.Entity<TblItemPurchaseRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ItemPurchaseRequest");

            entity.ToTable("Tbl_ItemPurchaseRequest");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DocumentDate).HasColumnType("datetime");
            entity.Property(e => e.IdSap).HasColumnName("IdSAP");
            entity.Property(e => e.IntegrationStatus)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RequiredDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.ValidUntilDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblItemPurchaseRequests)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_ItemPurchaseRequest_LegalEntity");
        });

        modelBuilder.Entity<TblItemPurchaseRequestItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ItemPurchaseRequest_Items");

            entity.ToTable("Tbl_ItemPurchaseRequest_Items");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ItemTax)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdInventoryUnitOfMeasureNavigation).WithMany(p => p.TblItemPurchaseRequestItems)
                .HasForeignKey(d => d.IdInventoryUnitOfMeasure)
                .HasConstraintName("FK_ItemPurchaseRequest_Items_InventoryUnitMeasure");

            entity.HasOne(d => d.IdItemPurchaseRequestNavigation).WithMany(p => p.TblItemPurchaseRequestItems)
                .HasForeignKey(d => d.IdItemPurchaseRequest)
                .HasConstraintName("FK_ItemPurchaseRequest_Items_ItemPurchaseRequest");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblItemPurchaseRequestItems)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_ItemPurchaseRequest_Items_LegalEntity");

            entity.HasOne(d => d.IdSupplierNavigation).WithMany(p => p.TblItemPurchaseRequestItems)
                .HasForeignKey(d => d.IdSupplier)
                .HasConstraintName("FK_ItemPurchaseRequest_Items_Supplier");
        });

        modelBuilder.Entity<TblItemSerialNumber>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Item_SerialNumber");

            entity.ToTable("Tbl_Item_SerialNumber");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdBinLocationNavigation).WithMany(p => p.TblItemSerialNumbers)
                .HasForeignKey(d => d.IdBinLocation)
                .HasConstraintName("FK_Item_SerialNumber_BinLocation");

            entity.HasOne(d => d.IdItemNavigation).WithMany(p => p.TblItemSerialNumbers)
                .HasForeignKey(d => d.IdItem)
                .HasConstraintName("FK_Item_SerialNumber_Item");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblItemSerialNumbers)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Item_SerialNumber_LegalEntity");

            entity.HasOne(d => d.IdWarehouseNavigation).WithMany(p => p.TblItemSerialNumbers)
                .HasForeignKey(d => d.IdWarehouse)
                .HasConstraintName("FK_Item_SerialNumber_Warehouse");
        });

        modelBuilder.Entity<TblJobTitle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_JobTitle");

            entity.ToTable("Tbl_JobTitle");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(25)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CommissionPercentage).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblJobTitles)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_JobTitle_LegalEntity");
        });

        modelBuilder.Entity<TblJournalEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_JournalEntry");

            entity.ToTable("Tbl_JournalEntry");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.TblJournalEntries)
                .HasForeignKey(d => d.IdCustomer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JournalEntry_Customer");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblJournalEntries)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_JournalEntry_LegalEntity");

            entity.HasOne(d => d.IdUnitNavigation).WithMany(p => p.TblJournalEntries)
                .HasForeignKey(d => d.IdUnit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JournalEntry_Unit");
        });

        modelBuilder.Entity<TblJournalEntryDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_JournalEntryDetails");

            entity.ToTable("Tbl_JournalEntryDetails");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Credit).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Debit).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.IdGlaccount).HasColumnName("IdGLAccount");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdGlaccountNavigation).WithMany(p => p.TblJournalEntryDetails)
                .HasForeignKey(d => d.IdGlaccount)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JournalEntryDetails_GLAccount");

            entity.HasOne(d => d.IdJournalEntryNavigation).WithMany(p => p.TblJournalEntryDetails)
                .HasForeignKey(d => d.IdJournalEntry)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JournalEntryDetails_JournalEntry");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblJournalEntryDetails)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_JournalEntryDetails_LegalEntity");
        });

        modelBuilder.Entity<TblKnowU>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_KnowUs");

            entity.ToTable("Tbl_KnowUs");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblKnowUs)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_KnowUs_LegalEntity");
        });

        modelBuilder.Entity<TblLegalEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Project");

            entity.ToTable("Tbl_LegalEntity");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AllowMultiBranches).HasDefaultValue(false);
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DatabaseName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DatabasePassword)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DatabaseServer)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DatabaseUsername)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LiscenceServer)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Sapserver)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("SAPServer");
            entity.Property(e => e.SapuserPassword)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("SAPUserPassword");
            entity.Property(e => e.SapuserUsername)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("SAPUserUsername");
            entity.Property(e => e.Sldserver)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("SLDServer");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblLegalEntityUser>(entity =>
        {
            entity.HasKey(e => new { e.IdLegalEntity, e.IdUser });

            entity.ToTable("Tbl_LegalEntity_User");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblLegalEntityUsers)
                .HasForeignKey(d => d.IdLegalEntity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LegalEntity_User_LegalEntity");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TblLegalEntityUsers)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LegalEntity_User_User");
        });

        modelBuilder.Entity<TblLegend>(entity =>
        {
            entity.ToTable("Tbl_Legend");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblLegends)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Legend_LegalEntity");
        });

        modelBuilder.Entity<TblLookup>(entity =>
        {
            entity.HasKey(e => new { e.IdLookupType, e.LookupKey });

            entity.ToTable("Tbl_Lookup");

            entity.Property(e => e.ForeignName).HasMaxLength(510);
            entity.Property(e => e.Name).HasMaxLength(510);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblLookups)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Lookup_LegalEntity");

            entity.HasOne(d => d.IdLookupTypeNavigation).WithMany(p => p.TblLookups)
                .HasForeignKey(d => d.IdLookupType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lookup_LookupType");
        });

        modelBuilder.Entity<TblLookupType>(entity =>
        {
            entity.ToTable("Tbl_LookupType");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.ForeignName).HasMaxLength(1020);
            entity.Property(e => e.Name).HasMaxLength(1020);

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblLookupTypes)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_LookupType_LegalEntity");
        });

        modelBuilder.Entity<TblMaintenanceType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Maintenance Type");

            entity.ToTable("Tbl_MaintenanceType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDescription)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Hours).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblMaintenanceTypes)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_MaintenanceType_LegalEntity");

            entity.HasOne(d => d.IdWorkOrderCategoryNavigation).WithMany(p => p.TblMaintenanceTypes)
                .HasForeignKey(d => d.IdWorkOrderCategory)
                .HasConstraintName("FK_MaintenanceType_WorkOrderCategory");

            entity.HasOne(d => d.IdWorkTypeNavigation).WithMany(p => p.TblMaintenanceTypes)
                .HasForeignKey(d => d.IdWorkType)
                .HasConstraintName("FK_MaintenanceType_WorkType");
        });

        modelBuilder.Entity<TblMaintenanceTypeImport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MaintenanceTYPEIMPORT");

            entity.ToTable("Tbl_MaintenanceType_import");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDescription)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Hours).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblMaintenanceTypeItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MaintenanceType_Item");

            entity.ToTable("Tbl_MaintenanceType_Item");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Item)
                .HasMaxLength(200)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceType)
                .HasMaxLength(200)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.IdItemNavigation).WithMany(p => p.TblMaintenanceTypeItems)
                .HasForeignKey(d => d.IdItem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_MaintenanceType_Item");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblMaintenanceTypeItems)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_MaintenanceType_Item_LegalEntity");

            entity.HasOne(d => d.IdMaintenanceTypeNavigation).WithMany(p => p.TblMaintenanceTypeItems)
                .HasForeignKey(d => d.IdMaintenanceType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_MaintenanceType_Item_MaintenanceType");
        });

        modelBuilder.Entity<TblMaintenanceTypeTechncian>(entity =>
        {
            entity.HasKey(e => new { e.IdMaintenanceType, e.IdTechncian });

            entity.ToTable("Tbl_MaintenanceType_Techncian");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Hours).HasColumnType("decimal(6, 2)");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblMaintenanceTypeTechncians)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_MaintenanceType_Techncian_LegalEntity");

            entity.HasOne(d => d.IdMaintenanceTypeNavigation).WithMany(p => p.TblMaintenanceTypeTechncians)
                .HasForeignKey(d => d.IdMaintenanceType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_MaintenanceType_Techncian_MaintenanceType");

            entity.HasOne(d => d.IdTechncianNavigation).WithMany(p => p.TblMaintenanceTypeTechncians)
                .HasForeignKey(d => d.IdTechncian)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_MaintenanceType_User");
        });

        modelBuilder.Entity<TblMaintenanceTypeTool>(entity =>
        {
            entity.HasKey(e => new { e.IdMaintenanceType, e.IdTool });

            entity.ToTable("Tbl_MaintenanceType_Tool");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblMaintenanceTypeTools)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_MaintenanceType_Tool_LegalEntity");

            entity.HasOne(d => d.IdMaintenanceTypeNavigation).WithMany(p => p.TblMaintenanceTypeTools)
                .HasForeignKey(d => d.IdMaintenanceType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_MaintenanceType_Tool_MaintenanceType");

            entity.HasOne(d => d.IdToolNavigation).WithMany(p => p.TblMaintenanceTypeTools)
                .HasForeignKey(d => d.IdTool)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_MaintenanceType_Tool");
        });

        modelBuilder.Entity<TblManufacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Manu__3214EC2750ECADE2");

            entity.ToTable("Tbl_Manufacturer");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblManufacturers)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Manufacturer_LegalEntity");
        });

        modelBuilder.Entity<TblMaterialRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MaterialRequest");

            entity.ToTable("Tbl_MaterialRequest");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblMaterialRequests)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_MaterialRequest_LegalEntity");

            entity.HasOne(d => d.IdTechncianNavigation).WithMany(p => p.TblMaterialRequests)
                .HasForeignKey(d => d.IdTechncian)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_MaterialRequest_User");

            entity.HasOne(d => d.IdWorkOrderNavigation).WithMany(p => p.TblMaterialRequests)
                .HasForeignKey(d => d.IdWorkOrder)
                .HasConstraintName("FK_Tbl_MaterialRequest_WorkOrder");
        });

        modelBuilder.Entity<TblMaterialRequestItem>(entity =>
        {
            entity.ToTable("Tbl_MaterialRequest_Items");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IssueDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdAssetNavigation).WithMany(p => p.TblMaterialRequestItems)
                .HasForeignKey(d => d.IdAsset)
                .HasConstraintName("FK_Tbl_MaterialRequest_Items_Asset");

            entity.HasOne(d => d.IdFromWarehouseNavigation).WithMany(p => p.TblMaterialRequestItemIdFromWarehouseNavigations)
                .HasForeignKey(d => d.IdFromWarehouse)
                .HasConstraintName("FK_MaterialRequest_Items_FromWarehouse");

            entity.HasOne(d => d.IdItemsNavigation).WithMany(p => p.TblMaterialRequestItems)
                .HasForeignKey(d => d.IdItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_MaterialRequest_Items_Items");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblMaterialRequestItems)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_MaterialRequest_Items_LegalEntity");

            entity.HasOne(d => d.IdMaintenanceTypeNavigation).WithMany(p => p.TblMaterialRequestItems)
                .HasForeignKey(d => d.IdMaintenanceType)
                .HasConstraintName("FK_Tbl_MaterialRequest_Items_MaintenanceType");

            entity.HasOne(d => d.IdMaterialRequestNavigation).WithMany(p => p.TblMaterialRequestItems)
                .HasForeignKey(d => d.IdMaterialRequest)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_MaterialRequest_Items");

            entity.HasOne(d => d.IdTechnicianNavigation).WithMany(p => p.TblMaterialRequestItems)
                .HasForeignKey(d => d.IdTechnician)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_MaterialRequest_Items_Users");

            entity.HasOne(d => d.IdToWarehouseNavigation).WithMany(p => p.TblMaterialRequestItemIdToWarehouseNavigations)
                .HasForeignKey(d => d.IdToWarehouse)
                .HasConstraintName("FK_MaterialRequest_Items_ToWarehouse");
        });

        modelBuilder.Entity<TblMaterialTransfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MaterialTransfer");

            entity.ToTable("Tbl_MaterialTransfer");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdSap).HasColumnName("IdSAP");
            entity.Property(e => e.IntegrationStatus).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IssuedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblMaterialTransfers)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_MaterialTransfer_LegalEntity");

            entity.HasOne(d => d.IdTechncianNavigation).WithMany(p => p.TblMaterialTransfers)
                .HasForeignKey(d => d.IdTechncian)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_MaterialTransfer_User");

            entity.HasOne(d => d.IdWorkOrderNavigation).WithMany(p => p.TblMaterialTransfers)
                .HasForeignKey(d => d.IdWorkOrder)
                .HasConstraintName("FK_Tbl_MaterialTransfer_WorkOrder");
        });

        modelBuilder.Entity<TblMaterialTransferItem>(entity =>
        {
            entity.ToTable("Tbl_MaterialTransfer_Items");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IssueDate).HasColumnType("datetime");
            entity.Property(e => e.StatusDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdAssetNavigation).WithMany(p => p.TblMaterialTransferItems)
                .HasForeignKey(d => d.IdAsset)
                .HasConstraintName("FK_Tbl_MaterialTransfer_Items_Asset");

            entity.HasOne(d => d.IdFromWarehouseNavigation).WithMany(p => p.TblMaterialTransferItemIdFromWarehouseNavigations)
                .HasForeignKey(d => d.IdFromWarehouse)
                .HasConstraintName("FK_MaterialTransfer_Items_FromWarehouse");

            entity.HasOne(d => d.IdItemsNavigation).WithMany(p => p.TblMaterialTransferItems)
                .HasForeignKey(d => d.IdItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_MaterialTransfer_Items_Items");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblMaterialTransferItems)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_MaterialTransfer_Items_LegalEntity");

            entity.HasOne(d => d.IdMaintenanceTypeNavigation).WithMany(p => p.TblMaterialTransferItems)
                .HasForeignKey(d => d.IdMaintenanceType)
                .HasConstraintName("FK_Tbl_MaterialTransfer_Items_MaintenanceType");

            entity.HasOne(d => d.IdMaterialRequestItemNavigation).WithMany(p => p.TblMaterialTransferItems)
                .HasForeignKey(d => d.IdMaterialRequestItem)
                .HasConstraintName("FK_MaterialTransfer_Items_MaterialRequestItems");

            entity.HasOne(d => d.IdMaterialTransferNavigation).WithMany(p => p.TblMaterialTransferItems)
                .HasForeignKey(d => d.IdMaterialTransfer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_MaterialTransfer_Items");

            entity.HasOne(d => d.IdTechnicianNavigation).WithMany(p => p.TblMaterialTransferItems)
                .HasForeignKey(d => d.IdTechnician)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_MaterialTransfer_Items_Users");

            entity.HasOne(d => d.IdToWarehouseNavigation).WithMany(p => p.TblMaterialTransferItemIdToWarehouseNavigations)
                .HasForeignKey(d => d.IdToWarehouse)
                .HasConstraintName("FK_MaterialTransfer_Items_ToWarehouse");

            entity.HasOne(d => d.IdWorkOrderNavigation).WithMany(p => p.TblMaterialTransferItems)
                .HasForeignKey(d => d.IdWorkOrder)
                .HasConstraintName("FK_MaterialTransfer_WorkOrder");
        });

        modelBuilder.Entity<TblMaterialTransferItemSerialNumber>(entity =>
        {
            entity.ToTable("Tbl_MaterialTransfer_ItemSerialNumber");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.IdItemSerialNumberNavigation).WithMany(p => p.TblMaterialTransferItemSerialNumbers)
                .HasForeignKey(d => d.IdItemSerialNumber)
                .HasConstraintName("FK_MaterialTransfer_ItemSerialNumber_ItemSerialNumber");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblMaterialTransferItemSerialNumbers)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_MaterialTransfer_ItemSerialNumber_LegalEntity");

            entity.HasOne(d => d.IdMaterialTransferNavigation).WithMany(p => p.TblMaterialTransferItemSerialNumbers)
                .HasForeignKey(d => d.IdMaterialTransfer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_MaterialTransfer_ItemSerialNumber_MaterialTransfer");

            entity.HasOne(d => d.IdMaterialTransferItemNavigation).WithMany(p => p.TblMaterialTransferItemSerialNumbers)
                .HasForeignKey(d => d.IdMaterialTransferItem)
                .HasConstraintName("FK_MaterialTransfer_ItemSerialNumber_MaterialTransferItem");
        });

        modelBuilder.Entity<TblMenu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Menu");

            entity.ToTable("Tbl_Menu");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Cssclass)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("CSSClass");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDescription)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("URL");
            entity.Property(e => e.UserPermissionName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblMenus)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Menu_LegalEntity");

            entity.HasOne(d => d.IdParentNavigation).WithMany(p => p.InverseIdParentNavigation)
                .HasForeignKey(d => d.IdParent)
                .HasConstraintName("FK_Menu_Parent");
        });

        modelBuilder.Entity<TblMeterReading>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MeterReading");

            entity.ToTable("Tbl_MeterReading");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DocumentNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PostingDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblMeterReadings)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_MeterReading_LegalEntity");
        });

        modelBuilder.Entity<TblMeterReadingDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MeterReadinDetailg");

            entity.ToTable("Tbl_MeterReadingDetail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReadingValue).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Remark)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdAssetNavigation).WithMany(p => p.TblMeterReadingDetails)
                .HasForeignKey(d => d.IdAsset)
                .HasConstraintName("FK_MeterReadingDetail_Asset");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblMeterReadingDetails)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_MeterReadingDetail_LegalEntity");

            entity.HasOne(d => d.IdMaintenanceTypeNavigation).WithMany(p => p.TblMeterReadingDetails)
                .HasForeignKey(d => d.IdMaintenanceType)
                .HasConstraintName("FK_MeterReadingDetail_MaintenanceType");

            entity.HasOne(d => d.IdMeterReadingNavigation).WithMany(p => p.TblMeterReadingDetails)
                .HasForeignKey(d => d.IdMeterReading)
                .HasConstraintName("FK_MeterReadingDetail_MeterReading");

            entity.HasOne(d => d.IdMeterTypeNavigation).WithMany(p => p.TblMeterReadingDetails)
                .HasForeignKey(d => d.IdMeterType)
                .HasConstraintName("FK_MeterReadingDetail_MeterType");
        });

        modelBuilder.Entity<TblMeterType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MeterType");

            entity.ToTable("Tbl_MeterType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblMeterTypes)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_MeterType_LegalEntity");
        });

        modelBuilder.Entity<TblMobileMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MobileMessage");

            entity.ToTable("Tbl_MobileMessage");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Body).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EffectiveDate).HasColumnType("datetime");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Title).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblMobileMessages)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_MobileMessage_LegalEntity");
        });

        modelBuilder.Entity<TblMobileMessageMobileUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MobileMessage_MobileUser");

            entity.ToTable("Tbl_MobileMessage_MobileUser");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblMobileMessageMobileUsers)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_MobileMessage_MobileUser_LegalEntity");

            entity.HasOne(d => d.IdMobileMessageNavigation).WithMany(p => p.TblMobileMessageMobileUsers)
                .HasForeignKey(d => d.IdMobileMessage)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MobileMessage_MobileUser_MobileMessage");

            entity.HasOne(d => d.IdMobileUserNavigation).WithMany(p => p.TblMobileMessageMobileUsers)
                .HasForeignKey(d => d.IdMobileUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MobileMessage_MobileUser_MobileUser");
        });

        modelBuilder.Entity<TblMobileNews>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MobileNews");

            entity.ToTable("Tbl_MobileNews");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Body).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EffectiveDate).HasColumnType("datetime");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Title).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblMobileNews)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_MobileNews_LegalEntity");
        });

        modelBuilder.Entity<TblMobileUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MobileUser");

            entity.ToTable("Tbl_MobileUser");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IdApprovalStatus).HasDefaultValue(1);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Password)
                .HasMaxLength(800)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblMobileUsers)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_MobileUser_LegalEntity");
        });

        modelBuilder.Entity<TblNeighborhood>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Neighborhoods");

            entity.ToTable("Tbl_Neighborhood");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblNeighborhoods)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Neighborhoods_LegalEntity");
        });

        modelBuilder.Entity<TblObjectReference>(entity =>
        {
            entity.ToTable("Tbl_ObjectReference");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.ZeroNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblObjectReferences)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Tbl_ObjectReference_LegalEntity");
        });

        modelBuilder.Entity<TblParameter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Parameter");

            entity.ToTable("Tbl_Parameter");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(225)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(225)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Value)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblParameters)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Parameter_LegalEntity");
        });

        modelBuilder.Entity<TblParking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Parking");

            entity.ToTable("Tbl_Parking");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Floor)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LevelNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SpaceNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblParkings)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Parking_LegalEntity");

            entity.HasOne(d => d.IdPropertyNavigation).WithMany(p => p.TblParkings)
                .HasForeignKey(d => d.IdProperty)
                .HasConstraintName("FK_Parking_Property");
        });

        modelBuilder.Entity<TblParticular>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Particular");

            entity.ToTable("Tbl_Particular");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RefundableAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Tax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblParticulars)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Particular_LegalEntity");
        });

        modelBuilder.Entity<TblPaymentTerm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PaymentTerm");

            entity.ToTable("Tbl_PaymentTerm");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActiveFromDate).HasColumnType("datetime");
            entity.Property(e => e.ActiveToDate).HasColumnType("datetime");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblPaymentTerms)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_PaymentTerm_LegalEntity");
        });

        modelBuilder.Entity<TblPaymentTermInstallment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Installment");

            entity.ToTable("Tbl_PaymentTermInstallment");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Percentage).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblPaymentTermInstallments)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Installment_LegalEntity");

            entity.HasOne(d => d.IdPaymentTermNavigation).WithMany(p => p.TblPaymentTermInstallments)
                .HasForeignKey(d => d.IdPaymentTerm)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Installment_PaymentTerm");
        });

        modelBuilder.Entity<TblPortalResource>(entity =>
        {
            entity.ToTable("Tbl_PortalResource");

            entity.HasIndex(e => e.Name, "Tbl_PortalResource_Name_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Comment).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignComment).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignValue).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Value).UseCollation("SQL_Latin1_General_CP1256_CS_AS");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblPortalResources)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_PortalResource_LegalEntity");
        });

        modelBuilder.Entity<TblPosition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Position");

            entity.ToTable("Tbl_Position");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblPositions)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Position_LegalEntity");
        });

        modelBuilder.Entity<TblProperty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Property");

            entity.ToTable("Tbl_Property");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Number)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SquareFeet)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdGroupNavigation).WithMany(p => p.TblPropertyIdGroupNavigations)
                .HasForeignKey(d => d.IdGroup)
                .HasConstraintName("FK_Property_Group");

            entity.HasOne(d => d.IdLandLordNavigation).WithMany(p => p.TblProperties)
                .HasForeignKey(d => d.IdLandLord)
                .HasConstraintName("FK_Property_LandLord");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblProperties)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Property_LegalEntity");

            entity.HasOne(d => d.IdMainPropertyNavigation).WithMany(p => p.InverseIdMainPropertyNavigation)
                .HasForeignKey(d => d.IdMainProperty)
                .HasConstraintName("FK_Property_MainProperty");

            entity.HasOne(d => d.IdPositionNavigation).WithMany(p => p.TblProperties)
                .HasForeignKey(d => d.IdPosition)
                .HasConstraintName("FK_Property_Position");

            entity.HasOne(d => d.IdPropertyClassNavigation).WithMany(p => p.TblProperties)
                .HasForeignKey(d => d.IdPropertyClass)
                .HasConstraintName("FK_Property_PropertyClass");

            entity.HasOne(d => d.IdSalesRepresentativeNavigation).WithMany(p => p.TblProperties)
                .HasForeignKey(d => d.IdSalesRepresentative)
                .HasConstraintName("FK_Property_SalesRepresentative");

            entity.HasOne(d => d.IdSubGroupNavigation).WithMany(p => p.TblPropertyIdSubGroupNavigations)
                .HasForeignKey(d => d.IdSubGroup)
                .HasConstraintName("FK_Property_SubGroup");
        });

        modelBuilder.Entity<TblPropertyAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Property_Address");

            entity.ToTable("Tbl_Property_Address");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BuiltupArea)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Details).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Latitude).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Phone)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PlotArea)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Street).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Zipcode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("ZIPCode");

            entity.HasOne(d => d.IdAreaNavigation).WithMany(p => p.TblPropertyAddresses)
                .HasForeignKey(d => d.IdArea)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Property_Address_Area");

            entity.HasOne(d => d.IdCityNavigation).WithMany(p => p.TblPropertyAddresses)
                .HasForeignKey(d => d.IdCity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Property_Address_City");

            entity.HasOne(d => d.IdCountryNavigation).WithMany(p => p.TblPropertyAddresses)
                .HasForeignKey(d => d.IdCountry)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Property_Address_Country");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblPropertyAddresses)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Property_Address_LegalEntity");

            entity.HasOne(d => d.IdPropertyNavigation).WithMany(p => p.TblPropertyAddresses)
                .HasForeignKey(d => d.IdProperty)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Property_Address_Property");

            entity.HasOne(d => d.IdStateNavigation).WithMany(p => p.TblPropertyAddresses)
                .HasForeignKey(d => d.IdState)
                .HasConstraintName("FK_Property_Address_State");
        });

        modelBuilder.Entity<TblPropertyFacility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PropertyFacility");

            entity.ToTable("Tbl_Property_Facility");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Note).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdFacilityNavigation).WithMany(p => p.TblPropertyFacilities)
                .HasForeignKey(d => d.IdFacility)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PropertyFacility_Facility");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblPropertyFacilities)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_PropertyFacility_LegalEntity");

            entity.HasOne(d => d.IdPropertyNavigation).WithMany(p => p.TblPropertyFacilities)
                .HasForeignKey(d => d.IdProperty)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PropertyFacility_Property");
        });

        modelBuilder.Entity<TblPropertyGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PropertyGroup");

            entity.ToTable("Tbl_PropertyGroup");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblPropertyGroups)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_PropertyGroup_LegalEntity");

            entity.HasOne(d => d.IdParentNavigation).WithMany(p => p.InverseIdParentNavigation)
                .HasForeignKey(d => d.IdParent)
                .HasConstraintName("FK_PropertyGroup_Parent");
        });

        modelBuilder.Entity<TblPropertyNeighborhood>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PropertyNeighborhood");

            entity.ToTable("Tbl_Property_Neighborhood");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Note).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblPropertyNeighborhoods)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_PropertyNeighborhood_LegalEntity");

            entity.HasOne(d => d.IdNeighborhoodNavigation).WithMany(p => p.TblPropertyNeighborhoods)
                .HasForeignKey(d => d.IdNeighborhood)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PropertyNeighborhood_Neighborhood");

            entity.HasOne(d => d.IdPropertyNavigation).WithMany(p => p.TblPropertyNeighborhoods)
                .HasForeignKey(d => d.IdProperty)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PropertyNeighborhood_Property");
        });

        modelBuilder.Entity<TblPropertyProgress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PropertyProgress");

            entity.ToTable("Tbl_PropertyProgress");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CheckedDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Percentage).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblPropertyProgresses)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_PropertyProgress_LegalEntity");

            entity.HasOne(d => d.IdPropertyNavigation).WithMany(p => p.TblPropertyProgresses)
                .HasForeignKey(d => d.IdProperty)
                .HasConstraintName("FK_PropertyProgress_Property");
        });

        modelBuilder.Entity<TblRemark>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Remarks");

            entity.ToTable("Tbl_Remarks");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdCreatedByNavigation).WithMany(p => p.TblRemarks)
                .HasForeignKey(d => d.IdCreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Remarks_User");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblRemarks)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Remarks_LegalEntity");

            entity.HasOne(d => d.IdWorkOrderNavigation).WithMany(p => p.TblRemarks)
                .HasForeignKey(d => d.IdWorkOrder)
                .HasConstraintName("FK_Remarks_WorkOrder");
        });

        modelBuilder.Entity<TblRequiredAttachmentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_RequiredAttachment");

            entity.ToTable("Tbl_RequiredAttachmentType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.IdAttachmentTypeNavigation).WithMany(p => p.TblRequiredAttachmentTypes)
                .HasForeignKey(d => d.IdAttachmentType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequiredAttachment_AttachmentType");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblRequiredAttachmentTypes)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_RequiredAttachment_LegalEntity");
        });

        modelBuilder.Entity<TblRequiredAttestationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_RequiredAttestationType");

            entity.ToTable("Tbl_RequiredAttestationType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.IdAttestationTypeNavigation).WithMany(p => p.TblRequiredAttestationTypes)
                .HasForeignKey(d => d.IdAttestationType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequiredAttestationType_AttestationType");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblRequiredAttestationTypes)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_RequiredAttestationType_LegalEntity");

            entity.HasOne(d => d.IdStateNavigation).WithMany(p => p.TblRequiredAttestationTypes)
                .HasForeignKey(d => d.IdState)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequiredAttestationType_State");
        });

        modelBuilder.Entity<TblRevenueRecognitionMonthly>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Reve__3214EC27A0666521");

            entity.ToTable("Tbl_RevenueRecognitionMonthly");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ContractCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(200)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DailyRate).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.InvoiceCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MonthlyAmount).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.PostingDate).HasColumnType("datetime");
            entity.Property(e => e.RemainingFromUnitRate).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.UnitRate).HasColumnType("decimal(18, 4)");
        });

        modelBuilder.Entity<TblSalesContract>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SalesContract");

            entity.ToTable("Tbl_SalesContract");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActualFromDate).HasColumnType("datetime");
            entity.Property(e => e.ActualToDate).HasColumnType("datetime");
            entity.Property(e => e.ApprovalDate).HasColumnType("datetime");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerInfoRef).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.DiscountPercent)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.HandOverDate).HasColumnType("datetime");
            entity.Property(e => e.IdDocumentStatus).HasDefaultValue(1);
            entity.Property(e => e.LegalStatus)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.NetAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ParticularAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ParticularDiscountAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ParticularTaxAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TaxAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TaxPercent)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.TblSalesContracts)
                .HasForeignKey(d => d.IdCustomer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesContract_Customer");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblSalesContracts)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_SalesContract_LegalEntity");

            entity.HasOne(d => d.IdPaymentTermNavigation).WithMany(p => p.TblSalesContracts)
                .HasForeignKey(d => d.IdPaymentTerm)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesContract_PaymentTerm");

            entity.HasOne(d => d.IdSalesOrderNavigation).WithMany(p => p.TblSalesContracts)
                .HasForeignKey(d => d.IdSalesOrder)
                .HasConstraintName("FK_SalesContract_SalesOrder");

            entity.HasOne(d => d.IdSalesQuotationNavigation).WithMany(p => p.TblSalesContracts)
                .HasForeignKey(d => d.IdSalesQuotation)
                .HasConstraintName("FK_SalesContract_SalesQuotation");

            entity.HasOne(d => d.IdTransactionSourceNavigation).WithMany(p => p.TblSalesContracts)
                .HasForeignKey(d => d.IdTransactionSource)
                .HasConstraintName("FK_SalesContract_TransactionSource");

            entity.HasOne(d => d.IdUnitNavigation).WithMany(p => p.TblSalesContracts)
                .HasForeignKey(d => d.IdUnit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesContract_Unit");

            entity.HasOne(d => d.IdUnitRateNavigation).WithMany(p => p.TblSalesContracts)
                .HasForeignKey(d => d.IdUnitRate)
                .HasConstraintName("FK_SalesContract_UnitRate");
        });

        modelBuilder.Entity<TblSalesInvoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SalesInvoice");

            entity.ToTable("Tbl_SalesInvoice");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 6)");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblSalesInvoices)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_SalesInvoice_LegalEntity");

            entity.HasOne(d => d.IdSalesContractNavigation).WithMany(p => p.TblSalesInvoices)
                .HasForeignKey(d => d.IdSalesContract)
                .HasConstraintName("FK_SalesInvoice_SalesContract");

            entity.HasOne(d => d.IdSalesOrderNavigation).WithMany(p => p.TblSalesInvoices)
                .HasForeignKey(d => d.IdSalesOrder)
                .HasConstraintName("FK_SalesInvoice_SalesOrder");
        });

        modelBuilder.Entity<TblSalesInvoiceInstallment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SalesInvoiceInstallment");

            entity.ToTable("Tbl_SalesInvoiceInstallment");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.InstallmentPercentage).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.IsPosted).HasDefaultValue(false);
            entity.Property(e => e.PostingDate).HasColumnType("datetime");
            entity.Property(e => e.RemainingAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.RemainingTaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ReveueGeneratedDate).HasColumnType("datetime");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TotalRemainingAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblSalesInvoiceInstallments)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_SalesInvoiceInstallment_LegalEntity");

            entity.HasOne(d => d.IdParticularNavigation).WithMany(p => p.TblSalesInvoiceInstallments)
                .HasForeignKey(d => d.IdParticular)
                .HasConstraintName("FK_SalesInvoiceInstallment_Particular");

            entity.HasOne(d => d.IdSalesContractNavigation).WithMany(p => p.TblSalesInvoiceInstallments)
                .HasForeignKey(d => d.IdSalesContract)
                .HasConstraintName("FK_SalesInvoiceInstallment_SalesContract");

            entity.HasOne(d => d.IdSalesInvoiceNavigation).WithMany(p => p.TblSalesInvoiceInstallments)
                .HasForeignKey(d => d.IdSalesInvoice)
                .HasConstraintName("FK_SalesInvoiceInstallment_SalesInvoice");

            entity.HasOne(d => d.IdSalesOrderNavigation).WithMany(p => p.TblSalesInvoiceInstallments)
                .HasForeignKey(d => d.IdSalesOrder)
                .HasConstraintName("FK_SalesInvoiceInstallment_SalesOrder");

            entity.HasOne(d => d.IdSalesQuotationNavigation).WithMany(p => p.TblSalesInvoiceInstallments)
                .HasForeignKey(d => d.IdSalesQuotation)
                .HasConstraintName("FK_SalesInvoiceInstallment_SalesQuotation");
        });

        modelBuilder.Entity<TblSalesOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SalesOrder");

            entity.ToTable("Tbl_SalesOrder");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActualFromDate).HasColumnType("datetime");
            entity.Property(e => e.ActualToDate).HasColumnType("datetime");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerInfoRef).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.DiscountPercent)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.HandOverDate).HasColumnType("datetime");
            entity.Property(e => e.IdDocumentStatus).HasDefaultValue(1);
            entity.Property(e => e.NetAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ParticularAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ParticularDiscountAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ParticularTaxAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TaxAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TaxPercent)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.TblSalesOrders)
                .HasForeignKey(d => d.IdCustomer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOrder_Customer");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblSalesOrders)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_SalesOrder_LegalEntity");

            entity.HasOne(d => d.IdPaymentTermNavigation).WithMany(p => p.TblSalesOrders)
                .HasForeignKey(d => d.IdPaymentTerm)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOrder_PaymentTerm");

            entity.HasOne(d => d.IdRenewalSalesContractNavigation).WithMany(p => p.TblSalesOrders)
                .HasForeignKey(d => d.IdRenewalSalesContract)
                .HasConstraintName("FK_SalesOrder_RenewalSalesContract");

            entity.HasOne(d => d.IdSalesQuotationNavigation).WithMany(p => p.TblSalesOrders)
                .HasForeignKey(d => d.IdSalesQuotation)
                .HasConstraintName("FK_SalesOrder_SalesQuotation");

            entity.HasOne(d => d.IdTransactionSourceNavigation).WithMany(p => p.TblSalesOrders)
                .HasForeignKey(d => d.IdTransactionSource)
                .HasConstraintName("FK_SalesOrder_TransactionSource");

            entity.HasOne(d => d.IdUnitNavigation).WithMany(p => p.TblSalesOrders)
                .HasForeignKey(d => d.IdUnit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesOrder_Unit");

            entity.HasOne(d => d.IdUnitRateNavigation).WithMany(p => p.TblSalesOrders)
                .HasForeignKey(d => d.IdUnitRate)
                .HasConstraintName("FK_SalesOrder_UnitRate");
        });

        modelBuilder.Entity<TblSalesQuotation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SalesQuotation");

            entity.ToTable("Tbl_SalesQuotation");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActualFromDate).HasColumnType("datetime");
            entity.Property(e => e.ActualToDate).HasColumnType("datetime");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerInfoRef).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.DiscountPercent)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.HandOverDate).HasColumnType("datetime");
            entity.Property(e => e.IdDocumentStatus).HasDefaultValue(1);
            entity.Property(e => e.NetAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ParticularAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ParticularDiscountAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ParticularTaxAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TaxAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TaxPercent)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.ValidTo).HasColumnType("datetime");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.TblSalesQuotations)
                .HasForeignKey(d => d.IdCustomer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesQuotation_Customer");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblSalesQuotations)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_SalesQuotation_LegalEntity");

            entity.HasOne(d => d.IdPaymentTermNavigation).WithMany(p => p.TblSalesQuotations)
                .HasForeignKey(d => d.IdPaymentTerm)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesQuotation_PaymentTerm");

            entity.HasOne(d => d.IdTransactionSourceNavigation).WithMany(p => p.TblSalesQuotations)
                .HasForeignKey(d => d.IdTransactionSource)
                .HasConstraintName("FK_SalesQuotation_TransactionSource");

            entity.HasOne(d => d.IdUnitNavigation).WithMany(p => p.TblSalesQuotations)
                .HasForeignKey(d => d.IdUnit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesQuotation_Unit");

            entity.HasOne(d => d.IdUnitRateNavigation).WithMany(p => p.TblSalesQuotations)
                .HasForeignKey(d => d.IdUnitRate)
                .HasConstraintName("FK_SalesQuotation_UnitRate");
        });

        modelBuilder.Entity<TblSecurityDepositReport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_Secu__3214EC272863D56F");

            entity.ToTable("Tbl_SecurityDepositReport");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ContractAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ContractCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ContractCreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(200)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.InstallmentType)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.InvoiceCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularName)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesOrderCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesQuotationCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SecurityDepositAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UnitName)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<TblSm>(entity =>
        {
            entity.ToTable("Tbl_SMS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Body).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RecipientNumber).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ResponseCode)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ResponseMessage).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SendDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblSms)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_SMS_LegalEntity");
        });

        modelBuilder.Entity<TblSmsResponse>(entity =>
        {
            entity.ToTable("Tbl_SMS_Response");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdSms).HasColumnName("IdSMS");
            entity.Property(e => e.RecipientNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ResponseCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ResponseMessage).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SentDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblSmsResponses)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_SMS_Response_LegalEntity");

            entity.HasOne(d => d.IdSmsNavigation).WithMany(p => p.TblSmsResponses)
                .HasForeignKey(d => d.IdSms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SMS_Response_SMS");
        });

        modelBuilder.Entity<TblSparePartRepairRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SparePartRepair");

            entity.ToTable("Tbl_SparePartRepairRequest");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StoreRemarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblSparePartRepairRequests)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_SparePartRepair_LegalEntity");

            entity.HasOne(d => d.IdTechncianNavigation).WithMany(p => p.TblSparePartRepairRequests)
                .HasForeignKey(d => d.IdTechncian)
                .HasConstraintName("FK_SparePartRepair_Techncian");
        });

        modelBuilder.Entity<TblSparePartRepairRequestDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SparePartRepairDetail");

            entity.ToTable("Tbl_SparePartRepairRequestDetail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Remark).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SerialNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SparePartNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdFromWarehouseNavigation).WithMany(p => p.TblSparePartRepairRequestDetailIdFromWarehouseNavigations)
                .HasForeignKey(d => d.IdFromWarehouse)
                .HasConstraintName("FK_SparePartRepairDetail_FromWarehouse");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblSparePartRepairRequestDetails)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_SparePartRepairDetail_LegalEntity");

            entity.HasOne(d => d.IdSparePartRepairRequestNavigation).WithMany(p => p.TblSparePartRepairRequestDetails)
                .HasForeignKey(d => d.IdSparePartRepairRequest)
                .HasConstraintName("FK_SparePartRepairDetail_SparePartRepair");

            entity.HasOne(d => d.IdToWarehouseNavigation).WithMany(p => p.TblSparePartRepairRequestDetailIdToWarehouseNavigations)
                .HasForeignKey(d => d.IdToWarehouse)
                .HasConstraintName("FK_SparePartRepairDetail_ToWarehouse");
        });

        modelBuilder.Entity<TblSparePartRepairTransfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SparePartRepairTransfer");

            entity.ToTable("Tbl_SparePartRepairTransfer");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblSparePartRepairTransfers)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_SparePartRepairTransfer_LegalEntity");

            entity.HasOne(d => d.IdSparePartRepairRequestNavigation).WithMany(p => p.TblSparePartRepairTransfers)
                .HasForeignKey(d => d.IdSparePartRepairRequest)
                .HasConstraintName("FK_SparePartRepairTransfer_SparePartRepairRequest");

            entity.HasOne(d => d.IdTechncianNavigation).WithMany(p => p.TblSparePartRepairTransfers)
                .HasForeignKey(d => d.IdTechncian)
                .HasConstraintName("FK_SparePartRepairTransfer_Techncian");
        });

        modelBuilder.Entity<TblSparePartRepairTransferDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SparePartRepairTransferDetail");

            entity.ToTable("Tbl_SparePartRepairTransferDetail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Remark).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SparePartNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdFromWarehouseNavigation).WithMany(p => p.TblSparePartRepairTransferDetailIdFromWarehouseNavigations)
                .HasForeignKey(d => d.IdFromWarehouse)
                .HasConstraintName("FK_SparePartRepairTransferDetail_FromWarehouse");

            entity.HasOne(d => d.IdItemNavigation).WithMany(p => p.TblSparePartRepairTransferDetails)
                .HasForeignKey(d => d.IdItem)
                .HasConstraintName("FK_SparePartRepairTransferDetail_Item");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblSparePartRepairTransferDetails)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_SparePartRepairTransferDetail_LegalEntity");

            entity.HasOne(d => d.IdSerialNumberNavigation).WithMany(p => p.TblSparePartRepairTransferDetails)
                .HasForeignKey(d => d.IdSerialNumber)
                .HasConstraintName("FK_SparePartRepairTransferDetail_SerialNumber");

            entity.HasOne(d => d.IdSparePartRepairTransferNavigation).WithMany(p => p.TblSparePartRepairTransferDetails)
                .HasForeignKey(d => d.IdSparePartRepairTransfer)
                .HasConstraintName("FK_SparePartRepairTransferDetail_SparePartRepairTransfer");

            entity.HasOne(d => d.IdToWarehouseNavigation).WithMany(p => p.TblSparePartRepairTransferDetailIdToWarehouseNavigations)
                .HasForeignKey(d => d.IdToWarehouse)
                .HasConstraintName("FK_SparePartRepairTransferDetail_ToWarehouse");
        });

        modelBuilder.Entity<TblSponsor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Sponsor");

            entity.ToTable("Tbl_Sponsor");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyRegistrationNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ComputerCard)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Number)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblSponsors)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Sponsor_LegalEntity");

            entity.HasOne(d => d.IdNationalityNavigation).WithMany(p => p.TblSponsors)
                .HasForeignKey(d => d.IdNationality)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sponsor_Nationality");
        });

        modelBuilder.Entity<TblState>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_State");

            entity.ToTable("Tbl_State");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdCountryNavigation).WithMany(p => p.TblStates)
                .HasForeignKey(d => d.IdCountry)
                .HasConstraintName("FK_State_Country");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblStates)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_State_LegalEntity");
        });

        modelBuilder.Entity<TblSupplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Supplier");

            entity.ToTable("Tbl_Supplier");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Group)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remark).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblSuppliers)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Supplier_LegalEntity");
        });

        modelBuilder.Entity<TblSystemAlert>(entity =>
        {
            entity.ToTable("Tbl_SystemAlert");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Body).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReadDate).HasColumnType("datetime");
            entity.Property(e => e.Subject).UseCollation("SQL_Latin1_General_CP1256_CS_AS");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblSystemAlerts)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_SystemAlert_LegalEntity");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TblSystemAlerts)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_SystemAlert_User");
        });

        modelBuilder.Entity<TblTimeSheet>(entity =>
        {
            entity.ToTable("Tbl_TimeSheet");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Hours).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdAssetNavigation).WithMany(p => p.TblTimeSheets)
                .HasForeignKey(d => d.IdAsset)
                .HasConstraintName("FK_TimeSheet_Asset");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblTimeSheets)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_TimeSheet_LegalEntity");

            entity.HasOne(d => d.IdMaintenanceTypeNavigation).WithMany(p => p.TblTimeSheets)
                .HasForeignKey(d => d.IdMaintenanceType)
                .HasConstraintName("FK_TimeSheet_MaintenanceType");

            entity.HasOne(d => d.IdTechncianNavigation).WithMany(p => p.TblTimeSheets)
                .HasForeignKey(d => d.IdTechncian)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TimeSheet_User");

            entity.HasOne(d => d.IdWorkOrderNavigation).WithMany(p => p.TblTimeSheets)
                .HasForeignKey(d => d.IdWorkOrder)
                .HasConstraintName("FK_TimeSheet_WorkOrder");
        });

        modelBuilder.Entity<TblTool>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Tools");

            entity.ToTable("Tbl_Tools");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblTools)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Tools_LegalEntity");
        });

        modelBuilder.Entity<TblToolTransfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ToolTransfer");

            entity.ToTable("Tbl_ToolTransfer");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblToolTransfers)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_ToolTransfer_LegalEntity");
        });

        modelBuilder.Entity<TblToolTransferTool>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Tbl_ToolTransfer_Items");

            entity.ToTable("Tbl_ToolTransfer_Tools");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdFromWarehouseNavigation).WithMany(p => p.TblToolTransferToolIdFromWarehouseNavigations)
                .HasForeignKey(d => d.IdFromWarehouse)
                .HasConstraintName("FK_ToolTransfer_Tools_FromWarehouse");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblToolTransferTools)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_ToolTransfer_Items_LegalEntity");

            entity.HasOne(d => d.IdTechnicianNavigation).WithMany(p => p.TblToolTransferTools)
                .HasForeignKey(d => d.IdTechnician)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_ToolTransfer_Items_Users");

            entity.HasOne(d => d.IdToWarehouseNavigation).WithMany(p => p.TblToolTransferToolIdToWarehouseNavigations)
                .HasForeignKey(d => d.IdToWarehouse)
                .HasConstraintName("FK_ToolTransfer_Items_ToWarehouse");

            entity.HasOne(d => d.IdToolNavigation).WithMany(p => p.TblToolTransferTools)
                .HasForeignKey(d => d.IdTool)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_ToolTransfer_Items_Tool");

            entity.HasOne(d => d.IdToolTransferNavigation).WithMany(p => p.TblToolTransferTools)
                .HasForeignKey(d => d.IdToolTransfer)
                .HasConstraintName("FK_ToolTransfer_Tools_ToolTransfer");
        });

        modelBuilder.Entity<TblTransactionSource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CommercialEvents");

            entity.ToTable("Tbl_TransactionSource");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblTransactionSources)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_CommercialEvents_LegalEntity");
        });

        modelBuilder.Entity<TblUnit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Unit");

            entity.ToTable("Tbl_Unit");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DefaultRentValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DefaultSalesValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Depth)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FlatNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Floor)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LastMergedDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Number)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SquareFeet)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Width)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblUnits)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Unit_LegalEntity");

            entity.HasOne(d => d.IdPropertyNavigation).WithMany(p => p.TblUnits)
                .HasForeignKey(d => d.IdProperty)
                .HasConstraintName("FK_Unit_Property");

            entity.HasOne(d => d.IdSalesEmployeeNavigation).WithMany(p => p.TblUnits)
                .HasForeignKey(d => d.IdSalesEmployee)
                .HasConstraintName("FK_Unit_SalesEmployee");

            entity.HasOne(d => d.IdUnitClassNavigation).WithMany(p => p.TblUnits)
                .HasForeignKey(d => d.IdUnitClass)
                .HasConstraintName("FK_Unit_UnitClass");

            entity.HasOne(d => d.IdUnitStatusNavigation).WithMany(p => p.TblUnits)
                .HasForeignKey(d => d.IdUnitStatus)
                .HasConstraintName("FK_Unit_Status");

            entity.HasOne(d => d.IdUnitUseNavigation).WithMany(p => p.TblUnits)
                .HasForeignKey(d => d.IdUnitUse)
                .HasConstraintName("FK_Unit_UnitUse");

            entity.HasOne(d => d.IdUnitViewNavigation).WithMany(p => p.TblUnits)
                .HasForeignKey(d => d.IdUnitView)
                .HasConstraintName("FK_Unit_UnitView");
        });

        modelBuilder.Entity<TblUnitClassParticular>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UnitClassParticular");

            entity.ToTable("Tbl_UnitClassParticular");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");
            entity.Property(e => e.EffectiveTo).HasColumnType("datetime");
            entity.Property(e => e.RefundableAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Tax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblUnitClassParticulars)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_UnitClassParticular_LegalEntity");

            entity.HasOne(d => d.IdParticularNavigation).WithMany(p => p.TblUnitClassParticulars)
                .HasForeignKey(d => d.IdParticular)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitClassParticular_Particular");

            entity.HasOne(d => d.IdUnitClassNavigation).WithMany(p => p.TblUnitClassParticulars)
                .HasForeignKey(d => d.IdUnitClass)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitClassParticular_UnitClass");
        });

        modelBuilder.Entity<TblUnitCounter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UnitCounters");

            entity.ToTable("Tbl_UnitCounters");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Count)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CounterDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblUnitCounters)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_UnitCounters_LegalEntity");

            entity.HasOne(d => d.IdUnitNavigation).WithMany(p => p.TblUnitCounters)
                .HasForeignKey(d => d.IdUnit)
                .HasConstraintName("FK_UnitCounters_Unit");
        });

        modelBuilder.Entity<TblUnitParking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UnitParking");

            entity.ToTable("Tbl_Unit_Parking");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblUnitParkings)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_UnitParking_LegalEntity");

            entity.HasOne(d => d.IdParkingNavigation).WithMany(p => p.TblUnitParkings)
                .HasForeignKey(d => d.IdParking)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitParking_Parking");

            entity.HasOne(d => d.IdUnitNavigation).WithMany(p => p.TblUnitParkings)
                .HasForeignKey(d => d.IdUnit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitParking_Unit");
        });

        modelBuilder.Entity<TblUnitParticular>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UnitParticular");

            entity.ToTable("Tbl_UnitParticular");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");
            entity.Property(e => e.EffectiveTo).HasColumnType("datetime");
            entity.Property(e => e.RefundableAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Tax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblUnitParticulars)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_UnitParticular_LegalEntity");

            entity.HasOne(d => d.IdParticularNavigation).WithMany(p => p.TblUnitParticulars)
                .HasForeignKey(d => d.IdParticular)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitParticular_Particular");

            entity.HasOne(d => d.IdUnitNavigation).WithMany(p => p.TblUnitParticulars)
                .HasForeignKey(d => d.IdUnit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitParticular_Unit");
        });

        modelBuilder.Entity<TblUnitRate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UnitRate");

            entity.ToTable("Tbl_UnitRate");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MaxDiscountPercent).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaximumRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinimumRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Rate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Remark).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TaxPercentage).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblUnitRates)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_UnitRate_LegalEntity");
        });

        modelBuilder.Entity<TblUnitStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UnitStatus");

            entity.ToTable("Tbl_UnitStatus");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblUnitStatuses)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_UnitStatus_LegalEntity");
        });

        modelBuilder.Entity<TblUnitUse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UnitUse");

            entity.ToTable("Tbl_UnitUse");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblUnitUses)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_UnitUse_LegalEntity");
        });

        modelBuilder.Entity<TblUnitView>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UnitView");

            entity.ToTable("Tbl_UnitView");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblUnitViews)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_UnitView_LegalEntity");
        });

        modelBuilder.Entity<TblUnitVisit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UnitVisit");

            entity.ToTable("Tbl_UnitVisit");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Note).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.ViewDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.TblUnitVisits)
                .HasForeignKey(d => d.IdCustomer)
                .HasConstraintName("FK_UnitVisit_Customer");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblUnitVisits)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_UnitVisit_LegalEntity");

            entity.HasOne(d => d.IdPropertyNavigation).WithMany(p => p.TblUnitVisits)
                .HasForeignKey(d => d.IdProperty)
                .HasConstraintName("FK_UnitVisit_Property");

            entity.HasOne(d => d.IdUnitNavigation).WithMany(p => p.TblUnitVisits)
                .HasForeignKey(d => d.IdUnit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitVisit_Unit");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_User");

            entity.ToTable("Tbl_User");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IdApprovalStatus).HasDefaultValue(1);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LoginKey)
                .HasMaxLength(800)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Password)
                .HasMaxLength(800)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RatePerHour).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.IdEmployee)
                .HasConstraintName("FK_User_Employee");
        });

        modelBuilder.Entity<TblUserGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserGroup");

            entity.ToTable("Tbl_UserGroup");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblUserGroups)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_UserGroup_LegalEntity");
        });

        modelBuilder.Entity<TblUserGroupMenu>(entity =>
        {
            entity.HasKey(e => new { e.IdUserGroup, e.IdMenu }).HasName("PK_UserGroup_Menu");

            entity.ToTable("Tbl_UserGroup_Menu");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblUserGroupMenus)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_UserGroup_Menu_LegalEntity");

            entity.HasOne(d => d.IdMenuNavigation).WithMany(p => p.TblUserGroupMenus)
                .HasForeignKey(d => d.IdMenu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserGroup_Menu_Menu");

            entity.HasOne(d => d.IdUserGroupNavigation).WithMany(p => p.TblUserGroupMenus)
                .HasForeignKey(d => d.IdUserGroup)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserGroup_Menu_UserGroup");
        });

        modelBuilder.Entity<TblUserGroupPermission>(entity =>
        {
            entity.HasKey(e => new { e.IdUserGroup, e.UserPermissionName });

            entity.ToTable("Tbl_UserGroup_Permission");

            entity.Property(e => e.UserPermissionName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblUserGroupPermissions)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_UserGroup_Permission_LegalEntity");

            entity.HasOne(d => d.IdUserGroupNavigation).WithMany(p => p.TblUserGroupPermissions)
                .HasForeignKey(d => d.IdUserGroup)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserGroup_Permission_UserGroup");
        });

        modelBuilder.Entity<TblUserGroupUser>(entity =>
        {
            entity.HasKey(e => new { e.IdUserGroup, e.IdUser }).HasName("PK_UserGroup_User");

            entity.ToTable("Tbl_UserGroup_User");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblUserGroupUsers)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_UserGroup_User_LegalEntity");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TblUserGroupUsers)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserGroup_User_User");

            entity.HasOne(d => d.IdUserGroupNavigation).WithMany(p => p.TblUserGroupUsers)
                .HasForeignKey(d => d.IdUserGroup)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserGroup_User_UserGroup");
        });

        modelBuilder.Entity<TblUserPermission>(entity =>
        {
            entity.HasKey(e => e.Name);

            entity.ToTable("Tbl_UserPermission");

            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDescription)
                .HasMaxLength(1000)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignScreenName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ScreenName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblUserPermissions)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_UserPermission_LegalEntity");
        });

        modelBuilder.Entity<REFRESH_TOKENS>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_REFRESH_TOKENS");

            entity.ToTable("REFRESH_TOKENS");

            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.UserId).HasColumnName("UserId");
            entity.Property(e => e.Token)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("Token");
            entity.Property(e => e.ExpiresAt)
                .IsRequired()
                .HasColumnType("datetime")
                .HasColumnName("ExpiresAt");
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("CreatedAt");
            entity.Property(e => e.RevokedAt)
                .HasColumnType("datetime")
                .HasColumnName("RevokedAt");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .HasColumnName("IsActive");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(50)
                .HasColumnName("IpAddress");
            entity.Property(e => e.UserAgent)
                .HasMaxLength(500)
                .HasColumnName("UserAgent");

            // Indexes
            entity.HasIndex(e => e.Token).HasDatabaseName("IX_REFRESH_TOKENS_Token");
            entity.HasIndex(e => e.UserId).HasDatabaseName("IX_REFRESH_TOKENS_UserId");
            entity.HasIndex(e => e.ExpiresAt).HasDatabaseName("IX_REFRESH_TOKENS_ExpiresAt");
        });

        modelBuilder.Entity<TblWarehouse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Warehouse");

            entity.ToTable("Tbl_Warehouse");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Location).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWarehouses)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Warehouse_LegalEntity");
        });

        modelBuilder.Entity<TblWarehouseBinLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Warehouse_BinLocation");

            entity.ToTable("Tbl_Warehouse_BinLocation");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWarehouseBinLocations)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Warehouse_BinLocation_LegalEntity");

            entity.HasOne(d => d.IdWarehouseNavigation).WithMany(p => p.TblWarehouseBinLocations)
                .HasForeignKey(d => d.IdWarehouse)
                .HasConstraintName("FK_Warehouse_BinLocation_Warehouse");
        });

        modelBuilder.Entity<TblWarehouseItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Warehouse_Item");

            entity.ToTable("Tbl_Warehouse_Item");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdBinLocationNavigation).WithMany(p => p.TblWarehouseItems)
                .HasForeignKey(d => d.IdBinLocation)
                .HasConstraintName("FK_Warehouse_Item_BinLocation");

            entity.HasOne(d => d.IdItemNavigation).WithMany(p => p.TblWarehouseItems)
                .HasForeignKey(d => d.IdItem)
                .HasConstraintName("FK_Warehouse_Item_Item");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWarehouseItems)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Warehouse_Item_LegalEntity");

            entity.HasOne(d => d.IdWarehouseNavigation).WithMany(p => p.TblWarehouseItems)
                .HasForeignKey(d => d.IdWarehouse)
                .HasConstraintName("FK_Warehouse_Item_Warehouse");
        });

        modelBuilder.Entity<TblWorkOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Work_Order");

            entity.ToTable("Tbl_WorkOrder");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClosingDate).HasColumnType("datetime");
            entity.Property(e => e.CompleteDate).HasColumnType("datetime");
            entity.Property(e => e.CorrectiveAction)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DocumentDate).HasColumnType("datetime");
            entity.Property(e => e.DocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.EquipmentDetails)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Findings)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IdWocategory).HasColumnName("IdWOCategory");
            entity.Property(e => e.InductionExplanation)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("Induction_Explanation");
            entity.Property(e => e.Inspection)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MachineDownTime).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MachineStartingTime).HasColumnType("datetime");
            entity.Property(e => e.MachineStoppingTime).HasColumnType("datetime");
            entity.Property(e => e.ManholeNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RequiredDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.TblWorkOrders)
                .HasForeignKey(d => d.IdCustomer)
                .HasConstraintName("FK_WorkOrder_Customer");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWorkOrders)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_WorkOrder_LegalEntity");

            entity.HasOne(d => d.IdWorkOrderCategoryNavigation).WithMany(p => p.TblWorkOrders)
                .HasForeignKey(d => d.IdWorkOrderCategory)
                .HasConstraintName("FK_WorkOrder_WorkOrderCategory");

            entity.HasOne(d => d.IdZoneNavigation).WithMany(p => p.TblWorkOrders)
                .HasForeignKey(d => d.IdZone)
                .HasConstraintName("FK_WorkOrder_Zone");
        });

        modelBuilder.Entity<TblWorkOrderAsset>(entity =>
        {
            entity.HasKey(e => new { e.IdWorkOrder, e.IdAsset });

            entity.ToTable("Tbl_WorkOrder_Asset");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdAssetNavigation).WithMany(p => p.TblWorkOrderAssets)
                .HasForeignKey(d => d.IdAsset)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_WorkOrder_Asset_Asset");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWorkOrderAssets)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_WorkOrder_Asset_LegalEntity");

            entity.HasOne(d => d.IdWorkOrderNavigation).WithMany(p => p.TblWorkOrderAssets)
                .HasForeignKey(d => d.IdWorkOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_WorkOrder_Asset_WorkOrder");
        });

        modelBuilder.Entity<TblWorkOrderAttachment>(entity =>
        {
            entity.HasKey(e => new { e.IdWorkOrder, e.IdAttachment }).HasName("PK__Tbl_Work__09F327265933BD92");

            entity.ToTable("Tbl_WorkOrder_Attachment");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdAttachmentNavigation).WithMany(p => p.TblWorkOrderAttachments)
                .HasForeignKey(d => d.IdAttachment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkOrder_Attachment_Attachment");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWorkOrderAttachments)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_WorkOrder_Attachment_LegalEntity");

            entity.HasOne(d => d.IdWorkOrderNavigation).WithMany(p => p.TblWorkOrderAttachments)
                .HasForeignKey(d => d.IdWorkOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkOrder_Attachment_WorkOrder");
        });

        modelBuilder.Entity<TblWorkOrderCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_WorkType");

            entity.ToTable("Tbl_WorkOrderCategory");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWorkOrderCategories)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_WorkType_LegalEntity");
        });

        modelBuilder.Entity<TblWorkOrderExpense>(entity =>
        {
            entity.ToTable("Tbl_WorkOrder_Expense");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdAssetNavigation).WithMany(p => p.TblWorkOrderExpenses)
                .HasForeignKey(d => d.IdAsset)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_WorkOrder_Expense_Asset");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWorkOrderExpenses)
                .HasForeignKey(d => d.IdLegalEntity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkOrder_Expense_LegalEntity");

            entity.HasOne(d => d.IdWorkOrderNavigation).WithMany(p => p.TblWorkOrderExpenses)
                .HasForeignKey(d => d.IdWorkOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_WorkOrder_Expense_WorkOrder");
        });

        modelBuilder.Entity<TblWorkOrderGoodsIssue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_WorkOrder_GoodsIssue");

            entity.ToTable("Tbl_WorkOrder_GoodsIssue");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActualQuantity).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdAssetNavigation).WithMany(p => p.TblWorkOrderGoodsIssues)
                .HasForeignKey(d => d.IdAsset)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_WorkOrder_GoodsIssue_Asset");

            entity.HasOne(d => d.IdItemNavigation).WithMany(p => p.TblWorkOrderGoodsIssues)
                .HasForeignKey(d => d.IdItem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_WorkOrder_GoodsIssue_Items");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWorkOrderGoodsIssues)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_WorkOrder_GoodsIssue_LegalEntity");

            entity.HasOne(d => d.IdMaintenanceTypeNavigation).WithMany(p => p.TblWorkOrderGoodsIssues)
                .HasForeignKey(d => d.IdMaintenanceType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_WorkOrder_GoodsIssue_MaintenanceType");

            entity.HasOne(d => d.IdWarehouseNavigation).WithMany(p => p.TblWorkOrderGoodsIssues)
                .HasForeignKey(d => d.IdWarehouse)
                .HasConstraintName("FK_WorkOrder_GoodsIssue_Warehouse");

            entity.HasOne(d => d.IdWorkOrderNavigation).WithMany(p => p.TblWorkOrderGoodsIssues)
                .HasForeignKey(d => d.IdWorkOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_WorkOrder_GoodsIssue_WorkOrder");
        });

        modelBuilder.Entity<TblWorkOrderHseAtmosphericMonitoring>(entity =>
        {
            entity.HasKey(e => new { e.IdWorkOrder, e.IdAtmosphericMonitoring }).HasName("PK__Tbl_HSEP__24958CA5BD3E68DC");

            entity.ToTable("Tbl_WorkOrderHSE_AtmosphericMonitoring");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Time).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWorkOrderHseAtmosphericMonitorings)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_WorkOrderHSE_AtmosphericMonitoring_LegalEntity");
        });

        modelBuilder.Entity<TblWorkOrderHseCondition>(entity =>
        {
            entity.HasKey(e => new { e.IdWorkOrder, e.IdConditions }).HasName("PK__Tbl_HSEP__153A930E1C2D3A01");

            entity.ToTable("Tbl_WorkOrderHSE_Conditions");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWorkOrderHseConditions)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_WorkOrderHSE_Conditions_LegalEntity");
        });

        modelBuilder.Entity<TblWorkOrderHseDeepExcavationControl>(entity =>
        {
            entity.HasKey(e => new { e.IdWorkOrder, e.IdControls }).HasName("PK__Tbl_Work__10C438EDC08D3DF4");

            entity.ToTable("Tbl_WorkOrderHSE_DeepExcavationControls");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWorkOrderHseDeepExcavationControls)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_WorkOrderHSE_DeepExcavationControls_LegalEntity");
        });

        modelBuilder.Entity<TblWorkOrderHseExcavationSafetyEquipment>(entity =>
        {
            entity.HasKey(e => new { e.IdWorkOrder, e.IdExcavationSafetyEquipment }).HasName("PK__Tbl_Work__9C6E87B92D66EC7E");

            entity.ToTable("Tbl_WorkOrderHSE_ExcavationSafetyEquipment");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWorkOrderHseExcavationSafetyEquipments)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_WorkOrderHSE_ExcavationSafetyEquipment_LegalEntity");
        });

        modelBuilder.Entity<TblWorkOrderHseExcavationWork>(entity =>
        {
            entity.HasKey(e => new { e.IdWorkOrder, e.IdExcavationWork }).HasName("PK__Tbl_Work__0C8061F83B027D51");

            entity.ToTable("Tbl_WorkOrderHSE_ExcavationWork");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Details).UseCollation("SQL_Latin1_General_CP1256_CS_AS");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWorkOrderHseExcavationWorks)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_WorkOrderHSE_ExcavationWork_LegalEntity");
        });

        modelBuilder.Entity<TblWorkOrderHseSafetyEquipment>(entity =>
        {
            entity.HasKey(e => new { e.IdWorkOrder, e.IdSafetyEquipment }).HasName("PK__Tbl_HSEP__FE04F658C9B284E1");

            entity.ToTable("Tbl_WorkOrderHSE_SafetyEquipment");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWorkOrderHseSafetyEquipments)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_WorkOrderHSE_SafetyEquipment_LegalEntity");
        });

        modelBuilder.Entity<TblWorkOrderHseprocedure>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.IdWorkOrder, e.IdHsetype }).HasName("PK_WorkOrder_HSEProcedure");

            entity.ToTable("Tbl_WorkOrder_HSEProcedure");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.IdHsetype).HasColumnName("IdHSEType");
            entity.Property(e => e.Area)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExcavationDescription).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWorkOrderHseprocedures)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_WorkOrder_HSEProcedure_LegalEntity");
        });

        modelBuilder.Entity<TblWorkOrderMaintenanceRequest>(entity =>
        {
            entity.ToTable("Tbl_WorkOrder_MaintenanceRequest");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeffectDate).HasColumnType("datetime");
            entity.Property(e => e.PostingDate).HasColumnType("datetime");
            entity.Property(e => e.Remark).UseCollation("SQL_Latin1_General_CP1256_CS_AS");

            entity.HasOne(d => d.IdAssetNavigation).WithMany(p => p.TblWorkOrderMaintenanceRequests)
                .HasForeignKey(d => d.IdAsset)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_WorkOrder_MaintenanceRequest_Asset");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.TblWorkOrderMaintenanceRequests)
                .HasForeignKey(d => d.IdCustomer)
                .HasConstraintName("FK_Tbl_WorkOrder_MaintenanceRequest_Customer");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWorkOrderMaintenanceRequests)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_WorkOrder_MaintenanceRequest_LegalEntity");

            entity.HasOne(d => d.IdMaintenanceTypeNavigation).WithMany(p => p.TblWorkOrderMaintenanceRequests)
                .HasForeignKey(d => d.IdMaintenanceType)
                .HasConstraintName("FK_Tbl_WorkOrder_MaintenanceRequest_MaintenanceType");

            entity.HasOne(d => d.IdWorkOrderNavigation).WithMany(p => p.TblWorkOrderMaintenanceRequests)
                .HasForeignKey(d => d.IdWorkOrder)
                .HasConstraintName("FK_WorkOrder_MaintenanceRequest_WorkOrder");
        });

        modelBuilder.Entity<TblWorkOrderMaintenanceType>(entity =>
        {
            entity.ToTable("Tbl_WorkOrder_MaintenanceType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWorkOrderMaintenanceTypes)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_WorkOrder_MaintenanceType_LegalEntity");

            entity.HasOne(d => d.IdMaintenanceTypeNavigation).WithMany(p => p.TblWorkOrderMaintenanceTypes)
                .HasForeignKey(d => d.IdMaintenanceType)
                .HasConstraintName("FK_Tbl_WorkOrder_MaintenanceType_MaintenanceType");

            entity.HasOne(d => d.IdWorkOrderNavigation).WithMany(p => p.TblWorkOrderMaintenanceTypes)
                .HasForeignKey(d => d.IdWorkOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_WorkOrder_MaintenanceType_WorkOrder");
        });

        modelBuilder.Entity<TblWorkOrderRemark>(entity =>
        {
            entity.HasKey(e => new { e.IdWorkOrder, e.IdRemark }).HasName("PK__Tbl_Work__F36CBEB5935B0C7E");

            entity.ToTable("Tbl_WorkOrder_Remark");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWorkOrderRemarks)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_WorkOrder_Remark_LegalEntity");

            entity.HasOne(d => d.IdRemarkNavigation).WithMany(p => p.TblWorkOrderRemarks)
                .HasForeignKey(d => d.IdRemark)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkOrder_Remark_Remark");

            entity.HasOne(d => d.IdWorkOrderNavigation).WithMany(p => p.TblWorkOrderRemarks)
                .HasForeignKey(d => d.IdWorkOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkOrder_Remark_WorkOrder");
        });

        modelBuilder.Entity<TblWorkOrderSparePart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_WorkOrder_SparePart");

            entity.ToTable("Tbl_WorkOrder_SparePart");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActualQuantity).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EstimatedQuantity).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.QuantityToIssue).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.WarehouseQuantity).HasColumnType("decimal(18, 6)");

            entity.HasOne(d => d.IdAssetNavigation).WithMany(p => p.TblWorkOrderSpareParts)
                .HasForeignKey(d => d.IdAsset)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_WorkOrder_SparePart_Asset");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWorkOrderSpareParts)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_WorkOrder_SparePart_LegalEntity");

            entity.HasOne(d => d.IdMaintenanceTypeNavigation).WithMany(p => p.TblWorkOrderSpareParts)
                .HasForeignKey(d => d.IdMaintenanceType)
                .HasConstraintName("FK_Tbl_WorkOrder_SparePart_MaintenanceType");

            entity.HasOne(d => d.IdSparePartNavigation).WithMany(p => p.TblWorkOrderSpareParts)
                .HasForeignKey(d => d.IdSparePart)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_WorkOrder_SparePart_Items");

            entity.HasOne(d => d.IdWarehouseNavigation).WithMany(p => p.TblWorkOrderSpareParts)
                .HasForeignKey(d => d.IdWarehouse)
                .HasConstraintName("FK_WorkOrder_SparePart_Warehouse");

            entity.HasOne(d => d.IdWorkOrderNavigation).WithMany(p => p.TblWorkOrderSpareParts)
                .HasForeignKey(d => d.IdWorkOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_WorkOrder_SparePart_WorkOrder");
        });

        modelBuilder.Entity<TblWorkOrderTechnician>(entity =>
        {
            entity.ToTable("Tbl_WorkOrder_Technician");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActualHours).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdAssetNavigation).WithMany(p => p.TblWorkOrderTechnicians)
                .HasForeignKey(d => d.IdAsset)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_WorkOrder_Technician_Asset");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWorkOrderTechnicians)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_WorkOrder_Technician_LegalEntity");

            entity.HasOne(d => d.IdMaintenanceTypeNavigation).WithMany(p => p.TblWorkOrderTechnicians)
                .HasForeignKey(d => d.IdMaintenanceType)
                .HasConstraintName("FK_Tbl_WorkOrder_Technician_MaintenanceType");

            entity.HasOne(d => d.IdTechnicianNavigation).WithMany(p => p.TblWorkOrderTechnicians)
                .HasForeignKey(d => d.IdTechnician)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_WorkOrder_Technician_User");

            entity.HasOne(d => d.IdWorkOrderNavigation).WithMany(p => p.TblWorkOrderTechnicians)
                .HasForeignKey(d => d.IdWorkOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_WorkOrder_Technician_WorkOrder");
        });

        modelBuilder.Entity<TblWorkOrderTool>(entity =>
        {
            entity.HasKey(e => new { e.IdWorkOrder, e.IdTool });

            entity.ToTable("Tbl_WorkOrder_Tool");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWorkOrderTools)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_WorkOrder_Tool_LegalEntity");

            entity.HasOne(d => d.IdToolNavigation).WithMany(p => p.TblWorkOrderTools)
                .HasForeignKey(d => d.IdTool)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_WorkOrder_Tool");

            entity.HasOne(d => d.IdWorkOrderNavigation).WithMany(p => p.TblWorkOrderTools)
                .HasForeignKey(d => d.IdWorkOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tbl_WorkOrder_Tool_WorkOrder");
        });

        modelBuilder.Entity<TblWorkType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_WorkCategory");

            entity.ToTable("Tbl_WorkType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblWorkTypes)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_WorkCategory_LegalEntity");
        });

        modelBuilder.Entity<TblZone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Zone");

            entity.ToTable("Tbl_Zone");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.TblZones)
                .HasForeignKey(d => d.IdLegalEntity)
                .HasConstraintName("FK_Zone_LegalEntity");
        });

        modelBuilder.Entity<VwAccessCard>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_AccessCard");

            entity.Property(e => e.AccessCardTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AccessCardTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PropertyCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesContractCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwAccessCardCar>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_AccessCardCar");

            entity.Property(e => e.AccessCardCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CarStateForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CarStateName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Color)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Make)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Model)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParkingLevel)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PlateNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RegisteredIn)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RegistrationNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SpaceNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwAlertMessage>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_AlertMessage");

            entity.Property(e => e.Body).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignBody).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignScreen)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignSubject)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MethodForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MethodName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Screen)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Subject)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwAlertMessageTag>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_AlertMessageTag");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDescription)
                .HasMaxLength(1000)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwAlertMessageUser>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_AlertMessageUser");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwApplicationAlertMessage>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_ApplicationAlertMessage");

            entity.Property(e => e.AlertBody).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AlertForeignBody).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AlertForeignSubject)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AlertScreen)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AlertSubject)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ApplicationName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwApplicationAlertTag>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_ApplicationAlertTag");

            entity.Property(e => e.ApplicationName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TagDescription)
                .HasMaxLength(1000)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TagForeignDescription)
                .HasMaxLength(1000)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TagName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwApplicationApprovalType>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_ApplicationApprovalType");

            entity.Property(e => e.ApplicationName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ApprovalTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ApprovalTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwApplicationDynamicReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_ApplicationDynamicReport");

            entity.Property(e => e.ApplicationName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DynamicReportCategory)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DynamicReportForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DynamicReportName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwApplicationLookUp>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_ApplicationLookUp");

            entity.Property(e => e.ApplicationName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LookUpForeignName).HasMaxLength(510);
            entity.Property(e => e.LookUpName).HasMaxLength(510);
            entity.Property(e => e.LookUpTypeForeignName).HasMaxLength(1020);
            entity.Property(e => e.LookUpTypeName).HasMaxLength(1020);
        });

        modelBuilder.Entity<VwApplicationLookUpType>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_ApplicationLookUpType");

            entity.Property(e => e.ApplicationName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LookUpTypeForeignName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LookUpTypeName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwApplicationMenu>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_ApplicationMenu");

            entity.Property(e => e.ApplicationName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MenuDescription)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MenuForeignDescription)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MenuName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwApplicationParameter>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_ApplicationParameter");

            entity.Property(e => e.ApplicationName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParameterForeignName)
                .HasMaxLength(225)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParameterName)
                .HasMaxLength(225)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParameterValue)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwApplicationUserPermission>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_ApplicationUserPermission");

            entity.Property(e => e.ApplicationName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserPermissionDescription)
                .HasMaxLength(1000)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserPermissionForeignDescription)
                .HasMaxLength(1000)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserPermissionForeignScreenName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserPermissionName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserPermissionScreenName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwApprovalAlert>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_ApprovalAlert");

            entity.Property(e => e.AlertDate).HasColumnType("datetime");
            entity.Property(e => e.CreatorForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatorName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDescription).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignSubject)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Note).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusDate).HasColumnType("datetime");
            entity.Property(e => e.StatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Subject)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitCodeName)
                .HasMaxLength(503)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwApprovalStageUser>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_ApprovalStageUser");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwApprovalTemplate>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_ApprovalTemplate");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Query).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StageForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StageName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwApprovalTemplateTerm>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_ApprovalTemplateTerm");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.FromValue).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.RatioForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RatioName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TermForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TermName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToValue).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwApprovalTemplateUser>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_ApprovalTemplateUser");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwArea>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_Area");

            entity.Property(e => e.CityCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CityForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CityName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StateCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StateForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StateName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwAsset>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_Asset");

            entity.Property(e => e.AssetTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Barcode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CostCenter1ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter1Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter2ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter2Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter3ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter3Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter4ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter4Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter5ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter5Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.GroupCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.GroupForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.GroupName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MainGroupCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MainGroupForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MainGroupName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ManufacturerCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ManufacturerForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ManufacturerName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MeterTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MeterTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParentCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParentForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParentName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SerialNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ServiceContractExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.ServiceContractRemark).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SubGroup1Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SubGroup1ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SubGroup1Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SubGroup2Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SubGroup2ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SubGroup2Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Vender)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WarrantyExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.WarrantyRemark).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ZoneCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ZoneForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ZoneName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwAssetGroup>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_AssetGroup");

            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.GroupTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.GroupTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwAssetMaintenanceSchedule>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_AssetMaintenanceSchedule");

            entity.Property(e => e.AssetBarcode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetGroupForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("assetGroupForeignName");
            entity.Property(e => e.AssetGroupName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("assetGroupName");
            entity.Property(e => e.AssetName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetSerialNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ForeignStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FrequencyForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FrequencyName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LastMaintenanceDate).HasColumnType("datetime");
            entity.Property(e => e.LastMaintenanceReading).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LastMeterReading).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MTypeCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeCode");
            entity.Property(e => e.MTypeDescription)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeDescription");
            entity.Property(e => e.MTypeForeignDescription)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeForeignDescription");
            entity.Property(e => e.MTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeForeignName");
            entity.Property(e => e.MTypeIdWorkOrderCategory).HasColumnName("mTypeIdWorkOrderCategory");
            entity.Property(e => e.MTypeIsActive).HasColumnName("mTypeIsActive");
            entity.Property(e => e.MTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeName");
            entity.Property(e => e.MeterTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MeterTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.NextMaintenanceDate).HasColumnType("datetime");
            entity.Property(e => e.NextMeterReading).HasColumnType("decimal(19, 2)");
            entity.Property(e => e.ParentCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParentForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParentName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SchedulerLimit).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.StatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderCategoryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderCategoryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderDocumentNumber");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderForeignName");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderName");
            entity.Property(e => e.ZoneCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ZoneForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("zoneForeignName");
            entity.Property(e => e.ZoneName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("zoneName");
        });

        modelBuilder.Entity<VwAssetPurchaseRequest>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_AssetPurchaseRequest");

            entity.Property(e => e.ApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.StoreRemarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StoreStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StoreStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwAssetPurchaseRequestAsset>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_AssetPurchaseRequestAssets");

            entity.Property(e => e.AssetDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetPurchaseRequestCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ItemCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ManufacturerCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ManufacturerForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ManufacturerName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SerialNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SupplierForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitOfMeasure)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwAssetRepair>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_AssetRepair");

            entity.Property(e => e.ApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.StoreRemarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StoreStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StoreStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwAssetRepairAsset>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_AssetRepairAssets");

            entity.Property(e => e.AssetCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetRepairCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetRepairDescription).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.FromWarehouseCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FromWarehouseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FromWarehouseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PurchaseRequestCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToWarehouseCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToWarehouseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToWarehouseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwAssetTransfer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_AssetTransfer");

            entity.Property(e => e.ApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwAssetTransferAsset>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_AssetTransferAssets");

            entity.Property(e => e.AssetCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetTransferCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.FromZoneCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FromZoneForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FromZoneName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Remarks)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.StatusDate).HasColumnType("datetime");
            entity.Property(e => e.ToZoneCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToZoneForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToZoneName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("userStatusForeignName");
            entity.Property(e => e.UserStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("userStatusName");
        });

        modelBuilder.Entity<VwAttachment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_Attachments");

            entity.Property(e => e.AssetCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AttachmentsName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AttachmentsTypeForeign)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaintenanceTypeCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ObjectTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ObjectTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Path)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderDocumentNumber");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwAttachmentTypeObjectType>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_AttachmentType_ObjectType");

            entity.Property(e => e.AttachmentTypeForeignName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AttachmentTypeName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ObjectTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ObjectTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwBank>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_Bank");

            entity.Property(e => e.Branch)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DefaultAccountCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DefaultAccountForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DefaultAccountName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdPdcaccount).HasColumnName("IdPDCAccount");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PdcaccountCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("PDCAccountCode");
            entity.Property(e => e.PdcaccountForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("PDCAccountForeignName");
            entity.Property(e => e.PdcaccountName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("PDCAccountName");
            entity.Property(e => e.Remark).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwCancellationProcess>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_CancellationProcess");

            entity.Property(e => e.AdjustmentAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.AdjustmentTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AdjustmentTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CancellationDate).HasColumnType("datetime");
            entity.Property(e => e.CancellationTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CancellationTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ContractAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DocumentStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DocumentStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdPeriodBase).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.PaidAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.PenaltyPropertyProgress).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.PenaltyTotal).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.PeriodBaseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PeriodBaseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Reason).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ReleaseDate).HasColumnType("datetime");
            entity.Property(e => e.SalesContractAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.SalesContractCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesContractFromDate).HasColumnType("datetime");
            entity.Property(e => e.SalesContractToDate).HasColumnType("datetime");
            entity.Property(e => e.SalesContractTotalAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.SecurityDeposit).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UnitCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwCancellationProcessChecklist>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_CancellationProcessChecklist");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CancellationProcessCode)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CancellationTypeChecklistDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CancellationTypeChecklistForeignDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CategoryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CheckingDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwCancellationType>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_CancellationType");

            entity.Property(e => e.ContractTypeCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ContractTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ContractTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PeriodBaseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PeriodBaseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwCancellationTypeChecklist>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_CancellationTypeChecklist");

            entity.Property(e => e.CancellationTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CancellationTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CategoryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Remark).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Value).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ValueTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ValueTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwCertification>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_Certification");

            entity.Property(e => e.AssetCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TechnicianForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TechnicianName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwCity>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_City");

            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StateCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StateForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StateName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwClass>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_Class");

            entity.Property(e => e.Code)
                .HasMaxLength(25)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignParentName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParentName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwCommission>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_Commission");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CommissionStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CommissionStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(25)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.EmployeeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.EmployeeLastCommissionDate).HasColumnType("datetime");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.JobTitleForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.JobTitleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ManagerForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ManagerName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Percentage).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Remark).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesContractCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwContractType>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_ContractType");

            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.GlaccountCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountCode");
            entity.Property(e => e.GlaccountForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountForeignName");
            entity.Property(e => e.GlaccountName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountName");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdGlaccount).HasColumnName("IdGLAccount");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwCostCenter>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_CostCenter");

            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DimensionDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DimensionForeignDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.ValidFrom).HasColumnType("datetime");
            entity.Property(e => e.ValidTo).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwCustomer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_Customer");

            entity.Property(e => e.BlackListDate).HasColumnType("datetime");
            entity.Property(e => e.BlackListReason).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.BusinessPartnerType)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Contact)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ContractTypeCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ContractTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ContractTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Fax)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignBusinessPartnerType)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignGender)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignMaritalStatus)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignMotherName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Gender)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.GlaccountCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountCode");
            entity.Property(e => e.GlaccountForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountForeignName");
            entity.Property(e => e.GlaccountName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountName");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdGlaccount).HasColumnName("IdGLAccount");
            entity.Property(e => e.Idexpiry)
                .HasColumnType("datetime")
                .HasColumnName("IDExpiry");
            entity.Property(e => e.Idnumber)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("IDNumber");
            entity.Property(e => e.LanguageForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LanguageName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LicenseNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MotherName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PassportExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.PassportIssueDate).HasColumnType("datetime");
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Pobox)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("PObox");
            entity.Property(e => e.PrimaryEmail)
                .HasMaxLength(150)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PrimaryPhone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SecondaryEmail)
                .HasMaxLength(150)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SecondaryPhone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.VatNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Vatcertificate)
                .HasMaxLength(200)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("VATCertificate");
        });

        modelBuilder.Entity<VwCustomerDependent>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_CustomerDependent");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.DependentTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DependentTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.GenderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.GenderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idnumber)
                .HasMaxLength(150)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("IDNumber");
            entity.Property(e => e.MarriageCertificationNumber)
                .HasMaxLength(150)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(150)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwCustomerRequest>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_CustomerRequest");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaintenanceEndDate).HasColumnType("datetime");
            entity.Property(e => e.MaintenanceStartDate).HasColumnType("datetime");
            entity.Property(e => e.PropertyCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesContractCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwCustomerSponsor>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_CustomerSponsor");

            entity.Property(e => e.CompanyRegistrationNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ComputerCard)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Number)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwDashboard>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_Dashboard");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DashboardXml)
                .HasColumnType("xml")
                .HasColumnName("DashboardXML");
            entity.Property(e => e.ForeignDashboardXml)
                .HasColumnType("xml")
                .HasColumnName("ForeignDashboardXML");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignReportQuery).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignReportQueryXml)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("ForeignReportQueryXML");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ReportCategory)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ReportForeignCategory)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ReportForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ReportName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ReportQuery).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ReportQueryXml)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("ReportQueryXML");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwDocumentAttestationType>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_DocumentAttestationType");

            entity.Property(e => e.AttestationDate).HasColumnType("datetime");
            entity.Property(e => e.AttestationTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AttestationTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CheckDate).HasColumnType("datetime");
            entity.Property(e => e.CheckedByForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CheckedByName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ObjectTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ObjectTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwDocumentEmployee>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_DocumentEmployee");

            entity.Property(e => e.Code)
                .HasMaxLength(25)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.EmployeeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.JobTitleCode)
                .HasMaxLength(25)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.JobTitleForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.JobTitleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.JoiningDate).HasColumnType("datetime");
            entity.Property(e => e.ObjectTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ObjectTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PeriodBaseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PeriodBaseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ResignationDate).HasColumnType("datetime");
            entity.Property(e => e.TypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwDocumentParking>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_DocumentParking");

            entity.Property(e => e.AvailabilityForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AvailabilityName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Floor)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LevelNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ObjectTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ObjectTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParkingForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParkingName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SpaceNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwDocumentParticular>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_DocumentParticular");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.AmountPercent).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.DiscountPercent).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ObjectTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ObjectTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ParticularCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularDiscount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ParticularForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularRefundableAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ParticularTax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PeriodBaseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PeriodBaseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TaxPercent).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TransactionTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwDynamicDashboardDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_DynamicDashboardDetails");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DashboardName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DashboardXml)
                .HasColumnType("xml")
                .HasColumnName("DashboardXML");
            entity.Property(e => e.DynamicDashboardForeignName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DynamicDashboardName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDashboardXml)
                .HasColumnType("xml")
                .HasColumnName("ForeignDashboardXML");
            entity.Property(e => e.ForeignReportQuery).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignReportQueryXml)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("ForeignReportQueryXML");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ReportCategory)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ReportForeignCategory)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ReportForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ReportName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ReportQuery).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ReportQueryXml)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("ReportQueryXML");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwDynamicLayout>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_DynamicLayout");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDescription).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LayoutPath)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LayoutTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LayoutTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwEmployee>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_Employee");

            entity.Property(e => e.Code)
                .HasMaxLength(25)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CommissionPercentage).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.GlaccountForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountForeignName");
            entity.Property(e => e.GlaccountName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountName");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdGlaccount).HasColumnName("IdGLAccount");
            entity.Property(e => e.JobTitleCode)
                .HasMaxLength(25)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.JobTitleForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.JobTitleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.JoiningDate).HasColumnType("datetime");
            entity.Property(e => e.LastCommissionDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParentCode)
                .HasMaxLength(25)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParentForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParentName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PeriodBaseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PeriodBaseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ResignationDate).HasColumnType("datetime");
            entity.Property(e => e.TypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwGlaccount>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_GLAccount");

            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParentCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParentForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParentName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwGlaccountDetermination>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_GLAccountDetermination");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.GlaccountCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountCode");
            entity.Property(e => e.GlaccountForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountForeignName");
            entity.Property(e => e.GlaccountName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountName");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdGlaccount).HasColumnName("IdGLAccount");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwGlaccountDeterminationDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_GLAccountDeterminationDetail");

            entity.Property(e => e.CancelTypeChckDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CancelTypeChckForeignDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CancellationTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CancellationTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.GlaccountCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountCode");
            entity.Property(e => e.GlaccountForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountForeignName");
            entity.Property(e => e.GlaccountName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountName");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdGlaccount).HasColumnName("IdGLAccount");
            entity.Property(e => e.IdGlaccountDetermination).HasColumnName("IdGLAccountDetermination");
            entity.Property(e => e.ParticularCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StateCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StateForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StateName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserUsername)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwGoodsIssue>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_GoodsIssue");

            entity.Property(e => e.ClosingDate).HasColumnType("datetime");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DocumentDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IntegrationStatus).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IssuedDate).HasColumnType("datetime");
            entity.Property(e => e.RequiredDate).HasColumnType("datetime");
            entity.Property(e => e.StatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwGoodsIssueItem>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_GoodsIssue_Items");

            entity.Property(e => e.AssetCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.BinLocationCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ClosingDate).HasColumnType("datetime");
            entity.Property(e => e.CostCenter1ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter1Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter2ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter2Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter3ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter3Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter4ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter4Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter5ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter5Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenterCode1)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenterCode2)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenterCode3)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenterCode4)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenterCode5)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DocumentDate).HasColumnType("datetime");
            entity.Property(e => e.GoodsIssueCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.GoodsIssueCreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.InventoryUnitMeasureCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.InventoryUnitMeasureForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.InventoryUnitMeasureName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IssueDate).HasColumnType("datetime");
            entity.Property(e => e.ItemCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RequiredDate).HasColumnType("datetime");
            entity.Property(e => e.StatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WarehouseCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WarehouseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WarehouseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwGoodsIssueItemsSerialNumber>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_GoodsIssue_ItemsSerialNumber");

            entity.Property(e => e.AssetCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.GoodsIssueCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.GoodsIssueCreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ItemCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SerialNumberCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransferItemIssueDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwHseatmosphericMonitoring>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_HSEAtmosphericMonitoring");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.HsetypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSETypeForeignName");
            entity.Property(e => e.HsetypeIsActive).HasColumnName("HSETypeIsActive");
            entity.Property(e => e.HsetypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSETypeName");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdHsetype).HasColumnName("IdHSEType");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remark)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SafeLimitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SafeLimitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwHsecondition>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_HSEConditions");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.HsetypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSETypeForeignName");
            entity.Property(e => e.HsetypeIsActive).HasColumnName("HSETypeIsActive");
            entity.Property(e => e.HsetypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSETypeName");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdHsetype).HasColumnName("IdHSEType");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remark)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwHseprocedure>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_HSEProcedure");

            entity.Property(e => e.Area)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.HsetypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSETypeForeignName");
            entity.Property(e => e.HsetypeIsActive).HasColumnName("HSETypeIsActive");
            entity.Property(e => e.HsetypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSETypeName");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdHsetype).HasColumnName("IdHSEType");
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.WorkDescriptionForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkDescriptionName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkLocationForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkLocationName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwHsesafetyEquipment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_HSESafetyEquipment");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.HsetypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSETypeForeignName");
            entity.Property(e => e.HsetypeIsActive).HasColumnName("HSETypeIsActive");
            entity.Property(e => e.HsetypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSETypeName");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdHsetype).HasColumnName("IdHSEType");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remark)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwHseworkDescription>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_HSEWorkDescription");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.HsetypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSETypeForeignName");
            entity.Property(e => e.HsetypeIsActive).HasColumnName("HSETypeIsActive");
            entity.Property(e => e.HsetypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSETypeName");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdHsetype).HasColumnName("IdHSEType");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remark)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwIncomingPayment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_IncomingPayment");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PaidToForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PaidToName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PostingDate).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesContractCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesOrderCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 6)");
        });

        modelBuilder.Entity<VwIncomingPaymentAccount>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_IncomingPaymentAccount");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.GlaccountCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountCode");
            entity.Property(e => e.GlaccountForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountForeignName");
            entity.Property(e => e.GlaccountIsPostable).HasColumnName("GLAccountIsPostable");
            entity.Property(e => e.GlaccountName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountName");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdGlaccount).HasColumnName("IdGLAccount");
            entity.Property(e => e.IncomingPaymentAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.IncomingPaymentCode)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IncomingPaymentTaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.IncomingPaymentTotalAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwIncomingPaymentCash>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_IncomingPaymentCash");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.BillNumber)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CashStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CashStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.GlaccountCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountCode");
            entity.Property(e => e.GlaccountForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountForeignName");
            entity.Property(e => e.GlaccountIsPostable).HasColumnName("GLAccountIsPostable");
            entity.Property(e => e.GlaccountName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountName");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdGlaccount).HasColumnName("IdGLAccount");
            entity.Property(e => e.IncomingPaymentAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.IncomingPaymentCode)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IncomingPaymentTaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.IncomingPaymentTotalAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ReceiptDate).HasColumnType("datetime");
            entity.Property(e => e.SalesContractCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesOrderCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwIncomingPaymentCheque>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_IncomingPaymentCheque");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.BankCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.BankForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.BankName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Branch)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ChequeNumber)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ChequeStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ChequeStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.GlaccountCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountCode");
            entity.Property(e => e.GlaccountForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountForeignName");
            entity.Property(e => e.GlaccountIsPostable).HasColumnName("GLAccountIsPostable");
            entity.Property(e => e.GlaccountName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountName");
            entity.Property(e => e.HolderName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdGlaccount).HasColumnName("IdGLAccount");
            entity.Property(e => e.IncomingPaymentAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.IncomingPaymentCode)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IncomingPaymentTaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.IncomingPaymentTotalAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.PropertyCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remark).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesContractCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesOrderCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusFee).HasColumnType("decimal(19, 6)");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.UnitCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwIncomingPaymentInstallment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_IncomingPaymentInstallment");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CashAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CashBillNumber)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CashReceiptDate).HasColumnType("datetime");
            entity.Property(e => e.ChequeAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ChequeChequeNumber)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ChequeDueDate).HasColumnType("datetime");
            entity.Property(e => e.ChequeTransactionDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IncomingPaymentAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.IncomingPaymentCode)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IncomingPaymentTaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.IncomingPaymentTotalAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.InvoiceInstallmentAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.InvoiceInstallmentTaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.InvoiceInstallmentTotalAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.PaymentStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PaymentStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TransferAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TransferDate).HasColumnType("datetime");
            entity.Property(e => e.TransferNumber)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransferReceivedFrom)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwIncomingPaymentTransfer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_IncomingPaymentTransfer");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerReferenceNumber)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.GlaccountCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountCode");
            entity.Property(e => e.GlaccountForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountForeignName");
            entity.Property(e => e.GlaccountIsPostable).HasColumnName("GLAccountIsPostable");
            entity.Property(e => e.GlaccountName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountName");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdGlaccount).HasColumnName("IdGLAccount");
            entity.Property(e => e.IncomingPaymentAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.IncomingPaymentCode)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IncomingPaymentTaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.IncomingPaymentTotalAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ReceivedFrom)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.TransferDate).HasColumnType("datetime");
            entity.Property(e => e.TransferNumber)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwItem>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_Items");

            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.InventoryUnitMeasureCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.InventoryUnitMeasureForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.InventoryUnitMeasureName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemGroupCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemGroupForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemGroupName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PreferrableVendorCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PreferrableVendorForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PreferrableVendorName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.QuantityInStock).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Tax)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwItemPurchaseRequest>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_ItemPurchaseRequest");

            entity.Property(e => e.ApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DocumentDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IntegrationStatus)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RequiredDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ValidUntilDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwItemPurchaseRequestItem>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_ItemPurchaseRequestItems");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ItemCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemCost).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ItemForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemPurchaseRequestCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemTax)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SupplierForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Uomcode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("UOMCode");
            entity.Property(e => e.UomforeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("UOMForeignName");
            entity.Property(e => e.Uomname)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("UOMName");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwItemSerialNumber>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_ItemSerialNumber");

            entity.Property(e => e.BinLocationCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("binLocationCode");
            entity.Property(e => e.BinLocationIsDefault).HasColumnName("binLocationIsDefault");
            entity.Property(e => e.BinLocationQuantity).HasColumnName("binLocationQuantity");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ItemCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.WarehouseCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WarehouseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WarehouseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwJobTitle>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_JobTitle");

            entity.Property(e => e.Code)
                .HasMaxLength(25)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CommissionPercentage).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PeriodBaseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PeriodBaseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwJournalEntry>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_JournalEntry");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Jeamount)
                .HasColumnType("decimal(38, 6)")
                .HasColumnName("JEAmount");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwJournalEntryDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_JournalEntryDetails");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Credit).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Debit).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.FirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.GlaccountCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountCode");
            entity.Property(e => e.GlaccountForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountForeignName");
            entity.Property(e => e.GlaccountName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("GLAccountName");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdGlaccount).HasColumnName("IdGLAccount");
            entity.Property(e => e.JournalEntryDate).HasColumnType("datetime");
            entity.Property(e => e.JournalEntryDueDate).HasColumnType("datetime");
            entity.Property(e => e.JournalEntryTransactionCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwLegalEntity>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_LegalEntity");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ConnectionTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ConnectionTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DatabaseName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DatabasePassword)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DatabaseServer)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DatabaseUsername)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LiscenceServer)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Sapserver)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("SAPServer");
            entity.Property(e => e.SapuserPassword)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("SAPUserPassword");
            entity.Property(e => e.SapuserUsername)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("SAPUserUsername");
            entity.Property(e => e.Sldserver)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("SLDServer");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwLegalEntityUser>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_LegalEntityUser");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EntityForeignName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("entityForeignName");
            entity.Property(e => e.EntityIsActive).HasColumnName("entityIsActive");
            entity.Property(e => e.EntityName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("entityName");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserPhone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserUsername)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwLookup>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_Lookup");

            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LookUpTypeForeignName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LookUpTypeName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwMaintenanceType>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_MaintenanceType");

            entity.Property(e => e.ActualHours).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDescription)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.WorkOrderCategoryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderCategoryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwMaintenanceTypeItem>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_MaintenanceTypeItem");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.InventoryUnitMeasureCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.InventoryUnitMeasureForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.InventoryUnitMeasureName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemCost).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ItemForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemGroupCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemGroupForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemGroupName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.WorkOrderCategoryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderCategoryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwMaintenanceTypeTechnician>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_MaintenanceTypeTechnician");

            entity.Property(e => e.Hours).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.MaintenanceTypeCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserUsername)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwMaintenanceTypeTool>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_MaintenanceTypeTool");

            entity.Property(e => e.MaintenanceTypeCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToolCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToolForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToolName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderCategoryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderCategoryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwMaterialRequest>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_MaterialRequest");

            entity.Property(e => e.ApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ClosingDate).HasColumnType("datetime");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DocumentDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.RequiredDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwMaterialRequestItem>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_MaterialRequest_Items");

            entity.Property(e => e.AssetCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ClosingDate).HasColumnType("datetime");
            entity.Property(e => e.DocumentDate).HasColumnType("datetime");
            entity.Property(e => e.FromWarehouseCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FromWarehouseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FromWarehouseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IssueDate).HasColumnType("datetime");
            entity.Property(e => e.ItemCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaterialRequestCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaterialRequestCreatedDate).HasColumnType("datetime");
            entity.Property(e => e.RequiredDate).HasColumnType("datetime");
            entity.Property(e => e.StatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToWarehouseCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToWarehouseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToWarehouseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwMaterialTransfer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_MaterialTransfer");

            entity.Property(e => e.ApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ClosingDate).HasColumnType("datetime");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DocumentDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdSap).HasColumnName("IdSAP");
            entity.Property(e => e.IntegrationStatus).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IssuedDate).HasColumnType("datetime");
            entity.Property(e => e.ObjectTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ObjectTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RequestCode).HasMaxLength(250);
            entity.Property(e => e.RequesterForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RequesterName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RequiredDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwMaterialTransferItem>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_MaterialTransfer_Items");

            entity.Property(e => e.AssetCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ClosingDate).HasColumnType("datetime");
            entity.Property(e => e.DocumentDate).HasColumnType("datetime");
            entity.Property(e => e.FromBinLocationCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FromWarehouseCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FromWarehouseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FromWarehouseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.InventoryUnitMeasureCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.InventoryUnitMeasureForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.InventoryUnitMeasureName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.IssueDate).HasColumnType("datetime");
            entity.Property(e => e.ItemCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaterialTransferCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaterialTransferCreatedDate).HasColumnType("datetime");
            entity.Property(e => e.RequiredDate).HasColumnType("datetime");
            entity.Property(e => e.StatusDate).HasColumnType("datetime");
            entity.Property(e => e.StatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToBinLocationCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToWarehouseCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToWarehouseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToWarehouseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("userStatusForeignName");
            entity.Property(e => e.UserStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("userStatusName");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwMaterialTransferItemsSerialNumber>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_MaterialTransfer_ItemsSerialNumber");

            entity.Property(e => e.AssetCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaterialTransferCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaterialTransferCreatedDate).HasColumnType("datetime");
            entity.Property(e => e.SerialNumberCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransferItemIssueDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwMenu>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_Menu");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Cssclass)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("CSSClass");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDescription)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParentDescription)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParentForeignDescription)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParentName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("URL");
        });

        modelBuilder.Entity<VwMeterReading>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_MeterReading");

            entity.Property(e => e.DocumentNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PostingDate).HasColumnType("datetime");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserUsername)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwMeterReadingDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_MeterReadingDetail");

            entity.Property(e => e.AssetBarcode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetSerialNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedByForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedByName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaintenanceTypeCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MeterReadingDocumentNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MeterReadingPostingDate).HasColumnType("datetime");
            entity.Property(e => e.MeterTypeCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MeterTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MeterTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ReadingValue).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Remark)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwMobileUser>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_MobileUser");

            entity.Property(e => e.ApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LanguageForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LanguageName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Password)
                .HasMaxLength(800)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwObjectReference>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_ObjectReference");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ObjectTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ObjectTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.ZeroNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwParameter>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_Parameter");

            entity.Property(e => e.ForeignName)
                .HasMaxLength(225)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LookUpTypeForeignName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LookUpTypeName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LookupForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LookupName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(225)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Value)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwParking>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_Parking");

            entity.Property(e => e.AvailabilityForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AvailabilityName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Floor)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LevelNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SpaceNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwParticular>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_Particular");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AmountTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AmountTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PeriodBaseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PeriodBaseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RefundableAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Tax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwPaymentTermInstallment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_PaymentTermInstallment");

            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PaymentTermCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PaymentTermForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PaymentTermName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Percentage).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwPermissionUser>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_PermissionUser");

            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDescription)
                .HasMaxLength(1000)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignScreenName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ScreenName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwProperty>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_Property");

            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MainPropertyCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MainPropertyForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MainPropertyName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Number)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PositionForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PositionName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyClassCode)
                .HasMaxLength(25)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyClassForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyClassName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyGroupForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyGroupName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertySubGroupForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertySubGroupName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesRepresentativeCode)
                .HasMaxLength(25)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesRepresentativeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesRepresentativeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SquareFeet)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwPropertyAddress>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_PropertyAddress");

            entity.Property(e => e.AreaCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AreaForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AreaName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.BuiltupArea)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CityCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CityForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CityName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Details).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Latitude).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Phone)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PlotArea)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StateCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StateForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StateName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Street).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Zipcode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("ZIPCode");
        });

        modelBuilder.Entity<VwPropertyFacility>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_PropertyFacility");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.FacilityForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FacilityName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Note).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwPropertyGroup>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_PropertyGroup");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParentForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParentName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwPropertyNeighborhood>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_PropertyNeighborhood");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.NeighborhoodForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.NeighborhoodName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Note).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwPropertyProgress>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_PropertyProgress");

            entity.Property(e => e.CheckedDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Percentage).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PropertyCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwRemark>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_Remarks");

            entity.Property(e => e.AssetCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaintenanceTypeCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ObjectTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ObjectTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(800)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserPhone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserUsername)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwRequiredAttachmentType>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_RequiredAttachmentType");

            entity.Property(e => e.AttachmentTypeForeignName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AttachmentTypeName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TransactionTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwRequiredAttestationType>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_RequiredAttestationType");

            entity.Property(e => e.AttestationTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AttestationTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.StateCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StateForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StateName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwRevenueRecognitionMonthly>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_RevenueRecognitionMonthly");

            entity.Property(e => e.ContractCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ContractFromDate).HasColumnType("datetime");
            entity.Property(e => e.ContractToDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(200)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DailyRate).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.InvoiceCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.InvoiceTotalAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.MonthlyAmount).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.RemainingFromUnitRate).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.UnitRate).HasColumnType("decimal(18, 4)");
        });

        modelBuilder.Entity<VwSalesContract>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_SalesContract");

            entity.Property(e => e.ActualFromDate).HasColumnType("datetime");
            entity.Property(e => e.ActualToDate).HasColumnType("datetime");
            entity.Property(e => e.ApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CalculationBaseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CalculationBaseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerInfoRef).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.DiscountPercent).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.DocumentStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DocumentStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.HandOverDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.InvCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.InvId).HasColumnName("InvID");
            entity.Property(e => e.Ipcode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("IPCode");
            entity.Property(e => e.Ipid).HasColumnName("IPID");
            entity.Property(e => e.LegalStatus)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.NetAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ParticularAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ParticularDiscountAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ParticularTaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.PaymentCalculationBaseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PaymentCalculationBaseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PaymentTermCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PaymentTermForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PaymentTermName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.QuotationCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesOrderCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TaxPercent).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TransactionSourceForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionSourceName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitRateRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwSalesInvoice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_SalesInvoice");

            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ContractCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ContractTransactionTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ContractTransactionTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ContractUnitCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ContractUnitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ContractUnitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(268)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.OrderCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.OrderTransactionTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.OrderTransactionTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.OrderUnitCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.OrderUnitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.OrderUnitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 6)");
        });

        modelBuilder.Entity<VwSalesInvoiceInstallment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_SalesInvoiceInstallment");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.InstallmentPercentage).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.InstallmentTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.InstallmentTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.InvoiceStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.InvoiceStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PostingDate).HasColumnType("datetime");
            entity.Property(e => e.QuotationCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RemainingAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.RemainingTaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ReveueGeneratedDate).HasColumnType("datetime");
            entity.Property(e => e.SalesContractCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesInvoiceCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesInvoiceTotalAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.SalesOrderCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TotalRemainingAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwSalesOrder>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_SalesOrder");

            entity.Property(e => e.ActualFromDate).HasColumnType("datetime");
            entity.Property(e => e.ActualToDate).HasColumnType("datetime");
            entity.Property(e => e.ApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CalculationBaseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CalculationBaseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerInfoRef).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.DiscountPercent).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.DocumentStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DocumentStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.HandOverDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.NetAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ParticularAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ParticularDiscountAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ParticularTaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.PaymentCalculationBaseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PaymentCalculationBaseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PaymentTermCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PaymentTermForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PaymentTermName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.QuotationCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RenewalSalesContractCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TaxPercent).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TransactionSourceForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionSourceName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitRateRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwSalesQuotation>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_SalesQuotation");

            entity.Property(e => e.ActualFromDate).HasColumnType("datetime");
            entity.Property(e => e.ActualToDate).HasColumnType("datetime");
            entity.Property(e => e.ApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CalculationBaseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CalculationBaseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerInfoRef).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.DiscountPercent).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.DocumentStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DocumentStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.HandOverDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.NetAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ParticularAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ParticularDiscountAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ParticularTaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.PaymentCalculationBaseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PaymentCalculationBaseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PaymentTermCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PaymentTermForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PaymentTermName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TaxPercent).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TransactionSourceForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionSourceName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitRateRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.ValidTo).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwSecurityDepositReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_SecurityDepositReport");

            entity.Property(e => e.ContractAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ContractCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ContractCreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(501)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.InstallmentType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.InvoiceCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesOrderCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesQuotationCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SecurityDepositAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UnitCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwSparePartRepairRequest>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_SparePartRepairRequest");

            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.StatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StoreRemarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StoreStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StoreStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwSparePartRepairRequestDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_SparePartRepairRequestDetail");

            entity.Property(e => e.FromWarehouseCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FromWarehouseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FromWarehouseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Remark).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SerialNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SparePartNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SparePartRepairCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SparePartRepairCreatedDate).HasColumnType("datetime");
            entity.Property(e => e.SparePartRepairDescription).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToWarehouseCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToWarehouseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToWarehouseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwSparePartRepairTransfer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_SparePartRepairTransfer");

            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.SparePartRepairRequestCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwSparePartRepairTransferDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_SparePartRepairTransferDetail");

            entity.Property(e => e.FromWarehouseCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FromWarehouseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FromWarehouseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ItemCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remark).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SerialNumberCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SparePartNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SparePartRepairCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SparePartRepairCreatedDate).HasColumnType("datetime");
            entity.Property(e => e.StatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToWarehouseCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToWarehouseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToWarehouseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwSponsor>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_Sponsor");

            entity.Property(e => e.CompanyRegistrationNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ComputerCard)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryForeignNationality)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryNationality)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Number)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwState>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_State");

            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CountryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwSystemAlert>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_SystemAlert");

            entity.Property(e => e.Body).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ReadDate).HasColumnType("datetime");
            entity.Property(e => e.Subject).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("userName");
            entity.Property(e => e.UserUserName)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwTimeSheet>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_TimeSheet");

            entity.Property(e => e.AssetCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.EstimatedHours).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.Hours).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaintenanceTypeCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MaintenanceTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserUsername)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwToolTransfer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_ToolTransfer");

            entity.Property(e => e.ApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwToolTransferTool>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_ToolTransferTools");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.FromWarehouseCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FromWarehouseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FromWarehouseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Remarks)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusDate).HasColumnType("datetime");
            entity.Property(e => e.ToWarehouseCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToWarehouseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToWarehouseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToolCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToolForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToolName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToolStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToolStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToolTransferCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("userStatusForeignName");
            entity.Property(e => e.UserStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("userStatusName");
        });

        modelBuilder.Entity<VwUnit>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_Unit");

            entity.Property(e => e.AvailabilityForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AvailabilityName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CalculationBaseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CalculationBaseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Depth)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FlatNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Floor)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LastMergedDate).HasColumnType("datetime");
            entity.Property(e => e.MergeTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MergeTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Number)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParentForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParentName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesEmployeeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesEmployeeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SquareFeet)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitClassForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitClassName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitLink)
                .HasMaxLength(57)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitUseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitUseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitViewForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitViewName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Width)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwUnitClassParticular>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_UnitClassParticular");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AmountTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AmountTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");
            entity.Property(e => e.EffectiveTo).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ParticularForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularTransactionTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularTransactionTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PeriodBaseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PeriodBaseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RefundableAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Tax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitClassForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitClassName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwUnitCounter>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_UnitCounters");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Count)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CounterDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ExpenseTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ExpenseTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwUnitParking>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_UnitParking");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ParkingForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParkingName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitNumber)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwUnitParticular>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_UnitParticular");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AmountTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AmountTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");
            entity.Property(e => e.EffectiveTo).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ParticularCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularTransactionTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularTransactionTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParticularTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PeriodBaseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PeriodBaseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RefundableAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Tax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitClassForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitClassName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwUnitRate>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_UnitRate");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaxDiscountPercent).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaximumRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinimumRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PeriodBaseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PeriodBaseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Rate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Remark).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TaxPercentage).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwUnitSalesContract>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_UnitSalesContract");

            entity.Property(e => e.ActualFromDate).HasColumnType("datetime");
            entity.Property(e => e.ActualToDate).HasColumnType("datetime");
            entity.Property(e => e.ApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CalculationBaseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CalculationBaseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerInfoRef).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.DiscountPercent).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.DocumentStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DocumentStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.HandOverDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.InvCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.InvId).HasColumnName("InvID");
            entity.Property(e => e.Ipcode)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("IPCode");
            entity.Property(e => e.Ipid).HasColumnName("IPID");
            entity.Property(e => e.LegalStatus)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.NetAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ParticularAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ParticularDiscountAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ParticularTaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.PaymentCalculationBaseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PaymentCalculationBaseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PaymentTermCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PaymentTermForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PaymentTermName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.QuotationCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SalesOrderCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TaxPercent).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TransactionSourceForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionSourceName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TransactionTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitRateRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwUnitVisit>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_UnitVisit");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Note).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.ViewDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwUser>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_User");

            entity.Property(e => e.ApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(25)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.EmployeeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LanguageForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LanguageName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.LoginKey)
                .HasMaxLength(800)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Password)
                .HasMaxLength(800)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RatePerHour).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.TypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WarehouseCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WarehouseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WarehouseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwUserGroupMenu>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_UserGroupMenu");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDescription)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwUserGroupPermission>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_UserGroupPermission");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDescription)
                .HasMaxLength(1000)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignScreenName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ScreenName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwUserGroupUser>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_UserGroupUser");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwUserMenu>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_UserMenu");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Cssclass)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("CSSClass");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignDescription)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("URL");
            entity.Property(e => e.UserPermissionName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwWarehouseBinLocation>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WarehouseBinLocation");

            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.WarehouseCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WarehouseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WarehouseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwWarehouseItem>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WarehouseItem");

            entity.Property(e => e.BinLocationCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("binLocationCode");
            entity.Property(e => e.BinLocationIsDefault).HasColumnName("binLocationIsDefault");
            entity.Property(e => e.BinLocationQuantity).HasColumnName("binLocationQuantity");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ItemCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemCost).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ItemForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemGroupCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemGroupForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemGroupName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.WarehouseCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WarehouseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WarehouseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwWorkOrder>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WorkOrder");

            entity.Property(e => e.ApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ClosingDate).HasColumnType("datetime");
            entity.Property(e => e.CompleteDate).HasColumnType("datetime");
            entity.Property(e => e.CorrectiveAction)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedByForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedByName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DocumentDate).HasColumnType("datetime");
            entity.Property(e => e.DocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.EquipmentDetails)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Findings)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdWocategory).HasColumnName("IdWOCategory");
            entity.Property(e => e.InductionExplanation)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("Induction_Explanation");
            entity.Property(e => e.Inspection)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.JobTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.JobTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MachineDownTime).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MachineStartingTime).HasColumnType("datetime");
            entity.Property(e => e.MachineStoppingTime).HasColumnType("datetime");
            entity.Property(e => e.ManholeNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PriorityForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PriorityName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RequiredDate).HasColumnType("datetime");
            entity.Property(e => e.SafetyAssessmentForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SafetyAssessmentName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SourceForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SourceName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.WocategoryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("WOCategoryForeignName");
            entity.Property(e => e.WocategoryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("WOCategoryName");
            entity.Property(e => e.WorkOrderCategoryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderCategoryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderIdZone).HasColumnName("workOrderIdZone");
            entity.Property(e => e.ZoneCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ZoneForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ZoneName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwWorkOrderAsset>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WorkOrderAsset");

            entity.Property(e => e.AssetBarcode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetSerialNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedByForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedByName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.JobTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.JobTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderApprovalStatusForeignName");
            entity.Property(e => e.WorkOrderApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderApprovalStatusName");
            entity.Property(e => e.WorkOrderCategoryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderCategoryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderClosingDate)
                .HasColumnType("datetime")
                .HasColumnName("workOrderClosingDate");
            entity.Property(e => e.WorkOrderDocumentDate)
                .HasColumnType("datetime")
                .HasColumnName("workOrderDocumentDate");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderDocumentNumber");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderIdApprovalStatus).HasColumnName("workOrderIdApprovalStatus");
            entity.Property(e => e.WorkOrderIdJobType).HasColumnName("workOrderIdJobType");
            entity.Property(e => e.WorkOrderIdPriority).HasColumnName("workOrderIdPriority");
            entity.Property(e => e.WorkOrderIdSafetyAssessment).HasColumnName("workOrderIdSafetyAssessment");
            entity.Property(e => e.WorkOrderIdSource).HasColumnName("workOrderIdSource");
            entity.Property(e => e.WorkOrderIdWocategory).HasColumnName("workOrderIdWOCategory");
            entity.Property(e => e.WorkOrderIdWorkOrderCategory).HasColumnName("workOrderIdWorkOrderCategory");
            entity.Property(e => e.WorkOrderIdZone).HasColumnName("workOrderIdZone");
            entity.Property(e => e.WorkOrderIsActive).HasColumnName("workOrderIsActive");
            entity.Property(e => e.WorkOrderManholeNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderManholeNumber");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderReferenceNumber).HasColumnName("workOrderReferenceNumber");
            entity.Property(e => e.WorkOrderRequiredDate)
                .HasColumnType("datetime")
                .HasColumnName("workOrderRequiredDate");
            entity.Property(e => e.WorkOrderStartDate)
                .HasColumnType("datetime")
                .HasColumnName("workOrderStartDate");
        });

        modelBuilder.Entity<VwWorkOrderAttachment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WorkOrderAttachments");

            entity.Property(e => e.AttachmentCreatedDate).HasColumnType("datetime");
            entity.Property(e => e.AttachmentDescription)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AttachmentName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AttachmentPath)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AttachmentTypeForeign)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AttachmentTypeName)
                .HasMaxLength(255)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AttachmentUpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.ObjectTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ObjectTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderApprovalStatusForeignName");
            entity.Property(e => e.WorkOrderApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderApprovalStatusName");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderDocumentNumber");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderIdApprovalStatus).HasColumnName("workOrderIdApprovalStatus");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwWorkOrderCategory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WorkOrderCategory");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.WorkOrderTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwWorkOrderExpense>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WorkOrderExpense");

            entity.Property(e => e.AssetBarcode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetSerialNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter1ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter1Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter2ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter2Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter3ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter3Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter4ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter4Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter5ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter5Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.WorkOrderApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderApprovalStatusForeignName");
            entity.Property(e => e.WorkOrderApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderApprovalStatusName");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderDocumentNumber");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderIdApprovalStatus).HasColumnName("workOrderIdApprovalStatus");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwWorkOrderGoodsIssue>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WorkOrderGoodsIssue");

            entity.Property(e => e.ActualQuantity).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.AssetBarcode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetSerialNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter1ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter1Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter2ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter2Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter3ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter3Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter4ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter4Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter5ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter5Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ItemTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemsCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("itemsCode");
            entity.Property(e => e.ItemsCost)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("itemsCost");
            entity.Property(e => e.ItemsForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("itemsForeignName");
            entity.Property(e => e.ItemsIdItemsType).HasColumnName("itemsIdItemsType");
            entity.Property(e => e.ItemsIsActive).HasColumnName("itemsIsActive");
            entity.Property(e => e.ItemsName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("itemsName");
            entity.Property(e => e.MTypeCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeCode");
            entity.Property(e => e.MTypeDescription)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeDescription");
            entity.Property(e => e.MTypeForeignDescription)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeForeignDescription");
            entity.Property(e => e.MTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeForeignName");
            entity.Property(e => e.MTypeIsActive).HasColumnName("mTypeIsActive");
            entity.Property(e => e.MTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeName");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserWarehouseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserWarehouseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WarehouseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WarehouseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderApprovalStatusForeignName");
            entity.Property(e => e.WorkOrderApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderApprovalStatusName");
            entity.Property(e => e.WorkOrderCategoryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderCategoryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderDocumentNumber");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderIdApprovalStatus).HasColumnName("workOrderIdApprovalStatus");
            entity.Property(e => e.WorkOrderIdWorkOrderCategory).HasColumnName("workOrderIdWorkOrderCategory");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwWorkOrderHseatmosphericMonitoring>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WorkOrderHSEAtmosphericMonitoring");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.HseatmosphericMonitoringForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSEAtmosphericMonitoringForeignName");
            entity.Property(e => e.HseatmosphericMonitoringIsActive).HasColumnName("HSEAtmosphericMonitoringIsActive");
            entity.Property(e => e.HseatmosphericMonitoringName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSEAtmosphericMonitoringName");
            entity.Property(e => e.HseatmosphericMonitoringRemark)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSEAtmosphericMonitoringRemark");
            entity.Property(e => e.SafeLimitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SafeLimitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Time).HasColumnType("datetime");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwWorkOrderHsecondition>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WorkOrderHSEConditions");

            entity.Property(e => e.ChoiceForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ChoiceName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.HseconditionsForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSEConditionsForeignName");
            entity.Property(e => e.HseconditionsIsActive).HasColumnName("HSEConditionsIsActive");
            entity.Property(e => e.HseconditionsName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSEConditionsName");
            entity.Property(e => e.HseconditionsRemark)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSEConditionsRemark");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwWorkOrderHsedeepExcavationControl>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WorkOrderHSEDeepExcavationControls");

            entity.Property(e => e.ChoiceForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ChoiceName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ControlsForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ControlsName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwWorkOrderHseexcavationSafetyEquipment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WorkOrderHSEExcavationSafetyEquipment");

            entity.Property(e => e.ChoiceForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ChoiceName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.SafetyEquipmentForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.SafetyEquipmentName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwWorkOrderHseexcavationWork>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WorkOrderHSEExcavationWork");

            entity.Property(e => e.ChoiceForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ChoiceName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Details).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.HseexcavationWorkForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSEExcavationWorkForeignName");
            entity.Property(e => e.HseexcavationWorkIsActive).HasColumnName("HSEExcavationWorkIsActive");
            entity.Property(e => e.HseexcavationWorkName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSEExcavationWorkName");
            entity.Property(e => e.HseexcavationWorkRemark)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSEExcavationWorkRemark");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwWorkOrderHseprocedure>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WorkOrderHSEProcedure");

            entity.Property(e => e.Area)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ExcavationDescription).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.HsetypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSETypeForeignName");
            entity.Property(e => e.HsetypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSETypeName");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdHsetype).HasColumnName("IdHSEType");
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.WorkDescriptionForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkDescriptionName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkLocationForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkLocationName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwWorkOrderHsesafetyEquipment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WorkOrderHSESafetyEquipment");

            entity.Property(e => e.ChoiceForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ChoiceName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.HsesafetyEquipmentForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSESafetyEquipmentForeignName");
            entity.Property(e => e.HsesafetyEquipmentIsActive).HasColumnName("HSESafetyEquipmentIsActive");
            entity.Property(e => e.HsesafetyEquipmentName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSESafetyEquipmentName");
            entity.Property(e => e.HsesafetyEquipmentRemark)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("HSESafetyEquipmentRemark");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwWorkOrderLog>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WorkOrderLog");

            entity.Property(e => e.ClosingDate).HasColumnType("datetime");
            entity.Property(e => e.CompleteDate).HasColumnType("datetime");
            entity.Property(e => e.CorrectiveAction)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DocumentDate).HasColumnType("datetime");
            entity.Property(e => e.DocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.EquipmentDetails)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Findings)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.IdWocategory).HasColumnName("IdWOCategory");
            entity.Property(e => e.InductionExplanation)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("Induction_Explanation");
            entity.Property(e => e.Inspection)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.MachineDownTime).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MachineStartingTime).HasColumnType("datetime");
            entity.Property(e => e.MachineStoppingTime).HasColumnType("datetime");
            entity.Property(e => e.ManholeNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RequiredDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwWorkOrderMaintenanceRequest>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WorkOrderMaintenanceRequest");

            entity.Property(e => e.AssetCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignFirstName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerForeignMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerLastName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CustomerMiddleName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DeffectDate).HasColumnType("datetime");
            entity.Property(e => e.DepartmentForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MTypeCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeCode");
            entity.Property(e => e.MTypeDescription)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeDescription");
            entity.Property(e => e.MTypeForeignDescription)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeForeignDescription");
            entity.Property(e => e.MTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeForeignName");
            entity.Property(e => e.MTypeIdWorkOrderCategory).HasColumnName("mTypeIdWorkOrderCategory");
            entity.Property(e => e.MTypeIsActive).HasColumnName("mTypeIsActive");
            entity.Property(e => e.MTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeName");
            entity.Property(e => e.ParentCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParentForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ParentName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.PostingDate).HasColumnType("datetime");
            entity.Property(e => e.Remark).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RequestIdWorkOrder).HasColumnName("requestIdWorkOrder");
            entity.Property(e => e.RequesterStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.RequesterStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ShiftForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ShiftName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.StatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UnitName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserUsername)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderApprovalStatusForeignName");
            entity.Property(e => e.WorkOrderApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderApprovalStatusName");
            entity.Property(e => e.WorkOrderCategoryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderCategoryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderIdApprovalStatus).HasColumnName("workOrderIdApprovalStatus");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ZoneCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ZoneForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("zoneForeignName");
            entity.Property(e => e.ZoneName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("zoneName");
        });

        modelBuilder.Entity<VwWorkOrderMaintenanceType>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WorkOrderMaintenanceType");

            entity.Property(e => e.AssetBarcode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetSerialNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MTypeCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeCode");
            entity.Property(e => e.MTypeDescription)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeDescription");
            entity.Property(e => e.MTypeForeignDescription)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeForeignDescription");
            entity.Property(e => e.MTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeForeignName");
            entity.Property(e => e.MTypeIdWorkOrderCategory).HasColumnName("mTypeIdWorkOrderCategory");
            entity.Property(e => e.MTypeIsActive).HasColumnName("mTypeIsActive");
            entity.Property(e => e.MTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeName");
            entity.Property(e => e.Remarks).UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderApprovalStatusForeignName");
            entity.Property(e => e.WorkOrderApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderApprovalStatusName");
            entity.Property(e => e.WorkOrderCategoryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderCategoryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderDocumentNumber");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderIdApprovalStatus).HasColumnName("workOrderIdApprovalStatus");
            entity.Property(e => e.WorkOrderIdWorkOrderCategory).HasColumnName("workOrderIdWorkOrderCategory");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwWorkOrderRemark>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WorkOrderRemark");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ObjectTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ObjectTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(800)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserPhone)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserUsername)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderApprovalStatusForeignName");
            entity.Property(e => e.WorkOrderApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderApprovalStatusName");
            entity.Property(e => e.WorkOrderCategoryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderCategoryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderIdApprovalStatus).HasColumnName("workOrderIdApprovalStatus");
            entity.Property(e => e.WorkOrderIdWorkOrderCategory).HasColumnName("workOrderIdWorkOrderCategory");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwWorkOrderSparePart>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WorkOrderSparePart");

            entity.Property(e => e.ActualQuantity).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.AssetBarcode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetSerialNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CostCenter1ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter1Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter2ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter2Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter3ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter3Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter4ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter4Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter5ForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CostCenter5Name)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EstimatedQuantity).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ItemTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ItemsCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("itemsCode");
            entity.Property(e => e.ItemsCost)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("itemsCost");
            entity.Property(e => e.ItemsForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("itemsForeignName");
            entity.Property(e => e.ItemsIdItemsType).HasColumnName("itemsIdItemsType");
            entity.Property(e => e.ItemsIsActive).HasColumnName("itemsIsActive");
            entity.Property(e => e.ItemsName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("itemsName");
            entity.Property(e => e.MTypeCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeCode");
            entity.Property(e => e.MTypeDescription)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeDescription");
            entity.Property(e => e.MTypeForeignDescription)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeForeignDescription");
            entity.Property(e => e.MTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeForeignName");
            entity.Property(e => e.MTypeIsActive).HasColumnName("mTypeIsActive");
            entity.Property(e => e.MTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeName");
            entity.Property(e => e.QuantityToIssue).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserWarehouseCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserWarehouseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserWarehouseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WarehouseCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WarehouseForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WarehouseName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WarehouseQuantity).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.WorkOrderApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderApprovalStatusForeignName");
            entity.Property(e => e.WorkOrderApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderApprovalStatusName");
            entity.Property(e => e.WorkOrderCategoryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderCategoryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderDocumentNumber");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderIdApprovalStatus).HasColumnName("workOrderIdApprovalStatus");
            entity.Property(e => e.WorkOrderIdWorkOrderCategory).HasColumnName("workOrderIdWorkOrderCategory");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwWorkOrderTechnician>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WorkOrderTechnician");

            entity.Property(e => e.ActualHours).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.AssetBarcode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignDescription)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.AssetSerialNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.EstimatedHours).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MTypeCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeCode");
            entity.Property(e => e.MTypeDescription)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeDescription");
            entity.Property(e => e.MTypeForeignDescription)
                .HasMaxLength(500)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeForeignDescription");
            entity.Property(e => e.MTypeForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeForeignName");
            entity.Property(e => e.MTypeIdWorkOrderCategory).HasColumnName("mTypeIdWorkOrderCategory");
            entity.Property(e => e.MTypeIsActive).HasColumnName("mTypeIsActive");
            entity.Property(e => e.MTypeName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("mTypeName");
            entity.Property(e => e.UserForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.UserRatePerHour).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UserUsername)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderApprovalStatusForeignName");
            entity.Property(e => e.WorkOrderApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderApprovalStatusName");
            entity.Property(e => e.WorkOrderCategoryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderCategoryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderDocumentNumber");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderIdApprovalStatus).HasColumnName("workOrderIdApprovalStatus");
            entity.Property(e => e.WorkOrderIdWorkOrderCategory).HasColumnName("workOrderIdWorkOrderCategory");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
        });

        modelBuilder.Entity<VwWorkOrderTool>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_WorkOrderTool");

            entity.Property(e => e.ToolCode)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToolForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.ToolName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderApprovalStatusForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderApprovalStatusForeignName");
            entity.Property(e => e.WorkOrderApprovalStatusName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS")
                .HasColumnName("workOrderApprovalStatusName");
            entity.Property(e => e.WorkOrderCategoryForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderCategoryName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderDocumentNumber)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderForeignName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderIdWorkOrderCategory).HasColumnName("workOrderIdWorkOrderCategory");
            entity.Property(e => e.WorkOrderName)
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1256_CS_AS");
            entity.Property(e => e.WorkOrderReferenceNumber).HasColumnName("workOrderReferenceNumber");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
