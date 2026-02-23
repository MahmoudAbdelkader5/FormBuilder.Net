using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwGlaccountDetermination
{
    public int Id { get; set; }

    public string? Remarks { get; set; }

    public bool IsActive { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdGlaccount { get; set; }

    public string? GlaccountName { get; set; }

    public string? GlaccountForeignName { get; set; }

    public string? GlaccountCode { get; set; }

    public int? IdType { get; set; }

    public string? TransactionTypeName { get; set; }

    public string? TransactionTypeForeignName { get; set; }
}
