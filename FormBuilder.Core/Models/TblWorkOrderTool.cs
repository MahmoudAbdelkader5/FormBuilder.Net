using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblWorkOrderTool
{
    public int IdWorkOrder { get; set; }

    public int IdTool { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblTool IdToolNavigation { get; set; } = null!;

    public virtual TblWorkOrder IdWorkOrderNavigation { get; set; } = null!;
}
