using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblPropertyProgress
{
    public int Id { get; set; }

    public DateTime CheckedDate { get; set; }

    public decimal Percentage { get; set; }

    public int? IdProperty { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblProperty? IdPropertyNavigation { get; set; }
}
