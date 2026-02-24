using FormBuilder.API.Attributes;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.API.Models;
using FormBuilder.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DocumentSeriesController : ControllerBase
    {
        private readonly IDocumentSeriesService _documentSeriesService;
        private readonly IDocumentNumberGeneratorService _documentNumberGeneratorService;

        public DocumentSeriesController(
            IDocumentSeriesService documentSeriesService,
            IDocumentNumberGeneratorService documentNumberGeneratorService)
        {
            _documentSeriesService = documentSeriesService;
            _documentNumberGeneratorService = documentNumberGeneratorService;
        }

        // ================================
        // GET ALL DOCUMENT SERIES
        // ================================
        [HttpGet]
        [RequirePermission("DocumentSeries_Allow_View")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _documentSeriesService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // GET BY ID
        // ================================
        [HttpGet("{id}")]
        [RequirePermission("DocumentSeries_Allow_View")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
        {
            var result = await _documentSeriesService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // GET BY SERIES CODE
        // ================================
        [HttpGet("code/{seriesCode}")]
        [RequirePermission("DocumentSeries_Allow_View")]
        public async Task<IActionResult> GetBySeriesCode(string seriesCode, CancellationToken cancellationToken = default)
        {
            var result = await _documentSeriesService.GetBySeriesCodeAsync(seriesCode);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // GET BY DOCUMENT TYPE ID
        // ================================
        [HttpGet("document-type/{documentTypeId}")]
        [RequirePermission("DocumentSeries_Allow_View")]
        public async Task<IActionResult> GetByDocumentTypeId(int documentTypeId, CancellationToken cancellationToken = default)
        {
            var result = await _documentSeriesService.GetByDocumentTypeIdAsync(documentTypeId);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // GET BY PROJECT ID
        // ================================
        [HttpGet("project/{projectId}")]
        [RequirePermission("DocumentSeries_Allow_View")]
        public async Task<IActionResult> GetByProjectId(int projectId, CancellationToken cancellationToken = default)
        {
            var result = await _documentSeriesService.GetByProjectIdAsync(projectId);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // GET ACTIVE SERIES
        // ================================
        [HttpGet("active")]
        [RequirePermission("DocumentSeries_Allow_View")]
        public async Task<IActionResult> GetActive(CancellationToken cancellationToken = default)
        {
            var result = await _documentSeriesService.GetActiveAsync();
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // GET DEFAULT SERIES
        // ================================
        [HttpGet("default")]
        [RequirePermission("DocumentSeries_Allow_View")]
        public async Task<IActionResult> GetDefaultSeries([FromQuery] int documentTypeId, [FromQuery] int projectId, CancellationToken cancellationToken = default)
        {
            var result = await _documentSeriesService.GetDefaultSeriesAsync(documentTypeId, projectId);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // CREATE NEW DOCUMENT SERIES
        // ================================
        [HttpPost]
        [RequirePermission("DocumentSeries_Allow_Create")]
        public async Task<IActionResult> Create([FromBody] CreateDocumentSeriesDto createDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", ModelState));

            var result = await _documentSeriesService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // UPDATE DOCUMENT SERIES
        // ================================
        [HttpPut("{id}")]
        [RequirePermission("DocumentSeries_Allow_Edit")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDocumentSeriesDto updateDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", ModelState));

            var result = await _documentSeriesService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // DELETE DOCUMENT SERIES
        // ================================
        [HttpDelete("{id}")]
        [RequirePermission("DocumentSeries_Allow_Delete")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var result = await _documentSeriesService.DeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // TOGGLE ACTIVE STATUS
        // ================================
        [HttpPatch("{id}/toggle-active")]
        [RequirePermission("DocumentSeries_Allow_Manage")]
        public async Task<IActionResult> ToggleActive(int id, [FromBody] bool isActive, CancellationToken cancellationToken = default)
        {
            var result = await _documentSeriesService.ToggleActiveAsync(id, isActive);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // SET AS DEFAULT SERIES
        // ================================
        [HttpPatch("{id}/set-default")]
        [RequirePermission("DocumentSeries_Allow_Manage")]
        public async Task<IActionResult> SetAsDefault(int id, CancellationToken cancellationToken = default)
        {
            var result = await _documentSeriesService.SetAsDefaultAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // GET NEXT NUMBER
        // ================================
        [HttpGet("{id}/next-number")]
        [RequirePermission("DocumentSeries_Allow_View")]
        public async Task<IActionResult> GetNextNumber(int id, CancellationToken cancellationToken = default)
        {
            var result = await _documentSeriesService.GetNextNumberAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // CHECK EXISTS
        // ================================
        [HttpGet("{id}/exists")]
        [RequirePermission("DocumentSeries_Allow_View")]
        public async Task<IActionResult> Exists(int id, CancellationToken cancellationToken = default)
        {
            var result = await _documentSeriesService.ExistsAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // TEST GENERATE NUMBER (MUTATES DATA)
        // ================================
        [HttpPost("test-generate/{submissionId}")]
        [RequirePermission("DocumentSeries_Allow_Manage")]
        public async Task<IActionResult> TestGenerateNumber(int submissionId,
            [FromQuery] string generatedOn = "Submit",
            [FromQuery] string? generatedByUserId = null, CancellationToken cancellationToken = default)
        {
            var generationResult = await _documentNumberGeneratorService
                .GenerateForSubmissionAsync(submissionId, generatedOn, generatedByUserId);

            if (!generationResult.Success)
            {
                return StatusCode(400, new ApiResponse(
                    400,
                    generationResult.ErrorMessage ?? "Failed to generate document number",
                    generationResult));
            }

            return StatusCode(200, new ApiResponse(
                200,
                "Document number generated successfully (test endpoint)",
                generationResult));
        }
    }
}
