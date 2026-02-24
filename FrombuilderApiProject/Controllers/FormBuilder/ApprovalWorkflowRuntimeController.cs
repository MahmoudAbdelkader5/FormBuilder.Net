using FormBuilder.Application.DTOs.ApprovalWorkflow;
using FormBuilder.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApprovalWorkflowRuntimeController : ControllerBase
    {
        private readonly IApprovalWorkflowRuntimeService _service;
        private readonly ILogger<ApprovalWorkflowRuntimeController> _logger;

        public ApprovalWorkflowRuntimeController(
            IApprovalWorkflowRuntimeService service,
            ILogger<ApprovalWorkflowRuntimeController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // POST: api/ApprovalWorkflowRuntime/activate-stage
        [HttpPost("activate-stage")]
        public async Task<IActionResult> ActivateStageForSubmission([FromBody] JsonElement body, CancellationToken cancellationToken = default)
        {
            int submissionId;

            // Accept either:
            // 1) Raw number: 1
            // 2) Object: { "submissionId": 1 } or { "SubmissionId": 1 }
            if (body.ValueKind == JsonValueKind.Number && body.TryGetInt32(out var idFromNumber))
            {
                submissionId = idFromNumber;
            }
            else if (body.ValueKind == JsonValueKind.Object)
            {
                if (body.TryGetProperty("submissionId", out var sid1) && sid1.TryGetInt32(out var id1))
                {
                    submissionId = id1;
                }
                else if (body.TryGetProperty("SubmissionId", out var sid2) && sid2.TryGetInt32(out var id2))
                {
                    submissionId = id2;
                }
                else
                {
                    return BadRequest(new { statusCode = 400, message = "submissionId is required" });
                }
            }
            else
            {
                return BadRequest(new { statusCode = 400, message = "Invalid request body. Send a number (e.g. 1) or { \"submissionId\": 1 }." });
            }

            var response = await _service.ActivateStageForSubmissionAsync(submissionId);
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/ApprovalWorkflowRuntime/resolve-approvers/stage/1
        [HttpGet("resolve-approvers/stage/{stageId}")]
        public async Task<IActionResult> ResolveApproversForStage(int stageId, CancellationToken cancellationToken = default)
        {
            var response = await _service.ResolveApproversForStageAsync(stageId);
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/ApprovalWorkflowRuntime/check-delegation/userId
        [HttpGet("check-delegation/{userId}")]
        public async Task<IActionResult> CheckDelegation(string userId, CancellationToken cancellationToken = default)
        {
            var response = await _service.CheckDelegationAsync(userId);
            return StatusCode(response.StatusCode, response);
        }

        // POST: api/ApprovalWorkflowRuntime/process-action
        [HttpPost("process-action")]
        public async Task<IActionResult> ProcessApprovalAction([FromBody] ApprovalActionDto dto, CancellationToken cancellationToken = default)
        {
            var response = await _service.ProcessApprovalActionAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        // POST: api/ApprovalWorkflowRuntime/request-signature
        [HttpPost("request-signature")]
        public async Task<IActionResult> RequestStageSignature([FromBody] RequestStageSignatureDto dto, CancellationToken cancellationToken = default)
        {
            if (dto == null)
                return BadRequest(new { statusCode = 400, message = "Request body is required" });

            if (string.IsNullOrWhiteSpace(dto.RequestedByUserId))
            {
                dto.RequestedByUserId =
                    User?.FindFirstValue(ClaimTypes.Name) ??
                    User?.FindFirstValue(ClaimTypes.NameIdentifier) ??
                    "system";
            }

            var response = await _service.RequestStageSignatureAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/ApprovalWorkflowRuntime/inbox/userId
        [HttpGet("inbox/{userId}")]
        public async Task<IActionResult> GetApprovalInbox(string userId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("=== GetApprovalInbox Request ===");
            _logger.LogInformation("Received userId: '{UserId}' (Type: {Type}, Length: {Length})", 
                userId, userId?.GetType().Name ?? "null", userId?.Length ?? 0);
            
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("UserId is null or empty");
                return BadRequest(new { statusCode = 400, message = "UserId is required" });
            }

            var response = await _service.GetApprovalInboxAsync(userId);
            
            _logger.LogInformation("Response StatusCode: {StatusCode}, Message: {Message}", 
                response.StatusCode, response.Message);
            
            if (response.Data is System.Collections.ICollection dataCollection)
            {
                _logger.LogInformation("Response Data Count: {Count}", dataCollection.Count);
            }
            else if (response.Data != null)
            {
                _logger.LogInformation("Response Data Type: {Type}", response.Data.GetType().Name);
            }
            else
            {
                _logger.LogWarning("Response Data is null");
            }
            
            _logger.LogInformation("=== End GetApprovalInbox Request ===");
            
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/ApprovalWorkflowRuntime/inbox-debug/userId
        [HttpGet("inbox-debug/{userId}")]
        public async Task<IActionResult> DebugInbox(string userId, CancellationToken cancellationToken = default)
        {
            var response = await _service.GetInboxDebugInfoAsync(userId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
