using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblEmployee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? ForeignName { get; set; }

    public int? IdJobTitle { get; set; }

    public int? IdParent { get; set; }

    public DateTime? JoiningDate { get; set; }

    public DateTime? ResignationDate { get; set; }

    public bool? IsResigned { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string Code { get; set; } = null!;

    public int? IdPeriodBase { get; set; }

    public int? NumberOfInstallments { get; set; }

    public decimal? CommissionPercentage { get; set; }

    public int? IdGlaccount { get; set; }

    public int? IdType { get; set; }

    public DateTime? LastCommissionDate { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblCommission> TblCommissions { get; set; } = new List<TblCommission>();

    public virtual ICollection<TblDocumentEmployee> TblDocumentEmployees { get; set; } = new List<TblDocumentEmployee>();

    public virtual ICollection<TblProperty> TblProperties { get; set; } = new List<TblProperty>();

    public virtual ICollection<TblUnit> TblUnits { get; set; } = new List<TblUnit>();

    public virtual ICollection<TblUser> TblUsers { get; set; } = new List<TblUser>();
}
