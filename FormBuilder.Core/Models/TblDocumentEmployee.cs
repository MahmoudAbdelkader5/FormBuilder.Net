using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblDocumentEmployee
{
    public int IdObjectType { get; set; }

    public int IdDocument { get; set; }

    public int IdEmployee { get; set; }

    public int? IdLegalEntity { get; set; }

    public int IdCreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual TblEmployee IdEmployeeNavigation { get; set; } = null!;

    public virtual TblLegalEntity? IdLegalEntityNavigation { get; set; }
}
