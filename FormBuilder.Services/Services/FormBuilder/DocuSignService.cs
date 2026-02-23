using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace FormBuilder.Services.Services.FormBuilder
{
    /// <summary>
    /// Backward-compatible adapter used by existing runtime flow.
    /// New submit flow uses IDocuSignEnvelopeService directly for embedded signing URL.
    /// </summary>
    public class DocuSignService : IDocuSignService
    {
        private readonly IDocuSignEnvelopeService _envelopeService;
        private readonly ILogger<DocuSignService> _logger;

        public DocuSignService(
            IDocuSignEnvelopeService envelopeService,
            ILogger<DocuSignService> logger)
        {
            _envelopeService = envelopeService;
            _logger = logger;
        }

        public async Task<bool> CreateSigningEnvelopeAsync(
            int submissionId,
            int stageId,
            string signerEmail,
            string signerName,
            string requestedByUserId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(signerEmail))
                {
                    _logger.LogWarning("DocuSign aborted: signer email is missing for submission {SubmissionId}, stage {StageId}", submissionId, stageId);
                    return false;
                }

                var envelopeId = await _envelopeService.CreateEnvelopeAsync(new DocuSignEnvelopeRequestDto
                {
                    SubmissionId = submissionId,
                    DocumentNumber = $"SUB-{submissionId}",
                    Signer = new DocuSignSignerDto
                    {
                        UserId = string.IsNullOrWhiteSpace(requestedByUserId) ? $"submission-{submissionId}" : requestedByUserId.Trim(),
                        Name = string.IsNullOrWhiteSpace(signerName) ? signerEmail : signerName,
                        Email = signerEmail
                    }
                });

                _logger.LogInformation(
                    "DocuSign envelope request accepted. SubmissionId={SubmissionId}, StageId={StageId}, EnvelopeId={EnvelopeId}",
                    submissionId,
                    stageId,
                    envelopeId);

                return !string.IsNullOrWhiteSpace(envelopeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DocuSign failed for submission {SubmissionId}, stage {StageId}", submissionId, stageId);
                return false;
            }
        }
    }
}

