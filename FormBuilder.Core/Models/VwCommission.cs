using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwCommission
{
    public int Id { get; set; }

    public int? NumberOfInstallments { get; set; }

    public decimal? Percentage { get; set; }

    public decimal? Amount { get; set; }

    public int? IdLegalEntity { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int IdCreatedBy { get; set; }

    public int? IdUpdatedBy { get; set; }

    public string? Remark { get; set; }

    public int? IdSalesContract { get; set; }

    public string SalesContractCode { get; set; } = null!;

    public int? IdEmployee { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public string? EmployeeForeignName { get; set; }

    public DateTime? EmployeeLastCommissionDate { get; set; }

    public string? Email { get; set; }

    public int? IdCommissionStatus { get; set; }

    public string CommissionStatusName { get; set; } = null!;

    public string? CommissionStatusForeignName { get; set; }

    public int? IdJobTitle { get; set; }

    public string? JobTitleName { get; set; }

    public string? JobTitleForeignName { get; set; }

    public int? IdParent { get; set; }

    public string? ManagerName { get; set; }

    public string? ManagerForeignName { get; set; }
}
