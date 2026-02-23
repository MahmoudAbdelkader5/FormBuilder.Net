using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblWorkOrderAttachment
{
    public int IdWorkOrder { get; set; }

    public int IdAttachment { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual TblAttachment IdAttachmentNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblWorkOrder IdWorkOrderNavigation { get; set; } = null!;
}
