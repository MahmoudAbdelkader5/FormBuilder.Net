using FormBuilder.API.Extensions;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Core.DTOS.FormBuilder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace FormBuilder.ApiProject.Controllers.FormBuilder
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FormBuilderDocumentSettingsController : ControllerBase
    {
        private readonly IFormBuilderDocumentSettingsService _documentSettingsService;
        private readonly IStringLocalizer<FormBuilderDocumentSettingsController> _localizer;

        public FormBuilderDocumentSettingsController(
            IFormBuilderDocumentSettingsService documentSettingsService,
            IStringLocalizer<FormBuilderDocumentSettingsController> localizer)
        {
            _documentSettingsService = documentSettingsService;
            _localizer = localizer;
        }

        /// <summary>
        /// Get Document Settings (Document Type + Series) for a specific Form Builder
        /// </summary>
        [HttpGet("form/{formBuilderId}")]
        [ProducesResponseType(typeof(DocumentSettingsDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetDocumentSettings(int formBuilderId, CancellationToken cancellationToken = default)
        {
            var result = await _documentSettingsService.GetDocumentSettingsAsync(formBuilderId);
            return result.ToActionResult();
        }

        /// <summary>
        /// Save Document Settings (Document Type + Series) for a Form Builder
        /// Creates or updates Document Type and manages Document Series
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(DocumentSettingsDto), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SaveDocumentSettings([FromBody] SaveDocumentSettingsDto dto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _documentSettingsService.SaveDocumentSettingsAsync(dto);
            return result.ToActionResult();
        }

        /// <summary>
        /// Delete Document Settings for a Form Builder (removes Document Type and all Series)
        /// </summary>
        [HttpDelete("form/{formBuilderId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteDocumentSettings(int formBuilderId, CancellationToken cancellationToken = default)
        {
            var result = await _documentSettingsService.DeleteDocumentSettingsAsync(formBuilderId);
            if (result.Success) return NoContent();
            return result.ToActionResult();
        }

        /// <summary>
        /// Auto-configure default Document Settings for a Form Builder
        /// Creates a default Document Type and Series if they don't exist
        /// This is a convenience endpoint to quickly set up a FormBuilder with sensible defaults
        /// </summary>
        [HttpPost("auto-configure/{formBuilderId}")]
        [ProducesResponseType(typeof(DocumentSettingsDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AutoConfigureDefaults(int formBuilderId, 
            [FromQuery] int projectId,
            [FromQuery] string? documentCode = null,
            [FromQuery] string? seriesCode = null, CancellationToken cancellationToken = default)
        {
            if (projectId <= 0)
            {
                return BadRequest(new { message = "ProjectId is required and must be greater than 0" });
            }

            var result = await _documentSettingsService.AutoConfigureDefaultsAsync(formBuilderId, projectId, documentCode, seriesCode);
            return result.ToActionResult();
        }
    }
}

