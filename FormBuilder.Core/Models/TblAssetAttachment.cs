using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblAssetAttachment
{
    public int IdDocument { get; set; }

    public int IdAsset { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }
}
