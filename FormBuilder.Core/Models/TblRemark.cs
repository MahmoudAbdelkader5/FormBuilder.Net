using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblRemark
{
    public int Id { get; set; }

    public int? IdObjectType { get; set; }

    public int? IdObject { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Description { get; set; }

    public int? IdWorkOrder { get; set; }

    public virtual TblUser IdCreatedByNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblWorkOrder? IdWorkOrderNavigation { get; set; }

    public virtual ICollection<TblWorkOrderRemark> TblWorkOrderRemarks { get; set; } = new List<TblWorkOrderRemark>();
}
