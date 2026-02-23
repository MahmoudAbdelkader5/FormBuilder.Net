using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwWorkOrderLog
{
    public int? ChangedColumns { get; set; }

    public DateTime ModifiedDate { get; set; }

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
}
