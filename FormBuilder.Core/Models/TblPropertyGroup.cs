using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblPropertyGroup
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ForeignName { get; set; }

    public int? IdParent { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblPropertyGroup? IdParentNavigation { get; set; }

    public virtual ICollection<TblPropertyGroup> InverseIdParentNavigation { get; set; } = new List<TblPropertyGroup>();

    public virtual ICollection<TblProperty> TblPropertyIdGroupNavigations { get; set; } = new List<TblProperty>();

    public virtual ICollection<TblProperty> TblPropertyIdSubGroupNavigations { get; set; } = new List<TblProperty>();
}
