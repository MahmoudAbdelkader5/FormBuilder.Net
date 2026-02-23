using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwWorkOrderMaintenanceRequest
{
    public int Id { get; set; }

    public DateTime? DeffectDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Remark { get; set; }

    public DateTime? PostingDate { get; set; }

    public string? Code { get; set; }

    public int? IdMaintenanceType { get; set; }

    public string? MTypeCode { get; set; }

    public string? MTypeName { get; set; }

    public string? MTypeForeignName { get; set; }

    public string? MTypeDescription { get; set; }

    public string? MTypeForeignDescription { get; set; }

    public bool? MTypeIsActive { get; set; }

    public int IdAsset { get; set; }

    public string AssetCode { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public string? AssetForeignName { get; set; }

    public int? AssetType { get; set; }

    public string? AssetDescription { get; set; }

    public string? AssetForeignDescription { get; set; }

    public int? IdCustomer { get; set; }

    public string? CustomerFirstName { get; set; }

    public string? CustomerForeignFirstName { get; set; }

    public string? CustomerLastName { get; set; }

    public string? CustomerForeignLastName { get; set; }

    public string? CustomerCode { get; set; }

    public string? CustomerMiddleName { get; set; }

    public string? CustomerForeignMiddleName { get; set; }

    public bool? CustomerIsBlackList { get; set; }

    public int IdStatus { get; set; }

    public string? StatusName { get; set; }

    public string? StatusForeignName { get; set; }

    public int? IdRequesterStatus { get; set; }

    public string? RequesterStatusName { get; set; }

    public string? RequesterStatusForeignName { get; set; }

    public int? IdShift { get; set; }

    public string? ShiftName { get; set; }

    public string? ShiftForeignName { get; set; }

    public int? IdDepartment { get; set; }

    public string? DepartmentName { get; set; }

    public string? DepartmentForeignName { get; set; }

    public int? AssetIdZone { get; set; }

    public string? ZoneCode { get; set; }

    public string? ZoneName { get; set; }

    public string? ZoneForeignName { get; set; }

    public int? MTypeIdWorkOrderCategory { get; set; }

    public string? WorkOrderCategoryName { get; set; }

    public string? WorkOrderCategoryForeignName { get; set; }

    public int? RequestIdWorkOrder { get; set; }

    public string? WorkOrderName { get; set; }

    public string? WorkOrderForeignName { get; set; }

    public string? WorkOrderDocumentNumber { get; set; }

    public int IdCreatedBy { get; set; }

    public string? UserUsername { get; set; }

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }

    public int? WorkOrderIdApprovalStatus { get; set; }

    public string? WorkOrderApprovalStatusName { get; set; }

    public string? WorkOrderApprovalStatusForeignName { get; set; }

    public int? IdUnit { get; set; }

    public string? UnitName { get; set; }

    public string? UnitForeignName { get; set; }

    public string? UnitCode { get; set; }

    public int? IdParent { get; set; }

    public string? ParentCode { get; set; }

    public string? ParentName { get; set; }

    public string? ParentForeignName { get; set; }
}
