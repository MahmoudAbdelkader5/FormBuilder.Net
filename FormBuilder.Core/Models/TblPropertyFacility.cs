using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblPropertyFacility
{
    public int Id { get; set; }

    public int IdFacility { get; set; }

    public int IdProperty { get; set; }

    public int Quantity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public string? Note { get; set; }

    public virtual TblFacility IdFacilityNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblProperty IdPropertyNavigation { get; set; } = null!;
}
