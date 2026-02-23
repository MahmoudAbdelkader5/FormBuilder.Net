using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwPropertyAddress
{
    public int Id { get; set; }

    public string? BuiltupArea { get; set; }

    public string? PlotArea { get; set; }

    public string? Details { get; set; }

    public string? Zipcode { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Phone { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string? Street { get; set; }

    public int IdProperty { get; set; }

    public string? PropertyName { get; set; }

    public string? PropertyForeignName { get; set; }

    public string? PropertyCode { get; set; }

    public string? PropertyNumber { get; set; }

    public int IdCountry { get; set; }

    public string? CountryName { get; set; }

    public string? CountryForeignName { get; set; }

    public string? CountryCode { get; set; }

    public int? IdState { get; set; }

    public string? StateName { get; set; }

    public string? StateForeignName { get; set; }

    public string? StateCode { get; set; }

    public int IdCity { get; set; }

    public string? CityName { get; set; }

    public string? CityForeignName { get; set; }

    public string? CityCode { get; set; }

    public int IdArea { get; set; }

    public string? AreaName { get; set; }

    public string? AreaForeignName { get; set; }

    public string? AreaCode { get; set; }
}
