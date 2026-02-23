using formBuilder.Domian.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Domian.Entitys.FormBuilder
{
    [Table("GRID_COLUMN_DATA_SOURCES")]
    public class GRID_COLUMN_DATA_SOURCES : BaseEntity
    {
        [ForeignKey("FORM_GRID_COLUMNS")]
        public int ColumnId { get; set; }
        public virtual FORM_GRID_COLUMNS FORM_GRID_COLUMNS { get; set; }

        [Required, StringLength(50)]
        public string SourceType { get; set; } = string.Empty; // "Static", "LookupTable", "API"

        /// <summary>
        /// Base URL for API data source
        /// Example: "https://dummyjson.com/"
        /// </summary>
        [StringLength(500)]
        public string? ApiUrl { get; set; }

        /// <summary>
        /// API Endpoint Path (e.g., "products", "users", "results")
        /// Combined with ApiUrl (Base URL) to form full URL: ApiUrl + ApiPath
        /// Example: ApiUrl = "https://dummyjson.com/", ApiPath = "products" -> Full URL = "https://dummyjson.com/products"
        /// </summary>
        [StringLength(200)]
        public string? ApiPath { get; set; }

        [StringLength(10)]
        public string? HttpMethod { get; set; }

        public string? RequestBodyJson { get; set; }

        /// <summary>
        /// JSON path to extract the value from API response
        /// Example: "id" or "data.id"
        /// </summary>
        [StringLength(200)]
        public string? ValuePath { get; set; }

        /// <summary>
        /// JSON path to extract the display text from API response
        /// Example: "name" or "data.name"
        /// </summary>
        [StringLength(200)]
        public string? TextPath { get; set; }

        /// <summary>
        /// JSON configuration for data source
        /// For LookupTable: {"table": "ITEMS", "valueColumn": "Id", "textColumn": "Name", "whereClause": "IsActive = 1"}
        /// For API: {"url": "...", "httpMethod": "GET", "valuePath": "...", "textPath": "...", "requestBodyJson": "...", "arrayPropertyNames": ["data", "results"]}
        /// For Static: options are stored in GRID_COLUMN_OPTIONS table
        /// </summary>
        public string? ConfigurationJson { get; set; }

        /// <summary>
        /// Custom array property names to search for in API response (comma-separated or JSON array)
        /// Example: "data,results,items" or ["data", "results", "items"]
        /// If not provided, uses default common names
        /// </summary>
        [StringLength(500)]
        public string? ArrayPropertyNames { get; set; }

        public new bool IsActive { get; set; } = true;
    }
}

