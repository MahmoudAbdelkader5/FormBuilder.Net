using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblGlaccountDeterminationPriority
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public int Sort { get; set; }

    public string? Remarks { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }
}
