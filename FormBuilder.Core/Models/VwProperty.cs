using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwProperty
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public string? Number { get; set; }

    public string? Code { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? SquareFeet { get; set; }

    public int? TotalNumberOfParkings { get; set; }

    public int? TotalNumberOfUnits { get; set; }

    public int IdStatus { get; set; }

    public string? PropertyStatusName { get; set; }

    public string? PropertyStatusForeignName { get; set; }

    public int? IdSalesRepresentative { get; set; }

    public string? SalesRepresentativeName { get; set; }

    public string? SalesRepresentativeForeignName { get; set; }

    public string? SalesRepresentativeCode { get; set; }

    public int? IdPropertyClass { get; set; }

    public string? PropertyClassName { get; set; }

    public string? PropertyClassForeignName { get; set; }

    public string? PropertyClassCode { get; set; }

    public int? IdGroup { get; set; }

    public string? PropertyGroupName { get; set; }

    public string? PropertyGroupForeignName { get; set; }

    public int? IdSubGroup { get; set; }

    public string? PropertySubGroupName { get; set; }

    public string? PropertySubGroupForeignName { get; set; }

    public int? IdPosition { get; set; }

    public string? PositionName { get; set; }

    public string? PositionForeignName { get; set; }

    public int? IdLandLord { get; set; }

    public string? CustomerFirstName { get; set; }

    public string? CustomerForeignFirstName { get; set; }

    public string? CustomerLastName { get; set; }

    public string? CustomerForeignLastName { get; set; }

    public string? CustomerCode { get; set; }

    public string? CustomerMiddleName { get; set; }

    public string? CustomerForeignMiddleName { get; set; }

    public int? IdMainProperty { get; set; }

    public string? MainPropertyName { get; set; }

    public string? MainPropertyForeignName { get; set; }

    public string? MainPropertyCode { get; set; }
}
