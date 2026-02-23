using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwRequiredAttestationType
{
    public int Id { get; set; }

    public bool? IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int IdAttestationType { get; set; }

    public string AttestationTypeName { get; set; } = null!;

    public string? AttestationTypeForeignName { get; set; }

    public int IdState { get; set; }

    public string? StateName { get; set; }

    public string? StateCode { get; set; }

    public string? StateForeignName { get; set; }
}
