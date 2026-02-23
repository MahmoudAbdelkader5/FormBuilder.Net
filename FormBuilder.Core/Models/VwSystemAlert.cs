using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwSystemAlert
{
    public int Id { get; set; }

    public string? Subject { get; set; }

    public string? Body { get; set; }

    public bool IsRead { get; set; }

    public DateTime? ReadDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IdUser { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public string UserUserName { get; set; } = null!;

    public int? IdAlertObject { get; set; }

    public int? IdAlertType { get; set; }

    public string? TypeName { get; set; }

    public string? TypeForeignName { get; set; }
}
