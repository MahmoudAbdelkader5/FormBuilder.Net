using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwGlaccountDeterminationDetail
{
    public int Id { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int IdGlaccountDetermination { get; set; }

    public int? IdGlaccount { get; set; }

    public string? GlaccountName { get; set; }

    public string? GlaccountForeignName { get; set; }

    public string? GlaccountCode { get; set; }

    public int? IdTransactionType { get; set; }

    public string? TransactionTypeName { get; set; }

    public string? TransactionTypeForeignName { get; set; }

    public int? IdProperty { get; set; }

    public string? PropertyName { get; set; }

    public string? PropertyForeignName { get; set; }

    public string? PropertyCode { get; set; }

    public int? IdParticular { get; set; }

    public string? ParticularName { get; set; }

    public string? ParticularForeignName { get; set; }

    public string? ParticularCode { get; set; }

    public int? IdState { get; set; }

    public string? StateName { get; set; }

    public string? StateForeignName { get; set; }

    public string? StateCode { get; set; }

    public int? IdCancellationType { get; set; }

    public string? CancellationTypeName { get; set; }

    public string? CancellationTypeForeignName { get; set; }

    public int? IdCustomer { get; set; }

    public string? CustomerFirstName { get; set; }

    public string? CustomerForeignFirstName { get; set; }

    public string? CustomerLastName { get; set; }

    public string? CustomerForeignLastName { get; set; }

    public string? CustomerCode { get; set; }

    public string? CustomerMiddleName { get; set; }

    public string? CustomerForeignMiddleName { get; set; }

    public int? IdCancellationTypeChecklist { get; set; }

    public string? CancelTypeChckDescription { get; set; }

    public string? CancelTypeChckForeignDescription { get; set; }

    public int? IdUser { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public string? UserUsername { get; set; }
}
