using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblProperty
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Number { get; set; }

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public int IdStatus { get; set; }

    public int? IdSalesRepresentative { get; set; }

    public int? IdPropertyClass { get; set; }

    public int? IdLandLord { get; set; }

    public int? IdPosition { get; set; }

    public int? IdGroup { get; set; }

    public int? IdSubGroup { get; set; }

    public int? IdMainProperty { get; set; }

    public string? SquareFeet { get; set; }

    public int? TotalNumberOfParkings { get; set; }

    public int? TotalNumberOfUnits { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblPropertyGroup? IdGroupNavigation { get; set; }

    public virtual TblCustomer? IdLandLordNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblProperty? IdMainPropertyNavigation { get; set; }

    public virtual TblPosition? IdPositionNavigation { get; set; }

    public virtual TblClass? IdPropertyClassNavigation { get; set; }

    public virtual TblEmployee? IdSalesRepresentativeNavigation { get; set; }

    public virtual TblPropertyGroup? IdSubGroupNavigation { get; set; }

    public virtual ICollection<TblProperty> InverseIdMainPropertyNavigation { get; set; } = new List<TblProperty>();

    public virtual ICollection<TblAccessCard> TblAccessCards { get; set; } = new List<TblAccessCard>();

    public virtual ICollection<TblCustomerRequest> TblCustomerRequests { get; set; } = new List<TblCustomerRequest>();

    public virtual ICollection<TblGlaccountDeterminationDetail> TblGlaccountDeterminationDetails { get; set; } = new List<TblGlaccountDeterminationDetail>();

    public virtual ICollection<TblParking> TblParkings { get; set; } = new List<TblParking>();

    public virtual ICollection<TblPropertyAddress> TblPropertyAddresses { get; set; } = new List<TblPropertyAddress>();

    public virtual ICollection<TblPropertyFacility> TblPropertyFacilities { get; set; } = new List<TblPropertyFacility>();

    public virtual ICollection<TblPropertyNeighborhood> TblPropertyNeighborhoods { get; set; } = new List<TblPropertyNeighborhood>();

    public virtual ICollection<TblPropertyProgress> TblPropertyProgresses { get; set; } = new List<TblPropertyProgress>();

    public virtual ICollection<TblUnitVisit> TblUnitVisits { get; set; } = new List<TblUnitVisit>();

    public virtual ICollection<TblUnit> TblUnits { get; set; } = new List<TblUnit>();
}
