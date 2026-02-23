using FormBuilder.Core.DTOS.FormRules;
using FormBuilder.Core.IServices.FormBuilder;
using FormBuilder.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using FormBuilder.Domian.Entitys.FormBuilder;
using formBuilder.Domian.Interfaces;
using Microsoft.EntityFrameworkCore;
using FormBuilder.Domain.Interfaces.Services;

namespace FormBuilder.ApiProject.Controllers.FormBuilder
{
    /// <summary>
    /// Controller for CopyToDocument action operations
    /// Handles manual execution and audit queries for CopyToDocument actions
    /// Execution Trigger:
    /// - OnFormSubmitted
    /// - OnApprovalCompleted
    /// - OnDocumentApprove
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CopyToDocumentController : ControllerBase
    {
        private readonly ICopyToDocumentService _copyToDocumentService;
        private readonly ILogger<CopyToDocumentController>? _logger;
        private readonly IunitOfwork? _unitOfWork;
        private readonly IDocumentTypeService? _documentTypeService;
        private readonly IFormBuilderService? _formBuilderService;

        public CopyToDocumentController(
            ICopyToDocumentService copyToDocumentService,
            ILogger<CopyToDocumentController>? logger = null,
            IunitOfwork? unitOfWork = null,
            IDocumentTypeService? documentTypeService = null,
            IFormBuilderService? formBuilderService = null)
        {
            _copyToDocumentService = copyToDocumentService ?? throw new ArgumentNullException(nameof(copyToDocumentService));
            _logger = logger;
            _unitOfWork = unitOfWork;
            _documentTypeService = documentTypeService;
            _formBuilderService = formBuilderService;
        }

        /// <summary>
        /// Execute CopyToDocument action manually (accepts IDs)
        /// POST /api/CopyToDocument/execute
        /// </summary>
        [HttpPost("execute")]
        [ProducesResponseType(typeof(CopyToDocumentResultDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ExecuteCopyToDocument([FromBody] ExecuteCopyToDocumentRequestDto request)
        {
            return await ExecuteCopyToDocumentInternalAsync(request, convertCodesToIds: false);
        }

        /// <summary>
        /// Execute CopyToDocument action manually (accepts Codes)
        /// POST /api/CopyToDocument/execute-by-codes
        /// </summary>
        [HttpPost("execute-by-codes")]
        [ProducesResponseType(typeof(CopyToDocumentResultDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ExecuteCopyToDocumentByCodes([FromBody] ExecuteCopyToDocumentByCodesRequestDto request)
        {
            return await ExecuteCopyToDocumentInternalAsync(request, convertCodesToIds: true);
        }

        private async Task<IActionResult> ExecuteCopyToDocumentInternalAsync(object request, bool convertCodesToIds)
        {
            // Log request for debugging
            _logger?.LogInformation("CopyToDocument execute request received. ConvertCodesToIds: {ConvertCodesToIds}, Request: {Request}", 
                convertCodesToIds, JsonSerializer.Serialize(request));

            if (request == null)
            {
                _logger?.LogWarning("CopyToDocument execute request is null");
                return BadRequest(new ApiResponse(400, "Request body is required"));
            }

            CopyToDocumentActionDto config;
            int? actionId;
            int? ruleId;

            // Convert codes to IDs if needed
            if (convertCodesToIds)
            {
                var codesRequest = request as ExecuteCopyToDocumentByCodesRequestDto;
                if (codesRequest == null)
                {
                    return BadRequest(new ApiResponse(400, "Invalid request type for execute-by-codes endpoint"));
                }

                if (_documentTypeService == null)
                {
                    return StatusCode(500, new ApiResponse(500, "DocumentTypeService is not available"));
                }

                if (_formBuilderService == null)
                {
                    return StatusCode(500, new ApiResponse(500, "FormBuilderService is not available"));
                }

                // Convert Source Document Type Code to ID
                if (string.IsNullOrWhiteSpace(codesRequest.Config?.SourceDocumentTypeCode))
                {
                    return BadRequest(new ApiResponse(400, "SourceDocumentTypeCode is required"));
                }

                var sourceDocTypeResult = await _documentTypeService.GetByCodeAsync(codesRequest.Config.SourceDocumentTypeCode);
                if (!sourceDocTypeResult.Success || sourceDocTypeResult.Data == null)
                {
                    return NotFound(new ApiResponse(404, $"Source document type with code '{codesRequest.Config.SourceDocumentTypeCode}' not found"));
                }

                // Convert Source Form Code to ID
                if (string.IsNullOrWhiteSpace(codesRequest.Config?.SourceFormCode))
                {
                    return BadRequest(new ApiResponse(400, "SourceFormCode is required"));
                }

                var sourceFormResult = await _formBuilderService.GetByCodeAsync(codesRequest.Config.SourceFormCode);
                if (!sourceFormResult.Success || sourceFormResult.Data == null)
                {
                    return NotFound(new ApiResponse(404, $"Source form with code '{codesRequest.Config.SourceFormCode}' not found"));
                }

                // Convert Target Document Type Code to ID
                if (string.IsNullOrWhiteSpace(codesRequest.Config?.TargetDocumentTypeCode))
                {
                    return BadRequest(new ApiResponse(400, "TargetDocumentTypeCode is required"));
                }

                var targetDocTypeResult = await _documentTypeService.GetByCodeAsync(codesRequest.Config.TargetDocumentTypeCode);
                if (!targetDocTypeResult.Success || targetDocTypeResult.Data == null)
                {
                    return NotFound(new ApiResponse(404, $"Target document type with code '{codesRequest.Config.TargetDocumentTypeCode}' not found"));
                }

                // Convert Target Form Code to ID
                if (string.IsNullOrWhiteSpace(codesRequest.Config?.TargetFormCode))
                {
                    return BadRequest(new ApiResponse(400, "TargetFormCode is required"));
                }

                var targetFormResult = await _formBuilderService.GetByCodeAsync(codesRequest.Config.TargetFormCode);
                if (!targetFormResult.Success || targetFormResult.Data == null)
                {
                    return NotFound(new ApiResponse(404, $"Target form with code '{codesRequest.Config.TargetFormCode}' not found"));
                }

                // Extract field codes from format like "f1 (F)" -> "f1"
                var fieldMapping = new Dictionary<string, string>();
                if (codesRequest.Config.FieldMapping != null)
                {
                    foreach (var kvp in codesRequest.Config.FieldMapping)
                    {
                        var sourceCode = ExtractFieldCode(kvp.Key);
                        var targetCode = ExtractFieldCode(kvp.Value);
                        fieldMapping[sourceCode] = targetCode;
                    }
                }

                // Build config with IDs
                config = new CopyToDocumentActionDto
                {
                    SourceDocumentTypeId = sourceDocTypeResult.Data.Id,
                    SourceFormId = sourceFormResult.Data.Id,
                    TargetDocumentTypeId = targetDocTypeResult.Data.Id,
                    TargetFormId = targetFormResult.Data.Id,
                    CreateNewDocument = codesRequest.Config.CreateNewDocument,
                    TargetDocumentId = codesRequest.Config.TargetDocumentId,
                    InitialStatus = codesRequest.Config.InitialStatus ?? "Draft",
                    FieldMapping = fieldMapping,
                    GridMapping = codesRequest.Config.GridMapping ?? new Dictionary<string, string>(),
                    CopyCalculatedFields = codesRequest.Config.CopyCalculatedFields,
                    CopyGridRows = codesRequest.Config.CopyGridRows,
                    StartWorkflow = codesRequest.Config.StartWorkflow,
                    LinkDocuments = codesRequest.Config.LinkDocuments,
                    CopyAttachments = codesRequest.Config.CopyAttachments,
                    CopyMetadata = codesRequest.Config.CopyMetadata,
                    OverrideTargetDefaults = codesRequest.Config.OverrideTargetDefaults,
                    MetadataFields = codesRequest.Config.MetadataFields ?? new List<string>()
                };

                actionId = codesRequest.ActionId;
                ruleId = codesRequest.RuleId;
            }
            else
            {
                var idsRequest = request as ExecuteCopyToDocumentRequestDto;
                if (idsRequest == null)
                {
                    return BadRequest(new ApiResponse(400, "Invalid request type"));
                }

                config = idsRequest.Config;
                actionId = idsRequest.ActionId;
                ruleId = idsRequest.RuleId;
            }

            // Manual validation for required fields
            var validationErrors = new Dictionary<string, string[]>();
            
            if (config.SourceDocumentTypeId <= 0)
            {
                validationErrors["config.sourceDocumentTypeId"] = new[] { "SourceDocumentTypeId must be greater than 0" };
            }

            if (config.SourceFormId <= 0)
            {
                validationErrors["config.sourceFormId"] = new[] { "SourceFormId must be greater than 0" };
            }

            if (config.TargetDocumentTypeId <= 0)
            {
                validationErrors["config.targetDocumentTypeId"] = new[] { "TargetDocumentTypeId must be greater than 0" };
            }

            if (config.TargetFormId <= 0)
            {
                validationErrors["config.targetFormId"] = new[] { "TargetFormId must be greater than 0" };
            }

            if (!string.IsNullOrWhiteSpace(config.InitialStatus) && 
                config.InitialStatus != "Draft" && config.InitialStatus != "Submitted")
            {
                validationErrors["config.initialStatus"] = new[] { "InitialStatus must be either 'Draft' or 'Submitted'" };
            }

            if (validationErrors.Any())
            {
                _logger?.LogWarning("CopyToDocument validation failed. Errors: {Errors}", 
                    JsonSerializer.Serialize(validationErrors));
                return BadRequest(new ApiResponse(400, "Validation failed", new 
                { 
                    errors = validationErrors,
                    message = "One or more validation errors occurred."
                }));
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>()
                    );
                
                _logger?.LogWarning("CopyToDocument ModelState validation failed. Errors: {Errors}", 
                    JsonSerializer.Serialize(errors));
                
                return BadRequest(new ApiResponse(400, "Invalid request data", new 
                { 
                    errors,
                    message = "One or more validation errors occurred."
                }));
            }

            try
            {
                // Resolve source submission automatically (latest submission for source doc type + form)
                if (_unitOfWork == null)
                    return StatusCode(500, new ApiResponse(500, "UnitOfWork not available to resolve SourceSubmissionId"));

                var effectiveSourceSubmissionId = await _unitOfWork.AppDbContext.Set<FORM_SUBMISSIONS>()
                    .AsNoTracking()
                    .Where(s =>
                        s.DocumentTypeId == config.SourceDocumentTypeId &&
                        s.FormBuilderId == config.SourceFormId &&
                        !s.IsDeleted)
                    .OrderByDescending(s => s.SubmittedDate)
                    .Select(s => s.Id)
                    .FirstOrDefaultAsync();

                if (effectiveSourceSubmissionId <= 0)
                {
                    _logger?.LogInformation(
                        "No source submission found for SourceDocumentTypeId {SourceDocumentTypeId} and SourceFormId {SourceFormId}. Proceeding without source submission.",
                        config.SourceDocumentTypeId, config.SourceFormId);
                }

                var executedByUserId = User?.FindFirstValue(ClaimTypes.Name) 
                    ?? User?.FindFirstValue(ClaimTypes.NameIdentifier) 
                    ?? "system";

                var result = await _copyToDocumentService.ExecuteCopyToDocumentAsync(
                    config,
                    effectiveSourceSubmissionId,
                    actionId,
                    ruleId,
                    executedByUserId);

                if (result.Success)
                {
                    return Ok(new ApiResponse(200, "CopyToDocument executed successfully", result));
                }
                else
                {
                    return StatusCode(500, new ApiResponse(500, result.ErrorMessage ?? "CopyToDocument execution failed", result));
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error executing CopyToDocument");
                return StatusCode(500, new ApiResponse(500, $"Error executing CopyToDocument: {ex.Message}"));
            }
        }

        /// <summary>
        /// Extract field code from format like "f1 (F)" -> "f1" or "fw (FIELD_420019)" -> "fw"
        /// </summary>
        private string ExtractFieldCode(string fieldDisplayValue)
        {
            if (string.IsNullOrWhiteSpace(fieldDisplayValue))
                return string.Empty;

            // If format is "code (name)", extract the code part
            var parts = fieldDisplayValue.Split(new[] { " (" }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 0 ? parts[0].Trim() : fieldDisplayValue.Trim();
        }

        /// <summary>
        /// Get all CopyToDocument audit records
        /// GET /api/CopyToDocument/audit
        /// </summary>
        [HttpGet("audit")]
        [ProducesResponseType(typeof(IEnumerable<CopyToDocumentAuditDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllAuditRecords(
            [FromQuery] int? targetDocumentId = null,
            [FromQuery] int? ruleId = null,
            [FromQuery] bool? success = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            try
            {
                if (_unitOfWork == null)
                    return StatusCode(500, new ApiResponse(500, "UnitOfWork not available"));

                var query = _unitOfWork.AppDbContext.Set<COPY_TO_DOCUMENT_AUDIT>()
                    .AsNoTracking()
                    .AsQueryable();

                // Apply filters
                if (targetDocumentId.HasValue)
                    query = query.Where(a => a.TargetDocumentId == targetDocumentId.Value);

                if (ruleId.HasValue)
                    query = query.Where(a => a.RuleId == ruleId.Value);

                if (success.HasValue)
                    query = query.Where(a => a.Success == success.Value);

                // Order by execution date (newest first)
                query = query.OrderByDescending(a => a.ExecutionDate);

                // Get total count
                var totalCount = await query.CountAsync();

                // Apply pagination
                var auditRecords = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(a => new CopyToDocumentAuditDto
                    {
                        Id = a.Id,
                        TargetDocumentId = a.TargetDocumentId,
                        ActionId = a.ActionId,
                        RuleId = a.RuleId,
                        SourceFormId = a.SourceFormId,
                        TargetFormId = a.TargetFormId,
                        TargetDocumentTypeId = a.TargetDocumentTypeId,
                        Success = a.Success,
                        ErrorMessage = a.ErrorMessage,
                        FieldsCopied = a.FieldsCopied,
                        GridRowsCopied = a.GridRowsCopied,
                        TargetDocumentNumber = a.TargetDocumentNumber,
                        ExecutionDate = a.ExecutionDate,
                        CreatedDate = a.CreatedDate,
                        CreatedByUserId = a.CreatedByUserId
                    })
                    .ToListAsync();

                var response = new
                {
                    data = auditRecords,
                    totalCount = totalCount,
                    page = page,
                    pageSize = pageSize,
                    totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                };

                return Ok(new ApiResponse(200, "Audit records retrieved successfully", response));
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error retrieving CopyToDocument audit records");
                return StatusCode(500, new ApiResponse(500, $"Error retrieving audit records: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get CopyToDocument audit record by ID
        /// GET /api/CopyToDocument/audit/{id}
        /// </summary>
        [HttpGet("audit/{id}")]
        [ProducesResponseType(typeof(CopyToDocumentAuditDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAuditRecordById(int id)
        {
            try
            {
                if (_unitOfWork == null)
                    return StatusCode(500, new ApiResponse(500, "UnitOfWork not available"));

                var auditRecord = await _unitOfWork.AppDbContext.Set<COPY_TO_DOCUMENT_AUDIT>()
                    .AsNoTracking()
                    .Where(a => a.Id == id)
                    .Select(a => new CopyToDocumentAuditDto
                    {
                        Id = a.Id,
                        TargetDocumentId = a.TargetDocumentId,
                        ActionId = a.ActionId,
                        RuleId = a.RuleId,
                        SourceFormId = a.SourceFormId,
                        TargetFormId = a.TargetFormId,
                        TargetDocumentTypeId = a.TargetDocumentTypeId,
                        Success = a.Success,
                        ErrorMessage = a.ErrorMessage,
                        FieldsCopied = a.FieldsCopied,
                        GridRowsCopied = a.GridRowsCopied,
                        TargetDocumentNumber = a.TargetDocumentNumber,
                        ExecutionDate = a.ExecutionDate,
                        CreatedDate = a.CreatedDate,
                        CreatedByUserId = a.CreatedByUserId
                    })
                    .FirstOrDefaultAsync();

                if (auditRecord == null)
                    return NotFound(new ApiResponse(404, $"Audit record with ID {id} not found"));

                return Ok(new ApiResponse(200, "Audit record retrieved successfully", auditRecord));
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error retrieving CopyToDocument audit record {Id}", id);
                return StatusCode(500, new ApiResponse(500, $"Error retrieving audit record: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get CopyToDocument audit records for a specific target document
        /// GET /api/CopyToDocument/audit/target/{targetDocumentId}
        /// </summary>
        [HttpGet("audit/target/{targetDocumentId}")]
        [ProducesResponseType(typeof(IEnumerable<CopyToDocumentAuditDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAuditRecordsByTargetDocumentId(int targetDocumentId)
        {
            try
            {
                if (_unitOfWork == null)
                    return StatusCode(500, new ApiResponse(500, "UnitOfWork not available"));

                var auditRecords = await _unitOfWork.AppDbContext.Set<COPY_TO_DOCUMENT_AUDIT>()
                    .AsNoTracking()
                    .Where(a => a.TargetDocumentId == targetDocumentId)
                    .OrderByDescending(a => a.ExecutionDate)
                    .Select(a => new CopyToDocumentAuditDto
                    {
                        Id = a.Id,
                        TargetDocumentId = a.TargetDocumentId,
                        ActionId = a.ActionId,
                        RuleId = a.RuleId,
                        SourceFormId = a.SourceFormId,
                        TargetFormId = a.TargetFormId,
                        TargetDocumentTypeId = a.TargetDocumentTypeId,
                        Success = a.Success,
                        ErrorMessage = a.ErrorMessage,
                        FieldsCopied = a.FieldsCopied,
                        GridRowsCopied = a.GridRowsCopied,
                        TargetDocumentNumber = a.TargetDocumentNumber,
                        ExecutionDate = a.ExecutionDate,
                        CreatedDate = a.CreatedDate,
                        CreatedByUserId = a.CreatedByUserId
                    })
                    .ToListAsync();

                return Ok(new ApiResponse(200, "Audit records retrieved successfully", auditRecords));
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error retrieving CopyToDocument audit records for target document {TargetDocumentId}", targetDocumentId);
                return StatusCode(500, new ApiResponse(500, $"Error retrieving audit records: {ex.Message}"));
            }
        }
    }

    /// <summary>
    /// Request DTO for executing CopyToDocument action (using IDs)
    /// </summary>
    public class ExecuteCopyToDocumentRequestDto
    {
        /// <summary>
        /// CopyToDocument configuration
        /// </summary>
        [Required]
        public CopyToDocumentActionDto Config { get; set; } = null!;

        /// <summary>
        /// Action ID (optional - for audit purposes)
        /// </summary>
        public int? ActionId { get; set; }

        /// <summary>
        /// Rule ID (optional - for audit purposes)
        /// </summary>
        public int? RuleId { get; set; }
    }

    /// <summary>
    /// Request DTO for executing CopyToDocument action (using Codes)
    /// </summary>
    public class ExecuteCopyToDocumentByCodesRequestDto
    {
        /// <summary>
        /// CopyToDocument configuration with codes
        /// </summary>
        [Required]
        public CopyToDocumentActionByCodesDto Config { get; set; } = null!;

        /// <summary>
        /// Action ID (optional - for audit purposes)
        /// </summary>
        public int? ActionId { get; set; }

        /// <summary>
        /// Rule ID (optional - for audit purposes)
        /// </summary>
        public int? RuleId { get; set; }
    }

    /// <summary>
    /// Configuration DTO for CopyToDocument action using codes instead of IDs
    /// </summary>
    public class CopyToDocumentActionByCodesDto
    {
        /// <summary>
        /// Source Document Type Code (required)
        /// </summary>
        [Required(ErrorMessage = "SourceDocumentTypeCode is required")]
        public string SourceDocumentTypeCode { get; set; } = string.Empty;

        /// <summary>
        /// Source Form Code (required)
        /// </summary>
        [Required(ErrorMessage = "SourceFormCode is required")]
        public string SourceFormCode { get; set; } = string.Empty;

        /// <summary>
        /// Target Document Type Code (required)
        /// </summary>
        [Required(ErrorMessage = "TargetDocumentTypeCode is required")]
        public string TargetDocumentTypeCode { get; set; } = string.Empty;

        /// <summary>
        /// Target Form Code (required)
        /// </summary>
        [Required(ErrorMessage = "TargetFormCode is required")]
        public string TargetFormCode { get; set; } = string.Empty;

        /// <summary>
        /// Create new document if true, update existing if false
        /// </summary>
        public bool CreateNewDocument { get; set; } = true;

        /// <summary>
        /// Target document ID to update when CreateNewDocument is false
        /// </summary>
        public int? TargetDocumentId { get; set; }

        /// <summary>
        /// Initial status for new target document (Draft / Submitted)
        /// Default: Draft
        /// </summary>
        public string? InitialStatus { get; set; }

        /// <summary>
        /// Field mapping: SourceFieldCode -> TargetFieldCode
        /// Example: {"f1": "fw", "TOTAL_AMOUNT": "CONTRACT_VALUE"}
        /// Note: Field codes should be extracted from format like "f1 (F)" -> "f1"
        /// </summary>
        public Dictionary<string, string>? FieldMapping { get; set; }

        /// <summary>
        /// Grid mapping: SourceGridCode -> TargetGridCode
        /// Example: {"GRID1": "GRID2"}
        /// </summary>
        public Dictionary<string, string>? GridMapping { get; set; }

        /// <summary>
        /// Copy calculated fields (Yes/No)
        /// </summary>
        public bool CopyCalculatedFields { get; set; } = true;

        /// <summary>
        /// Copy grid rows (Yes/No)
        /// </summary>
        public bool CopyGridRows { get; set; } = true;

        /// <summary>
        /// Start workflow for target document (Yes/No)
        /// </summary>
        public bool StartWorkflow { get; set; } = false;

        /// <summary>
        /// Link source and target documents (set ParentDocumentId)
        /// </summary>
        public bool LinkDocuments { get; set; } = true;

        /// <summary>
        /// Copy attachments (Yes/No)
        /// </summary>
        public bool CopyAttachments { get; set; } = false;

        /// <summary>
        /// Copy metadata (submission date, document number, etc.)
        /// </summary>
        public bool CopyMetadata { get; set; } = false;

        /// <summary>
        /// Override target default values with source values (Yes/No)
        /// If true, source values overwrite defaults. If false, defaults are preserved if source is empty.
        /// </summary>
        public bool OverrideTargetDefaults { get; set; } = false;

        /// <summary>
        /// Metadata fields to copy (if CopyMetadata = true)
        /// Example: ["DocumentNumber", "SubmittedDate", "SubmittedByUserId"]
        /// </summary>
        public List<string>? MetadataFields { get; set; }
    }

    /// <summary>
    /// DTO for CopyToDocument audit record
    /// </summary>
    public class CopyToDocumentAuditDto
    {
        public int Id { get; set; }
        public int? TargetDocumentId { get; set; }
        public int? ActionId { get; set; }
        public int? RuleId { get; set; }
        public int SourceFormId { get; set; }
        public int TargetFormId { get; set; }
        public int TargetDocumentTypeId { get; set; }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public int FieldsCopied { get; set; }
        public int GridRowsCopied { get; set; }
        public string? TargetDocumentNumber { get; set; }
        public DateTime ExecutionDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedByUserId { get; set; }
    }
}
