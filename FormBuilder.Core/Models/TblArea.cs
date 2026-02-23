using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblArea
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public int? IdCountry { get; set; }

    public int IdCity { get; set; }

    public int? IdState { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblCity IdCityNavigation { get; set; } = null!;

    public virtual TblCountry? IdCountryNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblState? IdStateNavigation { get; set; }

    public virtual ICollection<TblPropertyAddress> TblPropertyAddresses { get; set; } = new List<TblPropertyAddress>();
}
