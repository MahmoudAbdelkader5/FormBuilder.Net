using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblCommission
{
    public int Id { get; set; }

    public int? IdSalesContract { get; set; }

    public int? IdEmployee { get; set; }

    public int? NumberOfInstallments { get; set; }

    public decimal? Percentage { get; set; }

    public decimal? Amount { get; set; }

    public int? IdCommissionStatus { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public string? Remark { get; set; }

    public virtual TblEmployee? IdEmployeeNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblSalesContract? IdSalesContractNavigation { get; set; }
}
