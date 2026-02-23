using FormBuilder.API.Extensions;
using FormBuilder.API.Models;
using FormBuilder.Core.IServices.FormBuilder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CreateFieldOptionDto = FormBuilder.API.Models.CreateFieldOptionDto;
using UpdateFieldOptionDto = FormBuilder.API.Models.UpdateFieldOptionDto;

namespace FormBuilder.ApiProject.Controllers.FormBuilder
{
    [ApiController]
    [Route("api/[controller]")]
  
    public class FieldOptionsController : ControllerBase
    {
        private readonly IFieldOptionsService _fieldOptionsService;

        public FieldOptionsController(IFieldOptionsService fieldOptionsService)
        {
            _fieldOptionsService = fieldOptionsService;
        }

        // ================================
        // GET ALL FIELD OPTIONS
        // ================================
        [HttpGet]
        public async Task<IActionResult> GetAllFieldOptions()
        {
            var result = await _fieldOptionsService.GetAllAsync();
            return result.ToActionResult();
        }

        // ================================
        // GET FIELD OPTIONS BY FIELD ID
        // ================================
        [HttpGet("field/{fieldId}")]
        public async Task<IActionResult> GetFieldOptionsByFieldId(int fieldId)
        {
            var result = await _fieldOptionsService.GetByFieldIdAsync(fieldId);
            return result.ToActionResult();
        }

        // ================================
        // GET ACTIVE FIELD OPTIONS BY FIELD ID
        // ================================
        [HttpGet("field/{fieldId}/active")]
        public async Task<IActionResult> GetActiveFieldOptionsByFieldId(int fieldId)
        {
            var result = await _fieldOptionsService.GetActiveByFieldIdAsync(fieldId);
            return result.ToActionResult();
        }

        // ================================
        // GET FIELD OPTION BY ID
        // ================================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetFieldOptionById(int id)
        {
            var result = await _fieldOptionsService.GetByIdAsync(id, asNoTracking: true);
            return result.ToActionResult();
        }

        // ================================
        // GET DEFAULT OPTION BY FIELD ID
        // ================================
        [HttpGet("field/{fieldId}/default")]
        public async Task<IActionResult> GetDefaultOption(int fieldId)
        {
            var result = await _fieldOptionsService.GetDefaultOptionAsync(fieldId);
            return result.ToActionResult();
        }

        // ================================
        // GET OPTIONS COUNT BY FIELD ID
        // ================================
        [HttpGet("field/{fieldId}/count")]
        public async Task<IActionResult> GetOptionsCount(int fieldId)
        {
            var result = await _fieldOptionsService.GetOptionsCountAsync(fieldId);
            return result.ToActionResult();
        }

        // ================================
        // CREATE FIELD OPTION
        // ================================
        [HttpPost]
        public async Task<IActionResult> CreateFieldOption([FromBody] CreateFieldOptionDto createFieldOptionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _fieldOptionsService.CreateAsync(createFieldOptionDto);
            if (result.Success && result.Data != null)
            {
                return CreatedAtAction(nameof(GetFieldOptionById), new { id = result.Data.Id }, result.Data);
            }
            return result.ToActionResult();
        }

        // ================================
        // CREATE BULK FIELD OPTIONS
        // ================================
        [HttpPost("bulk")]
        public async Task<IActionResult> CreateBulkFieldOptions([FromBody] System.Collections.Generic.List<CreateFieldOptionDto> createDtos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _fieldOptionsService.CreateBulkAsync(createDtos);
            return result.ToActionResult();
        }

        // ================================
        // UPDATE FIELD OPTION
        // ================================
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateFieldOption(int id, [FromBody] UpdateFieldOptionDto updateFieldOptionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _fieldOptionsService.UpdateAsync(id, updateFieldOptionDto);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }

        // ================================
        // DELETE FIELD OPTION (HARD DELETE)
        // ================================
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteFieldOption(int id)
        {
            var result = await _fieldOptionsService.DeleteAsync(id);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }

        // ================================
        // SOFT DELETE FIELD OPTION
        // ================================
        [HttpDelete("{id:int}/soft")]
        public async Task<IActionResult> SoftDeleteFieldOption(int id)
        {
            var result = await _fieldOptionsService.SoftDeleteAsync(id);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }
    }
}