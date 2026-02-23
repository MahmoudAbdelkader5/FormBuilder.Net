using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblSystemAlert
{
    public int Id { get; set; }

    public string? Subject { get; set; }

    public string? Body { get; set; }

    public bool IsRead { get; set; }

    public DateTime? ReadDate { get; set; }

    public int? IdUser { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IdAlertType { get; set; }

    public int? IdAlertObject { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblUser? IdUserNavigation { get; set; }
}
