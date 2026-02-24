using FormBuilder.Application.Dtos.Auth;
using FormBuilder.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserPermissionController : ControllerBase
{
    private readonly IUserPermissionService _permissionService;
    private readonly IStringLocalizer<UserPermissionController> _localizer;
    private readonly AkhmanageItContext _context;

    public UserPermissionController(
        IUserPermissionService permissionService,
        IStringLocalizer<UserPermissionController> localizer,
        AkhmanageItContext context)
    {
        _permissionService = permissionService;
        _localizer = localizer;
        _context = context;
    }

    [HttpGet]
    [Authorize(Roles = "Administration")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var permissions = await _permissionService.GetAllAsync();
        return Ok(permissions);
    }

    [HttpGet("user/{userId}")]
    [Authorize(Roles = "Administration")]
    public async Task<IActionResult> GetUserPermissions(int userId, CancellationToken cancellationToken = default)
    {
        var permissions = await _permissionService.GetUserPermissionsAsync(userId);
        return Ok(permissions);
    }

    [HttpGet("current-user")]
    public async Task<IActionResult> GetCurrentUserPermissions(CancellationToken cancellationToken = default)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized(new { message = _localizer["Common_InvalidUserToken"] });

        var permissions = await _permissionService.GetUserPermissionsAsync(userId);
        return Ok(permissions);
    }

    [HttpPost("check")]
    public async Task<IActionResult> CheckPermission([FromBody] CheckPermissionRequestDto request, CancellationToken cancellationToken = default)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized(new { message = _localizer["Common_InvalidUserToken"] });

        var hasPermission = await _permissionService.HasPermissionAsync(userId, request.PermissionName);
        return Ok(new { hasPermission, permissionName = request.PermissionName });
    }

    [HttpPost("check-multiple")]
    public async Task<IActionResult> CheckMultiplePermissions([FromBody] CheckPermissionsRequestDto request, CancellationToken cancellationToken = default)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized(new { message = _localizer["Common_InvalidUserToken"] });

        var results = await _permissionService.CheckMultiplePermissionsAsync(userId, request.PermissionNames);
        return Ok(results);
    }

    [HttpGet("role/{roleId}")]
    [Authorize(Roles = "Administration")]
    public async Task<IActionResult> GetRolePermissions(int roleId, CancellationToken cancellationToken = default)
    {
        var permissions = await _permissionService.GetRolePermissionsAsync(roleId);
        return Ok(permissions);
    }

    [HttpGet("matrix")]
    [Authorize(Roles = "Administration")]
    public async Task<IActionResult> GetPermissionMatrix(CancellationToken cancellationToken = default)
    {
        var matrix = await _permissionService.GetPermissionMatrixAsync();
        return Ok(matrix);
    }

    /// <summary>
    /// Create a new permission in Tbl_UserPermission
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Administration")]
    public async Task<IActionResult> Create([FromBody] CreatePermissionRequestDto dto, CancellationToken cancellationToken = default)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest(new { message = "Name is required" });

        var name = dto.Name.Trim();

        // ensure permission does not already exist (PK = Name)
        var exists = await _context.TblUserPermissions.AnyAsync(p => p.Name == name);
        if (exists)
            return BadRequest(new { message = $"Permission '{name}' already exists" });

        var entity = new TblUserPermission
        {
            Name = name,
            Description = dto.Description,
            ScreenName = dto.ScreenName,
            IdLegalEntity = dto.LegalEntityId,
            IsActive = true
        };

        _context.TblUserPermissions.Add(entity);
        await _context.SaveChangesAsync();

        _permissionService.InvalidatePermissionMatrixCache();

        return StatusCode(201, entity);
    }
}
