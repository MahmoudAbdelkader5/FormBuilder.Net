using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwMaintenanceType
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public int? IdLegalEntity { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int IdCreatedBy { get; set; }

    public int? IdUpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public string? Description { get; set; }

    public string? ForeignDescription { get; set; }

    public decimal? ActualHours { get; set; }

    public bool? IsRequireShutDown { get; set; }

    public bool? IsBreakDown { get; set; }

    public int? IdWorkOrderCategory { get; set; }

    public string WorkOrderCategoryName { get; set; } = null!;

    public string? WorkOrderCategoryForeignName { get; set; }

    public int? IdWorkOrderType { get; set; }

    public string? WorkOrderTypeName { get; set; }

    public string? WorkOrderTypeForeignName { get; set; }

    public int? IdWorkType { get; set; }

    public string WorkTypeName { get; set; } = null!;

    public string? WorkTypeForeignName { get; set; }
}
