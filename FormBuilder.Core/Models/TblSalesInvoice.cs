using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblSalesInvoice
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public int? IdSalesContract { get; set; }

    public int? IdSalesOrder { get; set; }

    public decimal TotalAmount { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblSalesContract? IdSalesContractNavigation { get; set; }

    public virtual TblSalesOrder? IdSalesOrderNavigation { get; set; }

    public virtual ICollection<TblSalesInvoiceInstallment> TblSalesInvoiceInstallments { get; set; } = new List<TblSalesInvoiceInstallment>();
}
