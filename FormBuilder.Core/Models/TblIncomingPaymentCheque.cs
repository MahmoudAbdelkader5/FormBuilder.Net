using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblIncomingPaymentCheque
{
    public int Id { get; set; }

    public decimal? Amount { get; set; }

    public string? ChequeNumber { get; set; }

    public DateTime? DueDate { get; set; }

    public int? IdIncomingPayment { get; set; }

    public int IdBank { get; set; }

    public string? HolderName { get; set; }

    public int IdGlaccount { get; set; }

    public decimal? StatusFee { get; set; }

    public DateTime? TransactionDate { get; set; }

    public int IdChequeStatus { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Remark { get; set; }

    public virtual TblBank IdBankNavigation { get; set; } = null!;

    public virtual TblGlaccount IdGlaccountNavigation { get; set; } = null!;

    public virtual TblIncomingPayment? IdIncomingPaymentNavigation { get; set; }

    public virtual ICollection<TblIncomingPaymentInstallment> TblIncomingPaymentInstallments { get; set; } = new List<TblIncomingPaymentInstallment>();
}
