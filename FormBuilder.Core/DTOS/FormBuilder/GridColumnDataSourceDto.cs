using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FormBuilder.API.DTOs
{
    public class GridColumnDataSourceDto
    {
        public int Id { get; set; }
        public int ColumnId { get; set; }
        public string ColumnName { get; set; } = string.Empty;
        public string ColumnCode { get; set; } = string.Empty;
        public string GridName { get; set; } = string.Empty;
        public string FormBuilderName { get; set; } = string.Empty;
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
        /// For LookupTable: {"table": "ITEMS", "valueColumn": "Id", "textColumn": "Name", "whereClause": "IsActive = 1"}
        /// For API: {"url": "...", "httpMethod": "GET", "valuePath": "...", "textPath": "...", "requestBodyJson": "..."}
        /// </summary>
        public string? ConfigurationJson { get; set; }
        /// <summary>
        /// Custom array property names to search for in API response (comma-separated or JSON array)
        /// Example: "data,results,items" or ["data", "results", "items"]
        /// If not provided, uses default common names
        /// </summary>
        public List<string>? ArrayPropertyNames { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class CreateGridColumnDataSourceDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ColumnId must be greater than 0")]
        public int ColumnId { get; set; }

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

        [StringLength(200)]
        public string? ValuePath { get; set; }

        [StringLength(200)]
        public string? TextPath { get; set; }

        /// <summary>
        /// JSON configuration for data source
        /// For LookupTable: {"table": "ITEMS", "valueColumn": "Id", "textColumn": "Name", "whereClause": "IsActive = 1"}
        /// For API: {"url": "...", "httpMethod": "GET", "valuePath": "...", "textPath": "...", "requestBodyJson": "..."}
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

    public class UpdateGridColumnDataSourceDto
    {
        [StringLength(50)]
        public string? SourceType { get; set; }

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

        [StringLength(200)]
        public string? ValuePath { get; set; }

        [StringLength(200)]
        public string? TextPath { get; set; }

        /// <summary>
        /// JSON configuration for data source
        /// For LookupTable: {"table": "ITEMS", "valueColumn": "Id", "textColumn": "Name", "whereClause": "IsActive = 1"}
        /// For API: {"url": "...", "httpMethod": "GET", "valuePath": "...", "textPath": "...", "requestBodyJson": "..."}
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

