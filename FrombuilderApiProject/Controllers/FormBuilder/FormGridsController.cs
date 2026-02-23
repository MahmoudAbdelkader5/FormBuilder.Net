using FormBuilder.API.Attributes;
using FormBuilder.API.DTOs;
using FormBuilder.API.Models;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FormGridsController : ControllerBase
    {
        private readonly IFormGridService _formGridService;

        public FormGridsController(IFormGridService formGridService)
        {
            _formGridService = formGridService;
        }

        /// <summary>
        /// Get all form grids
        /// </summary>
        [HttpGet]
        [RequirePermission("Grid_Allow_View")]
        public async Task<ActionResult<ApiResponse>> GetAll()
        {
            var response = await _formGridService.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get form grid by ID
        /// </summary>
        /// <param name="id">Form grid ID</param>
        [HttpGet("{id}")]
        [RequirePermission("Grid_Allow_View")]
        public async Task<ActionResult<ApiResponse>> GetById(int id)
        {
            var response = await _formGridService.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get form grids by form builder ID
        /// </summary>
        /// <param name="formBuilderId">Form builder ID</param>
        [HttpGet("by-form-builder/{formBuilderId}")]
        [RequirePermission("Grid_Allow_View")]
        public async Task<ActionResult<ApiResponse>> GetByFormBuilderId(int formBuilderId)
        {
            var response = await _formGridService.GetByFormBuilderIdAsync(formBuilderId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get form grids by tab ID
        /// </summary>
        /// <param name="tabId">Tab ID</param>
        [HttpGet("by-tab/{tabId}")]
        [RequirePermission("Grid_Allow_View")]
        public async Task<ActionResult<ApiResponse>> GetByTabId(int tabId)
        {
            var response = await _formGridService.GetByTabIdAsync(tabId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get active form grids by form builder ID
        /// </summary>
        /// <param name="formBuilderId">Form builder ID</param>
        [HttpGet("active-by-form-builder/{formBuilderId}")]
        [RequirePermission("Grid_Allow_View")]
        public async Task<ActionResult<ApiResponse>> GetActiveByFormBuilderId(int formBuilderId)
        {
            var response = await _formGridService.GetActiveByFormBuilderIdAsync(formBuilderId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get form grid by code
        /// </summary>
        /// <param name="gridCode">Grid code</param>
        /// <param name="formBuilderId">Form builder ID</param>
        [HttpGet("by-code/{gridCode}/{formBuilderId}")]
        [RequirePermission("Grid_Allow_View")]
        public async Task<ActionResult<ApiResponse>> GetByGridCode(string gridCode, int formBuilderId)
        {
            var response = await _formGridService.GetByGridCodeAsync(gridCode, formBuilderId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Create new form grid
        /// </summary>
        /// <param name="createDto">Form grid data</param>
        [HttpPost]
        [RequirePermission("Grid_Allow_Create")]
        public async Task<ActionResult<ApiResponse>> Create([FromBody] CreateFormGridDto createDto)
        {
            var response = await _formGridService.CreateAsync(createDto);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Update existing form grid
        /// </summary>
        /// <param name="id">Form grid ID</param>
        /// <param name="updateDto">Updated form grid data</param>
        [HttpPut("{id}")]
        [RequirePermission("Grid_Allow_Edit")]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] UpdateFormGridDto updateDto)
        {
            var response = await _formGridService.UpdateAsync(id, updateDto);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Delete form grid
        /// </summary>
        /// <param name="id">Form grid ID</param>
        [HttpDelete("{id}")]
        [RequirePermission("Grid_Allow_Delete")]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            var response = await _formGridService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Toggle form grid active status
        /// </summary>
        /// <param name="id">Form grid ID</param>
        /// <param name="isActive">Active status</param>
        [HttpPatch("{id}/toggle-active")]
        [RequirePermission("Grid_Allow_Manage")]
        public async Task<ActionResult<ApiResponse>> ToggleActive(int id, [FromBody] bool isActive)
        {
            var response = await _formGridService.ToggleActiveAsync(id, isActive);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Check if form grid exists
        /// </summary>
        /// <param name="id">Form grid ID</param>
        [HttpGet("exists/{id}")]
        [RequirePermission("Grid_Allow_View")]
        public async Task<ActionResult<ApiResponse>> Exists(int id)
        {
            var response = await _formGridService.ExistsAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Check if grid code exists
        /// </summary>
        /// <param name="gridCode">Grid code</param>
        /// <param name="formBuilderId">Form builder ID</param>
        /// <param name="excludeId">Exclude ID (optional)</param>
        [HttpGet("code-exists/{gridCode}/{formBuilderId}")]
        [RequirePermission("Grid_Allow_View")]
        public async Task<ActionResult<ApiResponse>> CodeExists(string gridCode, int formBuilderId, [FromQuery] int? excludeId = null)
        {
            var response = await _formGridService.GridCodeExistsAsync(gridCode, formBuilderId, excludeId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get next grid order
        /// </summary>
        /// <param name="formBuilderId">Form builder ID</param>
        /// <param name="tabId">Tab ID (optional)</param>
        [HttpGet("next-order/{formBuilderId}")]
        [RequirePermission("Grid_Allow_View")]
        public async Task<ActionResult<ApiResponse>> GetNextGridOrder(int formBuilderId, [FromQuery] int? tabId = null)
        {
            var response = await _formGridService.GetNextGridOrderAsync(formBuilderId, tabId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Copy form grid with all columns
        /// </summary>
        /// <param name="copyDto">Copy grid data</param>
        [HttpPost("copy")]
        [RequirePermission("Grid_Allow_Create")]
        public async Task<ActionResult<ApiResponse>> Copy([FromBody] CopyFormGridDto copyDto)
        {
            var response = await _formGridService.CopyAsync(copyDto);
            return StatusCode(response.StatusCode, response);
        }
    }
}