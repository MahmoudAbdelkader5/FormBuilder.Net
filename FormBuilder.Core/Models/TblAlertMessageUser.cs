using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblAlertMessageUser
{
    public int IdAlertMessage { get; set; }

    public int IdUser { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual TblAlertMessage IdAlertMessageNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblUser IdUserNavigation { get; set; } = null!;
}
