using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.API.Models;
using FormBuilder.Domain.Interfaces.Services;
using FormBuilder.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormSubmissionAttachmentsController : ControllerBase
    {
        private readonly IFormSubmissionAttachmentsService _formSubmissionAttachmentsService;

        public FormSubmissionAttachmentsController(IFormSubmissionAttachmentsService formSubmissionAttachmentsService)
        {
            _formSubmissionAttachmentsService = formSubmissionAttachmentsService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<FormSubmissionAttachmentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _formSubmissionAttachmentsService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        // More specific routes must come before less specific ones
        [HttpGet("submission/{submissionId}/field/{fieldId}")]
        [ProducesResponseType(typeof(ApiResponse<List<FormSubmissionAttachmentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBySubmissionAndField(int submissionId, int fieldId, CancellationToken cancellationToken = default)
        {
            var result = await _formSubmissionAttachmentsService.GetBySubmissionAndFieldAsync(submissionId, fieldId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("submission/{submissionId}/stats")]
        [ProducesResponseType(typeof(ApiResponse<AttachmentStatsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAttachmentStats(int submissionId, CancellationToken cancellationToken = default)
        {
            var result = await _formSubmissionAttachmentsService.GetAttachmentStatsAsync(submissionId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("submission/{submissionId}")]
        [ProducesResponseType(typeof(ApiResponse<List<FormSubmissionAttachmentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBySubmissionId(int submissionId, CancellationToken cancellationToken = default)
        {
            var result = await _formSubmissionAttachmentsService.GetBySubmissionIdAsync(submissionId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("field/{fieldId}")]
        [ProducesResponseType(typeof(ApiResponse<List<FormSubmissionAttachmentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByFieldId(int fieldId, CancellationToken cancellationToken = default)
        {
            var result = await _formSubmissionAttachmentsService.GetByFieldIdAsync(fieldId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}/exists")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Exists(int id, CancellationToken cancellationToken = default)
        {
            var result = await _formSubmissionAttachmentsService.ExistsAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}/download")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DownloadFile(int id, CancellationToken cancellationToken = default)
        {
            var attachmentResult = await _formSubmissionAttachmentsService.GetByIdAsync(id);
            if (attachmentResult.StatusCode != 200 || attachmentResult.Data == null)
            {
                return NotFound(new { message = "File not found" });
            }

            var attachment = attachmentResult.Data;
            var fileStorageService = HttpContext.RequestServices.GetRequiredService<IFileStorageService>();

            var fileStream = await fileStorageService.GetFileAsync(attachment.FilePath);
            if (fileStream == null)
            {
                return NotFound(new { message = "File not found on disk" });
            }

            return File(fileStream, attachment.ContentType, attachment.FileName);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<FormSubmissionAttachmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
        {
            var result = await _formSubmissionAttachmentsService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<FormSubmissionAttachmentDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateFormSubmissionAttachmentDto createDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", ModelState));

            var result = await _formSubmissionAttachmentsService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(ApiResponse<List<FormSubmissionAttachmentDto>>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateBulk([FromBody] BulkAttachmentsDto bulkDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", ModelState));

            var result = await _formSubmissionAttachmentsService.CreateBulkAsync(bulkDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ApiResponse<FormSubmissionAttachmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadFile([FromForm] UploadAttachmentRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", ModelState));

            if (request.File == null || request.File.Length == 0)
                return BadRequest(new ApiResponse(400, "No file provided"));

            var uploadDto = new UploadAttachmentDto
            {
                SubmissionId = request.SubmissionId,
                FieldId = request.FieldId,
                FieldCode = request.FieldCode
            };

            var result = await _formSubmissionAttachmentsService.UploadAttachmentAsync(request.File, uploadDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("upload-multiple")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ApiResponse<List<AttachmentUploadResultDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadMultipleFiles([FromForm] UploadMultipleAttachmentsRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", ModelState));

            if (request.Files == null || !request.Files.Any())
                return BadRequest(new ApiResponse(400, "No files provided"));

            var uploadDto = new UploadAttachmentDto
            {
                SubmissionId = request.SubmissionId,
                FieldId = request.FieldId,
                FieldCode = request.FieldCode
            };

            var result = await _formSubmissionAttachmentsService.UploadMultipleAttachmentsAsync(request.Files, uploadDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<FormSubmissionAttachmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFormSubmissionAttachmentDto updateDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", ModelState));

            var result = await _formSubmissionAttachmentsService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var result = await _formSubmissionAttachmentsService.DeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("submission/{submissionId}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBySubmissionId(int submissionId, CancellationToken cancellationToken = default)
        {
            var result = await _formSubmissionAttachmentsService.DeleteBySubmissionIdAsync(submissionId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("submission/{submissionId}/field/{fieldId}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBySubmissionAndField(int submissionId, int fieldId, CancellationToken cancellationToken = default)
        {
            var result = await _formSubmissionAttachmentsService.DeleteBySubmissionAndFieldAsync(submissionId, fieldId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
