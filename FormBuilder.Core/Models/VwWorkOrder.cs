using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwWorkOrder
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public string? DocumentNumber { get; set; }

    public string? ManholeNumber { get; set; }

    public DateTime? DocumentDate { get; set; }

    public DateTime? RequiredDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime? ClosingDate { get; set; }

    public DateTime? StartDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public long ReferenceNumber { get; set; }

    public string? InductionExplanation { get; set; }

    public string? EquipmentDetails { get; set; }

    public string? Inspection { get; set; }

    public string? Findings { get; set; }

    public string? CorrectiveAction { get; set; }

    public string? Remarks { get; set; }

    public bool? IsCompleted { get; set; }

    public DateTime? MachineStartingTime { get; set; }

    public DateTime? MachineStoppingTime { get; set; }

    public decimal? MachineDownTime { get; set; }

    public DateTime? CompleteDate { get; set; }

    public int AboutToClose { get; set; }

    public int? IdSource { get; set; }

    public string? SourceName { get; set; }

    public string? SourceForeignName { get; set; }

    public int? IdWorkOrderCategory { get; set; }

    public string? WorkOrderCategoryName { get; set; }

    public string? WorkOrderCategoryForeignName { get; set; }

    public int? IdWocategory { get; set; }

    public string? WocategoryName { get; set; }

    public string? WocategoryForeignName { get; set; }

    public int? IdSafetyAssessment { get; set; }

    public string? SafetyAssessmentName { get; set; }

    public string? SafetyAssessmentForeignName { get; set; }

    public int? IdApprovalStatus { get; set; }

    public string? ApprovalStatusName { get; set; }

    public string? ApprovalStatusForeignName { get; set; }

    public int? IdPriority { get; set; }

    public string? PriorityName { get; set; }

    public string? PriorityForeignName { get; set; }

    public int? IdJobType { get; set; }

    public string? JobTypeName { get; set; }

    public string? JobTypeForeignName { get; set; }

    public int? IdCustomer { get; set; }

    public string? CustomerFirstName { get; set; }

    public string? CustomerForeignFirstName { get; set; }

    public string? CustomerLastName { get; set; }

    public string? CustomerForeignLastName { get; set; }

    public string? CustomerCode { get; set; }

    public string? CustomerMiddleName { get; set; }

    public string? CustomerForeignMiddleName { get; set; }

    public bool? CustomerIsBlackList { get; set; }

    public int WorkOrderIdZone { get; set; }

    public string? ZoneCode { get; set; }

    public string? ZoneName { get; set; }

    public string? ZoneForeignName { get; set; }

    public string? CreatedByName { get; set; }

    public string? CreatedByForeignName { get; set; }

    public int IdAsset { get; set; }

    public string? AssetCode { get; set; }

    public string? AssetName { get; set; }

    public string? AssetForeignName { get; set; }
}
