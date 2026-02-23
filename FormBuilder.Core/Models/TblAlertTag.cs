using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblAlertTag
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? ForeignDescription { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblAlertMessageTag> TblAlertMessageTags { get; set; } = new List<TblAlertMessageTag>();
}
