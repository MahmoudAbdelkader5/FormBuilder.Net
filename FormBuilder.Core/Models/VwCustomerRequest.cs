using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwCustomerRequest
{
    public int Id { get; set; }

    public DateTime? MaintenanceStartDate { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public DateTime? MaintenanceEndDate { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? MaxRequest { get; set; }

    public int? ReservedRequest { get; set; }

    public int? IdSalesContract { get; set; }

    public string SalesContractCode { get; set; } = null!;

    public int? IdUnit { get; set; }

    public string? UnitName { get; set; }

    public string? UnitForeignName { get; set; }

    public string? UnitCode { get; set; }

    public int? IdProperty { get; set; }

    public string? PropertyName { get; set; }

    public string? PropertyForeignName { get; set; }

    public string? PropertyCode { get; set; }

    public int? IdCustomer { get; set; }

    public string CustomerFirstName { get; set; } = null!;

    public string? CustomerForeignFirstName { get; set; }

    public string? CustomerLastName { get; set; }

    public string? CustomerForeignLastName { get; set; }

    public string? CustomerCode { get; set; }

    public string? CustomerMiddleName { get; set; }

    public string? CustomerForeignMiddleName { get; set; }
}
