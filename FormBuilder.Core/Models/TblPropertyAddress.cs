using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblPropertyAddress
{
    public int Id { get; set; }

    public int IdProperty { get; set; }

    public int IdCountry { get; set; }

    public int? IdState { get; set; }

    public int IdCity { get; set; }

    public int IdArea { get; set; }

    public string? Details { get; set; }

    public string? BuiltupArea { get; set; }

    public string? PlotArea { get; set; }

    public string? Zipcode { get; set; }

    public string? Phone { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public string? Street { get; set; }

    public virtual TblArea IdAreaNavigation { get; set; } = null!;

    public virtual TblCity IdCityNavigation { get; set; } = null!;

    public virtual TblCountry IdCountryNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblProperty IdPropertyNavigation { get; set; } = null!;

    public virtual TblState? IdStateNavigation { get; set; }
}
