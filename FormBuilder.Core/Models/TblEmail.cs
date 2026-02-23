using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblEmail
{
    public int Id { get; set; }

    public string? SenderEmail { get; set; }

    public string? RecipientEmail { get; set; }

    public string? Ccemail { get; set; }

    public string? Name { get; set; }

    public string? Subject { get; set; }

    public string? Body { get; set; }

    public bool IsSelected { get; set; }

    public bool IsSent { get; set; }

    public DateTime? SendDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? ResponseCode { get; set; }

    public string? ResponseMessage { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }
}
