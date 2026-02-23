using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblJobTitle
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public decimal? CommissionPercentage { get; set; }

    public int? NumberOfInstallments { get; set; }

    public int? IdPeriodBase { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }
}
