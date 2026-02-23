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

    public class FormSubmissionValuesController : ControllerBase
    {
        private readonly IFormSubmissionValuesService _formSubmissionValuesService;

        public FormSubmissionValuesController(IFormSubmissionValuesService formSubmissionValuesService)
        {
            _formSubmissionValuesService = formSubmissionValuesService;
        }

        // ================================
        // GET ALL FORM SUBMISSION VALUES
        // ================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _formSubmissionValuesService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // GET BY ID
        // ================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _formSubmissionValuesService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // GET BY SUBMISSION ID
        // ================================
        [HttpGet("submission/{submissionId}")]
        public async Task<IActionResult> GetBySubmissionId(int submissionId)
        {
            var result = await _formSubmissionValuesService.GetBySubmissionIdAsync(submissionId);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // GET BY FIELD ID
        // ================================
        [HttpGet("field/{fieldId}")]
        public async Task<IActionResult> GetByFieldId(int fieldId)
        {
            var result = await _formSubmissionValuesService.GetByFieldIdAsync(fieldId);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // GET BY SUBMISSION AND FIELD
        // ================================
        [HttpGet("submission/{submissionId}/field/{fieldId}")]
        public async Task<IActionResult> GetBySubmissionAndField(int submissionId, int fieldId)
        {
            var result = await _formSubmissionValuesService.GetBySubmissionAndFieldAsync(submissionId, fieldId);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // CREATE NEW FORM SUBMISSION VALUE
        // ================================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFormSubmissionValueDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", ModelState));

            var result = await _formSubmissionValuesService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // CREATE BULK FORM SUBMISSION VALUES
        // ================================
        [HttpPost("bulk")]
        public async Task<IActionResult> CreateBulk([FromBody] BulkFormSubmissionValuesDto bulkDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", ModelState));

            var result = await _formSubmissionValuesService.CreateBulkAsync(bulkDto);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // UPDATE FORM SUBMISSION VALUE
        // ================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFormSubmissionValueDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", ModelState));

            var result = await _formSubmissionValuesService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // UPDATE BY FIELD
        // ================================
        [HttpPut("submission/{submissionId}/field/{fieldId}")]
        public async Task<IActionResult> UpdateByField(int submissionId, int fieldId, [FromBody] UpdateFormSubmissionValueDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", ModelState));

            var result = await _formSubmissionValuesService.UpdateByFieldAsync(submissionId, fieldId, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // DELETE FORM SUBMISSION VALUE
        // ================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _formSubmissionValuesService.DeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // DELETE BY SUBMISSION ID
        // ================================
        [HttpDelete("submission/{submissionId}")]
        public async Task<IActionResult> DeleteBySubmissionId(int submissionId)
        {
            var result = await _formSubmissionValuesService.DeleteBySubmissionIdAsync(submissionId);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // CHECK EXISTS
        // ================================
        [HttpGet("{id}/exists")]
        public async Task<IActionResult> Exists(int id)
        {
            var result = await _formSubmissionValuesService.ExistsAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}