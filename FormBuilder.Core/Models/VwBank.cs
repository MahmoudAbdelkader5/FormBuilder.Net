using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwBank
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public string? Code { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public string? Branch { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsPostOffice { get; set; }

    public string? Remark { get; set; }

    public int? IdCountry { get; set; }

    public string? CountryCode { get; set; }

    public string? CountryName { get; set; }

    public string? CountryForeignName { get; set; }

    public int? IdDefaultAccount { get; set; }

    public string? DefaultAccountCode { get; set; }

    public string? DefaultAccountName { get; set; }

    public string? DefaultAccountForeignName { get; set; }

    public int? IdPdcaccount { get; set; }

    public string? PdcaccountCode { get; set; }

    public string? PdcaccountName { get; set; }

    public string? PdcaccountForeignName { get; set; }
}
