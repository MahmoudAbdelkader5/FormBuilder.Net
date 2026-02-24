using FormBuilder.API.Attributes;
using FormBuilder.API.Models.DTOs;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: api/projects
        [HttpGet]
        [RequirePermission("Project_Allow_View")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var result = await _projectService.GetPagedAsync(page, pageSize);
            return result.ToActionResult();
        }

        // GET: api/projects/5
        [HttpGet("{id}")]
        [RequirePermission("Project_Allow_View")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
        {
            var result = await _projectService.GetByIdAsync(id);
            return result.ToActionResult();
        }

        // GET: api/projects/code/PROJ001
        [HttpGet("code/{code}")]
        [RequirePermission("Project_Allow_View")]
        public async Task<IActionResult> GetByCode(string code, CancellationToken cancellationToken = default)
        {
            var result = await _projectService.GetByCodeAsync(code);
            return result.ToActionResult();
        }

        // GET: api/projects/active
        [HttpGet("active")]
        [RequirePermission("Project_Allow_View")]
        public async Task<IActionResult> GetActive(CancellationToken cancellationToken = default)
        {
            var result = await _projectService.GetActiveAsync();
            return result.ToActionResult();
        }

        // POST: api/projects
        [HttpPost]
        [RequirePermission("Project_Allow_Create")]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto createDto, CancellationToken cancellationToken = default)
        {
            var result = await _projectService.CreateAsync(createDto);
            if (result.Success && result.Data != null)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
            }
            return result.ToActionResult();
        }

        // PUT: api/projects/5
        [HttpPut("{id}")]
        [RequirePermission("Project_Allow_Edit")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectDto updateDto, CancellationToken cancellationToken = default)
        {
            var result = await _projectService.UpdateAsync(id, updateDto);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }

        // DELETE: api/projects/5
        [HttpDelete("{id}")]
        [RequirePermission("Project_Allow_Delete")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var result = await _projectService.DeleteAsync(id);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }


        // GET: api/projects/5/exists
        [HttpGet("{id}/exists")]
        [RequirePermission("Project_Allow_View")]
        public async Task<IActionResult> Exists(int id, CancellationToken cancellationToken = default)
        {
            var result = await _projectService.ExistsAsync(id);
            return result.ToActionResult();
        }

        // GET: api/projects/code/PROJ001/exists
        [HttpGet("code/{code}/exists")]
        [RequirePermission("Project_Allow_View")]
        public async Task<IActionResult> CodeExists(string code, [FromQuery] int? excludeId = null, CancellationToken cancellationToken = default)
        {
            var result = await _projectService.CodeExistsAsync(code, excludeId);
            return result.ToActionResult();
        }
    }
}