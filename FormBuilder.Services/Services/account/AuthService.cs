using FormBuilder.Application.Abstractions;
using FormBuilder.Application.Dtos.Auth;
using FormBuilder.Core.Models;
using formBuilder.Domian.Entitys;
using FormBuilder.Infrastructure.Data;          // <-- مهم: لإحضار FormBuilderDbContext
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class accountService : IaccountService
{
    private readonly AkhmanageItContext _identityContext;       // كان _context
    private readonly FormBuilderDbContext _formBuilderContext;  // جديد
    private readonly IServer _server;
    private readonly IConfiguration _configuration;

    public accountService(
        AkhmanageItContext identityContext,
        FormBuilderDbContext formBuilderContext,
        IServer server,
        IConfiguration configuration)
    {
        _identityContext = identityContext;
        _formBuilderContext = formBuilderContext;
        _server = server;
        _configuration = configuration;
    }

    // دالة SHA512
    private static string HashPassword(string password)
    {
        using (SHA512 sha = SHA512.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder(128);
            foreach (byte b in hash)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }

    public async Task<LoginResponseDto> LoginAsync(string username, string password, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return new LoginResponseDto { Success = false, ErrorMessage = "Username and password are required." };

        string hashedPassword = HashPassword(password);

        // ابحث عن المستخدم أولاً بدون التحقق من كلمة المرور (case-insensitive username)
        var user = await _identityContext.TblUsers
            .Include(u => u.TblUserGroupUsers)
                .ThenInclude(ugu => ugu.IdUserGroupNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username.Trim() == username.Trim(), cancellationToken);

        if (user == null)
            return new LoginResponseDto { Success = false, ErrorMessage = "User not found." };

        if (!user.IsActive)
            return new LoginResponseDto { Success = false, ErrorMessage = "User account is inactive." };

        // التحقق من كلمة المرور - مقارنة case-sensitive
        var storedPassword = (user.Password ?? "").Trim();
        if (storedPassword != hashedPassword)
            return new LoginResponseDto { Success = false, ErrorMessage = "Invalid password." };

        // Resolve primary user group deterministically:
        // 1) Prefer Tbl_User.IdUserType (authoritative role for this user)
        // 2) Fallback to active Tbl_UserGroup_User links
        var activeUserGroups = user.TblUserGroupUsers
            .Where(ugu => ugu.IdUserGroupNavigation.IsActive)
            .Select(ugu => ugu.IdUserGroupNavigation)
            .ToList();

        var userGroup = activeUserGroups.FirstOrDefault(g => g.Id == user.IdUserType)
            ?? activeUserGroups.FirstOrDefault();

        if (userGroup == null && user.IdUserType > 0)
        {
            userGroup = await _identityContext.TblUserGroups
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == user.IdUserType && g.IsActive, cancellationToken);
        }

        var roleName = userGroup?.Name ?? "User"; // Default role
        var userGroupId = userGroup?.Id ?? (user.IdUserType > 0 ? user.IdUserType : 0);

        var now = DateTime.UtcNow;
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, roleName),
            new Claim("userGroupId", userGroupId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // لا نضيف Permissions في Token لتقليل الحجم
        // سيتم التحقق من Permissions من PermissionService عند الحاجة

        var expiryMinutes = Convert.ToDouble(jwtSettings["ExpiryMinutes"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            NotBefore = now,
            Expires = now.AddMinutes(expiryMinutes).AddSeconds(1),
            SigningCredentials = creds,
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        // Generate refresh token
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiryDays = int.TryParse(jwtSettings["RefreshTokenExpiryDays"], out var days) ? days : 7;
        var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);

        // Save refresh token to FormBuilderDbContext
        var refreshTokenEntity = new REFRESH_TOKENS
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = refreshTokenExpiresAt,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _formBuilderContext.Set<REFRESH_TOKENS>().Add(refreshTokenEntity);
        
        // Revoke old refresh tokens for this user (keep only the latest 5) قبل SaveChanges
        await RevokeOldRefreshTokensAsync(user.Id, cancellationToken);
        
        // حفظ كل التغييرات في SaveChanges واحد
        await _formBuilderContext.SaveChangesAsync(cancellationToken);

        return new LoginResponseDto
        {
            Success = true,
            Token = tokenString,
            RefreshToken = refreshToken,
            Role = roleName,
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            Name = user.Name,
            ExpiresAt = tokenDescriptor.Expires!.Value,
            RefreshTokenExpiresAt = refreshTokenExpiresAt
        };
    }

    public async Task<RefreshTokenResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        // من FormBuilderDbContext
        var tokenEntity = await _formBuilderContext.Set<REFRESH_TOKENS>()
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.IsActive, cancellationToken);

        if (tokenEntity == null || tokenEntity.ExpiresAt < DateTime.UtcNow || tokenEntity.RevokedAt != null)
        {
            return new RefreshTokenResponseDto
            {
                Success = false,
                ErrorMessage = "Invalid or expired refresh token."
            };
        }

        // Get user مع UserGroup في استعلام واحد لتجنب N+1 Query
        var user = await _identityContext.TblUsers
            .Include(u => u.TblUserGroupUsers)
                .ThenInclude(ugu => ugu.IdUserGroupNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == tokenEntity.UserId && u.IsActive, cancellationToken);

        if (user == null)
        {
            return new RefreshTokenResponseDto
            {
                Success = false,
                ErrorMessage = "User not found."
            };
        }

        // Resolve primary group using IdUserType first (same logic as login)
        var activeUserGroups = user.TblUserGroupUsers
            .Where(ugu => ugu.IdUserGroupNavigation.IsActive)
            .Select(ugu => ugu.IdUserGroupNavigation)
            .ToList();

        var userGroup = activeUserGroups.FirstOrDefault(g => g.Id == user.IdUserType)
            ?? activeUserGroups.FirstOrDefault();

        if (userGroup == null && user.IdUserType > 0)
        {
            userGroup = await _identityContext.TblUserGroups
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == user.IdUserType && g.IsActive, cancellationToken);
        }

        var roleName = userGroup?.Name ?? "User";
        var userGroupId = userGroup?.Id ?? (user.IdUserType > 0 ? user.IdUserType : 0);

        // Generate new access token
        var now = DateTime.UtcNow;
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, roleName),
            new Claim("userGroupId", userGroupId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // لا نضيف Permissions في Token لتقليل الحجم
        // سيتم التحقق من Permissions من PermissionService عند الحاجة

        var expiryMinutes = Convert.ToDouble(jwtSettings["ExpiryMinutes"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            NotBefore = now,
            Expires = now.AddMinutes(expiryMinutes).AddSeconds(1),
            SigningCredentials = creds,
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        // Rotate refresh token في FormBuilderDbContext
        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiryDays = int.TryParse(jwtSettings["RefreshTokenExpiryDays"], out var days) ? days : 7;
        var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);

        // Revoke old refresh token
        tokenEntity.RevokedAt = DateTime.UtcNow;
        tokenEntity.IsActive = false;

        // Create new refresh token
        var newRefreshTokenEntity = new REFRESH_TOKENS
        {
            UserId = user.Id,
            Token = newRefreshToken,
            ExpiresAt = refreshTokenExpiresAt,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _formBuilderContext.Set<REFRESH_TOKENS>().Add(newRefreshTokenEntity);
        
        // Revoke old refresh tokens قبل SaveChanges
        await RevokeOldRefreshTokensAsync(user.Id, cancellationToken);
        
        await _formBuilderContext.SaveChangesAsync(cancellationToken);

        return new RefreshTokenResponseDto
        {
            Success = true,
            Token = tokenString,
            RefreshToken = newRefreshToken,
            ExpiresAt = tokenDescriptor.Expires!.Value,
            RefreshTokenExpiresAt = refreshTokenExpiresAt
        };
    }

    public async Task<bool> LogoutAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var tokenEntity = await _formBuilderContext.Set<REFRESH_TOKENS>()
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.IsActive, cancellationToken);

        if (tokenEntity != null)
        {
            tokenEntity.RevokedAt = DateTime.UtcNow;
            tokenEntity.IsActive = false;
            await _formBuilderContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        return false;
    }

    public async Task<bool> RevokeAllUserTokensAsync(int userId, CancellationToken cancellationToken)
    {
        var activeTokens = await _formBuilderContext.Set<REFRESH_TOKENS>()
            .Where(rt => rt.UserId == userId && rt.IsActive && rt.RevokedAt == null)
            .ToListAsync(cancellationToken);

        if (activeTokens.Any())
        {
            var now = DateTime.UtcNow;
            foreach (var token in activeTokens)
            {
                token.RevokedAt = now;
                token.IsActive = false;
            }

            await _formBuilderContext.SaveChangesAsync(cancellationToken);
        }

        return true;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<UserInfoDto?> GetCurrentUserAsync(int userId, CancellationToken cancellationToken)
    {
        var user = await _identityContext.TblUsers
            .Include(u => u.TblUserGroupUsers)
                .ThenInclude(ugu => ugu.IdUserGroupNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId && u.IsActive, cancellationToken);

        if (user == null)
            return null;

        var activeUserGroups = user.TblUserGroupUsers
            .Where(ugu => ugu.IdUserGroupNavigation.IsActive)
            .Select(ugu => ugu.IdUserGroupNavigation)
            .ToList();

        var userGroup = activeUserGroups.FirstOrDefault(g => g.Id == user.IdUserType)
            ?? activeUserGroups.FirstOrDefault();

        if (userGroup == null && user.IdUserType > 0)
        {
            userGroup = await _identityContext.TblUserGroups
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == user.IdUserType && g.IsActive, cancellationToken);
        }

        return new UserInfoDto
        {
            Id = user.Id,
            Username = user.Username,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            Role = userGroup?.Name ?? "User",
            IsActive = user.IsActive
        };
    }

    public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword, CancellationToken cancellationToken)
    {
        var hashedCurrentPassword = HashPassword(currentPassword);
        var hashedNewPassword = HashPassword(newPassword);

        var user = await _identityContext.TblUsers
            .FirstOrDefaultAsync(u => u.Id == userId && u.Password == hashedCurrentPassword && u.IsActive, cancellationToken);

        if (user == null)
            return false;

        user.Password = hashedNewPassword;
        user.UpdatedDate = DateTime.UtcNow;

        await _identityContext.SaveChangesAsync(cancellationToken);

        // Revoke all existing tokens for security
        await RevokeAllUserTokensAsync(userId, cancellationToken);

        return true;
    }

    public async Task<bool> UpdateUserProfileAsync(int userId, UpdateUserProfileDto profile, CancellationToken cancellationToken)
    {
        var user = await _identityContext.TblUsers
            .FirstOrDefaultAsync(u => u.Id == userId && u.IsActive, cancellationToken);

        if (user == null)
            return false;

        // Update only provided fields
        if (profile.Name != null)
            user.Name = profile.Name;

        if (profile.Email != null)
            user.Email = profile.Email;

        if (profile.Phone != null)
            user.Phone = profile.Phone;

        user.UpdatedDate = DateTime.UtcNow;

        await _identityContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    // تم إزالة GetUserPermissionsForClaimsAsync
    // Permissions لن تُضاف في JWT Token لتقليل الحجم
    // سيتم التحقق من Permissions من UserPermissionService عند الحاجة

    private async Task RevokeOldRefreshTokensAsync(int userId, CancellationToken cancellationToken)
    {
        // Keep only the latest 5 active refresh tokens per user
        var activeTokens = await _formBuilderContext.Set<REFRESH_TOKENS>()
            .Where(rt => rt.UserId == userId && rt.IsActive && rt.RevokedAt == null)
            .OrderByDescending(rt => rt.CreatedAt)
            .ToListAsync(cancellationToken);

        if (activeTokens.Count > 5)
        {
            var now = DateTime.UtcNow;
            var tokensToRevoke = activeTokens.Skip(5);
            foreach (var token in tokensToRevoke)
            {
                token.RevokedAt = now;
                token.IsActive = false;
            }
            // لا نحفظ هنا - سيتم الحفظ في SaveChanges الرئيسي
        }
    }
}
