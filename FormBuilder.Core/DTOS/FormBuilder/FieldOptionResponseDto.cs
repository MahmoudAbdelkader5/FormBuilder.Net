using System.Collections.Generic;

namespace FormBuilder.Core.DTOS.FormBuilder
{
    /// <summary>
    /// Unified response format for field options from any data source
    /// </summary>
    public class FieldOptionResponseDto
    {
        public string Value { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request DTO for getting field options
    /// </summary>
    public class GetFieldOptionsRequestDto
    {
        public int FieldId { get; set; }
        public Dictionary<string, object>? Context { get; set; }
        public string? RequestBodyJson { get; set; }
    }

    /// <summary>
    /// Request DTO for previewing data source
    /// </summary>
    public class PreviewDataSourceRequestDto
    {
        public int? FieldId { get; set; }
        public int? SapConfigId { get; set; }
        public string SourceType { get; set; } = string.Empty;
        public string? ApiUrl { get; set; }
        /// <summary>
        /// API Endpoint Path (e.g., "products", "users", "results")
        /// Combined with ApiUrl (Base URL) to form full URL: ApiUrl + ApiPath
        /// </summary>
        public string? ApiPath { get; set; }
        public string? HttpMethod { get; set; }
        public string? RequestBodyJson { get; set; }
        public string? ValuePath { get; set; }
        public string? TextPath { get; set; }
        /// <summary>
        /// JSON configuration for data source
        /// For LookupTable: {"table": "CUSTOMERS", "valueColumn": "Id", "textColumn": "Name", "database": "FormBuilder"}
        /// For SQL Query: {"sqlQuery": "SELECT Id, Name FROM TblAreas WHERE IsActive = 1", "valueColumn": "Id", "textColumn": "Name", "database": "AkhmanageIt"}
        /// For API: {"url": "...", "httpMethod": "GET", "valuePath": "...", "textPath": "...", "requestBodyJson": "..."}
        /// </summary>
        public string? ConfigurationJson { get; set; }
        /// <summary>
        /// Custom array property names to search for in API response (comma-separated)
        /// Example: "data,results,items,users" or ["data", "results", "items"]
        /// If not provided, uses default common names
        /// </summary>
        public List<string>? ArrayPropertyNames { get; set; }
    }

    /// <summary>
    /// Request DTO for getting column options (similar to GetFieldOptionsRequestDto)
    /// </summary>
    public class GetColumnOptionsRequestDto
    {
        public int ColumnId { get; set; }
        public Dictionary<string, object>? Context { get; set; }
        public string? RequestBodyJson { get; set; }
    }

    /// <summary>
    /// Request DTO for previewing column data source
    /// </summary>
    public class PreviewColumnDataSourceRequestDto
    {
        public int? ColumnId { get; set; }
        public string SourceType { get; set; } = string.Empty;
        public string? ApiUrl { get; set; }
        /// <summary>
        /// API Endpoint Path (e.g., "products", "users", "results")
        /// Combined with ApiUrl (Base URL) to form full URL: ApiUrl + ApiPath
        /// </summary>
        public string? ApiPath { get; set; }
        public string? HttpMethod { get; set; }
        public string? RequestBodyJson { get; set; }
        public string? ValuePath { get; set; }
        public string? TextPath { get; set; }
        /// <summary>
        /// Custom array property names to search for in API response (comma-separated)
        /// Example: "data,results,items,users" or ["data", "results", "items"]
        /// If not provided, uses default common names
        /// </summary>
        public List<string>? ArrayPropertyNames { get; set; }
    }
}
