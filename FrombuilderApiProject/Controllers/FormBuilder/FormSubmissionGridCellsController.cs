using FormBuilder.API.DTOs;
using FormBuilder.API.Models;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Administration")]
    public class FormSubmissionGridCellsController : ControllerBase
    {
        private readonly IFormSubmissionGridCellService _formSubmissionGridCellService;

        public FormSubmissionGridCellsController(IFormSubmissionGridCellService formSubmissionGridCellService)
        {
            _formSubmissionGridCellService = formSubmissionGridCellService;
        }

        // GET: api/FormSubmissionGridCells
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll()
        {
            var result = await _formSubmissionGridCellService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        // GET: api/FormSubmissionGridCells/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetById(int id)
        {
            var result = await _formSubmissionGridCellService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        // GET: api/FormSubmissionGridCells/row/5
        [HttpGet("row/{rowId}")]
        public async Task<ActionResult<ApiResponse>> GetByRowId(int rowId)
        {
            var result = await _formSubmissionGridCellService.GetByRowIdAsync(rowId);
            return StatusCode(result.StatusCode, result);
        }

        // GET: api/FormSubmissionGridCells/row/5/column/10
        [HttpGet("row/{rowId}/column/{columnId}")]
        public async Task<ActionResult<ApiResponse>> GetByRowAndColumn(int rowId, int columnId)
        {
            var result = await _formSubmissionGridCellService.GetByRowAndColumnAsync(rowId, columnId);
            return StatusCode(result.StatusCode, result);
        }

        // POST: api/FormSubmissionGridCells
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Create([FromBody] CreateFormSubmissionGridCellDto createDto)
        {
            var result = await _formSubmissionGridCellService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        // PUT: api/FormSubmissionGridCells/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] UpdateFormSubmissionGridCellDto updateDto)
        {
            var result = await _formSubmissionGridCellService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        // DELETE: api/FormSubmissionGridCells/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            var result = await _formSubmissionGridCellService.DeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        // DELETE: api/FormSubmissionGridCells/row/5
        [HttpDelete("row/{rowId}")]
        public async Task<ActionResult<ApiResponse>> DeleteByRowId(int rowId)
        {
            var result = await _formSubmissionGridCellService.DeleteByRowIdAsync(rowId);
            return StatusCode(result.StatusCode, result);
        }

       
    }
}