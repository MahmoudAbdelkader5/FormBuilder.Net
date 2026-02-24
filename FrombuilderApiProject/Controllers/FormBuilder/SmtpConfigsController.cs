using FormBuilder.API.Attributes;
using FormBuilder.API.Extensions;
using FormBuilder.API.Models.DTOs;
using FormBuilder.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SmtpConfigsController : ControllerBase
    {
        private readonly ISmtpConfigsService _service;

        public SmtpConfigsController(ISmtpConfigsService service)
        {
            _service = service;
        }

        [HttpGet]
        [RequirePermission("SmtpConfig_Allow_View")]
        public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = true, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetAllAsync(includeInactive);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        [RequirePermission("SmtpConfig_Allow_View")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpPost]
        [RequirePermission("SmtpConfig_Allow_Create")]
        public async Task<IActionResult> Create([FromBody] CreateSmtpConfigDto dto, CancellationToken cancellationToken = default)
        {
            var result = await _service.CreateAsync(dto);
            if (result.Success && result.Data != null)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
            }
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [RequirePermission("SmtpConfig_Allow_Edit")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSmtpConfigDto dto, CancellationToken cancellationToken = default)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [RequirePermission("SmtpConfig_Allow_Delete")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var result = await _service.DeleteAsync(id);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }

        [HttpPatch("{id}/activate")]
        [RequirePermission("SmtpConfig_Allow_Manage")]
        public async Task<IActionResult> Activate(int id, CancellationToken cancellationToken = default)
        {
            var result = await _service.ToggleActiveAsync(id, true);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }

        [HttpPatch("{id}/deactivate")]
        [RequirePermission("SmtpConfig_Allow_Manage")]
        public async Task<IActionResult> Deactivate(int id, CancellationToken cancellationToken = default)
        {
            var result = await _service.ToggleActiveAsync(id, false);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }
    }
}


