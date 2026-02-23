namespace FormBuilder.Core.DTOS.FormBuilder
{
    /// <summary>
    /// Request DTO for inspecting API structure
    /// </summary>
    public class InspectApiRequestDto
    {
        public string ApiUrl { get; set; } = string.Empty;
        /// <summary>
        /// API Endpoint Path (e.g., "products", "users", "results")
        /// Combined with ApiUrl (Base URL) to form full URL: ApiUrl + ApiPath
        /// </summary>
        public string? ApiPath { get; set; }
        public string? HttpMethod { get; set; } = "GET";
        public string? RequestBodyJson { get; set; }
        /// <summary>
        /// Custom array property names to search for in API response (comma-separated)
        /// Example: "data,results,items,users" or ["data", "results", "items"]
        /// If not provided, uses default common names
        /// </summary>
        public List<string>? ArrayPropertyNames { get; set; }
    }

    /// <summary>
    /// Response DTO showing available fields in API response
    /// </summary>
    public class ApiInspectionResponseDto
    {
        public string FullUrl { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public int? ItemsCount { get; set; }
        public List<string> AvailableFields { get; set; } = new List<string>();
        public List<string> NestedFields { get; set; } = new List<string>();
        public object? SampleItem { get; set; }
        public string? RawResponse { get; set; }
        public List<string> SuggestedValuePaths { get; set; } = new List<string>();
        public List<string> SuggestedTextPaths { get; set; } = new List<string>();
    }
}

