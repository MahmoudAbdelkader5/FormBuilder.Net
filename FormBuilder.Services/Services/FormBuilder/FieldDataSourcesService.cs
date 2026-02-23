using formBuilder.Domian.Interfaces;
using FormBuilder.API.Models;
using FormBuilder.Domian.Entitys.froms;
using FormBuilder.Core.IServices.FormBuilder;
using FormBuilder.Domain.Interfaces;
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
using System.Security.Authentication;
using System.Xml.Linq;

namespace FormBuilder.Services.Services
{
    public class FieldDataSourcesService : BaseService<FIELD_DATA_SOURCES, FieldDataSourceDto, CreateFieldDataSourceDto, UpdateFieldDataSourceDto>, IFieldDataSourcesService
    {
        private readonly IunitOfwork _unitOfWork;
        private readonly IFieldDataSourcesRepository _fieldDataSourcesRepository;
        private readonly AkhmanageItContext _akhmanageItContext;
        private readonly FormBuilderDbContext _formBuilderDbContext;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<FieldDataSourcesService> _logger;
        private readonly ISapHanaService? _sapHanaService;
        private readonly ISapHanaConfigsService _sapHanaConfigsService;

        public FieldDataSourcesService(
            IunitOfwork unitOfWork, 
            IFieldDataSourcesRepository fieldDataSourcesRepository, 
            AkhmanageItContext akhmanageItContext,
            FormBuilderDbContext formBuilderDbContext,
            IHttpClientFactory httpClientFactory,
            ILogger<FieldDataSourcesService> logger,
            IMapper mapper,
            ISapHanaConfigsService sapHanaConfigsService,
            ISapHanaService? sapHanaService = null) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _fieldDataSourcesRepository = fieldDataSourcesRepository ?? throw new ArgumentNullException(nameof(fieldDataSourcesRepository));
            _akhmanageItContext = akhmanageItContext ?? throw new ArgumentNullException(nameof(akhmanageItContext));
            _formBuilderDbContext = formBuilderDbContext ?? throw new ArgumentNullException(nameof(formBuilderDbContext));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sapHanaConfigsService = sapHanaConfigsService ?? throw new ArgumentNullException(nameof(sapHanaConfigsService));
            _sapHanaService = sapHanaService;
        }

        protected override IBaseRepository<FIELD_DATA_SOURCES> Repository => _unitOfWork.FieldDataSourcesRepository;

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

        public async Task<ApiResponse> GetByFieldIdAsync(int fieldId)
        {
            var dataSources = await _fieldDataSourcesRepository.GetByFieldIdAsync(fieldId);
            var dataSourceDtos = _mapper.Map<IEnumerable<FieldDataSourceDto>>(dataSources);
            return new ApiResponse(200, "Field data sources retrieved successfully", dataSourceDtos);
        }

        public async Task<ApiResponse> GetActiveByFieldIdAsync(int fieldId)
        {
            var dataSources = await _fieldDataSourcesRepository.GetActiveByFieldIdAsync(fieldId);
            var dataSourceDtos = _mapper.Map<IEnumerable<FieldDataSourceDto>>(dataSources);
            return new ApiResponse(200, "Active field data sources retrieved successfully", dataSourceDtos);
        }

        public async Task<ApiResponse> CreateAsync(CreateFieldDataSourceDto createDto)
        {
            createDto.SourceType = NormalizeSourceType(createDto.SourceType);

            // If ConfigurationJson is provided, use it; otherwise, build it from individual fields
            if (string.IsNullOrEmpty(createDto.ConfigurationJson))
            {
                createDto.ConfigurationJson = BuildConfigurationJson(createDto);
            }

            var result = await base.CreateAsync(createDto);
            
            // If DataSource is Api, LookupTable, or SqlQuery, delete all existing options for this field
            if (result.Success && (string.Equals(createDto.SourceType, "Api", StringComparison.OrdinalIgnoreCase) ||
                                  string.Equals(createDto.SourceType, "LookupTable", StringComparison.OrdinalIgnoreCase) ||
                                  string.Equals(createDto.SourceType, "SqlQuery", StringComparison.OrdinalIgnoreCase) ||
                                  string.Equals(createDto.SourceType, "SapHana", StringComparison.OrdinalIgnoreCase) ||
                                  string.Equals(createDto.SourceType, "Sap", StringComparison.OrdinalIgnoreCase)))
            {
                await DeleteAllOptionsForFieldAsync(createDto.FieldId);
            }
            
            return ConvertToApiResponse(result);
        }

        private string? BuildConfigurationJson(CreateFieldDataSourceDto dto)
        {
            try
            {
                if (dto.SourceType.Equals("LookupTable", StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(dto.ApiUrl) && !string.IsNullOrEmpty(dto.ValuePath) && !string.IsNullOrEmpty(dto.TextPath))
                    {
                        var config = new Dictionary<string, object?>
                        {
                            ["table"] = dto.ApiUrl,
                            ["valueColumn"] = dto.ValuePath,
                            ["textColumn"] = dto.TextPath
                        };
                        // Add database if provided (default: FormBuilder)
                        if (!string.IsNullOrEmpty(dto.RequestBodyJson))
                        {
                            // RequestBodyJson can be used to store database name
                            config["database"] = dto.RequestBodyJson;
                        }
                        else
                        {
                            // Default to FormBuilder
                            config["database"] = "FormBuilder";
                        }
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
                else if (dto.SourceType.Equals("SqlQuery", StringComparison.OrdinalIgnoreCase))
                {
                    // For SqlQuery, store SQL query and column mappings
                    var sqlQuery = dto.SqlQuery ?? dto.RequestBodyJson ?? string.Empty;
                    
                    // Build config even if some fields are empty (will be validated later)
                    // This prevents warnings in frontend when user is still typing
                    var config = new
                    {
                        sqlQuery = sqlQuery,
                        valueColumn = dto.ValuePath ?? string.Empty,
                        textColumn = dto.TextPath ?? string.Empty
                    };
                    
                    // Only return JSON if at least SQL query is provided
                    if (!string.IsNullOrWhiteSpace(sqlQuery))
                    {
                        return System.Text.Json.JsonSerializer.Serialize(config);
                    }
                }
                else if (dto.SourceType.Equals("SapHana", StringComparison.OrdinalIgnoreCase) || 
                         dto.SourceType.Equals("SAP", StringComparison.OrdinalIgnoreCase))
                {
                    // For SAP HANA, store connection details and SQL query
                    // Connection details can be in ApiUrl (as JSON) or ConfigurationJson
                    // SQL query in SqlQuery or RequestBodyJson
                    var sqlQuery = dto.SqlQuery ?? dto.RequestBodyJson ?? string.Empty;
                    
                    var config = new Dictionary<string, object?>();
                    
                    // Parse connection from ApiUrl if it's JSON
                    if (!string.IsNullOrEmpty(dto.ApiUrl))
                    {
                        try
                        {
                            var connectionJson = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(dto.ApiUrl);
                            if (connectionJson != null)
                            {
                                config["connection"] = connectionJson;
                            }
                        }
                        catch
                        {
                            // ApiUrl is not JSON, ignore
                        }
                    }
                    
                    config["sqlQuery"] = sqlQuery;
                    config["valueColumn"] = dto.ValuePath ?? string.Empty;
                    config["textColumn"] = dto.TextPath ?? string.Empty;
                    
                    // Only return JSON if at least SQL query is provided
                    if (!string.IsNullOrWhiteSpace(sqlQuery))
                    {
                        return System.Text.Json.JsonSerializer.Serialize(config);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to build ConfigurationJson from individual fields");
            }

            return null;
        }

        protected override async Task<ValidationResult> ValidateCreateAsync(CreateFieldDataSourceDto dto)
        {
            dto.SourceType = NormalizeSourceType(dto.SourceType);

            // Validate if field exists
            var fieldExists = await _unitOfWork.FormFieldRepository.AnyAsync(x => x.Id == dto.FieldId);
            if (!fieldExists)
                return ValidationResult.Failure("Invalid field ID");

            // Validate SqlQuery data source requirements
            if (string.Equals(dto.SourceType, "SqlQuery", StringComparison.OrdinalIgnoreCase))
            {
                // Check if SQL query is provided (either in SqlQuery field or RequestBodyJson)
                var hasSqlQuery = !string.IsNullOrWhiteSpace(dto.SqlQuery) || !string.IsNullOrWhiteSpace(dto.RequestBodyJson);
                
                // Check if ConfigurationJson is provided (which might contain sqlQuery)
                var hasConfigJson = !string.IsNullOrWhiteSpace(dto.ConfigurationJson);
                
                // If neither SqlQuery/RequestBodyJson nor ConfigurationJson is provided, it's invalid
                if (!hasSqlQuery && !hasConfigJson)
                {
                    return ValidationResult.Failure("SQL query is required for SqlQuery source type. Please provide it in SqlQuery field, RequestBodyJson, or ConfigurationJson.");
                }

                // Validate ValuePath and TextPath are provided
                if (string.IsNullOrWhiteSpace(dto.ValuePath))
                {
                    return ValidationResult.Failure("ValuePath is required for SqlQuery source type. Please specify the column name for the value.");
                }

                if (string.IsNullOrWhiteSpace(dto.TextPath))
                {
                    return ValidationResult.Failure("TextPath is required for SqlQuery source type. Please specify the column name for the text.");
                }
            }

            return ValidationResult.Success();
        }

        public async Task<ApiResponse> CreateBulkAsync(List<CreateFieldDataSourceDto> createDtos)
        {
            if (createDtos == null || !createDtos.Any())
                return new ApiResponse(400, "No field data sources provided");

            // Build ConfigurationJson for each DTO if not provided
            foreach (var dto in createDtos)
            {
                if (string.IsNullOrEmpty(dto.ConfigurationJson))
                {
                    dto.ConfigurationJson = BuildConfigurationJson(dto);
                }
            }

            // Validate all field IDs exist
            var fieldIds = createDtos.Select(d => d.FieldId).Distinct().ToList();
            foreach (var fieldId in fieldIds)
            {
                var fieldExists = await _unitOfWork.FormFieldRepository.AnyAsync(f => f.Id == fieldId);
                if (!fieldExists)
                    return new ApiResponse(400, $"Invalid field ID: {fieldId}");
            }

            // Validate each DTO
            foreach (var dto in createDtos)
            {
                var validation = await ValidateCreateAsync(dto);
                if (!validation.IsValid)
                    return new ApiResponse(400, validation.ErrorMessage ?? "Validation failed");
            }

            var entities = _mapper.Map<List<FIELD_DATA_SOURCES>>(createDtos);
            foreach (var entity in entities)
            {
                entity.CreatedDate = entity.CreatedDate == default ? DateTime.UtcNow : entity.CreatedDate;
                entity.IsActive = true;
            }

            _unitOfWork.FieldDataSourcesRepository.AddRange(entities);
            await _unitOfWork.CompleteAsyn();

            var resultDtos = _mapper.Map<IEnumerable<FieldDataSourceDto>>(entities);
            return new ApiResponse(200, "Field data sources created successfully", resultDtos);
        }

        public async Task<ApiResponse> UpdateAsync(int id, UpdateFieldDataSourceDto updateDto)
        {
            updateDto.SourceType = NormalizeSourceType(updateDto.SourceType);

            // Get existing DataSource to check if sourceType changed
            var existingDataSource = await Repository.SingleOrDefaultAsync(e => e.Id == id, asNoTracking: true);
            string? oldSourceType = existingDataSource?.SourceType;
            
            // If ConfigurationJson is provided, use it; otherwise, build it from individual fields
            if (string.IsNullOrEmpty(updateDto.ConfigurationJson))
            {
                // Convert UpdateFieldDataSourceDto to CreateFieldDataSourceDto for building JSON
                var createDto = new CreateFieldDataSourceDto
                {
                    SourceType = updateDto.SourceType,
                    ApiUrl = updateDto.ApiUrl,
                    ApiPath = updateDto.ApiPath,
                    HttpMethod = updateDto.HttpMethod,
                    RequestBodyJson = updateDto.RequestBodyJson,
                    SqlQuery = updateDto.SqlQuery,
                    ValuePath = updateDto.ValuePath,
                    TextPath = updateDto.TextPath
                };
                updateDto.ConfigurationJson = BuildConfigurationJson(createDto);
            }

            var result = await base.UpdateAsync(id, updateDto);
            
            // If sourceType changed from Static to Api/LookupTable, delete all options
            if (result.Success && existingDataSource != null)
            {
                bool wasStatic = string.Equals(oldSourceType, "Static", StringComparison.OrdinalIgnoreCase);
                bool isNowExternal = string.Equals(updateDto.SourceType, "Api", StringComparison.OrdinalIgnoreCase) ||
                                     string.Equals(updateDto.SourceType, "LookupTable", StringComparison.OrdinalIgnoreCase) ||
                                     string.Equals(updateDto.SourceType, "SqlQuery", StringComparison.OrdinalIgnoreCase);
                
                if (wasStatic && isNowExternal)
                {
                    await DeleteAllOptionsForFieldAsync(existingDataSource.FieldId);
                }
                // Also delete if directly setting to Api/LookupTable/SqlQuery (even if old was null)
                else if (isNowExternal && (oldSourceType == null || !string.Equals(oldSourceType, "Api", StringComparison.OrdinalIgnoreCase) && 
                                            !string.Equals(oldSourceType, "LookupTable", StringComparison.OrdinalIgnoreCase) &&
                                            !string.Equals(oldSourceType, "SqlQuery", StringComparison.OrdinalIgnoreCase)))
                {
                    await DeleteAllOptionsForFieldAsync(existingDataSource.FieldId);
                }
            }
            
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var result = await base.DeleteAsync(id);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> SoftDeleteAsync(int id)
        {
            var result = await base.SoftDeleteAsync(id);
            return ConvertToApiResponse(result);
        }

        public async Task<ApiResponse> GetByFieldIdAndTypeAsync(int fieldId, string sourceType)
        {
            sourceType = NormalizeSourceType(sourceType);
            var dataSource = await _fieldDataSourcesRepository.GetByFieldIdAsync(fieldId, sourceType);
            if (dataSource == null)
                return new ApiResponse(404, "Field data source not found for the specified type");

            var dataSourceDto = _mapper.Map<FieldDataSourceDto>(dataSource);
            return new ApiResponse(200, "Field data source retrieved successfully", dataSourceDto);
        }

        public async Task<ApiResponse> GetDataSourcesCountAsync(int fieldId)
        {
            var count = await _fieldDataSourcesRepository.GetDataSourcesCountAsync(fieldId);
            return new ApiResponse(200, "Data sources count retrieved successfully", count);
        }

        // ================================
        // FIELD OPTIONS METHODS
        // ================================

        public async Task<ApiResponse> GetFieldOptionsAsync(int fieldId, Dictionary<string, object>? context = null, string? requestBodyJson = null)
        {
            try
            {
                // Get active data source for the field
                var dataSource = await _fieldDataSourcesRepository.GetActiveByFieldIdAsync(fieldId);
                var activeDataSource = dataSource.FirstOrDefault(ds => ds.IsActive);

                if (activeDataSource == null)
                {
                    // No data source configured, return static options from FIELD_OPTIONS
                    var staticOptions = await GetStaticOptionsAsync(fieldId);
                    return new ApiResponse(200, "Field options retrieved successfully", staticOptions);
                }

                // Load options based on source type
                List<FieldOptionResponseDto> responseOptions;
                var sourceType = activeDataSource.SourceType?.Trim().ToUpper() ?? string.Empty;
                
                // Log for debugging
                _logger.LogInformation("Processing field options for field {FieldId}, SourceType: '{SourceType}' (normalized: '{NormalizedSourceType}')", 
                    fieldId, activeDataSource.SourceType, sourceType);
                
                switch (sourceType)
                {
                    case "STATIC":
                        responseOptions = await GetStaticOptionsAsync(fieldId);
                        break;

                    case "LOOKUPTABLE":
                        // For LookupTable, fetch options from database table (do NOT save to FIELD_OPTIONS)
                        responseOptions = await GetLookupTableOptionsAsync(activeDataSource, context);
                        break;

                    case "API":
                        // For Api, fetch options from external API (do NOT save to FIELD_OPTIONS)
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

                    case "SQLQUERY":
                    case "DATASOURCESQLQUERY":
                        // For SqlQuery, execute custom SQL query to fetch options
                        responseOptions = await GetSqlQueryOptionsAsync(activeDataSource, context);
                        break;

                    case "SAPHANA":
                    case "SAP":
                        // For SAP HANA, execute query on SAP HANA database
                        if (_sapHanaService == null)
                        {
                            _logger.LogError("SAP HANA service is not registered");
                            return new ApiResponse(500, "SAP HANA service is not available");
                        }
                        responseOptions = await GetSapHanaOptionsAsync(activeDataSource, context);
                        break;

                    default:
                        _logger.LogWarning("Unsupported source type: {SourceType} for field {FieldId}. Available types: Static, LookupTable, Api, SqlQuery, SapHana", 
                            activeDataSource.SourceType, fieldId);
                        return new ApiResponse(400, $"Unsupported source type: {activeDataSource.SourceType}. Supported types: Static, LookupTable, Api, SqlQuery, SapHana");
                }

                // Return FieldOptionResponseDto (with 'text' and 'value' properties) for frontend compatibility
                return new ApiResponse(200, "Field options retrieved successfully", responseOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving field options for field {FieldId}", fieldId);
                return new ApiResponse(500, $"Error retrieving field options: {ex.Message}");
            }
        }

        public async Task<ApiResponse> PreviewDataSourceAsync(PreviewDataSourceRequestDto request)
        {
            try
            {
                request.SourceType = NormalizeSourceType(request.SourceType);
                List<FieldOptionResponseDto> options;
                switch (request.SourceType.ToUpper())
                {
                    case "STATIC":
                        if (request.FieldId.HasValue)
                        {
                            options = await GetStaticOptionsAsync(request.FieldId.Value);
                        }
                        else
                        {
                            return new ApiResponse(400, "FieldId is required for Static source type");
                        }
                        break;

                    case "LOOKUPTABLE":
                        if (string.IsNullOrEmpty(request.ApiUrl) || string.IsNullOrEmpty(request.ValuePath) || string.IsNullOrEmpty(request.TextPath))
                        {
                            return new ApiResponse(400, "Table name, ValuePath, and TextPath are required for LookupTable source type");
                        }
                        var lookupDataSource = new FIELD_DATA_SOURCES
                        {
                            ApiUrl = request.ApiUrl,
                            ValuePath = request.ValuePath,
                            TextPath = request.TextPath
                        };
                        options = await GetLookupTableOptionsAsync(lookupDataSource, null);
                        break;

                    case "API":
                        if (string.IsNullOrEmpty(request.ApiUrl))
                        {
                            return new ApiResponse(400, "ApiUrl is required for API source type");
                        }
                        
                        // If valuePath or textPath are not provided, auto-detect them using InspectApi
                        string valuePath = request.ValuePath ?? string.Empty;
                        string textPath = request.TextPath ?? string.Empty;
                        
                        if (string.IsNullOrEmpty(valuePath) || string.IsNullOrEmpty(textPath))
                        {
                            _logger.LogInformation("valuePath or textPath not provided, auto-detecting from API structure...");
                            
                            // Auto-inspect API to get suggested paths
                            var inspectRequest = new InspectApiRequestDto
                            {
                                ApiUrl = request.ApiUrl,
                                ApiPath = request.ApiPath,
                                HttpMethod = request.HttpMethod ?? "GET",
                                RequestBodyJson = request.RequestBodyJson,
                                ArrayPropertyNames = request.ArrayPropertyNames
                            };
                            
                            var inspectResult = await InspectApiAsync(inspectRequest);
                            
                            if (inspectResult.StatusCode == 200 && inspectResult.Data != null)
                            {
                                var inspectionData = System.Text.Json.JsonSerializer.Deserialize<ApiInspectionResponseDto>(
                                    System.Text.Json.JsonSerializer.Serialize(inspectResult.Data));
                                
                                if (inspectionData != null && inspectionData.Success)
                                {
                                    // Use suggested paths if available
                                    if (string.IsNullOrEmpty(valuePath) && inspectionData.SuggestedValuePaths.Any())
                                    {
                                        valuePath = inspectionData.SuggestedValuePaths.First();
                                        _logger.LogInformation("Auto-detected valuePath: {ValuePath}", valuePath);
                                    }
                                    
                                    if (string.IsNullOrEmpty(textPath) && inspectionData.SuggestedTextPaths.Any())
                                    {
                                        textPath = inspectionData.SuggestedTextPaths.First();
                                        _logger.LogInformation("Auto-detected textPath: {TextPath}", textPath);
                                    }
                                    
                                    // If still empty, try availableFields
                                    if (string.IsNullOrEmpty(valuePath) && inspectionData.AvailableFields.Any())
                                    {
                                        valuePath = inspectionData.AvailableFields.First();
                                        _logger.LogInformation("Using first available field as valuePath: {ValuePath}", valuePath);
                                    }
                                    
                                    if (string.IsNullOrEmpty(textPath) && inspectionData.AvailableFields.Count > 1)
                                    {
                                        textPath = inspectionData.AvailableFields.Skip(1).First();
                                        _logger.LogInformation("Using second available field as textPath: {TextPath}", textPath);
                                    }
                                    
                                    // If still empty, return helpful error with available fields
                                    if (string.IsNullOrEmpty(valuePath) || string.IsNullOrEmpty(textPath))
                                    {
                                        var availableFieldsMsg = inspectionData.AvailableFields.Any() 
                                            ? $"Available fields: {string.Join(", ", inspectionData.AvailableFields)}" 
                                            : "No fields found in API response";
                                        var nestedFieldsMsg = inspectionData.NestedFields.Any() 
                                            ? $"Nested fields: {string.Join(", ", inspectionData.NestedFields)}" 
                                            : "";
                                        
                                        return new ApiResponse(400, 
                                            $"Could not auto-detect valuePath or textPath. {availableFieldsMsg}. {nestedFieldsMsg}. " +
                                            $"Please provide valuePath and textPath manually, or use /inspect-api endpoint first to see available fields.");
                                    }
                                }
                                else
                                {
                                    return new ApiResponse(400, 
                                        $"Failed to inspect API structure: {inspectionData?.ErrorMessage ?? "Unknown error"}. " +
                                        $"Please provide valuePath and textPath manually.");
                                }
                            }
                            else
                            {
                                return new ApiResponse(400, 
                                    "Could not auto-detect valuePath and textPath. Please provide them manually or use /inspect-api endpoint first.");
                            }
                        }
                        
                        var apiDataSource = new FIELD_DATA_SOURCES
                        {
                            ApiUrl = request.ApiUrl,
                            ApiPath = request.ApiPath,
                            HttpMethod = request.HttpMethod ?? "GET",
                            RequestBodyJson = request.RequestBodyJson,
                            ValuePath = valuePath,
                            TextPath = textPath
                        };
                        options = await GetApiOptionsAsync(apiDataSource, request.RequestBodyJson, null, request.ArrayPropertyNames, request.SapConfigId);
                        break;

                    case "SQLQUERY":
                    case "DATASOURCESQLQUERY":
                        // For SqlQuery, execute custom SQL query to fetch options
                        // Also check ConfigurationJson if RequestBodyJson is empty
                        string sqlQuery = request.RequestBodyJson ?? string.Empty;
                        string valueColumn = request.ValuePath ?? string.Empty;
                        string textColumn = request.TextPath ?? string.Empty;
                        
                        // Try to get SQL query from ConfigurationJson if RequestBodyJson is empty
                        if (string.IsNullOrWhiteSpace(sqlQuery) && !string.IsNullOrEmpty(request.ConfigurationJson))
                        {
                            try
                            {
                                var config = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(request.ConfigurationJson);
                                if (config != null)
                                {
                                    if (config.ContainsKey("sqlQuery"))
                                        sqlQuery = config["sqlQuery"]?.ToString() ?? string.Empty;
                                    else if (config.ContainsKey("sqlquery"))
                                        sqlQuery = config["sqlquery"]?.ToString() ?? string.Empty;
                                    else if (config.ContainsKey("SQLQuery"))
                                        sqlQuery = config["SQLQuery"]?.ToString() ?? string.Empty;
                                    
                                    if (config.ContainsKey("valueColumn"))
                                        valueColumn = config["valueColumn"]?.ToString() ?? valueColumn;
                                    else if (config.ContainsKey("valuecolumn"))
                                        valueColumn = config["valuecolumn"]?.ToString() ?? valueColumn;
                                    else if (config.ContainsKey("ValueColumn"))
                                        valueColumn = config["ValueColumn"]?.ToString() ?? valueColumn;
                                    
                                    if (config.ContainsKey("textColumn"))
                                        textColumn = config["textColumn"]?.ToString() ?? textColumn;
                                    else if (config.ContainsKey("textcolumn"))
                                        textColumn = config["textcolumn"]?.ToString() ?? textColumn;
                                    else if (config.ContainsKey("TextColumn"))
                                        textColumn = config["TextColumn"]?.ToString() ?? textColumn;
                                }
                            }
                            catch
                            {
                                // If parsing fails, use the original values
                            }
                        }
                        
                        if (string.IsNullOrWhiteSpace(sqlQuery))
                        {
                            return new ApiResponse(400, "SQL query is required. Please provide it in RequestBodyJson or ConfigurationJson.");
                        }
                        
                        if (string.IsNullOrWhiteSpace(valueColumn))
                        {
                            return new ApiResponse(400, "Value column name is required. Please provide it in ValuePath or ConfigurationJson.");
                        }
                        
                        if (string.IsNullOrWhiteSpace(textColumn))
                        {
                            return new ApiResponse(400, "Text column name is required. Please provide it in TextPath or ConfigurationJson.");
                        }
                        
                        var sqlDataSource = new FIELD_DATA_SOURCES
                        {
                            RequestBodyJson = sqlQuery,
                            ConfigurationJson = request.ConfigurationJson,
                            ValuePath = valueColumn,
                            TextPath = textColumn
                        };
                        options = await GetSqlQueryOptionsAsync(sqlDataSource, null);
                        break;

                    case "SAPHANA":
                    case "SAP":
                        // For SAP HANA, execute query on SAP HANA using active connection from DB
                        if (_sapHanaService == null)
                        {
                            return new ApiResponse(500, "SAP HANA service is not available");
                        }

                        var sapDataSource = new FIELD_DATA_SOURCES
                        {
                            SourceType = "SapHana",
                            RequestBodyJson = request.RequestBodyJson,
                            ConfigurationJson = request.ConfigurationJson,
                            ValuePath = request.ValuePath,
                            TextPath = request.TextPath,
                            ApiUrl = request.ApiUrl // optional legacy (ignored if active config exists)
                        };

                        options = await GetSapHanaOptionsAsync(sapDataSource, null);
                        break;

                    default:
                        return new ApiResponse(400, $"Unsupported source type: {request.SourceType}");
                }

                return new ApiResponse(200, "Preview successful", options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error previewing data source");
                // Ensure error message is safe for JSON serialization
                var errorMessage = SanitizeForJson(ex.Message ?? "An unknown error occurred");
                return new ApiResponse(500, $"Error previewing data source: {errorMessage}");
            }
        }

        // ================================
        // PRIVATE HELPER METHODS
        // ================================

        internal static string NormalizeSourceType(string? sourceType)
        {
            var value = (sourceType ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(value))
                return value;

            return value.ToUpperInvariant() switch
            {
                "STATIC" => "Static",
                "API" => "Api",
                "LOOKUPTABLE" => "LookupTable",
                "SQLQUERY" => "SqlQuery",
                "DATASOURCESQLQUERY" => "SqlQuery",
                "SAPHANA" => "SapHana",
                "SAP" => "SapHana",
                _ => value
            };
        }

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

        private async Task<List<FieldOptionResponseDto>> GetStaticOptionsAsync(int fieldId)
        {
            var options = await _unitOfWork.FieldOptionsRepository.GetActiveByFieldIdAsync(fieldId);
            return options
                .OrderBy(o => o.OptionOrder)
                .Select(o => new FieldOptionResponseDto
                {
                    Value = o.OptionValue,
                    Text = o.OptionText
                })
                .ToList();
        }

        private async Task<List<FieldOptionResponseDto>> GetLookupTableOptionsAsync(FIELD_DATA_SOURCES dataSource, Dictionary<string, object>? context)
        {
            string tableName;
            string valueColumn;
            string textColumn;
            string database = "FormBuilder"; // Default to FormBuilder

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
                        database = config.ContainsKey("database") ? config["database"]?.ToString() ?? "FormBuilder" : "FormBuilder";
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
                    _logger.LogError(ex, "Invalid JSON in ConfigurationJson for field data source {DataSourceId}. Error: {ErrorMessage}", 
                        dataSource.Id, ex.Message);
                    throw new ArgumentException($"Invalid JSON in ConfigurationJson: {ex.Message}. Please check the JSON format.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error parsing ConfigurationJson for field data source {DataSourceId}", dataSource.Id);
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

            _logger.LogInformation("GetLookupTableOptionsAsync - FieldId: {FieldId}, Table: {TableName}, ValueColumn: {ValueColumn}, TextColumn: {TextColumn}, Database: {Database}, ConfigurationJson: {ConfigJson}", 
                dataSource.FieldId, tableName, valueColumn, textColumn, database, dataSource.ConfigurationJson ?? "null");

            async Task EnsureCanConnectAsync(DbContext ctx, string ctxName)
            {
                try
                {
                    if (!await ctx.Database.CanConnectAsync())
                    {
                        _logger.LogError("Database connection failed for LookupTable query on {DbContextName}", ctxName);
                        throw new InvalidOperationException($"Database connection failed for {ctxName}. Please check your database connection.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking database connection for {DbContextName}", ctxName);
                    throw new InvalidOperationException($"Database connection error for {ctxName}: {ex.Message}");
                }
            }

            async Task<List<FieldOptionResponseDto>> QueryViaReflectionAsync(DbContext ctx, Type ctxType, string ctxName)
            {
                await EnsureCanConnectAsync(ctx, ctxName);

                var dbSetProperty = ctxType.GetProperty(tableName, System.Reflection.BindingFlags.IgnoreCase |
                                                                  System.Reflection.BindingFlags.Public |
                                                                  System.Reflection.BindingFlags.Instance);

                if (dbSetProperty == null)
                {
                    // Try to find table with different case
                    var allProperties = ctxType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    var matchingProperty = allProperties.FirstOrDefault(p =>
                        string.Equals(p.Name, tableName, StringComparison.OrdinalIgnoreCase));

                    if (matchingProperty == null)
                    {
                        var availableTables = string.Join(", ", allProperties
                            .Where(p => p.PropertyType.IsGenericType &&
                                       p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                            .Select(p => p.Name));

                        _logger.LogError("Table '{TableName}' not found in {DbContextName}. Available tables: {AvailableTables}",
                            tableName, ctxName, availableTables);
                        throw new ArgumentException($"Table '{tableName}' not found in {ctxName}. Available tables: {availableTables}");
                    }

                    dbSetProperty = matchingProperty;
                }

                // Get the DbSet value from the context
                var dbSetValue = dbSetProperty.GetValue(ctx);
                if (dbSetValue == null)
                {
                    throw new InvalidOperationException($"DbSet '{tableName}' is null in {ctxName}");
                }

                // Get the generic type of the DbSet (e.g., TblCustomer)
                var dbSetType = dbSetProperty.PropertyType;
                if (!dbSetType.IsGenericType || dbSetType.GetGenericTypeDefinition() != typeof(DbSet<>))
                {
                    throw new InvalidOperationException($"Property '{tableName}' is not a DbSet");
                }

                var entityType = dbSetType.GetGenericArguments()[0];

                // Use reflection to call QueryTableAsync with the correct type
                var method = typeof(FieldDataSourcesService)
                    .GetMethod(nameof(QueryTableAsync), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.MakeGenericMethod(entityType);

                if (method == null)
                {
                    throw new InvalidOperationException("QueryTableAsync method not found");
                }

                var task = (Task<List<FieldOptionResponseDto>>)method.Invoke(this, new[] { dbSetValue, valueColumn, textColumn, context })!;
                return await task;
            }

            async Task<List<FieldOptionResponseDto>> QueryWithFallbackAsync()
            {
                // List of all databases to try (in order)
                var databasesToTry = new List<(DbContext ctx, Type ctxType, string ctxName, string dbKey)>();

                // If database is specified, try it first; otherwise try both in order
                if (!string.IsNullOrWhiteSpace(database) && database.Equals("AkhmanageIt", StringComparison.OrdinalIgnoreCase))
                {
                    // Try AkhmanageIt first if specified
                    databasesToTry.Add((_akhmanageItContext, typeof(AkhmanageItContext), "AkhmanageItContext", "AkhmanageIt"));
                    databasesToTry.Add((_formBuilderDbContext, typeof(FormBuilderDbContext), "FormBuilderDbContext", "FormBuilder"));
                }
                else if (!string.IsNullOrWhiteSpace(database) && database.Equals("FormBuilder", StringComparison.OrdinalIgnoreCase))
                {
                    // Try FormBuilder first if specified
                    databasesToTry.Add((_formBuilderDbContext, typeof(FormBuilderDbContext), "FormBuilderDbContext", "FormBuilder"));
                    databasesToTry.Add((_akhmanageItContext, typeof(AkhmanageItContext), "AkhmanageItContext", "AkhmanageIt"));
                }
                else
                {
                    // If database not specified or unknown, try both databases automatically
                    _logger.LogInformation("Database not specified or unknown ('{Database}') for table '{TableName}'. Will try both databases automatically.", database, tableName);
                    databasesToTry.Add((_formBuilderDbContext, typeof(FormBuilderDbContext), "FormBuilderDbContext", "FormBuilder"));
                    databasesToTry.Add((_akhmanageItContext, typeof(AkhmanageItContext), "AkhmanageItContext", "AkhmanageIt"));
                }

                Exception? lastException = null;
                string? lastTriedDb = null;

                // Try reflection method first for each database
                foreach (var (ctx, ctxType, ctxName, dbKey) in databasesToTry)
                {
                    try
                    {
                        _logger.LogInformation("Attempting to find table '{TableName}' in {DbContextName} using reflection.", tableName, ctxName);
                        var result = await QueryViaReflectionAsync(ctx, ctxType, ctxName);
                        _logger.LogInformation("Successfully found table '{TableName}' in {DbContextName}. Returning {Count} options.", tableName, ctxName, result.Count);
                        return result;
                    }
                    catch (ArgumentException ex) when (ex.Message.Contains("Table", StringComparison.OrdinalIgnoreCase) || ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    {
                        // Table not found in this database, try next one
                        _logger.LogWarning("Table '{TableName}' not found in {DbContextName}. Will try next database.", tableName, ctxName);
                        lastException = ex;
                        lastTriedDb = dbKey;
                        continue; // Try next database
                    }
                    catch (InvalidOperationException ex) when (ex.Message.Contains("connection", StringComparison.OrdinalIgnoreCase) || ex.Message.Contains("DbSet", StringComparison.OrdinalIgnoreCase))
                    {
                        // Connection or DbSet issue, try next database
                        _logger.LogWarning("Error accessing {DbContextName}: {Error}. Will try next database.", ctxName, ex.Message);
                        lastException = ex;
                        lastTriedDb = dbKey;
                        continue; // Try next database
                    }
                    catch (Exception ex)
                    {
                        // Other errors - log but continue to next database
                        _logger.LogWarning(ex, "Unexpected error accessing table '{TableName}' in {DbContextName}. Will try next database.", tableName, ctxName);
                        lastException = ex;
                        lastTriedDb = dbKey;
                        continue; // Try next database
                    }
                }

                // If reflection failed for all databases, try SQL fallback for each database
                _logger.LogInformation("Reflection method failed for all databases. Trying SQL fallback method.");
                foreach (var (ctx, ctxType, ctxName, dbKey) in databasesToTry)
                {
                    try
                    {
                        _logger.LogInformation("Attempting SQL fallback for table '{TableName}' in {DbContextName}.", tableName, ctxName);
                        var result = await QueryTableUsingSqlAsync(tableName, valueColumn, textColumn, context, dbKey);
                        _logger.LogInformation("Successfully found table '{TableName}' in {DbContextName} using SQL fallback. Returning {Count} options.", tableName, ctxName, result.Count);
                        return result;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "SQL fallback failed for table '{TableName}' in {DbContextName}. Will try next database.", tableName, ctxName);
                        lastException = ex;
                        lastTriedDb = dbKey;
                        continue; // Try next database
                    }
                }

                // If all attempts failed, throw a comprehensive error
                var triedDatabases = string.Join(", ", databasesToTry.Select(d => d.ctxName));
                var errorMessage = $"Table '{tableName}' not found in any database. Tried databases: {triedDatabases}.";
                if (lastException != null)
                {
                    _logger.LogError(lastException, errorMessage);
                    throw new InvalidOperationException($"{errorMessage} Last error: {lastException.Message}", lastException);
                }
                throw new InvalidOperationException(errorMessage);
            }

            return await QueryWithFallbackAsync();
        }

        /// <summary>
        /// Fallback method to query table using raw SQL if reflection fails
        /// </summary>
        private async Task<List<FieldOptionResponseDto>> QueryTableUsingSqlAsync(
            string tableName, 
            string valueColumn, 
            string textColumn, 
            Dictionary<string, object>? context,
            string database = "FormBuilder")
        {
            // Validate table and column names to prevent SQL injection
            if (!IsValidIdentifier(tableName) || !IsValidIdentifier(valueColumn) || !IsValidIdentifier(textColumn))
            {
                throw new ArgumentException("Invalid table or column name. Only alphanumeric characters and underscores are allowed.");
            }

            // Determine which database context to use
            bool useFormBuilderDb = database.Equals("FormBuilder", StringComparison.OrdinalIgnoreCase);
            DbContext dbContext = useFormBuilderDb ? _formBuilderDbContext : _akhmanageItContext;
            string dbContextName = useFormBuilderDb ? "FormBuilderDbContext" : "AkhmanageItContext";

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
                _logger.LogWarning(ex, "Could not verify column existence for table '{TableName}'", tableName);
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

            _logger.LogInformation("Executing SQL query on {DbContextName}. Table: {TableName}, ValueColumn: {ValueColumn}, TextColumn: {TextColumn}, SQL: {Sql}", 
                dbContextName, tableName, valueColumn, textColumn, sql);

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

            _logger.LogInformation("QueryTableUsingSqlAsync - Returning {Count} options from table '{TableName}' (ValueColumn: {ValueColumn}, TextColumn: {TextColumn})", 
                options.Count, tableName, valueColumn, textColumn);

            return options;
        }

        /// <summary>
        /// Executes a custom SQL query to fetch field options
        /// SQL query should return at least two columns: one for value and one for text
        /// </summary>
        private async Task<List<FieldOptionResponseDto>> GetSqlQueryOptionsAsync(FIELD_DATA_SOURCES dataSource, Dictionary<string, object>? context)
        {
            string sqlQuery = string.Empty;
            string valueColumn = string.Empty;
            string textColumn = string.Empty;

            // 1. Try to parse configuration from ConfigurationJson first
            if (!string.IsNullOrEmpty(dataSource.ConfigurationJson))
            {
                try
                {
                    var config = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(dataSource.ConfigurationJson);
                    if (config != null)
                    {
                        // Try different possible keys for SQL query (supporting old JSON formats too)
                        if (config.ContainsKey("sqlQuery"))
                            sqlQuery = config["sqlQuery"]?.ToString() ?? string.Empty;
                        else if (config.ContainsKey("sqlquery"))
                            sqlQuery = config["sqlquery"]?.ToString() ?? string.Empty;
                        else if (config.ContainsKey("SQLQuery"))
                            sqlQuery = config["SQLQuery"]?.ToString() ?? string.Empty;
                        else if (config.ContainsKey("query")) // legacy key
                            sqlQuery = config["query"]?.ToString() ?? string.Empty;
                        
                        // Try different possible keys for value column
                        if (config.ContainsKey("valueColumn"))
                            valueColumn = config["valueColumn"]?.ToString() ?? string.Empty;
                        else if (config.ContainsKey("valuecolumn"))
                            valueColumn = config["valuecolumn"]?.ToString() ?? string.Empty;
                        else if (config.ContainsKey("ValueColumn"))
                            valueColumn = config["ValueColumn"]?.ToString() ?? string.Empty;
                        
                        // Try different possible keys for text column
                        if (config.ContainsKey("textColumn"))
                            textColumn = config["textColumn"]?.ToString() ?? string.Empty;
                        else if (config.ContainsKey("textcolumn"))
                            textColumn = config["textcolumn"]?.ToString() ?? string.Empty;
                        else if (config.ContainsKey("TextColumn"))
                            textColumn = config["TextColumn"]?.ToString() ?? string.Empty;
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning(ex, "Invalid JSON in ConfigurationJson for SQL query data source {DataSourceId}. Will try fallback options.", 
                        dataSource.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error parsing ConfigurationJson for SQL query data source {DataSourceId}. Will try fallback options.", 
                        dataSource.Id);
                }
            }

            // 2. Fallback to individual fields if ConfigurationJson didn't provide the query
            // Also check if RequestBodyJson is a JSON object containing query and database info
            string? databaseFromRequestBody = null;
            if (string.IsNullOrWhiteSpace(sqlQuery) && !string.IsNullOrEmpty(dataSource.RequestBodyJson))
            {
                // Try to parse RequestBodyJson as JSON first
                try
                {
                    var requestBodyConfig = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(dataSource.RequestBodyJson);
                    if (requestBodyConfig != null)
                    {
                        // Check for SQL query in RequestBodyJson (supports old {\"query\": \"...\"} format)
                        if (requestBodyConfig.ContainsKey("sqlQuery"))
                            sqlQuery = requestBodyConfig["sqlQuery"]?.ToString() ?? string.Empty;
                        else if (requestBodyConfig.ContainsKey("sqlquery"))
                            sqlQuery = requestBodyConfig["sqlquery"]?.ToString() ?? string.Empty;
                        else if (requestBodyConfig.ContainsKey("SQLQuery"))
                            sqlQuery = requestBodyConfig["SQLQuery"]?.ToString() ?? string.Empty;
                        else if (requestBodyConfig.ContainsKey("query")) // legacy key
                            sqlQuery = requestBodyConfig["query"]?.ToString() ?? string.Empty;
                        
                        // Check for database in RequestBodyJson
                        if (requestBodyConfig.ContainsKey("database"))
                            databaseFromRequestBody = requestBodyConfig["database"]?.ToString();
                        
                        // Check for value and text columns in RequestBodyJson
                        if (string.IsNullOrWhiteSpace(valueColumn))
                        {
                            if (requestBodyConfig.ContainsKey("valueColumn"))
                                valueColumn = requestBodyConfig["valueColumn"]?.ToString() ?? string.Empty;
                            else if (requestBodyConfig.ContainsKey("valuecolumn"))
                                valueColumn = requestBodyConfig["valuecolumn"]?.ToString() ?? string.Empty;
                            else if (requestBodyConfig.ContainsKey("ValueColumn"))
                                valueColumn = requestBodyConfig["ValueColumn"]?.ToString() ?? string.Empty;
                        }
                        
                        if (string.IsNullOrWhiteSpace(textColumn))
                        {
                            if (requestBodyConfig.ContainsKey("textColumn"))
                                textColumn = requestBodyConfig["textColumn"]?.ToString() ?? string.Empty;
                            else if (requestBodyConfig.ContainsKey("textcolumn"))
                                textColumn = requestBodyConfig["textcolumn"]?.ToString() ?? string.Empty;
                            else if (requestBodyConfig.ContainsKey("TextColumn"))
                                textColumn = requestBodyConfig["TextColumn"]?.ToString() ?? string.Empty;
                        }
                    }
                }
                catch
                {
                    // RequestBodyJson is not JSON, treat it as SQL query string
                    sqlQuery = dataSource.RequestBodyJson;
                }
            }
            
            // Final fallback: treat RequestBodyJson as SQL query string if still empty
            if (string.IsNullOrWhiteSpace(sqlQuery))
            {
                sqlQuery = dataSource.RequestBodyJson ?? string.Empty;
            }
            
            if (string.IsNullOrWhiteSpace(valueColumn))
            {
                valueColumn = dataSource.ValuePath ?? string.Empty;
            }
            
            if (string.IsNullOrWhiteSpace(textColumn))
            {
                textColumn = dataSource.TextPath ?? string.Empty;
            }

            // 3. Validate required fields
            if (string.IsNullOrWhiteSpace(sqlQuery))
            {
                _logger.LogError("SQL query is missing for SqlQuery data source {DataSourceId}. ConfigurationJson: {ConfigJson}, RequestBodyJson: {RequestBodyJson}", 
                    dataSource.Id, dataSource.ConfigurationJson ?? "null", dataSource.RequestBodyJson ?? "null");
                throw new ArgumentException("SQL query is required for SqlQuery source type. Please provide it in RequestBodyJson or ConfigurationJson.");
            }

            if (string.IsNullOrWhiteSpace(valueColumn))
            {
                throw new ArgumentException("Value column name is required for SqlQuery source type. Please provide it in ValuePath or ConfigurationJson.");
            }

            if (string.IsNullOrWhiteSpace(textColumn))
            {
                throw new ArgumentException("Text column name is required for SqlQuery source type. Please provide it in TextPath or ConfigurationJson.");
            }

            // 4. Determine which database context to use
            // Priority: 1. ConfigurationJson, 2. RequestBodyJson (if JSON), 3. Auto-detect
            string? specifiedDatabase = null;
            
            // Check ConfigurationJson first
            if (!string.IsNullOrEmpty(dataSource.ConfigurationJson))
            {
                try
                {
                    var config = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(dataSource.ConfigurationJson);
                    if (config != null && config.ContainsKey("database"))
                    {
                        specifiedDatabase = config["database"]?.ToString();
                    }
                }
                catch { }
            }
            
            // If not found in ConfigurationJson, use database from RequestBodyJson
            if (string.IsNullOrWhiteSpace(specifiedDatabase) && !string.IsNullOrWhiteSpace(databaseFromRequestBody))
            {
                specifiedDatabase = databaseFromRequestBody;
            }

            // 4. Try to execute SQL query on each database until one succeeds
            var databasesToTry = new List<(DbContext ctx, string ctxName, string dbKey)>();

            if (!string.IsNullOrWhiteSpace(specifiedDatabase) && specifiedDatabase.Equals("AkhmanageIt", StringComparison.OrdinalIgnoreCase))
            {
                // Try AkhmanageIt first if specified
                databasesToTry.Add((_akhmanageItContext, "AkhmanageItContext", "AkhmanageIt"));
                databasesToTry.Add((_formBuilderDbContext, "FormBuilderDbContext", "FormBuilder"));
            }
            else if (!string.IsNullOrWhiteSpace(specifiedDatabase) && specifiedDatabase.Equals("FormBuilder", StringComparison.OrdinalIgnoreCase))
            {
                // Try FormBuilder first if specified
                databasesToTry.Add((_formBuilderDbContext, "FormBuilderDbContext", "FormBuilder"));
                databasesToTry.Add((_akhmanageItContext, "AkhmanageItContext", "AkhmanageIt"));
            }
            else
            {
                // Auto-detect: Try to guess based on query content, but try both if needed
                // FormBuilder database patterns
                bool likelyFormBuilder = sqlQuery.Contains("FORM_", StringComparison.OrdinalIgnoreCase) ||
                                       sqlQuery.Contains("FIELD_", StringComparison.OrdinalIgnoreCase) ||
                                       sqlQuery.Contains("DOCUMENT_", StringComparison.OrdinalIgnoreCase) ||
                                       sqlQuery.Contains("FORM_BUILDER", StringComparison.OrdinalIgnoreCase) ||
                                       sqlQuery.Contains("FORM_FIELDS", StringComparison.OrdinalIgnoreCase) ||
                                       sqlQuery.Contains("FORM_SUBMISSION", StringComparison.OrdinalIgnoreCase) ||
                                       sqlQuery.Contains("APPROVAL_", StringComparison.OrdinalIgnoreCase);

                // AKHManageIT database patterns (common table naming conventions)
                bool likelyAkhmanageIt = sqlQuery.Contains("Tbl", StringComparison.OrdinalIgnoreCase) ||
                                        sqlQuery.Contains("TBL_", StringComparison.OrdinalIgnoreCase) ||
                                        (sqlQuery.Contains("FROM", StringComparison.OrdinalIgnoreCase) && 
                                         !sqlQuery.Contains("FORM_", StringComparison.OrdinalIgnoreCase) &&
                                         !sqlQuery.Contains("FIELD_", StringComparison.OrdinalIgnoreCase));

                // If both patterns match, prioritize FormBuilder for clarity
                // Otherwise, try the detected database first
                if (likelyFormBuilder && !likelyAkhmanageIt)
                {
                    // Likely FormBuilder only
                    _logger.LogInformation("SQL query appears to target FormBuilder tables. Will try FormBuilder first, then AkhmanageIt if needed.");
                    databasesToTry.Add((_formBuilderDbContext, "FormBuilderDbContext", "FormBuilder"));
                    databasesToTry.Add((_akhmanageItContext, "AkhmanageItContext", "AkhmanageIt"));
                }
                else if (likelyAkhmanageIt && !likelyFormBuilder)
                {
                    // Likely AKHManageIT only
                    _logger.LogInformation("SQL query appears to target AKHManageIT tables. Will try AkhmanageIt first, then FormBuilder if needed.");
                    databasesToTry.Add((_akhmanageItContext, "AkhmanageItContext", "AkhmanageIt"));
                    databasesToTry.Add((_formBuilderDbContext, "FormBuilderDbContext", "FormBuilder"));
                }
                else
                {
                    // Ambiguous or no clear pattern - try FormBuilder first (default), then AKHManageIT
                    _logger.LogInformation("Could not auto-detect database from query. Will try FormBuilder first, then AkhmanageIt.");
                    databasesToTry.Add((_formBuilderDbContext, "FormBuilderDbContext", "FormBuilder"));
                    databasesToTry.Add((_akhmanageItContext, "AkhmanageItContext", "AkhmanageIt"));
                }
            }

            Exception? lastException = null;
            string? lastTriedDb = null;

            // Try executing query on each database
            foreach (var (dbContext, ctxName, dbKey) in databasesToTry)
            {
                try
                {
                    _logger.LogInformation("Attempting to execute SQL query on {DbContextName}. Query: {SqlQuery}", ctxName, sqlQuery);

                    // Check database connection
                    if (!await dbContext.Database.CanConnectAsync())
                    {
                        _logger.LogWarning("Database connection failed for SQL query on {DbContextName}. Will try next database.", ctxName);
                        lastException = new InvalidOperationException($"Database connection failed on {ctxName}");
                        lastTriedDb = dbKey;
                        continue;
                    }

                    // Execute SQL query using ADO.NET
                    var options = new List<FieldOptionResponseDto>();
                    var connection = dbContext.Database.GetDbConnection();

                    try
                    {
                        await connection.OpenAsync();

                        using var command = connection.CreateCommand();
                        command.CommandText = sqlQuery;
                        command.CommandTimeout = 60; // 60 seconds timeout

                        // Add context parameters if provided (for parameterized queries)
                        if (context != null)
                        {
                            foreach (var kvp in context)
                            {
                                var parameter = command.CreateParameter();
                                parameter.ParameterName = $"@{kvp.Key}";
                                parameter.Value = kvp.Value ?? DBNull.Value;
                                command.Parameters.Add(parameter);
                            }
                        }

                        using var reader = await command.ExecuteReaderAsync();
                        
                        // Get column names from reader (case-insensitive lookup)
                        var columnNames = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            columnNames[reader.GetName(i)] = i;
                        }

                        // Find column indices (case-insensitive)
                        if (!columnNames.ContainsKey(valueColumn))
                        {
                            throw new InvalidOperationException($"Column '{valueColumn}' not found in query result. Available columns: {string.Join(", ", columnNames.Keys)}");
                        }

                        if (!columnNames.ContainsKey(textColumn))
                        {
                            throw new InvalidOperationException($"Column '{textColumn}' not found in query result. Available columns: {string.Join(", ", columnNames.Keys)}");
                        }

                        int valueColumnIndex = columnNames[valueColumn];
                        int textColumnIndex = columnNames[textColumn];

                        while (await reader.ReadAsync())
                        {
                            // Get value and text from the specified columns using index (more reliable)
                            var value = reader.IsDBNull(valueColumnIndex) ? "" : reader.GetValue(valueColumnIndex)?.ToString() ?? "";
                            var text = reader.IsDBNull(textColumnIndex) ? "" : reader.GetValue(textColumnIndex)?.ToString() ?? "";

                            options.Add(new FieldOptionResponseDto
                            {
                                Value = value,
                                Text = text
                            });
                        }

                        _logger.LogInformation("Successfully executed SQL query on {DbContextName}. Returning {Count} options.", ctxName, options.Count);
                        return options;
                    }
                    catch (Exception ex) when (ex.Message.Contains("Invalid object name", StringComparison.OrdinalIgnoreCase) ||
                                               ex.Message.Contains("does not exist", StringComparison.OrdinalIgnoreCase) ||
                                               ex.Message.Contains("Invalid column name", StringComparison.OrdinalIgnoreCase))
                    {
                        // Table or column doesn't exist in this database, try next one
                        _logger.LogWarning("SQL query failed on {DbContextName}: {Error}. Will try next database.", ctxName, ex.Message);
                        lastException = ex;
                        lastTriedDb = dbKey;
                        continue;
                    }
                    finally
                    {
                        if (connection.State == System.Data.ConnectionState.Open)
                        {
                            await connection.CloseAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error executing SQL query on {DbContextName}. Will try next database.", ctxName);
                    lastException = ex;
                    lastTriedDb = dbKey;
                    continue;
                }
            }

            // If all attempts failed, throw a comprehensive error
            var triedDatabases = string.Join(", ", databasesToTry.Select(d => d.ctxName));
            var errorMessage = $"SQL query failed on all databases. Tried databases: {triedDatabases}.";
            if (lastException != null)
            {
                _logger.LogError(lastException, errorMessage);
                throw new InvalidOperationException($"{errorMessage} Last error: {lastException.Message}", lastException);
            }
            throw new InvalidOperationException(errorMessage);
        }

        /// <summary>
        /// Executes a SQL query on SAP HANA database to fetch field options
        /// </summary>
        private async Task<List<FieldOptionResponseDto>> GetSapHanaOptionsAsync(FIELD_DATA_SOURCES dataSource, Dictionary<string, object>? context)
        {
            if (_sapHanaService == null)
            {
                throw new InvalidOperationException("SAP HANA service is not available");
            }

            string sqlQuery = string.Empty;
            string valueColumn = string.Empty;
            string textColumn = string.Empty;
            SapHanaConnectionDto? connection = null;

            // 1. Parse configuration from ConfigurationJson
            if (!string.IsNullOrEmpty(dataSource.ConfigurationJson))
            {
                try
                {
                    var config = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(dataSource.ConfigurationJson);
                    if (config != null)
                    {
                        // Get SQL query
                        if (config.ContainsKey("sqlQuery"))
                            sqlQuery = config["sqlQuery"]?.ToString() ?? string.Empty;
                        else if (config.ContainsKey("sqlquery"))
                            sqlQuery = config["sqlquery"]?.ToString() ?? string.Empty;

                        // Get column mappings
                        if (config.ContainsKey("valueColumn"))
                            valueColumn = config["valueColumn"]?.ToString() ?? string.Empty;
                        if (config.ContainsKey("textColumn"))
                            textColumn = config["textColumn"]?.ToString() ?? string.Empty;

                        // Get connection details
                        if (config.ContainsKey("connection"))
                        {
                            var connectionJson = System.Text.Json.JsonSerializer.Serialize(config["connection"]);
                            connection = System.Text.Json.JsonSerializer.Deserialize<SapHanaConnectionDto>(connectionJson);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to parse ConfigurationJson for SAP HANA data source {DataSourceId}", dataSource.Id);
                }
            }

            // 2. Fallback to individual fields if ConfigurationJson is empty or incomplete
            if (string.IsNullOrWhiteSpace(sqlQuery))
            {
                sqlQuery = dataSource.RequestBodyJson ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(valueColumn))
            {
                valueColumn = dataSource.ValuePath ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(textColumn))
            {
                textColumn = dataSource.TextPath ?? string.Empty;
            }

            // 3. Parse connection from ApiUrl if not found in ConfigurationJson
            if (connection == null && !string.IsNullOrEmpty(dataSource.ApiUrl))
            {
                try
                {
                    connection = System.Text.Json.JsonSerializer.Deserialize<SapHanaConnectionDto>(dataSource.ApiUrl);
                }
                catch
                {
                    // ApiUrl is not JSON, ignore
                }
            }

            // 4. Validate required fields
            if (connection == null)
            {
                // Prefer global DB-stored SAP HANA connection (encrypted at rest)
                var activeCs = await _sapHanaConfigsService.GetActiveConnectionStringAsync();
                if (string.IsNullOrWhiteSpace(activeCs))
                {
                    throw new ArgumentException("No active SAP HANA connection configured. Please configure it first in SAP_HANA_CONFIGS.");
                }

                connection = ParseHanaConnectionString(activeCs);
                if (connection == null)
                {
                    throw new ArgumentException("Failed to parse active SAP HANA connection string. Expected format: Server=...;UserName=...;Password=...;Current Schema=...");
                }
            }

            if (string.IsNullOrWhiteSpace(sqlQuery))
            {
                throw new ArgumentException("SQL query is required for SAP HANA source type. Please provide it in SqlQuery, RequestBodyJson, or ConfigurationJson.");
            }

            if (string.IsNullOrWhiteSpace(valueColumn))
            {
                throw new ArgumentException("Value column name is required for SAP HANA source type. Please provide it in ValuePath or ConfigurationJson.");
            }

            if (string.IsNullOrWhiteSpace(textColumn))
            {
                throw new ArgumentException("Text column name is required for SAP HANA source type. Please provide it in TextPath or ConfigurationJson.");
            }

            // 5. Execute query on SAP HANA
            var queryRequest = new SapHanaQueryRequestDto
            {
                Connection = connection,
                Query = sqlQuery,
                MaxRows = 1000 // Limit results for field options
            };

            var queryResult = await _sapHanaService.ExecuteQueryAsync(queryRequest);

            if (queryResult.StatusCode != 200 || queryResult.Data == null)
            {
                var errorMsg = queryResult.Message ?? "Failed to execute SAP HANA query";
                _logger.LogError("SAP HANA query execution failed: {Error}", errorMsg);
                throw new InvalidOperationException($"SAP HANA query failed: {errorMsg}");
            }

            // 6. Parse response and extract value/text columns
            var responseData = System.Text.Json.JsonSerializer.Serialize(queryResult.Data);
            var queryResponse = System.Text.Json.JsonSerializer.Deserialize<SapHanaQueryResponseDto>(responseData);

            if (queryResponse == null || !queryResponse.Success || queryResponse.Data == null)
            {
                throw new InvalidOperationException($"SAP HANA query failed: {queryResponse?.ErrorMessage ?? "Unknown error"}");
            }

            // If value/text columns are not specified, auto-detect from result columns
            if (string.IsNullOrWhiteSpace(valueColumn) || string.IsNullOrWhiteSpace(textColumn))
            {
                var columns = queryResponse.Columns ?? new List<SapHanaColumnInfo>();

                if (string.IsNullOrWhiteSpace(valueColumn) && columns.Count >= 1)
                {
                    valueColumn = columns[0].Name ?? string.Empty;
                    _logger.LogInformation("SAP HANA datasource: Auto-detected value column: {ValueColumn}", valueColumn);
                }

                if (string.IsNullOrWhiteSpace(textColumn) && columns.Count >= 2)
                {
                    textColumn = columns[1].Name ?? string.Empty;
                    _logger.LogInformation("SAP HANA datasource: Auto-detected text column: {TextColumn}", textColumn);
                }
            }

            // 7. Map results to FieldOptionResponseDto
            var options = new List<FieldOptionResponseDto>();

            foreach (var row in queryResponse.Data)
            {
                string GetValueCaseInsensitive(string columnName)
                {
                    if (string.IsNullOrWhiteSpace(columnName))
                        return string.Empty;

                    // Try exact key first
                    if (row.TryGetValue(columnName, out var direct))
                    {
                        return direct?.ToString() ?? string.Empty;
                    }

                    // Fallback: case-insensitive lookup
                    var match = row.FirstOrDefault(kvp =>
                        string.Equals(kvp.Key, columnName, StringComparison.OrdinalIgnoreCase));

                    return match.Equals(default(KeyValuePair<string, object?>))
                        ? string.Empty
                        : match.Value?.ToString() ?? string.Empty;
                }

                var value = GetValueCaseInsensitive(valueColumn);
                var text = GetValueCaseInsensitive(textColumn);

                options.Add(new FieldOptionResponseDto
                {
                    Value = value,
                    Text = text
                });
            }

            _logger.LogInformation("SAP HANA query executed successfully. Returning {Count} options.", options.Count);
            return options;
        }

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
            
            _logger.LogInformation("QueryTableAsync - Found {Count} items from table '{TableName}'", items.Count, entityType.Name);

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

            // Try to find text property with case-insensitive search and fallback options
            var textProperty = entityType.GetProperty(textColumn, System.Reflection.BindingFlags.IgnoreCase | 
                                                                   System.Reflection.BindingFlags.Public | 
                                                                   System.Reflection.BindingFlags.Instance);
            
            // If not found, try common fallback names (case-insensitive)
            if (textProperty == null)
            {
                var allProperties = entityType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                
                // Try exact match first (case-insensitive)
                textProperty = allProperties.FirstOrDefault(p => 
                    string.Equals(p.Name, textColumn, StringComparison.OrdinalIgnoreCase));
                
                // Try common fallback names
                if (textProperty == null)
                {
                    textProperty = allProperties.FirstOrDefault(p => 
                        string.Equals(p.Name, "Name", StringComparison.OrdinalIgnoreCase)) ??
                        allProperties.FirstOrDefault(p => 
                            string.Equals(p.Name, "Title", StringComparison.OrdinalIgnoreCase)) ??
                        allProperties.FirstOrDefault(p => 
                            string.Equals(p.Name, "Code", StringComparison.OrdinalIgnoreCase)) ??
                        allProperties.FirstOrDefault(p => 
                            p.Name.EndsWith("Name", StringComparison.OrdinalIgnoreCase) &&
                            !p.Name.Equals("ForeignName", StringComparison.OrdinalIgnoreCase)) ??
                        allProperties.FirstOrDefault(p => 
                            string.Equals(p.Name, "Description", StringComparison.OrdinalIgnoreCase)) ??
                        allProperties.FirstOrDefault(p => 
                            string.Equals(p.Name, "Text", StringComparison.OrdinalIgnoreCase));
                }
                
                if (textProperty != null)
                {
                    _logger.LogWarning("Text column '{TextColumn}' not found, using fallback '{FallbackColumn}' for table '{TableName}'", 
                        textColumn, textProperty.Name, entityType.Name);
                }
            }

            if (valueProperty == null)
            {
                var availableColumns = string.Join(", ", entityType.GetProperties().Select(p => p.Name));
                _logger.LogError("Value column '{ValueColumn}' not found in table '{TableName}'. Available columns: {AvailableColumns}", 
                    valueColumn, entityType.Name, availableColumns);
                throw new ArgumentException($"Value column '{valueColumn}' not found in table '{entityType.Name}'. Available columns: {availableColumns}");
            }

            if (textProperty == null)
            {
                var availableColumns = string.Join(", ", entityType.GetProperties().Select(p => p.Name));
                _logger.LogError("Text column '{TextColumn}' not found in table '{TableName}'. Available columns: {AvailableColumns}", 
                    textColumn, entityType.Name, availableColumns);
                throw new ArgumentException($"Text column '{textColumn}' not found in table '{entityType.Name}'. Available columns: {availableColumns}");
            }

            var result = items
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
            
            _logger.LogInformation("QueryTableAsync - Returning {Count} options from table '{TableName}' (ValueColumn: {ValueColumn}, TextColumn: {TextColumn})", 
                result.Count, entityType.Name, valueProperty.Name, textProperty.Name);
            
            return result;
        }

        private async Task<List<FieldOptionResponseDto>> GetApiOptionsAsync(
            FIELD_DATA_SOURCES dataSource, 
            string? requestBodyJson, 
            Dictionary<string, object>? context,
            List<string>? customArrayPropertyNames = null,
            int? sapConfigId = null)
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
                        requestBody = !string.IsNullOrEmpty(requestBodyJson) ? requestBodyJson : dataSource.RequestBodyJson;
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
            ConfigureExternalApiClientHeaders(httpClient);

            var sapSessionCookie = await TryResolveSapSessionCookieAsync(fullApiUrl, requestBody, sapConfigId);
            if (!string.IsNullOrWhiteSpace(sapSessionCookie))
            {
                httpClient.DefaultRequestHeaders.Remove("Cookie");
                httpClient.DefaultRequestHeaders.Add("Cookie", sapSessionCookie);
            }

            HttpResponseMessage response;
            string jsonContent;
            try
            {
                var requestMethod = httpMethod.ToUpper() == "POST" ? HttpMethod.Post : HttpMethod.Get;
                var body = requestMethod == HttpMethod.Post && !string.IsNullOrEmpty(requestBody)
                    ? JsonSerializer.Deserialize<JsonElement>(requestBody)
                    : (JsonElement?)null;

                response = await SendApiRequestWithSslFallbackAsync(
                    httpClient,
                    requestMethod,
                    fullApiUrl,
                    body,
                    "preview data source");

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

                // Special handling for OData metadata XML (e.g., SAP Service Layer $metadata)
                if (IsMetadataUrl(fullApiUrl) || IsXmlResponse(response, jsonContent))
                {
                    var metadataOptions = TryExtractMetadataFields(jsonContent);
                    if (metadataOptions.Count > 0)
                    {
                        _logger.LogInformation("Extracted {Count} fields from metadata endpoint: {Url}", metadataOptions.Count, fullApiUrl);
                        return metadataOptions;
                    }
                }
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

        private static bool IsMetadataUrl(string? url)
        {
            return !string.IsNullOrWhiteSpace(url) &&
                   url.Contains("$metadata", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsXmlResponse(HttpResponseMessage response, string content)
        {
            var mediaType = response.Content?.Headers?.ContentType?.MediaType ?? string.Empty;
            if (mediaType.Contains("xml", StringComparison.OrdinalIgnoreCase))
                return true;

            var trimmed = (content ?? string.Empty).TrimStart();
            return trimmed.StartsWith("<?xml", StringComparison.OrdinalIgnoreCase) ||
                   trimmed.StartsWith("<edmx:Edmx", StringComparison.OrdinalIgnoreCase) ||
                   trimmed.StartsWith("<Edmx", StringComparison.OrdinalIgnoreCase);
        }

        private List<FieldOptionResponseDto> TryExtractMetadataFields(string xmlContent)
        {
            try
            {
                var doc = XDocument.Parse(xmlContent);
                XNamespace edmNsV4 = "http://docs.oasis-open.org/odata/ns/edm";
                XNamespace edmNsV3 = "http://schemas.microsoft.com/ado/2009/11/edm";

                var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var p in doc.Descendants(edmNsV4 + "Property"))
                {
                    var name = p.Attribute("Name")?.Value?.Trim();
                    if (!string.IsNullOrWhiteSpace(name))
                        names.Add(name);
                }

                foreach (var p in doc.Descendants(edmNsV3 + "Property"))
                {
                    var name = p.Attribute("Name")?.Value?.Trim();
                    if (!string.IsNullOrWhiteSpace(name))
                        names.Add(name);
                }

                return names
                    .OrderBy(x => x)
                    .Select(x => new FieldOptionResponseDto
                    {
                        Value = x,
                        Text = x
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to parse metadata XML response.");
                return new List<FieldOptionResponseDto>();
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

        // ================================
        // INSPECT API STRUCTURE (Get Available Fields)
        // ================================
        public async Task<ApiResponse> InspectApiAsync(InspectApiRequestDto request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ApiUrl))
                {
                    return new ApiResponse(400, "ApiUrl is required");
                }

                var response = new ApiInspectionResponseDto
                {
                    FullUrl = CombineApiUrl(request.ApiUrl, request.ApiPath),
                    Success = false
                };

                try
                {
                    // Use named HttpClient configured with automatic decompression
                    var httpClient = _httpClientFactory.CreateClient("ExternalApi");
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                    ConfigureExternalApiClientHeaders(httpClient);

                    HttpResponseMessage httpResponse;
                    var requestMethod = (request.HttpMethod ?? "GET").ToUpper() == "POST" ? HttpMethod.Post : HttpMethod.Get;
                    var body = requestMethod == HttpMethod.Post && !string.IsNullOrEmpty(request.RequestBodyJson)
                        ? JsonSerializer.Deserialize<JsonElement>(request.RequestBodyJson)
                        : (JsonElement?)null;
                    httpResponse = await SendApiRequestWithSslFallbackAsync(
                        httpClient,
                        requestMethod,
                        response.FullUrl,
                        body,
                        "inspect api");

                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        var errorContent = await httpResponse.Content.ReadAsStringAsync();
                        response.ErrorMessage = $"API returned status {httpResponse.StatusCode}: {errorContent.Substring(0, Math.Min(200, errorContent.Length))}";
                        return new ApiResponse(200, "API inspection completed", response);
                    }

                    var jsonContent = await httpResponse.Content.ReadAsStringAsync();
                    response.RawResponse = jsonContent.Length > 1000 ? jsonContent.Substring(0, 1000) + "..." : jsonContent;

                    var jsonDoc = JsonDocument.Parse(jsonContent);
                    var root = jsonDoc.RootElement;

                    // Find array in response
                    JsonElement? arrayElement = null;
                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        arrayElement = root;
                    }
                    else if (root.ValueKind == JsonValueKind.Object)
                    {
                        var arrayPropertyNames = request.ArrayPropertyNames != null && request.ArrayPropertyNames.Any()
                            ? request.ArrayPropertyNames.ToList()
                            : new List<string> { 
                                "data", "results", "items", "list", "records", "values", "content", "collection",
                                "users", "products", "entries", "objects", "entities", "rows", "elements"
                            };
                        
                        foreach (var propName in arrayPropertyNames)
                        {
                            if (root.TryGetProperty(propName, out var prop) && prop.ValueKind == JsonValueKind.Array && prop.GetArrayLength() > 0)
                            {
                                arrayElement = prop;
                                break;
                            }
                            
                            foreach (var jsonProp in root.EnumerateObject())
                            {
                                if (string.Equals(jsonProp.Name, propName, StringComparison.OrdinalIgnoreCase) &&
                                    jsonProp.Value.ValueKind == JsonValueKind.Array && jsonProp.Value.GetArrayLength() > 0)
                                {
                                    arrayElement = jsonProp.Value;
                                    break;
                                }
                            }
                            
                            if (arrayElement.HasValue)
                                break;
                        }
                        
                        if (!arrayElement.HasValue)
                        {
                            arrayElement = FindFirstArray(root);
                        }
                    }

                    if (arrayElement.HasValue && arrayElement.Value.GetArrayLength() > 0)
                    {
                        var firstItem = arrayElement.Value[0];
                        response.ItemsCount = arrayElement.Value.GetArrayLength();
                        response.SampleItem = JsonSerializer.Deserialize<object>(firstItem.GetRawText());
                        
                        // Extract all available fields from first item
                        ExtractFields(firstItem, "", response.AvailableFields, response.NestedFields);
                        
                        // Suggest value paths (prefer id, key, code, value)
                        var valueSuggestions = new[] { "id", "key", "code", "value", "uuid", "identifier" };
                        foreach (var suggestion in valueSuggestions)
                        {
                            if (response.AvailableFields.Any(f => f.Equals(suggestion, StringComparison.OrdinalIgnoreCase)))
                            {
                                response.SuggestedValuePaths.Add(suggestion);
                            }
                        }
                        if (!response.SuggestedValuePaths.Any())
                        {
                            response.SuggestedValuePaths.AddRange(response.AvailableFields.Take(3));
                        }
                        
                        // Suggest text paths (prefer name, title, text, label)
                        var textSuggestions = new[] { "name", "title", "text", "label", "description", "firstname", "first_name" };
                        foreach (var suggestion in textSuggestions)
                        {
                            if (response.AvailableFields.Any(f => f.Equals(suggestion, StringComparison.OrdinalIgnoreCase)) ||
                                response.NestedFields.Any(f => f.Contains(suggestion, StringComparison.OrdinalIgnoreCase)))
                            {
                                var matchingField = response.AvailableFields.FirstOrDefault(f => f.Equals(suggestion, StringComparison.OrdinalIgnoreCase)) ??
                                                   response.NestedFields.FirstOrDefault(f => f.Contains(suggestion, StringComparison.OrdinalIgnoreCase));
                                if (matchingField != null)
                                {
                                    response.SuggestedTextPaths.Add(matchingField);
                                }
                            }
                        }
                        if (!response.SuggestedTextPaths.Any())
                        {
                            response.SuggestedTextPaths.AddRange(response.AvailableFields.Skip(1).Take(3));
                        }
                        
                        response.Success = true;
                    }
                    else
                    {
                        // No array found, show root structure
                        if (root.ValueKind == JsonValueKind.Object)
                        {
                            ExtractFields(root, "", response.AvailableFields, response.NestedFields);
                            response.SampleItem = JsonSerializer.Deserialize<object>(root.GetRawText());
                        }
                        response.ErrorMessage = "No array found in API response. Showing root structure instead.";
                        response.Success = false;
                    }
                }
                catch (Exception ex)
                {
                    response.ErrorMessage = SanitizeForJson(ex.Message);
                    response.Success = false;
                }

                return new ApiResponse(200, "API inspection completed", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inspecting API structure");
                return new ApiResponse(500, $"Error inspecting API: {SanitizeForJson(ex.Message)}");
            }
        }

        private async Task<string?> TryResolveSapSessionCookieAsync(string requestUrl, string? requestBodyJson, int? sapConfigId)
        {
            if (!IsSapServiceLayerUrl(requestUrl))
            {
                return null;
            }

            // Prefer credentials in request body when provided.
            if (TryExtractSapCredentialsFromJson(requestBodyJson, out var bodyCompanyDb, out var bodyUserName, out var bodyPassword))
            {
                var bodyBaseUrl = ResolveSapServiceLayerBaseUrl(requestUrl);
                if (!string.IsNullOrWhiteSpace(bodyBaseUrl))
                {
                    return await LoginToSapServiceLayerAndGetCookieAsync(bodyBaseUrl, bodyCompanyDb!, bodyUserName!, bodyPassword!);
                }
            }

            // Otherwise use server-side configured SAP connection credentials.
            if (!sapConfigId.HasValue || sapConfigId.Value <= 0)
            {
                return null;
            }

            var rawConnectionString = await _sapHanaConfigsService.GetConnectionStringByIdAsync(sapConfigId.Value);
            if (string.IsNullOrWhiteSpace(rawConnectionString))
            {
                return null;
            }

            var values = ParseKeyValueConnectionString(rawConnectionString);
            if (!values.TryGetValue("Type", out var type) ||
                !string.Equals(type, "ServiceLayer", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var baseUrl = values.TryGetValue("BaseUrl", out var configuredBaseUrl)
                ? configuredBaseUrl
                : ResolveSapServiceLayerBaseUrl(requestUrl);
            var companyDb = values.TryGetValue("CompanyDB", out var configuredCompanyDb) ? configuredCompanyDb : null;
            var userName = values.TryGetValue("UserName", out var configuredUserName) ? configuredUserName : null;
            var password = values.TryGetValue("Password", out var configuredPassword) ? configuredPassword : null;

            if (string.IsNullOrWhiteSpace(baseUrl) ||
                string.IsNullOrWhiteSpace(companyDb) ||
                string.IsNullOrWhiteSpace(userName) ||
                string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            return await LoginToSapServiceLayerAndGetCookieAsync(baseUrl, companyDb, userName, password);
        }

        private async Task<string?> LoginToSapServiceLayerAndGetCookieAsync(string baseUrl, string companyDb, string userName, string password)
        {
            var loginUrl = $"{baseUrl.TrimEnd('/')}/Login";
            var payload = JsonSerializer.SerializeToElement(new
            {
                CompanyDB = companyDb,
                UserName = userName,
                Password = password
            });

            using var httpClient = _httpClientFactory.CreateClient("ExternalApi");
            httpClient.Timeout = TimeSpan.FromSeconds(30);
            ConfigureExternalApiClientHeaders(httpClient);

            var response = await SendApiRequestWithSslFallbackAsync(
                httpClient,
                HttpMethod.Post,
                loginUrl,
                payload,
                "SAP Service Layer login");

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                if ((int)response.StatusCode == 401)
                {
                    throw new InvalidOperationException($"SAP login unauthorized: HTTP 401. SAP Error: {SanitizeForJson(errorBody, 400)}");
                }

                throw new InvalidOperationException($"SAP login failed: HTTP {(int)response.StatusCode}. SAP Error: {SanitizeForJson(errorBody, 400)}");
            }

            var cookies = new List<string>();
            if (response.Headers.TryGetValues("Set-Cookie", out var setCookies))
            {
                foreach (var c in setCookies)
                {
                    var pair = c.Split(';', 2)[0]?.Trim();
                    if (!string.IsNullOrWhiteSpace(pair))
                    {
                        cookies.Add(pair);
                    }
                }
            }

            if (cookies.Count > 0)
            {
                return string.Join("; ", cookies);
            }

            var body = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(body))
            {
                return null;
            }

            try
            {
                using var doc = JsonDocument.Parse(body);
                if (doc.RootElement.TryGetProperty("SessionId", out var sessionIdEl))
                {
                    var sessionId = sessionIdEl.GetString();
                    if (!string.IsNullOrWhiteSpace(sessionId))
                    {
                        return $"B1SESSION={sessionId}";
                    }
                }
            }
            catch
            {
                // Ignore parse failures and return null (caller will continue without cookie).
            }

            return null;
        }

        private static bool IsSapServiceLayerUrl(string url)
        {
            return !string.IsNullOrWhiteSpace(url) &&
                   url.IndexOf("/b1s/v1", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string ResolveSapServiceLayerBaseUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }

            var idx = url.IndexOf("/b1s/v1", StringComparison.OrdinalIgnoreCase);
            if (idx < 0)
            {
                return url.TrimEnd('/');
            }

            return url.Substring(0, idx + "/b1s/v1".Length).TrimEnd('/');
        }

        private static bool TryExtractSapCredentialsFromJson(string? requestBodyJson, out string? companyDb, out string? userName, out string? password)
        {
            companyDb = null;
            userName = null;
            password = null;

            if (string.IsNullOrWhiteSpace(requestBodyJson))
            {
                return false;
            }

            try
            {
                using var doc = JsonDocument.Parse(requestBodyJson);
                if (doc.RootElement.ValueKind != JsonValueKind.Object)
                {
                    return false;
                }

                companyDb = GetJsonPropertyCaseInsensitive(doc.RootElement, "CompanyDB");
                userName = GetJsonPropertyCaseInsensitive(doc.RootElement, "UserName");
                password = GetJsonPropertyCaseInsensitive(doc.RootElement, "Password");

                return !string.IsNullOrWhiteSpace(companyDb) &&
                       !string.IsNullOrWhiteSpace(userName) &&
                       !string.IsNullOrWhiteSpace(password);
            }
            catch
            {
                return false;
            }
        }

        private static string? GetJsonPropertyCaseInsensitive(JsonElement element, string propertyName)
        {
            foreach (var prop in element.EnumerateObject())
            {
                if (string.Equals(prop.Name, propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    return prop.Value.ValueKind == JsonValueKind.String
                        ? prop.Value.GetString()
                        : prop.Value.GetRawText()?.Trim('"');
                }
            }

            return null;
        }

        private static Dictionary<string, string> ParseKeyValueConnectionString(string connectionString)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var parts = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                var pair = part.Split('=', 2, StringSplitOptions.TrimEntries);
                if (pair.Length == 2 && !string.IsNullOrWhiteSpace(pair[0]))
                {
                    dict[pair[0]] = pair[1];
                }
            }

            return dict;
        }

        private void ConfigureExternalApiClientHeaders(HttpClient httpClient)
        {
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
                httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            }
            if (!httpClient.DefaultRequestHeaders.Contains("Cache-Control"))
            {
                httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            }
        }

        private async Task<HttpResponseMessage> SendApiRequestWithSslFallbackAsync(
            HttpClient primaryClient,
            HttpMethod method,
            string url,
            JsonElement? jsonBody,
            string operationName)
        {
            try
            {
                if (method == HttpMethod.Post)
                {
                    return await primaryClient.PostAsJsonAsync(url, jsonBody);
                }

                return await primaryClient.GetAsync(url);
            }
            catch (Exception ex) when (IsSslCertificateValidationError(ex))
            {
                _logger.LogWarning(ex, "SSL validation failed while trying to {Operation} at {Url}. Retrying with SSL validation disabled.", operationName, url);

                using var insecureClient = CreateInsecureExternalApiClient();
                ConfigureExternalApiClientHeaders(insecureClient);
                CopyDefaultHeaders(primaryClient, insecureClient);

                if (method == HttpMethod.Post)
                {
                    return await insecureClient.PostAsJsonAsync(url, jsonBody);
                }

                return await insecureClient.GetAsync(url);
            }
        }

        private static void CopyDefaultHeaders(HttpClient source, HttpClient target)
        {
            foreach (var header in source.DefaultRequestHeaders)
            {
                target.DefaultRequestHeaders.Remove(header.Key);
                target.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        private static HttpClient CreateInsecureExternalApiClient()
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = System.Net.DecompressionMethods.All,
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            return new HttpClient(handler, disposeHandler: true)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        private static bool IsSslCertificateValidationError(Exception ex)
        {
            for (var current = ex; current != null; current = current.InnerException)
            {
                if (current is AuthenticationException)
                {
                    return true;
                }

                var message = current.Message ?? string.Empty;
                if (message.Contains("SSL connection could not be established", StringComparison.OrdinalIgnoreCase) ||
                    message.Contains("RemoteCertificate", StringComparison.OrdinalIgnoreCase) ||
                    message.Contains("Could not establish trust relationship", StringComparison.OrdinalIgnoreCase) ||
                    message.Contains("trust relationship", StringComparison.OrdinalIgnoreCase) ||
                    message.Contains("certificate chain", StringComparison.OrdinalIgnoreCase) ||
                    message.Contains("certificate", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private void ExtractFields(JsonElement element, string prefix, List<string> availableFields, List<string> nestedFields, int maxDepth = 3, int currentDepth = 0)
        {
            if (currentDepth >= maxDepth || element.ValueKind != JsonValueKind.Object)
                return;

            foreach (var prop in element.EnumerateObject())
            {
                var fieldPath = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}.{prop.Name}";
                
                if (prop.Value.ValueKind == JsonValueKind.Object)
                {
                    nestedFields.Add(fieldPath);
                    ExtractFields(prop.Value, fieldPath, availableFields, nestedFields, maxDepth, currentDepth + 1);
                }
                else if (prop.Value.ValueKind == JsonValueKind.Array && prop.Value.GetArrayLength() > 0)
                {
                    nestedFields.Add(fieldPath);
                    if (prop.Value[0].ValueKind == JsonValueKind.Object)
                    {
                        ExtractFields(prop.Value[0], fieldPath, availableFields, nestedFields, maxDepth, currentDepth + 1);
                    }
                }
                else
                {
                    availableFields.Add(fieldPath);
                }
            }
        }

        // ================================
        // GET AVAILABLE LOOKUP TABLES
        // ================================
        public async Task<ApiResponse> GetAvailableLookupTablesAsync(string? database = null)
        {
            try
            {
                // Determine which database context to use
                // Default: FormBuilderDbContext
                // Options: "FormBuilder" or "AkhmanageIt"
                bool useFormBuilderDb = string.IsNullOrWhiteSpace(database) || 
                                       database.Equals("FormBuilder", StringComparison.OrdinalIgnoreCase);
                
                Type contextType;
                string dbContextName;
                
                if (useFormBuilderDb)
                {
                    contextType = typeof(FormBuilderDbContext);
                    dbContextName = "FormBuilderDbContext";
                }
                else if (database.Equals("AkhmanageIt", StringComparison.OrdinalIgnoreCase))
                {
                    contextType = typeof(AkhmanageItContext);
                    dbContextName = "AkhmanageItContext";
                }
                else
                {
                    return new ApiResponse(400, $"Invalid database name: {database}. Valid options: 'FormBuilder' or 'AkhmanageIt'");
                }

                var suitableTables = new List<object>();

                _logger.LogInformation("Getting available lookup tables from {DbContextName}", dbContextName);

                var dbSetProperties = contextType.GetProperties()
                    .Where(p => p.PropertyType.IsGenericType && 
                               p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                    .ToList();

                _logger.LogInformation("Found {Count} DbSet properties in {DbContextName}", dbSetProperties.Count, dbContextName);

                foreach (var prop in dbSetProperties)
                {
                    var entityType = prop.PropertyType.GetGenericArguments()[0];
                    var properties = entityType.GetProperties();

                    // Check if entity has Id property (or ID, Id, etc.)
                    var hasIdProperty = properties.Any(p => 
                        p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase) ||
                        p.Name.Equals("ID", StringComparison.OrdinalIgnoreCase));

                    // Check if entity has Name property (supporting various naming patterns)
                    // Pattern: *Name, Name, Title, Description, Code, Text, etc.
                    var hasNameProperty = properties.Any(p => 
                        p.Name.Equals("Name", StringComparison.OrdinalIgnoreCase) ||
                        p.Name.EndsWith("Name", StringComparison.OrdinalIgnoreCase) || // FormName, FieldName, TabName, GridName, TemplateName, etc.
                        p.Name.Equals("Title", StringComparison.OrdinalIgnoreCase) ||
                        p.Name.Equals("Description", StringComparison.OrdinalIgnoreCase) ||
                        p.Name.Equals("Code", StringComparison.OrdinalIgnoreCase) ||
                        p.Name.EndsWith("Code", StringComparison.OrdinalIgnoreCase) || // SeriesCode, FormCode, FieldCode, etc.
                        p.Name.Equals("Text", StringComparison.OrdinalIgnoreCase) ||
                        p.Name.Equals("TypeName", StringComparison.OrdinalIgnoreCase));

                    // Only include tables that have both Id and Name-like properties
                    if (hasIdProperty && hasNameProperty)
                    {
                        // Get the actual property names
                        var idProperty = properties.FirstOrDefault(p => 
                            p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase) ||
                            p.Name.Equals("ID", StringComparison.OrdinalIgnoreCase));
                        
                        // Find name property with priority order
                        var nameProperty = properties.FirstOrDefault(p => 
                            p.Name.Equals("Name", StringComparison.OrdinalIgnoreCase)) ??
                            // Try *Name patterns (FormName, FieldName, TabName, GridName, TemplateName, etc.)
                            properties.FirstOrDefault(p => 
                                p.Name.EndsWith("Name", StringComparison.OrdinalIgnoreCase) && 
                                !p.Name.Equals("ForeignName", StringComparison.OrdinalIgnoreCase) &&
                                !p.Name.Equals("ForeignFormName", StringComparison.OrdinalIgnoreCase) &&
                                !p.Name.Equals("ForeignFieldName", StringComparison.OrdinalIgnoreCase) &&
                                !p.Name.Equals("ForeignTabName", StringComparison.OrdinalIgnoreCase)) ??
                            properties.FirstOrDefault(p => 
                                p.Name.Equals("Title", StringComparison.OrdinalIgnoreCase)) ??
                            properties.FirstOrDefault(p => 
                                p.Name.Equals("TypeName", StringComparison.OrdinalIgnoreCase)) ??
                            // Try *Code patterns (SeriesCode, FormCode, FieldCode, etc.)
                            properties.FirstOrDefault(p => 
                                p.Name.EndsWith("Code", StringComparison.OrdinalIgnoreCase)) ??
                            properties.FirstOrDefault(p => 
                                p.Name.Equals("Code", StringComparison.OrdinalIgnoreCase)) ??
                            properties.FirstOrDefault(p => 
                                p.Name.Equals("Description", StringComparison.OrdinalIgnoreCase)) ??
                            properties.FirstOrDefault(p => 
                                p.Name.Equals("Text", StringComparison.OrdinalIgnoreCase));

                        suitableTables.Add(new
                        {
                            Name = prop.Name,
                            EntityType = entityType.Name,
                            IdColumn = idProperty?.Name ?? "Id",
                            NameColumn = nameProperty?.Name ?? "Name",
                            DisplayName = $"{prop.Name} ({entityType.Name})"
                        });

                        _logger.LogDebug("Added table: {TableName} (Entity: {EntityType}, IdColumn: {IdColumn}, NameColumn: {NameColumn})", 
                            prop.Name, entityType.Name, idProperty?.Name ?? "Id", nameProperty?.Name ?? "Name");
                    }
                    else
                    {
                        _logger.LogDebug("Skipped table: {TableName} (Entity: {EntityType}) - Missing Id: {HasId}, Missing Name: {HasName}", 
                            prop.Name, entityType.Name, !hasIdProperty, !hasNameProperty);
                    }
                }

                // Return array of table names (strings) for frontend compatibility
                var tableNames = suitableTables
                    .Select(t => ((dynamic)t).Name.ToString())
                    .OrderBy(name => name)
                    .ToList();

                _logger.LogInformation("Returning {Count} suitable tables from {DbContextName}: {TableNames}", 
                    tableNames.Count, dbContextName, string.Join(", ", tableNames));

                return new ApiResponse(200, $"Available lookup tables retrieved successfully from {dbContextName}", tableNames);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving available lookup tables");
                return new ApiResponse(500, $"Error retrieving available lookup tables: {ex.Message}");
            }
        }

        // ================================
        // GET LOOKUP TABLE COLUMNS
        // ================================
        public async Task<ApiResponse> GetLookupTableColumnsAsync(string tableName, string? database = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    return new ApiResponse(400, "Table name is required");
                }

                // Validate table name to prevent SQL injection
                if (!IsValidIdentifier(tableName))
                {
                    return new ApiResponse(400, $"Invalid table name: {tableName}");
                }

                // Determine which database context to use
                // Default: FormBuilderDbContext
                bool useFormBuilderDb = string.IsNullOrWhiteSpace(database) || 
                                       database.Equals("FormBuilder", StringComparison.OrdinalIgnoreCase);
                
                Type contextType;
                string dbContextName;
                
                if (useFormBuilderDb)
                {
                    contextType = typeof(FormBuilderDbContext);
                    dbContextName = "FormBuilderDbContext";
                }
                else if (database.Equals("AkhmanageIt", StringComparison.OrdinalIgnoreCase))
                {
                    contextType = typeof(AkhmanageItContext);
                    dbContextName = "AkhmanageItContext";
                }
                else
                {
                    return new ApiResponse(400, $"Invalid database name: {database}. Valid options: 'FormBuilder' or 'AkhmanageIt'");
                }

                // Check if table exists using reflection
                var dbSetProperty = contextType.GetProperty(tableName, System.Reflection.BindingFlags.IgnoreCase | 
                                                                        System.Reflection.BindingFlags.Public | 
                                                                        System.Reflection.BindingFlags.Instance);

                if (dbSetProperty == null)
                {
                    // Try case-insensitive search
                    var allProperties = contextType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    var matchingProperty = allProperties.FirstOrDefault(p => 
                        string.Equals(p.Name, tableName, StringComparison.OrdinalIgnoreCase) &&
                        p.PropertyType.IsGenericType && 
                        p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));
                    
                    if (matchingProperty == null)
                    {
                        var availableTables = string.Join(", ", allProperties
                            .Where(p => p.PropertyType.IsGenericType && 
                                       p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                            .Select(p => p.Name));
                        
                        _logger.LogWarning("Table '{TableName}' not found. Available tables: {AvailableTables}", 
                            tableName, availableTables);
                        return new ApiResponse(404, $"Table '{tableName}' not found. Available tables: {availableTables}");
                    }
                    
                    dbSetProperty = matchingProperty;
                }

                // Get the entity type
                var dbSetType = dbSetProperty.PropertyType;
                if (!dbSetType.IsGenericType || dbSetType.GetGenericTypeDefinition() != typeof(DbSet<>))
                {
                    return new ApiResponse(400, $"Property '{tableName}' is not a DbSet");
                }

                var entityType = dbSetType.GetGenericArguments()[0];

                // Get all properties (columns) from the entity type
                var columns = entityType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                    .Where(p => !p.Name.StartsWith("_")) // Skip private fields
                    .Select(p => p.Name)
                    .OrderBy(name => name)
                    .ToList();

                if (columns.Count == 0)
                {
                    return new ApiResponse(404, $"No columns found for table '{tableName}'");
                }

                return new ApiResponse(200, $"Columns retrieved successfully for table '{tableName}'", columns);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving columns for table '{TableName}'", tableName);
                return new ApiResponse(500, $"Error retrieving columns: {SanitizeForJson(ex.Message)}");
            }
        }

        // ================================
        // HELPER METHODS
        // ================================
        private ApiResponse ConvertToApiResponse<T>(ServiceResult<T> result)
        {
            if (result.Success)
                return new ApiResponse(result.StatusCode, "Success", result.Data);
            else
                return new ApiResponse(result.StatusCode, result.ErrorMessage);
        }

        private ApiResponse ConvertToApiResponse(ServiceResult<bool> result)
        {
            if (result.Success)
                return new ApiResponse(result.StatusCode, "Success", result.Data);
            else
                return new ApiResponse(result.StatusCode, result.ErrorMessage);
        }

        // ================================
        // DELETE ALL OPTIONS FOR FIELD
        // ================================
        private async Task DeleteAllOptionsForFieldAsync(int fieldId)
        {
            try
            {
                var existingOptions = await _unitOfWork.FieldOptionsRepository.GetByFieldIdAsync(fieldId);
                // Soft Delete
                foreach (var option in existingOptions)
                {
                    option.IsDeleted = true;
                    option.DeletedDate = DateTime.UtcNow;
                    option.IsActive = false;
                    _unitOfWork.FieldOptionsRepository.Update(option);
                }
                await _unitOfWork.CompleteAsyn();
                _logger.LogInformation("Soft deleted all options for field {FieldId} after setting Api/LookupTable DataSource", fieldId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting options for field {FieldId}", fieldId);
                // Don't throw - we still want to continue with DataSource creation/update
            }
        }

        // ================================
        // SAVE OPTIONS TO DATABASE (DEPRECATED - Only used for Static DataSource caching, not for Api/LookupTable)
        // ================================
        private async Task SaveOptionsToDatabaseAsync(int fieldId, List<FieldOptionResponseDto> options, int dataSourceId)
        {
            try
            {
                // Get existing options for this field
                var existingOptions = await _unitOfWork.FieldOptionsRepository.GetByFieldIdAsync(fieldId);
                var dataSource = await _fieldDataSourcesRepository.GetByIdAsync(dataSourceId);
                
                if (dataSource != null)
                {
                    // Soft Delete options that were created after or at the same time as the data source
                    // This helps identify options that came from data sources
                    var optionsToDelete = existingOptions
                        .Where(opt => opt.CreatedDate >= dataSource.CreatedDate && !opt.IsDeleted)
                        .ToList();

                    foreach (var opt in optionsToDelete)
                    {
                        opt.IsDeleted = true;
                        opt.DeletedDate = DateTime.UtcNow;
                        opt.IsActive = false;
                        _unitOfWork.FieldOptionsRepository.Update(opt);
                    }
                }

                // Add new options
                int order = 1;
                foreach (var option in options)
                {
                    var fieldOption = new FIELD_OPTIONS
                    {
                        FieldId = fieldId,
                        OptionValue = option.Value?.ToString() ?? "",
                        OptionText = option.Text ?? "",
                        OptionOrder = order++,
                        IsActive = true,
                        IsDefault = false,
                        CreatedDate = DateTime.UtcNow
                    };

                    _unitOfWork.FieldOptionsRepository.Add(fieldOption);
                }

                await _unitOfWork.CompleteAsyn();
                _logger.LogInformation("Saved {Count} options to database for field {FieldId} from data source {DataSourceId}", 
                    options.Count, fieldId, dataSourceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving options to database for field {FieldId}", fieldId);
                // Don't throw - we still want to return the options even if saving fails
            }
        }

        /// <summary>
        /// Combines Base URL and API Path to form full URL
        /// Supports flexible input: full URL or Base URL + Path
        /// Examples:
        /// - Base: "https://dummyjson.com/products" -> "https://dummyjson.com/products" (full URL, any random URL)
        /// - Base: "https://dummyjson.com/", Path: "products" -> "https://dummyjson.com/products"
        /// - Base: "https://randomuser.me/api/?results" -> "https://randomuser.me/api/?results" (full URL)
        /// - Base: "https://randomuser.me/api/", Path: "?results" -> "https://randomuser.me/api/?results"
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
    }
}
