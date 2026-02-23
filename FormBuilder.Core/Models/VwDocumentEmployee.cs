using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwDocumentEmployee
{
    public int? IdLegalEntity { get; set; }

    public DateTime CreatedDate { get; set; }

    public int IdDocument { get; set; }

    public int IdCreatedBy { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public int IdEmployee { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string? EmployeeForeignName { get; set; }

    public string Code { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateTime? JoiningDate { get; set; }

    public DateTime? ResignationDate { get; set; }

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

    public int IdObjectType { get; set; }

    public string ObjectTypeName { get; set; } = null!;

    public string? ObjectTypeForeignName { get; set; }
}
