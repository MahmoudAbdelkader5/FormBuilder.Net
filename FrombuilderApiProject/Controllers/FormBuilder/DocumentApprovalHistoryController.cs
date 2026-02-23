using FormBuilder.Application.DTOs.ApprovalWorkflow;
using FormBuilder.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication
    public class DocumentApprovalHistoryController : ControllerBase
    {
        private readonly IDocumentApprovalHistoryService _service;

        public DocumentApprovalHistoryController(IDocumentApprovalHistoryService service)
        {
            _service = service;
        }

        // GET: api/DocumentApprovalHistory/submission/1
        [HttpGet("submission/{submissionId}")]
        public async Task<IActionResult> GetBySubmissionId(int submissionId)
        {
            var response = await _service.GetBySubmissionIdAsync(submissionId);
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/DocumentApprovalHistory/stage/1
        [HttpGet("stage/{stageId}")]
        public async Task<IActionResult> GetByStageId(int stageId)
        {
            var response = await _service.GetByStageIdAsync(stageId);
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/DocumentApprovalHistory/user/userId
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var response = await _service.GetByUserIdAsync(userId);
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/DocumentApprovalHistory/all
        // Get all approval history (for admin) - shows all approved and rejected submissions
        [HttpGet("all")]
        [Authorize] // Only admins can see all approval history
        public async Task<IActionResult> GetAllApprovalHistory()
        {
            var response = await _service.GetAllApprovalHistoryAsync();
            return StatusCode(response.StatusCode, response);
        }

        // POST: api/DocumentApprovalHistory
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DocumentApprovalHistoryCreateDto dto)
        {
            var response = await _service.CreateAsync(dto);
            return StatusCode(response.StatusCode, response);
        }
    }
}

