using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwMeterReading
{
    public int Id { get; set; }

    public string? DocumentNumber { get; set; }

    public DateTime PostingDate { get; set; }

    public int IdCreatedBy { get; set; }

    public string UserUsername { get; set; } = null!;

    public string? UserName { get; set; }

    public string? UserForeignName { get; set; }
}
