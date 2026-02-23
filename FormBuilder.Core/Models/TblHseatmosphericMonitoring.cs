using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblHseatmosphericMonitoring
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public string? Remark { get; set; }

    public int? IdHsetype { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int IdSafeLimit { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }
}
