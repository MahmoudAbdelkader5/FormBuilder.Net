using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblAlertMessageTag
{
    public int IdAlertMessage { get; set; }

    public int IdAlertTag { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual TblAlertMessage IdAlertMessageNavigation { get; set; } = null!;

    public virtual TblAlertTag IdAlertTagNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }
}
