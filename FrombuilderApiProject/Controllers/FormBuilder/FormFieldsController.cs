using FormBuilder.API.Attributes;
using FormBuilder.API.Extensions;
using FormBuilder.API.Models;
using FormBuilder.Core.IServices.FormBuilder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CreateFormFieldDto = FormBuilder.Core.DTOS.FormFields.CreateFormFieldDto;
using UpdateFormFieldDto = FormBuilder.API.Models.UpdateFormFieldDto;

namespace FormBuilder.ApiProject.Controllers.FormBuilder
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FormFieldsController : ControllerBase
    {
        private readonly IFormFieldService _formFieldService;

        public FormFieldsController(IFormFieldService formFieldService)
        {
            _formFieldService = formFieldService;
        }

        // ================================
        // GET ALL FORM FIELDS
        // ================================
        [HttpGet]
        public async Task<IActionResult> GetAllFormFields()
        {
            var result = await _formFieldService.GetAllAsync();
            return result.ToActionResult();
        }

        // ================================
        // GET ACTIVE FORM FIELDS
        // ================================
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveFormFields()
        {
            var result = await _formFieldService.GetActiveAsync();
            return result.ToActionResult();
        }

        // ================================
        // GET FORM FIELD BY ID
        // ================================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetFormFieldById(int id)
        {
            var result = await _formFieldService.GetByIdAsync(id, asNoTracking: true);
            return result.ToActionResult();
        }

        // ================================
        // GET FORM FIELD BY FIELD CODE
        // ================================
        [HttpGet("code/{fieldCode}")]
        public async Task<IActionResult> GetFormFieldByCode(string fieldCode)
        {
            var result = await _formFieldService.GetByFieldCodeAsync(fieldCode);
            return result.ToActionResult();
        }

        // ================================
        // GET FORM FIELDS BY TAB ID
        // ================================
        [HttpGet("tab/{tabId}")]
        public async Task<IActionResult> GetFormFieldsByTabId(int tabId)
        {
            var result = await _formFieldService.GetByTabIdAsync(tabId);
            return result.ToActionResult();
        }

        // ================================
        // GET FORM FIELDS BY FORM ID
        // ================================
        [HttpGet("form/{formBuilderId}")]
        public async Task<IActionResult> GetFormFieldsByFormId(int formBuilderId)
        {
            var result = await _formFieldService.GetByFormIdAsync(formBuilderId);
            return result.ToActionResult();
        }

        // ================================
        // GET FORM FIELDS BY GRID ID
        // ================================
        [HttpGet("grid/{gridId}")]
        public async Task<IActionResult> GetFormFieldsByGridId(int gridId)
        {
            var result = await _formFieldService.GetFieldsByGridIdAsync(gridId);
            return result.ToActionResult();
        }

        // ================================
        // GET MANDATORY FIELDS BY TAB ID
        // ================================
        [HttpGet("tab/{tabId}/mandatory")]
        public async Task<IActionResult> GetMandatoryFields(int tabId)
        {
            var result = await _formFieldService.GetMandatoryFieldsAsync(tabId);
            return result.ToActionResult();
        }

        // ================================
        // GET VISIBLE FIELDS BY TAB ID
        // ================================
        [HttpGet("tab/{tabId}/visible")]
        public async Task<IActionResult> GetVisibleFields(int tabId)
        {
            var result = await _formFieldService.GetVisibleFieldsAsync(tabId);
            return result.ToActionResult();
        }

        // ================================
        // GET EDITABLE FIELDS BY TAB ID
        // ================================
        [HttpGet("tab/{tabId}/editable")]
        public async Task<IActionResult> GetEditableFields(int tabId)
        {
            var result = await _formFieldService.GetEditableFieldsAsync(tabId);
            return result.ToActionResult();
        }

        // ================================
        // GET USAGE COUNT
        // ================================
        [HttpGet("{id:int}/usage-count")]
        public async Task<IActionResult> GetFormFieldUsageCount(int id)
        {
            var result = await _formFieldService.GetUsageCountAsync(id);
            return result.ToActionResult();
        }

        // ================================
        // GET FIELDS COUNT BY TAB
        // ================================
        [HttpGet("tab/{tabId}/count")]
        public async Task<IActionResult> GetFieldsCountByTab(int tabId)
        {
            var result = await _formFieldService.GetFieldsCountByTabAsync(tabId);
            return result.ToActionResult();
        }

        // ================================
        // GET FIELDS COUNT BY FORM
        // ================================
        [HttpGet("form/{formBuilderId}/count")]
        public async Task<IActionResult> GetFieldsCountByForm(int formBuilderId)
        {
            var result = await _formFieldService.GetFieldsCountByFormAsync(formBuilderId);
            return result.ToActionResult();
        }

        // ================================
        // CREATE FORM FIELD
        // ================================
        [HttpPost]
        [RequirePermission("FormField_Allow_Create")]
        public async Task<IActionResult> CreateFormField([FromBody] CreateFormFieldDto createFormFieldDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _formFieldService.CreateAsync(createFormFieldDto);
            if (result.Success && result.Data != null)
            {
                return CreatedAtAction(nameof(GetFormFieldById), new { id = result.Data.Id }, result.Data);
            }
            return result.ToActionResult();
        }

        // ================================
        // UPDATE FORM FIELD
        // ================================
        [HttpPut("{id:int}")]
        [RequirePermission("FormField_Allow_Edit")]
        public async Task<IActionResult> UpdateFormField(int id, [FromBody] UpdateFormFieldDto updateFormFieldDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _formFieldService.UpdateAsync(id, updateFormFieldDto);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }

        // ================================
        // DELETE FORM FIELD (HARD DELETE)
        // ================================
        [HttpDelete("{id:int}")]
        [RequirePermission("FormField_Allow_Delete")]
        public async Task<IActionResult> DeleteFormField(int id)
        {
            var result = await _formFieldService.DeleteAsync(id);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }

        // ================================
        // SOFT DELETE FORM FIELD
        // ================================
        [HttpDelete("{id:int}/soft")]
        [RequirePermission("FormField_Allow_Delete")]
        public async Task<IActionResult> SoftDeleteFormField(int id)
        {
            var result = await _formFieldService.SoftDeleteAsync(id);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }

        // ================================
        // RESTORE SOFT-DELETED FORM FIELD
        // ================================
        [HttpPost("{id:int}/restore")]
        [RequirePermission("FormField_Allow_Manage")]
        public async Task<IActionResult> RestoreFormField(int id)
        {
            var result = await _formFieldService.RestoreAsync(id);
            return result.ToActionResult();
        }
    }
}
