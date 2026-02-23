
using formBuilder.Domian.Entitys;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("FORM_GRIDS")]
public class FORM_GRIDS : BaseEntity
{
 
    [ForeignKey("FORM_BUILDER")]
    public int FormBuilderId { get; set; }
    public virtual FORM_BUILDER FORM_BUILDER { get; set; } = null!;

    [Required, StringLength(200)]
    public string GridName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string GridCode { get; set; } = string.Empty;

    [ForeignKey("FORM_TABS")]
    public int? TabId { get; set; }
    public virtual FORM_TABS FORM_TABS { get; set; } = null!;

    public int GridOrder { get; set; }
    public new bool IsActive { get; set; }

    /// <summary>
    /// Minimum number of rows required in the grid
    /// </summary>
    public int? MinRows { get; set; }

    /// <summary>
    /// Maximum number of rows allowed in the grid
    /// </summary>
    public int? MaxRows { get; set; }

    /// <summary>
    /// JSON configuration for grid-level rules
    /// Example: {"allowDelete": true, "allowAdd": true, "readOnlyAfterSubmission": true}
    /// </summary>
    public string? GridRulesJson { get; set; }

    public virtual ICollection<FORM_GRID_COLUMNS> FORM_GRID_COLUMNS { get; set; } = null!;
    public virtual ICollection<FORM_SUBMISSION_GRID_ROWS> FORM_SUBMISSION_GRID_ROWS { get; set; } = null!;
}
