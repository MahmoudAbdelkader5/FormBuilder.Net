using FormBuilder.API.Attributes;
using FormBuilder.API.Models;
using FormBuilder.Application.DTOs.ApprovalWorkflow;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Domian.Entitys.FormBuilder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApprovalWorkflowController : ControllerBase
    {
        private readonly IApprovalWorkflowService _workflowService;

        public ApprovalWorkflowController(IApprovalWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        // GET: api/ApprovalWorkflow
        [HttpGet]
        [RequirePermission("ApprovalWorkflow_Allow_View")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _workflowService.GetAllAsync(cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        // GET: api/ApprovalWorkflow/5
        [HttpGet("{id:int}")]
        [RequirePermission("ApprovalWorkflow_Allow_View")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
        {
            var result = await _workflowService.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        // POST: api/ApprovalWorkflow
        [HttpPost]
        [RequirePermission("ApprovalWorkflow_Allow_Create")]
        public async Task<IActionResult> Create([FromBody] ApprovalWorkflowCreateDto dto, CancellationToken cancellationToken = default)
        {
            if (dto == null)
                return BadRequest(new ApiResponse(400, "Invalid request"));

            var result = await _workflowService.CreateAsync(dto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        // PUT: api/ApprovalWorkflow/5
        [HttpPut("{id:int}")]
        [RequirePermission("ApprovalWorkflow_Allow_Edit")]
        public async Task<IActionResult> Update(int id, [FromBody] ApprovalWorkflowUpdateDto dto, CancellationToken cancellationToken = default)
        {
            if (dto == null)
                return BadRequest(new ApiResponse(400, "Invalid request"));

            var result = await _workflowService.UpdateAsync(id, dto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        // PATCH: api/ApprovalWorkflow/5/toggle
        [HttpPatch("{id:int}/toggle")]
        [RequirePermission("ApprovalWorkflow_Allow_Manage")]
        public async Task<IActionResult> ToggleActive(int id, [FromQuery] bool isActive, CancellationToken cancellationToken = default)
        {
            var result = await _workflowService.ToggleActiveAsync(id, isActive, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        // GET: api/ApprovalWorkflow/name/{name}
        [HttpGet("name/{name}")]
        [RequirePermission("ApprovalWorkflow_Allow_View")]
        public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken = default)
        {
            var result = await _workflowService.GetByNameAsync(name, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        // GET: api/ApprovalWorkflow/active
        [HttpGet("active")]
        [RequirePermission("ApprovalWorkflow_Allow_View")]
        public async Task<IActionResult> GetActive(CancellationToken cancellationToken = default)
        {
            var result = await _workflowService.GetActiveAsync(cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        // DELETE: api/ApprovalWorkflow/5
        [HttpDelete("{id:int}")]
        [RequirePermission("ApprovalWorkflow_Allow_Delete")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var result = await _workflowService.DeleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        // POST: api/ApprovalWorkflow/5/ensure-default-stage
        // يتأكد من وجود Stage افتراضي للـ Workflow
        [HttpPost("{id:int}/ensure-default-stage")]
        [RequirePermission("ApprovalWorkflow_Allow_Manage")]
        public async Task<IActionResult> EnsureDefaultStage(int id, CancellationToken cancellationToken = default)
        {
            var result = await _workflowService.EnsureDefaultStageAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        // POST: api/ApprovalWorkflow/fix-all-without-stages
        // يُصلح جميع الـ Workflows التي ليس لها stages
        [HttpPost("fix-all-without-stages")]
        public async Task<IActionResult> FixAllWorkflowsWithoutStages(CancellationToken cancellationToken = default)
        {
            var result = await _workflowService.FixAllWorkflowsWithoutStagesAsync(cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}
