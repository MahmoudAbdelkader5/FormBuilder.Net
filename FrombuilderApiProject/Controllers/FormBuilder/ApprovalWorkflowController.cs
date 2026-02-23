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
        public async Task<IActionResult> GetAll()
        {
            var result = await _workflowService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        // GET: api/ApprovalWorkflow/5
        [HttpGet("{id:int}")]
        [RequirePermission("ApprovalWorkflow_Allow_View")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _workflowService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        // POST: api/ApprovalWorkflow
        [HttpPost]
        [RequirePermission("ApprovalWorkflow_Allow_Create")]
        public async Task<IActionResult> Create([FromBody] ApprovalWorkflowCreateDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse(400, "Invalid request"));

            var result = await _workflowService.CreateAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        // PUT: api/ApprovalWorkflow/5
        [HttpPut("{id:int}")]
        [RequirePermission("ApprovalWorkflow_Allow_Edit")]
        public async Task<IActionResult> Update(int id, [FromBody] ApprovalWorkflowUpdateDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse(400, "Invalid request"));

            var result = await _workflowService.UpdateAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        // PATCH: api/ApprovalWorkflow/5/toggle
        [HttpPatch("{id:int}/toggle")]
        [RequirePermission("ApprovalWorkflow_Allow_Manage")]
        public async Task<IActionResult> ToggleActive(int id, [FromQuery] bool isActive)
        {
            var result = await _workflowService.ToggleActiveAsync(id, isActive);
            return StatusCode(result.StatusCode, result);
        }

        // GET: api/ApprovalWorkflow/name/{name}
        [HttpGet("name/{name}")]
        [RequirePermission("ApprovalWorkflow_Allow_View")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _workflowService.GetByNameAsync(name);
            return StatusCode(result.StatusCode, result);
        }

        // GET: api/ApprovalWorkflow/active
        [HttpGet("active")]
        [RequirePermission("ApprovalWorkflow_Allow_View")]
        public async Task<IActionResult> GetActive()
        {
            var result = await _workflowService.GetActiveAsync();
            return StatusCode(result.StatusCode, result);
        }
        // DELETE: api/ApprovalWorkflow/5
        [HttpDelete("{id:int}")]
        [RequirePermission("ApprovalWorkflow_Allow_Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _workflowService.DeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        // POST: api/ApprovalWorkflow/5/ensure-default-stage
        // يتأكد من وجود Stage افتراضي للـ Workflow
        [HttpPost("{id:int}/ensure-default-stage")]
        [RequirePermission("ApprovalWorkflow_Allow_Manage")]
        public async Task<IActionResult> EnsureDefaultStage(int id)
        {
            var result = await _workflowService.EnsureDefaultStageAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        // POST: api/ApprovalWorkflow/fix-all-without-stages
        // يُصلح جميع الـ Workflows التي ليس لها stages
        [HttpPost("fix-all-without-stages")]
        public async Task<IActionResult> FixAllWorkflowsWithoutStages()
        {
            var result = await _workflowService.FixAllWorkflowsWithoutStagesAsync();
            return StatusCode(result.StatusCode, result);
        }
    }
}
