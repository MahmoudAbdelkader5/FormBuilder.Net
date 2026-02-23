using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblAssetAsset
{
    public int IdAsset { get; set; }

    public int IdParent { get; set; }

    public int? Quantiy { get; set; }

    public decimal? Cost { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }
}
