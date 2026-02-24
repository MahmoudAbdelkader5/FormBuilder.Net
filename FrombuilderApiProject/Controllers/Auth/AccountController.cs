using FormBuilder.Application.Abstractions;
using FormBuilder.Application.Dtos.Auth;
using FormBuilder.API.Models;
using FormBuilder.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly IaccountService _accountService;
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly AkhmanageItContext _context;

        public AccountController(
            IaccountService accountService, 
            IStringLocalizer<AccountController> localizer,
            AkhmanageItContext context)
        {
            _accountService = accountService;
            _localizer = localizer;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _accountService.LoginAsync(request.Username, request.Password, cancellationToken);

            if (!response.Success)
                return Unauthorized(new { message = _localizer["Login_InvalidCredentials"] });

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _accountService.RefreshTokenAsync(request.RefreshToken, cancellationToken);

            if (!response.Success)
                return Unauthorized(new { response.ErrorMessage });

            return Ok(response);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _accountService.LogoutAsync(request.RefreshToken, cancellationToken);

            if (!result)
                return BadRequest(new { message = _localizer["Auth_InvalidRefreshToken"] });

            return Ok(new { message = _localizer["Auth_LoggedOut"] });
        }

        [HttpPost("revoke-all-tokens/{userId}")]
        [Authorize(Roles = "Administration")]
        public async Task<IActionResult> RevokeAllUserTokens(int userId, CancellationToken cancellationToken)
        {
            if (userId <= 0)
                return BadRequest(new { message = _localizer["Auth_InvalidUserId"] });

            var result = await _accountService.RevokeAllUserTokensAsync(userId, cancellationToken);

            if (!result)
                return BadRequest(new { message = _localizer["Auth_RevokeFailed"] });

            return Ok(new { message = _localizer["Auth_RevokeSuccess"] });
        }

        [HttpGet("current-user")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized(new { message = _localizer["Auth_InvalidUserToken"] });

            var userInfo = await _accountService.GetCurrentUserAsync(userId, cancellationToken);

            if (userInfo == null)
                return NotFound(new { message = _localizer["Auth_UserNotFound"] });

            return Ok(userInfo);
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized(new { message = _localizer["Auth_InvalidUserToken"] });

            var result = await _accountService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword, cancellationToken);

            if (!result)
                return BadRequest(new { message = _localizer["Auth_ChangePasswordFailed"] });

            return Ok(new { message = _localizer["Auth_ChangePasswordSuccess"] });
        }

        [HttpPut("update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized(new { message = _localizer["Auth_InvalidUserToken"] });

            var result = await _accountService.UpdateUserProfileAsync(userId, request, cancellationToken);

            if (!result)
                return BadRequest(new { message = "Failed to update user profile" });

            // Return updated user info
            var userInfo = await _accountService.GetCurrentUserAsync(userId, cancellationToken);
            return Ok(new { message = "Profile updated successfully", user = userInfo });
        }

        [HttpPut("update-profile/{userId}")]
        [Authorize(Roles = "Administration")]
        public async Task<IActionResult> UpdateUserProfile(int userId, [FromBody] UpdateUserProfileDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (userId <= 0)
                return BadRequest(new { message = "Invalid user ID" });

            var result = await _accountService.UpdateUserProfileAsync(userId, request, cancellationToken);

            if (!result)
                return BadRequest(new { message = "Failed to update user profile. User may not exist or is inactive." });

            // Return updated user info
            var userInfo = await _accountService.GetCurrentUserAsync(userId, cancellationToken);
            return Ok(new { message = "User profile updated successfully", user = userInfo });
        }

        /// <summary>
        /// Get active users (alternative route for /api/Users/active)
        /// </summary>
        [HttpGet("Users/active")]
        [Authorize]
        public async Task<IActionResult> GetActiveUsers(CancellationToken cancellationToken = default)
        {
            var users = await _context.TblUsers
                .Where(u => u.IsActive)
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Name,
                    u.Email,
                    u.Phone,
                    u.IsActive,
                    u.CreatedDate,
                    u.UpdatedDate
                })
                .ToListAsync();

            return Ok(new ApiResponse(200, "Active users retrieved successfully", users));
        }

        /// <summary>
        /// Get active user groups (alternative route for /api/UserGroups/active)
        /// </summary>
        [HttpGet("UserGroups/active")]
        [Authorize]
        public async Task<IActionResult> GetActiveUserGroups(CancellationToken cancellationToken = default)
        {
            var userGroups = await _context.TblUserGroups
                .Where(ug => ug.IsActive)
                .Select(ug => new
                {
                    ug.Id,
                    ug.Name,
                    ug.ForeignName,
                    ug.Description,
                    ug.IsActive,
                    ug.CreatedDate,
                    ug.UpdatedDate
                })
                .ToListAsync();

            return Ok(new ApiResponse(200, "Active user groups retrieved successfully", userGroups));
        }
    }
}
