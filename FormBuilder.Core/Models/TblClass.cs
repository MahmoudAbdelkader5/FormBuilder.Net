using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblClass
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IdParent { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblProperty> TblProperties { get; set; } = new List<TblProperty>();

    public virtual ICollection<TblUnitClassParticular> TblUnitClassParticulars { get; set; } = new List<TblUnitClassParticular>();

    public virtual ICollection<TblUnit> TblUnits { get; set; } = new List<TblUnit>();
}
