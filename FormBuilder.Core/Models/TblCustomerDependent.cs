using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblCustomerDependent
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int IdDependentType { get; set; }

    public int? IdGender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Idnumber { get; set; }

    public string? PassportNumber { get; set; }

    public string? MarriageCertificationNumber { get; set; }

    public int IdCustomer { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual TblCustomer IdCustomerNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }
}
