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
    public class EmailTemplatesController : ControllerBase
    {
        private readonly IEmailTemplatesService _service;

        public EmailTemplatesController(IEmailTemplatesService service)
        {
            _service = service;
        }

        [HttpGet]
        [RequirePermission("EmailTemplate_Allow_View")]
        public async Task<IActionResult> GetAll([FromQuery] int? documentTypeId = null, [FromQuery] bool includeInactive = true)
        {
            var result = await _service.GetAllAsync(documentTypeId, includeInactive);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        [RequirePermission("EmailTemplate_Allow_View")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpPost]
        [RequirePermission("EmailTemplate_Allow_Create")]
        public async Task<IActionResult> Create([FromBody] CreateEmailTemplateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            if (result.Success && result.Data != null)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
            }
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [RequirePermission("EmailTemplate_Allow_Edit")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmailTemplateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [RequirePermission("EmailTemplate_Allow_Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }

        [HttpPatch("{id}/activate")]
        [RequirePermission("EmailTemplate_Allow_Manage")]
        public async Task<IActionResult> Activate(int id)
        {
            var result = await _service.ToggleActiveAsync(id, true);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }

        [HttpPatch("{id}/deactivate")]
        [RequirePermission("EmailTemplate_Allow_Manage")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await _service.ToggleActiveAsync(id, false);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }
    }
}


