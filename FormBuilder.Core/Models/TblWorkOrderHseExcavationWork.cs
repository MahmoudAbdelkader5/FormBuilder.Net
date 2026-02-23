using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblWorkOrderHseExcavationWork
{
    public int IdWorkOrder { get; set; }

    public int IdExcavationWork { get; set; }

    public int? IdChoice { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Details { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }
}
