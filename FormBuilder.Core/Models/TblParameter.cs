using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblParameter
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public string? Value { get; set; }

    public int IdParameterType { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLookupType { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }
}
