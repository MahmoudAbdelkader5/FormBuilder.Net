using formBuilder.Domian.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Domian.Entitys.froms
{

    [Table("FIELD_DATA_SOURCES")]
    public class FIELD_DATA_SOURCES : BaseEntity
    {
       

        [ForeignKey("FORM_FIELDS")]
        public int FieldId { get; set; }
        public virtual FORM_FIELDS FORM_FIELDS { get; set; }

        [Required, StringLength(50)]
        public string SourceType { get; set; }

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
        public string? ValuePath { get; set; }
        public string? TextPath { get; set; }
        
        /// <summary>
        /// JSON configuration for data source
        /// For LookupTable: {"table": "CUSTOMERS", "valueColumn": "Id", "textColumn": "Name"}
        /// For API: {"url": "...", "httpMethod": "GET", "valuePath": "...", "textPath": "...", "requestBodyJson": "..."}
        /// </summary>
        public string? ConfigurationJson { get; set; }
        
        public new bool IsActive { get; set; }
    }
}
