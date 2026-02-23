using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblDocumentParking
{
    public int IdObjectType { get; set; }

    public int IdDocument { get; set; }

    public int IdParking { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblParking IdParkingNavigation { get; set; } = null!;
}
