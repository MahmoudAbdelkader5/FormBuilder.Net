using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblSponsor
{
    public int Id { get; set; }

    public string? Number { get; set; }

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public int IdNationality { get; set; }

    public string? PassportNumber { get; set; }

    public string? ComputerCard { get; set; }

    public string? CompanyRegistrationNumber { get; set; }

    public string? Phone { get; set; }

    public string? Mobile { get; set; }

    public string? Email { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblCountry IdNationalityNavigation { get; set; } = null!;

    public virtual ICollection<TblCustomerSponsor> TblCustomerSponsors { get; set; } = new List<TblCustomerSponsor>();
}
