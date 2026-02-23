using FormBuilder.API.Models.DTOs;
using FormBuilder.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administration")]

    public class FormAttachmentTypesController : ControllerBase
    {
        private readonly IFormAttachmentTypeService _formAttachmentTypeService;

        public FormAttachmentTypesController(IFormAttachmentTypeService formAttachmentTypeService)
        {
            _formAttachmentTypeService = formAttachmentTypeService;
        }

        // ================================
        // GET ENDPOINTS
        // ================================

        /// <summary>
        /// Get all form attachment types with FormBuilder and AttachmentType details
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _formAttachmentTypeService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get form attachment type by ID with FormBuilder and AttachmentType details
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _formAttachmentTypeService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get form attachment types by FormBuilder ID
        /// </summary>
        [HttpGet("form-builder/{formBuilderId}")]
        public async Task<IActionResult> GetByFormBuilderId(int formBuilderId)
        {
            var result = await _formAttachmentTypeService.GetByFormBuilderIdAsync(formBuilderId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get form attachment types by AttachmentType ID
        /// </summary>
        [HttpGet("attachment-type/{attachmentTypeId}")]
        public async Task<IActionResult> GetByAttachmentTypeId(int attachmentTypeId)
        {
            var result = await _formAttachmentTypeService.GetByAttachmentTypeIdAsync(attachmentTypeId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get all active form attachment types
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var result = await _formAttachmentTypeService.GetActiveAsync();
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get active form attachment types by FormBuilder ID
        /// </summary>
        [HttpGet("form-builder/{formBuilderId}/active")]
        public async Task<IActionResult> GetActiveByFormBuilderId(int formBuilderId)
        {
            var result = await _formAttachmentTypeService.GetActiveByFormBuilderIdAsync(formBuilderId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get mandatory form attachment types by FormBuilder ID
        /// </summary>
        [HttpGet("form-builder/{formBuilderId}/mandatory")]
        public async Task<IActionResult> GetMandatoryByFormBuilderId(int formBuilderId)
        {
            var result = await _formAttachmentTypeService.GetMandatoryByFormBuilderIdAsync(formBuilderId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Check if form builder has any mandatory attachments
        /// </summary>
        [HttpGet("form-builder/{formBuilderId}/has-mandatory")]
        public async Task<IActionResult> HasMandatoryAttachments(int formBuilderId)
        {
            var result = await _formAttachmentTypeService.HasMandatoryAttachmentsAsync(formBuilderId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Check if form attachment type exists
        /// </summary>
        [HttpGet("{id}/exists")]
        public async Task<IActionResult> Exists(int id)
        {
            var result = await _formAttachmentTypeService.ExistsAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Check if form attachment type is active
        /// </summary>
        [HttpGet("{id}/is-active")]
        public async Task<IActionResult> IsActive(int id)
        {
            var result = await _formAttachmentTypeService.IsActiveAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        // ================================
        // CREATE ENDPOINTS
        // ================================

        /// <summary>
        /// Create a new form attachment type association
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFormAttachmentTypeDto createDto)
        {
            var result = await _formAttachmentTypeService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

     
        // ================================
        // UPDATE ENDPOINTS
        // ================================

        /// <summary>
        /// Update an existing form attachment type association
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFormAttachmentTypeDto updateDto)
        {
            var result = await _formAttachmentTypeService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }



        // ================================
        // DELETE ENDPOINTS
        // ================================

        /// <summary>
        /// Delete a form attachment type association by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _formAttachmentTypeService.DeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Delete all form attachment type associations by FormBuilder ID
        /// </summary>
        [HttpDelete("form-builder/{formBuilderId}")]
        public async Task<IActionResult> DeleteByFormBuilderId(int formBuilderId)
        {
            var result = await _formAttachmentTypeService.DeleteByFormBuilderIdAsync(formBuilderId);
            return StatusCode(result.StatusCode, result);
        }
    }
}