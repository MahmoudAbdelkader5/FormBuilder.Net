using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwWorkOrderHseprocedure
{
    public int Id { get; set; }

    public string? Area { get; set; }

    public int? IdLegalEntity { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? ExcavationDescription { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int IdCreatedBy { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public int IdHsetype { get; set; }

    public string? HsetypeName { get; set; }

    public string? HsetypeForeignName { get; set; }

    public int? IdWorkDescription { get; set; }

    public string? WorkDescriptionName { get; set; }

    public string? WorkDescriptionForeignName { get; set; }

    public int? IdWorkLocation { get; set; }

    public string? WorkLocationName { get; set; }

    public string? WorkLocationForeignName { get; set; }

    public int IdWorkOrder { get; set; }

    public string WorkOrderName { get; set; } = null!;

    public string? WorkOrderForeignName { get; set; }

    public string? WorkOrderDocumentNumber { get; set; }
}
