using FormBuilder.API.Attributes;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Security.Claims; // REQUIRED for accessing claims/user ID
using System.Threading.Tasks;

namespace FormBuilder.ApiProject.Controllers.FormBuilder
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class FormBuilderController : ControllerBase
    {
        private readonly IFormBuilderService _formBuilderService;
        private readonly IStringLocalizer<FormBuilderController> _localizer;

        public FormBuilderController(IFormBuilderService formBuilderService,
                                     IStringLocalizer<FormBuilderController> localizer)
        {
            _formBuilderService = formBuilderService;
            _localizer = localizer;
        }

        // --- GET Operations (Read) ---
        [HttpGet]
        [RequirePermission("FormBuilder_Allow_View")]
        [ProducesResponseType(typeof(IEnumerable<FormBuilderDto>), 200)]
        public async Task<IActionResult> GetAllForms([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _formBuilderService.GetPagedAsync(page, pageSize);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        [RequirePermission("FormBuilder_Allow_View")]
        [ProducesResponseType(typeof(FormBuilderDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetFormById(int id)
        {
            var result = await _formBuilderService.GetByIdAsync(id, asNoTracking: true);
            return result.ToActionResult();
        }

        [HttpGet("code/{formCode}")]
        [ProducesResponseType(typeof(FormBuilderDto), 200)]
        [ProducesResponseType(404)]
        [AllowAnonymous]

        public async Task<IActionResult> GetFormByCode(string formCode)
        {
            var result = await _formBuilderService.GetByCodeAsync(formCode, asNoTracking: true);
            return result.ToActionResult();
        }

        // --- POST Operation (Create) ---
        [HttpPost]
        [RequirePermission("FormBuilder_Allow_Create")]
        [ProducesResponseType(typeof(FormBuilderDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> CreateForm([FromBody] CreateFormBuilderDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Successfully retrieving authenticated user's ID
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new { message = _localizer["FormBuilder_UserIdNotFound"] });
            }

            createDto.CreatedByUserId = currentUserId;
            var result = await _formBuilderService.CreateAsync(createDto);
            if (result.Success && result.Data != null)
            {
                return CreatedAtAction(nameof(GetFormById), new { id = result.Data.Id }, result.Data);
            }

            return result.ToActionResult();
        }

        // --- PUT Operation (Update) ---
        [HttpPut("{id}")]
        [RequirePermission("FormBuilder_Allow_Edit")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> UpdateForm(int id, [FromBody] UpdateFormBuilderDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _formBuilderService.UpdateAsync(id, updateDto);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }

        // --- DELETE Operation ---
        [HttpDelete("{id}")]
        [RequirePermission("FormBuilder_Allow_Delete")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteForm(int id)
        {
            var result = await _formBuilderService.DeleteAsync(id);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }

        // --- PATCH Operation (Toggle Active) ---
        [HttpPatch("{id}/toggle-active")]
        [RequirePermission("FormBuilder_Allow_Manage")]
        [ProducesResponseType(typeof(FormBuilderDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ToggleActive(int id, [FromBody] bool isActive)
        {
            var result = await _formBuilderService.ToggleActiveAsync(id, isActive);
            return result.ToActionResult();
        }

        // --- POST Operation (Restore Soft-Deleted Form) ---
        [HttpPost("{id}/restore")]
        [RequirePermission("FormBuilder_Allow_Manage")]
        [ProducesResponseType(typeof(FormBuilderDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RestoreForm(int id)
        {
            var result = await _formBuilderService.RestoreAsync(id);
            return result.ToActionResult();
        }

        // --- POST Operation (Duplicate) ---
        [HttpPost("{id}/duplicate")]
        [RequirePermission("FormBuilder_Allow_Create")]
        [ProducesResponseType(typeof(FormBuilderDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> DuplicateForm(int id, [FromBody] DuplicateFormRequestDto? requestDto = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newFormCode = requestDto?.NewFormCode;
            var newFormName = requestDto?.NewFormName;

            var result = await _formBuilderService.DuplicateAsync(id, newFormCode, newFormName);
            
            if (result.Success && result.Data != null)
            {
                return CreatedAtAction(nameof(GetFormById), new { id = result.Data.Id }, result.Data);
            }

            return result.ToActionResult();
        }
    }

    // DTO for duplicate request
    public class DuplicateFormRequestDto
    {
        public string? NewFormCode { get; set; }
        public string? NewFormName { get; set; }
    }
}