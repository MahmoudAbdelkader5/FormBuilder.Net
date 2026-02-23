using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwUser
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? EmployeeLiscence { get; set; }

    public decimal? RatePerHour { get; set; }

    public string? LoginKey { get; set; }

    public int IdUserType { get; set; }

    public string TypeName { get; set; } = null!;

    public string? TypeForeignName { get; set; }

    public int? IdApprovalStatus { get; set; }

    public string? ApprovalStatusName { get; set; }

    public string? ApprovalStatusForeignName { get; set; }

    public int IdPreferableLanguage { get; set; }

    public string LanguageName { get; set; } = null!;

    public string? LanguageForeignName { get; set; }

    public int? IdWarehouse { get; set; }

    public string? WarehouseName { get; set; }

    public string? WarehouseForeignName { get; set; }

    public string? WarehouseCode { get; set; }

    public int? IdEmployee { get; set; }

    public string? EmployeeName { get; set; }

    public string? EmployeeForeignName { get; set; }

    public string? EmployeeCode { get; set; }
}
