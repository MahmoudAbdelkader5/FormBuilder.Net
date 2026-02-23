using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwSponsor
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public string? Number { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? PassportNumber { get; set; }

    public string? ComputerCard { get; set; }

    public string? CompanyRegistrationNumber { get; set; }

    public string? Phone { get; set; }

    public string? Mobile { get; set; }

    public string? Email { get; set; }

    public int IdNationality { get; set; }

    public string? CountryCode { get; set; }

    public string? CountryName { get; set; }

    public string? CountryForeignName { get; set; }

    public string? CountryNationality { get; set; }

    public string? CountryForeignNationality { get; set; }
}
