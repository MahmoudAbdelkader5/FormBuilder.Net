using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblIncomingPaymentTransfer
{
    public int Id { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? TransferDate { get; set; }

    public int? IdIncomingPayment { get; set; }

    public int? IdGlaccount { get; set; }

    public string? TransferNumber { get; set; }

    public string? CustomerReferenceNumber { get; set; }

    public string? ReceivedFrom { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public DateTime? TransactionDate { get; set; }

    public virtual TblGlaccount? IdGlaccountNavigation { get; set; }

    public virtual TblIncomingPayment? IdIncomingPaymentNavigation { get; set; }

    public virtual ICollection<TblIncomingPaymentInstallment> TblIncomingPaymentInstallments { get; set; } = new List<TblIncomingPaymentInstallment>();
}
