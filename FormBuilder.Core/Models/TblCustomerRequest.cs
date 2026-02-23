using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblCustomerRequest
{
    public int Id { get; set; }

    public int? IdCustomer { get; set; }

    public int? IdSalesContract { get; set; }

    public int? IdProperty { get; set; }

    public int? IdUnit { get; set; }

    public DateTime? MaintenanceStartDate { get; set; }

    public DateTime? MaintenanceEndDate { get; set; }

    public int? MaxRequest { get; set; }

    public int? ReservedRequest { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblCustomer? IdCustomerNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblProperty? IdPropertyNavigation { get; set; }

    public virtual TblSalesContract? IdSalesContractNavigation { get; set; }

    public virtual TblUnit? IdUnitNavigation { get; set; }
}
