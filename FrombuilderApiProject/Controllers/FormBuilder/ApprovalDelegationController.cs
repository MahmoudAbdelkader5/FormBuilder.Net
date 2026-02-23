using FormBuilder.API.Attributes;
using FormBuilder.Application.DTOs.ApprovalWorkflow;
using FormBuilder.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApprovalDelegationController : ControllerBase
    {
        private readonly IApprovalDelegationService _service;

        public ApprovalDelegationController(IApprovalDelegationService service)
        {
            _service = service;
        }

        // GET: api/ApprovalDelegation?fromUserId=xxx
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string fromUserId = null)
        {
            var response = await _service.GetAllAsync(fromUserId);
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/ApprovalDelegation/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _service.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/ApprovalDelegation/active/{userId} - Get active delegations by ToUserId
        [HttpGet("active/{userId}")]
        public async Task<IActionResult> GetActiveDelegations(string userId)
        {
            var response = await _service.GetActiveDelegationsByToUserIdAsync(userId);
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/ApprovalDelegation/active/to/{toUserId} - Get active delegations by ToUserId
        [HttpGet("active/to/{toUserId}")]
        public async Task<IActionResult> GetActiveDelegationsByToUserId(string toUserId)
        {
            var response = await _service.GetActiveDelegationsByToUserIdAsync(toUserId);
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/ApprovalDelegation/all/to/{toUserId} - Get ALL delegations by ToUserId (for debugging)
        [HttpGet("all/to/{toUserId}")]
        public async Task<IActionResult> GetAllDelegationsByToUserId(string toUserId)
        {
            var response = await _service.GetAllDelegationsByToUserIdAsync(toUserId);
            return StatusCode(response.StatusCode, response);
        }

        // POST: api/ApprovalDelegation
        [HttpPost]
        [RequirePermission("ApprovalDelegation_Allow_Create")]
        public async Task<IActionResult> Create([FromBody] ApprovalDelegationCreateDto dto)
        {
            var response = await _service.CreateAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        // PUT: api/ApprovalDelegation/5
        [HttpPut("{id}")]
        [RequirePermission("ApprovalDelegation_Allow_Edit")]
        public async Task<IActionResult> Update(int id, [FromBody] ApprovalDelegationUpdateDto dto)
        {
            var response = await _service.UpdateAsync(id, dto);
            return StatusCode(response.StatusCode, response);
        }

        // DELETE: api/ApprovalDelegation/5
        [HttpDelete("{id}")]
        [RequirePermission("ApprovalDelegation_Allow_Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _service.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        // POST: api/ApprovalDelegation/resolve
        // This endpoint is used internally by the approval process
        [HttpPost("resolve")]
        [AllowAnonymous]  // Allow anonymous for internal use
        public async Task<IActionResult> ResolveDelegatedApprover([FromBody] ResolveDelegationRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.OriginalApproverId))
            {
                return BadRequest(new { message = "OriginalApproverId is required" });
            }

            var delegatedUserId = await _service.ResolveDelegatedApproverAsync(
                request.OriginalApproverId,
                request.WorkflowId,
                request.SubmissionId);

            return Ok(new
            {
                originalApproverId = request.OriginalApproverId,
                delegatedUserId = delegatedUserId,
                hasDelegation = delegatedUserId != null
            });
        }
    }

    // DTO for resolve request
    public class ResolveDelegationRequestDto
    {
        public string OriginalApproverId { get; set; }
        public int? WorkflowId { get; set; }
        public int? SubmissionId { get; set; }
    }
}
