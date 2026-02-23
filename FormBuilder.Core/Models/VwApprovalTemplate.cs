using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwApprovalTemplate
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public string? Description { get; set; }

    public bool IsCustom { get; set; }

    public bool IsSequential { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Query { get; set; }

    public int IdApprovalType { get; set; }

    public string TypeName { get; set; } = null!;

    public string? TypeForeignName { get; set; }

    public int IdApprovalStage { get; set; }

    public string StageName { get; set; } = null!;

    public string StageForeignName { get; set; } = null!;
}
