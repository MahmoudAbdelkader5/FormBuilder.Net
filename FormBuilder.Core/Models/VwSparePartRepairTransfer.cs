using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwSparePartRepairTransfer
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsIntegrated { get; set; }

    public int? IdSparePartRepairRequest { get; set; }

    public string? SparePartRepairRequestCode { get; set; }

    public int? IdTechncian { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public int? IdApprovalStatus { get; set; }

    public string? StatusName { get; set; }

    public string? StatusForeignName { get; set; }
}
