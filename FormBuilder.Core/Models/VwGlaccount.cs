using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwGlaccount
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public string? Code { get; set; }

    public bool? IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsPostable { get; set; }

    public bool? IsControlAccount { get; set; }

    public int? IdParentAccount { get; set; }

    public string? ParentCode { get; set; }

    public string? ParentName { get; set; }

    public string? ParentForeignName { get; set; }

    public int? IdType { get; set; }

    public string? TypeName { get; set; }

    public string? TypeForeignName { get; set; }
}
