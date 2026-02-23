using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwWorkOrderHsedeepExcavationControl
{
    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int IdControls { get; set; }

    public string ControlsName { get; set; } = null!;

    public string? ControlsForeignName { get; set; }

    public bool ControlsIsActive { get; set; }

    public int? IdChoice { get; set; }

    public string ChoiceName { get; set; } = null!;

    public string? ChoiceForeignName { get; set; }

    public bool ChoiceIsActive { get; set; }

    public int IdWorkOrder { get; set; }

    public string WorkOrderName { get; set; } = null!;

    public string? WorkOrderForeignName { get; set; }

    public string? WorkOrderDocumentNumber { get; set; }
}
