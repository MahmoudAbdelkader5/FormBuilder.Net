using FormBuilder.Core.DTOS.FormBuilder;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IDocumentNumberGeneratorService
    {
        Task<DocumentNumberGenerationResultDto> GenerateForSubmissionAsync(
            int submissionId,
            string generatedOn,
            string? generatedByUserId = null);
    }
}
