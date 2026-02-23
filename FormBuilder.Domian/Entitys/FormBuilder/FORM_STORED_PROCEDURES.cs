using formBuilder.Domian.Entitys;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormBuilder.Domian.Entitys.FormBuilder
{
    [Table("FORM_STORED_PROCEDURES")]
    public class FORM_STORED_PROCEDURES : BaseEntity
    {
        /// <summary>
        /// Display name shown in UI
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Optional description
        /// </summary>
        [StringLength(1000)]
        public string? Description { get; set; }

        /// <summary>
        /// Target database name (FormBuilder or AKHManageIT)
        /// </summary>
        [Required]
        [StringLength(128)]
        public string DatabaseName { get; set; } = string.Empty;

        /// <summary>
        /// Schema name (default: dbo)
        /// </summary>
        [Required]
        [StringLength(128)]
        public string SchemaName { get; set; } = "dbo";

        /// <summary>
        /// Stored Procedure name (e.g., sp_CheckEmployeeGrade) - extracted from ProcedureCode
        /// </summary>
        [StringLength(128)]
        public string? ProcedureName { get; set; }

        /// <summary>
        /// Stored Procedure code/text (the actual SP definition)
        /// Example: "CREATE PROCEDURE sp_CheckEmployeeGrade @EmployeeId INT, @RequiredGrade INT AS BEGIN ... END"
        /// </summary>
        [Required]
        public string ProcedureCode { get; set; } = string.Empty;

        /// <summary>
        /// Usage type: 'Rule' or 'Options' (optional classification)
        /// </summary>
        [StringLength(30)]
        public string? UsageType { get; set; }

        /// <summary>
        /// Is Read-Only (for Rules, typically should be 1)
        /// </summary>
        public bool IsReadOnly { get; set; } = true;

        /// <summary>
        /// Execution order (optional)
        /// </summary>
        public int? ExecutionOrder { get; set; }
    }
}

