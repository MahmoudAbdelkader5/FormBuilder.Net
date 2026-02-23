using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblUnitParking
{
    public int Id { get; set; }

    public int IdParking { get; set; }

    public int IdUnit { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblParking IdParkingNavigation { get; set; } = null!;

    public virtual TblUnit IdUnitNavigation { get; set; } = null!;
}
