using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblGlaccountDetermination
{
    public int Id { get; set; }

    public int? IdGlaccount { get; set; }

    public int? IdType { get; set; }

    public string? Remarks { get; set; }

    public int? IdLegalEntity { get; set; }

    public bool IsActive { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? IdUpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual TblGlaccount? IdGlaccountNavigation { get; set; }

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }

    public virtual ICollection<TblGlaccountDeterminationDetail> TblGlaccountDeterminationDetails { get; set; } = new List<TblGlaccountDeterminationDetail>();
}
