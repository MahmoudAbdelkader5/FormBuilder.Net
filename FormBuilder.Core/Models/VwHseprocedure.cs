using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwHseprocedure
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Area { get; set; }

    public int? IdLegalEntity { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? ReferenceNumber { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int IdCreatedBy { get; set; }

    public int? IdUpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public int IdHsetype { get; set; }

    public string HsetypeName { get; set; } = null!;

    public string? HsetypeForeignName { get; set; }

    public bool HsetypeIsActive { get; set; }

    public int IdWorkDescription { get; set; }

    public string WorkDescriptionName { get; set; } = null!;

    public string? WorkDescriptionForeignName { get; set; }

    public bool WorkDescriptionIsActive { get; set; }

    public int IdWorkLocation { get; set; }

    public string WorkLocationName { get; set; } = null!;

    public string? WorkLocationForeignName { get; set; }

    public bool WorkLocationIsActive { get; set; }
}
