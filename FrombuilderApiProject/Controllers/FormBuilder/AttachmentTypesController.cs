using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Administration")]

    public class AttachmentTypesController : ControllerBase
    {
        private readonly IAttachmentTypeService _attachmentTypeService;

        public AttachmentTypesController(IAttachmentTypeService attachmentTypeService)
        {
            _attachmentTypeService = attachmentTypeService;
        }

        // GET: api/attachmenttypes
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _attachmentTypeService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        // GET: api/attachmenttypes/active
        [HttpGet("active")]
        public async Task<IActionResult> GetActive(CancellationToken cancellationToken = default)
        {
            var result = await _attachmentTypeService.GetActiveAsync();
            return StatusCode(result.StatusCode, result);
        }

        // GET: api/attachmenttypes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
        {
            var result = await _attachmentTypeService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        // GET: api/attachmenttypes/code/DOCUMENT
        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetByCode(string code, CancellationToken cancellationToken = default)
        {
            var result = await _attachmentTypeService.GetByCodeAsync(code);
            return StatusCode(result.StatusCode, result);
        }

        // POST: api/attachmenttypes
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAttachmentTypeDto createDto, CancellationToken cancellationToken = default)
        {
            var result = await _attachmentTypeService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        // PUT: api/attachmenttypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAttachmentTypeDto updateDto, CancellationToken cancellationToken = default)
        {
            var result = await _attachmentTypeService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        // DELETE: api/attachmenttypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var result = await _attachmentTypeService.DeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        // PATCH: api/attachmenttypes/5/toggle-active
        [HttpPatch("{id}/toggle-active")]
        public async Task<IActionResult> ToggleActive(int id, [FromBody] ToggleActiveDto toggleDto, CancellationToken cancellationToken = default)
        {
            var result = await _attachmentTypeService.ToggleActiveAsync(id, toggleDto.IsActive);
            return StatusCode(result.StatusCode, result);
        }

        // GET: api/attachmenttypes/5/exists
        [HttpGet("{id}/exists")]
        public async Task<IActionResult> Exists(int id, CancellationToken cancellationToken = default)
        {
            var result = await _attachmentTypeService.ExistsAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}