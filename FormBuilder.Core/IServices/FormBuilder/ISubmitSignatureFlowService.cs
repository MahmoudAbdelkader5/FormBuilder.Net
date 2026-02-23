using FormBuilder.Core.DTOS.FormBuilder;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface ISubmitSignatureFlowService
    {
        Task<SubmitSignatureFlowResultDto> ExecuteAsync(SubmitSignatureFlowInputDto input);
    }
}

