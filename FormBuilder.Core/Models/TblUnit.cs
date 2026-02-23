using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblUnit
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public string Number { get; set; } = null!;

    public int? IdUnitStatus { get; set; }

    public int? IdAvailability { get; set; }

    public string? FlatNumber { get; set; }

    public string? Floor { get; set; }

    public int? IdSalesEmployee { get; set; }

    public int? IdUnitView { get; set; }

    public int? IdUnitUse { get; set; }

    public int? NumberOfBedrooms { get; set; }

    public int? NumberOfBathrooms { get; set; }

    public int? NumberOfParkings { get; set; }

    public string? SquareFeet { get; set; }

    public string? Width { get; set; }

    public string? Depth { get; set; }

    public int? IdUnitClass { get; set; }

    public int? IdProperty { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public decimal? DefaultRentValue { get; set; }

    public decimal? DefaultSalesValue { get; set; }

    public bool IsAllowSale { get; set; }

    public bool IsAllowRent { get; set; }

    public bool IsAllowLease { get; set; }

    public int? IdSalesCalculationBase { get; set; }

    public int? IdMergeType { get; set; }

    public int? IdParent { get; set; }

    public int? IdLastMergedBy { get; set; }

    public DateTime? LastMergedDate { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblProperty? IdPropertyNavigation { get; set; }

    public virtual TblEmployee? IdSalesEmployeeNavigation { get; set; }

    public virtual TblClass? IdUnitClassNavigation { get; set; }

    public virtual TblUnitStatus? IdUnitStatusNavigation { get; set; }

    public virtual TblUnitUse? IdUnitUseNavigation { get; set; }

    public virtual TblUnitView? IdUnitViewNavigation { get; set; }

    public virtual ICollection<TblAccessCard> TblAccessCards { get; set; } = new List<TblAccessCard>();

    public virtual ICollection<TblCustomerRequest> TblCustomerRequests { get; set; } = new List<TblCustomerRequest>();

    public virtual ICollection<TblJournalEntry> TblJournalEntries { get; set; } = new List<TblJournalEntry>();

    public virtual ICollection<TblSalesContract> TblSalesContracts { get; set; } = new List<TblSalesContract>();

    public virtual ICollection<TblSalesOrder> TblSalesOrders { get; set; } = new List<TblSalesOrder>();

    public virtual ICollection<TblSalesQuotation> TblSalesQuotations { get; set; } = new List<TblSalesQuotation>();

    public virtual ICollection<TblUnitCounter> TblUnitCounters { get; set; } = new List<TblUnitCounter>();

    public virtual ICollection<TblUnitParking> TblUnitParkings { get; set; } = new List<TblUnitParking>();

    public virtual ICollection<TblUnitParticular> TblUnitParticulars { get; set; } = new List<TblUnitParticular>();

    public virtual ICollection<TblUnitVisit> TblUnitVisits { get; set; } = new List<TblUnitVisit>();
}
