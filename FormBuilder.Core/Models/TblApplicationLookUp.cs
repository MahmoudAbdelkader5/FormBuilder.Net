using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class TblApplicationLookUp
{
    public int IdApplication { get; set; }

    public int IdLookUp { get; set; }

    public int IdLookUpType { get; set; }
}
