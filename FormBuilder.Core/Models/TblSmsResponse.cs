using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblSmsResponse
{
    public int Id { get; set; }

    public int IdSms { get; set; }

    public string RecipientNumber { get; set; } = null!;

    public bool IsSent { get; set; }

    public DateTime? SentDate { get; set; }

    public string? ResponseCode { get; set; }

    public string? ResponseMessage { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblSm IdSmsNavigation { get; set; } = null!;
}
