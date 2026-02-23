using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwCustomer
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? VatNumber { get; set; }

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

    public string LicenseNo { get; set; } = null!;

    public DateOnly LicenseExpiry { get; set; }

    public string CustomerType { get; set; } = null!;

    public string? PrimaryPhone { get; set; }

    public string? SecondaryPhone { get; set; }

    public string? Fax { get; set; }

    public string? PrimaryEmail { get; set; }

    public string? SecondaryEmail { get; set; }

    public int? IdMaritalStatus { get; set; }

    public string? Contact { get; set; }

    public byte[]? Photo { get; set; }

    public string? Pobox { get; set; }

    public string? CountryName { get; set; }

    public string? CountryForeignName { get; set; }

    public int? IdGlaccount { get; set; }

    public bool? IsBlackList { get; set; }

    public string? BlackListReason { get; set; }

    public DateTime? BlackListDate { get; set; }

    public string? GlaccountCode { get; set; }

    public string? GlaccountName { get; set; }

    public string? GlaccountForeignName { get; set; }

    public string? BusinessPartnerType { get; set; }

    public string? ForeignBusinessPartnerType { get; set; }

    public string? Gender { get; set; }

    public string? ForeignGender { get; set; }

    public string? MaritalStatus { get; set; }

    public string? ForeignMaritalStatus { get; set; }

    public int? IdKnowledgeSource { get; set; }

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public int? IdContractType { get; set; }

    public string? ContractTypeCode { get; set; }

    public string? ContractTypeName { get; set; }

    public string? ContractTypeForeignName { get; set; }

    public int? IdPreferableLanguage { get; set; }

    public string? LanguageName { get; set; }

    public string? LanguageForeignName { get; set; }

    public string? Vatcertificate { get; set; }

    public bool AllowConfidencialContractView { get; set; }
}
