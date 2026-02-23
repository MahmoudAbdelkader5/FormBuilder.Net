using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwCancellationType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? NoticePeriod { get; set; }

    public int? MaxDaysAllowed { get; set; }

    public int? IdPeriodBase { get; set; }

    public int? IdTransactionType { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsReturnSecurityDeposit { get; set; }

    public string? TransactionTypeName { get; set; }

    public string? TransactionTypeForeignName { get; set; }

    public string? PeriodBaseName { get; set; }

    public string? PeriodBaseForeignName { get; set; }

    public int? IdContractType { get; set; }

    public string? ContractTypeCode { get; set; }

    public string? ContractTypeName { get; set; }

    public string? ContractTypeForeignName { get; set; }
}
