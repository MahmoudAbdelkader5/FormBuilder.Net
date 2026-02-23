using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Core.IServices.FormBuilder;
using FormBuilder.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SapHanaController : ControllerBase
    {
        private readonly ISapHanaService _sapHanaService;
        private readonly IConfiguration _configuration;
        private readonly ISapHanaConfigsService _sapHanaConfigsService;

        public SapHanaController(
            ISapHanaService sapHanaService,
            IConfiguration configuration,
            ISapHanaConfigsService sapHanaConfigsService)
        {
            _sapHanaService = sapHanaService ?? throw new ArgumentNullException(nameof(sapHanaService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _sapHanaConfigsService = sapHanaConfigsService ?? throw new ArgumentNullException(nameof(sapHanaConfigsService));
        }

        /// <summary>
        /// Test connection to SAP HANA database from database (preferred), fallback to appsettings if DB not configured.
        /// </summary>
        [HttpGet("test-connection-from-config")]
        public async Task<ActionResult<ApiResponse>> TestConnectionFromConfig()
        {
            try
            {
                // Preferred: DB-backed secret (encrypted at rest)
                var connectionString = await _sapHanaConfigsService.GetActiveConnectionStringAsync();

                // Backward compatible fallback (until appsettings is removed)
                connectionString ??= _configuration.GetConnectionString("HanaConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    return BadRequest(new ApiResponse(400, "HanaConnection is not configured (DB or appsettings)"));
                }

                // Parse connection string: Server=...;UserName=...;Current Schema=...;Password=...
                var connection = ParseHanaConnectionString(connectionString);
                if (connection == null)
                {
                    return BadRequest(new ApiResponse(400, "Failed to parse HanaConnection string. Expected format: Server=...;UserName=...;Current Schema=...;Password=..."));
                }

                var result = await _sapHanaService.TestConnectionAsync(connection);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error testing connection: {ex.Message}"));
            }
        }

        /// <summary>
        /// Test connection to SAP HANA database from database only (no appsettings fallback)
        /// </summary>
        [HttpGet("test-connection-from-db")]
        public async Task<ActionResult<ApiResponse>> TestConnectionFromDb()
        {
            try
            {
                var connectionString = await _sapHanaConfigsService.GetActiveConnectionStringAsync();
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    return BadRequest(new ApiResponse(400, "No active SAP HANA connection configured in DB"));
                }

                var connection = ParseHanaConnectionString(connectionString);
                if (connection == null)
                {
                    return BadRequest(new ApiResponse(400, "Failed to parse HanaConnection string from DB. Expected format: Server=...;UserName=...;Current Schema=...;Password=..."));
                }

                var result = await _sapHanaService.TestConnectionAsync(connection);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error testing connection: {ex.Message}"));
            }
        }

        /// <summary>
        /// List available schemas in SAP HANA using the active DB-stored connection (does not require schema to be set in connection string).
        /// Note: In SAP HANA, "schemas" are typically what you choose (not "databases" like SQL Server).
        /// </summary>
        [HttpGet("schemas")]
        public async Task<ActionResult<ApiResponse>> GetSchemas([FromQuery] string? like = null, [FromQuery] int max = 200)
        {
            try
            {
                var connectionString = await _sapHanaConfigsService.GetActiveConnectionStringAsync();
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    return BadRequest(new ApiResponse(400, "No active SAP HANA connection configured in DB"));
                }

                var connection = ParseHanaConnectionString(connectionString);
                if (connection == null)
                {
                    return BadRequest(new ApiResponse(400, "Failed to parse active SAP HANA connection string"));
                }

                // Schemas live in SYS.SCHEMAS. Filter optional.
                max = max <= 0 ? 200 : Math.Min(max, 1000);

                // Sanitize LIKE pattern to avoid breaking the query
                var safeLike = string.IsNullOrWhiteSpace(like)
                    ? null
                    : (like ?? string.Empty).Replace("'", "''");

                var filter = string.IsNullOrWhiteSpace(safeLike)
                    ? string.Empty
                    : " WHERE SCHEMA_NAME LIKE '" + safeLike + "'";

                var query = $"SELECT TOP {max} SCHEMA_NAME FROM SYS.SCHEMAS{filter} ORDER BY SCHEMA_NAME";

                var result = await _sapHanaService.ExecuteQueryAsync(new SapHanaQueryRequestDto
                {
                    Connection = connection,
                    Query = query,
                    MaxRows = max
                });

                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error getting schemas: {ex.Message}"));
            }
        }

        /// <summary>
        /// List tables in a specific SAP HANA schema using the active DB-stored connection.
        /// </summary>
        [HttpGet("tables")]
        public async Task<ActionResult<ApiResponse>> GetTables([FromQuery] string schema, [FromQuery] string? like = null, [FromQuery] int max = 500)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(schema))
                {
                    return BadRequest(new ApiResponse(400, "Schema is required"));
                }

                var connectionString = await _sapHanaConfigsService.GetActiveConnectionStringAsync();
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    return BadRequest(new ApiResponse(400, "No active SAP HANA connection configured in DB"));
                }

                var connection = ParseHanaConnectionString(connectionString);
                if (connection == null)
                {
                    return BadRequest(new ApiResponse(400, "Failed to parse active SAP HANA connection string"));
                }

                max = max <= 0 ? 500 : Math.Min(max, 2000);

                // Sanitize schema and pattern for SQL
                var safeSchema = schema.Replace("'", "''");
                var safeLike = string.IsNullOrWhiteSpace(like)
                    ? null
                    : (like ?? string.Empty).Replace("'", "''");

                var filter = string.IsNullOrWhiteSpace(safeLike)
                    ? string.Empty
                    : " AND TABLE_NAME LIKE '" + safeLike + "'";

                var query =
                    $"SELECT TOP {max} TABLE_NAME " +
                    $"FROM SYS.TABLES " +
                    $"WHERE SCHEMA_NAME = '{safeSchema}'{filter} " +
                    "ORDER BY TABLE_NAME";

                var result = await _sapHanaService.ExecuteQueryAsync(new SapHanaQueryRequestDto
                {
                    Connection = connection,
                    Query = query,
                    MaxRows = max
                });

                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error getting tables: {ex.Message}"));
            }
        }

        /// <summary>
        /// List columns of a specific table in a schema using the active SAP HANA connection.
        /// </summary>
        [HttpGet("table-columns")]
        public async Task<ActionResult<ApiResponse>> GetTableColumnsSimple(
            [FromQuery] string schema,
            [FromQuery] string table,
            [FromQuery] int max = 500)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(schema) || string.IsNullOrWhiteSpace(table))
                {
                    return BadRequest(new ApiResponse(400, "Schema and table are required"));
                }

                var connectionString = await _sapHanaConfigsService.GetActiveConnectionStringAsync();
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    return BadRequest(new ApiResponse(400, "No active SAP HANA connection configured in DB"));
                }

                var connection = ParseHanaConnectionString(connectionString);
                if (connection == null)
                {
                    return BadRequest(new ApiResponse(400, "Failed to parse active SAP HANA connection string"));
                }

                max = max <= 0 ? 500 : Math.Min(max, 2000);

                var safeSchema = schema.Replace("'", "''");
                var safeTable = table.Replace("'", "''");

                // Use SYS.TABLE_COLUMNS to include both tables and views; HANA stores names in uppercase
                var query =
                    $"SELECT TOP {max} COLUMN_NAME, DATA_TYPE_NAME, POSITION " +
                    $"FROM SYS.TABLE_COLUMNS " +
                    $"WHERE UPPER(SCHEMA_NAME) = UPPER('{safeSchema}') AND UPPER(TABLE_NAME) = UPPER('{safeTable}') " +
                    "ORDER BY POSITION";

                var result = await _sapHanaService.ExecuteQueryAsync(new SapHanaQueryRequestDto
                {
                    Connection = connection,
                    Query = query,
                    MaxRows = max
                });

                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error getting table columns: {ex.Message}"));
            }
        }

        /// <summary>
        /// Parse HANA connection string from appsettings format to DTO
        /// </summary>
        private SapHanaConnectionDto? ParseHanaConnectionString(string connectionString)
        {
            try
            {
                var parts = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries);
                var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                foreach (var part in parts)
                {
                    var keyValue = part.Split('=', 2);
                    if (keyValue.Length == 2)
                    {
                        dict[keyValue[0].Trim()] = keyValue[1].Trim();
                    }
                }

                if (!dict.ContainsKey("Server") || !dict.ContainsKey("UserName") || !dict.ContainsKey("Password"))
                {
                    return null;
                }

                return new SapHanaConnectionDto
                {
                    Server = dict["Server"],
                    UserName = dict["UserName"],
                    Password = dict["Password"],
                    Schema = dict.ContainsKey("Current Schema") ? dict["Current Schema"] : dict.ContainsKey("Schema") ? dict["Schema"] : string.Empty
                };
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Test connection to SAP HANA database
        /// </summary>
        [HttpPost("test-connection")]
        public async Task<ActionResult<ApiResponse>> TestConnection([FromBody] SapHanaTestConnectionDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse(400, "Invalid connection data", ModelState));
                }

                var result = await _sapHanaService.TestConnectionAsync(request.Connection);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error testing connection: {ex.Message}"));
            }
        }

        /// <summary>
        /// Execute SQL query on SAP HANA database
        /// </summary>
        [HttpPost("execute-query")]
        public async Task<ActionResult<ApiResponse>> ExecuteQuery([FromBody] SapHanaQueryRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse(400, "Invalid query request", ModelState));
                }

                if (string.IsNullOrWhiteSpace(request.Query))
                {
                    return BadRequest(new ApiResponse(400, "Query is required"));
                }

                var result = await _sapHanaService.ExecuteQueryAsync(request);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error executing query: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get list of tables from SAP HANA database
        /// </summary>
        [HttpPost("tables")]
        public async Task<ActionResult<ApiResponse>> GetTables([FromBody] SapHanaGetTablesRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse(400, "Invalid request", ModelState));
                }

                var result = await _sapHanaService.GetTablesAsync(request);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error getting tables: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get columns of a specific table from SAP HANA database
        /// </summary>
        [HttpPost("table-columns")]
        public async Task<ActionResult<ApiResponse>> GetTableColumns([FromBody] SapHanaGetTableColumnsRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse(400, "Invalid request", ModelState));
                }

                if (string.IsNullOrWhiteSpace(request.TableName))
                {
                    return BadRequest(new ApiResponse(400, "Table name is required"));
                }

                var result = await _sapHanaService.GetTableColumnsAsync(request);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Error getting table columns: {ex.Message}"));
            }
        }
    }
}

