using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblPropertyNeighborhood
{
    public int Id { get; set; }

    public int IdNeighborhood { get; set; }

    public int IdProperty { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public string? Note { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblNeighborhood IdNeighborhoodNavigation { get; set; } = null!;

    public virtual TblProperty IdPropertyNavigation { get; set; } = null!;
}
