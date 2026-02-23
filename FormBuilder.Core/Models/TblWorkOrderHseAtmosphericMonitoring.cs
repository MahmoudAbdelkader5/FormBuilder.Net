using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblWorkOrderHseAtmosphericMonitoring
{
    public int IdWorkOrder { get; set; }

    public int IdAtmosphericMonitoring { get; set; }

    public int? ActualReading { get; set; }

    public DateTime? Time { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }
}
