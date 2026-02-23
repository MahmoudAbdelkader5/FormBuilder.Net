using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwCancellationProcess
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public DateTime CancellationDate { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public int NoticePeriodDays { get; set; }

    public string? Reason { get; set; }

    public decimal? SecurityDeposit { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public decimal? PenaltyTotal { get; set; }

    public decimal? PenaltyPropertyProgress { get; set; }

    public decimal? AdjustmentAmount { get; set; }

    public int? PenaltyNumberOfDays { get; set; }

    public decimal? PaidAmount { get; set; }

    public decimal? ContractAmount { get; set; }

    public int IdSalesContract { get; set; }

    public string SalesContractCode { get; set; } = null!;

    public decimal SalesContractAmount { get; set; }

    public decimal SalesContractTotalAmount { get; set; }

    public DateTime SalesContractFromDate { get; set; }

    public DateTime? SalesContractToDate { get; set; }

    public int SalesContractIdTransactionType { get; set; }

    public int ContractIdUnit { get; set; }

    public string? UnitName { get; set; }

    public string? UnitForeignName { get; set; }

    public string? UnitCode { get; set; }

    public int IdCustomer { get; set; }

    public string? CustomerFirstName { get; set; }

    public string? CustomerForeignFirstName { get; set; }

    public string? CustomerLastName { get; set; }

    public string? CustomerForeignLastName { get; set; }

    public string? CustomerCode { get; set; }

    public string? CustomerMiddleName { get; set; }

    public string? CustomerForeignMiddleName { get; set; }

    public bool? CustomerIsBlackList { get; set; }

    public int? UnitIdProperty { get; set; }

    public string? PropertyName { get; set; }

    public string? PropertyForeignName { get; set; }

    public string? PropertyCode { get; set; }

    public int IdCancellationType { get; set; }

    public string CancellationTypeName { get; set; } = null!;

    public string? CancellationTypeForeignName { get; set; }

    public int? InitialNoticePeriod { get; set; }

    public int? IdApprovalStatus { get; set; }

    public string ApprovalStatusName { get; set; } = null!;

    public string? ApprovalStatusForeignName { get; set; }

    public int IdDocumentStatus { get; set; }

    public string? DocumentStatusName { get; set; }

    public string? DocumentStatusForeignName { get; set; }

    public int? IdAdjustmentType { get; set; }

    public string? AdjustmentTypeName { get; set; }

    public string? AdjustmentTypeForeignName { get; set; }

    public decimal? IdPeriodBase { get; set; }

    public string? PeriodBaseName { get; set; }

    public string? PeriodBaseForeignName { get; set; }

    public int? ContractRemaimningDays { get; set; }
}
