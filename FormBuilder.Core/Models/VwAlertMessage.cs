using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwAlertMessage
{
    public int Id { get; set; }

    public string? Subject { get; set; }

    public string? Body { get; set; }

    public string? ForeignSubject { get; set; }

    public string? ForeignBody { get; set; }

    public string? Screen { get; set; }

    public string? ForeignScreen { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int IdAlertType { get; set; }

    public string TypeName { get; set; } = null!;

    public string? TypeForeignName { get; set; }

    public int IdAlertMethod { get; set; }

    public string MethodName { get; set; } = null!;

    public string? MethodForeignName { get; set; }
}
