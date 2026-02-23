using System;
using System.Collections.Generic;

namespace FormBuilder.Core.Models;

public partial class VwPropertyProgress
{
    public int Id { get; set; }

    public DateTime CheckedDate { get; set; }

    public decimal Percentage { get; set; }

    public int? IdLegalEntity { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int IdCreatedBy { get; set; }

    public int? IdUpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public int? IdProperty { get; set; }

    public string? PropertyName { get; set; }

    public string? PropertyForeignName { get; set; }

    public string? PropertyCode { get; set; }
}
