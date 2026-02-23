using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwLegalEntity
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? DatabaseServer { get; set; }

    public string? DatabaseName { get; set; }

    public string? DatabaseUsername { get; set; }

    public string? DatabasePassword { get; set; }

    public string? SapuserUsername { get; set; }

    public string? SapuserPassword { get; set; }

    public string? LiscenceServer { get; set; }

    public string? Sldserver { get; set; }

    public bool? AllowMultiBranches { get; set; }

    public string? Sapserver { get; set; }

    public int? IdConnectionType { get; set; }

    public string ConnectionTypeName { get; set; } = null!;

    public string? ConnectionTypeForeignName { get; set; }
}
