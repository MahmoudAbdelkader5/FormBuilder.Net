using System;
using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.DTOS.FormBuilder
{
    public class StoredProcedureDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string DatabaseName { get; set; } = string.Empty;
        public string SchemaName { get; set; } = "dbo";
        public string? ProcedureName { get; set; }
        public string ProcedureCode { get; set; } = string.Empty;
        public string? UsageType { get; set; }
        public bool IsReadOnly { get; set; }
        public int? ExecutionOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedByUserId { get; set; }
    }

    public class CreateStoredProcedureDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [StringLength(128)]
        public string DatabaseName { get; set; } = string.Empty;

        [StringLength(128)]
        public string SchemaName { get; set; } = "dbo";

        [StringLength(128)]
        public string? ProcedureName { get; set; }

        [Required]
        public string ProcedureCode { get; set; } = string.Empty;

        [StringLength(30)]
        public string? UsageType { get; set; }

        public bool IsReadOnly { get; set; } = true;

        public int? ExecutionOrder { get; set; }
    }

    public class UpdateStoredProcedureDto
    {
        [StringLength(200)]
        public string? Title { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(128)]
        public string? DatabaseName { get; set; }

        [StringLength(128)]
        public string? SchemaName { get; set; }

        [StringLength(128)]
        public string? ProcedureName { get; set; }

        public string? ProcedureCode { get; set; }

        [StringLength(30)]
        public string? UsageType { get; set; }

        public bool? IsReadOnly { get; set; }

        public int? ExecutionOrder { get; set; }

        public bool? IsActive { get; set; }
    }
}

