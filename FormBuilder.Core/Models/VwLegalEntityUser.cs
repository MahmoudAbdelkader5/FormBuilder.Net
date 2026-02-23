using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwLegalEntityUser
{
    public int IdLegalEntity { get; set; }

    public DateTime CreatedDate { get; set; }

    public int IdCreatedBy { get; set; }

    public int Id { get; set; }

    public string UserUsername { get; set; } = null!;

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public string? UserEmail { get; set; }

    public string? UserPhone { get; set; }

    public string EntityName { get; set; } = null!;

    public string? EntityForeignName { get; set; }

    public bool EntityIsActive { get; set; }
}
