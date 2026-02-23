using formBuilder.Domian.Entitys;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Domian.Entitys.froms;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("FORM_FIELDS")]
public class FORM_FIELDS :BaseEntity
{
    

    [ForeignKey("FORM_TABS")]
    public int TabId { get; set; }
    public virtual FORM_TABS FORM_TABS { get; set; } = null!;

    [ForeignKey("FIELD_TYPES")]
    public int? FieldTypeId { get; set; }
    public virtual FIELD_TYPES? FIELD_TYPES { get; set; }

    [ForeignKey("FORM_GRIDS")]
    public int? GridId { get; set; }
    public virtual FORM_GRIDS? Grid { get; set; }

    [Required, StringLength(200)]
    public string FieldName { get; set; } = string.Empty;

    [StringLength(200)]
    public string? ForeignFieldName { get; set; }

    [Required, StringLength(100)]
    public string FieldCode { get; set; } = string.Empty;

    public int FieldOrder { get; set; }
    public string ?Placeholder { get; set; }
    public string? ForeignPlaceholder { get; set; }
    public string HintText { get; set; } = string.Empty;
    public string? ForeignHintText { get; set; }
    public bool ?IsMandatory { get; set; }
    public bool ?IsEditable { get; set; }
    public bool IsVisible { get; set; }
    public string ? DefaultValueJson { get; set; }
    public decimal? MinValue { get; set; }
    public decimal? MaxValue { get; set; }
    public string ? RegexPattern { get; set; }
    public string ?ValidationMessage { get; set; }
    public string? ForeignValidationMessage { get; set; }

    // Calculation Fields Properties
    [StringLength(int.MaxValue)]
    public string? ExpressionText { get; set; }

    [StringLength(50)]
    public string? CalculationMode { get; set; }

    [StringLength(50)]
    public string? RecalculateOn { get; set; }

    [StringLength(50)]
    public string? ResultType { get; set; }

    public new bool IsActive { get; set; }

    public virtual ICollection<FIELD_OPTIONS> FIELD_OPTIONS { get; set; } = null!;
    public virtual ICollection<FIELD_DATA_SOURCES> FIELD_DATA_SOURCES { get; set; } = null!;
    public virtual ICollection<FORM_SUBMISSION_VALUES> FORM_SUBMISSION_VALUES { get; set; } = null!;
    public virtual ICollection<FORM_SUBMISSION_ATTACHMENTS> FORM_SUBMISSION_ATTACHMENTS { get; set; } = null!;
    public virtual ICollection<SAP_FIELD_MAPPINGS> SAP_FIELD_MAPPINGS { get; set; } = null!;
    public virtual ICollection<FORMULA_VARIABLES> FORMULA_VARIABLES { get; set; } = null!;
}
