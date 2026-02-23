using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblMaterialTransfer
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

    public int? IdRequest { get; set; }

    public bool? IsIntegrated { get; set; }

    public int? IdObjectType { get; set; }

    public DateTime? IssuedDate { get; set; }

    public string? IntegrationStatus { get; set; }

    public int? IdSap { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblUser IdTechncianNavigation { get; set; } = null!;

    public virtual TblWorkOrder? IdWorkOrderNavigation { get; set; }

    public virtual ICollection<TblMaterialTransferItemSerialNumber> TblMaterialTransferItemSerialNumbers { get; set; } = new List<TblMaterialTransferItemSerialNumber>();

    public virtual ICollection<TblMaterialTransferItem> TblMaterialTransferItems { get; set; } = new List<TblMaterialTransferItem>();
}
