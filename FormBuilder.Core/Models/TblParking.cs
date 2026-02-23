using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblParking
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ForeignName { get; set; }

    public string? Floor { get; set; }

    public string SpaceNumber { get; set; } = null!;

    public string LevelNumber { get; set; } = null!;

    public int? IdProperty { get; set; }

    public int? IdAvailability { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblProperty? IdPropertyNavigation { get; set; }

    public virtual ICollection<TblDocumentParking> TblDocumentParkings { get; set; } = new List<TblDocumentParking>();

    public virtual ICollection<TblUnitParking> TblUnitParkings { get; set; } = new List<TblUnitParking>();
}
