using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblPortalResource
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Value { get; set; } = null!;

    public string ForeignValue { get; set; } = null!;

    public string? Comment { get; set; }

    public string? ForeignComment { get; set; }

    public bool IsActive { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }
}
