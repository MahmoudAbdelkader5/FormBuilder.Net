using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblWorkOrderRemark
{
    public int IdWorkOrder { get; set; }

    public int IdRemark { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdateBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblRemark IdRemarkNavigation { get; set; } = null!;

    public virtual TblWorkOrder IdWorkOrderNavigation { get; set; } = null!;
}
