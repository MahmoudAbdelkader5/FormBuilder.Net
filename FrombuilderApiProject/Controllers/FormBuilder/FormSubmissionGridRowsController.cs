using FormBuilder.API.DTOs;
using FormBuilder.API.Models;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Core.DTOS.FormBuilder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormSubmissionGridRowsController : ControllerBase
    {
        private readonly IFormSubmissionGridRowService _formSubmissionGridRowService;

        public FormSubmissionGridRowsController(IFormSubmissionGridRowService formSubmissionGridRowService)
        {
            _formSubmissionGridRowService = formSubmissionGridRowService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll()
        {
            var result = await _formSubmissionGridRowService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetById(int id)
        {
            var result = await _formSubmissionGridRowService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("submission/{submissionId}")]
        public async Task<ActionResult<ApiResponse>> GetBySubmissionId(int submissionId)
        {
            var result = await _formSubmissionGridRowService.GetBySubmissionIdAsync(submissionId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("grid/{gridId}")]
        public async Task<ActionResult<ApiResponse>> GetByGridId(int gridId)
        {
            var result = await _formSubmissionGridRowService.GetByGridIdAsync(gridId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("submission/{submissionId}/grid/{gridId}")]
        public async Task<ActionResult<ApiResponse>> GetBySubmissionAndGrid(int submissionId, int gridId)
        {
            var result = await _formSubmissionGridRowService.GetBySubmissionAndGridAsync(submissionId, gridId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("submission/{submissionId}/grid/{gridId}/active")]
        public async Task<ActionResult<ApiResponse>> GetActiveRows(int submissionId, int gridId)
        {
            var result = await _formSubmissionGridRowService.GetActiveRowsAsync(submissionId, gridId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Create([FromBody] CreateFormSubmissionGridRowDto createDto)
        {
            var result = await _formSubmissionGridRowService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("multiple")]
        public async Task<ActionResult<ApiResponse>> CreateMultiple([FromBody] List<CreateFormSubmissionGridRowDto> createDtos)
        {
            var result = await _formSubmissionGridRowService.CreateMultipleAsync(createDtos);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] UpdateFormSubmissionGridRowDto updateDto)
        {
            var result = await _formSubmissionGridRowService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            var result = await _formSubmissionGridRowService.DeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("submission/{submissionId}/grid/{gridId}")]
        public async Task<ActionResult<ApiResponse>> DeleteBySubmissionAndGrid(int submissionId, int gridId)
        {
            var result = await _formSubmissionGridRowService.DeleteBySubmissionAndGridAsync(submissionId, gridId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{id}/toggle-active")]
        public async Task<ActionResult<ApiResponse>> ToggleActive(int id, [FromBody] bool isActive)
        {
            var result = await _formSubmissionGridRowService.ToggleActiveAsync(id, isActive);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("exists/{id}")]
        public async Task<ActionResult<ApiResponse>> Exists(int id)
        {
            var result = await _formSubmissionGridRowService.ExistsAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("submission/{submissionId}/grid/{gridId}/row-index/{rowIndex}/exists")]
        public async Task<ActionResult<ApiResponse>> RowExists(int submissionId, int gridId, int rowIndex)
        {
            var result = await _formSubmissionGridRowService.RowExistsAsync(submissionId, gridId, rowIndex);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("submission/{submissionId}/grid/{gridId}/next-index")]
        public async Task<ActionResult<ApiResponse>> GetNextRowIndex(int submissionId, int gridId)
        {
            var result = await _formSubmissionGridRowService.GetNextRowIndexAsync(submissionId, gridId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("submission/{submissionId}/count")]
        public async Task<ActionResult<ApiResponse>> GetRowCountBySubmission(int submissionId)
        {
            var result = await _formSubmissionGridRowService.GetRowCountBySubmissionAsync(submissionId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("grid/{gridId}/count")]
        public async Task<ActionResult<ApiResponse>> GetRowCountByGrid(int gridId)
        {
            var result = await _formSubmissionGridRowService.GetRowCountByGridAsync(gridId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("form-builder/{formBuilderId}")]
        public async Task<ActionResult<ApiResponse>> GetByFormBuilderId(int formBuilderId)
        {
            var result = await _formSubmissionGridRowService.GetByFormBuilderIdAsync(formBuilderId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("submission/{submissionId}/grid/{gridId}/reorder")]
        public async Task<ActionResult<ApiResponse>> ReorderRows(int submissionId, int gridId)
        {
            var result = await _formSubmissionGridRowService.ReorderRowsAsync(submissionId, gridId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("submission/{submissionId}/grid/{gridId}/bulk")]
        [ProducesResponseType(typeof(ApiResponse<List<FormSubmissionGridRowDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SaveBulkGridData(
            int submissionId, 
            int gridId, 
            [FromBody] List<SaveFormSubmissionGridDto> rows)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", ModelState));

            var bulkDto = new BulkSaveGridDataDto
            {
                SubmissionId = submissionId,
                GridId = gridId,
                Rows = rows
            };
            
            var result = await _formSubmissionGridRowService.SaveBulkGridDataAsync(bulkDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("submission/{submissionId}/grid/{gridId}/complete")]
        [ProducesResponseType(typeof(ApiResponse<List<FormSubmissionGridRowWithCellsDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCompleteGridData(int submissionId, int gridId)
        {
            var result = await _formSubmissionGridRowService.GetCompleteGridDataAsync(submissionId, gridId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("submission/{submissionId}/grid/{gridId}/validate")]
        [ProducesResponseType(typeof(ApiResponse<GridValidationResultDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ValidateGridData(
            int submissionId, 
            int gridId, 
            [FromBody] List<SaveFormSubmissionGridDto> rows)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", ModelState));

            var bulkDto = new BulkSaveGridDataDto
            {
                SubmissionId = submissionId,
                GridId = gridId,
                Rows = rows
            };
            
            var result = await _formSubmissionGridRowService.ValidateGridDataAsync(bulkDto);
            return StatusCode(result.StatusCode, result);
        }
    }
}