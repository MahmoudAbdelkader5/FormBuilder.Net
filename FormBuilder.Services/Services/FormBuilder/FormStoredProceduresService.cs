using AutoMapper;
using formBuilder.Domian.Interfaces;
using FormBuilder.API.Models;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Core.IServices.FormBuilder;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domian.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Services.Services.FormBuilder
{
    public class FormStoredProceduresService : IFormStoredProceduresService
    {
        private readonly IFormStoredProceduresRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FormStoredProceduresService> _logger;
        private readonly IunitOfwork _unitOfWork;

        public FormStoredProceduresService(
            IFormStoredProceduresRepository repository,
            IMapper mapper,
            ILogger<FormStoredProceduresService> logger,
            IunitOfwork unitOfWork)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ApiResponse> GetAllAsync()
        {
            try
            {
                var storedProcedures = await _repository.GetActiveAsync();
                var storedProceduresDto = _mapper.Map<System.Collections.Generic.List<StoredProcedureDto>>(storedProcedures);
                return new ApiResponse(200, "Stored procedures retrieved successfully", storedProceduresDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving stored procedures");
                return new ApiResponse(500, $"Error retrieving stored procedures: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            try
            {
                var storedProcedure = await _repository.SingleOrDefaultAsync(sp => sp.Id == id && !sp.IsDeleted);
                if (storedProcedure == null)
                {
                    return new ApiResponse(404, "Stored procedure not found");
                }

                var storedProcedureDto = _mapper.Map<StoredProcedureDto>(storedProcedure);
                return new ApiResponse(200, "Stored procedure retrieved successfully", storedProcedureDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving stored procedure {StoredProcedureId}", id);
                return new ApiResponse(500, $"Error retrieving stored procedure: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetByUsageTypeAsync(string? usageType)
        {
            try
            {
                var storedProcedures = await _repository.GetByUsageTypeAsync(usageType);
                var storedProceduresDto = _mapper.Map<System.Collections.Generic.List<StoredProcedureDto>>(storedProcedures);
                return new ApiResponse(200, "Stored procedures retrieved successfully", storedProceduresDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving stored procedures by usage type {UsageType}", usageType);
                return new ApiResponse(500, $"Error retrieving stored procedures: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetByDatabaseAsync(string databaseName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(databaseName))
                {
                    return new ApiResponse(400, "Database name is required");
                }

                var storedProcedures = await _repository.GetByDatabaseAsync(databaseName);
                var storedProceduresDto = _mapper.Map<System.Collections.Generic.List<StoredProcedureDto>>(storedProcedures);
                return new ApiResponse(200, "Stored procedures retrieved successfully", storedProceduresDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving stored procedures for database {DatabaseName}", databaseName);
                return new ApiResponse(500, $"Error retrieving stored procedures: {ex.Message}");
            }
        }

        public async Task<ApiResponse> CreateAsync(CreateStoredProcedureDto createDto, string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new ApiResponse(400, "User ID is required");
                }

                if (createDto == null)
                {
                    return new ApiResponse(400, "Stored procedure data is required");
                }

                // Validate database name
                if (!createDto.DatabaseName.Equals("FormBuilder", StringComparison.OrdinalIgnoreCase) &&
                    !createDto.DatabaseName.Equals("AKHManageIT", StringComparison.OrdinalIgnoreCase))
                {
                    return new ApiResponse(400, "Database name must be either 'FormBuilder' or 'AKHManageIT'");
                }

                // Extract procedure name from ProcedureCode if not provided
                var procedureName = createDto.ProcedureName;
                if (string.IsNullOrWhiteSpace(procedureName) && !string.IsNullOrWhiteSpace(createDto.ProcedureCode))
                {
                    procedureName = ExtractProcedureName(createDto.ProcedureCode);
                }

                // Check for duplicate by ProcedureCode (exact match)
                var existing = await _repository.GetByDatabaseSchemaAndProcedureAsync(
                    createDto.DatabaseName,
                    createDto.SchemaName ?? "dbo",
                    procedureName);

                if (existing != null && !existing.IsDeleted)
                {
                    return new ApiResponse(409, "A stored procedure with the same database, schema, and procedure name already exists");
                }

                var entity = _mapper.Map<FORM_STORED_PROCEDURES>(createDto);
                entity.CreatedByUserId = userId;
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsActive = true;
                entity.IsDeleted = false;
                entity.SchemaName = entity.SchemaName ?? "dbo";
                
                // Set ProcedureName if extracted
                if (!string.IsNullOrWhiteSpace(procedureName))
                {
                    entity.ProcedureName = procedureName;
                }

                _repository.Add(entity);
                await _unitOfWork.CompleteAsyn();

                var storedProcedureDto = _mapper.Map<StoredProcedureDto>(entity);
                return new ApiResponse(201, "Stored procedure created successfully", storedProcedureDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating stored procedure for user {UserId}", userId);
                return new ApiResponse(500, $"Error creating stored procedure: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateAsync(int id, UpdateStoredProcedureDto updateDto, string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new ApiResponse(400, "User ID is required");
                }

                var entity = await _repository.SingleOrDefaultAsync(sp => sp.Id == id && !sp.IsDeleted);
                if (entity == null)
                {
                    return new ApiResponse(404, "Stored procedure not found");
                }

                // Update only provided fields
                if (!string.IsNullOrWhiteSpace(updateDto.Title))
                {
                    entity.Title = updateDto.Title;
                }

                if (updateDto.Description != null)
                {
                    entity.Description = updateDto.Description;
                }

                if (!string.IsNullOrWhiteSpace(updateDto.DatabaseName))
                {
                    // Validate database name
                    if (!updateDto.DatabaseName.Equals("FormBuilder", StringComparison.OrdinalIgnoreCase) &&
                        !updateDto.DatabaseName.Equals("AKHManageIT", StringComparison.OrdinalIgnoreCase))
                    {
                        return new ApiResponse(400, "Database name must be either 'FormBuilder' or 'AKHManageIT'");
                    }

                    // Extract procedure name from ProcedureCode if updating
                    string? newProcedureName = null;
                    if (!string.IsNullOrWhiteSpace(updateDto.ProcedureCode))
                    {
                        newProcedureName = ExtractProcedureName(updateDto.ProcedureCode);
                    }
                    else if (!string.IsNullOrWhiteSpace(updateDto.ProcedureName))
                    {
                        newProcedureName = updateDto.ProcedureName;
                    }

                    // Check for duplicate if database/schema/procedure name changed
                    if (updateDto.DatabaseName != entity.DatabaseName ||
                        updateDto.SchemaName != entity.SchemaName ||
                        !string.IsNullOrWhiteSpace(newProcedureName) && newProcedureName != entity.ProcedureName)
                    {
                        var checkName = newProcedureName ?? entity.ProcedureName;
                        if (!string.IsNullOrWhiteSpace(checkName))
                        {
                            var existing = await _repository.GetByDatabaseSchemaAndProcedureAsync(
                                updateDto.DatabaseName ?? entity.DatabaseName,
                                updateDto.SchemaName ?? entity.SchemaName,
                                checkName);

                            if (existing != null && existing.Id != id && !existing.IsDeleted)
                            {
                                return new ApiResponse(409, "A stored procedure with the same database, schema, and procedure name already exists");
                            }
                        }
                    }

                    entity.DatabaseName = updateDto.DatabaseName;
                }

                if (!string.IsNullOrWhiteSpace(updateDto.SchemaName))
                {
                    entity.SchemaName = updateDto.SchemaName;
                }

                // Update ProcedureCode if provided
                if (!string.IsNullOrWhiteSpace(updateDto.ProcedureCode))
                {
                    entity.ProcedureCode = updateDto.ProcedureCode;
                    
                    // Extract and update ProcedureName from ProcedureCode
                    var extractedName = ExtractProcedureName(updateDto.ProcedureCode);
                    if (!string.IsNullOrWhiteSpace(extractedName))
                    {
                        entity.ProcedureName = extractedName;
                    }
                }
                else if (!string.IsNullOrWhiteSpace(updateDto.ProcedureName))
                {
                    entity.ProcedureName = updateDto.ProcedureName;
                }

                if (!string.IsNullOrWhiteSpace(updateDto.UsageType))
                {
                    entity.UsageType = updateDto.UsageType;
                }

                if (updateDto.IsReadOnly.HasValue)
                {
                    entity.IsReadOnly = updateDto.IsReadOnly.Value;
                }

                if (updateDto.ExecutionOrder.HasValue)
                {
                    entity.ExecutionOrder = updateDto.ExecutionOrder.Value;
                }

                if (updateDto.IsActive.HasValue)
                {
                    entity.IsActive = updateDto.IsActive.Value;
                }

                entity.UpdatedDate = DateTime.UtcNow;

                _repository.Update(entity);
                await _unitOfWork.CompleteAsyn();

                var storedProcedureDto = _mapper.Map<StoredProcedureDto>(entity);
                return new ApiResponse(200, "Stored procedure updated successfully", storedProcedureDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating stored procedure {StoredProcedureId} for user {UserId}", id, userId);
                return new ApiResponse(500, $"Error updating stored procedure: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteAsync(int id, string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new ApiResponse(400, "User ID is required");
                }

                var entity = await _repository.SingleOrDefaultAsync(sp => sp.Id == id && !sp.IsDeleted);
                if (entity == null)
                {
                    return new ApiResponse(404, "Stored procedure not found");
                }

                _repository.Delete(entity);
                await _unitOfWork.CompleteAsyn();

                return new ApiResponse(200, "Stored procedure deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting stored procedure {StoredProcedureId} for user {UserId}", id, userId);
                return new ApiResponse(500, $"Error deleting stored procedure: {ex.Message}");
            }
        }

        public async Task<ApiResponse> SoftDeleteAsync(int id, string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new ApiResponse(400, "User ID is required");
                }

                var entity = await _repository.SingleOrDefaultAsync(sp => sp.Id == id && !sp.IsDeleted);
                if (entity == null)
                {
                    return new ApiResponse(404, "Stored procedure not found");
                }

                entity.IsDeleted = true;
                entity.DeletedDate = DateTime.UtcNow;
                entity.DeletedByUserId = userId;

                _repository.Update(entity);
                await _unitOfWork.CompleteAsyn();

                return new ApiResponse(200, "Stored procedure deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error soft deleting stored procedure {StoredProcedureId} for user {UserId}", id, userId);
                return new ApiResponse(500, $"Error deleting stored procedure: {ex.Message}");
            }
        }

        public async Task<ApiResponse> ValidateStoredProcedureAsync(int storedProcedureId)
        {
            try
            {
                var storedProcedure = await _repository.SingleOrDefaultAsync(sp => sp.Id == storedProcedureId && !sp.IsDeleted);
                if (storedProcedure == null)
                {
                    return new ApiResponse(404, "Stored procedure not found");
                }

                if (!storedProcedure.IsActive)
                {
                    return new ApiResponse(400, "Stored procedure is not active");
                }

                return new ApiResponse(200, "Stored procedure is valid", _mapper.Map<StoredProcedureDto>(storedProcedure));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating stored procedure {StoredProcedureId}", storedProcedureId);
                return new ApiResponse(500, $"Error validating stored procedure: {ex.Message}");
            }
        }

        /// <summary>
        /// Extract procedure name from ProcedureCode (CREATE PROCEDURE ...)
        /// </summary>
        private string? ExtractProcedureName(string procedureCode)
        {
            if (string.IsNullOrWhiteSpace(procedureCode))
                return null;

            // Try to extract procedure name from CREATE PROCEDURE statement
            // Pattern: CREATE PROCEDURE [schema.]procedure_name
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
                    // If schema is captured, return schema.procedure, otherwise just procedure
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
    }
}

