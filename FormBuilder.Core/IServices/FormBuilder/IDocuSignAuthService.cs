using FormBuilder.Core.DTOS.FormBuilder;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IDocuSignAuthService
    {
        Task<string> GetAccessTokenAsync();
        Task<DocuSignAccountInfoDto> GetAccountInfoAsync(string accessToken);
    }
}

