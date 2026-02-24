using FormBuilder.API.Attributes;
using FormBuilder.API.Extensions;
using FormBuilder.API.Models;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Core.IServices.FormBuilder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SapHanaConfigsController : ControllerBase
    {
        private readonly ISapHanaConfigsService _configsService;

        public SapHanaConfigsController(ISapHanaConfigsService configsService)
        {
            _configsService = configsService;
        }

        [HttpGet]
        [RequirePermission("SapHanaConfig_Allow_View")]
        public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = true, CancellationToken cancellationToken = default)
        {
            var result = await _configsService.GetAllAsync(includeInactive);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        [RequirePermission("SapHanaConfig_Allow_View")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
        {
            var result = await _configsService.GetByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpPost]
        [RequirePermission("SapHanaConfig_Allow_Create")]
        public async Task<IActionResult> Create([FromBody] CreateSapHanaConfigDto dto, CancellationToken cancellationToken = default)
        {
            var result = await _configsService.CreateAsync(dto);
            if (result.Success && result.Data != null)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
            }
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [RequirePermission("SapHanaConfig_Allow_Edit")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSapHanaConfigDto dto, CancellationToken cancellationToken = default)
        {
            var result = await _configsService.UpdateAsync(id, dto);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [RequirePermission("SapHanaConfig_Allow_Delete")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var result = await _configsService.DeleteAsync(id);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }

        [HttpPatch("{id}/activate")]
        [RequirePermission("SapHanaConfig_Allow_Manage")]
        public async Task<IActionResult> Activate(int id, CancellationToken cancellationToken = default)
        {
            var result = await _configsService.ToggleActiveAsync(id, true);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }

        [HttpPatch("{id}/deactivate")]
        [RequirePermission("SapHanaConfig_Allow_Manage")]
        public async Task<IActionResult> Deactivate(int id, CancellationToken cancellationToken = default)
        {
            var result = await _configsService.ToggleActiveAsync(id, false);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }

        /// <summary>
        /// Store SAP HANA connection string in DB (encrypted) and mark it as active.
        /// Backward-compatible endpoint (kept for existing clients).
        /// </summary>
        [HttpPost("set-active")]
        [RequirePermission("SapHanaConfig_Allow_Create")]
        public async Task<ActionResult<ApiResponse>> SetActive([FromBody] CreateSapHanaConfigDto request)
        {
            request.IsActive = true;
            var result = await _configsService.CreateAsync(request);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new ApiResponse(result.StatusCode, result.ErrorMessage));
            }

            return Ok(new ApiResponse(200, "SAP HANA config saved and activated", new { id = result.Data?.Id }));
        }

        /// <summary>
        /// Checks if an active SAP HANA connection exists in DB (does not return the secret).
        /// </summary>
        [HttpGet("has-active")]
        [RequirePermission("SapHanaConfig_Allow_View")]
        public async Task<ActionResult<ApiResponse>> HasActive()
        {
            var cs = await _configsService.GetActiveConnectionStringAsync();
            return Ok(new ApiResponse(200, "OK", new { hasActive = !string.IsNullOrWhiteSpace(cs) }));
        }
    }
}


