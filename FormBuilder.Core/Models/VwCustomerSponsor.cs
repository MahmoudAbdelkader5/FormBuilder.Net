using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwCustomerSponsor
{
    public int Id { get; set; }

    public int IdSponsor { get; set; }

    public int IdCustomer { get; set; }

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

    public int? IdLegalEntity { get; set; }
}
