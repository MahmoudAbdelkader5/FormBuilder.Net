using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblRequiredAttestationType
{
    public int Id { get; set; }

    public int IdAttestationType { get; set; }

    public int IdState { get; set; }

    public bool? IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdLegalEntity { get; set; }

    public virtual TblAttestationType IdAttestationTypeNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual TblState IdStateNavigation { get; set; } = null!;
}
