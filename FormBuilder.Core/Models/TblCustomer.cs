using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblCustomer
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string FirstName { get; set; } = null!;

    public string? ForeignFirstName { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? MiddleName { get; set; }

    public string? ForeignMiddleName { get; set; }

    public string? LastName { get; set; }

    public string? ForeignLastName { get; set; }

    public int? IdBusinessPartnerType { get; set; }

    public int? IdGender { get; set; }

    public string? MotherName { get; set; }

    public string? ForeignMotherName { get; set; }

    public string? Idnumber { get; set; }

    public DateTime? Idexpiry { get; set; }

    public string? CardNumber { get; set; }

    public string? PassportNumber { get; set; }

    public DateTime? PassportIssueDate { get; set; }

    public DateTime? PassportExpiryDate { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public int? IdNationality { get; set; }

    public string? Mobile { get; set; }

    public string? PrimaryPhone { get; set; }

    public string? SecondaryPhone { get; set; }

    public string? Fax { get; set; }

    public string? PrimaryEmail { get; set; }

    public string? SecondaryEmail { get; set; }

    public int? IdMaritalStatus { get; set; }

    public string? Contact { get; set; }

    public byte[]? Photo { get; set; }

    public int? IdGlaccount { get; set; }

    public int? IdPreferableLanguage { get; set; }

    public string? CardCode { get; set; }

    public int? IdKnowledgeSource { get; set; }

    public bool? IsBlackList { get; set; }

    public DateTime? BlackListDate { get; set; }

    public string? BlackListReason { get; set; }

    public string? Pobox { get; set; }

    public int? IdContractType { get; set; }

    public string? Vatcertificate { get; set; }

    public string LicenseNo { get; set; } = null!;

    public DateOnly LicenseExpiry { get; set; }

    public string CustomerType { get; set; } = null!;

    public string? VatNumber { get; set; }

    public bool AllowConfidencialContractView { get; set; }

    public virtual TblContractType? IdContractTypeNavigation { get; set; }

    public virtual TblGlaccount? IdGlaccountNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblCountry? IdNationalityNavigation { get; set; }

    public virtual ICollection<TblAccessCard> TblAccessCards { get; set; } = new List<TblAccessCard>();

    public virtual ICollection<TblCustomerDependent> TblCustomerDependents { get; set; } = new List<TblCustomerDependent>();

    public virtual ICollection<TblCustomerRequest> TblCustomerRequests { get; set; } = new List<TblCustomerRequest>();

    public virtual ICollection<TblCustomerSponsor> TblCustomerSponsors { get; set; } = new List<TblCustomerSponsor>();

    public virtual ICollection<TblGlaccountDeterminationDetail> TblGlaccountDeterminationDetails { get; set; } = new List<TblGlaccountDeterminationDetail>();

    public virtual ICollection<TblJournalEntry> TblJournalEntries { get; set; } = new List<TblJournalEntry>();

    public virtual ICollection<TblProperty> TblProperties { get; set; } = new List<TblProperty>();

    public virtual ICollection<TblSalesContract> TblSalesContracts { get; set; } = new List<TblSalesContract>();

    public virtual ICollection<TblSalesOrder> TblSalesOrders { get; set; } = new List<TblSalesOrder>();

    public virtual ICollection<TblSalesQuotation> TblSalesQuotations { get; set; } = new List<TblSalesQuotation>();

    public virtual ICollection<TblUnitVisit> TblUnitVisits { get; set; } = new List<TblUnitVisit>();

    public virtual ICollection<TblWorkOrderMaintenanceRequest> TblWorkOrderMaintenanceRequests { get; set; } = new List<TblWorkOrderMaintenanceRequest>();

    public virtual ICollection<TblWorkOrder> TblWorkOrders { get; set; } = new List<TblWorkOrder>();
}
