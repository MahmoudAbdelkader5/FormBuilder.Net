using FormBuilder.API.Attributes;
using FormBuilder.Application.DTOS;
using FormBuilder.Application.DTOs.Formula;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FormulasController : ControllerBase
    {
        private readonly IFormulaService _formulaService;

        public FormulasController(IFormulaService formulaService)
        {
            _formulaService = formulaService;
        }

        #region Basic CRUD Operations

        [HttpGet("form-builder/{formBuilderId}")]
        [RequirePermission("Formula_Allow_View")]
        public async Task<IActionResult> GetByFormBuilder(int formBuilderId)
        {
            var result = await _formulaService.GetAllAsync(formBuilderId);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        [RequirePermission("Formula_Allow_View")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _formulaService.GetByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpGet("code/{code}/{formBuilderId}")]
        [RequirePermission("Formula_Allow_View")]
        public async Task<IActionResult> GetByCode(string code, int formBuilderId)
        {
            var result = await _formulaService.GetByCodeAsync(code, formBuilderId);
            return result.ToActionResult();
        }

        [HttpPost]
        [RequirePermission("Formula_Allow_Create")]
        public async Task<IActionResult> Create([FromBody] CreateFormulaDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _formulaService.CreateAsync(createDto);
            if (result.Success && result.Data != null)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
            }
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [RequirePermission("Formula_Allow_Edit")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFormulaDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _formulaService.UpdateAsync(id, updateDto);
            if (result.Success)
            {
                return NoContent();
            }
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [RequirePermission("Formula_Allow_Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _formulaService.DeleteAsync(id);
            if (result.Success)
            {
                return NoContent();
            }
            return result.ToActionResult();
        }

        [HttpDelete("form-builder/{formBuilderId}")]
        [RequirePermission("Formula_Allow_Delete")]
        public async Task<IActionResult> DeleteByFormBuilder(int formBuilderId)
        {
            var result = await _formulaService.DeleteByFormBuilderIdAsync(formBuilderId);
            return result.ToActionResult();
        }

        #endregion

        #region Status Management

        [HttpPatch("{id}/toggle-active")]
        [RequirePermission("Formula_Allow_Manage")]
        public async Task<IActionResult> ToggleActive(int id, [FromBody] bool isActive)
        {
            var result = await _formulaService.ToggleActiveAsync(id, isActive);
            if (result.Success)
            {
                return NoContent();
            }
            return result.ToActionResult();
        }

        [HttpGet("form-builder/{formBuilderId}/active")]
        public async Task<IActionResult> GetActive(int formBuilderId)
        {
            var result = await _formulaService.GetActiveAsync(formBuilderId);
            return result.ToActionResult();
        }

        [HttpGet("form-builder/{formBuilderId}/inactive")]
        public async Task<IActionResult> GetInactive(int formBuilderId)
        {
            var result = await _formulaService.GetInactiveAsync(formBuilderId);
            return result.ToActionResult();
        }

        #endregion

        #region Query Operations

        [HttpGet("with-details")]
        public async Task<IActionResult> GetFormulasWithDetails([FromQuery] int formBuilderId = 0)
        {
            var result = await _formulaService.GetFormulasWithDetailsAsync(formBuilderId);
            return result.ToActionResult();
        }

        [HttpGet("{id}/with-details")]
        public async Task<IActionResult> GetByIdWithDetails(int id)
        {
            var result = await _formulaService.GetByIdWithDetailsAsync(id);
            return result.ToActionResult();
        }

        [HttpGet("result-field/{resultFieldId}")]
        public async Task<IActionResult> GetByResultField(int? resultFieldId)
        {
            var result = await _formulaService.GetByResultFieldAsync(resultFieldId);
            return result.ToActionResult();
        }

        [HttpGet("form-builder/{formBuilderId}/without-result-field")]
        public async Task<IActionResult> GetFormulasWithoutResultField(int formBuilderId)
        {
            var result = await _formulaService.GetFormulasWithoutResultFieldAsync(formBuilderId);
            return result.ToActionResult();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchFormulas([FromQuery] string term, [FromQuery] int formBuilderId)
        {
            var result = await _formulaService.SearchFormulasAsync(term, formBuilderId);
            return result.ToActionResult();
        }

        #endregion

        #region Validation Operations

        [HttpPost("validate-expression")]
        public async Task<IActionResult> ValidateExpression([FromBody] ValidateExpressionDto validationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _formulaService.ValidateExpressionAsync(validationDto);
            return result.ToActionResult();
        }

        [HttpPost("validate-expression-with-details")]
        public async Task<IActionResult> ValidateExpressionWithDetails([FromBody] ValidateExpressionDto validationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _formulaService.ValidateExpressionWithDetailsAsync(validationDto);
            return result.ToActionResult();
        }

        [HttpGet("check-code-exists")]
        public async Task<IActionResult> CodeExists([FromQuery] string code, [FromQuery] int formBuilderId, [FromQuery] int? excludeId = null)
        {
            var result = await _formulaService.CodeExistsAsync(code, formBuilderId, excludeId);
            return result.ToActionResult();
        }

        [HttpGet("{id}/is-active")]
        public async Task<IActionResult> IsActive(int id)
        {
            var result = await _formulaService.IsActiveAsync(id);
            return result.ToActionResult();
        }

        [HttpGet("form-builder/{formBuilderId}/has-active")]
        public async Task<IActionResult> HasActiveFormulas(int formBuilderId)
        {
            var result = await _formulaService.HasActiveFormulasAsync(formBuilderId);
            return result.ToActionResult();
        }

        #endregion

        #region Utility Operations

        [HttpGet("{id}/referenced-field-codes")]
        public async Task<IActionResult> GetReferencedFieldCodes(int id)
        {
            var result = await _formulaService.GetReferencedFieldCodesAsync(id);
            return result.ToActionResult();
        }

        [HttpGet("form-builder/{formBuilderId}/count")]
        public async Task<IActionResult> CountFormulas(int formBuilderId)
        {
            var result = await _formulaService.CountFormulasAsync(formBuilderId);
            return result.ToActionResult();
        }

        [HttpPatch("{id}/expression")]
        public async Task<IActionResult> UpdateFormulaExpression(int id, [FromBody] string expressionText)
        {
            var result = await _formulaService.UpdateFormulaExpressionAsync(id, expressionText);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return result.ToActionResult();
        }

        [HttpGet("form-builder/{formBuilderId}/field-codes")]
        public async Task<IActionResult> GetFieldCodesForForm(int formBuilderId)
        {
            var result = await _formulaService.GetFieldCodesForFormAsync(formBuilderId);
            return result.ToActionResult();
        }

        #endregion

        #region FORMULA_VARIABLES Operations

        [HttpGet("{formulaId}/variables")]
        public async Task<IActionResult> GetFormulaVariables(int formulaId)
        {
            var result = await _formulaService.GetFormulaVariablesAsync(formulaId);
            return result.ToActionResult();
        }

        [HttpGet("{formulaId}/variables/count")]
        public async Task<IActionResult> CountVariables(int formulaId)
        {
            var result = await _formulaService.CountVariablesAsync(formulaId);
            return result.ToActionResult();
        }

        [HttpGet("{formulaId}/has-variables")]
        public async Task<IActionResult> HasVariables(int formulaId)
        {
            var result = await _formulaService.HasVariablesAsync(formulaId);
            return result.ToActionResult();
        }

        #endregion

        #region Additional Formula Operations

        [HttpGet("by-field/{fieldId}")]
        public async Task<IActionResult> GetFormulasByField(int fieldId)
        {
            var result = await _formulaService.GetFormulasByFieldAsync(fieldId);
            return result.ToActionResult();
        }

        [HttpGet("form-builder/{formBuilderId}/statistics")]
        public async Task<IActionResult> GetFormulaStatistics(int formBuilderId)
        {
            var result = await _formulaService.GetFormulaStatisticsAsync(formBuilderId);
            return result.ToActionResult();
        }

        [HttpPost("batch-update-status")]
        public async Task<IActionResult> BatchUpdateFormulaStatus([FromBody] BatchUpdateFormulaStatusDto batchDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _formulaService.BatchUpdateFormulaStatusAsync(batchDto.FormulaIds, batchDto.IsActive);
            return result.ToActionResult();
        }

        [HttpPost("{sourceFormulaId}/duplicate")]
        public async Task<IActionResult> DuplicateFormula(int sourceFormulaId, [FromBody] DuplicateFormulaDto duplicateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _formulaService.DuplicateFormulaAsync(sourceFormulaId, duplicateDto);
            if (result.Success && result.Data != null)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
            }
            return result.ToActionResult();
        }

        #endregion

        #region Formula Calculation Endpoints

        [HttpPost("{formulaId}/calculate")]
        public async Task<IActionResult> CalculateFormula(int formulaId, [FromBody] Dictionary<string, object> fieldValues)
        {
            var result = await _formulaService.CalculateFormulaAsync(formulaId, fieldValues);
            return result.ToActionResult();
        }

        [HttpPost("calculate-expression")]
        public async Task<IActionResult> CalculateExpression([FromBody] CalculateExpressionDto calculateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _formulaService.CalculateExpressionAsync(calculateDto.ExpressionText, calculateDto.FieldValues);
            return result.ToActionResult();
        }

        [HttpPost("calculate-advanced")]
        public async Task<IActionResult> CalculateAdvanced([FromBody] CalculateAdvancedDto calculateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _formulaService.CalculateWithAllOperationsAsync(
                calculateDto.ExpressionText,
                calculateDto.FieldValues);

            return result.ToActionResult();
        }

        [HttpPost("calculate-safe")]
        public async Task<IActionResult> CalculateSafe([FromBody] CalculateExpressionDto calculateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _formulaService.SafeCalculateExpressionAsync(
                calculateDto.ExpressionText,
                calculateDto.FieldValues);

            return result.ToActionResult();
        }

        [HttpPost("form-builder/{formBuilderId}/batch-calculate")]
        public async Task<IActionResult> BatchCalculateFormulas(int formBuilderId, [FromBody] Dictionary<string, object> fieldValues)
        {
            var result = await _formulaService.BatchCalculateFormulasAsync(formBuilderId, fieldValues);
            return result.ToActionResult();
        }

        [HttpPost("preview-calculation")]
        public async Task<IActionResult> PreviewCalculation([FromBody] PreviewCalculationDto previewDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _formulaService.PreviewCalculationAsync(previewDto);
            return result.ToActionResult();
        }

        [HttpGet("{formulaId}/test-with-samples")]
        public async Task<IActionResult> TestFormulaWithSampleData(int formulaId)
        {
            var result = await _formulaService.TestFormulaWithSampleDataAsync(formulaId);
            return result.ToActionResult();
        }

        #endregion
    }
}