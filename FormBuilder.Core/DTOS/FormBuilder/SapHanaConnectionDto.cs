using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.DTOS.FormBuilder
{
    /// <summary>
    /// DTO for SAP HANA connection details
    /// </summary>
    public class SapHanaConnectionDto
    {
        [Required]
        [StringLength(500)]
        public string Server { get; set; } = string.Empty; // e.g., "hb152.tyconz.com:30015"

        [Required]
        [StringLength(100)]
        public string Schema { get; set; } = string.Empty; // e.g., "SBO_DEV"

        [Required]
        [StringLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for SAP HANA query execution request
    /// </summary>
    public class SapHanaQueryRequestDto
    {
        [Required]
        public SapHanaConnectionDto Connection { get; set; } = new();

        [Required]
        public string Query { get; set; } = string.Empty;

        /// <summary>
        /// Optional: Maximum number of rows to return (default: 1000)
        /// </summary>
        public int? MaxRows { get; set; } = 1000;
    }

    /// <summary>
    /// DTO for SAP HANA query execution response
    /// </summary>
    public class SapHanaQueryResponseDto
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public List<Dictionary<string, object?>>? Data { get; set; }
        public List<SapHanaColumnInfo>? Columns { get; set; }
        public int RowCount { get; set; }
    }

    /// <summary>
    /// Column information for SAP HANA query results
    /// </summary>
    public class SapHanaColumnInfo
    {
        public string Name { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public int? Length { get; set; }
        public int? Scale { get; set; }
    }

    /// <summary>
    /// DTO for testing SAP HANA connection
    /// </summary>
    public class SapHanaTestConnectionDto
    {
        [Required]
        public SapHanaConnectionDto Connection { get; set; } = new();
    }

    /// <summary>
    /// DTO for getting SAP HANA tables list
    /// </summary>
    public class SapHanaGetTablesRequestDto
    {
        [Required]
        public SapHanaConnectionDto Connection { get; set; } = new();
    }

    /// <summary>
    /// DTO for getting SAP HANA table columns
    /// </summary>
    public class SapHanaGetTableColumnsRequestDto
    {
        [Required]
        public SapHanaConnectionDto Connection { get; set; } = new();

        [Required]
        [StringLength(200)]
        public string TableName { get; set; } = string.Empty;
    }
}

