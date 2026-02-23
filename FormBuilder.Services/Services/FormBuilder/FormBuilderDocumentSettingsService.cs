using AutoMapper;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Domian.Entitys.FromBuilder;
using formBuilder.Domian.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Services.Services.FormBuilder
{
    public class FormBuilderDocumentSettingsService : IFormBuilderDocumentSettingsService
    {
        private readonly IunitOfwork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<FormBuilderDocumentSettingsService>? _localizer;

        public FormBuilderDocumentSettingsService(
            IunitOfwork unitOfWork,
            IMapper mapper,
            IStringLocalizer<FormBuilderDocumentSettingsService>? localizer = null)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _localizer = localizer;
        }

        public async Task<ServiceResult<DocumentSettingsDto>> GetDocumentSettingsAsync(int formBuilderId)
        {
            try
            {
                // Verify FormBuilder exists
                var formBuilder = await _unitOfWork.FormBuilderRepository.SingleOrDefaultAsync(fb => fb.Id == formBuilderId);
                if (formBuilder == null)
                {
                    var message = _localizer?["FormBuilder_NotFound"] ?? "Form Builder not found";
                    return ServiceResult<DocumentSettingsDto>.NotFound(message);
                }

                // Get Document Type for this FormBuilder
                var documentTypes = await _unitOfWork.DocumentTypeRepository.GetByFormBuilderIdAsync(formBuilderId);
                var documentType = documentTypes.FirstOrDefault();

                var result = new DocumentSettingsDto
                {
                    FormBuilderId = formBuilderId,
                    FormBuilderName = formBuilder.FormName
                };

                if (documentType != null)
                {
                    result.DocumentTypeId = documentType.Id;
                    result.DocumentName = documentType.Name;
                    result.DocumentCode = documentType.Code;
                    result.MenuCaption = documentType.MenuCaption;
                    result.MenuOrder = documentType.MenuOrder;
                    result.ParentMenuId = documentType.ParentMenuId;
                    result.IsActive = documentType.IsActive;

                    // Get all Document Series for this Document Type
                    var seriesList = await _unitOfWork.DocumentSeriesRepository.GetByDocumentTypeIdAsync(documentType.Id);
                    result.DocumentSeries = _mapper.Map<List<DocumentSeriesDto>>(seriesList);
                }

                return ServiceResult<DocumentSettingsDto>.Ok(result);
            }
            catch (Exception ex)
            {
                var message = _localizer?["FormBuilderDocumentSettings_GetError"] ?? "Error retrieving document settings";
                return ServiceResult<DocumentSettingsDto>.Error($"{message}: {ex.Message}");
            }
        }

        public async Task<ServiceResult<DocumentSettingsDto>> SaveDocumentSettingsAsync(SaveDocumentSettingsDto dto)
        {
            try
            {
                // Verify FormBuilder exists
                var formBuilder = await _unitOfWork.FormBuilderRepository.SingleOrDefaultAsync(fb => fb.Id == dto.FormBuilderId);
                if (formBuilder == null)
                {
                    var message = _localizer?["FormBuilder_NotFound"] ?? "Form Builder not found";
                    return ServiceResult<DocumentSettingsDto>.BadRequest(message);
                }

                // Check if Document Code already exists for another FormBuilder
                var existingDocType = await _unitOfWork.DocumentTypeRepository.GetByCodeAsync(dto.DocumentCode);
                if (existingDocType != null && existingDocType.FormBuilderId != dto.FormBuilderId)
                {
                    var message = _localizer?["DocumentType_CodeExists"] ?? "Document code already exists for another form";
                    return ServiceResult<DocumentSettingsDto>.BadRequest(message);
                }

                // Get or create Document Type
                // Load with no tracking to avoid navigation property update conflicts
                var dbContextForQuery = _unitOfWork.AppDbContext;
                var documentType = await dbContextForQuery.Set<DOCUMENT_TYPES>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(dt => dt.FormBuilderId == dto.FormBuilderId);

                if (documentType == null)
                {
                    // Create new Document Type
                    documentType = new DOCUMENT_TYPES
                    {
                        Name = dto.DocumentName,
                        Code = dto.DocumentCode,
                        FormBuilderId = dto.FormBuilderId,
                        MenuCaption = dto.MenuCaption,
                        MenuOrder = dto.MenuOrder,
                        ParentMenuId = dto.ParentMenuId,
                        IsActive = dto.IsActive,
                        CreatedDate = DateTime.UtcNow
                    };

                    _unitOfWork.DocumentTypeRepository.Add(documentType);
                    
                    // Save Document Type first to get the generated ID
                    // This is required before creating Document Series that reference it
                    await _unitOfWork.CompleteAsyn();
                    
                    // Reload to get the generated ID
                    documentType = await _unitOfWork.DocumentTypeRepository.GetByIdAsync(documentType.Id);
                    if (documentType == null)
                    {
                        var message = _localizer?["DocumentType_CreateError"] ?? "Failed to create Document Type";
                        return ServiceResult<DocumentSettingsDto>.Error(message);
                    }
                }
                else
                {
                    // Check if ParentMenuId is being changed (removed or changed)
                    bool parentMenuIdChanged = dto.ParentMenuId.HasValue 
                        ? (documentType.ParentMenuId != dto.ParentMenuId.Value)
                        : (documentType.ParentMenuId.HasValue);

                    // If removing parent relationship (setting to null) and this entity has children
                    if (parentMenuIdChanged && (!dto.ParentMenuId.HasValue) && documentType.ParentMenuId.HasValue)
                    {
                        // Use raw SQL to update children directly - this bypasses EF tracking issues
                        // and works even if the constraint is still RESTRICT
                        var dbContext = _unitOfWork.AppDbContext;
                        await dbContext.Database.ExecuteSqlRawAsync(
                            "UPDATE DOCUMENT_TYPES SET ParentMenuId = NULL, UpdatedDate = GETUTCDATE() WHERE ParentMenuId = {0}",
                            documentType.Id);
                    }

                    // Use raw SQL to update the entity directly - this bypasses EF tracking issues
                    // and prevents conflicts with Foreign Key constraints
                    var dbContextForUpdate = _unitOfWork.AppDbContext;
                    var sqlParams = new List<object>();
                    var updateFields = new List<string>();
                    int paramIndex = 0;

                    updateFields.Add($"Name = {{{paramIndex}}}");
                    sqlParams.Add(dto.DocumentName);
                    paramIndex++;

                    updateFields.Add($"Code = {{{paramIndex}}}");
                    sqlParams.Add(dto.DocumentCode);
                    paramIndex++;

                    if (dto.MenuCaption != null)
                    {
                        updateFields.Add($"MenuCaption = {{{paramIndex}}}");
                        sqlParams.Add(dto.MenuCaption);
                        paramIndex++;
                    }

                    updateFields.Add($"MenuOrder = {{{paramIndex}}}");
                    sqlParams.Add(dto.MenuOrder);
                    paramIndex++;

                    // Handle ParentMenuId: use NULL in SQL if value is null
                    if (dto.ParentMenuId.HasValue)
                    {
                        updateFields.Add($"ParentMenuId = {{{paramIndex}}}");
                        sqlParams.Add(dto.ParentMenuId.Value);
                        paramIndex++;
                    }
                    else
                    {
                        updateFields.Add("ParentMenuId = NULL");
                    }

                    updateFields.Add($"IsActive = {{{paramIndex}}}");
                    sqlParams.Add(dto.IsActive);
                    paramIndex++;

                    updateFields.Add("UpdatedDate = GETUTCDATE()");

                    // Add id as the last parameter for WHERE clause
                    sqlParams.Add(documentType.Id);
                    var sql = $"UPDATE DOCUMENT_TYPES SET {string.Join(", ", updateFields)} WHERE Id = {{{paramIndex}}}";
                    await dbContextForUpdate.Database.ExecuteSqlRawAsync(sql, sqlParams.ToArray());

                    // Reload the entity to ensure we have the latest state (with no tracking to avoid conflicts)
                    documentType = await _unitOfWork.DocumentTypeRepository.GetByIdAsync(documentType.Id);
                    
                    // Detach the entity from EF tracking to prevent navigation property updates
                    var dbContextForDetach = _unitOfWork.AppDbContext;
                    dbContextForDetach.Entry(documentType).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                }

                // No need to call CompleteAsyn() here since we used raw SQL for DocumentType update
                // CompleteAsyn() will be called after DocumentSeries updates

                // Handle Document Series
                if (dto.DocumentSeries != null && dto.DocumentSeries.Any())
                {
                    // Validate all Projects exist before processing
                    foreach (var seriesDto in dto.DocumentSeries)
                    {
                        NormalizeSeriesDto(seriesDto);

                        if (!DocumentSeriesEngineRules.TryValidateTemplate(seriesDto.Template, out var templateError))
                        {
                            return ServiceResult<DocumentSettingsDto>.BadRequest(templateError);
                        }

                        if (!DocumentSeriesEngineRules.TryNormalizeResetPolicy(seriesDto.ResetPolicy, out var normalizedResetPolicy))
                        {
                            return ServiceResult<DocumentSettingsDto>.BadRequest("ResetPolicy must be one of: None, Yearly, Monthly, Daily.");
                        }
                        seriesDto.ResetPolicy = normalizedResetPolicy;

                        if (!DocumentSeriesEngineRules.TryNormalizeGenerateOn(seriesDto.GenerateOn, out var normalizedGenerateOn))
                        {
                            return ServiceResult<DocumentSettingsDto>.BadRequest("GenerateOn must be one of: Submit, Approval.");
                        }
                        seriesDto.GenerateOn = normalizedGenerateOn;

                        var project = await _unitOfWork.ProjectRepository.GetByIdAsync(seriesDto.ProjectId);
                        if (project == null)
                        {
                            var message = _localizer?["Project_NotFound"] ?? $"Project with ID {seriesDto.ProjectId} not found. Please verify the Project ID exists in the system.";
                            return ServiceResult<DocumentSettingsDto>.NotFound(message);
                        }
                        if (!project.IsActive)
                        {
                            var message = _localizer?["Project_Inactive"] ?? $"Project with ID {seriesDto.ProjectId} exists but is not active. Please activate the project first.";
                            return ServiceResult<DocumentSettingsDto>.BadRequest(message);
                        }
                    }
                    
                    foreach (var seriesDto in dto.DocumentSeries)
                    {
                        if (seriesDto.Id.HasValue)
                        {
                            // Update existing series
                            var existingSeries = await _unitOfWork.DocumentSeriesRepository.GetByIdAsync(seriesDto.Id.Value);
                            if (existingSeries != null)
                            {
                                // Check if series code already exists for another series
                                var codeExists = await _unitOfWork.DocumentSeriesRepository.SeriesCodeExistsAsync(
                                    seriesDto.SeriesCode, seriesDto.Id);
                                if (codeExists)
                                {
                                    var message = _localizer?["DocumentSeries_CodeExists"] ?? "Series code already exists";
                                    return ServiceResult<DocumentSettingsDto>.BadRequest(message);
                                }

                                existingSeries.ProjectId = seriesDto.ProjectId;
                                existingSeries.SeriesCode = seriesDto.SeriesCode;
                                existingSeries.SeriesName = seriesDto.SeriesName!;
                                existingSeries.Template = seriesDto.Template!;
                                existingSeries.SequenceStart = seriesDto.SequenceStart;
                                existingSeries.SequencePadding = seriesDto.SequencePadding;
                                existingSeries.ResetPolicy = seriesDto.ResetPolicy;
                                existingSeries.GenerateOn = seriesDto.GenerateOn;
                                existingSeries.NextNumber = seriesDto.NextNumber;
                                existingSeries.IsDefault = seriesDto.IsDefault;
                                existingSeries.IsActive = seriesDto.IsActive;
                                existingSeries.UpdatedDate = DateTime.UtcNow;

                                // If setting as default, remove default from other series with same document type and project
                                if (seriesDto.IsDefault)
                                {
                                    await RemoveDefaultFromOtherSeriesAsync(documentType.Id, seriesDto.ProjectId, existingSeries.Id);
                                }

                                _unitOfWork.DocumentSeriesRepository.Update(existingSeries);
                            }
                        }
                        else
                        {
                            // Create new series
                            // Check if series code already exists
                            var codeExists = await _unitOfWork.DocumentSeriesRepository.SeriesCodeExistsAsync(seriesDto.SeriesCode);
                            if (codeExists)
                            {
                                var message = _localizer?["DocumentSeries_CodeExists"] ?? "Series code already exists";
                                return ServiceResult<DocumentSettingsDto>.BadRequest(message);
                            }

                            var newSeries = new DOCUMENT_SERIES
                            {
                                ProjectId = seriesDto.ProjectId,
                                SeriesCode = seriesDto.SeriesCode,
                                SeriesName = seriesDto.SeriesName!,
                                Template = seriesDto.Template!,
                                SequenceStart = seriesDto.SequenceStart,
                                SequencePadding = seriesDto.SequencePadding,
                                ResetPolicy = seriesDto.ResetPolicy,
                                GenerateOn = seriesDto.GenerateOn,
                                NextNumber = seriesDto.NextNumber,
                                IsDefault = seriesDto.IsDefault,
                                IsActive = seriesDto.IsActive,
                                CreatedDate = DateTime.UtcNow
                            };

                            // If setting as default, remove default from other series with same document type and project
                            if (seriesDto.IsDefault)
                            {
                                await RemoveDefaultFromOtherSeriesAsync(documentType.Id, seriesDto.ProjectId, null);
                            }

                            _unitOfWork.DocumentSeriesRepository.Add(newSeries);
                        }
                    }
                }

                await _unitOfWork.CompleteAsyn();

                // Return updated settings
                return await GetDocumentSettingsAsync(dto.FormBuilderId);
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database-specific errors (constraints, foreign keys, etc.)
                var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                var message = _localizer?["FormBuilderDocumentSettings_SaveError"] ?? "Error saving document settings";
                
                // Check for common constraint violations
                if (innerMessage.Contains("UNIQUE") || innerMessage.Contains("duplicate"))
                {
                    return ServiceResult<DocumentSettingsDto>.BadRequest(
                        "A document with this code or series code already exists. Please use a unique code.");
                }
                if (innerMessage.Contains("FOREIGN KEY") || innerMessage.Contains("constraint"))
                {
                    // Try to extract more specific information
                    string detailedMessage = "Invalid reference detected. ";
                    if (innerMessage.Contains("PROJECTS") || innerMessage.Contains("Project") || innerMessage.Contains("FK_DOCUMENT_SERIES_PROJECTS"))
                    {
                        detailedMessage += "The specified Project ID does not exist or is not active. ";
                        detailedMessage += "Please verify the Project ID exists and is active before creating Document Series.";
                    }
                    else if (innerMessage.Contains("DOCUMENT_TYPES") || innerMessage.Contains("DocumentType") || innerMessage.Contains("FK_DOCUMENT_SERIES_DOCUMENT_TYPES"))
                    {
                        detailedMessage += "The Document Type reference is invalid. ";
                    }
                    else
                    {
                        detailedMessage += "Please verify that all referenced entities exist and are active.";
                    }
                    
                    return ServiceResult<DocumentSettingsDto>.BadRequest(detailedMessage);
                }
                
                return ServiceResult<DocumentSettingsDto>.Error($"{message}: {innerMessage}");
            }
            catch (Exception ex)
            {
                var message = _localizer?["FormBuilderDocumentSettings_SaveError"] ?? "Error saving document settings";
                var fullMessage = ex.InnerException != null 
                    ? $"{ex.Message} | Inner Exception: {ex.InnerException.Message}"
                    : ex.Message;
                return ServiceResult<DocumentSettingsDto>.Error($"{message}: {fullMessage}");
            }
        }

        public async Task<ServiceResult<bool>> DeleteDocumentSettingsAsync(int formBuilderId)
        {
            try
            {
                // Get Document Type for this FormBuilder
                var documentTypes = await _unitOfWork.DocumentTypeRepository.GetByFormBuilderIdAsync(formBuilderId);
                var documentType = documentTypes.FirstOrDefault();

                if (documentType == null)
                {
                    // Nothing to delete
                    return ServiceResult<bool>.Ok(true);
                }

                // Get all Document Series for this Document Type
                var seriesList = await _unitOfWork.DocumentSeriesRepository.GetByDocumentTypeIdAsync(documentType.Id);

                // Soft Delete all series
                foreach (var series in seriesList)
                {
                    series.IsDeleted = true;
                    series.DeletedDate = DateTime.UtcNow;
                    series.IsActive = false;
                    _unitOfWork.DocumentSeriesRepository.Update(series);
                }

                // Soft Delete Document Type
                documentType.IsDeleted = true;
                documentType.DeletedDate = DateTime.UtcNow;
                documentType.IsActive = false;
                _unitOfWork.DocumentTypeRepository.Update(documentType);

                await _unitOfWork.CompleteAsyn();

                return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                var message = _localizer?["FormBuilderDocumentSettings_DeleteError"] ?? "Error deleting document settings";
                return ServiceResult<bool>.Error($"{message}: {ex.Message}");
            }
        }

        /// <summary>
        /// Auto-configure default Document Settings for a Form Builder
        /// Creates a default Document Type and Series if they don't exist
        /// </summary>
        public async Task<ServiceResult<DocumentSettingsDto>> AutoConfigureDefaultsAsync(int formBuilderId, int projectId, string? documentCode = null, string? seriesCode = null)
        {
            try
            {
                // Verify FormBuilder exists
                var formBuilder = await _unitOfWork.FormBuilderRepository.SingleOrDefaultAsync(fb => fb.Id == formBuilderId);
                if (formBuilder == null)
                {
                    var message = _localizer?["FormBuilder_NotFound"] ?? "Form Builder not found";
                    return ServiceResult<DocumentSettingsDto>.NotFound(message);
                }

                // Check if settings already exist
                var existingSettings = await GetDocumentSettingsAsync(formBuilderId);
                if (existingSettings.Success && existingSettings.Data?.DocumentTypeId.HasValue == true)
                {
                    // Settings already exist, return them
                    return existingSettings;
                }

                // Generate default codes if not provided
                // Use FormCode if available, otherwise use FormName, fallback to "DOC"
                var baseCode = documentCode;
                if (string.IsNullOrWhiteSpace(baseCode))
                {
                    if (!string.IsNullOrWhiteSpace(formBuilder.FormCode))
                    {
                        baseCode = formBuilder.FormCode.ToUpperInvariant();
                    }
                    else if (!string.IsNullOrWhiteSpace(formBuilder.FormName))
                    {
                        // Use FormName, remove spaces and special characters
                        baseCode = new string(formBuilder.FormName.Where(c => char.IsLetterOrDigit(c)).ToArray()).ToUpperInvariant();
                        if (string.IsNullOrWhiteSpace(baseCode))
                        {
                            baseCode = "DOC";
                        }
                    }
                    else
                    {
                        baseCode = "DOC";
                    }
                }
                
                var defaultDocumentCode = baseCode;
                var defaultSeriesCode = seriesCode ?? $"{defaultDocumentCode}-SERIES";

                // Check if document code already exists
                var existingDocType = await _unitOfWork.DocumentTypeRepository.GetByCodeAsync(defaultDocumentCode);
                if (existingDocType != null && existingDocType.FormBuilderId != formBuilderId)
                {
                    // Code exists for another form, append form ID to make it unique
                    defaultDocumentCode = $"{defaultDocumentCode}-{formBuilderId}";
                    defaultSeriesCode = seriesCode ?? $"{defaultDocumentCode}-SERIES";
                }
                
                // Validate Project exists and is active
                var project = await _unitOfWork.ProjectRepository.GetByIdAsync(projectId);
                if (project == null)
                {
                    var message = _localizer?["Project_NotFound"] ?? $"Project with ID {projectId} not found. Please verify the Project ID exists in the system.";
                    return ServiceResult<DocumentSettingsDto>.NotFound(message);
                }
                if (!project.IsActive)
                {
                    var message = _localizer?["Project_Inactive"] ?? $"Project with ID {projectId} exists but is not active. Please activate the project first.";
                    return ServiceResult<DocumentSettingsDto>.BadRequest(message);
                }

                // Create default Document Settings
                var saveDto = new SaveDocumentSettingsDto
                {
                    FormBuilderId = formBuilderId,
                    DocumentName = formBuilder.FormName,
                    DocumentCode = defaultDocumentCode,
                    MenuCaption = formBuilder.FormName,
                    MenuOrder = 0,
                    ParentMenuId = null,
                    IsActive = true,
                    DocumentSeries = new List<SaveDocumentSeriesDto>
                    {
                        new SaveDocumentSeriesDto
                        {
                            ProjectId = projectId,
                            SeriesCode = defaultSeriesCode,
                            SeriesName = defaultSeriesCode,
                            Template = $"{defaultDocumentCode}-{{YYYY}}-{{SEQ}}",
                            SequenceStart = 1,
                            SequencePadding = 3,
                            ResetPolicy = "Yearly",
                            GenerateOn = "Submit",
                            NextNumber = 1,
                            IsDefault = true,
                            IsActive = true
                        }
                    }
                };

                // Use existing SaveDocumentSettingsAsync to create the settings
                return await SaveDocumentSettingsAsync(saveDto);
            }
            catch (Exception ex)
            {
                var message = _localizer?["FormBuilderDocumentSettings_AutoConfigureError"] ?? "Error auto-configuring document settings";
                return ServiceResult<DocumentSettingsDto>.Error($"{message}: {ex.Message}");
            }
        }

        /// <summary>
        /// Remove default flag from other series with the same document type and project
        /// </summary>
        private async Task RemoveDefaultFromOtherSeriesAsync(int documentTypeId, int projectId, int? excludeSeriesId)
        {
            var allSeries = await _unitOfWork.DocumentSeriesRepository.GetByDocumentTypeIdAsync(documentTypeId);
            var otherSeries = allSeries.Where(s => s.ProjectId == projectId && 
                                                   s.IsDefault && 
                                                   (!excludeSeriesId.HasValue || s.Id != excludeSeriesId.Value))
                                      .ToList();

            foreach (var series in otherSeries)
            {
                series.IsDefault = false;
                series.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.DocumentSeriesRepository.Update(series);
            }
        }

        private static void NormalizeSeriesDto(SaveDocumentSeriesDto dto)
        {
            dto.SeriesName = string.IsNullOrWhiteSpace(dto.SeriesName) ? dto.SeriesCode : dto.SeriesName.Trim();
            dto.Template = string.IsNullOrWhiteSpace(dto.Template) ? $"{dto.SeriesCode}-{{SEQ}}" : dto.Template.Trim();
            dto.SequenceStart = dto.SequenceStart <= 0 ? 1 : dto.SequenceStart;
            dto.SequencePadding = dto.SequencePadding <= 0 ? 3 : dto.SequencePadding;
            dto.NextNumber = dto.NextNumber <= 0 ? dto.SequenceStart : dto.NextNumber;
            dto.ResetPolicy = string.IsNullOrWhiteSpace(dto.ResetPolicy) ? "None" : dto.ResetPolicy.Trim();
            dto.GenerateOn = string.IsNullOrWhiteSpace(dto.GenerateOn) ? "Submit" : dto.GenerateOn.Trim();
        }
    }
}
