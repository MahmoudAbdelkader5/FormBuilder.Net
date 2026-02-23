using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblAlertMessage
{
    public int Id { get; set; }

    public string? Subject { get; set; }

    public string? Body { get; set; }

    public string? ForeignSubject { get; set; }

    public string? ForeignBody { get; set; }

    public string? Screen { get; set; }

    public int IdAlertType { get; set; }

    public int IdAlertMethod { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? ForeignScreen { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblAlertMessageTag> TblAlertMessageTags { get; set; } = new List<TblAlertMessageTag>();

    public virtual ICollection<TblAlertMessageUser> TblAlertMessageUsers { get; set; } = new List<TblAlertMessageUser>();
}
