using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblCostCenter
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public DateTime? ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public int IdDimension { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual TblDimension IdDimensionNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblAsset> TblAssetIdCostCenter1Navigations { get; set; } = new List<TblAsset>();

    public virtual ICollection<TblAsset> TblAssetIdCostCenter2Navigations { get; set; } = new List<TblAsset>();

    public virtual ICollection<TblAsset> TblAssetIdCostCenter3Navigations { get; set; } = new List<TblAsset>();

    public virtual ICollection<TblAsset> TblAssetIdCostCenter4Navigations { get; set; } = new List<TblAsset>();

    public virtual ICollection<TblAsset> TblAssetIdCostCenter5Navigations { get; set; } = new List<TblAsset>();
}
