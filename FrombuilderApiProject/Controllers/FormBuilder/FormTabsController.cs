using FormBuilder.API.Attributes;
using FormBuilder.Core.DTOS.FormTabs;
using FormBuilder.Services.Services;
using FormBuilder.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FormBuilder.ApiProject.Controllers.FormBuilder
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FormTabsController : ControllerBase
    {
        private readonly IFormTabService _formTabService;
        private readonly IStringLocalizer<FormTabsController> _localizer;

        public FormTabsController(IFormTabService formTabService,
                                  IStringLocalizer<FormTabsController> localizer)
        {
            _formTabService = formTabService ?? throw new ArgumentNullException(nameof(formTabService));
            _localizer = localizer;
        }

        // GET: api/FormTabs
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FormTabDto>), 200)]
        public async Task<IActionResult> GetAllTabs([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _formTabService.GetPagedAsync(page, pageSize);
            return result.ToActionResult();
        }

        // GET: api/FormTabs/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FormTabDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTabById(int id)
        {
            var result = await _formTabService.GetByIdAsync(id, asNoTracking: true);
            return result.ToActionResult();
        }

        // GET: api/FormTabs/form/{formBuilderId}
        [HttpGet("form/{formBuilderId}")]
        [ProducesResponseType(typeof(IEnumerable<FormTabDto>), 200)]
        public async Task<IActionResult> GetTabsByFormId(int formBuilderId)
        {
            var result = await _formTabService.GetByFormIdAsync(formBuilderId);
            return result.ToActionResult();
        }

        // GET: api/FormTabs/code/{tabCode}
        [HttpGet("code/{tabCode}")]
        [ProducesResponseType(typeof(FormTabDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTabByCode(string tabCode)
        {
            var result = await _formTabService.GetByCodeAsync(tabCode, asNoTracking: true);
            return result.ToActionResult();
        }

        // POST: api/FormTabs
        [HttpPost]
        [RequirePermission("FormTab_Allow_Create")]
        [ProducesResponseType(typeof(FormTabDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> CreateTab([FromBody] CreateFormTabDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized(new { message = _localizer["FormTabs_UserIdNotFound"] });

            createDto.CreatedByUserId = currentUserId;
            var result = await _formTabService.CreateAsync(createDto);
            
            if (result.Success && result.Data != null)
            {
                return CreatedAtAction(nameof(GetTabById), new { id = result.Data.Id }, result.Data);
            }
            
            return result.ToActionResult();
        }

        // PUT: api/FormTabs/{id}
        [HttpPut("{id}")]
        [RequirePermission("FormTab_Allow_Edit")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> UpdateTab(int id, [FromBody] UpdateFormTabDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _formTabService.UpdateAsync(id, updateDto);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }

        // DELETE: api/FormTabs/{id}
        [HttpDelete("{id}")]
        [RequirePermission("FormTab_Allow_Delete")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTab(int id)
        {
            var result = await _formTabService.DeleteAsync(id);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }

        // PATCH: api/FormTabs/{id}/toggle-active
        [HttpPatch("{id}/toggle-active")]
        [RequirePermission("FormTab_Allow_Manage")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ToggleActive(int id, [FromBody] bool isActive)
        {
            var result = await _formTabService.ToggleActiveAsync(id, isActive);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }

        // POST: api/FormTabs/{id}/restore
        [HttpPost("{id}/restore")]
        [RequirePermission("FormTab_Allow_Manage")]
        [ProducesResponseType(typeof(FormTabDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RestoreTab(int id)
        {
            var result = await _formTabService.RestoreAsync(id);
            return result.ToActionResult();
        }

        // GET: api/FormTabs/check-code/{formBuilderId}/{tabCode}
        [HttpGet("check-code/{formBuilderId}/{tabCode}")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> CheckTabCodeUnique(int formBuilderId, string tabCode, [FromQuery] int? ignoreId = null)
        {
            var result = await _formTabService.CodeExistsAsync(formBuilderId, tabCode, ignoreId);
            if (result.Success)
            {
                return Ok(new
                {
                    tabCode,
                    exists = result.Data,
                    message = result.Data
                        ? _localizer["FormTabs_TabCodeExists"]
                        : _localizer["FormTabs_TabCodeAvailable"]
                });
            }
            return result.ToActionResult();
        }

        // GET: api/FormTabs/{id}/exists
        [HttpGet("{id}/exists")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> TabExists(int id)
        {
            var result = await _formTabService.ExistsAsync(id);
            if (result.Success)
            {
                return Ok(new
                {
                    id,
                    exists = result.Data,
                    message = result.Data
                        ? _localizer["FormTabs_TabExists"]
                        : _localizer["FormTabs_TabNotExists"]
                });
            }
            return result.ToActionResult();
        }

        // GET: api/FormTabs/deleted
        [HttpGet("deleted")]
        [ProducesResponseType(typeof(IEnumerable<FormTabDto>), 200)]
        public async Task<IActionResult> GetDeletedTabs([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _formTabService.GetPagedDeletedAsync(page, pageSize);
            return result.ToActionResult();
        }

        // GET: api/FormTabs/all-including-deleted
        [HttpGet("all-including-deleted")]
        [ProducesResponseType(typeof(IEnumerable<FormTabDto>), 200)]
        public async Task<IActionResult> GetAllTabsIncludingDeleted()
        {
            var result = await _formTabService.GetAllIncludingDeletedAsync();
            return result.ToActionResult();
        }
    }
}
