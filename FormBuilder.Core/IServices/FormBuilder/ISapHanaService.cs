using FormBuilder.API.Models;
using FormBuilder.Core.DTOS.FormBuilder;

namespace FormBuilder.Core.IServices.FormBuilder
{
    /// <summary>
    /// Service interface for SAP HANA database operations
    /// </summary>
    public interface ISapHanaService
    {
        /// <summary>
        /// Test connection to SAP HANA database
        /// </summary>
        Task<ApiResponse> TestConnectionAsync(SapHanaConnectionDto connection);

        /// <summary>
        /// Execute SQL query on SAP HANA database
        /// </summary>
        Task<ApiResponse> ExecuteQueryAsync(SapHanaQueryRequestDto request);

        /// <summary>
        /// Get list of tables from SAP HANA database
        /// </summary>
        Task<ApiResponse> GetTablesAsync(SapHanaGetTablesRequestDto request);

        /// <summary>
        /// Get columns of a specific table from SAP HANA database
        /// </summary>
        Task<ApiResponse> GetTableColumnsAsync(SapHanaGetTableColumnsRequestDto request);
    }
}

