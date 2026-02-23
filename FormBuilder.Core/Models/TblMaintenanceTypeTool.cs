using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblMaintenanceTypeTool
{
    public int IdMaintenanceType { get; set; }

    public int IdTool { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblMaintenanceType IdMaintenanceTypeNavigation { get; set; } = null!;

    public virtual TblTool IdToolNavigation { get; set; } = null!;
}
