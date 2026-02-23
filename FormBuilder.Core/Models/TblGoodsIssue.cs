using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblGoodsIssue
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public int? IdWorkOrder { get; set; }

    public int IdTechncian { get; set; }

    public int? IdApprovalStatus { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsIntegrated { get; set; }

    public DateTime? IssuedDate { get; set; }

    public string? IntegrationStatus { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblUser IdTechncianNavigation { get; set; } = null!;

    public virtual TblWorkOrder? IdWorkOrderNavigation { get; set; }

    public virtual ICollection<TblGoodsIssueItemSerialNumber> TblGoodsIssueItemSerialNumbers { get; set; } = new List<TblGoodsIssueItemSerialNumber>();

    public virtual ICollection<TblGoodsIssueItem> TblGoodsIssueItems { get; set; } = new List<TblGoodsIssueItem>();
}
