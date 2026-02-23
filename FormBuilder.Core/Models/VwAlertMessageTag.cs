using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwAlertMessageTag
{
    public int IdAlertMessage { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? ForeignDescription { get; set; }

    public int? IdApplication { get; set; }
}
