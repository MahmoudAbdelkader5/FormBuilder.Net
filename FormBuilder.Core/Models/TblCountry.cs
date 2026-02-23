using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblCountry
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public string? Nationality { get; set; }

    public string? ForeignNationality { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblArea> TblAreas { get; set; } = new List<TblArea>();

    public virtual ICollection<TblBank> TblBanks { get; set; } = new List<TblBank>();

    public virtual ICollection<TblCity> TblCities { get; set; } = new List<TblCity>();

    public virtual ICollection<TblCustomer> TblCustomers { get; set; } = new List<TblCustomer>();

    public virtual ICollection<TblPropertyAddress> TblPropertyAddresses { get; set; } = new List<TblPropertyAddress>();

    public virtual ICollection<TblSponsor> TblSponsors { get; set; } = new List<TblSponsor>();

    public virtual ICollection<TblState> TblStates { get; set; } = new List<TblState>();
}
