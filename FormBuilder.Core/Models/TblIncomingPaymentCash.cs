using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblIncomingPaymentCash
{
    public int Id { get; set; }

    public decimal? Amount { get; set; }

    public string? BillNumber { get; set; }

    public int? IdIncomingPayment { get; set; }

    public DateTime? ReceiptDate { get; set; }

    public int? IdGlaccount { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public DateTime? TransactionDate { get; set; }

    public int? IdCashStatus { get; set; }

    public virtual TblGlaccount? IdGlaccountNavigation { get; set; }

    public virtual TblIncomingPayment? IdIncomingPaymentNavigation { get; set; }

    public virtual ICollection<TblIncomingPaymentInstallment> TblIncomingPaymentInstallments { get; set; } = new List<TblIncomingPaymentInstallment>();
}
