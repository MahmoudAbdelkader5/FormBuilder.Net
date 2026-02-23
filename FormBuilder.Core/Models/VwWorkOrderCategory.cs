using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwWorkOrderCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public string? ForeignName { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdWorkOrderType { get; set; }

    public string? WorkOrderTypeName { get; set; }

    public string? WorkOrderTypeForeignName { get; set; }
}
