using FormBuilder.API.Attributes;
using FormBuilder.API.Models;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AlertRulesController : ControllerBase
    {
        private readonly IAlertRuleService _alertRuleService;

        public AlertRulesController(IAlertRuleService alertRuleService)
        {
            _alertRuleService = alertRuleService;
        }

        /// <summary>
        /// Get all alert rules
        /// </summary>
        [HttpGet]
        [RequirePermission("AlertRule_Allow_View")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _alertRuleService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get alert rule by ID
        /// </summary>
        [HttpGet("{id}")]
        [RequirePermission("AlertRule_Allow_View")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _alertRuleService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get alert rules by document type ID
        /// </summary>
        [HttpGet("document-type/{documentTypeId}")]
        [RequirePermission("AlertRule_Allow_View")]
        public async Task<IActionResult> GetByDocumentTypeId(int documentTypeId)
        {
            var result = await _alertRuleService.GetByDocumentTypeIdAsync(documentTypeId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get alert rules by trigger type
        /// </summary>
        [HttpGet("trigger-type/{triggerType}")]
        [RequirePermission("AlertRule_Allow_View")]
        public async Task<IActionResult> GetByTriggerType(string triggerType)
        {
            var result = await _alertRuleService.GetByTriggerTypeAsync(triggerType);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get active alert rules by document type and trigger type
        /// </summary>
        [HttpGet("active")]
        [RequirePermission("AlertRule_Allow_View")]
        public async Task<IActionResult> GetActiveByDocumentTypeAndTrigger([FromQuery] int documentTypeId, [FromQuery] string triggerType)
        {
            var result = await _alertRuleService.GetActiveByDocumentTypeAndTriggerAsync(documentTypeId, triggerType);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Create a new alert rule
        /// </summary>
        [HttpPost]
        [RequirePermission("AlertRule_Allow_Create")]
        public async Task<IActionResult> Create([FromBody] CreateAlertRuleDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", ModelState));

            var result = await _alertRuleService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Update an existing alert rule
        /// </summary>
        [HttpPut("{id}")]
        [RequirePermission("AlertRule_Allow_Edit")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAlertRuleDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", ModelState));

            var result = await _alertRuleService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Delete an alert rule (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [RequirePermission("AlertRule_Allow_Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _alertRuleService.DeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Activate an alert rule
        /// </summary>
        [HttpPatch("{id}/activate")]
        [RequirePermission("AlertRule_Allow_Manage")]
        public async Task<IActionResult> Activate(int id)
        {
            var result = await _alertRuleService.ActivateAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Deactivate an alert rule
        /// </summary>
        [HttpPatch("{id}/deactivate")]
        [RequirePermission("AlertRule_Allow_Manage")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await _alertRuleService.DeactivateAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}

