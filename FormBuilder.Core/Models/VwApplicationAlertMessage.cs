using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwApplicationAlertMessage
{
    public int IdApplication { get; set; }

    public string ApplicationName { get; set; } = null!;

    public int IdAlertMessage { get; set; }

    public string? AlertSubject { get; set; }

    public string? AlertForeignSubject { get; set; }

    public string? AlertBody { get; set; }

    public string? AlertForeignBody { get; set; }

    public string? AlertScreen { get; set; }
}
