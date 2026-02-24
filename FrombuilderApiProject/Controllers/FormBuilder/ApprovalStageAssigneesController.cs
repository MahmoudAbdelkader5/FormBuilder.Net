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
    [Authorize]
    public class ApprovalStageAssigneesController : ControllerBase
    {
        private readonly IApprovalStageAssigneesService _service;

        public ApprovalStageAssigneesController(IApprovalStageAssigneesService service)
        {
            _service = service;
        }

        // GET: api/ApprovalStageAssignees/stage/1
        [HttpGet("stage/{stageId}")]
        [RequirePermission("ApprovalStageAssignee_Allow_View")]
        public async Task<IActionResult> GetByStageId(int stageId, CancellationToken cancellationToken = default)
        {
            var response = await _service.GetByStageIdAsync(stageId);
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/ApprovalStageAssignees/5
        [HttpGet("{id}")]
        [RequirePermission("ApprovalStageAssignee_Allow_View")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
        {
            var response = await _service.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        // POST: api/ApprovalStageAssignees
        [HttpPost]
        [RequirePermission("ApprovalStageAssignee_Allow_Create")]
        public async Task<IActionResult> Create([FromBody] ApprovalStageAssigneesCreateDto dto, CancellationToken cancellationToken = default)
        {
            var response = await _service.CreateAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        // PUT: api/ApprovalStageAssignees/5
        [HttpPut("{id}")]
        [RequirePermission("ApprovalStageAssignee_Allow_Edit")]
        public async Task<IActionResult> Update(int id, [FromBody] ApprovalStageAssigneesUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var response = await _service.UpdateAsync(id, dto);
            return StatusCode(response.StatusCode, response);
        }

        // POST: api/ApprovalStageAssignees/bulk-update
        [HttpPost("bulk-update")]
        [RequirePermission("ApprovalStageAssignee_Allow_Manage")]
        public async Task<IActionResult> BulkUpdate([FromBody] StageAssigneesBulkDto dto, CancellationToken cancellationToken = default)
        {
            var response = await _service.BulkUpdateAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        // DELETE: api/ApprovalStageAssignees/5
        [HttpDelete("{id}")]
        [RequirePermission("ApprovalStageAssignee_Allow_Delete")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var response = await _service.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        // POST: api/ApprovalStageAssignees/update-missing-role-ids
        [HttpPost("update-missing-role-ids")]
        [RequirePermission("ApprovalStageAssignee_Allow_Manage")]
        public async Task<IActionResult> UpdateMissingRoleIds(CancellationToken cancellationToken = default)
        {
            var response = await _service.UpdateMissingRoleIdsAsync();
            return StatusCode(response.StatusCode, response);
        }
    }
}

