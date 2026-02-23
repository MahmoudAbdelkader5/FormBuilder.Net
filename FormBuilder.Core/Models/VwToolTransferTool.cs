using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwToolTransferTool
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public int? IdLegalEntity { get; set; }

    public DateTime? StatusDate { get; set; }

    public string? Remarks { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? IdCreatedBy { get; set; }

    public int? IdToolTransfer { get; set; }

    public string ToolTransferCode { get; set; } = null!;

    public int IdTechnician { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public int IdTool { get; set; }

    public string? ToolCode { get; set; }

    public string ToolName { get; set; } = null!;

    public string? ToolForeignName { get; set; }

    public int? IdToWarehouse { get; set; }

    public string? ToWarehouseCode { get; set; }

    public string? ToWarehouseName { get; set; }

    public string? ToWarehouseForeignName { get; set; }

    public int? IdFromWarehouse { get; set; }

    public string? FromWarehouseCode { get; set; }

    public string? FromWarehouseName { get; set; }

    public string? FromWarehouseForeignName { get; set; }

    public int? IdStatus { get; set; }

    public string? ToolStatusName { get; set; }

    public string? ToolStatusForeignName { get; set; }

    public int IdStatusBy { get; set; }

    public string? UserStatusName { get; set; }

    public string? UserStatusForeignName { get; set; }
}
