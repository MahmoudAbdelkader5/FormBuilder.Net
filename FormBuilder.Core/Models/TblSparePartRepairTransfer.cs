using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblSparePartRepairTransfer
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public int? IdSparePartRepairRequest { get; set; }

    public int? IdTechncian { get; set; }

    public int? IdApprovalStatus { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool? IsIntegrated { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblSparePartRepairRequest? IdSparePartRepairRequestNavigation { get; set; }

    public virtual TblUser? IdTechncianNavigation { get; set; }

    public virtual ICollection<TblSparePartRepairTransferDetail> TblSparePartRepairTransferDetails { get; set; } = new List<TblSparePartRepairTransferDetail>();
}
