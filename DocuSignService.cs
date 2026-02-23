using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FormBuilder.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FormBuilder.Services.Services.FormBuilder
{
  
    public class DocuSignService : IDocuSignService
    {
        private readonly ILogger<DocuSignService> _logger;
        private readonly IConfiguration _configuration;

        public DocuSignService(ILogger<DocuSignService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> CreateSigningEnvelopeAsync(int submissionId, int stageId, string signerEmail, string signerName, string requestedByUserId)
        {
            try
            {
                // TODO: Implement actual DocuSign integration:
                // - Use _configuration to read DocuSign credentials (IntegratorKey, AccountId, BaseUrl, RSA/Secret)
                // - Use IHttpClientFactory or DocuSign SDK to:
                //    1) Authenticate
                //    2) Create an envelope with the submission PDF (or generate PDF from submission)
                //    3) Add signer and send
                // - Persist envelopeId and status in DB if needed.

                _logger?.LogInformation("DocuSignService.CreateSigningEnvelopeAsync called for SubmissionId={SubmissionId}, StageId={StageId}, Signer={SignerEmail}", submissionId, stageId, signerEmail);

                // Simulated delay to mimic network call
                await Task.Delay(100);

                // For now we return true to indicate the envelope would be created.
                _logger?.LogInformation("DocuSign stubbed: envelope created for submission {SubmissionId}", submissionId);
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "DocuSignService failed to create envelope for submission {SubmissionId}", submissionId);
                return false;
            }
        }

        /// <summary>
        /// Creates DocuSign (or Adobe Sign) agreement if stage requires signing and agreement doesn't exist
        /// </summary>
        private async Task CreateAdobeSignAgreementIfNeededAsync(FORM_SUBMISSIONS submission, APPROVAL_STAGES stage)
        {
            try
            {
                // If the stage does not require e-sign, nothing to do
                if (!stage.RequiresAdobeSign)
                    return;

                // Get signer information (use submitted by user or first approver)
                var signerEmail = submission.SubmittedByUserId;
                var signerName = submission.SubmittedByUserId;

                // Try to get user email from identity context if SubmittedByUserId is numeric
                if (int.TryParse(submission.SubmittedByUserId, out int userId))
                {
                    var user = await _identityContext.TblUsers.FirstOrDefaultAsync(u => u.Id == userId);
                    if (user != null)
                    {
                        signerEmail = string.IsNullOrWhiteSpace(user.Email) ? user.Username : user.Email;
                        signerName = string.IsNullOrWhiteSpace(user.Name) ? user.Username : user.Name;
                    }
                }

                if (string.IsNullOrWhiteSpace(signerEmail))
                {
                    _logger?.LogWarning("Cannot create signing agreement: signer email not found for submission {SubmissionId}", submission.Id);
                    return;
                }

                if (_scopeFactory == null)
                {
                    _logger?.LogWarning("IServiceScopeFactory is null; cannot create signing agreement for submission {SubmissionId}", submission.Id);
                    return;
                }

                using var scope = _scopeFactory.CreateScope();
                var docuSignService = scope.ServiceProvider.GetService<IDocuSignService>();
                if (docuSignService == null)
                {
                    _logger?.LogWarning("IDocuSignService not registered in DI; skipping signing for submission {SubmissionId}", submission.Id);
                    return;
                }

                _logger?.LogInformation("Attempting to create signing envelope for submission {SubmissionId}, stage {StageId} using DocuSign", submission.Id, stage.Id);
                var created = await docuSignService.CreateSigningEnvelopeAsync(
                    submission.Id,
                    stage.Id,
                    signerEmail,
                    signerName,
                    submission.SubmittedByUserId ?? "system");

                if (created)
                {
                    _logger?.LogInformation("DocuSign envelope created for submission {SubmissionId}, stage {StageId}", submission.Id, stage.Id);
                }
                else
                {
                    _logger?.LogWarning("Failed to create DocuSign envelope for submission {SubmissionId}, stage {StageId}", submission.Id, stage.Id);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error creating signing agreement for submission {SubmissionId}, stage {StageId}", submission.Id, stage.Id);
                // Do not throw — failure to create agreement must not block workflow activation
            }
        }
    }
}