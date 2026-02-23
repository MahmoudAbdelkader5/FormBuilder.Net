using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblGoodsIssueItemSerialNumber
{
    public int Id { get; set; }

    public int IdGoodsIssue { get; set; }

    public int? IdGoodsIssueItem { get; set; }

    public int? IdItemSerialNumber { get; set; }

    public int? IdLegalEntity { get; set; }

    public decimal? Cost { get; set; }

    public virtual TblGoodsIssueItem? IdGoodsIssueItemNavigation { get; set; }

    public virtual TblGoodsIssue IdGoodsIssueNavigation { get; set; } = null!;

    public virtual TblItemSerialNumber? IdItemSerialNumberNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }
}
