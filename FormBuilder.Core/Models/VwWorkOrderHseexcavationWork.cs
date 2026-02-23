using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwWorkOrderHseexcavationWork
{
    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Details { get; set; }

    public int IdExcavationWork { get; set; }

    public string HseexcavationWorkName { get; set; } = null!;

    public string? HseexcavationWorkForeignName { get; set; }

    public bool HseexcavationWorkIsActive { get; set; }

    public string? HseexcavationWorkRemark { get; set; }

    public int? IdChoice { get; set; }

    public string ChoiceName { get; set; } = null!;

    public string? ChoiceForeignName { get; set; }

    public bool ChoiceIsActive { get; set; }

    public int IdWorkOrder { get; set; }

    public string WorkOrderName { get; set; } = null!;

    public string? WorkOrderForeignName { get; set; }

    public string? WorkOrderDocumentNumber { get; set; }
}
