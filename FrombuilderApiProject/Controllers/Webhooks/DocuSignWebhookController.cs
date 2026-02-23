using System.Text.Json;
using formBuilder.Domian.Interfaces;
using FormBuilder.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormBuilder.API.Controllers.Webhooks
{
    [ApiController]
    [AllowAnonymous]
    [Route("webhooks/docusign")]
    public class DocuSignWebhookController : ControllerBase
    {
        private readonly IunitOfwork _unitOfWork;
        private readonly ILogger<DocuSignWebhookController> _logger;

        public DocuSignWebhookController(IunitOfwork unitOfWork, ILogger<DocuSignWebhookController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] JsonElement payload)
        {
            var envelopeId = ReadString(payload, "data", "envelopeId")
                             ?? ReadString(payload, "data", "envelopeSummary", "envelopeId")
                             ?? ReadString(payload, "envelopeId");

            var status = ReadString(payload, "data", "envelopeSummary", "status")
                         ?? ReadString(payload, "status")
                         ?? ReadString(payload, "event");

            if (string.IsNullOrWhiteSpace(envelopeId))
            {
                return BadRequest(new ApiResponse(400, "envelopeId not found in webhook payload"));
            }

            _logger.LogInformation("DocuSign webhook received. EnvelopeId={EnvelopeId}, Status={Status}", envelopeId, status);

            if (!string.Equals(status, "completed", StringComparison.OrdinalIgnoreCase))
            {
                return Ok(new { received = true, updated = false });
            }

            var submission = await _unitOfWork.FormSubmissionsRepository.SingleOrDefaultAsync(
                s => s.DocuSignEnvelopeId == envelopeId && !s.IsDeleted,
                asNoTracking: false);

            if (submission == null)
            {
                _logger.LogWarning("DocuSign webhook envelope {EnvelopeId} does not match any submission", envelopeId);
                return Ok(new { received = true, updated = false });
            }

            submission.SignatureStatus = "signed";
            submission.SignedAt = DateTime.UtcNow;
            submission.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.FormSubmissionsRepository.Update(submission);
            await _unitOfWork.CompleteAsyn();

            return Ok(new { received = true, updated = true });
        }

        private static string? ReadString(JsonElement element, params string[] path)
        {
            var current = element;
            foreach (var segment in path)
            {
                if (current.ValueKind != JsonValueKind.Object || !current.TryGetProperty(segment, out var child))
                    return null;
                current = child;
            }

            return current.ValueKind == JsonValueKind.String ? current.GetString() : current.ToString();
        }
    }
}

