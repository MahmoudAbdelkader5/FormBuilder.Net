using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblWorkOrderHseprocedure
{
    public int IdWorkOrder { get; set; }

    public int IdHsetype { get; set; }

    public int? IdWorkDescription { get; set; }

    public int? IdWorkLocation { get; set; }

    public string? Area { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int Id { get; set; }

    public string? ExcavationDescription { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }
}
