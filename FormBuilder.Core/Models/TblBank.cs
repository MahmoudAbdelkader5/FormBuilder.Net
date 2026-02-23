using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblBank
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public int? IdCountry { get; set; }

    public int? IdDefaultAccount { get; set; }

    public string? Remark { get; set; }

    public bool? IsPostOffice { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public string? Branch { get; set; }

    public int? IdPdcaccount { get; set; }

    public virtual TblCountry? IdCountryNavigation { get; set; }

    public virtual TblGlaccount? IdDefaultAccountNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblGlaccount? IdPdcaccountNavigation { get; set; }

    public virtual ICollection<TblIncomingPaymentCheque> TblIncomingPaymentCheques { get; set; } = new List<TblIncomingPaymentCheque>();
}
