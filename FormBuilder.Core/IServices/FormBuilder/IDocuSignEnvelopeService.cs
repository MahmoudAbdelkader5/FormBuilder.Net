using FormBuilder.Core.DTOS.FormBuilder;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IDocuSignEnvelopeService
    {
        Task<string> CreateEnvelopeAsync(DocuSignEnvelopeRequestDto request);
        Task<string> CreateRecipientViewAsync(DocuSignRecipientViewRequestDto request);
    }
}

