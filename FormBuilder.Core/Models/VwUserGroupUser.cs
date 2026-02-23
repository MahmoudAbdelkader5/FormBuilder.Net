using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwUserGroupUser
{
    public int IdUserGroup { get; set; }

    public DateTime CreatedDate { get; set; }

    public int IdCreatedBy { get; set; }

    public int? IdLegalEntity { get; set; }

    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }
}
