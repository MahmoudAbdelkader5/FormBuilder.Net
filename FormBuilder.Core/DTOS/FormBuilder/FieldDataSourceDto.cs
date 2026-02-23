using System.ComponentModel.DataAnnotations;

namespace FormBuilder.API.Models
{
    public class FieldDataSourceDto
    {
        public int Id { get; set; }
        public int FieldId { get; set; }
        public string SourceType { get; set; } = string.Empty;
        public string? ApiUrl { get; set; }
        /// <summary>
        /// API Endpoint Path (e.g., "products", "users", "results")
        /// Combined with ApiUrl (Base URL) to form full URL: ApiUrl + ApiPath
        /// </summary>
        public string? ApiPath { get; set; }
        public string? HttpMethod { get; set; }
        public string? RequestBodyJson { get; set; }
        // SQL query (for SqlQuery source type)
        public string? SqlQuery { get; set; }
        public string? ValuePath { get; set; }
        public string? TextPath { get; set; }
        /// <summary>
        /// JSON configuration for data source
        /// For LookupTable: {"table": "CUSTOMERS", "valueColumn": "Id", "textColumn": "Name"}
        /// For API: {"url": "...", "httpMethod": "GET", "valuePath": "...", "textPath": "...", "requestBodyJson": "..."
        /// </summary>
        public string? ConfigurationJson { get; set; }
        /// <summary>
        /// Custom array property names to search for in API response (comma-separated or JSON array)
        /// Example: "data,results,items" or ["data", "results", "items"]
        /// If not provided, uses default common names
        /// </summary>
        public List<string>? ArrayPropertyNames { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateFieldDataSourceDto
    {
        [Required]
        public int FieldId { get; set; }

        [Required, StringLength(50)]
        public string SourceType { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ApiUrl { get; set; }

        /// <summary>
        /// API Endpoint Path (e.g., "products", "users", "results")
        /// Combined with ApiUrl (Base URL) to form full URL: ApiUrl + ApiPath
        /// </summary>
        [StringLength(200)]
        public string? ApiPath { get; set; }

        [StringLength(10)]
        public string? HttpMethod { get; set; }

        public string? RequestBodyJson { get; set; }

        // SQL query (for SqlQuery source type)
        [StringLength(4000)]
        public string? SqlQuery { get; set; }

        [StringLength(200)]
        public string? ValuePath { get; set; }

        [StringLength(200)]
        public string? TextPath { get; set; }

        /// <summary>
        /// JSON configuration for data source
        /// For LookupTable: {"table": "CUSTOMERS", "valueColumn": "Id", "textColumn": "Name"}
        /// For API: {"url": "...", "httpMethod": "GET", "valuePath": "...", "textPath": "...", "requestBodyJson": "..."
        /// </summary>
        public string? ConfigurationJson { get; set; }
        /// <summary>
        /// Custom array property names to search for in API response (comma-separated or JSON array)
        /// Example: "data,results,items" or ["data", "results", "items"]
        /// If not provided, uses default common names
        /// </summary>
        public List<string>? ArrayPropertyNames { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpdateFieldDataSourceDto
    {
        [Required, StringLength(50)]
        public string SourceType { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ApiUrl { get; set; }

        /// <summary>
        /// API Endpoint Path (e.g., "products", "users", "results")
        /// Combined with ApiUrl (Base URL) to form full URL: ApiUrl + ApiPath
        /// </summary>
        [StringLength(200)]
        public string? ApiPath { get; set; }

        [StringLength(10)]
        public string? HttpMethod { get; set; }

        public string? RequestBodyJson { get; set; }

        // SQL query (for SqlQuery source type)
        [StringLength(4000)]
        public string? SqlQuery { get; set; }

        [StringLength(200)]
        public string? ValuePath { get; set; }

        [StringLength(200)]
        public string? TextPath { get; set; }

        /// <summary>
        /// JSON configuration for data source
        /// For LookupTable: {"table": "CUSTOMERS", "valueColumn": "Id", "textColumn": "Name"}
        /// For API: {"url": "...", "httpMethod": "GET", "valuePath": "...", "textPath": "...", "requestBodyJson": "..."
        /// </summary>
        public string? ConfigurationJson { get; set; }
        /// <summary>
        /// Custom array property names to search for in API response (comma-separated or JSON array)
        /// Example: "data,results,items" or ["data", "results", "items"]
        /// If not provided, uses default common names
        /// </summary>
        public List<string>? ArrayPropertyNames { get; set; }

        public bool IsActive { get; set; }
    }
}