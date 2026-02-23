using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblMobileNews
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Body { get; set; }

    public DateTime? EffectiveDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }
}
