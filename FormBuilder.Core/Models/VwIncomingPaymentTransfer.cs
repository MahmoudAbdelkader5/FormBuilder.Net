using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwIncomingPaymentTransfer
{
    public int Id { get; set; }

    public decimal? Amount { get; set; }

    public string? TransferNumber { get; set; }

    public DateTime? TransferDate { get; set; }

    public string? CustomerReferenceNumber { get; set; }

    public string? ReceivedFrom { get; set; }

    public DateTime? TransactionDate { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdIncomingPayment { get; set; }

    public decimal IncomingPaymentAmount { get; set; }

    public decimal? IncomingPaymentTaxAmount { get; set; }

    public decimal? IncomingPaymentTotalAmount { get; set; }

    public string IncomingPaymentCode { get; set; } = null!;

    public int? IdLegalEntity { get; set; }

    public int? IdGlaccount { get; set; }

    public string? GlaccountCode { get; set; }

    public string? GlaccountName { get; set; }

    public string? GlaccountForeignName { get; set; }

    public bool? GlaccountIsPostable { get; set; }
}
