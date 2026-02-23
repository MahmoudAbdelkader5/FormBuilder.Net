using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblSm
{
    public int Id { get; set; }

    public string RecipientNumber { get; set; } = null!;

    public string? Name { get; set; }

    public string? Body { get; set; }

    public bool IsSelected { get; set; }

    public bool IsSent { get; set; }

    public DateTime? SendDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? ResponseCode { get; set; }

    public string? ResponseMessage { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblSmsResponse> TblSmsResponses { get; set; } = new List<TblSmsResponse>();
}
