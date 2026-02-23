using FormBuilder.Core.Models;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domian.Interfaces;
using FormBuilder.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FormBuilder.Services.Services.FormBuilder
{
    /// <summary>
    /// Service for executing Stored Procedures for Form Rules
    /// </summary>
    public class StoredProcedureService
    {
        private readonly FormBuilderDbContext _formBuilderDbContext;
        private readonly AkhmanageItContext _akhmanageItContext;
        private readonly ILogger<StoredProcedureService> _logger;
        private readonly IFormStoredProceduresRepository? _storedProceduresRepository;

        public StoredProcedureService(
            FormBuilderDbContext formBuilderDbContext,
            AkhmanageItContext akhmanageItContext,
            ILogger<StoredProcedureService> logger,
            IFormStoredProceduresRepository? storedProceduresRepository = null)
        {
            _formBuilderDbContext = formBuilderDbContext ?? throw new ArgumentNullException(nameof(formBuilderDbContext));
            _akhmanageItContext = akhmanageItContext ?? throw new ArgumentNullException(nameof(akhmanageItContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _storedProceduresRepository = storedProceduresRepository;
        }

        /// <summary>
        /// Execute a Stored Procedure by ID (from Whitelist) and return the result as a boolean
        /// </summary>
        /// <param name="storedProcedureId">ID of the stored procedure from FORM_STORED_PROCEDURES</param>
        /// <param name="parameters">Dictionary of parameter names and values</param>
        /// <param name="resultMapping">JSON mapping for interpreting the result (optional, uses default from whitelist if not provided)</param>
        /// <returns>Boolean result indicating if the condition is true</returns>
        public async Task<bool> ExecuteStoredProcedureByIdAsync(
            int storedProcedureId,
            Dictionary<string, object> parameters,
            string? resultMapping = null)
        {
            if (_storedProceduresRepository == null)
            {
                throw new InvalidOperationException("StoredProceduresRepository is not configured. Please register IFormStoredProceduresRepository in DI container.");
            }

            // Get stored procedure from whitelist
            var storedProcedure = await _storedProceduresRepository.SingleOrDefaultAsync(
                sp => sp.Id == storedProcedureId && sp.IsActive && !sp.IsDeleted);

            if (storedProcedure == null)
            {
                throw new ArgumentException($"Stored procedure with ID {storedProcedureId} not found in whitelist or is not active", nameof(storedProcedureId));
            }

            // Use provided result mapping
            var finalResultMapping = resultMapping;

            // Extract procedure name from ProcedureCode if ProcedureName is not set
            var procedureName = storedProcedure.ProcedureName;
            if (string.IsNullOrWhiteSpace(procedureName) && !string.IsNullOrWhiteSpace(storedProcedure.ProcedureCode))
            {
                procedureName = ExtractProcedureNameFromCode(storedProcedure.ProcedureCode);
            }

            if (string.IsNullOrWhiteSpace(procedureName))
            {
                throw new InvalidOperationException($"Cannot determine procedure name for stored procedure ID {storedProcedureId}. Either ProcedureName or ProcedureCode must be provided.");
            }

            // Build full procedure name with schema
            var fullProcedureName = string.IsNullOrWhiteSpace(storedProcedure.SchemaName) || storedProcedure.SchemaName == "dbo"
                ? procedureName
                : $"{storedProcedure.SchemaName}.{procedureName}";

            _logger.LogInformation("Executing whitelisted stored procedure: {ProcedureName} (ID: {Id}) from database: {Database}",
                fullProcedureName, storedProcedureId, storedProcedure.DatabaseName);

            return await ExecuteStoredProcedureAsync(
                fullProcedureName,
                storedProcedure.DatabaseName,
                parameters,
                finalResultMapping);
        }

        /// <summary>
        /// Execute a Stored Procedure by ID (from Whitelist) and return debug information (raw output/return/first row).
        /// Intended for troubleshooting only.
        /// </summary>
        public async Task<StoredProcedureExecutionDebug> ExecuteStoredProcedureByIdWithDebugAsync(
            int storedProcedureId,
            Dictionary<string, object> parameters,
            string? resultMapping = null)
        {
            if (_storedProceduresRepository == null)
            {
                throw new InvalidOperationException("StoredProceduresRepository is not configured. Please register IFormStoredProceduresRepository in DI container.");
            }

            var storedProcedure = await _storedProceduresRepository.SingleOrDefaultAsync(
                sp => sp.Id == storedProcedureId && sp.IsActive && !sp.IsDeleted);

            if (storedProcedure == null)
            {
                throw new ArgumentException($"Stored procedure with ID {storedProcedureId} not found in whitelist or is not active", nameof(storedProcedureId));
            }

            var procedureName = storedProcedure.ProcedureName;
            if (string.IsNullOrWhiteSpace(procedureName) && !string.IsNullOrWhiteSpace(storedProcedure.ProcedureCode))
            {
                procedureName = ExtractProcedureNameFromCode(storedProcedure.ProcedureCode);
            }

            if (string.IsNullOrWhiteSpace(procedureName))
            {
                throw new InvalidOperationException($"Cannot determine procedure name for stored procedure ID {storedProcedureId}. Either ProcedureName or ProcedureCode must be provided.");
            }

            var fullProcedureName = string.IsNullOrWhiteSpace(storedProcedure.SchemaName) || storedProcedure.SchemaName == "dbo"
                ? procedureName
                : $"{storedProcedure.SchemaName}.{procedureName}";

            _logger.LogInformation("Executing whitelisted stored procedure (debug): {ProcedureName} (ID: {Id}) from database: {Database}",
                fullProcedureName, storedProcedureId, storedProcedure.DatabaseName);

            // For by-ID calls, whitelist is already enforced by lookup above
            return await ExecuteStoredProcedureWithDebugAsync(
                fullProcedureName,
                storedProcedure.DatabaseName,
                parameters,
                resultMapping,
                skipWhitelistCheck: true);
        }

        /// <summary>
        /// Execute a Stored Procedure and return the result as a boolean
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure</param>
        /// <param name="databaseName">Database name: "FormBuilder" or "AKHManageIT"</param>
        /// <param name="parameters">Dictionary of parameter names and values</param>
        /// <param name="resultMapping">JSON mapping for interpreting the result</param>
        /// <param name="skipWhitelistCheck">Skip whitelist validation (for backward compatibility with old rules)</param>
        /// <returns>Boolean result indicating if the condition is true</returns>
        public async Task<bool> ExecuteStoredProcedureAsync(
            string procedureName,
            string databaseName,
            Dictionary<string, object> parameters,
            string? resultMapping = null,
            bool skipWhitelistCheck = false)
        {
            if (string.IsNullOrWhiteSpace(procedureName))
            {
                throw new ArgumentException("Procedure name is required", nameof(procedureName));
            }

            if (string.IsNullOrWhiteSpace(databaseName))
            {
                throw new ArgumentException("Database name is required", nameof(databaseName));
            }

            // Validate procedure name to prevent SQL injection
            if (!IsValidStoredProcedureName(procedureName))
            {
                throw new ArgumentException($"Invalid stored procedure name: {procedureName}", nameof(procedureName));
            }

            // Check whitelist if repository is available and check is not skipped
            if (!skipWhitelistCheck && _storedProceduresRepository != null)
            {
                // Extract schema and procedure name
                var parts = procedureName.Split('.');
                var schemaName = parts.Length > 1 ? parts[0] : "dbo";
                var procName = parts.Length > 1 ? parts[1] : parts[0];

                var whitelisted = await _storedProceduresRepository.GetByDatabaseSchemaAndProcedureAsync(
                    databaseName, schemaName, procName);

                if (whitelisted == null || !whitelisted.IsActive || whitelisted.IsDeleted)
                {
                    throw new UnauthorizedAccessException(
                        $"Stored procedure '{procedureName}' in database '{databaseName}' is not in the whitelist or is not active. " +
                        "Please add it to FORM_STORED_PROCEDURES first.");
                }

                _logger.LogInformation("Executing whitelisted stored procedure: {ProcedureName} from database: {Database}",
                    procedureName, databaseName);
            }
            else if (!skipWhitelistCheck)
            {
                _logger.LogWarning("StoredProceduresRepository not available. Whitelist check skipped. This is less secure.");
            }

            DbConnection? connection = null;
            try
            {
                // Get the appropriate database context
                DbContext context = databaseName.Equals("FormBuilder", StringComparison.OrdinalIgnoreCase)
                    ? _formBuilderDbContext
                    : _akhmanageItContext;

                connection = context.Database.GetDbConnection();
                
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = procedureName;
                command.CommandTimeout = 30; // 30 seconds timeout

                // Derive parameters from stored procedure metadata
                // This will automatically detect OUTPUT parameters
                if (command is SqlCommand sqlCommand)
                {
                    try
                    {
                        SqlCommandBuilder.DeriveParameters(sqlCommand);
                        _logger.LogInformation("Derived {Count} parameters from stored procedure '{ProcedureName}': {Params}",
                            sqlCommand.Parameters.Count, procedureName,
                            string.Join(", ", sqlCommand.Parameters.Cast<SqlParameter>().Select(p => 
                                $"{p.ParameterName}({p.Direction}, {p.SqlDbType})")));
                    }
                    catch (Exception deriveEx)
                    {
                        _logger.LogError(deriveEx, "Failed to derive parameters for '{ProcedureName}'. Will try manual parameter setup.", procedureName);
                        // Continue with manual parameter setup
                    }
                }

                // Add/update input parameter values from provided parameters
                if (parameters != null && parameters.Any())
                {
                    foreach (var param in parameters)
                    {
                        var paramName = param.Key.StartsWith("@") ? param.Key : "@" + param.Key;
                        var existingParam = command.Parameters.Cast<DbParameter>()
                            .FirstOrDefault(p => p.ParameterName.Equals(paramName, StringComparison.OrdinalIgnoreCase));
                        
                        if (existingParam != null)
                        {
                            // Update existing parameter value
                            var normalized = NormalizeParameterValue(param.Value);
                            existingParam.Value = normalized ?? DBNull.Value;
                            _logger.LogDebug("Updated parameter '{ParamName}' = {Value} ({Type})", paramName, normalized, normalized?.GetType().Name ?? "null");
                        }
                        else
                        {
                            // Add new parameter (if not derived)
                            var dbParam = command.CreateParameter();
                            dbParam.ParameterName = paramName;
                            var normalized = NormalizeParameterValue(param.Value);
                            dbParam.Value = normalized ?? DBNull.Value;
                            dbParam.Direction = ParameterDirection.Input;
                            command.Parameters.Add(dbParam);
                            _logger.LogDebug("Added parameter '{ParamName}' = {Value} ({Type})", paramName, normalized, normalized?.GetType().Name ?? "null");
                        }
                    }
                }
                
                // CRITICAL FIX: For InputOutput parameters, SQL Server requires an initial value
                // Even if DeriveParameters added them, we MUST set a value (even if NULL/false)
                // Otherwise SQL Server throws "parameter was not supplied" error
                foreach (var param in command.Parameters.Cast<DbParameter>())
                {
                    if (param.Direction == ParameterDirection.InputOutput)
                    {
                        // InputOutput parameters MUST have a value set, even if it's just a default
                        if (param.Value == null || param.Value == DBNull.Value)
                        {
                            if (param.DbType == DbType.Boolean)
                            {
                                param.Value = false;
                                _logger.LogInformation("Set default value (false) for InputOutput parameter '{ParamName}' (was null)", param.ParameterName);
                            }
                            else if (param.DbType == DbType.Int32 || param.DbType == DbType.Int64 || param.DbType == DbType.Decimal)
                            {
                                param.Value = 0;
                                _logger.LogInformation("Set default value (0) for InputOutput parameter '{ParamName}' (was null)", param.ParameterName);
                            }
                            else
                            {
                                param.Value = DBNull.Value;
                                _logger.LogInformation("Set DBNull for InputOutput parameter '{ParamName}' (was null)", param.ParameterName);
                            }
                        }
                        else
                        {
                            _logger.LogDebug("InputOutput parameter '{ParamName}' already has value: {Value}", param.ParameterName, param.Value);
                        }
                    }
                }

                // Check for OUTPUT parameters (after deriving)
                var outputParams = command.Parameters.Cast<DbParameter>()
                    .Where(p => p.Direction == ParameterDirection.Output || 
                               p.Direction == ParameterDirection.InputOutput)
                    .ToList();

                _logger.LogDebug("Found {Count} OUTPUT parameters after derivation: {Params}",
                    outputParams.Count,
                    string.Join(", ", outputParams.Select(p => $"{p.ParameterName}({p.Direction})")));

                // ALWAYS ensure we have an OUTPUT parameter - DeriveParameters might not work correctly
                // Try to add OUTPUT parameter based on resultMapping or common names
                string? outputParamName = null;
                if (!string.IsNullOrWhiteSpace(resultMapping))
                {
                    try
                    {
                        var mapping = JsonSerializer.Deserialize<ResultMapping>(resultMapping);
                        if (mapping != null && !string.IsNullOrWhiteSpace(mapping.ResultColumn))
                        {
                            outputParamName = mapping.ResultColumn.StartsWith("@") 
                                ? mapping.ResultColumn 
                                : "@" + mapping.ResultColumn;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogDebug(ex, "Failed to parse resultMapping for OUTPUT parameter name");
                    }
                }
                
                // Fallback to common OUTPUT parameter names
                var commonOutputNames = new[] { outputParamName, "@IsBlocked", "@Result", "@Blocked", "@IsValid" }
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .Distinct()
                    .ToArray();
                
                // If no OUTPUT params found, or if the expected OUTPUT param doesn't exist, add it
                bool needToAddOutput = !outputParams.Any();
                if (!needToAddOutput && !string.IsNullOrWhiteSpace(outputParamName))
                {
                    // Check if the expected OUTPUT param exists
                    needToAddOutput = !outputParams.Any(p => p.ParameterName.Equals(outputParamName, StringComparison.OrdinalIgnoreCase));
                }
                
                if (needToAddOutput)
                {
                    _logger.LogInformation("Ensuring OUTPUT parameter exists. Trying names: {Names}", string.Join(", ", commonOutputNames));
                    
                    foreach (var outputName in commonOutputNames)
                    {
                        var existingParam = command.Parameters.Cast<DbParameter>()
                            .FirstOrDefault(p => p.ParameterName.Equals(outputName, StringComparison.OrdinalIgnoreCase));
                        
                        if (existingParam != null)
                        {
                            // Parameter exists, ensure it's marked as OUTPUT
                            if (existingParam.Direction != ParameterDirection.Output && 
                                existingParam.Direction != ParameterDirection.InputOutput)
                            {
                                existingParam.Direction = ParameterDirection.Output;
                                _logger.LogInformation("Changed parameter '{ParamName}' direction to OUTPUT", outputName);
                            }
                            if (existingParam.DbType == DbType.Object || existingParam.DbType == DbType.String)
                            {
                                existingParam.DbType = DbType.Boolean;
                            }
                            if (!outputParams.Contains(existingParam))
                            {
                                outputParams.Add(existingParam);
                            }
                            _logger.LogInformation("Using existing parameter '{ParamName}' as OUTPUT", outputName);
                            break;
                        }
                        else
                        {
                            // Parameter doesn't exist, add it as OUTPUT
                            var dbParam = command.CreateParameter();
                            dbParam.ParameterName = outputName;
                            dbParam.DbType = DbType.Boolean;
                            dbParam.Direction = ParameterDirection.Output;
                            command.Parameters.Add(dbParam);
                            outputParams.Add(dbParam);
                            _logger.LogInformation("Added OUTPUT parameter '{ParamName}' manually before execution", outputName);
                            break;
                        }
                    }
                }

                // If SP has OUTPUT parameter, use ExecuteNonQuery and read OUTPUT parameter
                if (outputParams.Any())
                {
                    _logger.LogInformation("Stored procedure '{ProcedureName}' has {Count} OUTPUT parameter(s): {Params}",
                        procedureName, outputParams.Count, string.Join(", ", outputParams.Select(p => $"{p.ParameterName}({p.DbType})")));
                    
                    // Pick the most relevant OUTPUT parameter:
                    // 1) Prefer parameter matching resultMapping.resultColumn (e.g., "IsBlocked" or "@IsBlocked")
                    // 2) Otherwise prefer common names (IsBlocked/Result/Blocked/IsValid)
                    // 3) Fallback to the first OUTPUT parameter
                    var outputParam = SelectBestOutputParameter(outputParams, resultMapping);
                    _logger.LogInformation("Selected OUTPUT parameter: '{ParamName}' (DbType: {DbType}, Direction: {Direction})",
                        outputParam.ParameterName, outputParam.DbType, outputParam.Direction);
                    
                    // Execute stored procedure
                    _logger.LogDebug("Executing stored procedure '{ProcedureName}' with ExecuteNonQuery...", procedureName);
                    await command.ExecuteNonQueryAsync();
                    _logger.LogDebug("Stored procedure '{ProcedureName}' execution completed.", procedureName);

                    // Log all OUTPUT parameter values after execution
                    foreach (var op in outputParams)
                    {
                        _logger.LogInformation("OUTPUT parameter '{ParamName}' after execution: Value={Value} (Type: {Type}, IsNull: {IsNull}, IsDBNull: {IsDBNull})",
                            op.ParameterName, op.Value, op.Value?.GetType().Name ?? "null", op.Value == null, op.Value == DBNull.Value);
                    }

                    // Prefer: selected OUTPUT parameter -> any other OUTPUT parameter -> RETURN VALUE
                    var returnParams = command.Parameters.Cast<DbParameter>()
                        .Where(p => p.Direction == ParameterDirection.ReturnValue)
                        .ToList();

                    if (returnParams.Any())
                    {
                        foreach (var rp in returnParams)
                        {
                            _logger.LogInformation("RETURN parameter '{ParamName}' after execution: Value={Value}",
                                rp.ParameterName, rp.Value);
                        }
                    }

                    var candidates = new List<DbParameter>();
                    candidates.Add(outputParam);
                    candidates.AddRange(outputParams.Where(p => !ReferenceEquals(p, outputParam)));
                    candidates.AddRange(returnParams);

                    foreach (var candidate in candidates)
                    {
                        if (candidate.Value != null && candidate.Value != DBNull.Value)
                        {
                            var candidateBool = ConvertToBoolean(candidate.Value);
                            _logger.LogInformation("✓ Using parameter '{ParamName}' ({Direction}): RawValue={RawValue} -> BoolValue={BoolValue}",
                                candidate.ParameterName, candidate.Direction, candidate.Value, candidateBool);
                            return candidateBool;
                        }
                        else
                        {
                            _logger.LogWarning("✗ Parameter '{ParamName}' ({Direction}) is null/DBNull, skipping",
                                candidate.ParameterName, candidate.Direction);
                        }
                    }

                    _logger.LogWarning("Stored procedure '{ProcedureName}' returned OUTPUT/RETURN parameters but all were null/DBNull. Falling back to resultset parsing if available.",
                        procedureName);
                    // Fall through to resultset parsing attempt (some procs return both OUTPUT params and a SELECT).
                }

                // Execute and get result from DataReader (for SPs that return rows)
                using var reader = await command.ExecuteReaderAsync();
                
                if (!reader.HasRows)
                {
                    _logger.LogWarning("Stored procedure '{ProcedureName}' returned no rows", procedureName);
                    return false;
                }

                // Read first row
                await reader.ReadAsync();

                // Parse result based on resultMapping
                bool readerResult = ParseStoredProcedureResult(reader, resultMapping);

                return readerResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing stored procedure '{ProcedureName}' on database '{DatabaseName}'", 
                    procedureName, databaseName);
                throw;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }
        }

        /// <summary>
        /// Execute a Stored Procedure and return a boolean along with raw outputs for debugging.
        /// </summary>
        public async Task<StoredProcedureExecutionDebug> ExecuteStoredProcedureWithDebugAsync(
            string procedureName,
            string databaseName,
            Dictionary<string, object> parameters,
            string? resultMapping = null,
            bool skipWhitelistCheck = false)
        {
            DbConnection? connection = null;
            var debug = new StoredProcedureExecutionDebug();

            try
            {
                if (string.IsNullOrWhiteSpace(procedureName))
                    throw new ArgumentException("Procedure name is required", nameof(procedureName));
                if (string.IsNullOrWhiteSpace(databaseName))
                    throw new ArgumentException("Database name is required", nameof(databaseName));
                if (!IsValidStoredProcedureName(procedureName))
                    throw new ArgumentException($"Invalid stored procedure name: {procedureName}", nameof(procedureName));

                // Get the appropriate database context
                DbContext context = databaseName.Equals("FormBuilder", StringComparison.OrdinalIgnoreCase)
                    ? _formBuilderDbContext
                    : _akhmanageItContext;

                connection = context.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = procedureName;
                command.CommandTimeout = 30;

                bool deriveSucceeded = false;
                if (command is SqlCommand sqlCommand)
                {
                    try
                    {
                        // Clear any existing parameters first
                        sqlCommand.Parameters.Clear();
                        SqlCommandBuilder.DeriveParameters(sqlCommand);
                        deriveSucceeded = true;
                        _logger.LogInformation("DEBUG: Derived {Count} parameters from SP metadata: {Params}",
                            sqlCommand.Parameters.Count,
                            string.Join(", ", sqlCommand.Parameters.Cast<SqlParameter>().Select(p => 
                                $"{p.ParameterName}({p.Direction}, {p.SqlDbType})")));
                    }
                    catch (Exception deriveEx)
                    {
                        _logger.LogError(deriveEx, "DEBUG: Failed to derive parameters using SqlCommandBuilder. Error: {Error}. Will add parameters manually.",
                            deriveEx.Message);
                        deriveSucceeded = false;
                        // Continue - we'll add parameters manually
                    }
                }
                else
                {
                    _logger.LogWarning("DEBUG: Command is not SqlCommand, cannot use DeriveParameters. Will add parameters manually.");
                    deriveSucceeded = false;
                }
                
                // If DeriveParameters failed or didn't add OUTPUT params, we MUST add them manually
                if (!deriveSucceeded)
                {
                    _logger.LogWarning("DEBUG: DeriveParameters failed or not available. Will add all parameters manually including OUTPUT.");
                }

                if (parameters != null && parameters.Any())
                {
                    foreach (var param in parameters)
                    {
                        var paramName = param.Key.StartsWith("@") ? param.Key : "@" + param.Key;
                        var existingParam = command.Parameters.Cast<DbParameter>()
                            .FirstOrDefault(p => p.ParameterName.Equals(paramName, StringComparison.OrdinalIgnoreCase));

                        if (existingParam != null)
                        {
                            var normalized = NormalizeParameterValue(param.Value);
                            existingParam.Value = normalized ?? DBNull.Value;
                            _logger.LogDebug("DEBUG: Set input parameter '{ParamName}' = {Value}", paramName, normalized);
                        }
                        else
                        {
                            var dbParam = command.CreateParameter();
                            dbParam.ParameterName = paramName;
                            var normalized = NormalizeParameterValue(param.Value);
                            dbParam.Value = normalized ?? DBNull.Value;
                            dbParam.Direction = ParameterDirection.Input;
                            command.Parameters.Add(dbParam);
                            _logger.LogDebug("DEBUG: Added input parameter '{ParamName}' = {Value}", paramName, normalized);
                        }
                    }
                }
                
                // CRITICAL FIX: For InputOutput parameters, SQL Server requires an initial value
                // Even if DeriveParameters added them, we MUST set a value (even if NULL/false)
                // Otherwise SQL Server throws "parameter was not supplied" error
                foreach (var param in command.Parameters.Cast<DbParameter>())
                {
                    if (param.Direction == ParameterDirection.InputOutput)
                    {
                        // InputOutput parameters MUST have a value set, even if it's just a default
                        // SQL Server treats InputOutput params as required inputs
                        if (param.Value == null || param.Value == DBNull.Value)
                        {
                            if (param.DbType == DbType.Boolean)
                            {
                                param.Value = false;
                                _logger.LogInformation("DEBUG: Set default value (false) for InputOutput parameter '{ParamName}' (was null)", param.ParameterName);
                            }
                            else if (param.DbType == DbType.Int32 || param.DbType == DbType.Int64 || param.DbType == DbType.Decimal)
                            {
                                param.Value = 0;
                                _logger.LogInformation("DEBUG: Set default value (0) for InputOutput parameter '{ParamName}' (was null)", param.ParameterName);
                            }
                            else
                            {
                                param.Value = DBNull.Value;
                                _logger.LogInformation("DEBUG: Set DBNull for InputOutput parameter '{ParamName}' (was null)", param.ParameterName);
                            }
                        }
                        else
                        {
                            _logger.LogDebug("DEBUG: InputOutput parameter '{ParamName}' already has value: {Value}", param.ParameterName, param.Value);
                        }
                    }
                }

                var outputParams = command.Parameters.Cast<DbParameter>()
                    .Where(p => p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput)
                    .ToList();

                _logger.LogInformation("DEBUG: After DeriveParameters and input params, found {Count} OUTPUT params: {Params}",
                    outputParams.Count, string.Join(", ", outputParams.Select(p => $"{p.ParameterName}({p.Direction})")));

                // ALWAYS ensure we have an OUTPUT parameter (same logic as ExecuteStoredProcedureAsync)
                // This is critical - SQL Server requires OUTPUT parameters to be declared before execution
                string? outputParamName = null;
                if (!string.IsNullOrWhiteSpace(resultMapping))
                {
                    try
                    {
                        var mapping = JsonSerializer.Deserialize<ResultMapping>(resultMapping);
                        if (mapping != null && !string.IsNullOrWhiteSpace(mapping.ResultColumn))
                        {
                            outputParamName = mapping.ResultColumn.StartsWith("@") 
                                ? mapping.ResultColumn 
                                : "@" + mapping.ResultColumn;
                            _logger.LogInformation("DEBUG: Extracted OUTPUT parameter name from resultMapping: '{ParamName}'", outputParamName);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "DEBUG: Failed to parse resultMapping for OUTPUT parameter name");
                    }
                }
                
                var commonOutputNames = new[] { outputParamName, "@IsBlocked", "@Result", "@Blocked", "@IsValid" }
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .Distinct()
                    .ToArray();
                
                _logger.LogInformation("DEBUG: Checking OUTPUT parameters. Found: {Count}, Will try names: {Names}",
                    outputParams.Count, string.Join(", ", commonOutputNames));
                
                bool needToAddOutput = !outputParams.Any();
                if (!needToAddOutput && !string.IsNullOrWhiteSpace(outputParamName))
                {
                    needToAddOutput = !outputParams.Any(p => p.ParameterName.Equals(outputParamName, StringComparison.OrdinalIgnoreCase));
                    _logger.LogInformation("DEBUG: OUTPUT params exist but expected '{ParamName}' not found. NeedToAdd={NeedToAdd}",
                        outputParamName, needToAddOutput);
                }
                
                // CRITICAL: Always ensure OUTPUT parameter exists before execution
                // SQL Server will throw error if OUTPUT parameter is missing
                if (needToAddOutput || outputParams.Count == 0)
                {
                    _logger.LogWarning("DEBUG: No OUTPUT parameters found! Ensuring OUTPUT parameter exists. Trying names: {Names}", 
                        string.Join(", ", commonOutputNames));
                    
                    foreach (var outputName in commonOutputNames)
                    {
                        var existingParam = command.Parameters.Cast<DbParameter>()
                            .FirstOrDefault(p => p.ParameterName.Equals(outputName, StringComparison.OrdinalIgnoreCase));
                        
                        if (existingParam != null)
                        {
                            _logger.LogInformation("DEBUG: Found existing parameter '{ParamName}' with Direction={Direction}, changing to OUTPUT", 
                                outputName, existingParam.Direction);
                            
                            if (existingParam.Direction != ParameterDirection.Output && 
                                existingParam.Direction != ParameterDirection.InputOutput)
                            {
                                existingParam.Direction = ParameterDirection.Output;
                                _logger.LogInformation("DEBUG: Changed parameter '{ParamName}' Direction to OUTPUT", outputName);
                            }
                            if (existingParam.DbType == DbType.Object || existingParam.DbType == DbType.String)
                            {
                                existingParam.DbType = DbType.Boolean;
                                _logger.LogInformation("DEBUG: Changed parameter '{ParamName}' DbType to Boolean", outputName);
                            }
                            if (!outputParams.Contains(existingParam))
                            {
                                outputParams.Add(existingParam);
                            }
                            _logger.LogInformation("DEBUG: Using existing parameter '{ParamName}' as OUTPUT. Final: Direction={Direction}, DbType={DbType}", 
                                outputName, existingParam.Direction, existingParam.DbType);
                            break;
                        }
                        else
                        {
                            _logger.LogWarning("DEBUG: Parameter '{ParamName}' does not exist. Creating new OUTPUT parameter.", outputName);
                            var dbParam = command.CreateParameter();
                            dbParam.ParameterName = outputName;
                            dbParam.DbType = DbType.Boolean;
                            dbParam.Direction = ParameterDirection.Output;
                            command.Parameters.Add(dbParam);
                            outputParams.Add(dbParam);
                            _logger.LogInformation("DEBUG: ✓ Added OUTPUT parameter '{ParamName}' manually. Direction={Direction}, DbType={DbType}", 
                                outputName, dbParam.Direction, dbParam.DbType);
                            break;
                        }
                    }
                    
                    // Re-check outputParams after adding
                    outputParams = command.Parameters.Cast<DbParameter>()
                        .Where(p => p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput)
                        .ToList();
                    _logger.LogInformation("DEBUG: After ensuring OUTPUT param, found {Count} OUTPUT params: {Params}",
                        outputParams.Count, string.Join(", ", outputParams.Select(p => $"{p.ParameterName}({p.Direction})")));
                }

                var returnParams = command.Parameters.Cast<DbParameter>()
                    .Where(p => p.Direction == ParameterDirection.ReturnValue)
                    .ToList();

                // CRITICAL: Re-check outputParams one more time before execution
                // Sometimes DeriveParameters fails silently or OUTPUT params get lost
                outputParams = command.Parameters.Cast<DbParameter>()
                    .Where(p => p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput)
                    .ToList();
                
                // If still no OUTPUT params, force add @IsBlocked (or first common name)
                if (!outputParams.Any())
                {
                    var paramToAdd = outputParamName ?? "@IsBlocked";
                    _logger.LogWarning("DEBUG: Still no OUTPUT params found! Force adding '{ParamName}' immediately before execution.", paramToAdd);
                    
                    // Check if parameter already exists (maybe as Input)
                    var existing = command.Parameters.Cast<DbParameter>()
                        .FirstOrDefault(p => p.ParameterName.Equals(paramToAdd, StringComparison.OrdinalIgnoreCase));
                    
                    if (existing != null)
                    {
                        _logger.LogInformation("DEBUG: Parameter '{ParamName}' exists with Direction={Direction}, changing to OUTPUT", 
                            paramToAdd, existing.Direction);
                        existing.Direction = ParameterDirection.Output;
                        if (existing.DbType == DbType.Object || existing.DbType == DbType.String)
                            existing.DbType = DbType.Boolean;
                        outputParams.Add(existing);
                    }
                    else
                    {
                        var forceParam = command.CreateParameter();
                        forceParam.ParameterName = paramToAdd;
                        forceParam.DbType = DbType.Boolean;
                        forceParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(forceParam);
                        outputParams.Add(forceParam);
                        _logger.LogInformation("DEBUG: ✓ Force-added OUTPUT parameter '{ParamName}' immediately before execution", paramToAdd);
                    }
                }

                // FINAL FIX: One more time, ensure ALL InputOutput parameters have values
                // This is critical - SQL Server will reject InputOutput params without values
                foreach (var param in command.Parameters.Cast<DbParameter>())
                {
                    if (param.Direction == ParameterDirection.InputOutput)
                    {
                        if (param.Value == null || param.Value == DBNull.Value)
                        {
                            if (param.DbType == DbType.Boolean)
                            {
                                param.Value = false;
                                _logger.LogWarning("DEBUG: FINAL FIX - Set value (false) for InputOutput '{ParamName}' immediately before execution", param.ParameterName);
                            }
                            else if (param.DbType == DbType.Int32 || param.DbType == DbType.Int64 || param.DbType == DbType.Decimal)
                            {
                                param.Value = 0;
                                _logger.LogWarning("DEBUG: FINAL FIX - Set value (0) for InputOutput '{ParamName}' immediately before execution", param.ParameterName);
                            }
                            else
                            {
                                param.Value = DBNull.Value;
                                _logger.LogWarning("DEBUG: FINAL FIX - Set DBNull for InputOutput '{ParamName}' immediately before execution", param.ParameterName);
                            }
                        }
                    }
                }
                
                // Log all parameters before execution
                _logger.LogInformation("DEBUG: All parameters before execution: {Params}",
                    string.Join(", ", command.Parameters.Cast<DbParameter>().Select(p => 
                        $"{p.ParameterName}({p.Direction}, {p.DbType}, Value={p.Value})")));

                // FINAL CHECK: Ensure OUTPUT parameter exists before execution
                // This is the last chance - if we still don't have OUTPUT params, SQL Server will fail
                if (!outputParams.Any())
                {
                    _logger.LogError("DEBUG: CRITICAL - No OUTPUT parameters found even after all attempts! Adding @IsBlocked as last resort.");
                    var lastResortParam = command.CreateParameter();
                    lastResortParam.ParameterName = "@IsBlocked";
                    lastResortParam.DbType = DbType.Boolean;
                    lastResortParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(lastResortParam);
                    outputParams.Add(lastResortParam);
                    _logger.LogInformation("DEBUG: ✓ Added @IsBlocked as last resort OUTPUT parameter");
                }

                // FINAL VERIFICATION: Double-check OUTPUT parameter exists
                // Sometimes parameters get lost or not properly added
                var finalOutputCheck = command.Parameters.Cast<DbParameter>()
                    .Where(p => p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput)
                    .ToList();
                
                if (!finalOutputCheck.Any())
                {
                    _logger.LogError("DEBUG: FINAL CHECK FAILED - Still no OUTPUT parameters! This should not happen. Adding @IsBlocked immediately.");
                    var emergencyParam = command.CreateParameter();
                    emergencyParam.ParameterName = "@IsBlocked";
                    emergencyParam.DbType = DbType.Boolean;
                    emergencyParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(emergencyParam);
                    outputParams = new List<DbParameter> { emergencyParam };
                    _logger.LogInformation("DEBUG: ✓ Emergency OUTPUT parameter added. Total params now: {Count}",
                        command.Parameters.Count);
                }

                // Execute with OUTPUT params if available, otherwise try reader
                if (outputParams.Any() || returnParams.Any())
                {
                    var selected = outputParams.Any()
                        ? SelectBestOutputParameter(outputParams, resultMapping)
                        : returnParams.FirstOrDefault();

                    if (selected != null)
                        debug.SelectedResultParam = $"{selected.ParameterName} ({selected.Direction})";

                    // Final parameter list before execution
                    var allParamsBeforeExec = string.Join(", ", command.Parameters.Cast<DbParameter>().Select(p => 
                        $"{p.ParameterName}({p.Direction}, {p.DbType})"));
                    
                    _logger.LogInformation("DEBUG: About to execute. OUTPUT params: {OutputCount}, RETURN params: {ReturnCount}, Selected: {Selected}",
                        outputParams.Count, returnParams.Count, selected?.ParameterName ?? "none");
                    _logger.LogInformation("DEBUG: All parameters before ExecuteNonQueryAsync: {AllParams}", allParamsBeforeExec);
                    
                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        _logger.LogInformation("DEBUG: ExecuteNonQueryAsync succeeded!");
                    }
                    catch (Exception execEx)
                    {
                        _logger.LogError(execEx, "DEBUG: ExecuteNonQueryAsync FAILED with error: {Error}. Parameters were: {Params}",
                            execEx.Message, allParamsBeforeExec);
                        throw; // Re-throw to be caught by outer catch
                    }

                    // capture outputs
                    if (outputParams.Any())
                    {
                        debug.OutputValues = outputParams.ToDictionary(
                            p => p.ParameterName,
                            p => p.Value == DBNull.Value ? null : p.Value);
                    }

                    if (returnParams.Any())
                    {
                        var ret = returnParams.First();
                        debug.ReturnValue = ret.Value == DBNull.Value ? null : ret.Value;
                    }

                    // choose boolean from outputs/return
                    var candidates = new List<DbParameter>();
                    if (selected != null) candidates.Add(selected);
                    candidates.AddRange(outputParams.Where(p => selected == null || !ReferenceEquals(p, selected)));
                    candidates.AddRange(returnParams);

                    foreach (var candidate in candidates)
                    {
                        if (candidate.Value != null && candidate.Value != DBNull.Value)
                        {
                            debug.Result = ConvertToBoolean(candidate.Value);
                            return debug;
                        }
                    }
                }

                // fallback to reader
                using var reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                {
                    debug.Result = false;
                    return debug;
                }

                await reader.ReadAsync();
                var firstRow = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var name = reader.GetName(i);
                    var val = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    firstRow[name] = val;
                }
                debug.FirstRow = firstRow;

                debug.Result = ParseStoredProcedureResult(reader, resultMapping);
                return debug;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }

        public class StoredProcedureExecutionDebug
        {
            public bool Result { get; set; }
            public string? SelectedResultParam { get; set; }
            public Dictionary<string, object?>? OutputValues { get; set; }
            public object? ReturnValue { get; set; }
            public Dictionary<string, object?>? FirstRow { get; set; }
        }

        /// <summary>
        /// Parse the result from Stored Procedure based on resultMapping configuration
        /// </summary>
        private bool ParseStoredProcedureResult(DbDataReader reader, string? resultMapping)
        {
            // If no mapping provided, try to read first column as boolean
            if (string.IsNullOrWhiteSpace(resultMapping))
            {
                if (reader.FieldCount > 0)
                {
                    var value = reader[0];
                    return ConvertToBoolean(value);
                }
                return false;
            }

            try
            {
                var mapping = JsonSerializer.Deserialize<ResultMapping>(resultMapping);
                if (mapping == null)
                {
                    _logger.LogWarning("Invalid result mapping JSON, using default parsing");
                    return ConvertToBoolean(reader[0]);
                }

                // Get the result column value
                object? resultValue = null;
                if (!string.IsNullOrWhiteSpace(mapping.ResultColumn))
                {
                    if (reader.HasColumn(mapping.ResultColumn))
                    {
                        resultValue = reader[mapping.ResultColumn];
                    }
                    else if (reader.FieldCount > 0)
                    {
                        resultValue = reader[0];
                    }
                }
                else if (reader.FieldCount > 0)
                {
                    resultValue = reader[0];
                }

                if (resultValue == null || resultValue == DBNull.Value)
                {
                    return false;
                }

                // Compare with true/false values
                if (mapping.TrueValue != null)
                {
                    var trueValueStr = mapping.TrueValue.ToString();
                    var resultValueStr = resultValue.ToString();
                    
                    if (string.Equals(trueValueStr, resultValueStr, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }

                if (mapping.FalseValue != null)
                {
                    var falseValueStr = mapping.FalseValue.ToString();
                    var resultValueStr = resultValue.ToString();
                    
                    if (string.Equals(falseValueStr, resultValueStr, StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }

                // Fallback to direct boolean conversion
                return ConvertToBoolean(resultValue);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error parsing result mapping, using default parsing");
                return ConvertToBoolean(reader[0]);
            }
        }

        private DbParameter SelectBestOutputParameter(List<DbParameter> outputParams, string? resultMapping)
        {
            if (outputParams == null || outputParams.Count == 0)
                throw new ArgumentException("Output parameters list is empty", nameof(outputParams));

            // 1) Try to use resultMapping.resultColumn to select output parameter
            if (!string.IsNullOrWhiteSpace(resultMapping))
            {
                try
                {
                    var mapping = JsonSerializer.Deserialize<ResultMapping>(resultMapping);
                    var resultColumn = mapping?.ResultColumn;
                    if (!string.IsNullOrWhiteSpace(resultColumn))
                    {
                        var candidates = new[]
                        {
                            resultColumn.StartsWith("@") ? resultColumn : "@" + resultColumn,
                            resultColumn.StartsWith("@") ? resultColumn.TrimStart('@') : resultColumn
                        };

                        foreach (var candidate in candidates.Where(c => !string.IsNullOrWhiteSpace(c)))
                        {
                            var match = outputParams.FirstOrDefault(p =>
                                p.ParameterName.Equals(candidate, StringComparison.OrdinalIgnoreCase));
                            if (match != null)
                                return match;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex, "Failed to parse resultMapping for selecting OUTPUT parameter. Falling back to common names.");
                }
            }

            // 2) Try common output names
            var preferredNames = new[] { "@IsBlocked", "@Result", "@Blocked", "@IsValid" };
            foreach (var preferred in preferredNames)
            {
                var match = outputParams.FirstOrDefault(p =>
                    p.ParameterName.Equals(preferred, StringComparison.OrdinalIgnoreCase));
                if (match != null)
                    return match;
            }

            // 3) Fallback
            return outputParams[0];
        }

        private static object? NormalizeParameterValue(object? value)
        {
            if (value == null)
                return null;

            if (value is JsonElement je)
            {
                switch (je.ValueKind)
                {
                    case JsonValueKind.String:
                        return je.GetString();
                    case JsonValueKind.Number:
                        // Prefer decimal to preserve precision; fallback to long/double
                        if (je.TryGetDecimal(out var dec))
                            return dec;
                        if (je.TryGetInt64(out var l))
                            return l;
                        return je.GetDouble();
                    case JsonValueKind.True:
                    case JsonValueKind.False:
                        return je.GetBoolean();
                    case JsonValueKind.Null:
                    case JsonValueKind.Undefined:
                        return null;
                    default:
                        // Objects/arrays: pass as JSON string (or adjust per your SP contract)
                        return je.GetRawText();
                }
            }

            return value;
        }

        /// <summary>
        /// Convert various types to boolean
        /// </summary>
        private bool ConvertToBoolean(object value)
        {
            if (value == null || value == DBNull.Value)
                return false;

            if (value is bool boolValue)
                return boolValue;

            if (value is int intValue)
                return intValue != 0;

            if (value is string stringValue)
            {
                if (bool.TryParse(stringValue, out bool parsed))
                    return parsed;
                
                if (int.TryParse(stringValue, out int parsedInt))
                    return parsedInt != 0;
                
                return stringValue.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                       stringValue.Equals("1", StringComparison.OrdinalIgnoreCase) ||
                       stringValue.Equals("yes", StringComparison.OrdinalIgnoreCase);
            }

            // Try to convert to string and parse
            var stringVal = value.ToString();
            return !string.IsNullOrWhiteSpace(stringVal) && 
                   (stringVal.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                    stringVal.Equals("1", StringComparison.OrdinalIgnoreCase) ||
                    stringVal.Equals("yes", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Validate stored procedure name to prevent SQL injection
        /// </summary>
        private bool IsValidStoredProcedureName(string procedureName)
        {
            if (string.IsNullOrWhiteSpace(procedureName))
                return false;

            // Allow alphanumeric, underscores, dots, and brackets (for schema.table format)
            return System.Text.RegularExpressions.Regex.IsMatch(
                procedureName, 
                @"^[a-zA-Z0-9_\[\]\.]+$");
        }

        /// <summary>
        /// Build parameter dictionary from form field values and parameter mapping
        /// </summary>
        public Dictionary<string, object> BuildParameters(
            Dictionary<string, object> formFieldValues,
            string? parameterMappingJson)
        {
            var parameters = new Dictionary<string, object>();

            if (string.IsNullOrWhiteSpace(parameterMappingJson))
            {
                // If no mapping, use all form field values as parameters.
                // Normalize JsonElement values to actual .NET types
                var normalizedParams = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                foreach (var kvp in formFieldValues)
                {
                    var normalized = NormalizeParameterValue(kvp.Value);
                    normalizedParams[kvp.Key] = normalized ?? DBNull.Value;
                }
                return normalizedParams;
            }

            try
            {
                var mapping = JsonSerializer.Deserialize<Dictionary<string, string>>(parameterMappingJson);
                if (mapping == null)
                {
                    _logger.LogWarning("Invalid parameter mapping JSON, using all form field values");
                    // Normalize JsonElement values to actual .NET types
                    var normalizedParams = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                    foreach (var kvp in formFieldValues)
                    {
                        var normalized = NormalizeParameterValue(kvp.Value);
                        normalizedParams[kvp.Key] = normalized ?? DBNull.Value;
                    }
                    return normalizedParams;
                }

                foreach (var map in mapping)
                {
                    var paramName = map.Key; // e.g., "@EmployeeId"
                    var fieldCode = map.Value; // e.g., "employeeId"

                    if (TryGetValueCaseInsensitive(formFieldValues, fieldCode, out var value))
                    {
                        // Normalize JsonElement to actual .NET types before storing in parameters dictionary
                        var normalizedValue = NormalizeParameterValue(value);
                        parameters[paramName] = normalizedValue ?? DBNull.Value;
                        _logger.LogDebug("Mapped field '{FieldCode}' -> parameter '{ParamName}': {Value} ({Type}) -> {NormalizedValue} ({NormalizedType})",
                            fieldCode, paramName, value, value?.GetType().Name ?? "null",
                            normalizedValue, normalizedValue?.GetType().Name ?? "null");
                    }
                    else
                    {
                        _logger.LogWarning("Form field '{FieldCode}' not found in form values for parameter '{ParamName}'", 
                            fieldCode, paramName);
                    }
                }

                return parameters;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing parameter mapping JSON");
                // Normalize JsonElement values to actual .NET types
                var normalizedParams = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                foreach (var kvp in formFieldValues)
                {
                    var normalized = NormalizeParameterValue(kvp.Value);
                    normalizedParams[kvp.Key] = normalized ?? DBNull.Value;
                }
                return normalizedParams;
            }
        }

        private static bool TryGetValueCaseInsensitive(
            IDictionary<string, object> dict,
            string key,
            out object? value)
        {
            value = null;
            if (dict == null || string.IsNullOrWhiteSpace(key))
                return false;

            // Fast path if exact key exists
            if (dict.TryGetValue(key, out var direct))
            {
                value = direct;
                return true;
            }

            // Fallback to case-insensitive scan
            foreach (var kvp in dict)
            {
                if (kvp.Key.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    value = kvp.Value;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Extract procedure name from ProcedureCode
        /// </summary>
        private string? ExtractProcedureNameFromCode(string procedureCode)
        {
            if (string.IsNullOrWhiteSpace(procedureCode))
                return null;

            // Try to extract procedure name from CREATE PROCEDURE statement
            var patterns = new[]
            {
                @"CREATE\s+PROCEDURE\s+(?:\[?(\w+)\]?\.)?\[?(\w+)\]?",
                @"CREATE\s+PROC\s+(?:\[?(\w+)\]?\.)?\[?(\w+)\]?",
                @"PROCEDURE\s+(?:\[?(\w+)\]?\.)?\[?(\w+)\]?"
            };

            foreach (var pattern in patterns)
            {
                var match = System.Text.RegularExpressions.Regex.Match(
                    procedureCode,
                    pattern,
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    // Return the procedure name (group 2 if schema exists, group 1 otherwise)
                    if (match.Groups.Count > 2 && !string.IsNullOrWhiteSpace(match.Groups[2].Value))
                    {
                        return match.Groups[2].Value;
                    }
                    else if (match.Groups.Count > 1 && !string.IsNullOrWhiteSpace(match.Groups[1].Value))
                    {
                        return match.Groups[1].Value;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Result mapping configuration
        /// </summary>
        private class ResultMapping
        {
            public string? ResultColumn { get; set; }
            public object? TrueValue { get; set; }
            public object? FalseValue { get; set; }
        }
    }

    /// <summary>
    /// Extension method to check if DataReader has a column
    /// </summary>
    public static class DbDataReaderExtensions
    {
        public static bool HasColumn(this DbDataReader reader, string columnName)
        {
            try
            {
                return reader.GetOrdinal(columnName) >= 0;
            }
            catch
            {
                return false;
            }
        }
    }
}

