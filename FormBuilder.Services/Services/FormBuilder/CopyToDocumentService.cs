using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FormBuilder.Core.DTOS.FormRules;
using FormBuilder.Core.IServices.FormBuilder;
using FormBuilder.Domain.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domian.Entitys.froms;
using formBuilder.Domian.Interfaces;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Core.IServices;
using FormBuilder.Infrastructure.Data;

namespace FormBuilder.Services.Services.FormBuilder
{
    /// <summary>
    /// Service for executing CopyToDocument action
    /// Handles copying data from one form submission to another document
    /// </summary>
    public class CopyToDocumentService : ICopyToDocumentService
    {
        private readonly IunitOfwork _unitOfWork;
        private readonly ILogger<CopyToDocumentService>? _logger;
        private readonly IFormSubmissionsService _formSubmissionsService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IServiceScopeFactory? _serviceScopeFactory;

        public CopyToDocumentService(
            IunitOfwork unitOfWork,
            ILogger<CopyToDocumentService>? logger,
            IFormSubmissionsService formSubmissionsService,
            IFileStorageService fileStorageService,
            IServiceScopeFactory? serviceScopeFactory = null)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _formSubmissionsService = formSubmissionsService;
            _fileStorageService = fileStorageService;
            _serviceScopeFactory = serviceScopeFactory;
            
            if (_serviceScopeFactory == null)
            {
                _logger?.LogWarning("IServiceScopeFactory is null in CopyToDocumentService constructor. Audit logging may fail!");
            }
            else
            {
                _logger?.LogInformation("IServiceScopeFactory successfully injected into CopyToDocumentService");
            }
        }

        public async Task<CopyToDocumentResultDto> ExecuteCopyToDocumentAsync(
            CopyToDocumentActionDto config,
            int sourceSubmissionId,
            int? actionId = null,
            int? ruleId = null,
            string? executedByUserId = null)
        {
            var actualSourceSubmissionId = config.SourceSubmissionId ?? sourceSubmissionId;
            var result = new CopyToDocumentResultDto
            {
                Success = false,
                SourceSubmissionId = actualSourceSubmissionId,
                ActionId = actionId,
                FieldsCopied = 0,
                GridRowsCopied = 0
            };

            int? sourceFormIdForAudit = null;
            var database = _unitOfWork.AppDbContext.Database;
            IDbContextTransaction? transaction = null;
            var ownsTransaction = database.CurrentTransaction == null;
            if (ownsTransaction)
            {
                transaction = await database.BeginTransactionAsync();
            }
            FORM_SUBMISSIONS? targetDocument = null;
            bool auditLogged = false;

            try
            {
                _logger?.LogInformation("Starting CopyToDocument execution. SourceSubmissionId: {SourceSubmissionId}, SourceDocumentTypeId: {SourceDocumentTypeId}, SourceFormId: {SourceFormId}, TargetFormId: {TargetFormId}, TargetDocumentTypeId: {TargetDocumentTypeId}",
                    actualSourceSubmissionId, config.SourceDocumentTypeId, config.SourceFormId, config.TargetFormId, config.TargetDocumentTypeId);

                // 1. Validate source document type and form
                var sourceDocumentType = await _unitOfWork.DocumentTypeRepository.GetByIdAsync(config.SourceDocumentTypeId);
                if (sourceDocumentType == null)
                {
                    result.ErrorMessage = $"Source document type {config.SourceDocumentTypeId} not found";
                    sourceFormIdForAudit = config.SourceFormId;
                    return result;
                }

                var sourceForm = await _unitOfWork.FormBuilderRepository.SingleOrDefaultAsync(f => f.Id == config.SourceFormId && !f.IsDeleted);
                if (sourceForm == null)
                {
                    result.ErrorMessage = $"Source form {config.SourceFormId} not found";
                    sourceFormIdForAudit = config.SourceFormId;
                    return result;
                }

                // 2. Load source submission (optional)
                FORM_SUBMISSIONS? sourceSubmission = null;
                if (actualSourceSubmissionId > 0)
                {
                    sourceSubmission = await _unitOfWork.FormSubmissionsRepository.GetByIdWithDetailsAsync(actualSourceSubmissionId);
                    if (sourceSubmission == null)
                    {
                        result.ErrorMessage = $"Source submission {actualSourceSubmissionId} not found";
                        sourceFormIdForAudit = config.SourceFormId;
                        return result;
                    }

                    // 3. Validate source document compatibility
                    if (sourceSubmission.DocumentTypeId != config.SourceDocumentTypeId)
                    {
                        result.ErrorMessage = $"Source submission {actualSourceSubmissionId} document type {sourceSubmission.DocumentTypeId} does not match configured SourceDocumentTypeId {config.SourceDocumentTypeId}";
                        sourceFormIdForAudit = config.SourceFormId;
                        return result;
                    }

                    if (sourceSubmission.FormBuilderId != config.SourceFormId)
                    {
                        result.ErrorMessage = $"Source submission {actualSourceSubmissionId} form {sourceSubmission.FormBuilderId} does not match configured SourceFormId {config.SourceFormId}";
                        sourceFormIdForAudit = config.SourceFormId;
                        return result;
                    }
                }

                sourceFormIdForAudit = config.SourceFormId;

                // 4. Validate target form and document type
                var targetForm = await _unitOfWork.FormBuilderRepository.SingleOrDefaultAsync(f => f.Id == config.TargetFormId && !f.IsDeleted);
                if (targetForm == null)
                {
                    result.ErrorMessage = $"Target form {config.TargetFormId} not found";
                    return result;
                }

                var targetDocumentType = await _unitOfWork.DocumentTypeRepository.GetByIdAsync(config.TargetDocumentTypeId);
                if (targetDocumentType == null)
                {
                    result.ErrorMessage = $"Target document type {config.TargetDocumentTypeId} not found";
                    return result;
                }

                // 5. Validate field mappings exist and data type compatibility
                if (config.FieldMapping != null && config.FieldMapping.Any())
                {
                    var validationError = await ValidateFieldMappingsAsync(config, sourceForm.Id, targetForm.Id);
                    if (!string.IsNullOrEmpty(validationError))
                    {
                        result.ErrorMessage = validationError;
                        return result;
                    }
                }

                // 6. Get or create target document
                if (config.CreateNewDocument)
                {
                    // Create new document
                    var (createdDocument, createError) = await CreateTargetDocumentAsync(
                        config,
                        sourceSubmission,
                        targetForm,
                        targetDocumentType,
                        executedByUserId);
                    if (createdDocument == null)
                    {
                        result.ErrorMessage = createError ?? "Failed to create target document";
                        return result;
                    }
                    targetDocument = createdDocument;
                }
                else
                {
                    var (existingDocument, getError) = await GetExistingTargetDocumentAsync(config);
                    if (existingDocument == null)
                    {
                        result.ErrorMessage = getError ?? "Failed to load target document";
                        return result;
                    }
                    targetDocument = existingDocument;
                }

                // 7. Copy field values
                if (sourceSubmission != null && config.FieldMapping != null && config.FieldMapping.Any())
                {
                    result.FieldsCopied = await CopyFieldValuesAsync(
                        sourceSubmission,
                        targetDocument,
                        config.FieldMapping,
                        config.CopyCalculatedFields,
                        config.OverrideTargetDefaults);
                }

                // 8. Copy grid data
                if (sourceSubmission != null && config.CopyGridRows && config.GridMapping != null && config.GridMapping.Any())
                {
                    result.GridRowsCopied = await CopyGridDataAsync(sourceSubmission, targetDocument, config.GridMapping);
                }

                // 9. Copy metadata if requested
                if (sourceSubmission != null && config.CopyMetadata && config.MetadataFields != null && config.MetadataFields.Any())
                {
                    await CopyMetadataAsync(sourceSubmission, targetDocument, config);
                }

                // 10. Copy attachments if requested
                if (sourceSubmission != null && config.CopyAttachments)
                {
                    await CopyAttachmentsAsync(sourceSubmission, targetDocument, executedByUserId);
                }

                // 11. Link documents if requested
                if (sourceSubmission != null && config.LinkDocuments)
                {
                    targetDocument.ParentDocumentId = sourceSubmission.Id;
                }

                // 12. Save changes
                await _unitOfWork.CompleteAsyn();

                // 13. Update result
                result.Success = true;
                result.TargetDocumentId = targetDocument.Id;
                result.TargetDocumentNumber = targetDocument.DocumentNumber;

                _logger?.LogInformation("CopyToDocument execution completed successfully. TargetDocumentId: {TargetDocumentId}, FieldsCopied: {FieldsCopied}, GridRowsCopied: {GridRowsCopied}",
                    targetDocument.Id, result.FieldsCopied, result.GridRowsCopied);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error executing CopyToDocument. SourceSubmissionId: {SourceSubmissionId}", actualSourceSubmissionId);
                result.ErrorMessage = $"Error executing CopyToDocument: {ex.Message}";
            }
            finally
            {
                try
                {
                    if (ownsTransaction && transaction != null)
                    {
                        if (result.Success)
                        {
                            await transaction.CommitAsync();
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                            _unitOfWork.AppDbContext.ChangeTracker.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error finalizing CopyToDocument transaction");
                    result.Success = false;
                    result.ErrorMessage = result.ErrorMessage ?? "Error finalizing CopyToDocument transaction";
                }
            }

            // Start workflow after transaction is finalized to avoid nested transaction errors
            if (result.Success && config.StartWorkflow && targetDocument != null && targetDocument.Status == "Draft")
            {
                try
                {
                    var submitDto = new SubmitFormDto
                    {
                        SubmissionId = targetDocument.Id,
                        SubmittedByUserId = executedByUserId ?? "system"
                    };
                    var submitResult = await _formSubmissionsService.SubmitAsync(submitDto);
                    if (submitResult.StatusCode >= 400)
                    {
                        result.Success = false;
                        result.ErrorMessage = submitResult.Message ?? "Failed to start workflow";
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error starting workflow for target document {TargetDocumentId}", targetDocument.Id);
                    result.Success = false;
                    result.ErrorMessage = $"Error starting workflow: {ex.Message}";
                }
            }

            // Log audit - this should happen even if transaction was rolled back
            // We use a separate try-catch to ensure audit is logged regardless of main operation result
            _logger?.LogInformation("=== About to log CopyToDocument audit ===");
            _logger?.LogInformation("Result: Success={Success}, TargetDocumentId={TargetDocumentId}, SourceSubmissionId={SourceSubmissionId}",
                result.Success, result.TargetDocumentId, result.SourceSubmissionId);
            
            if (result.SourceSubmissionId > 0)
            {
                try
                {
                    await LogAuditAsync(result, config, actionId, ruleId, executedByUserId, sourceFormIdForAudit);
                    auditLogged = true;
                    _logger?.LogInformation("=== CopyToDocument audit logged successfully ===");
                }
                catch (Exception auditEx)
                {
                    _logger?.LogError(auditEx, "=== CRITICAL: Failed to log CopyToDocument audit ===");
                    _logger?.LogError(auditEx, "SourceSubmissionId: {SourceSubmissionId}, TargetDocumentId: {TargetDocumentId}", 
                        result.SourceSubmissionId, result.TargetDocumentId);
                    _logger?.LogError(auditEx, "Exception: {ExceptionType} - {ExceptionMessage}", 
                        auditEx.GetType().Name, auditEx.Message);
                    _logger?.LogError(auditEx, "StackTrace: {StackTrace}", auditEx.StackTrace);
                    // Even if audit logging fails, we still return the result
                }
            }
            else
            {
                _logger?.LogWarning("Skipping CopyToDocument audit logging because SourceSubmissionId is not available.");
            }
            
            return result;
        }

        private async Task<(FORM_SUBMISSIONS? Document, string? ErrorMessage)> CreateTargetDocumentAsync(
            CopyToDocumentActionDto config,
            FORM_SUBMISSIONS? sourceSubmission,
            FORM_BUILDER targetForm,
            DOCUMENT_TYPES targetDocumentType,
            string? executedByUserId)
        {
            try
            {
                // Resolve ProjectId from source submission's series (if available), otherwise fall back to target document series.
                int projectId = 0;
                if (sourceSubmission != null)
                {
                    if (sourceSubmission.DOCUMENT_SERIES != null)
                    {
                        projectId = sourceSubmission.DOCUMENT_SERIES.ProjectId;
                    }
                    else if (sourceSubmission.SeriesId > 0)
                    {
                        // Load series if not loaded
                        var sourceSeries = await _unitOfWork.DocumentSeriesRepository.GetByIdAsync(sourceSubmission.SeriesId);
                        if (sourceSeries != null)
                        {
                            projectId = sourceSeries.ProjectId;
                        }
                    }

                    if (projectId <= 0)
                    {
                        var errorMsg = $"Cannot determine ProjectId from source submission {sourceSubmission.Id}. SeriesId: {sourceSubmission.SeriesId}";
                        _logger?.LogError(errorMsg);
                        return (null, errorMsg);
                    }
                }
                else
                {
                    var seriesCandidates = await _unitOfWork.DocumentSeriesRepository.GetByDocumentTypeIdAsync(targetDocumentType.Id);
                    var selectedSeries = seriesCandidates
                        .OrderByDescending(s => s.IsDefault)
                        .ThenBy(s => s.SeriesCode)
                        .FirstOrDefault();

                    if (selectedSeries == null)
                    {
                        var errorMsg = $"No active document series found for target document type {targetDocumentType.Id} (Name: {targetDocumentType.Name}). Please configure Document Series first.";
                        _logger?.LogError(errorMsg);
                        return (null, errorMsg);
                    }

                    projectId = selectedSeries.ProjectId;
                }

                // Get default series for target document type
                var series = await _unitOfWork.DocumentSeriesRepository.SelectSeriesForSubmissionAsync(
                    targetDocumentType.Id, 
                    projectId);

                if (series == null)
                {
                    var errorMsg = $"No active document series found for document type {targetDocumentType.Id} (Name: {targetDocumentType.Name}) and project {projectId}. Please configure Document Series first.";
                    _logger?.LogError(errorMsg);
                    return (null, errorMsg);
                }

                // Generate document number with retry logic to avoid duplicates
                const int maxRetries = 10;
                int attempts = 0;
                FORM_SUBMISSIONS? targetDocument = null;

                while (attempts < maxRetries && targetDocument == null)
                {
                    try
                    {
                        // Generate document number
                        var nextNumber = await _unitOfWork.DocumentSeriesRepository.GetNextNumberAsync(series.Id);
                        var documentNumber = $"{series.SeriesCode}-{nextNumber:D6}";

                        // Check if document number already exists (double-check before insert)
                        var exists = await _unitOfWork.FormSubmissionsRepository.DocumentNumberExistsAsync(documentNumber);
                        if (exists)
                        {
                            attempts++;
                        if (attempts >= maxRetries)
                        {
                            var errorMsg = $"Failed to generate unique document number after {maxRetries} attempts";
                            _logger?.LogError(errorMsg);
                            return (null, errorMsg);
                        }
                            await Task.Delay(50 * attempts);
                            continue;
                        }

                        // Determine initial status (Draft or Submitted)
                        var initialStatus = config.InitialStatus?.Trim();
                        if (string.IsNullOrWhiteSpace(initialStatus) || 
                            (initialStatus != "Draft" && initialStatus != "Submitted"))
                        {
                            initialStatus = "Draft"; // Default to Draft if invalid
                        }

                        // Create new submission
                        targetDocument = new FORM_SUBMISSIONS
                        {
                            FormBuilderId = config.TargetFormId,
                            DocumentTypeId = config.TargetDocumentTypeId,
                            SeriesId = series.Id,
                            DocumentNumber = documentNumber,
                            Version = 1,
                            Status = initialStatus,
                            SubmittedByUserId = executedByUserId ?? sourceSubmission?.SubmittedByUserId ?? "system",
                            SubmittedDate = initialStatus == "Submitted" ? DateTime.UtcNow : default(DateTime),
                            CreatedDate = DateTime.UtcNow,
                            UpdatedDate = DateTime.UtcNow,
                            IsActive = true,
                            IsDeleted = false,
                            CreatedByUserId = executedByUserId ?? sourceSubmission?.SubmittedByUserId ?? "system"
                        };

                        _unitOfWork.FormSubmissionsRepository.Add(targetDocument);
                        await _unitOfWork.CompleteAsyn();

                        _logger?.LogInformation("Target document created successfully. DocumentNumber: {DocumentNumber}, Id: {Id}", 
                            documentNumber, targetDocument.Id);

                        return (targetDocument, null);
                    }
                    catch (Exception ex)
                    {
                        attempts++;
                        if (attempts >= maxRetries)
                        {
                            _logger?.LogError(ex, "Failed to create target document after {MaxRetries} attempts", maxRetries);
                            throw;
                        }
                        await Task.Delay(50 * attempts);
                    }
                }

                return (null, "Failed to create target document after retries");
            }
            catch (Exception ex)
            {
                var errorMsg = $"Error creating target document: {ex.Message}";
                _logger?.LogError(ex, errorMsg);
                return (null, errorMsg);
            }
        }

        private async Task<(FORM_SUBMISSIONS? Document, string? ErrorMessage)> GetExistingTargetDocumentAsync(
            CopyToDocumentActionDto config)
        {
            if (!config.TargetDocumentId.HasValue)
                return (null, "TargetDocumentId is required when CreateNewDocument is false");

            var targetDocument = await _unitOfWork.FormSubmissionsRepository.GetByIdWithDetailsAsync(config.TargetDocumentId.Value);
            if (targetDocument == null)
                return (null, $"Target document {config.TargetDocumentId.Value} not found");

            if (targetDocument.IsDeleted)
                return (null, $"Target document {config.TargetDocumentId.Value} is deleted");

            if (targetDocument.FormBuilderId != config.TargetFormId)
                return (null, $"Target document {config.TargetDocumentId.Value} does not match target form {config.TargetFormId}");

            if (targetDocument.DocumentTypeId != config.TargetDocumentTypeId)
                return (null, $"Target document {config.TargetDocumentId.Value} does not match target document type {config.TargetDocumentTypeId}");

            return (targetDocument, null);
        }

        private async Task<string?> ValidateFieldMappingsAsync(
            CopyToDocumentActionDto config,
            int sourceFormId,
            int targetFormId)
        {
            try
            {
                // Load source form fields
                var sourceFields = await _unitOfWork.FormFieldRepository.GetFieldsByFormIdAsync(sourceFormId);
                var sourceFieldsDict = sourceFields
                    .Where(f => !f.IsDeleted)
                    .ToDictionary(f => f.FieldCode.ToUpperInvariant(), f => f, StringComparer.OrdinalIgnoreCase);

                // Load target form fields
                var targetFields = await _unitOfWork.FormFieldRepository.GetFieldsByFormIdAsync(targetFormId);
                var targetFieldsDict = targetFields
                    .Where(f => !f.IsDeleted)
                    .ToDictionary(f => f.FieldCode.ToUpperInvariant(), f => f, StringComparer.OrdinalIgnoreCase);

                foreach (var mapping in config.FieldMapping)
                {
                    var sourceFieldCode = mapping.Key.ToUpperInvariant();
                    var targetFieldCode = mapping.Value.ToUpperInvariant();

                    // Check if source field exists
                    if (!sourceFieldsDict.TryGetValue(sourceFieldCode, out var sourceField))
                    {
                        return $"Source field '{mapping.Key}' (FieldCode: {sourceFieldCode}) not found in source form {sourceFormId}";
                    }

                    // Check if target field exists
                    if (!targetFieldsDict.TryGetValue(targetFieldCode, out var targetField))
                    {
                        return $"Target field '{mapping.Value}' (FieldCode: {targetFieldCode}) not found in target form {targetFormId}";
                    }

                    // Check data type compatibility
                    if (!AreFieldTypesCompatible(sourceField, targetField))
                    {
                        var sourceTypeName = sourceField.FIELD_TYPES?.TypeName ?? "Unknown";
                        var targetTypeName = targetField.FIELD_TYPES?.TypeName ?? "Unknown";
                        return $"Data type mismatch: Source field '{mapping.Key}' (Type: {sourceTypeName}) cannot be copied to target field '{mapping.Value}' (Type: {targetTypeName})";
                    }
                }

                return null; // Validation passed
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error validating field mappings");
                return $"Error validating field mappings: {ex.Message}";
            }
        }

        private static bool AreFieldTypesCompatible(FORM_FIELDS sourceField, FORM_FIELDS targetField)
        {
            // Get field types from navigation property (normalize to uppercase for comparison)
            var sourceType = sourceField.FIELD_TYPES?.TypeName?.Trim().ToUpperInvariant() ?? "";
            var targetType = targetField.FIELD_TYPES?.TypeName?.Trim().ToUpperInvariant() ?? "";

            // Exact match
            if (sourceType == targetType)
                return true;

            // Compatible type mappings
            var compatibleTypes = new Dictionary<string, HashSet<string>>
            {
                { "TEXT", new HashSet<string> { "TEXT", "TEXTAREA", "RICH_TEXT", "EMAIL", "PHONE", "URL" } },
                { "NUMBER", new HashSet<string> { "NUMBER", "DECIMAL", "CURRENCY", "PERCENTAGE" } },
                { "DECIMAL", new HashSet<string> { "NUMBER", "DECIMAL", "CURRENCY", "PERCENTAGE" } },
                { "CURRENCY", new HashSet<string> { "NUMBER", "DECIMAL", "CURRENCY", "PERCENTAGE" } },
                { "DATE", new HashSet<string> { "DATE", "DATETIME" } },
                { "DATETIME", new HashSet<string> { "DATE", "DATETIME" } },
                { "BOOLEAN", new HashSet<string> { "BOOLEAN", "CHECKBOX" } },
                { "CHECKBOX", new HashSet<string> { "BOOLEAN", "CHECKBOX" } }
            };

            // Check if source type can be mapped to target type
            if (compatibleTypes.TryGetValue(sourceType, out var compatibleSet))
            {
                return compatibleSet.Contains(targetType);
            }

            // If no compatibility mapping found, only exact match is allowed
            return false;
        }

        private async Task<int> CopyFieldValuesAsync(
            FORM_SUBMISSIONS sourceSubmission,
            FORM_SUBMISSIONS targetDocument,
            Dictionary<string, string> fieldMapping,
            bool copyCalculatedFields,
            bool overrideTargetDefaults)
        {
            int fieldsCopied = 0;

            try
            {
                // Load source field values
                var sourceValues = await _unitOfWork.FormSubmissionValuesRepository.GetBySubmissionIdAsync(sourceSubmission.Id);
                var sourceValuesDict = sourceValues
                    .Where(v => !v.IsDeleted && !string.IsNullOrEmpty(v.FieldCode))
                    .ToDictionary(v => v.FieldCode.ToUpperInvariant(), v => v, StringComparer.OrdinalIgnoreCase);

                // Load source form fields (for calculated field detection)
                var sourceFields = await _unitOfWork.FormFieldRepository.GetFieldsByFormIdAsync(sourceSubmission.FormBuilderId);
                var sourceFieldsDict = sourceFields
                    .Where(f => !f.IsDeleted)
                    .ToDictionary(f => f.FieldCode.ToUpperInvariant(), f => f, StringComparer.OrdinalIgnoreCase);

                // Load target form fields
                var targetFields = await _unitOfWork.FormFieldRepository.GetFieldsByFormIdAsync(targetDocument.FormBuilderId);
                var targetFieldsDict = targetFields
                    .Where(f => !f.IsDeleted)
                    .ToDictionary(f => f.FieldCode.ToUpperInvariant(), f => f, StringComparer.OrdinalIgnoreCase);

                foreach (var mapping in fieldMapping)
                {
                    var sourceFieldCode = mapping.Key.ToUpperInvariant();
                    var targetFieldCode = mapping.Value.ToUpperInvariant();

                    if (!copyCalculatedFields)
                    {
                        if (sourceFieldsDict.TryGetValue(sourceFieldCode, out var sourceField) && IsCalculatedField(sourceField))
                        {
                            _logger?.LogInformation("Skipping calculated source field '{SourceFieldCode}'", sourceFieldCode);
                            continue;
                        }
                    }

                    // Find source value
                    if (!sourceValuesDict.TryGetValue(sourceFieldCode, out var sourceValue))
                    {
                        _logger?.LogWarning("Source field '{SourceFieldCode}' not found in submission {SubmissionId}", sourceFieldCode, sourceSubmission.Id);
                        continue;
                    }

                    // Find target field
                    if (!targetFieldsDict.TryGetValue(targetFieldCode, out var targetField))
                    {
                        _logger?.LogWarning("Target field '{TargetFieldCode}' not found in form {FormId}", targetFieldCode, targetDocument.FormBuilderId);
                        continue;
                    }

                    if (!copyCalculatedFields && IsCalculatedField(targetField))
                    {
                        _logger?.LogInformation("Skipping calculated target field '{TargetFieldCode}'", targetFieldCode);
                        continue;
                    }

                    // Check if target value already exists
                    var existingValue = await _unitOfWork.FormSubmissionValuesRepository
                        .GetBySubmissionAndFieldAsync(targetDocument.Id, targetField.Id);

                    if (existingValue != null)
                    {
                        if (overrideTargetDefaults)
                        {
                            // Update existing value only if override is true
                            CopyFieldValue(sourceValue, existingValue);
                            _unitOfWork.FormSubmissionValuesRepository.Update(existingValue);
                            fieldsCopied++;
                        }
                    }
                    else
                    {
                        // Create new value
                        var newValue = new FORM_SUBMISSION_VALUES
                        {
                            SubmissionId = targetDocument.Id,
                            FieldId = targetField.Id,
                            FieldCode = targetField.FieldCode,
                            ValueString = sourceValue.ValueString,
                            ValueNumber = sourceValue.ValueNumber,
                            ValueDate = sourceValue.ValueDate,
                            ValueBool = sourceValue.ValueBool,
                            ValueJson = sourceValue.ValueJson,
                            CreatedDate = DateTime.UtcNow,
                            UpdatedDate = DateTime.UtcNow,
                            IsActive = true,
                            IsDeleted = false,
                            CreatedByUserId = sourceValue.CreatedByUserId
                        };
                        _unitOfWork.FormSubmissionValuesRepository.Add(newValue);
                        fieldsCopied++;
                    }
                }

                await _unitOfWork.CompleteAsyn();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error copying field values");
                throw;
            }

            return fieldsCopied;
        }

        private static bool IsCalculatedField(FORM_FIELDS field)
        {
            return !string.IsNullOrWhiteSpace(field.ExpressionText);
        }

        private void CopyFieldValue(FORM_SUBMISSION_VALUES source, FORM_SUBMISSION_VALUES target)
        {
            target.ValueString = source.ValueString;
            target.ValueNumber = source.ValueNumber;
            target.ValueDate = source.ValueDate;
            target.ValueBool = source.ValueBool;
            target.ValueJson = source.ValueJson;
            target.UpdatedDate = DateTime.UtcNow;
        }

        private async Task<int> CopyGridDataAsync(
            FORM_SUBMISSIONS sourceSubmission,
            FORM_SUBMISSIONS targetDocument,
            Dictionary<string, string> gridMapping)
        {
            int rowsCopied = 0;

            try
            {
                // Load source grid rows
                var sourceGridRows = await _unitOfWork.FormSubmissionGridRowRepository.GetBySubmissionIdAsync(sourceSubmission.Id);
                var sourceGridRowsByCode = sourceGridRows
                    .Where(r => !r.IsDeleted)
                    .GroupBy(r => r.FORM_GRIDS?.GridCode?.ToUpperInvariant())
                    .ToDictionary(g => g.Key ?? "", g => g.ToList(), StringComparer.OrdinalIgnoreCase);

                // Load target form grids
                var targetGrids = await _unitOfWork.FormGridRepository.GetByFormBuilderIdAsync(targetDocument.FormBuilderId);
                var targetGridsDict = targetGrids
                    .Where(g => !g.IsDeleted)
                    .ToDictionary(g => g.GridCode.ToUpperInvariant(), g => g, StringComparer.OrdinalIgnoreCase);

                foreach (var mapping in gridMapping)
                {
                    var sourceGridCode = mapping.Key.ToUpperInvariant();
                    var targetGridCode = mapping.Value.ToUpperInvariant();

                    // Find source grid rows
                    if (!sourceGridRowsByCode.TryGetValue(sourceGridCode, out var sourceRows))
                    {
                        _logger?.LogWarning("Source grid '{SourceGridCode}' not found in submission {SubmissionId}", sourceGridCode, sourceSubmission.Id);
                        continue;
                    }

                    // Find target grid
                    if (!targetGridsDict.TryGetValue(targetGridCode, out var targetGrid))
                    {
                        _logger?.LogWarning("Target grid '{TargetGridCode}' not found in form {FormId}", targetGridCode, targetDocument.FormBuilderId);
                        continue;
                    }

                    // Copy rows
                    foreach (var sourceRow in sourceRows)
                    {
                        var newRow = new FORM_SUBMISSION_GRID_ROWS
                        {
                            SubmissionId = targetDocument.Id,
                            GridId = targetGrid.Id,
                            RowIndex = sourceRow.RowIndex,
                            CreatedDate = DateTime.UtcNow,
                            UpdatedDate = DateTime.UtcNow,
                            IsActive = true,
                            IsDeleted = false,
                            CreatedByUserId = sourceRow.CreatedByUserId
                        };

                        _unitOfWork.FormSubmissionGridRowRepository.Add(newRow);
                        await _unitOfWork.CompleteAsyn();

                        // Copy cells
                        var sourceCells = await _unitOfWork.FormSubmissionGridCellRepository.GetByRowIdAsync(sourceRow.Id);
                        var targetColumns = await _unitOfWork.FormGridColumnRepository.GetByGridIdAsync(targetGrid.Id);
                        var targetColumnsDict = targetColumns
                            .Where(c => !c.IsDeleted)
                            .ToDictionary(c => c.ColumnCode.ToUpperInvariant(), c => c, StringComparer.OrdinalIgnoreCase);

                        foreach (var sourceCell in sourceCells.Where(c => !c.IsDeleted))
                        {
                            var columnCode = sourceCell.FORM_GRID_COLUMNS?.ColumnCode?.ToUpperInvariant();
                            if (string.IsNullOrEmpty(columnCode))
                                continue;

                            // Try to find matching column in target grid by code
                            if (targetColumnsDict.TryGetValue(columnCode, out var targetColumn))
                            {
                                var newCell = new FORM_SUBMISSION_GRID_CELLS
                                {
                                    RowId = newRow.Id,
                                    ColumnId = targetColumn.Id,
                                    ValueString = sourceCell.ValueString,
                                    ValueNumber = sourceCell.ValueNumber,
                                    ValueDate = sourceCell.ValueDate,
                                    ValueBool = sourceCell.ValueBool,
                                    ValueJson = sourceCell.ValueJson,
                                    CreatedDate = DateTime.UtcNow,
                                    UpdatedDate = DateTime.UtcNow,
                                    IsActive = true,
                                    IsDeleted = false,
                                    CreatedByUserId = sourceCell.CreatedByUserId
                                };

                                _unitOfWork.FormSubmissionGridCellRepository.Add(newCell);
                            }
                        }

                        rowsCopied++;
                    }
                }

                await _unitOfWork.CompleteAsyn();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error copying grid data");
                throw;
            }

            return rowsCopied;
        }

        private async Task CopyAttachmentsAsync(
            FORM_SUBMISSIONS sourceSubmission,
            FORM_SUBMISSIONS targetDocument,
            string? executedByUserId)
        {
            try
            {
                var sourceAttachments = await _unitOfWork.FormSubmissionAttachmentsRepository.GetBySubmissionIdAsync(sourceSubmission.Id);
                
                foreach (var attachment in sourceAttachments.Where(a => !a.IsDeleted))
                {
                   try
                    {
                        // Check if physical file exists
                        if (!await _fileStorageService.FileExistsAsync(attachment.FilePath))
                        {
                            _logger?.LogWarning("Source attachment file not found: {FilePath}", attachment.FilePath);
                            continue;
                        }

                        // Read source file
                        using var fileStream = await _fileStorageService.GetFileAsync(attachment.FilePath);
                        if (fileStream == null)
                            continue;

                        // Create new file path for target (using target document number/id for organization if possible, or just unique name)
                        // Assuming SaveFileAsync handles unique naming or we pass a subfolder
                        var subFolder = $"Attachments/{targetDocument.DocumentNumber}";
                        var newFilePath = await _fileStorageService.SaveFileAsync(fileStream, attachment.FileName, subFolder);

                        // Create attachment record
                        var newAttachment = new FORM_SUBMISSION_ATTACHMENTS
                        {
                            SubmissionId = targetDocument.Id,
                            FieldId = attachment.FieldId, // Assuming same field ID schema or relying on field definition consistency
                            FileName = attachment.FileName,
                            FilePath = newFilePath,
                            FileSize = attachment.FileSize,
                            ContentType = attachment.ContentType,
                            UploadedDate = DateTime.UtcNow,
                            CreatedDate = DateTime.UtcNow,
                            UpdatedDate = DateTime.UtcNow,
                            IsActive = true,
                            IsDeleted = false,
                            CreatedByUserId = executedByUserId ?? "system"
                        };

                        _unitOfWork.FormSubmissionAttachmentsRepository.Add(newAttachment);
                    }
                    catch (Exception ex)
                    {
                         _logger?.LogError(ex, "Failed to copy attachment {AttachmentId}", attachment.Id);
                    }
                }
                
                await _unitOfWork.CompleteAsyn();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error copying attachments for submission {SourceSubmissionId}", sourceSubmission.Id);
            }
        }

        private Task CopyMetadataAsync(
            FORM_SUBMISSIONS sourceSubmission,
            FORM_SUBMISSIONS targetDocument,
            CopyToDocumentActionDto config)
        {
            foreach (var field in config.MetadataFields)
            {
                if (string.IsNullOrWhiteSpace(field))
                    continue;

                var normalizedField = field.Trim().ToUpperInvariant();
                switch (normalizedField)
                {
                    case "SUBMITTEDDATE":
                        targetDocument.SubmittedDate = sourceSubmission.SubmittedDate;
                        break;
                    case "SUBMITTEDBYUSERID":
                        targetDocument.SubmittedByUserId = sourceSubmission.SubmittedByUserId;
                        break;
                    case "STATUS":
                        targetDocument.Status = sourceSubmission.Status;
                        break;
                    case "STAGEID":
                        targetDocument.StageId = sourceSubmission.StageId;
                        break;
                    case "DOCUMENTNUMBER":
                        if (config.CreateNewDocument)
                        {
                            _logger?.LogInformation("Skipping DocumentNumber metadata copy for new documents to preserve series numbering");
                        }
                        else
                        {
                            targetDocument.DocumentNumber = sourceSubmission.DocumentNumber;
                        }
                        break;
                    case "SERIESID":
                        if (config.CreateNewDocument)
                        {
                            _logger?.LogInformation("Skipping SeriesId metadata copy for new documents to preserve series selection");
                        }
                        else
                        {
                            targetDocument.SeriesId = sourceSubmission.SeriesId;
                        }
                        break;
                    case "VERSION":
                        if (config.CreateNewDocument)
                        {
                            _logger?.LogInformation("Skipping Version metadata copy for new documents");
                        }
                        else
                        {
                            targetDocument.Version = sourceSubmission.Version;
                        }
                        break;
                    default:
                        _logger?.LogWarning("Unsupported metadata field '{MetadataField}' requested for CopyToDocument", field);
                        break;
                }
            }

            targetDocument.UpdatedDate = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        private async Task LogAuditAsync(
            CopyToDocumentResultDto result,
            CopyToDocumentActionDto config,
            int? actionId,
            int? ruleId,
            string? executedByUserId,
            int? sourceFormId = null)
        {
            _logger?.LogInformation("=== STARTING CopyToDocument AUDIT LOGGING ===");
            _logger?.LogInformation("TargetDocumentId: {TargetDocumentId}, Success: {Success}, ErrorMessage: {ErrorMessage}",
                result.TargetDocumentId, result.Success, result.ErrorMessage);
            
            try
            {

                // Use provided sourceFormId or get from config, or fetch from source submission
                int finalSourceFormId = sourceFormId ?? config.SourceFormId;
                if (finalSourceFormId == 0 && result.SourceSubmissionId > 0)
                {
                    var sourceSubmission = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(result.SourceSubmissionId);
                    if (sourceSubmission != null)
                    {
                        finalSourceFormId = sourceSubmission.FormBuilderId;
                        _logger?.LogInformation("Retrieved SourceFormId from submission: {SourceFormId}", finalSourceFormId);
                    }
                    else
                    {
                        _logger?.LogWarning("Source submission {SourceSubmissionId} not found for audit logging", result.SourceSubmissionId);
                    }
                }

                if (finalSourceFormId == 0)
                {
                    _logger?.LogWarning("SourceFormId is 0, using config.SourceFormId: {SourceFormId}", config.SourceFormId);
                    finalSourceFormId = config.SourceFormId;
                }

                var audit = new COPY_TO_DOCUMENT_AUDIT
                {
                    TargetDocumentId = result.TargetDocumentId,
                    ActionId = actionId,
                    RuleId = ruleId,
                    SourceFormId = finalSourceFormId,
                    TargetFormId = config.TargetFormId,
                    TargetDocumentTypeId = config.TargetDocumentTypeId,
                    Success = result.Success,
                    ErrorMessage = result.ErrorMessage,
                    FieldsCopied = result.FieldsCopied,
                    GridRowsCopied = result.GridRowsCopied,
                    TargetDocumentNumber = result.TargetDocumentNumber,
                    ExecutionDate = DateTime.UtcNow,
                    CreatedDate = DateTime.UtcNow,
                    CreatedByUserId = executedByUserId ?? "system",
                    IsActive = true,
                    IsDeleted = false
                };

                _logger?.LogInformation("Created audit entity. TargetDocumentId: {TargetDocumentId}, SourceFormId: {SourceFormId}, TargetFormId: {TargetFormId}",
                    audit.TargetDocumentId, audit.SourceFormId, audit.TargetFormId);

                // Try multiple methods to save audit record
                bool auditSaved = false;
                Exception? lastException = null;

                // Method 1: Try using current DbContext with a new transaction
                try
                {
                    _logger?.LogInformation("Attempting to save audit using current DbContext with new transaction");
                    
                    var dbSet = _unitOfWork.AppDbContext.Set<COPY_TO_DOCUMENT_AUDIT>();
                    var database = _unitOfWork.AppDbContext.Database;
                    
                    // Start a new transaction
                    using var auditTransaction = await database.BeginTransactionAsync();
                    _logger?.LogInformation("Created new transaction for audit logging (Method 1)");
                    
                    // Add audit entity
                    await dbSet.AddAsync(audit);
                    _logger?.LogInformation("Added audit entity to DbContext (Method 1)");

                    // Save changes
                    var savedCount = await _unitOfWork.AppDbContext.SaveChangesAsync();
                    _logger?.LogInformation("SaveChangesAsync completed (Method 1). Returned count: {SavedCount}", savedCount);

                    // Verify audit entity has an ID
                    if (audit.Id > 0)
                    {
                        _logger?.LogInformation("Audit entity has ID: {AuditId} before commit (Method 1)", audit.Id);
                    }
                    else
                    {
                        _logger?.LogWarning("Audit entity ID is still 0 after SaveChanges (Method 1). This may indicate an issue.");
                    }

                    // Commit transaction
                    await auditTransaction.CommitAsync();
                    _logger?.LogInformation("Audit transaction committed successfully (Method 1). Audit ID: {AuditId}", audit.Id);
                    
                    // Verify again after commit
                    if (audit.Id > 0)
                    {
                        _logger?.LogInformation("✓ Audit record saved successfully (Method 1) with ID: {AuditId}", audit.Id);
                        auditSaved = true;
                    }
                    else
                    {
                        _logger?.LogError("✗ Audit ID is still 0 after commit (Method 1). Will try Method 2.");
                    }
                }
                catch (Exception ex1)
                {
                    lastException = ex1;
                    _logger?.LogWarning(ex1, "Method 1 failed: {ErrorMessage}. Will try Method 2.", ex1.Message);
                }

                // Method 2: If Method 1 failed, use a new DbContext scope
                if (!auditSaved && _serviceScopeFactory != null)
                {
                    try
                    {
                        _logger?.LogInformation("Attempting to save audit using new DbContext scope (Method 2)");
                        
                        using var scope = _serviceScopeFactory.CreateScope();
                        var auditDbContext = scope.ServiceProvider.GetRequiredService<FormBuilderDbContext>();
                        var auditDbSet = auditDbContext.Set<COPY_TO_DOCUMENT_AUDIT>();
                        
                        _logger?.LogInformation("Created new DbContext scope for audit logging (Method 2)");

                        // Create a new transaction for audit
                        using var auditTransaction = await auditDbContext.Database.BeginTransactionAsync();
                        _logger?.LogInformation("Created new transaction for audit logging (Method 2)");
                        
                        // Add audit entity
                        await auditDbSet.AddAsync(audit);
                        _logger?.LogInformation("Added audit entity to new DbContext (Method 2)");

                        // Save changes
                        var savedCount = await auditDbContext.SaveChangesAsync();
                        _logger?.LogInformation("SaveChangesAsync completed (Method 2). Returned count: {SavedCount}", savedCount);

                        // Verify audit entity has an ID
                        if (audit.Id > 0)
                        {
                            _logger?.LogInformation("Audit entity has ID: {AuditId} before commit (Method 2)", audit.Id);
                        }
                        else
                        {
                            _logger?.LogWarning("Audit entity ID is still 0 after SaveChanges (Method 2). This may indicate an issue.");
                        }

                        // Commit transaction
                        await auditTransaction.CommitAsync();
                        _logger?.LogInformation("Audit transaction committed successfully (Method 2). Audit ID: {AuditId}", audit.Id);
                        
                        // Verify again after commit
                        if (audit.Id > 0)
                        {
                            _logger?.LogInformation("✓ Audit record saved successfully (Method 2) with ID: {AuditId}", audit.Id);
                            auditSaved = true;
                        }
                        else
                        {
                            _logger?.LogError("✗ Audit ID is still 0 after commit (Method 2). Audit may not have been saved.");
                        }
                    }
                    catch (Exception ex2)
                    {
                        lastException = ex2;
                        _logger?.LogError(ex2, "Method 2 also failed: {ErrorMessage}", ex2.Message);
                    }
                }

                // If both methods failed, log critical error
                if (!auditSaved)
                {
                    var errorMsg = $"CRITICAL: Failed to save audit record using both methods. Last error: {lastException?.Message}";
                    _logger?.LogError(lastException, errorMsg);
                    throw new InvalidOperationException(errorMsg, lastException);
                }

                // Verify the audit record was saved by querying the database using a fresh DbContext
                if (result.TargetDocumentId.HasValue && _serviceScopeFactory != null)
                {
                    try
                    {
                        using var verifyScope = _serviceScopeFactory.CreateScope();
                        var verifyDbContext = verifyScope.ServiceProvider.GetRequiredService<FormBuilderDbContext>();
                        
                        // Wait a bit to ensure transaction is committed
                        await Task.Delay(100);
                        
                        var verifyAudit = await verifyDbContext.Set<COPY_TO_DOCUMENT_AUDIT>()
                            .AsNoTracking()
                            .Where(a => a.TargetDocumentId == result.TargetDocumentId)
                            .OrderByDescending(a => a.ExecutionDate)
                            .FirstOrDefaultAsync();
                        
                        if (verifyAudit != null)
                        {
                            _logger?.LogInformation("VERIFIED: Audit record exists in database with ID: {AuditId}, TargetDocumentId: {TargetDocumentId}",
                                verifyAudit.Id, verifyAudit.TargetDocumentId);
                        }
                        else
                        {
                            _logger?.LogError("CRITICAL: Audit record was NOT found in database! TargetDocumentId: {TargetDocumentId}",
                                result.TargetDocumentId);
                        }
                    }
                    catch (Exception verifyEx)
                    {
                        _logger?.LogWarning(verifyEx, "Could not verify audit record due to error: {ErrorMessage}", verifyEx.Message);
                    }
                }
                else if (!result.TargetDocumentId.HasValue)
                {
                    _logger?.LogWarning("Cannot verify audit record - TargetDocumentId is null.");

                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "=== CRITICAL ERROR in CopyToDocument audit logging ===");
                _logger?.LogError(ex, "SourceSubmissionId: {SourceSubmissionId}, TargetDocumentId: {TargetDocumentId}", 
                    result.SourceSubmissionId, result.TargetDocumentId);
                _logger?.LogError(ex, "Exception Type: {ExceptionType}, Message: {ExceptionMessage}", 
                    ex.GetType().Name, ex.Message);
                _logger?.LogError(ex, "StackTrace: {StackTrace}", ex.StackTrace);
                _logger?.LogError(ex, "Inner Exception: {InnerException}", ex.InnerException?.Message);
                _logger?.LogError(ex, "=== END OF AUDIT LOGGING ERROR ===");
                
                // Re-throw the exception so it's caught by the outer catch block
                // This ensures we know if audit logging fails
                throw;
            }
            finally
            {
                _logger?.LogInformation("=== COMPLETED CopyToDocument AUDIT LOGGING ===");
            }
        }
    }
}

