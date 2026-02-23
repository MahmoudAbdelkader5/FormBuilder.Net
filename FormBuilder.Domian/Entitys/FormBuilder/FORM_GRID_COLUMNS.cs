using formBuilder.Domian.Entitys;
using FormBuilder.Domian.Entitys.FormBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Domian.Entitys.FormBuilder
{
    [Table("FORM_GRID_COLUMNS")]
    public class FORM_GRID_COLUMNS : BaseEntity
    {
     

        [ForeignKey("FORM_GRIDS")]
        public int GridId { get; set; }
        public virtual FORM_GRIDS FORM_GRIDS { get; set; }

        [ForeignKey("FIELD_TYPES")]
        public int? FieldTypeId { get; set; }
        public virtual FIELD_TYPES? FIELD_TYPES { get; set; }

        [Required, StringLength(200)]
        public string ColumnName { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string ColumnCode { get; set; } = string.Empty;

        public int ColumnOrder { get; set; }
        public bool IsMandatory { get; set; }
        public string DataType { get; set; } = string.Empty;
        public int? MaxLength { get; set; }
        public string? DefaultValueJson { get; set; }
        public string? ValidationRuleJson { get; set; }
        public new bool IsActive { get; set; }

        /// <summary>
        /// Indicates if the column is read-only (cannot be edited by end users)
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Indicates if the column is visible in the grid
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// JSON configuration for dynamic visibility rules
        /// Example: {"showWhen": {"columnCode": "status", "operator": "equals", "value": "active"}}
        /// </summary>
        public string? VisibilityRuleJson { get; set; }

        public virtual ICollection<FORM_SUBMISSION_GRID_CELLS> FORM_SUBMISSION_GRID_CELLS { get; set; }
        public virtual ICollection<GRID_COLUMN_DATA_SOURCES> GRID_COLUMN_DATA_SOURCES { get; set; }
        public virtual ICollection<GRID_COLUMN_OPTIONS> GRID_COLUMN_OPTIONS { get; set; }
    }
}
