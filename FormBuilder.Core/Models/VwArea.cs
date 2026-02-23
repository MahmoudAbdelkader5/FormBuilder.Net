using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwArea
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public string? Code { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdCountry { get; set; }

    public string? CountryCode { get; set; }

    public string? CountryName { get; set; }

    public string? CountryForeignName { get; set; }

    public int? IdState { get; set; }

    public string? StateCode { get; set; }

    public string? StateName { get; set; }

    public string? StateForeignName { get; set; }

    public int IdCity { get; set; }

    public string? CityCode { get; set; }

    public string? CityName { get; set; }

    public string? CityForeignName { get; set; }
}
