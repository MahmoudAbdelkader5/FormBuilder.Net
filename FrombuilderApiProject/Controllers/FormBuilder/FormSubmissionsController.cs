using FormBuilder.API.Attributes;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.API.Models;
using FormBuilder.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FormSubmissionsController : ControllerBase
    {
        private readonly IFormSubmissionsService _formSubmissionsService;

        public FormSubmissionsController(IFormSubmissionsService formSubmissionsService)
        {
            _formSubmissionsService = formSubmissionsService;
        }

        /// <summary>
        /// Resolve the submitter identifier in a safe way.
        /// IMPORTANT: We do NOT trust client-provided SubmittedByUserId for anonymous endpoints
        /// to prevent spoofing (e.g. sending "admin" from a public form).
        /// If the request is authenticated, we prefer the username (ClaimTypes.Name),
        /// otherwise we fallback to "public-user".
        /// 
        /// Note: forcePublic=true only forces "public-user" if user is NOT authenticated.
        /// If user IS authenticated, we use their real username even if forcePublic=true.
        /// </summary>
        private string ResolveSubmittedByUserId(bool forcePublic = false)
        {
            // Always check authentication first - authenticated users should use their real username
            if (User?.Identity?.IsAuthenticated == true)
            {
                var username = User.FindFirstValue(ClaimTypes.Name);
                if (!string.IsNullOrWhiteSpace(username))
                    return username;

                var nameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrWhiteSpace(nameIdentifier))
                    return nameIdentifier;
            }

            // Only return "public-user" if user is NOT authenticated
            // (or if forcePublic=true and we want to ensure anonymous users get "public-user")
            return "public-user";
        }

        [HttpGet]
        [RequirePermission("Submission_Allow_View")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _formSubmissionsService.GetAllAsync(cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        [RequirePermission("Submission_Allow_View")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
        {
            var result = await _formSubmissionsService.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("details/{id}")]
        [RequirePermission("Submission_Allow_View")]
        public async Task<IActionResult> GetByIdWithDetails(int id, CancellationToken cancellationToken = default)
        {
            var result = await _formSubmissionsService.GetByIdWithDetailsAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("document/{documentNumber}")]
        [RequirePermission("Submission_Allow_View")]
        public async Task<IActionResult> GetByDocumentNumber(string documentNumber, CancellationToken cancellationToken = default)
        {
            var result = await _formSubmissionsService.GetByDocumentNumberAsync(documentNumber, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("form/{formBuilderId}")]
        [RequirePermission("Submission_Allow_View")]
        public async Task<IActionResult> GetByFormBuilderId(int formBuilderId, CancellationToken cancellationToken = default)
        {
            var result = await _formSubmissionsService.GetByFormBuilderIdAsync(formBuilderId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("document-type/{documentTypeId}")]
        [RequirePermission("Submission_Allow_View")]
        public async Task<IActionResult> GetByDocumentTypeId(int documentTypeId, CancellationToken cancellationToken = default)
        {
            var result = await _formSubmissionsService.GetByDocumentTypeIdAsync(documentTypeId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("user/{userId}")]
        [RequirePermission("Submission_Allow_View")]
        public async Task<IActionResult> GetByUserId(string userId, CancellationToken cancellationToken = default)
        {
            var result = await _formSubmissionsService.GetByUserIdAsync(userId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("status/{status}")]
        [RequirePermission("Submission_Allow_View")]
        public async Task<IActionResult> GetByStatus(string status, CancellationToken cancellationToken = default)
        {
            var result = await _formSubmissionsService.GetByStatusAsync(status, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get draft submission by formBuilderId, projectId, and submittedByUserId
        /// This endpoint allows checking if a draft exists before creating a new one
        /// Returns 404 if no draft exists
        /// </summary>
        [HttpGet("draft")]
        [AllowAnonymous] // Allow anonymous for public form submissions
        public async Task<IActionResult> GetDraft([FromQuery] int formBuilderId, [FromQuery] int projectId, [FromQuery] string? submittedByUserId = null, CancellationToken cancellationToken = default)
        {
            if (formBuilderId <= 0)
                return BadRequest(new ApiResponse(400, "FormBuilderId is required"));
            if (projectId <= 0)
                return BadRequest(new ApiResponse(400, "ProjectId is required"));

            // Security: do not trust client-provided submittedByUserId for public endpoints
            var userId = ResolveSubmittedByUserId(forcePublic: true);

            var result = await _formSubmissionsService.GetDraftAsync(formBuilderId, projectId, userId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get existing draft submission or create a new one if none exists
        /// This is a convenience endpoint that combines GET and POST operations
        /// </summary>
        [HttpGet("draft-or-create")]
        [AllowAnonymous] // Allow anonymous for public form submissions
        public async Task<IActionResult> GetOrCreateDraft([FromQuery] int formBuilderId, 
            [FromQuery] int projectId, 
            [FromQuery] string? submittedByUserId = null,
            [FromQuery] int? seriesId = null, CancellationToken cancellationToken = default)
        {
            if (formBuilderId <= 0)
                return BadRequest(new ApiResponse(400, "FormBuilderId is required"));
            if (projectId <= 0)
                return BadRequest(new ApiResponse(400, "ProjectId is required"));

            // Security: do not trust client-provided submittedByUserId for public endpoints
            var userId = ResolveSubmittedByUserId(forcePublic: true);

            var result = await _formSubmissionsService.GetOrCreateDraftAsync(formBuilderId, projectId, userId, seriesId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [AllowAnonymous] // Allow anonymous for public form submissions
        public async Task<IActionResult> Create([FromBody] CreateFormSubmissionDto createDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", ModelState));

            // Security: do not trust client-provided SubmittedByUserId for public endpoints
            createDto.SubmittedByUserId = ResolveSubmittedByUserId(forcePublic: true);

            var result = await _formSubmissionsService.CreateAsync(createDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Create a new draft submission automatically determining Document Type and Series from FormBuilderId
        /// This endpoint implements the runtime flow: automatically loads Document Type and selects default Series
        /// Accepts JSON body with formBuilderId, projectId (optional - can be derived from seriesId), seriesId (optional), and submittedByUserId
        /// Also accepts query parameters as an alternative to the request body
        /// </summary>
        [HttpPost("draft")]
        [AllowAnonymous] // Allow anonymous for public form submissions
        public async Task<IActionResult> CreateDraft([FromBody] CreateDraftDto? createDraftDto = null,
            [FromQuery] int? formBuilderId = null,
            [FromQuery] int? projectId = null,
            [FromQuery] int? seriesId = null,
            [FromQuery] string? submittedByUserId = null, CancellationToken cancellationToken = default)
        {
            // If body is not provided or has invalid FormBuilderId, try to construct DTO from query parameters
            // Check if DTO is null OR if it has invalid FormBuilderId (default/empty value)
            if (createDraftDto == null || createDraftDto.FormBuilderId <= 0)
            {
                // Clear ModelState errors since we're using query parameters instead of body
                ModelState.Clear();
                
                // Prioritize query parameters over body if body has invalid values
                if (formBuilderId.HasValue && formBuilderId.Value > 0)
                {
                    createDraftDto = new CreateDraftDto
                    {
                        FormBuilderId = formBuilderId.Value,
                        ProjectId = projectId,
                        SeriesId = seriesId,
                        SubmittedByUserId = submittedByUserId
                    };
                }
                else
                {
                    // No valid FormBuilderId in either body or query parameters
                    return BadRequest(new ApiResponse(400, "FormBuilderId is required"));
                }
            }
            else
            {
                // Remove SubmittedByUserId errors from ModelState since it's optional
                var submittedByUserIdKeys = ModelState.Keys
                    .Where(k => k.IndexOf("SubmittedByUserId", StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
                foreach (var key in submittedByUserIdKeys)
                {
                    ModelState.Remove(key);
                }
                
                // Merge missing values from query parameters into the body DTO
                // This allows partial body with query parameters to fill in missing fields
                if (formBuilderId.HasValue && formBuilderId.Value > 0 && createDraftDto.FormBuilderId <= 0)
                {
                    createDraftDto.FormBuilderId = formBuilderId.Value;
                    // Clear ModelState error for this field if it was missing
                    ModelState.Remove(nameof(createDraftDto.FormBuilderId));
                }
                
                if (projectId.HasValue && (!createDraftDto.ProjectId.HasValue || createDraftDto.ProjectId.Value <= 0))
                {
                    createDraftDto.ProjectId = projectId;
                }
                
                if (seriesId.HasValue && (!createDraftDto.SeriesId.HasValue || createDraftDto.SeriesId.Value <= 0))
                {
                    createDraftDto.SeriesId = seriesId;
                }
                
                if (!string.IsNullOrWhiteSpace(submittedByUserId) && string.IsNullOrWhiteSpace(createDraftDto.SubmittedByUserId))
                {
                    createDraftDto.SubmittedByUserId = submittedByUserId;
                    // Clear ModelState error for this field if it was missing
                    ModelState.Remove(nameof(createDraftDto.SubmittedByUserId));
                    // Also remove nested property errors
                    ModelState.Remove("createDraftDto.SubmittedByUserId");
                }
                
                // Only validate ModelState if there are still errors after merging query parameters
                if (!ModelState.IsValid)
                {
                    // Check if remaining errors are only for fields we can't fill from query params
                    var hasUnresolvableErrors = ModelState.Keys.Any(key => 
                        !key.Contains("FormBuilderId") && 
                        !key.Contains("SubmittedByUserId") &&
                        !key.Contains("ProjectId") &&
                        !key.Contains("SeriesId"));
                    
                    if (hasUnresolvableErrors)
                    {
                        return BadRequest(new ApiResponse(400, "Invalid data", ModelState));
                    }
                }
            }

            // Security: do not trust client-provided SubmittedByUserId for public endpoints.
            // Use authenticated username if available; otherwise force "public-user".
            createDraftDto.SubmittedByUserId = ResolveSubmittedByUserId(forcePublic: true);

            var result = await _formSubmissionsService.CreateDraftAsync(createDraftDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFormSubmissionDto updateDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", ModelState));

            var result = await _formSubmissionsService.UpdateAsync(id, updateDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var result = await _formSubmissionsService.DeleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("submit")]
        [AllowAnonymous] // Allow anonymous for public form submissions
        public async Task<IActionResult> Submit([FromBody] SubmitFormDto submitDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", ModelState));

            // Security: do not trust client-provided SubmittedByUserId
            // For public endpoint: use authenticated username if available, otherwise "public-user"
            submitDto.SubmittedByUserId = ResolveSubmittedByUserId(forcePublic: true);

            var result = await _formSubmissionsService.SubmitAsync(submitDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("/api/submissions/{id}/submit")]
        [AllowAnonymous]
        public async Task<IActionResult> SubmitById(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
                return BadRequest(new ApiResponse(400, "Submission ID is required"));

            var submitDto = new SubmitFormDto
            {
                SubmissionId = id,
                SubmittedByUserId = ResolveSubmittedByUserId(forcePublic: true)
            };

            var result = await _formSubmissionsService.SubmitAsync(submitDto, cancellationToken);
            if (result.StatusCode == 200 && result.Data is SubmitFormResponseDto submitResponse)
            {
                return Ok(new
                {
                    submitted = submitResponse.Submitted,
                    signatureRequired = submitResponse.SignatureRequired,
                    signatureStatus = submitResponse.SignatureStatus,
                    signingUrl = submitResponse.SigningUrl
                });
            }

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("/api/submissions/{id}/signing-url")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSigningUrlById(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
                return BadRequest(new ApiResponse(400, "Submission ID is required"));

            var requestedByUserId = ResolveSubmittedByUserId(forcePublic: false);
            var result = await _formSubmissionsService.GetSigningUrlAsync(id, requestedByUserId, cancellationToken);
            if (result.StatusCode == 200)
            {
                return Ok(result.Data);
            }

            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status, CancellationToken cancellationToken = default)
        {
            var result = await _formSubmissionsService.UpdateStatusAsync(id, status, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// الموافقة على Submission وإنشاء سجل في Approval History
        /// </summary>
        [HttpPost("approve")]
        public async Task<IActionResult> ApproveSubmission([FromBody] ApproveSubmissionDto dto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", ModelState));

            // Security: never trust client-provided approver ID.
            dto.ActionByUserId = ResolveSubmittedByUserId(forcePublic: false);

            var result = await _formSubmissionsService.ApproveSubmissionAsync(dto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// رفض Submission وإنشاء سجل في Approval History
        /// </summary>
        [HttpPost("reject")]
        public async Task<IActionResult> RejectSubmission([FromBody] RejectSubmissionDto dto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", ModelState));

            // Security: never trust client-provided approver ID.
            dto.ActionByUserId = ResolveSubmittedByUserId(forcePublic: false);

            var result = await _formSubmissionsService.RejectSubmissionAsync(dto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}/exists")]
        public async Task<IActionResult> Exists(int id, CancellationToken cancellationToken = default)
        {
            var result = await _formSubmissionsService.ExistsAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Check if submission has grid data and return grid rows count
        /// Useful for debugging grid data loading issues
        /// </summary>
        [HttpGet("{id}/grid-data-check")]
        public async Task<IActionResult> CheckGridData(int id, CancellationToken cancellationToken = default)
        {
            var submission = await _formSubmissionsService.GetByIdAsync(id, cancellationToken);
            if (submission.StatusCode == 404)
                return StatusCode(404, submission);

            var submissionData = submission.Data as FormSubmissionDetailDto;
            
            // Debug info: check cells loading directly
            var debugInfo = new
            {
                submissionId = id,
                hasGridData = submissionData?.GridData != null && submissionData.GridData.Any(),
                gridDataCount = submissionData?.GridData?.Count ?? 0,
                totalRows = submissionData?.GridData?.Sum(g => 1) ?? 0,
                totalCells = submissionData?.GridData?.Sum(g => g.Cells?.Count ?? 0) ?? 0,
                gridData = submissionData?.GridData?.Select(g => new
                {
                    gridId = g.GridId,
                    gridName = g.GridName,
                    gridCode = g.GridCode,
                    rowsCount = 1,
                    cellsCount = g.Cells?.Count ?? 0,
                    cells = g.Cells?.Select(c => new
                    {
                        cellId = c.Id,
                        columnId = c.ColumnId,
                        columnCode = c.ColumnCode,
                        hasValue = !string.IsNullOrEmpty(c.ValueString) || c.ValueNumber.HasValue || c.ValueDate.HasValue || c.ValueBool.HasValue
                    })
                })
            };

            return Ok(new ApiResponse(200, "Grid data check completed", debugInfo));
        }

        [HttpPost("save-data")]
        [AllowAnonymous] // Allow anonymous for public form submissions
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SaveFormSubmissionData([FromBody] SaveFormSubmissionDataDto saveDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", ModelState));

            // Security: For public endpoints, we need to ensure SubmittedByUserId is set correctly
            // Pass the resolved userId to the service so it can update the submission if needed
            var resolvedUserId = ResolveSubmittedByUserId(forcePublic: true);
            var result = await _formSubmissionsService.SaveFormSubmissionDataAsync(saveDto, resolvedUserId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}
