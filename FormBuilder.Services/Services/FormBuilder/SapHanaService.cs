using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Core.IServices.FormBuilder;
using FormBuilder.Application.DTOS;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Odbc;
using FormBuilder.API.Models;
using System;

namespace FormBuilder.Services.Services.FormBuilder
{
    /// <summary>
    /// Service for SAP HANA database operations using ODBC
    /// </summary>
    public class SapHanaService : ISapHanaService
    {
        private readonly ILogger<SapHanaService> _logger;

        public SapHanaService(ILogger<SapHanaService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Build ODBC connection string for SAP HANA
        /// </summary>
        private string BuildOdbcConnectionString(SapHanaConnectionDto connection)
        {
            // Clean server string: remove https://, http://, and trailing slashes
            var serverNode = connection.Server.Trim();
            
            // Remove protocol if present
            if (serverNode.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                serverNode = serverNode.Substring(8);
            }
            else if (serverNode.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                serverNode = serverNode.Substring(7);
            }
            
            // Remove trailing slash
            serverNode = serverNode.TrimEnd('/');
            
            // Driver name must be installed in ODBC Drivers (64-bit): HDBODBC
            return
                "Driver={HDBODBC};" +
                $"ServerNode={serverNode};" +   // e.g., hb152.tyconz.com:30015
                $"UID={connection.UserName};" +
                $"PWD={connection.Password};";
        }

        /// <summary>
        /// Test connection to SAP HANA database using ODBC
        /// </summary>
        public async Task<ApiResponse> TestConnectionAsync(SapHanaConnectionDto connection)
        {
            try
            {
                _logger.LogInformation("Testing SAP HANA ODBC connection to Server: {Server}, Schema: {Schema}, User: {UserName}",
                    connection.Server, connection.Schema, connection.UserName);

                var connectionString = BuildOdbcConnectionString(connection);

                using var conn = new OdbcConnection(connectionString);
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT 1 FROM DUMMY";
                var result = await cmd.ExecuteScalarAsync();

                await conn.CloseAsync();

                _logger.LogInformation("SAP HANA ODBC connection test successful");
                return new ApiResponse(200, "Connection successful", new { connected = true, result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SAP HANA ODBC connection test failed");
                return new ApiResponse(500, $"Connection failed: {ex.Message}", new { connected = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Execute SQL query on SAP HANA database using ODBC
        /// </summary>
        public async Task<ApiResponse> ExecuteQueryAsync(SapHanaQueryRequestDto request)
        {
            try
            {
                _logger.LogInformation("Executing SAP HANA ODBC query. Server: {Server}, Schema: {Schema}, Query length: {QueryLength}",
                    request.Connection.Server, request.Connection.Schema, request.Query?.Length ?? 0);

                var connectionString = BuildOdbcConnectionString(request.Connection);

                var response = new SapHanaQueryResponseDto
                {
                    Success = false,
                    Data = new List<Dictionary<string, object?>>(),
                    Columns = new List<SapHanaColumnInfo>()
                };

                using var conn = new OdbcConnection(connectionString);
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = request.Query;
                cmd.CommandTimeout = 60;

                using var reader = await cmd.ExecuteReaderAsync();

                var columnCount = reader.FieldCount;
                for (int i = 0; i < columnCount; i++)
                {
                    response.Columns.Add(new SapHanaColumnInfo
                    {
                        Name = reader.GetName(i),
                        DataType = reader.GetFieldType(i)?.Name ?? "Unknown"
                    });
                }

                int rowCount = 0;
                int maxRows = request.MaxRows ?? 1000;

                while (await reader.ReadAsync() && rowCount < maxRows)
                {
                    var row = new Dictionary<string, object?>();
                    for (int i = 0; i < columnCount; i++)
                    {
                        var name = reader.GetName(i);
                        row[name] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    }
                    response.Data.Add(row);
                    rowCount++;
                }

                response.RowCount = rowCount;
                response.Success = true;

                _logger.LogInformation("SAP HANA ODBC query executed successfully. Returned {RowCount} rows", rowCount);
                return new ApiResponse(200, "Query executed successfully", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SAP HANA ODBC query execution failed");
                return new ApiResponse(500, $"Query execution failed: {ex.Message}",
                    new SapHanaQueryResponseDto { Success = false, ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Get list of tables from SAP HANA database using ODBC
        /// </summary>
        public async Task<ApiResponse> GetTablesAsync(SapHanaGetTablesRequestDto request)
        {
            try
            {
                _logger.LogInformation("Getting SAP HANA tables via ODBC. Server: {Server}, Schema: {Schema}",
                    request.Connection.Server, request.Connection.Schema);

                var connectionString = BuildOdbcConnectionString(request.Connection);
                var tables = new List<Dictionary<string, object>>();

                using var conn = new OdbcConnection(connectionString);
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT TABLE_NAME, TABLE_TYPE
                    FROM SYS.TABLES
                    WHERE SCHEMA_NAME = ?
                    UNION ALL
                    SELECT VIEW_NAME AS TABLE_NAME, 'VIEW' AS TABLE_TYPE
                    FROM SYS.VIEWS
                    WHERE SCHEMA_NAME = ?
                    ORDER BY TABLE_NAME";

                cmd.Parameters.Add(new OdbcParameter { Value = request.Connection.Schema });
                cmd.Parameters.Add(new OdbcParameter { Value = request.Connection.Schema });

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    tables.Add(new Dictionary<string, object>
                    {
                        { "TABLE_NAME", reader["TABLE_NAME"] ?? "" },
                        { "TABLE_TYPE", reader["TABLE_TYPE"] ?? "" }
                    });
                }

                _logger.LogInformation("Retrieved {Count} tables from SAP HANA via ODBC", tables.Count);
                return new ApiResponse(200, "Tables retrieved successfully", tables);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get SAP HANA tables via ODBC");
                return new ApiResponse(500, $"Failed to get tables: {ex.Message}");
            }
        }

        /// <summary>
        /// Get columns of a specific table from SAP HANA database using ODBC
        /// </summary>
        public async Task<ApiResponse> GetTableColumnsAsync(SapHanaGetTableColumnsRequestDto request)
        {
            try
            {
                _logger.LogInformation("Getting SAP HANA table columns via ODBC. Server: {Server}, Schema: {Schema}, Table: {TableName}",
                    request.Connection.Server, request.Connection.Schema, request.TableName);

                var connectionString = BuildOdbcConnectionString(request.Connection);
                var columns = new List<Dictionary<string, object>>();

                using var conn = new OdbcConnection(connectionString);
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT 
                        TABLE_NAME,
                        COLUMN_NAME,
                        DATA_TYPE_NAME,
                        LENGTH,
                        SCALE,
                        IS_NULLABLE
                    FROM SYS.COLUMNS
                    WHERE SCHEMA_NAME = ? AND TABLE_NAME = ?
                    ORDER BY POSITION";

                cmd.Parameters.Add(new OdbcParameter { Value = request.Connection.Schema });
                cmd.Parameters.Add(new OdbcParameter { Value = request.TableName });

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    columns.Add(new Dictionary<string, object>
                    {
                        { "TABLE_NAME", reader["TABLE_NAME"] ?? "" },
                        { "COLUMN_NAME", reader["COLUMN_NAME"] ?? "" },
                        { "DATA_TYPE_NAME", reader["DATA_TYPE_NAME"] ?? "" },
                        { "LENGTH", reader["LENGTH"] ?? DBNull.Value },
                        { "SCALE", reader["SCALE"] ?? DBNull.Value },
                        { "IS_NULLABLE", reader["IS_NULLABLE"] ?? "" }
                    });
                }

                _logger.LogInformation("Retrieved {Count} columns from SAP HANA table {TableName} via ODBC", columns.Count, request.TableName);
                return new ApiResponse(200, "Columns retrieved successfully", columns);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get SAP HANA table columns via ODBC");
                return new ApiResponse(500, $"Failed to get columns: {ex.Message}");
            }
        }
    }
}
