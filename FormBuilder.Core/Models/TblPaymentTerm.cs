using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblPaymentTerm
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public int Months { get; set; }

    public int Days { get; set; }

    public bool IsTaxOnFirstInstallment { get; set; }

    public int NumberOfInstallments { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public DateTime? ActiveFromDate { get; set; }

    public DateTime? ActiveToDate { get; set; }

    public bool? IsApplyPercentage { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblPaymentTermInstallment> TblPaymentTermInstallments { get; set; } = new List<TblPaymentTermInstallment>();

    public virtual ICollection<TblSalesContract> TblSalesContracts { get; set; } = new List<TblSalesContract>();

    public virtual ICollection<TblSalesOrder> TblSalesOrders { get; set; } = new List<TblSalesOrder>();

    public virtual ICollection<TblSalesQuotation> TblSalesQuotations { get; set; } = new List<TblSalesQuotation>();
}
