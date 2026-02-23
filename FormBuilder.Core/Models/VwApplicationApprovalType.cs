using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwApplicationApprovalType
{
    public int IdApplication { get; set; }

    public string ApplicationName { get; set; } = null!;

    public int IdApprovalType { get; set; }

    public string ApprovalTypeName { get; set; } = null!;

    public string? ApprovalTypeForeignName { get; set; }
}
