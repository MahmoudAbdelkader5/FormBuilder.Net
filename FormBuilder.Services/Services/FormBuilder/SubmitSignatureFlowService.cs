using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domain.Interfaces.Services;

namespace FormBuilder.Services.Services.FormBuilder
{
    public class SubmitSignatureFlowService : ISubmitSignatureFlowService
    {
        private readonly IDocuSignEnvelopeService _docuSignEnvelopeService;

        public SubmitSignatureFlowService(IDocuSignEnvelopeService docuSignEnvelopeService)
        {
            _docuSignEnvelopeService = docuSignEnvelopeService;
        }

        public async Task<SubmitSignatureFlowResultDto> ExecuteAsync(SubmitSignatureFlowInputDto input)
        {
            if (!input.SignatureRequired)
            {
                return new SubmitSignatureFlowResultDto
                {
                    SignatureRequired = false,
                    SignatureStatus = "not_required",
                    SigningUrl = null,
                    EnvelopeId = null,
                    DocuSignCalled = false
                };
            }

            var envelopeId = input.ExistingEnvelopeId;
            var called = false;
            if (string.IsNullOrWhiteSpace(envelopeId))
            {
                envelopeId = await _docuSignEnvelopeService.CreateEnvelopeAsync(new DocuSignEnvelopeRequestDto
                {
                    SubmissionId = input.SubmissionId,
                    DocumentNumber = input.DocumentNumber,
                    Signer = input.Signer
                });
                called = true;
            }

            var signingUrl = await _docuSignEnvelopeService.CreateRecipientViewAsync(new DocuSignRecipientViewRequestDto
            {
                EnvelopeId = envelopeId!,
                ReturnUrl = input.ReturnUrl,
                Signer = input.Signer
            });
            called = true;

            return new SubmitSignatureFlowResultDto
            {
                SignatureRequired = true,
                SignatureStatus = "pending",
                SigningUrl = signingUrl,
                EnvelopeId = envelopeId,
                DocuSignCalled = called
            };
        }
    }
}

