using FormBuilder.Application.Dtos.Auth;
using System.Threading;
using System.Threading.Tasks;

namespace FormBuilder.Application.Abstractions;

public interface IaccountService
{
    Task<LoginResponseDto> LoginAsync(string username, string password, CancellationToken cancellationToken);
    Task<RefreshTokenResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    Task<bool> LogoutAsync(string refreshToken, CancellationToken cancellationToken);
    Task<bool> RevokeAllUserTokensAsync(int userId, CancellationToken cancellationToken);
    Task<UserInfoDto?> GetCurrentUserAsync(int userId, CancellationToken cancellationToken);
    Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword, CancellationToken cancellationToken);
    Task<bool> UpdateUserProfileAsync(int userId, UpdateUserProfileDto profile, CancellationToken cancellationToken);
}
