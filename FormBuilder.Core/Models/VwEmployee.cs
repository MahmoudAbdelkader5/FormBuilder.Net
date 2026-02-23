using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwEmployee
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? ForeignName { get; set; }

    public DateTime? JoiningDate { get; set; }

    public DateTime? ResignationDate { get; set; }

    public bool? IsResigned { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public decimal? CommissionPercentage { get; set; }

    public int? NumberOfInstallments { get; set; }

    public DateTime? LastCommissionDate { get; set; }

    public int? IdGlaccount { get; set; }

    public string? GlaccountName { get; set; }

    public string? GlaccountForeignName { get; set; }

    public int? IdType { get; set; }

    public string? TypeName { get; set; }

    public string? TypeForeignName { get; set; }

    public int? IdPeriodBase { get; set; }

    public string? PeriodBaseName { get; set; }

    public string? PeriodBaseForeignName { get; set; }

    public int? IdJobTitle { get; set; }

    public string? JobTitleName { get; set; }

    public string? JobTitleForeignName { get; set; }

    public string? JobTitleCode { get; set; }

    public int? IdParent { get; set; }

    public string? ParentName { get; set; }

    public string? ParentForeignName { get; set; }

    public string? ParentCode { get; set; }
}
