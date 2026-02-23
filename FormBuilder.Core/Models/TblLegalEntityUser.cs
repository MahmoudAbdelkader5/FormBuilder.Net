using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblLegalEntityUser
{
    public int IdLegalEntity { get; set; }

    public int IdUser { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual TblLegalEntity IdLegalEntityNavigation { get; set; } = null!;

    public virtual TblUser IdUserNavigation { get; set; } = null!;
}
