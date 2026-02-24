using formBuilder.Domian.Interfaces;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Core.IServices.FormBuilder;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Domian.Entitys.froms;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Services.Services.Base;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using FormBuilder.Core.Models;
using FormBuilder.API.Models;
using FormBuilder.API.DTOs;
using FormBuilder.Application.DTOs.ApprovalWorkflow;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FormBuilder.Services.Services.FormBuilder;

namespace FormBuilder.Services
{
    public class FormSubmissionsService : BaseService<FORM_SUBMISSIONS, FormSubmissionDto, CreateFormSubmissionDto, UpdateFormSubmissionDto>, IFormSubmissionsService
    {
        private readonly IunitOfwork _unitOfWork;
        private readonly IFormSubmissionGridRowService _formSubmissionGridRowService;
        private readonly IFormSubmissionValuesService _formSubmissionValuesService;
        private readonly IFormSubmissionAttachmentsService _formSubmissionAttachmentsService;
        private readonly IFormulaService _formulaService;
        private readonly IApprovalWorkflowRuntimeService _approvalWorkflowRuntimeService;
        private readonly IDocumentApprovalHistoryService _documentApprovalHistoryService;
        private readonly FormBuilder.Services.Services.Email.EmailNotificationService _emailNotificationService;
        private readonly IFormSubmissionTriggersService _triggersService;
        private readonly IDocumentNumberGeneratorService _documentNumberGenerator;
        private readonly ILogger<FormSubmissionsService>? _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IFormRuleEvaluationService? _ruleEvaluationService;
        private readonly AkhmanageItContext _identityContext;
        private readonly ISubmitSignatureFlowService _submitSignatureFlowService;
        private readonly IConfiguration _configuration;

        public FormSubmissionsService(
            IunitOfwork unitOfWork, 
            IMapper mapper,
            IFormSubmissionGridRowService formSubmissionGridRowService,
            IFormSubmissionValuesService formSubmissionValuesService,
            IFormSubmissionAttachmentsService formSubmissionAttachmentsService,
            IFormulaService formulaService,
            IApprovalWorkflowRuntimeService approvalWorkflowRuntimeService,
            IDocumentApprovalHistoryService documentApprovalHistoryService,
            FormBuilder.Services.Services.Email.EmailNotificationService emailNotificationService,
            IFormSubmissionTriggersService triggersService,
            IDocumentNumberGeneratorService documentNumberGenerator,
            IServiceScopeFactory scopeFactory,
            AkhmanageItContext identityContext,
            ISubmitSignatureFlowService submitSignatureFlowService,
            IConfiguration configuration,
            ILogger<FormSubmissionsService>? logger = null,
            IFormRuleEvaluationService? ruleEvaluationService = null) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _formSubmissionGridRowService = formSubmissionGridRowService ?? throw new ArgumentNullException(nameof(formSubmissionGridRowService));
            _formSubmissionValuesService = formSubmissionValuesService ?? throw new ArgumentNullException(nameof(formSubmissionValuesService));
            _formSubmissionAttachmentsService = formSubmissionAttachmentsService ?? throw new ArgumentNullException(nameof(formSubmissionAttachmentsService));
            _formulaService = formulaService ?? throw new ArgumentNullException(nameof(formulaService));
            _approvalWorkflowRuntimeService = approvalWorkflowRuntimeService ?? throw new ArgumentNullException(nameof(approvalWorkflowRuntimeService));
            _documentApprovalHistoryService = documentApprovalHistoryService ?? throw new ArgumentNullException(nameof(documentApprovalHistoryService));
            _emailNotificationService = emailNotificationService ?? throw new ArgumentNullException(nameof(emailNotificationService));
            _triggersService = triggersService ?? throw new ArgumentNullException(nameof(triggersService));
            _documentNumberGenerator = documentNumberGenerator ?? throw new ArgumentNullException(nameof(documentNumberGenerator));
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
            _identityContext = identityContext ?? throw new ArgumentNullException(nameof(identityContext));
            _submitSignatureFlowService = submitSignatureFlowService ?? throw new ArgumentNullException(nameof(submitSignatureFlowService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger;
            _ruleEvaluationService = ruleEvaluationService;
        }

        private void FireAndForgetEmail(Func<FormBuilder.Services.Services.Email.EmailNotificationService, Task> sendAsync, string logContext)
        {
            // IMPORTANT: Never use the request-scoped DbContext on a background thread.
            // Create a NEW DI scope so EmailNotificationService gets NEW DbContexts.
            _ = Task.Run(async () =>
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var emailService = scope.ServiceProvider.GetRequiredService<FormBuilder.Services.Services.Email.EmailNotificationService>();
                    await sendAsync(emailService);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Failed to send email ({Context})", logContext);
                }
            });
        }

        protected override IBaseRepository<FORM_SUBMISSIONS> Repository => _unitOfWork.FormSubmissionsRepository;

        public async Task<ApiResponse> GetAllAsync()
        {
            var submissions = await _unitOfWork.FormSubmissionsRepository.GetSubmissionsWithDetailsAsync();
            var submissionDtos = _mapper.Map<IEnumerable<FormSubmissionDto>>(submissions);
            return new ApiResponse(200, "All form submissions retrieved successfully", submissionDtos);
        }

        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            var submission = await _unitOfWork.FormSubmissionsRepository.GetByIdWithDetailsAsync(id);
            if (submission == null)
                return new ApiResponse(404, "Form submission not found");

            var submissionDto = _mapper.Map<FormSubmissionDetailDto>(submission);
            
            // Map nested collections manually (AutoMapper profile ignores them)
            if (submission.FORM_SUBMISSION_VALUES != null && submission.FORM_SUBMISSION_VALUES.Any())
            {
                submissionDto.FieldValues = _mapper.Map<List<FormSubmissionValueDto>>(submission.FORM_SUBMISSION_VALUES);
            }
            if (submission.FORM_SUBMISSION_ATTACHMENTS != null && submission.FORM_SUBMISSION_ATTACHMENTS.Any())
            {
                submissionDto.Attachments = _mapper.Map<List<FormSubmissionAttachmentDto>>(submission.FORM_SUBMISSION_ATTACHMENTS);
            }
            if (submission.FORM_SUBMISSION_GRID_ROWS != null && submission.FORM_SUBMISSION_GRID_ROWS.Any())
            {
                // Load cells separately if not already loaded (due to AsNoTracking)
                var rowIds = submission.FORM_SUBMISSION_GRID_ROWS.Where(r => !r.IsDeleted).Select(r => r.Id).ToList();
                var cellsDict = new Dictionary<int, List<FormBuilder.Domian.Entitys.FormBuilder.FORM_SUBMISSION_GRID_CELLS>>();
                
                if (rowIds.Any())
                {
                    // Load cells directly from DbContext for all rows at once
                    // Note: We don't Include FORM_GRIDS via FORM_GRID_COLUMNS to avoid tracking conflicts
                    // FORM_GRIDS is already loaded via FORM_SUBMISSION_GRID_ROWS in the repository
                    // Using IgnoreAutoIncludes() to prevent automatic loading of FORM_GRIDS navigation property
                    var allCells = await _unitOfWork.AppDbContext
                        .Set<FormBuilder.Domian.Entitys.FormBuilder.FORM_SUBMISSION_GRID_CELLS>()
                        .IgnoreAutoIncludes()
                        .Include(c => c.FORM_GRID_COLUMNS)
                        .AsNoTracking()
                        .Where(c => rowIds.Contains(c.RowId) && !c.IsDeleted)
                        .ToListAsync();
                    
                    cellsDict = allCells
                        .GroupBy(c => c.RowId)
                        .ToDictionary(g => g.Key, g => g.OrderBy(c => c.ColumnId).ToList());
                }

                // Map grid rows with cells (filter deleted rows and cells)
                submissionDto.GridData = submission.FORM_SUBMISSION_GRID_ROWS
                    .Where(row => !row.IsDeleted)
                    .OrderBy(row => row.RowIndex)
                    .Select(row => 
                    {
                        // Use cells from dictionary if navigation property is empty
                        List<FormBuilder.Domian.Entitys.FormBuilder.FORM_SUBMISSION_GRID_CELLS> cells;
                        if (row.FORM_SUBMISSION_GRID_CELLS != null && row.FORM_SUBMISSION_GRID_CELLS.Any())
                        {
                            cells = row.FORM_SUBMISSION_GRID_CELLS.Where(c => !c.IsDeleted).ToList();
                        }
                        else
                        {
                            cells = cellsDict.ContainsKey(row.Id) ? cellsDict[row.Id] : new List<FormBuilder.Domian.Entitys.FormBuilder.FORM_SUBMISSION_GRID_CELLS>();
                        }

                        return new FormSubmissionGridDto
                        {
                            Id = row.Id,
                            SubmissionId = row.SubmissionId,
                            GridId = row.GridId,
                            GridName = row.FORM_GRIDS?.GridName ?? string.Empty,
                            GridCode = row.FORM_GRIDS?.GridCode ?? string.Empty,
                            RowIndex = row.RowIndex,
                            Cells = cells
                                .OrderBy(c => c.ColumnId)
                                .Select(c => new FormBuilder.Core.DTOS.FormBuilder.FormSubmissionGridCellDto
                                {
                                    Id = c.Id,
                                    RowId = c.RowId,
                                    ColumnId = c.ColumnId,
                                    ColumnCode = c.FORM_GRID_COLUMNS?.ColumnCode ?? string.Empty,
                                    ColumnName = c.FORM_GRID_COLUMNS?.ColumnName ?? string.Empty,
                                    ValueString = c.ValueString,
                                    ValueNumber = c.ValueNumber,
                                    ValueDate = c.ValueDate,
                                    ValueBool = c.ValueBool,
                                    ValueJson = c.ValueJson
                                }).ToList()
                        };
                    }).ToList();
            }

            return new ApiResponse(200, "Form submission retrieved successfully", submissionDto);
        }

        public async Task<ApiResponse> GetByIdWithDetailsAsync(int id)
        {
            var submission = await _unitOfWork.FormSubmissionsRepository.GetByIdWithDetailsAsync(id);
            if (submission == null)
                return new ApiResponse(404, "Form submission not found");

            var submissionDto = _mapper.Map<FormSubmissionDetailDto>(submission);
            
            // Map nested collections if needed
            if (submission.FORM_SUBMISSION_VALUES != null)
            {
                submissionDto.FieldValues = _mapper.Map<List<FormSubmissionValueDto>>(submission.FORM_SUBMISSION_VALUES);
            }
            if (submission.FORM_SUBMISSION_ATTACHMENTS != null)
            {
                submissionDto.Attachments = _mapper.Map<List<FormSubmissionAttachmentDto>>(submission.FORM_SUBMISSION_ATTACHMENTS);
            }
            if (submission.FORM_SUBMISSION_GRID_ROWS != null)
            {
                // Load cells separately if not already loaded (due to AsNoTracking)
                var rowIds = submission.FORM_SUBMISSION_GRID_ROWS.Where(r => !r.IsDeleted).Select(r => r.Id).ToList();
                var cellsDict = new Dictionary<int, List<FormBuilder.Domian.Entitys.FormBuilder.FORM_SUBMISSION_GRID_CELLS>>();
                
                if (rowIds.Any())
                {
                    // Load cells directly from DbContext for all rows at once
                    // Note: We don't Include FORM_GRIDS via FORM_GRID_COLUMNS to avoid tracking conflicts
                    // FORM_GRIDS is already loaded via FORM_SUBMISSION_GRID_ROWS in the repository
                    // Using IgnoreAutoIncludes() to prevent automatic loading of FORM_GRIDS navigation property
                    var allCells = await _unitOfWork.AppDbContext
                        .Set<FormBuilder.Domian.Entitys.FormBuilder.FORM_SUBMISSION_GRID_CELLS>()
                        .IgnoreAutoIncludes()
                        .Include(c => c.FORM_GRID_COLUMNS)
                        .AsNoTracking()
                        .Where(c => rowIds.Contains(c.RowId) && !c.IsDeleted)
                        .ToListAsync();
                    
                    cellsDict = allCells
                        .GroupBy(c => c.RowId)
                        .ToDictionary(g => g.Key, g => g.OrderBy(c => c.ColumnId).ToList());
                }

                // Map grid rows with cells (filter deleted rows and cells)
                submissionDto.GridData = submission.FORM_SUBMISSION_GRID_ROWS
                    .Where(row => !row.IsDeleted)
                    .OrderBy(row => row.RowIndex)
                    .Select(row => 
                    {
                        // Use cells from dictionary if navigation property is empty
                        List<FormBuilder.Domian.Entitys.FormBuilder.FORM_SUBMISSION_GRID_CELLS> cells;
                        if (row.FORM_SUBMISSION_GRID_CELLS != null && row.FORM_SUBMISSION_GRID_CELLS.Any())
                        {
                            cells = row.FORM_SUBMISSION_GRID_CELLS.Where(c => !c.IsDeleted).ToList();
                        }
                        else
                        {
                            cells = cellsDict.ContainsKey(row.Id) ? cellsDict[row.Id] : new List<FormBuilder.Domian.Entitys.FormBuilder.FORM_SUBMISSION_GRID_CELLS>();
                        }

                        return new FormSubmissionGridDto
                        {
                            Id = row.Id,
                            SubmissionId = row.SubmissionId,
                            GridId = row.GridId,
                            GridName = row.FORM_GRIDS?.GridName ?? string.Empty,
                            GridCode = row.FORM_GRIDS?.GridCode ?? string.Empty,
                            RowIndex = row.RowIndex,
                            Cells = cells
                                .OrderBy(c => c.ColumnId)
                                .Select(c => new FormBuilder.Core.DTOS.FormBuilder.FormSubmissionGridCellDto
                                {
                                    Id = c.Id,
                                    RowId = c.RowId,
                                    ColumnId = c.ColumnId,
                                    ColumnCode = c.FORM_GRID_COLUMNS?.ColumnCode ?? string.Empty,
                                    ColumnName = c.FORM_GRID_COLUMNS?.ColumnName ?? string.Empty,
                                    ValueString = c.ValueString,
                                    ValueNumber = c.ValueNumber,
                                    ValueDate = c.ValueDate,
                                    ValueBool = c.ValueBool,
                                    ValueJson = c.ValueJson
                                }).ToList()
                        };
                    }).ToList();
            }

            return new ApiResponse(200, "Form submission with details retrieved successfully", submissionDto);
        }

        public async Task<ApiResponse> GetByDocumentNumberAsync(string documentNumber)
        {
            var submission = await _unitOfWork.FormSubmissionsRepository.GetByDocumentNumberAsync(documentNumber);
            if (submission == null)
                return new ApiResponse(404, "Form submission not found");

            var submissionDto = _mapper.Map<FormSubmissionDto>(submission);
            return new ApiResponse(200, "Form submission retrieved successfully", submissionDto);
        }

        public async Task<ApiResponse> GetByFormBuilderIdAsync(int formBuilderId)
        {
            var submissions = await _unitOfWork.FormSubmissionsRepository.GetByFormBuilderIdAsync(formBuilderId);
            var submissionDtos = _mapper.Map<IEnumerable<FormSubmissionDto>>(submissions);
            return new ApiResponse(200, "Form submissions retrieved successfully", submissionDtos);
        }

        public async Task<ApiResponse> GetByDocumentTypeIdAsync(int documentTypeId)
        {
            var submissions = await _unitOfWork.FormSubmissionsRepository.GetByDocumentTypeIdAsync(documentTypeId);
            var submissionDtos = _mapper.Map<IEnumerable<FormSubmissionDto>>(submissions);
            return new ApiResponse(200, "Form submissions retrieved successfully", submissionDtos);
        }

        public async Task<ApiResponse> GetByUserIdAsync(string userId)
        {
            var submissions = await _unitOfWork.FormSubmissionsRepository.GetByUserIdAsync(userId);
            var submissionDtos = _mapper.Map<IEnumerable<FormSubmissionDto>>(submissions);
            return new ApiResponse(200, "User form submissions retrieved successfully", submissionDtos);
        }

        public async Task<ApiResponse> GetByStatusAsync(string status)
        {
            var submissions = await _unitOfWork.FormSubmissionsRepository.GetByStatusAsync(status);
            var submissionDtos = _mapper.Map<IEnumerable<FormSubmissionDto>>(submissions);
            return new ApiResponse(200, "Form submissions by status retrieved successfully", submissionDtos);
        }

        /// <summary>
        /// Get draft submission by formBuilderId, projectId, and submittedByUserId
        /// Returns the most recent draft submission if found, otherwise returns 404
        /// </summary>
        public async Task<ApiResponse> GetDraftAsync(int formBuilderId, int projectId, string submittedByUserId)
        {
            // First, get all series IDs for the project to filter efficiently
            var projectSeries = await _unitOfWork.DocumentSeriesRepository.GetByProjectIdAsync(projectId);
            var seriesIds = projectSeries.Select(s => s.Id).ToList();

            if (!seriesIds.Any())
            {
                var errorData = new
                {
                    formBuilderId = formBuilderId,
                    projectId = projectId,
                    submittedByUserId = submittedByUserId,
                    message = "No Document Series found for this project. Please configure Document Series first.",
                    suggestion = "Use POST /api/FormSubmissions/draft to create a new draft, or configure Document Series using POST /api/FormBuilderDocumentSettings"
                };
                return new ApiResponse(404, 
                    "No draft submission found. No Document Series configured for Project ID " + projectId + ". " +
                    "Please configure Document Series first or use POST /api/FormSubmissions/draft to create a new draft.",
                    errorData);
            }

            // Query for draft submissions matching the criteria with SeriesIds in the project
            var drafts = await _unitOfWork.FormSubmissionsRepository.GetAllAsync(
                s => s.FormBuilderId == formBuilderId 
                    && s.Status == "Draft" 
                    && s.SubmittedByUserId == submittedByUserId
                    && seriesIds.Contains(s.SeriesId)
            );

            // Get the most recent draft (by CreatedDate descending)
            var draft = drafts.OrderByDescending(s => s.CreatedDate).FirstOrDefault();

            if (draft == null)
            {
                var errorData = new
                {
                    formBuilderId = formBuilderId,
                    projectId = projectId,
                    submittedByUserId = submittedByUserId,
                    message = "No draft submission exists for the specified criteria",
                    suggestion = "Use POST /api/FormSubmissions/draft to create a new draft submission"
                };
                return new ApiResponse(404, 
                    "No draft submission found for the specified criteria. " +
                    "Use POST /api/FormSubmissions/draft to create a new draft.",
                    errorData);
            }

            var submissionDto = _mapper.Map<FormSubmissionDto>(draft);
            return new ApiResponse(200, "Draft submission retrieved successfully", submissionDto);
        }

        /// <summary>
        /// Get existing draft submission or create a new one if none exists
        /// This is a convenience method that combines GET and POST operations
        /// </summary>
        public async Task<ApiResponse> GetOrCreateDraftAsync(int formBuilderId, int projectId, string submittedByUserId, int? seriesId = null)
        {
            // First, try to get existing draft
            var existingDraft = await GetDraftAsync(formBuilderId, projectId, submittedByUserId);
            
            // If draft exists (status 200), return it
            if (existingDraft.StatusCode == 200)
            {
                return existingDraft;
            }
            
            // If draft doesn't exist (404), create a new one
            return await CreateDraftAsync(formBuilderId, projectId, submittedByUserId, seriesId);
        }

        public async Task<ApiResponse> CreateAsync(CreateFormSubmissionDto createDto)
        {
            if (createDto == null)
                return new ApiResponse(400, "DTO is required");

            // Generate document number
            var series = await _unitOfWork.DocumentSeriesRepository.GetByIdAsync(createDto.SeriesId);
            if (series == null)
                return new ApiResponse(404, "Document series not found");

            // Retry logic to handle concurrent requests and avoid duplicate document numbers
            const int maxRetries = 10;
            int attempts = 0;
            FORM_SUBMISSIONS createdEntity = null;

            while (attempts < maxRetries)
            {
                try
                {
                    // Generate document number
                    var nextNumber = await _unitOfWork.DocumentSeriesRepository.GetNextNumberAsync(createDto.SeriesId);
                    var documentNumber = $"{series.SeriesCode}-{nextNumber:D6}";

                    // Check if document number already exists (double-check before insert)
                    var exists = await _unitOfWork.FormSubmissionsRepository.DocumentNumberExistsAsync(documentNumber);
                    if (exists)
                    {
                        attempts++;
                        if (attempts >= maxRetries)
                        {
                            return new ApiResponse(500, "Failed to generate unique document number after multiple attempts. Please try again.");
                        }
                        await Task.Delay(50 * attempts);
                        continue;
                    }

                    // Get next version
                    var version = await _unitOfWork.FormSubmissionsRepository.GetNextVersionAsync(createDto.FormBuilderId);

                    var entity = _mapper.Map<FORM_SUBMISSIONS>(createDto);
                    entity.DocumentNumber = documentNumber;
                    entity.Version = version;
                    entity.SubmittedDate = DateTime.UtcNow;
                    entity.CreatedDate = DateTime.UtcNow;
                    entity.UpdatedDate = DateTime.UtcNow;
                    entity.StageId = null; // Ensure StageId is null for new submissions
                    entity.SignatureStatus = "not_required";
                    entity.DocuSignEnvelopeId = null;
                    entity.SignedAt = null;

                    _unitOfWork.FormSubmissionsRepository.Add(entity);
                    await _unitOfWork.CompleteAsyn();

                    createdEntity = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(entity.Id);
                    break; // Success, exit retry loop
                }
                catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && 
                    (sqlEx.Number == 2601 || sqlEx.Number == 2627)) // Duplicate key error
                {
                    attempts++;
                    if (attempts >= maxRetries)
                    {
                        return new ApiResponse(500, 
                            $"Failed to create form submission due to duplicate document number conflict after {maxRetries} attempts. " +
                            "This may occur when multiple requests are processed simultaneously. Please try again.");
                    }

                    // Wait before retrying (exponential backoff)
                    // EF Core will automatically rollback the transaction on exception
                    await Task.Delay(100 * attempts);
                }
                catch (Exception ex)
                {
                    // For other exceptions, return error immediately
                    return new ApiResponse(500, $"Error creating form submission: {ex.Message}");
                }
            }

            if (createdEntity == null)
            {
                return new ApiResponse(500, "Failed to create form submission after multiple attempts.");
            }

            var submissionDto = _mapper.Map<FormSubmissionDto>(createdEntity);
            return new ApiResponse(200, "Form submission created successfully", submissionDto);
        }

        /// <summary>
        /// Create a new draft submission automatically determining Document Type and Series from FormBuilderId
        /// This method implements the runtime flow as specified in the requirements:
        /// 1. Loads the form (FormBuilderId)
        /// 2. Loads the linked Document Type from DOCUMENT_TYPES
        /// 3. Selects the correct Document Series from DOCUMENT_SERIES (default series for the project)
        ///    OR uses provided seriesId if valid
        /// 4. Creates a draft record without final document number
        /// 5. Final number is generated on Submit/Approval based on series.GenerateOn
        /// </summary>
        public async Task<ApiResponse> CreateDraftAsync(int formBuilderId, int projectId, string submittedByUserId, int? seriesId = null)
        {
            // 0. Evaluate Pre-Open Blocking Rules (before form creation)
            if (_ruleEvaluationService != null)
            {
                var blockingResult = await _ruleEvaluationService.EvaluateBlockingRulesAsync(
                    formBuilderId, 
                    "PreOpen");
                
                if (blockingResult.IsBlocked)
                {
                    return new ApiResponse(403, blockingResult.BlockMessage ?? "Form access is blocked", new
                    {
                        isBlocked = true,
                        message = blockingResult.BlockMessage,
                        ruleId = blockingResult.MatchedRuleId,
                        ruleName = blockingResult.MatchedRuleName
                    });
                }
            }

            // 1. Verify FormBuilder exists
            var formBuilder = await _unitOfWork.FormBuilderRepository.SingleOrDefaultAsync(fb => fb.Id == formBuilderId);
            if (formBuilder == null)
                return new ApiResponse(404, "Form Builder not found");

            // 2. Load the linked Document Type from DOCUMENT_TYPES (only active ones)
            var documentTypes = await _unitOfWork.DocumentTypeRepository.GetByFormBuilderIdAsync(formBuilderId);
            // Only get active DocumentType - do not allow inactive ones
            var documentType = documentTypes.FirstOrDefault(dt => dt.IsActive);
            if (documentType == null)
            {
                // Check if any DocumentType exists but is inactive
                var inactiveDocumentType = documentTypes.FirstOrDefault();
                if (inactiveDocumentType != null)
                {
                    var errorData = new
                    {
                        documentTypeId = inactiveDocumentType.Id,
                        documentTypeName = inactiveDocumentType.Name,
                        formBuilderId = formBuilderId,
                        formBuilderName = formBuilder.FormName,
                        activateEndpoint = $"/api/DocumentTypes/{inactiveDocumentType.Id}",
                        message = "Document Type must be activated before creating submissions"
                    };
                    
                    return new ApiResponse(400, 
                        $"Document Type '{inactiveDocumentType.Name}' (ID: {inactiveDocumentType.Id}) is not active. " +
                        "Please activate the Document Type in Document Types settings first.",
                        errorData);
                }
                
                // No DocumentType found at all
                var errorDataNotFound = new
                {
                    formBuilderId = formBuilderId,
                    formBuilderName = formBuilder.FormName,
                    configurationEndpoint = "/api/FormBuilderDocumentSettings",
                    checkSettingsEndpoint = $"/api/FormBuilderDocumentSettings/form/{formBuilderId}",
                    message = "Document Type must be configured before creating draft submissions"
                };
                
                return new ApiResponse(404, 
                    $"No active Document Type configured for Form Builder '{formBuilder.FormName}' (ID: {formBuilderId}). " +
                    "Please configure Document Settings using: POST /api/FormBuilderDocumentSettings. " +
                    "Check existing settings using: GET /api/FormBuilderDocumentSettings/form/{formBuilderId}",
                    errorDataNotFound);
            }

            // 3. Select or validate Document Series
            DOCUMENT_SERIES? series = null;
            
            if (seriesId.HasValue && seriesId.Value > 0)
            {
                // If seriesId is provided, validate it
                series = await _unitOfWork.DocumentSeriesRepository.GetByIdAsync(seriesId.Value);
                if (series == null)
                {
                    return new ApiResponse(404, $"Document Series with ID {seriesId.Value} not found");
                }
                
                // Validate series belongs to correct project
                if (series.ProjectId != projectId)
                {
                    return new ApiResponse(400, 
                        $"Document Series {seriesId.Value} does not belong to Project {projectId}");
                }
                
                // Validate series is active
                if (!series.IsActive)
                {
                    return new ApiResponse(400, 
                        $"Document Series {seriesId.Value} is not active. Please activate the series first.");
                }
            }
            else
            {
                // If seriesId not provided, auto-select using existing logic
                // Series Selection Logic:
                // - If only one series exists, it is selected automatically
                // - If multiple series exist: Match by Project, Select default series
                // - End users do not select series
                series = await _unitOfWork.DocumentSeriesRepository.SelectSeriesForSubmissionAsync(documentType.Id, projectId);
            }
            
            if (series == null)
            {
                var errorData = new
                {
                    formBuilderId = formBuilderId,
                    formBuilderName = formBuilder.FormName,
                    documentTypeId = documentType.Id,
                    documentTypeName = documentType.Name,
                    projectId = projectId,
                    configurationEndpoint = "/api/FormBuilderDocumentSettings",
                    documentSeriesEndpoint = "/api/DocumentSeries",
                    message = "Active Document Series must be configured for the Document Type and Project before creating draft submissions"
                };

                return new ApiResponse(404,
                    $"No active Document Series found for Document Type '{documentType.Name}' (ID: {documentType.Id}) and Project ID {projectId}. " +
                    "Please configure Document Series using: POST /api/FormBuilderDocumentSettings or POST /api/DocumentSeries. " +
                    "Ensure at least one active series exists for this Document Type and Project.",
                    errorData);
            }

            if (!series.IsActive)
                return new ApiResponse(400, "Document Series is not active");

            // 4. Do not generate document number at draft creation.
            // Final number is generated by the Document Series engine on Submit/Approval.
            var version = await _unitOfWork.FormSubmissionsRepository.GetNextVersionAsync(formBuilderId);

            var entity = new FORM_SUBMISSIONS
            {
                FormBuilderId = formBuilderId,
                Version = version,
                DocumentTypeId = documentType.Id,
                SeriesId = series.Id,
                DocumentNumber = string.Empty,
                SubmittedByUserId = submittedByUserId,
                SubmittedDate = DateTime.UtcNow,
                Status = "Draft",
                SignatureStatus = "not_required",
                DocuSignEnvelopeId = null,
                SignedAt = null,
                StageId = null,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            _unitOfWork.FormSubmissionsRepository.Add(entity);
            await _unitOfWork.CompleteAsyn();

            var createdEntity = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(entity.Id);
            if (createdEntity == null)
                return new ApiResponse(500, "Failed to create draft form submission.");

            var submissionDto = _mapper.Map<FormSubmissionDto>(createdEntity);
            return new ApiResponse(200, "Draft form submission created successfully", submissionDto);
        }

        public async Task<ApiResponse> CreateDraftAsync(CreateDraftDto createDraftDto)
        {
            if (createDraftDto == null)
                return new ApiResponse(400, "DTO is required");

            if (createDraftDto.FormBuilderId <= 0)
                return new ApiResponse(400, "FormBuilderId is required");

            int projectId = 0;

            // Derive projectId from seriesId if projectId is not provided
            if (createDraftDto.ProjectId.HasValue && createDraftDto.ProjectId.Value > 0)
            {
                projectId = createDraftDto.ProjectId.Value;
            }
            else if (createDraftDto.SeriesId.HasValue && createDraftDto.SeriesId.Value > 0)
            {
                // Get projectId from series
                var series = await _unitOfWork.DocumentSeriesRepository.GetByIdAsync(createDraftDto.SeriesId.Value);
                if (series == null)
                    return new ApiResponse(404, $"Document Series with ID {createDraftDto.SeriesId.Value} not found");
                
                projectId = series.ProjectId;
            }
            else
            {
                return new ApiResponse(400, "Either ProjectId or SeriesId must be provided");
            }

            if (projectId <= 0)
                return new ApiResponse(400, "ProjectId is required");

            // Use default value if submittedByUserId is null or empty
            var userId = string.IsNullOrWhiteSpace(createDraftDto.SubmittedByUserId) ? "public-user" : createDraftDto.SubmittedByUserId;

            // Call the existing CreateDraftAsync method
            return await CreateDraftAsync(createDraftDto.FormBuilderId, projectId, userId, createDraftDto.SeriesId);
        }

        public async Task<ApiResponse> UpdateAsync(int id, UpdateFormSubmissionDto updateDto)
        {
            if (updateDto == null)
                return new ApiResponse(400, "DTO is required");

            var entity = await _unitOfWork.FormSubmissionsRepository.SingleOrDefaultAsync(s => s.Id == id, asNoTracking: false);
            if (entity == null)
                return new ApiResponse(404, "Form submission not found");

            // ✅ TRIGGER: Document Locking Enforcement - Prevent edits when rejected or approved
            if (entity.Status == "Rejected")
            {
                return new ApiResponse(403, "Cannot update submission. Document has been rejected and is locked for editing.");
            }
            if (entity.Status == "Approved")
            {
                return new ApiResponse(403, "Cannot update submission. Document has been approved and is locked for editing.");
            }

            // Final document numbers are generated on Submit or final Approval (per series.GenerateOn)
            // and remain fixed thereafter.
            // No renumbering allowed - prevent changing document number if it's already set
            if (!string.IsNullOrEmpty(updateDto.DocumentNumber) && 
                !string.IsNullOrEmpty(entity.DocumentNumber) && 
                updateDto.DocumentNumber != entity.DocumentNumber)
            {
                return new ApiResponse(400, "Document number cannot be changed once it has been generated. Document numbers remain fixed after generation.");
            }

            // Preserve document number - don't allow it to be changed via update
            var originalDocumentNumber = entity.DocumentNumber;
            _mapper.Map(updateDto, entity);
            
            // Ensure document number is never changed
            if (!string.IsNullOrEmpty(originalDocumentNumber))
            {
                entity.DocumentNumber = originalDocumentNumber;
            }
            
            entity.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.FormSubmissionsRepository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            var updatedEntity = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(id);
            var submissionDto = _mapper.Map<FormSubmissionDto>(updatedEntity);
            return new ApiResponse(200, "Form submission updated successfully", submissionDto);
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            // Check if submission exists (and not already deleted)
            var entity = await _unitOfWork.FormSubmissionsRepository.SingleOrDefaultAsync(
                s => s.Id == id && !s.IsDeleted, asNoTracking: false);
            if (entity == null)
                return new ApiResponse(404, "Form submission not found");

            // Soft Delete child records first
            // 1. Soft delete approval history records
            await _documentApprovalHistoryService.DeleteBySubmissionIdAsync(id);
            
            // 2. Soft delete attachments
            await _formSubmissionAttachmentsService.DeleteBySubmissionIdAsync(id);
            
            // 3. Soft delete submission values
            await _formSubmissionValuesService.DeleteBySubmissionIdAsync(id);
            
            // 4. Soft delete grid rows
            await SoftDeleteGridRowsAsync(id);
            
            // 5. Soft delete the submission itself
            entity.IsDeleted = true;
            entity.DeletedDate = DateTime.UtcNow;
            entity.IsActive = false;
            entity.Status = "Deleted";
            
            _unitOfWork.FormSubmissionsRepository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            return new ApiResponse(200, "Form submission deleted successfully");
        }

        private async Task SoftDeleteGridRowsAsync(int submissionId)
        {
            // 1. Soft delete all grid cells for this submission
            await _unitOfWork.FormSubmissionGridCellRepository.DeleteBySubmissionIdAsync(submissionId);
            
            // 2. Soft delete all grid rows for this submission
            var gridRows = await _unitOfWork.FormSubmissionGridRowRepository
                .GetBySubmissionIdAsync(submissionId);
            
            foreach (var row in gridRows.Where(r => !r.IsDeleted))
            {
                row.IsDeleted = true;
                row.DeletedDate = DateTime.UtcNow;
                row.IsActive = false;
                _unitOfWork.FormSubmissionGridRowRepository.Update(row);
            }
            await _unitOfWork.CompleteAsyn();
        }

        public async Task<ApiResponse> SubmitAsync(SubmitFormDto submitDto)
        {
            var entity = await _unitOfWork.FormSubmissionsRepository.SingleOrDefaultAsync(s => s.Id == submitDto.SubmissionId && !s.IsDeleted, asNoTracking: false);
            if (entity == null)
                return new ApiResponse(404, "Form submission not found");

            var submittedByUserId = string.IsNullOrWhiteSpace(submitDto.SubmittedByUserId)
                ? (string.IsNullOrWhiteSpace(entity.SubmittedByUserId) ? "public-user" : entity.SubmittedByUserId)
                : submitDto.SubmittedByUserId;

            if (string.Equals(entity.Status, "Approved", StringComparison.OrdinalIgnoreCase))
                return new ApiResponse(400, "Form submission is already Approved");

            // If already submitted, do not fail; continue with post-submit signature flow.
            // This supports flows where status became Submitted before /api/submissions/{id}/submit is called.
            if (string.Equals(entity.Status, "Submitted", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(entity.SubmittedByUserId))
                {
                    entity.SubmittedByUserId = submittedByUserId;
                    entity.UpdatedDate = DateTime.UtcNow;
                    _unitOfWork.FormSubmissionsRepository.Update(entity);
                    await _unitOfWork.CompleteAsyn();
                }

                var existingSubmittedSignatureResult = await HandlePostSubmitSignatureAsync(entity, submittedByUserId);
                return new ApiResponse(200, "Form submission is already Submitted", new SubmitFormResponseDto
                {
                    Submitted = true,
                    SignatureRequired = existingSubmittedSignatureResult.SignatureRequired,
                    SignatureStatus = existingSubmittedSignatureResult.SignatureStatus,
                    SigningUrl = existingSubmittedSignatureResult.SigningUrl
                });
            }

            // 0. Evaluate Pre-Submit Blocking Rules (before submission)
            if (_ruleEvaluationService != null)
            {
                // Get field values for submission-based rules
                var fieldValues = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                var submissionValues = await _unitOfWork.FormSubmissionValuesRepository
                    .GetBySubmissionIdAsync(entity.Id);
                
                foreach (var value in submissionValues.Where(v => !v.IsDeleted))
                {
                    if (!string.IsNullOrWhiteSpace(value.FieldCode))
                    {
                        // Get value from appropriate property based on type
                        object fieldValue = GetFieldValueAsObject(value);
                        // Use uppercase for consistency with rule evaluation
                        var fieldCode = value.FieldCode.ToUpperInvariant();
                        fieldValues[fieldCode] = fieldValue;
                    }
                }

                // Also get grid totals if needed (calculated fields)
                // This would be handled by formula service if needed

                var blockingResult = await _ruleEvaluationService.EvaluateBlockingRulesAsync(
                    entity.FormBuilderId, 
                    "PreSubmit",
                    entity.Id,
                    fieldValues);
                
                if (blockingResult.IsBlocked)
                {
                    return new ApiResponse(403, blockingResult.BlockMessage ?? "Form submission is blocked", new
                    {
                        isBlocked = true,
                        message = blockingResult.BlockMessage,
                        ruleId = blockingResult.MatchedRuleId,
                        ruleName = blockingResult.MatchedRuleName
                    });
                }
            }

            // Get Document Type to check for approval workflow
            var documentType = await _unitOfWork.DocumentTypeRepository.GetByIdAsync(entity.DocumentTypeId);
            if (documentType == null)
                return new ApiResponse(404, "Document type not found");

            entity.SubmittedDate = DateTime.UtcNow;
            entity.SubmittedByUserId = submittedByUserId;

            var series = await _unitOfWork.DocumentSeriesRepository.GetByIdAsync(entity.SeriesId);
            if (series == null || !series.IsActive)
                return new ApiResponse(400, "Document series not found or inactive.");

            if (series.GenerateOn.Equals("Submit", StringComparison.OrdinalIgnoreCase) &&
                DocumentSeriesEngineRules.IsDraftDocumentNumber(entity.DocumentNumber))
            {
                var generation = await _documentNumberGenerator.GenerateForSubmissionAsync(
                    entity.Id,
                    "Submit",
                    submittedByUserId);

                if (!generation.Success || string.IsNullOrWhiteSpace(generation.DocumentNumber))
                {
                    return new ApiResponse(500, generation.ErrorMessage ?? "Failed to generate document number.");
                }

                entity.DocumentNumber = generation.DocumentNumber;
            }

            entity.UpdatedDate = DateTime.UtcNow;

            // Requirement: Always keep status as "Submitted" when user submits.
            // Do NOT auto-approve when no workflow is assigned or workflow is inactive.
            entity.Status = "Submitted";

            _unitOfWork.FormSubmissionsRepository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            // We must execute FormSubmitted trigger regardless of workflow presence.
            // Previously this trigger ran only when workflow activation succeeded, which caused emails
            // (and other trigger side-effects) to be skipped when no workflow exists or activation fails.
            bool formSubmittedTriggerExecuted = false;

            // If workflow is active, activate the first stage
            int? activatedStageId = null;
            if (entity.Status == "Submitted")
            {
                // Prefer explicitly assigned workflow. If not assigned, fallback to an active workflow for the document type.
                APPROVAL_WORKFLOWS? workflow = null;
                if (documentType.ApprovalWorkflowId.HasValue)
                {
                    workflow = await _unitOfWork.ApprovalWorkflowRepository.GetByIdAsync(documentType.ApprovalWorkflowId.Value);
                }
                if (workflow == null)
                {
                    workflow = await _unitOfWork.ApprovalWorkflowRepository.GetActiveWorkflowByDocumentTypeIdAsync(entity.DocumentTypeId);
                }

                if (workflow != null && workflow.IsActive)
                {
                    // Activate the first stage of the workflow
                    var activationResult = await _approvalWorkflowRuntimeService.ActivateStageForSubmissionAsync(entity.Id);
                    if (activationResult.StatusCode == 200)
                    {
                        // Get the activated stage ID from the result or from the updated submission
                        var updatedSubmission = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(entity.Id);
                        activatedStageId = updatedSubmission?.StageId;

                        // ✅ TRIGGER: FormSubmitted - Execute trigger after workflow activation (StageId assigned)
                        // This avoids FK issues on DOCUMENT_APPROVAL_HISTORY.StageId.
                        if (updatedSubmission != null)
                        {
                            await _triggersService.ExecuteFormSubmittedTriggerAsync(updatedSubmission);
                            formSubmittedTriggerExecuted = true;
                        }

                        // ✅ TRIGGER: ApprovalRequired - Execute trigger when stage is activated
                        if (activatedStageId.HasValue && activatedStageId.Value > 0)
                        {
                            await _triggersService.ExecuteApprovalRequiredTriggerAsync(updatedSubmission, activatedStageId.Value);
                        }
                    }
                    else
                    {
                        // If activation fails, log warning but don't fail the submission
                        // The submission is already saved with Status = "Submitted"
                        // You might want to log this error for monitoring
                        _logger?.LogWarning("Workflow activation failed for submission {SubmissionId}. StatusCode: {StatusCode}. FormSubmitted trigger will still execute without StageId.",
                            entity.Id, activationResult.StatusCode);
                    }
                }
            }

            // ✅ TRIGGER: FormSubmitted fallback
            // Ensure trigger is executed even when there is no workflow or activation fails.
            if (entity.Status == "Submitted" && !formSubmittedTriggerExecuted)
            {
                var updatedSubmission = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(entity.Id);
                if (updatedSubmission != null)
                {
                    await _triggersService.ExecuteFormSubmittedTriggerAsync(updatedSubmission);
                }
                else
                {
                    // As a last resort, execute with the tracked entity (should be safe, but may miss navigation updates).
                    await _triggersService.ExecuteFormSubmittedTriggerAsync(entity);
                }
            }

            // NOTE: Submission history is handled inside ExecuteFormSubmittedTriggerAsync,
            // and only when StageId is valid (>0).

            // IMPORTANT: Email sending is controlled ONLY via ALERT_RULES and handled inside triggers service.
            // Do NOT send any emails directly from here.

            // Reload to ensure response reflects latest StageId/Status updates (workflow activation may update StageId in DB)
            var latestSubmission = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(entity.Id) ?? entity;
            var signatureResult = await HandlePostSubmitSignatureAsync(latestSubmission, submittedByUserId);

            return new ApiResponse(200, "Form submission submitted successfully", new SubmitFormResponseDto
            {
                Submitted = true,
                SignatureRequired = signatureResult.SignatureRequired,
                SignatureStatus = signatureResult.SignatureStatus,
                SigningUrl = signatureResult.SigningUrl
            });
        }

        public async Task<ApiResponse> GetSigningUrlAsync(int submissionId, string requestedByUserId)
        {
            var submission = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(submissionId);
            if (submission == null || submission.IsDeleted)
                return new ApiResponse(404, "Form submission not found");

            if (!string.Equals(submission.Status, "Submitted", StringComparison.OrdinalIgnoreCase))
                return new ApiResponse(400, "Signing is available only for submitted forms");

            if (string.Equals(submission.SignatureStatus, "signed", StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse(400, "Submission is already signed");
            }

            var signatureResult = await HandlePostSubmitSignatureAsync(submission, requestedByUserId);
            if (!signatureResult.SignatureRequired)
                return new ApiResponse(400, "Signature is not required for this submission");

            if (string.Equals(signatureResult.SignatureStatus, "signed", StringComparison.OrdinalIgnoreCase))
                return new ApiResponse(400, "Submission is already signed");

            if (string.IsNullOrWhiteSpace(signatureResult.SigningUrl))
                return new ApiResponse(400, "Signing URL is not available for this submission");

            return new ApiResponse(200, "Signing URL generated successfully", new SubmitFormResponseDto
            {
                Submitted = true,
                SignatureRequired = true,
                SignatureStatus = signatureResult.SignatureStatus,
                SigningUrl = signatureResult.SigningUrl
            });
        }

        public async Task<ApiResponse> UpdateStatusAsync(int id, string status)
        {
            var entity = await _unitOfWork.FormSubmissionsRepository.SingleOrDefaultAsync(s => s.Id == id, asNoTracking: false);
            if (entity == null)
                return new ApiResponse(404, "Form submission not found");

            entity.Status = status;
            entity.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.FormSubmissionsRepository.Update(entity);
            await _unitOfWork.CompleteAsyn();

            var updatedEntity = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(id);
            var submissionDto = _mapper.Map<FormSubmissionDto>(updatedEntity);
            return new ApiResponse(200, $"Form submission status updated to {status} successfully", submissionDto);
        }

        /// <summary>
        /// الموافقة على Submission وإنشاء سجل في Approval History
        /// </summary>
        public async Task<ApiResponse> ApproveSubmissionAsync(ApproveSubmissionDto dto)
        {
            // Security: Route all approval actions through runtime service to enforce
            // stage assignee authorization and delegation rules.
            var actionDto = new ApprovalActionDto
            {
                SubmissionId = dto.SubmissionId,
                StageId = dto.StageId,
                ActionType = "Approved",
                ActionByUserId = dto.ActionByUserId,
                Comments = dto.Comments
            };

            return await _approvalWorkflowRuntimeService.ProcessApprovalActionAsync(actionDto);
        }

        /// <summary>
        /// رفض Submission وإنشاء سجل في Approval History
        /// </summary>
        public async Task<ApiResponse> RejectSubmissionAsync(RejectSubmissionDto dto)
        {
            // Security: Route all rejection actions through runtime service to enforce
            // stage assignee authorization and delegation rules.
            var actionDto = new ApprovalActionDto
            {
                SubmissionId = dto.SubmissionId,
                StageId = dto.StageId,
                ActionType = "Rejected",
                ActionByUserId = dto.ActionByUserId,
                Comments = dto.Comments
            };

            return await _approvalWorkflowRuntimeService.ProcessApprovalActionAsync(actionDto);
        }

        public async Task<ApiResponse> ExistsAsync(int id)
        {
            var exists = await _unitOfWork.FormSubmissionsRepository.AnyAsync(s => s.Id == id);
            return new ApiResponse(200, "Form submission existence checked successfully", exists);
        }

        public async Task<ApiResponse> SaveFormSubmissionDataAsync(SaveFormSubmissionDataDto saveDto, string? resolvedUserId = null)
        {
            if (saveDto == null)
                return new ApiResponse(400, "DTO is required");

            // التحقق من Submission
            var submission = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(saveDto.SubmissionId);
            if (submission == null)
                return new ApiResponse(404, "Submission not found");

            // ✅ TRIGGER: Document Locking Enforcement - Prevent edits when rejected or approved
            if (submission.Status == "Rejected")
            {
                return new ApiResponse(403, "Cannot save submission data. Document has been rejected and is locked for editing.");
            }
            if (submission.Status == "Approved")
            {
                return new ApiResponse(403, "Cannot save submission data. Document has been approved and is locked for editing.");
            }

            // حفظ Field Values
            if (saveDto.FieldValues != null && saveDto.FieldValues.Any())
            {
                // Convert BulkSaveFieldValuesDto to BulkFormSubmissionValuesDto
                // Convert JsonElement to string for ValueJson (handles arrays like ["1"] for checkboxes)
                var bulkFieldValuesDto = new BulkFormSubmissionValuesDto
                {
                    SubmissionId = saveDto.SubmissionId,
                    Values = saveDto.FieldValues.Select(fv => 
                    {
                        // Convert JsonElement to string for ValueJson
                        string? valueJsonString = null;
                        if (fv.ValueJson.ValueKind != System.Text.Json.JsonValueKind.Null && 
                            fv.ValueJson.ValueKind != System.Text.Json.JsonValueKind.Undefined)
                        {
                            valueJsonString = fv.ValueJson.GetRawText();
                        }
                        
                        return new CreateFormSubmissionValueDto
                        {
                            SubmissionId = saveDto.SubmissionId,
                            FieldId = fv.FieldId,
                            FieldCode = fv.FieldCode ?? "",
                            ValueString = fv.ValueString,
                            ValueNumber = fv.ValueNumber,
                            ValueDate = fv.ValueDate,
                            ValueBool = fv.ValueBool,
                            ValueJson = valueJsonString // Convert JsonElement to string
                        };
                    }).ToList()
                };
                var result = await _formSubmissionValuesService.CreateBulkAsync(bulkFieldValuesDto);
                if (result.StatusCode != 200)
                    return result;
            }

            // حساب وحفظ الحقول المحسوبة (Calculated Fields)
            await CalculateAndSaveCalculatedFieldsAsync(saveDto.SubmissionId);

            // حفظ Attachments
            if (saveDto.Attachments != null && saveDto.Attachments.Any())
            {
                // Convert SaveFormSubmissionAttachmentDto to CreateFormSubmissionAttachmentDto
                var bulkAttachmentsDto = new BulkAttachmentsDto
                {
                    SubmissionId = saveDto.SubmissionId,
                    Attachments = saveDto.Attachments.Select(att => new CreateFormSubmissionAttachmentDto
                    {
                        SubmissionId = saveDto.SubmissionId,
                        FieldId = att.FieldId,
                        FieldCode = att.FieldCode ?? "",
                        FileName = att.FileName,
                        FilePath = att.FilePath,
                        FileSize = att.FileSize,
                        ContentType = att.ContentType
                    }).ToList()
                };
                var attachmentResult = await _formSubmissionAttachmentsService.CreateBulkAsync(bulkAttachmentsDto);
                if (attachmentResult.StatusCode != 200)
                    return new ApiResponse(attachmentResult.StatusCode, attachmentResult.Message);
            }

            // حفظ Grid Data
            if (saveDto.GridData != null && saveDto.GridData.Any())
            {
                // تجميع Grid data حسب GridId
                var gridDataGroups = saveDto.GridData.GroupBy(g => g.GridId);
                
                foreach (var group in gridDataGroups)
                {
                    var gridId = group.Key;
                    var rows = group.ToList();
                    
                    var bulkDto = new BulkSaveGridDataDto
                    {
                        SubmissionId = saveDto.SubmissionId,
                        GridId = gridId,
                        Rows = rows
                    };
                    
                    // التحقق من البيانات أولاً
                    var validationResult = await _formSubmissionGridRowService.ValidateGridDataAsync(bulkDto);
                    if (validationResult.StatusCode == 200)
                    {
                        var validationData = validationResult.Data as GridValidationResultDto;
                        if (validationData != null && !validationData.IsValid)
                        {
                            return new ApiResponse(400, "Grid validation failed", validationData);
                        }
                    }
                    
                    // حفظ البيانات
                    await _formSubmissionGridRowService.SaveBulkGridDataAsync(bulkDto);
                }
            }

            // Keep Draft editable after save-data.
            // save-data is for persisting draft content only; final transition to Submitted
            // must happen explicitly through SubmitAsync.
            if (submission.Status == "Draft")
            {
                var submissionToUpdate = await _unitOfWork.FormSubmissionsRepository.SingleOrDefaultAsync(
                    s => s.Id == saveDto.SubmissionId, asNoTracking: false);

                if (submissionToUpdate != null)
                {
                    if (string.IsNullOrWhiteSpace(submissionToUpdate.SubmittedByUserId))
                    {
                        submissionToUpdate.SubmittedByUserId = !string.IsNullOrWhiteSpace(resolvedUserId)
                            ? resolvedUserId
                            : "public-user";
                    }
                    else if (!string.IsNullOrWhiteSpace(resolvedUserId) &&
                             resolvedUserId != "public-user" &&
                             submissionToUpdate.SubmittedByUserId == "public-user")
                    {
                        submissionToUpdate.SubmittedByUserId = resolvedUserId;
                    }

                    submissionToUpdate.UpdatedDate = DateTime.UtcNow;
                    _unitOfWork.FormSubmissionsRepository.Update(submissionToUpdate);
                    await _unitOfWork.CompleteAsyn();
                }
            }

            // إعادة تحميل Submission المحدثة للعودة بها
            var updatedSubmission = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(saveDto.SubmissionId);
            var submissionDto = _mapper.Map<FormSubmissionDto>(updatedSubmission);
            
            var statusMessage = updatedSubmission?.Status == "Draft"
                ? "Draft data saved successfully"
                : "Form submission data saved successfully";
            
            return new ApiResponse(200, statusMessage, submissionDto);
        }

        /// <summary>
        /// Calculates and saves calculated field values based on saved field values
        /// </summary>
        private async Task CalculateAndSaveCalculatedFieldsAsync(int submissionId)
        {
            // Get submission with form builder
            var submission = await _unitOfWork.FormSubmissionsRepository.GetByIdAsync(submissionId);
            if (submission == null) return;

            var formBuilderId = submission.FormBuilderId;

            // Get all calculated fields for this form
            var allFields = await _unitOfWork.FormFieldRepository.GetFieldsByFormIdAsync(formBuilderId);
            var calculatedFields = allFields.Where(f => 
                !string.IsNullOrWhiteSpace(f.ExpressionText) &&
                f.IsActive
            ).ToList();

            if (!calculatedFields.Any())
                return;

            // Get all saved field values for this submission
            var savedValues = await _unitOfWork.FormSubmissionValuesRepository.GetBySubmissionIdAsync(submissionId);
            var fieldValuesDict = new Dictionary<string, object>();

            // Build dictionary of field values by FieldCode
            foreach (var savedValue in savedValues)
            {
                if (string.IsNullOrEmpty(savedValue.FieldCode))
                    continue;

                object? value = null;
                if (savedValue.ValueNumber.HasValue)
                    value = savedValue.ValueNumber.Value;
                else if (!string.IsNullOrEmpty(savedValue.ValueString))
                    value = savedValue.ValueString;
                else if (savedValue.ValueDate.HasValue)
                    value = savedValue.ValueDate.Value;
                else if (savedValue.ValueBool.HasValue)
                    value = savedValue.ValueBool.Value;

                if (value != null)
                {
                    fieldValuesDict[savedValue.FieldCode.ToUpper()] = value;
                }
            }

            // Calculate and save calculated field values
            var calculatedValuesToSave = new List<CreateFormSubmissionValueDto>();

            foreach (var calculatedField in calculatedFields)
            {
                // Skip if already saved
                var alreadyExists = savedValues.Any(sv => sv.FieldId == calculatedField.Id);
                if (alreadyExists)
                    continue;

                // Check RecalculateOn setting
                var recalculateOn = calculatedField.RecalculateOn ?? "OnFieldChange";
                if (recalculateOn == "OnSubmitOnly")
                {
                    // Only calculate on submit, but we're saving, so calculate anyway
                }
                else if (recalculateOn == "OnLoad")
                {
                    // Skip if not on load
                    continue;
                }

                // Calculate the value
                var calculationResult = await _formulaService.SafeCalculateExpressionAsync(
                    calculatedField.ExpressionText,
                    fieldValuesDict
                );

                if (!calculationResult.Success || calculationResult.Data == null)
                    continue;

                var calculatedValue = calculationResult.Data;
                var resultType = calculatedField.ResultType?.ToLower() ?? "decimal";

                // Create submission value DTO based on result type
                var submissionValue = new CreateFormSubmissionValueDto
                {
                    SubmissionId = submissionId,
                    FieldId = calculatedField.Id,
                    FieldCode = calculatedField.FieldCode
                };

                // Set value based on result type
                switch (resultType)
                {
                    case "integer":
                    case "int":
                    case "decimal":
                        if (decimal.TryParse(calculatedValue.ToString(), out var decimalValue))
                        {
                            submissionValue.ValueNumber = decimalValue;
                        }
                        break;
                    case "text":
                    case "string":
                        submissionValue.ValueString = calculatedValue.ToString();
                        break;
                    default:
                        if (decimal.TryParse(calculatedValue.ToString(), out var defaultDecimal))
                        {
                            submissionValue.ValueNumber = defaultDecimal;
                        }
                        else
                        {
                            submissionValue.ValueString = calculatedValue.ToString();
                        }
                        break;
                }

                calculatedValuesToSave.Add(submissionValue);
            }

            // Save calculated values
            if (calculatedValuesToSave.Any())
            {
                var bulkCalculatedValuesDto = new BulkFormSubmissionValuesDto
                {
                    SubmissionId = submissionId,
                    Values = calculatedValuesToSave
                };
                await _formSubmissionValuesService.CreateBulkAsync(bulkCalculatedValuesDto);
            }
        }

        private async Task<(bool SignatureRequired, string SignatureStatus, string? SigningUrl)> HandlePostSubmitSignatureAsync(
            FORM_SUBMISSIONS submission,
            string submittedByUserId)
        {
            var stageRequiresSignature = false;
            if (submission.StageId.HasValue && submission.StageId.Value > 0)
            {
                var stage = await _unitOfWork.ApprovalStageRepository.GetByIdAsync(submission.StageId.Value);
                stageRequiresSignature = stage?.RequiresAdobeSign == true;
            }

            if (!stageRequiresSignature)
            {
                await PersistSignatureStateAsync(submission.Id, "not_required", null, null, keepExistingEnvelope: true);
                return (false, "not_required", null);
            }

            if (string.Equals(submission.SignatureStatus, "signed", StringComparison.OrdinalIgnoreCase))
            {
                return (true, "signed", null);
            }

            var signer = await ResolveSignerAsync(submittedByUserId, submission.SubmittedByUserId);
            if (string.IsNullOrWhiteSpace(signer.Email))
            {
                _logger?.LogWarning("Signature is required but signer email could not be resolved for submission {SubmissionId}", submission.Id);
                await PersistSignatureStateAsync(submission.Id, "pending", submission.DocuSignEnvelopeId, null, keepExistingEnvelope: true);
                return (true, "pending", null);
            }

            var flowResult = await _submitSignatureFlowService.ExecuteAsync(new SubmitSignatureFlowInputDto
            {
                SignatureRequired = true,
                SubmissionId = submission.Id,
                ExistingEnvelopeId = submission.DocuSignEnvelopeId,
                DocumentNumber = string.IsNullOrWhiteSpace(submission.DocumentNumber) ? $"SUB-{submission.Id}" : submission.DocumentNumber,
                ReturnUrl = BuildRecipientReturnUrl(submission.Id),
                Signer = new DocuSignSignerDto
                {
                    UserId = signer.ClientUserId,
                    Name = signer.Name,
                    Email = signer.Email
                }
            });

            await PersistSignatureStateAsync(submission.Id, flowResult.SignatureStatus, flowResult.EnvelopeId, null, keepExistingEnvelope: false);
            return (flowResult.SignatureRequired, flowResult.SignatureStatus, flowResult.SigningUrl);
        }

        private async Task PersistSignatureStateAsync(
            int submissionId,
            string signatureStatus,
            string? envelopeId,
            DateTime? signedAt,
            bool keepExistingEnvelope)
        {
            var tracked = await _unitOfWork.FormSubmissionsRepository.SingleOrDefaultAsync(
                s => s.Id == submissionId && !s.IsDeleted,
                asNoTracking: false);

            if (tracked == null)
                return;

            tracked.SignatureStatus = signatureStatus;
            if (!keepExistingEnvelope)
            {
                tracked.DocuSignEnvelopeId = envelopeId;
            }
            else if (!string.IsNullOrWhiteSpace(envelopeId))
            {
                tracked.DocuSignEnvelopeId = envelopeId;
            }

            tracked.SignedAt = signedAt;
            tracked.UpdatedDate = DateTime.UtcNow;
            _unitOfWork.FormSubmissionsRepository.Update(tracked);
            await _unitOfWork.CompleteAsyn();
        }

        private string BuildRecipientReturnUrl(int submissionId)
        {
            var explicitReturnUrl = Environment.GetEnvironmentVariable("DS_RECIPIENT_RETURN_URL")
                                   ?? _configuration["DocuSign:RecipientReturnUrl"];
            if (!string.IsNullOrWhiteSpace(explicitReturnUrl))
            {
                var normalized = explicitReturnUrl.Trim();
                var separator = normalized.Contains('?') ? "&" : "?";
                return $"{normalized}{separator}submissionId={submissionId}";
            }

            var appBaseUrl = Environment.GetEnvironmentVariable("APP_BASE_URL")
                             ?? _configuration["App:BaseUrl"];
            var redirectUri = Environment.GetEnvironmentVariable("DS_REDIRECT_URI")
                              ?? _configuration["DocuSign:RedirectUri"];
            var baseUrl = !string.IsNullOrWhiteSpace(appBaseUrl) ? appBaseUrl : ExtractOrigin(redirectUri);
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return $"https://localhost/signature/return?submissionId={submissionId}";
            }

            return $"{baseUrl.TrimEnd('/')}/signature/return?submissionId={submissionId}";
        }

        private static string? ExtractOrigin(string? uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
                return null;

            if (!Uri.TryCreate(uri, UriKind.Absolute, out var parsed))
                return uri;

            return $"{parsed.Scheme}://{parsed.Authority}";
        }

        private async Task<(string Email, string Name, string ClientUserId)> ResolveSignerAsync(string submitterFromRequest, string? submitterFromEntity)
        {
            var candidates = new List<string>();
            if (!string.IsNullOrWhiteSpace(submitterFromRequest))
                candidates.Add(submitterFromRequest.Trim());
            if (!string.IsNullOrWhiteSpace(submitterFromEntity))
                candidates.Add(submitterFromEntity.Trim());

            foreach (var candidate in candidates.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                TblUser? user = null;
                if (int.TryParse(candidate, out var numericId))
                {
                    user = await _identityContext.TblUsers.FirstOrDefaultAsync(u => u.Id == numericId);
                }
                else
                {
                    user = await _identityContext.TblUsers.FirstOrDefaultAsync(u => u.Username == candidate);
                }

                if (user == null)
                    continue;

                var email = string.IsNullOrWhiteSpace(user.Email) ? user.Username : user.Email;
                var name = string.IsNullOrWhiteSpace(user.Name) ? user.Username : user.Name;
                var clientUserId = user.Id.ToString();

                return (email ?? string.Empty, name ?? "Signer", clientUserId);
            }

            // Fallback for non-resolved users (e.g. public-user)
            var fallback = !string.IsNullOrWhiteSpace(submitterFromRequest)
                ? submitterFromRequest.Trim()
                : (submitterFromEntity ?? "public-user");
            return (string.Empty, fallback, fallback);
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

        /// <summary>
        /// Gets field value as object from FORM_SUBMISSION_VALUES
        /// </summary>
        private object GetFieldValueAsObject(FORM_SUBMISSION_VALUES value)
        {
            if (!string.IsNullOrWhiteSpace(value.ValueString))
                return value.ValueString;
            if (value.ValueNumber.HasValue)
                return value.ValueNumber.Value;
            if (value.ValueDate.HasValue)
                return value.ValueDate.Value;
            if (value.ValueBool.HasValue)
                return value.ValueBool.Value;
            if (!string.IsNullOrWhiteSpace(value.ValueJson))
                return value.ValueJson;
            return string.Empty;
        }
    }
}
