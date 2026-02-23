using formBuilder.Domian.Interfaces;
using FormBuilder.API.DTOs;
using FormBuilder.API.Models;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domain.Interfaces;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Services.Services.Base;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Core.Models;
using FormBuilder.Infrastructure.Data;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace FormBuilder.Services
{
    public class GridColumnDataSourcesService : BaseService<GRID_COLUMN_DATA_SOURCES, GridColumnDataSourceDto, CreateGridColumnDataSourceDto, UpdateGridColumnDataSourceDto>, IGridColumnDataSourcesService
    {
        private readonly IunitOfwork _unitOfWork;
        private readonly IGridColumnDataSourcesRepository _gridColumnDataSourcesRepository;
        private readonly AkhmanageItContext _akhmanageItContext;
        private readonly FormBuilderDbContext _formBuilderDbContext;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<GridColumnDataSourcesService> _logger;

        public GridColumnDataSourcesService(
            IunitOfwork unitOfWork, 
            IGridColumnDataSourcesRepository gridColumnDataSourcesRepository,
            AkhmanageItContext akhmanageItContext,
            FormBuilderDbContext formBuilderDbContext,
            IHttpClientFactory httpClientFactory,
            ILogger<GridColumnDataSourcesService> logger,
            IMapper mapper) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _gridColumnDataSourcesRepository = gridColumnDataSourcesRepository ?? throw new ArgumentNullException(nameof(gridColumnDataSourcesRepository));
            _akhmanageItContext = akhmanageItContext ?? throw new ArgumentNullException(nameof(akhmanageItContext));
            _formBuilderDbContext = formBuilderDbContext ?? throw new ArgumentNullException(nameof(formBuilderDbContext));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override IBaseRepository<GRID_COLUMN_DATA_SOURCES> Repository => _unitOfWork.GridColumnDataSourcesRepository;

        public async Task<ApiResponse> GetAllAsync()
        {
            var result = await base.GetAllAsync();
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            var result = await base.GetByIdAsync(id);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> GetByColumnIdAsync(int columnId)
        {
            var dataSources = await _gridColumnDataSourcesRepository.GetByColumnIdAsync(columnId);
            var dataSourceDtos = _mapper.Map<IEnumerable<GridColumnDataSourceDto>>(dataSources);
            return new ApiResponse(200, "Grid column data sources retrieved successfully", dataSourceDtos);
        }

        public async Task<ApiResponse> GetActiveByColumnIdAsync(int columnId)
        {
            var dataSources = await _gridColumnDataSourcesRepository.GetActiveByColumnIdAsync(columnId);
            var dataSourceDtos = _mapper.Map<IEnumerable<GridColumnDataSourceDto>>(dataSources);
            return new ApiResponse(200, "Active grid column data sources retrieved successfully", dataSourceDtos);
        }

        public async Task<ApiResponse> GetByColumnIdAndTypeAsync(int columnId, string sourceType)
        {
            var dataSource = await _gridColumnDataSourcesRepository.GetByColumnIdAsync(columnId, sourceType);
            if (dataSource == null)
                return new ApiResponse(404, "Grid column data source not found for the specified type");

            var dataSourceDto = _mapper.Map<GridColumnDataSourceDto>(dataSource);
            return new ApiResponse(200, "Grid column data source retrieved successfully", dataSourceDto);
        }

        public async Task<ApiResponse> CreateAsync(CreateGridColumnDataSourceDto createDto)
        {
            // Validate column exists
            var column = await _unitOfWork.FormGridColumnRepository.GetByIdAsync(createDto.ColumnId);
            if (column == null)
                return new ApiResponse(404, "Grid column not found");

            // Build ConfigurationJson if not provided
            if (string.IsNullOrEmpty(createDto.ConfigurationJson))
            {
                createDto.ConfigurationJson = BuildConfigurationJson(createDto);
            }

            var result = await base.CreateAsync(createDto);
            
            // If DataSource is Api or LookupTable, delete all existing options for this column
            if (result.Success && (string.Equals(createDto.SourceType, "Api", StringComparison.OrdinalIgnoreCase) ||
                                  string.Equals(createDto.SourceType, "LookupTable", StringComparison.OrdinalIgnoreCase)))
            {
                await DeleteAllOptionsForColumnAsync(createDto.ColumnId);
            }
            
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> UpdateAsync(int id, UpdateGridColumnDataSourceDto updateDto)
        {
            var result = await base.UpdateAsync(id, updateDto);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var result = await base.DeleteAsync(id);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> ToggleActiveAsync(int id, bool isActive)
        {
            var result = await base.ToggleActiveAsync(id, isActive);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> ExistsAsync(int id)
        {
            var exists = await Repository.AnyAsync(e => e.Id == id);
            return new ApiResponse(200, exists ? "Data source exists" : "Data source does not exist", exists);
        }

        public async Task<ApiResponse> ColumnHasDataSourcesAsync(int columnId)
        {
            var hasDataSources = await _gridColumnDataSourcesRepository.ColumnHasDataSourcesAsync(columnId);
            return new ApiResponse(200, "Check completed", hasDataSources);
        }

        public async Task<ApiResponse> GetDataSourcesCountAsync(int columnId)
        {
            var count = await _gridColumnDataSourcesRepository.GetDataSourcesCountAsync(columnId);
            return new ApiResponse(200, "Data sources count retrieved successfully", count);
        }

        public async Task<ApiResponse> GetColumnOptionsAsync(int columnId, Dictionary<string, object>? context = null, string? requestBodyJson = null)
        {
            try
            {
                // Get active data source for the column
                var dataSources = await _gridColumnDataSourcesRepository.GetActiveByColumnIdAsync(columnId);
                var activeDataSource = dataSources.FirstOrDefault(ds => ds.IsActive);

                if (activeDataSource == null)
                {
                    // No data source configured, return static options from GRID_COLUMN_OPTIONS
                    var staticOptions = await GetStaticColumnOptionsAsync(columnId);
                    return new ApiResponse(200, "Column options retrieved successfully", staticOptions);
                }

                // Load options based on source type
                List<FieldOptionResponseDto> responseOptions;
                switch (activeDataSource.SourceType.ToUpper())
                {
                    case "STATIC":
                        responseOptions = await GetStaticColumnOptionsAsync(columnId);
                        break;

                    case "LOOKUPTABLE":
                        // For LookupTable, fetch options from database table (do NOT save to GRID_COLUMN_OPTIONS)
                        responseOptions = await GetLookupTableOptionsAsync(activeDataSource, context);
                        break;

                    case "API":
                        // For Api, fetch options from external API (do NOT save to GRID_COLUMN_OPTIONS)
                        // Get custom array property names from ConfigurationJson or DTO
                        List<string>? customArrayNames = null;
                        if (!string.IsNullOrEmpty(activeDataSource.ConfigurationJson))
                        {
                            try
                            {
                                var config = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(activeDataSource.ConfigurationJson);
                                if (config != null && config.ContainsKey("arrayPropertyNames"))
                                {
                                    var arrayNamesJson = config["arrayPropertyNames"]?.ToString();
                                    if (!string.IsNullOrEmpty(arrayNamesJson))
                                    {
                                        customArrayNames = System.Text.Json.JsonSerializer.Deserialize<List<string>>(arrayNamesJson);
                                    }
                                }
                            }
                            catch { }
                        }
                        responseOptions = await GetApiOptionsAsync(activeDataSource, requestBodyJson, context, customArrayNames);
                        break;

                    default:
                        return new ApiResponse(400, $"Unsupported source type: {activeDataSource.SourceType}");
                }

                // Return FieldOptionResponseDto (with 'text' and 'value' properties) for frontend compatibility
                return new ApiResponse(200, "Column options retrieved successfully", responseOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving column options for column {ColumnId}", columnId);
                return new ApiResponse(500, $"Error retrieving column options: {ex.Message}");
            }
        }

        /// <summary>
        /// Get static options from GRID_COLUMN_OPTIONS table for a column
        /// </summary>
        private async Task<List<FormBuilder.Core.DTOS.FormBuilder.FieldOptionResponseDto>> GetStaticColumnOptionsAsync(int columnId)
        {
            var options = await _unitOfWork.GridColumnOptionsRepository.GetActiveByColumnIdAsync(columnId);
            return options
                .Where(o => o.IsActive && !o.IsDeleted) // Filter active and not deleted
                .OrderBy(o => o.OptionOrder)
                .ThenBy(o => o.OptionText)
                .Select(o => new FormBuilder.Core.DTOS.FormBuilder.FieldOptionResponseDto
                {
                    Value = o.OptionValue ?? string.Empty,
                    Text = o.OptionText ?? string.Empty
                })
                .ToList();
        }

        private string? BuildConfigurationJson(CreateGridColumnDataSourceDto dto)
        {
            try
            {
                if (dto.SourceType.Equals("LookupTable", StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(dto.ApiUrl) && !string.IsNullOrEmpty(dto.ValuePath) && !string.IsNullOrEmpty(dto.TextPath))
                    {
                        var config = new
                        {
                            table = dto.ApiUrl,
                            valueColumn = dto.ValuePath,
                            textColumn = dto.TextPath
                        };
                        return System.Text.Json.JsonSerializer.Serialize(config);
                    }
                }
                else if (dto.SourceType.Equals("Api", StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(dto.ApiUrl) && !string.IsNullOrEmpty(dto.ValuePath) && !string.IsNullOrEmpty(dto.TextPath))
                    {
                        var config = new Dictionary<string, object?>
                        {
                            ["url"] = dto.ApiUrl,
                            ["httpMethod"] = dto.HttpMethod ?? "GET",
                            ["valuePath"] = dto.ValuePath,
                            ["textPath"] = dto.TextPath
                        };
                        if (!string.IsNullOrEmpty(dto.ApiPath))
                        {
                            config["apiPath"] = dto.ApiPath;
                        }
                        if (!string.IsNullOrEmpty(dto.RequestBodyJson))
                        {
                            config["requestBodyJson"] = dto.RequestBodyJson;
                        }
                        if (dto.ArrayPropertyNames != null && dto.ArrayPropertyNames.Any())
                        {
                            config["arrayPropertyNames"] = dto.ArrayPropertyNames;
                        }
                        return System.Text.Json.JsonSerializer.Serialize(config);
                    }
                }
            }
            catch (Exception)
            {
                // If building fails, return null
            }

            return null;
        }

        private async Task DeleteAllOptionsForColumnAsync(int columnId)
        {
            var options = await _unitOfWork.GridColumnOptionsRepository.GetByColumnIdAsync(columnId);
            // Soft Delete
            foreach (var option in options)
            {
                option.IsDeleted = true;
                option.DeletedDate = DateTime.UtcNow;
                option.IsActive = false;
                _unitOfWork.GridColumnOptionsRepository.Update(option);
            }
            await _unitOfWork.CompleteAsyn();
        }

        // ================================
        // PRIVATE HELPER METHODS
        // ================================

        /// <summary>
        /// Sanitizes a string to be safe for JSON serialization by removing problematic characters
        /// </summary>
        private string SanitizeForJson(string? input, int maxLength = 500)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // Limit length to prevent huge error messages
            var sanitized = input.Length > maxLength 
                ? input.Substring(0, maxLength) + "..." 
                : input;

            // Remove null bytes and other control characters that can corrupt JSON
            sanitized = sanitized.Replace("\0", ""); // Remove null bytes
            
            // Remove other problematic control characters but keep newlines and tabs for readability
            var sb = new StringBuilder();
            foreach (char c in sanitized)
            {
                // Keep printable characters, newlines, tabs, and common punctuation
                if (char.IsLetterOrDigit(c) || 
                    char.IsPunctuation(c) || 
                    char.IsWhiteSpace(c) || 
                    c == '\n' || c == '\r' || c == '\t' ||
                    (c >= 32 && c <= 126)) // ASCII printable range
                {
                    sb.Append(c);
                }
                else
                {
                    // Replace problematic characters with space
                    sb.Append(' ');
                }
            }

            return sb.ToString().Trim();
        }

        private async Task<List<FieldOptionResponseDto>> GetLookupTableOptionsAsync(GRID_COLUMN_DATA_SOURCES dataSource, Dictionary<string, object>? context)
        {
            string tableName;
            string valueColumn;
            string textColumn;

            // 1. Parse configuration from ConfigurationJson or individual fields
            if (!string.IsNullOrEmpty(dataSource.ConfigurationJson))
            {
                try
                {
                    var config = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(dataSource.ConfigurationJson);
                    if (config != null)
                    {
                        tableName = config.ContainsKey("table") ? config["table"]?.ToString() ?? string.Empty : string.Empty;
                        valueColumn = config.ContainsKey("valueColumn") ? config["valueColumn"]?.ToString() ?? string.Empty : string.Empty;
                        textColumn = config.ContainsKey("textColumn") ? config["textColumn"]?.ToString() ?? string.Empty : string.Empty;
                    }
                    else
                    {
                        // Fallback to individual fields
                        tableName = dataSource.ApiUrl ?? string.Empty;
                        valueColumn = dataSource.ValuePath ?? string.Empty;
                        textColumn = dataSource.TextPath ?? string.Empty;
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Invalid JSON in ConfigurationJson for grid column data source {DataSourceId}. Error: {ErrorMessage}", 
                        dataSource.Id, ex.Message);
                    throw new ArgumentException($"Invalid JSON in ConfigurationJson: {ex.Message}. Please check the JSON format.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error parsing ConfigurationJson for grid column data source {DataSourceId}", dataSource.Id);
                    // Fallback to individual fields if JSON parsing fails
                    tableName = dataSource.ApiUrl ?? string.Empty;
                    valueColumn = dataSource.ValuePath ?? string.Empty;
                    textColumn = dataSource.TextPath ?? string.Empty;
                }
            }
            else
            {
                // Use individual fields
                tableName = dataSource.ApiUrl ?? string.Empty; // Table name is stored in ApiUrl
                valueColumn = dataSource.ValuePath ?? string.Empty; // Column name for value
                textColumn = dataSource.TextPath ?? string.Empty; // Column name for text
            }

            // 2. Validate required fields
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentException("Table name is required for LookupTable source type. Please provide it in ApiUrl or ConfigurationJson.");
            }

            if (string.IsNullOrWhiteSpace(valueColumn))
            {
                throw new ArgumentException("Value column is required for LookupTable source type. Please provide it in ValuePath or ConfigurationJson.");
            }

            if (string.IsNullOrWhiteSpace(textColumn))
            {
                throw new ArgumentException("Text column is required for LookupTable source type. Please provide it in TextPath or ConfigurationJson.");
            }

            // 3. Resolve correct DbContext for target table (FormBuilder tables vs AkhmanageIt tables)
            var (dbContext, contextType, contextName) = ResolveDbContextForTable(tableName);

            // 4. Check database connection
            try
            {
                if (!await dbContext.Database.CanConnectAsync())
                {
                    _logger.LogError("Database connection failed for LookupTable query using context {ContextName}", contextName);
                    throw new InvalidOperationException("Database connection failed. Please check your database connection.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking database connection using context {ContextName}", contextName);
                throw new InvalidOperationException($"Database connection error: {ex.Message}");
            }

            // 5. Try to use reflection to access DbSet (preferred method)
            try
            {
                var dbSetProperty = contextType.GetProperty(tableName, System.Reflection.BindingFlags.IgnoreCase | 
                                                                        System.Reflection.BindingFlags.Public | 
                                                                        System.Reflection.BindingFlags.Instance);

                if (dbSetProperty == null)
                {
                    // Try to find table with different case
                    var allProperties = contextType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    var matchingProperty = allProperties.FirstOrDefault(p => 
                        string.Equals(p.Name, tableName, StringComparison.OrdinalIgnoreCase));
                    
                    if (matchingProperty == null)
                    {
                        var availableTables = string.Join(", ", allProperties
                            .Where(p => p.PropertyType.IsGenericType && 
                                       p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                            .Select(p => p.Name));
                        
                        _logger.LogError("Table '{TableName}' not found in {ContextName}. Available tables: {AvailableTables}", 
                            tableName, contextName, availableTables);
                        throw new ArgumentException($"Table '{tableName}' not found in {contextName}. Available tables: {availableTables}");
                    }
                    
                    dbSetProperty = matchingProperty;
                }

                // Get the DbSet value from the context
                var dbSetValue = dbSetProperty.GetValue(dbContext);
                if (dbSetValue == null)
                {
                    throw new InvalidOperationException($"DbSet '{tableName}' is null in {contextName}");
                }

                // Get the generic type of the DbSet (e.g., TblCustomer)
                var dbSetType = dbSetProperty.PropertyType;
                if (!dbSetType.IsGenericType || dbSetType.GetGenericTypeDefinition() != typeof(DbSet<>))
                {
                    throw new InvalidOperationException($"Property '{tableName}' is not a DbSet");
                }

                var entityType = dbSetType.GetGenericArguments()[0];

                // Use reflection to call QueryTableAsync with the correct type
                var method = typeof(GridColumnDataSourcesService)
                    .GetMethod(nameof(QueryTableAsync), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.MakeGenericMethod(entityType);

                if (method == null)
                {
                    throw new InvalidOperationException("QueryTableAsync method not found");
                }

                var task = (Task<List<FieldOptionResponseDto>>)method.Invoke(this, new[] { dbSetValue, valueColumn, textColumn, context })!;
                return await task;
            }
            catch (ArgumentException)
            {
                // Re-throw validation errors
                throw;
            }
            catch (InvalidOperationException)
            {
                // Re-throw operation errors
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error accessing table '{TableName}' using reflection. Attempting SQL fallback.", tableName);
                
                // 6. Fallback to SQL Raw Query if reflection fails
                try
                {
                    return await QueryTableUsingSqlAsync(dbContext, contextName, tableName, valueColumn, textColumn, context);
                }
                catch (Exception sqlEx)
                {
                    _logger.LogError(sqlEx, "SQL fallback also failed for table '{TableName}'", tableName);
                    throw new InvalidOperationException($"Failed to query table '{tableName}'. Reflection error: {ex.Message}. SQL error: {sqlEx.Message}");
                }
            }
        }

        /// <summary>
        /// Resolve which DbContext contains the target table.
        /// FORM_* and other FormBuilder tables live in FormBuilderDbContext,
        /// while business/identity tables live in AkhmanageItContext.
        /// </summary>
        private (DbContext Context, Type ContextType, string ContextName) ResolveDbContextForTable(string tableName)
        {
            var flags = System.Reflection.BindingFlags.IgnoreCase |
                        System.Reflection.BindingFlags.Public |
                        System.Reflection.BindingFlags.Instance;

            var formBuilderType = typeof(FormBuilderDbContext);
            var akhType = typeof(AkhmanageItContext);

            if (formBuilderType.GetProperty(tableName, flags) != null)
            {
                return (_formBuilderDbContext, formBuilderType, nameof(FormBuilderDbContext));
            }

            if (akhType.GetProperty(tableName, flags) != null)
            {
                return (_akhmanageItContext, akhType, nameof(AkhmanageItContext));
            }

            var fbTables = string.Join(", ", formBuilderType.GetProperties(flags)
                .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .Select(p => p.Name));
            var akhTables = string.Join(", ", akhType.GetProperties(flags)
                .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .Select(p => p.Name));

            throw new ArgumentException(
                $"Table '{tableName}' not found in either FormBuilderDbContext or AkhmanageItContext. " +
                $"FormBuilder tables: {fbTables}. AkhmanageIt tables: {akhTables}");
        }

        /// <summary>
        /// Fallback method to query table using raw SQL if reflection fails
        /// </summary>
        private async Task<List<FieldOptionResponseDto>> QueryTableUsingSqlAsync(
            DbContext dbContext,
            string contextName,
            string tableName, 
            string valueColumn, 
            string textColumn, 
            Dictionary<string, object>? context)
        {
            // Validate table and column names to prevent SQL injection
            if (!IsValidIdentifier(tableName) || !IsValidIdentifier(valueColumn) || !IsValidIdentifier(textColumn))
            {
                throw new ArgumentException("Invalid table or column name. Only alphanumeric characters and underscores are allowed.");
            }

            // Build SQL query with parameterized values to prevent SQL injection
            var sql = $@"
                SELECT 
                    [{valueColumn}] AS Value, 
                    [{textColumn}] AS Text 
                FROM [{tableName}] 
                WHERE 1=1";

            // Add IsActive filter if column exists (check first using connection)
            var connection = dbContext.Database.GetDbConnection();
            var hasIsActiveColumn = false;
            var hasLegalEntityColumn = false;

            try
            {
                await connection.OpenAsync();
                
                // Check if IsActive column exists
                using (var checkCommand = connection.CreateCommand())
                {
                    checkCommand.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName AND COLUMN_NAME = 'IsActive'";
                    var tableParam = checkCommand.CreateParameter();
                    tableParam.ParameterName = "@tableName";
                    tableParam.Value = tableName;
                    checkCommand.Parameters.Add(tableParam);
                    
                    var result = await checkCommand.ExecuteScalarAsync();
                    hasIsActiveColumn = Convert.ToInt32(result) > 0;
                }

                // Check if LegalEntityId column exists
                if (context != null && context.ContainsKey("LegalEntityId"))
                {
                    using (var checkCommand = connection.CreateCommand())
                    {
                        checkCommand.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName AND (COLUMN_NAME = 'IdLegalEntity' OR COLUMN_NAME = 'LegalEntityId')";
                        var tableParam = checkCommand.CreateParameter();
                        tableParam.ParameterName = "@tableName";
                        tableParam.Value = tableName;
                        checkCommand.Parameters.Add(tableParam);
                        
                        var result = await checkCommand.ExecuteScalarAsync();
                        hasLegalEntityColumn = Convert.ToInt32(result) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not verify column existence for table '{TableName}' in context {ContextName}", tableName, contextName);
            }

            // Add IsActive filter if column exists
            if (hasIsActiveColumn)
            {
                sql += " AND [IsActive] = 1";
            }

            // Add context filters
            if (hasLegalEntityColumn && context != null && context.ContainsKey("LegalEntityId"))
            {
                try
                {
                    var legalEntityId = Convert.ToInt32(context["LegalEntityId"]);
                    sql += $" AND ([IdLegalEntity] = {legalEntityId} OR [LegalEntityId] = {legalEntityId})";
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not apply LegalEntityId filter for table '{TableName}'", tableName);
                }
            }

            sql += $" ORDER BY [{textColumn}]";

            // Execute query using ADO.NET directly since EF Core doesn't have SqlQueryRaw for arbitrary types
            var options = new List<FieldOptionResponseDto>();

            try
            {
                // Connection is already open from column check above
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using var command = connection.CreateCommand();
                command.CommandText = sql;

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    options.Add(new FieldOptionResponseDto
                    {
                        Value = reader["Value"]?.ToString() ?? "",
                        Text = reader["Text"]?.ToString() ?? ""
                    });
                }
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }

            return options;
        }

        /// <summary>
        /// Validates that a string is a valid SQL identifier (prevents SQL injection)
        /// </summary>
        private bool IsValidIdentifier(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                return false;

            // Only allow alphanumeric characters, underscores, and brackets
            return System.Text.RegularExpressions.Regex.IsMatch(identifier, @"^[a-zA-Z0-9_\[\]]+$");
        }

        private async Task<List<FieldOptionResponseDto>> QueryTableAsync<T>(
            DbSet<T> dbSet, 
            string valueColumn, 
            string textColumn, 
            Dictionary<string, object>? context) where T : class
        {
            var query = dbSet.AsQueryable();
            var entityType = typeof(T);

            // Apply context filters if provided
            if (context != null)
            {
                // Filter by LegalEntityId if property exists
                if (context.ContainsKey("LegalEntityId"))
                {
                    var legalEntityId = Convert.ToInt32(context["LegalEntityId"]);
                    var property = entityType.GetProperty("IdLegalEntity") ?? 
                                  entityType.GetProperty("LegalEntityId");
                    if (property != null)
                    {
                        // Use expression tree for filtering
                        var parameter = System.Linq.Expressions.Expression.Parameter(entityType, "x");
                        var propertyAccess = System.Linq.Expressions.Expression.Property(parameter, property);
                        var constant = System.Linq.Expressions.Expression.Constant(legalEntityId);
                        var equals = System.Linq.Expressions.Expression.Equal(propertyAccess, constant);
                        var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(equals, parameter);
                        query = query.Where(lambda);
                    }
                }
            }

            // Default: filter by IsActive if property exists
            var isActiveProperty = entityType.GetProperty("IsActive");
            if (isActiveProperty != null)
            {
                // Use compiled expression for IsActive filter
                var parameter = System.Linq.Expressions.Expression.Parameter(entityType, "x");
                var property = System.Linq.Expressions.Expression.Property(parameter, isActiveProperty);
                var trueValue = System.Linq.Expressions.Expression.Constant(true);
                var equals = System.Linq.Expressions.Expression.Equal(property, trueValue);
                var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(equals, parameter);
                query = query.Where(lambda);
            }

            var items = await query.ToListAsync();

            // Use reflection to get property values - try multiple possible column names (case-insensitive)
            var valueProperty = entityType.GetProperty(valueColumn, System.Reflection.BindingFlags.IgnoreCase | 
                                                                     System.Reflection.BindingFlags.Public | 
                                                                     System.Reflection.BindingFlags.Instance) ??
                               entityType.GetProperty("Id", System.Reflection.BindingFlags.IgnoreCase | 
                                                            System.Reflection.BindingFlags.Public | 
                                                            System.Reflection.BindingFlags.Instance) ??
                               entityType.GetProperty("ID", System.Reflection.BindingFlags.IgnoreCase | 
                                                            System.Reflection.BindingFlags.Public | 
                                                            System.Reflection.BindingFlags.Instance);

            var textProperty = entityType.GetProperty(textColumn, System.Reflection.BindingFlags.IgnoreCase | 
                                                                   System.Reflection.BindingFlags.Public | 
                                                                   System.Reflection.BindingFlags.Instance) ??
                               entityType.GetProperty("Name", System.Reflection.BindingFlags.IgnoreCase | 
                                                               System.Reflection.BindingFlags.Public | 
                                                               System.Reflection.BindingFlags.Instance) ??
                               entityType.GetProperty("Title", System.Reflection.BindingFlags.IgnoreCase | 
                                                                System.Reflection.BindingFlags.Public | 
                                                                System.Reflection.BindingFlags.Instance) ??
                               entityType.GetProperty("Code", System.Reflection.BindingFlags.IgnoreCase | 
                                                               System.Reflection.BindingFlags.Public | 
                                                               System.Reflection.BindingFlags.Instance);

            if (valueProperty == null)
            {
                var availableColumns = string.Join(", ", entityType.GetProperties().Select(p => p.Name));
                throw new ArgumentException($"Value column '{valueColumn}' not found in table. Available columns: {availableColumns}");
            }

            if (textProperty == null)
            {
                var availableColumns = string.Join(", ", entityType.GetProperties().Select(p => p.Name));
                throw new ArgumentException($"Text column '{textColumn}' not found in table. Available columns: {availableColumns}");
            }

            return items
                .Where(item => 
                {
                    var textValue = textProperty.GetValue(item)?.ToString();
                    return !string.IsNullOrWhiteSpace(textValue);
                })
                .Select(item => new FieldOptionResponseDto
                {
                    Value = valueProperty.GetValue(item)?.ToString() ?? "",
                    Text = textProperty.GetValue(item)?.ToString() ?? ""
                })
                .OrderBy(o => o.Text)
                .ToList();
        }

        private async Task<List<FieldOptionResponseDto>> GetApiOptionsAsync(
            GRID_COLUMN_DATA_SOURCES dataSource, 
            string? requestBodyJson, 
            Dictionary<string, object>? context,
            List<string>? customArrayPropertyNames = null)
        {
            string baseUrl;
            string? apiPath;
            string httpMethod;
            string valuePath;
            string textPath;
            string? requestBody;

            // Try to read from ConfigurationJson first
            if (!string.IsNullOrEmpty(dataSource.ConfigurationJson))
            {
                try
                {
                    var config = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(dataSource.ConfigurationJson);
                    if (config != null)
                    {
                        baseUrl = config.ContainsKey("url") ? config["url"]?.ToString() ?? string.Empty : string.Empty;
                        apiPath = config.ContainsKey("apiPath") ? config["apiPath"]?.ToString() : null;
                        httpMethod = config.ContainsKey("httpMethod") ? config["httpMethod"]?.ToString() ?? "GET" : "GET";
                        valuePath = config.ContainsKey("valuePath") ? config["valuePath"]?.ToString() ?? string.Empty : string.Empty;
                        textPath = config.ContainsKey("textPath") ? config["textPath"]?.ToString() ?? string.Empty : string.Empty;
                        requestBody = config.ContainsKey("requestBodyJson") ? config["requestBodyJson"]?.ToString() : null;
                        
                        // Get custom array property names from config if not provided as parameter
                        if (customArrayPropertyNames == null && config.ContainsKey("arrayPropertyNames"))
                        {
                            try
                            {
                                var arrayNamesJson = config["arrayPropertyNames"];
                                if (arrayNamesJson != null)
                                {
                                    customArrayPropertyNames = System.Text.Json.JsonSerializer.Deserialize<List<string>>(arrayNamesJson.ToString() ?? "[]");
                                }
                            }
                            catch { }
                        }
                    }
                    else
                    {
                        // Fallback to individual fields
                        baseUrl = dataSource.ApiUrl ?? string.Empty;
                        apiPath = dataSource.ApiPath;
                        httpMethod = dataSource.HttpMethod ?? "GET";
                        valuePath = dataSource.ValuePath ?? string.Empty;
                        textPath = dataSource.TextPath ?? string.Empty;
                        requestBody = dataSource.RequestBodyJson;
                    }
                }
                catch
                {
                    // Fallback to individual fields if JSON parsing fails
                    baseUrl = dataSource.ApiUrl ?? string.Empty;
                    apiPath = dataSource.ApiPath;
                    httpMethod = dataSource.HttpMethod ?? "GET";
                    valuePath = dataSource.ValuePath ?? string.Empty;
                    textPath = dataSource.TextPath ?? string.Empty;
                    requestBody = !string.IsNullOrEmpty(requestBodyJson) ? requestBodyJson : dataSource.RequestBodyJson;
                }
            }
            else
            {
                // Use individual fields
                baseUrl = dataSource.ApiUrl ?? string.Empty;
                apiPath = dataSource.ApiPath;
                httpMethod = dataSource.HttpMethod ?? "GET";
                valuePath = dataSource.ValuePath ?? string.Empty;
                textPath = dataSource.TextPath ?? string.Empty;
                requestBody = !string.IsNullOrEmpty(requestBodyJson) 
                    ? requestBodyJson 
                    : dataSource.RequestBodyJson;
            }

            // Combine Base URL + Path to form full URL
            string fullApiUrl = CombineApiUrl(baseUrl, apiPath);
            _logger.LogInformation("API Request: Base URL: {BaseUrl}, Path: {Path}, Full URL: {FullUrl}", 
                baseUrl, apiPath ?? "(none)", fullApiUrl);

            // Use named HttpClient configured with automatic decompression
            var httpClient = _httpClientFactory.CreateClient("ExternalApi");
            httpClient.Timeout = TimeSpan.FromSeconds(30); // Set timeout
            
            // Add browser-like headers to avoid Cloudflare challenges and 403 Forbidden errors
            // Use a real browser User-Agent to make requests look legitimate
            if (!httpClient.DefaultRequestHeaders.Contains("User-Agent"))
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
            }
            if (!httpClient.DefaultRequestHeaders.Contains("Accept"))
            {
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
            }
            if (!httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
            {
                httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
            }
            if (!httpClient.DefaultRequestHeaders.Contains("Accept-Encoding"))
            {
                // Only request gzip and deflate - HttpClient handles these automatically
                // Brotli (br) may not be supported and can cause encoding issues
                httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            }
            if (!httpClient.DefaultRequestHeaders.Contains("Cache-Control"))
            {
                httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            }

            HttpResponseMessage response;
            string jsonContent;
            try
            {
                if (httpMethod.ToUpper() == "POST")
                {
                    var body = !string.IsNullOrEmpty(requestBody) 
                        ? JsonSerializer.Deserialize<JsonElement>(requestBody) 
                        : (JsonElement?)null;

                    response = await httpClient.PostAsJsonAsync(fullApiUrl, body);
                }
                else
                {
                    response = await httpClient.GetAsync(fullApiUrl);
                }

                // Check if response is successful
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var sanitizedError = SanitizeForJson(errorContent, 200); // Limit to 200 chars for error messages
                    _logger.LogWarning("API returned non-success status: {StatusCode} - {ReasonPhrase}. Response: {ErrorContent}", 
                        response.StatusCode, response.ReasonPhrase, sanitizedError);
                    throw new HttpRequestException($"API request failed with status {response.StatusCode} ({response.ReasonPhrase}). Response: {sanitizedError}");
                }

                // Read response content inside try-catch to handle encoding issues
                jsonContent = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error when calling API: {FullUrl}", fullApiUrl);
                var sanitizedMessage = SanitizeForJson(ex.Message);
                throw new InvalidOperationException($"Failed to fetch data from API: {sanitizedMessage}", ex);
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Request timeout when calling API: {FullUrl}", fullApiUrl);
                throw new InvalidOperationException($"API request timed out after 30 seconds: {fullApiUrl}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading response from API: {FullUrl}", fullApiUrl);
                var sanitizedMessage = SanitizeForJson(ex.Message);
                throw new InvalidOperationException($"Failed to read response from API: {sanitizedMessage}", ex);
            }
            
            // Check if response is HTML (Cloudflare challenge or error page)
            if (jsonContent.TrimStart().StartsWith("<!DOCTYPE", StringComparison.OrdinalIgnoreCase) ||
                jsonContent.TrimStart().StartsWith("<html", StringComparison.OrdinalIgnoreCase) ||
                jsonContent.Contains("cloudflare", StringComparison.OrdinalIgnoreCase) ||
                jsonContent.Contains("challenge", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("API returned HTML instead of JSON. This might be a Cloudflare challenge page. Response preview: {Preview}", 
                    jsonContent.Length > 500 ? jsonContent.Substring(0, 500) : jsonContent);
                throw new InvalidOperationException("API returned HTML page instead of JSON. This might be due to Cloudflare protection or the API endpoint is incorrect. Please check the API URL and try again.");
            }
            
            // Check if response might be compressed (starts with GZIP magic number 0x1F 0x8B)
            if (jsonContent.Length >= 2 && (byte)jsonContent[0] == 0x1F && (byte)jsonContent[1] == 0x8B)
            {
                _logger.LogError("API returned compressed (GZIP) response that was not decompressed. This indicates HttpClient automatic decompression is not working properly.");
                throw new InvalidOperationException("API returned compressed response that could not be decompressed. Please check HttpClient configuration.");
            }
            
            try
            {
                var jsonDoc = JsonDocument.Parse(jsonContent);
                var root = jsonDoc.RootElement;

                // Try to find array in response - support any API format
                JsonElement? arrayElement = null;

                // 1. Check if root is array directly
                if (root.ValueKind == JsonValueKind.Array)
                {
                    arrayElement = root;
                    _logger.LogInformation("Found array at root level");
                }
                // 2. Check if valuePath starts with array notation (e.g., "results[].id")
                else if (root.ValueKind == JsonValueKind.Object)
                {
                    // Extract array path from valuePath (e.g., "results[].id" -> "results")
                    var arrayPath = ExtractArrayPath(valuePath, textPath);
                    
                    if (!string.IsNullOrEmpty(arrayPath))
                    {
                        _logger.LogInformation("Trying to navigate to array path: {ArrayPath}", arrayPath);
                        // Navigate to array using path
                        var element = NavigateToJsonElement(root, arrayPath);
                        if (element.HasValue && element.Value.ValueKind == JsonValueKind.Array)
                        {
                            arrayElement = element;
                            _logger.LogInformation("Found array at path: {ArrayPath}", arrayPath);
                        }
                    }
                    
                    // 3. If no array path found, try common property names (supports any API structure)
                    if (!arrayElement.HasValue)
                    {
                        // Use custom array property names if provided by user, otherwise use default common names
                        var arrayPropertyNames = customArrayPropertyNames != null && customArrayPropertyNames.Any()
                            ? customArrayPropertyNames.ToList()
                            : new List<string> { 
                                "data", "results", "items", "list", "records", "values", "content", "collection",
                                "users", "products", "entries", "objects", "entities", "rows", "elements",
                                "array", "itemsList", "dataList", "resultList", "response", "payload", "body"
                            };
                        
                        _logger.LogInformation("Searching for array in properties: {Properties}", string.Join(", ", arrayPropertyNames));
                        
                        foreach (var propName in arrayPropertyNames)
                        {
                            // Try exact match first
                            if (root.TryGetProperty(propName, out var prop) && 
                                prop.ValueKind == JsonValueKind.Array && prop.GetArrayLength() > 0)
                            {
                                arrayElement = prop;
                                _logger.LogInformation("Found array at property: {PropertyName}", propName);
                                break;
                            }
                            
                            // Try case-insensitive match by enumerating properties
                            foreach (var jsonProp in root.EnumerateObject())
                            {
                                if (string.Equals(jsonProp.Name, propName, StringComparison.OrdinalIgnoreCase) &&
                                    jsonProp.Value.ValueKind == JsonValueKind.Array && jsonProp.Value.GetArrayLength() > 0)
                                {
                                    arrayElement = jsonProp.Value;
                                    _logger.LogInformation("Found array at property: {PropertyName} (case-insensitive match)", jsonProp.Name);
                                    break;
                                }
                            }
                            
                            if (arrayElement.HasValue)
                                break;
                        }
                    }
                    
                    // 4. Search recursively for first array found
                    if (!arrayElement.HasValue)
                    {
                        _logger.LogInformation("Searching recursively for array...");
                        arrayElement = FindFirstArray(root);
                        if (arrayElement.HasValue)
                        {
                            _logger.LogInformation("Found array recursively");
                        }
                    }
                }

                if (arrayElement.HasValue)
                {
                    // Clean paths from array notation before extracting values
                    var cleanValuePath = CleanPathFromArrayNotation(valuePath);
                    var cleanTextPath = CleanPathFromArrayNotation(textPath);
                    
                    _logger.LogInformation("Extracting options with valuePath: {ValuePath}, textPath: {TextPath}", cleanValuePath, cleanTextPath);
                    
                    return ExtractOptionsFromArray(arrayElement.Value, cleanValuePath, cleanTextPath);
                }

                // Log the actual response for debugging
                var structure = GetResponseStructure(root, 2);
                _logger.LogWarning("Could not find array in API response. Response structure: {Structure}", structure);
                throw new InvalidOperationException($"Invalid API response format. Could not find array in response. Response structure: {structure}");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse JSON response from API: {FullUrl}. Error: {Error}", fullApiUrl, ex.Message);
                
                // Check if the error indicates a compression issue (0x1F is GZIP magic number)
                if (ex.Message.Contains("0x1F") || ex.Message.Contains("invalid start of a value"))
                {
                    var errorMsg = "API returned compressed response that could not be decompressed. " +
                                  "This usually means the response is GZIP-compressed but HttpClient automatic decompression is not enabled. " +
                                  "Please ensure HttpClient is configured with AutomaticDecompression enabled.";
                    _logger.LogError(errorMsg);
                    throw new InvalidOperationException(errorMsg);
                }
                
                var sanitizedMessage = SanitizeForJson(ex.Message);
                throw new InvalidOperationException($"Invalid JSON response from API: {sanitizedMessage}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing API response from: {FullUrl}", fullApiUrl);
                throw;
            }
        }

        private List<FieldOptionResponseDto> ExtractOptionsFromArray(JsonElement arrayElement, string valuePath, string textPath)
        {
            var options = new List<FieldOptionResponseDto>();
            var itemCount = 0;
            var skippedCount = 0;

            // Note: paths are already cleaned before calling this method
            _logger.LogInformation("Extracting options from array - valuePath: '{ValuePath}', textPath: '{TextPath}'", 
                valuePath, textPath);

            foreach (var item in arrayElement.EnumerateArray())
            {
                itemCount++;
                var value = GetJsonValue(item, valuePath);
                var text = GetJsonValue(item, textPath);

                // Log first 3 items for debugging
                if (itemCount <= 3)
                {
                    _logger.LogInformation("Item {ItemCount} - valuePath: '{ValuePath}', extracted value: '{Value}', textPath: '{TextPath}', extracted text: '{Text}'", 
                        itemCount, valuePath, value ?? "null", textPath, text ?? "null");
                    
                    // Log item structure for first item
                    if (itemCount == 1)
                    {
                        var itemStructure = GetResponseStructure(item, 2);
                        _logger.LogInformation("First item structure: {ItemStructure}", itemStructure);
                    }
                }

                if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(text))
                {
                    options.Add(new FieldOptionResponseDto
                    {
                        Value = value.Trim(),
                        Text = text.Trim()
                    });
                }
                else
                {
                    skippedCount++;
                    if (skippedCount <= 5) // Log first 5 skipped items
                    {
                        var itemStructure = GetResponseStructure(item, 1);
                        _logger.LogWarning("Skipped item {ItemCount} - value: '{Value}', text: '{Text}'. Item structure: {ItemStructure}", 
                            itemCount, value ?? "null", text ?? "null", itemStructure);
                    }
                }
            }

            _logger.LogInformation("Extracted {Count} options from {Total} items (skipped {Skipped})", 
                options.Count, itemCount, skippedCount);

            if (options.Count == 0 && itemCount > 0)
            {
                var firstItem = arrayElement.EnumerateArray().First();
                var itemStructure = GetResponseStructure(firstItem, 2);
                var availableProps = string.Join(", ", firstItem.EnumerateObject().Select(p => p.Name));
                
                _logger.LogError("No options extracted! Check valuePath and textPath. " +
                    "valuePath: '{ValuePath}', textPath: '{TextPath}'. " +
                    "Available properties in first item: {Properties}. " +
                    "First item structure: {ItemStructure}", 
                    valuePath, textPath, availableProps, itemStructure);
                
                // Throw exception with helpful message
                throw new InvalidOperationException(
                    $"No options extracted from API response. " +
                    $"Please verify that valuePath '{valuePath}' and textPath '{textPath}' are correct. " +
                    $"Available properties in the first item: {availableProps}. " +
                    $"For nested properties, use dot notation (e.g., 'name.first' instead of 'first_name').");
            }

            return options;
        }

        private string CleanPathFromArrayNotation(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            // Remove array notation if present (e.g., "results[].id" -> "id" or "results.id")
            // Handle patterns like "results[].id", "data[].name", etc.
            var pattern = @"^[^\[\]]+\[\]\.";
            path = Regex.Replace(path, pattern, "");
            
            // Also handle if path starts with array notation
            if (path.StartsWith("[]."))
            {
                path = path.Substring(3);
            }
            else if (path.Contains("[]"))
            {
                path = path.Replace("[]", "");
            }

            return path;
        }

        private string GetJsonValue(JsonElement element, string path)
        {
            if (string.IsNullOrEmpty(path))
                return "";

            // Note: path should already be cleaned from array notation before calling this method
            var originalPath = path;

            // Simple path navigation (e.g., "id" or "name.first" or "login.uuid")
            var parts = path.Split('.');
            var current = element;

            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part))
                    continue;

                // Try exact match first (case-sensitive)
                if (current.ValueKind == JsonValueKind.Object && current.TryGetProperty(part, out var property))
                {
                    current = property;
                    continue;
                }

                // Try case-insensitive match
                if (current.ValueKind == JsonValueKind.Object)
                {
                    var found = false;
                    foreach (var prop in current.EnumerateObject())
                    {
                        if (string.Equals(prop.Name, part, StringComparison.OrdinalIgnoreCase))
                        {
                            current = prop.Value;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        // Log available properties for debugging
                        var availableProps = string.Join(", ", current.EnumerateObject().Select(p => p.Name));
                        _logger.LogWarning("Property '{Part}' not found in path '{Path}'. Available properties: {Properties}", 
                            part, originalPath, availableProps);
                        return "";
                    }
                }
                else
                {
                    _logger.LogWarning("Cannot navigate to '{Part}' in path '{Path}'. Current element is not an object (ValueKind: {ValueKind})", 
                        part, originalPath, current.ValueKind);
                    return "";
                }
            }

            // Extract value based on type
            var result = current.ValueKind switch
            {
                JsonValueKind.String => current.GetString() ?? "",
                JsonValueKind.Number => current.GetRawText().Trim(),
                JsonValueKind.True => "true",
                JsonValueKind.False => "false",
                JsonValueKind.Null => "",
                JsonValueKind.Undefined => "",
                _ => current.GetRawText().Trim()
            };

            return result;
        }

        private string? ExtractArrayPath(string valuePath, string textPath)
        {
            // Extract array path from paths like "results[].id" or "data[].name"
            var paths = new[] { valuePath, textPath };
            
            foreach (var path in paths)
            {
                if (string.IsNullOrEmpty(path))
                    continue;

                // Check for array notation
                var arrayMatch = Regex.Match(path, @"^([^\[\]]+)\[\]");
                if (arrayMatch.Success)
                {
                    return arrayMatch.Groups[1].Value;
                }
            }

            return null;
        }

        private JsonElement? NavigateToJsonElement(JsonElement root, string path)
        {
            if (string.IsNullOrEmpty(path))
                return root;

            var parts = path.Split('.');
            var current = root;

            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part))
                    continue;

                if (current.ValueKind == JsonValueKind.Object && current.TryGetProperty(part, out var property))
                {
                    current = property;
                }
                else
                {
                    return null;
                }
            }

            return current;
        }

        private JsonElement? FindFirstArray(JsonElement element, int maxDepth = 5, int currentDepth = 0)
        {
            if (currentDepth >= maxDepth)
                return null;

            // Recursively search for first array
            if (element.ValueKind == JsonValueKind.Array && element.GetArrayLength() > 0)
            {
                return element;
            }

            if (element.ValueKind == JsonValueKind.Object)
            {
                foreach (var prop in element.EnumerateObject())
                {
                    // Skip metadata properties
                    if (prop.Name.Equals("info", StringComparison.OrdinalIgnoreCase) ||
                        prop.Name.Equals("meta", StringComparison.OrdinalIgnoreCase) ||
                        prop.Name.Equals("pagination", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var result = FindFirstArray(prop.Value, maxDepth, currentDepth + 1);
                    if (result.HasValue)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        private string GetResponseStructure(JsonElement element, int maxDepth = 3, int currentDepth = 0)
        {
            if (currentDepth >= maxDepth)
                return "...";

            return element.ValueKind switch
            {
                JsonValueKind.Array => $"[Array with {element.GetArrayLength()} items]",
                JsonValueKind.Object => $"{{ {string.Join(", ", element.EnumerateObject().Take(5).Select(p => $"\"{p.Name}\": {GetResponseStructure(p.Value, maxDepth, currentDepth + 1)}"))} }}",
                JsonValueKind.String => $"\"{element.GetString()?.Substring(0, Math.Min(50, element.GetString()?.Length ?? 0))}...\"",
                JsonValueKind.Number => element.GetRawText(),
                JsonValueKind.True => "true",
                JsonValueKind.False => "false",
                JsonValueKind.Null => "null",
                _ => element.GetRawText()
            };
        }

        /// <summary>
        /// Combines Base URL and API Path to form full URL
        /// Supports flexible input: full URL or Base URL + Path
        /// </summary>
        private string CombineApiUrl(string baseUrl, string? apiPath)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentException("Base URL cannot be empty");
            }

            // If no path provided, use baseUrl as full URL (supports any random URL)
            // User can enter any URL directly in apiUrl field
            if (string.IsNullOrWhiteSpace(apiPath))
            {
                return baseUrl.Trim();
            }

            // If path is provided, combine Base URL + Path
            // Normalize: remove trailing slash from base, leading slash from path
            baseUrl = baseUrl.TrimEnd('/');
            apiPath = apiPath.TrimStart('/');

            // Combine: baseUrl + "/" + apiPath
            return $"{baseUrl}/{apiPath}";
        }

        private ApiResponse ConvertToApiResponse<T>(ServiceResult<T> result)
        {
            if (result.Success)
            {
                return new ApiResponse(result.StatusCode, "Operation completed successfully", result.Data);
            }
            else if (result.StatusCode == 404)
            {
                return new ApiResponse(404, result.ErrorMessage ?? "Resource not found");
            }
            else
            {
                return new ApiResponse(result.StatusCode, result.ErrorMessage ?? "Operation failed");
            }
        }
    }
}
