using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblAccessCard
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public int? IdContract { get; set; }

    public int? IdAccessCardType { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IdCustomer { get; set; }

    public string? Remarks { get; set; }

    public int? IdProperty { get; set; }

    public int? IdUnit { get; set; }

    public virtual TblSalesContract? IdContractNavigation { get; set; }

    public virtual TblCustomer? IdCustomerNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblProperty? IdPropertyNavigation { get; set; }

    public virtual TblUnit? IdUnitNavigation { get; set; }

    public virtual ICollection<TblAccessCardCar> TblAccessCardCars { get; set; } = new List<TblAccessCardCar>();
}
