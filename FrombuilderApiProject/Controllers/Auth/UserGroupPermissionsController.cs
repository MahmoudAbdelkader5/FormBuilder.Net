using FormBuilder.API.Models;
using FormBuilder.Application.Dtos.Auth;
using FormBuilder.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace FormBuilder.API.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserGroupPermissionsController : ControllerBase
    {
        private readonly AkhmanageItContext _context;
        private readonly ILogger<UserGroupPermissionsController> _logger;
        private readonly IUserPermissionService _permissionService;

        public UserGroupPermissionsController(
            AkhmanageItContext context,
            ILogger<UserGroupPermissionsController> logger,
            IUserPermissionService permissionService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));
        }

        /// <summary>
        /// Get permissions by UserId
        /// Looks up user's groups from Tbl_UserGroup_User, then gets permissions from Tbl_UserGroup_Permission
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of permissions with userPermissionName</returns>
        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            _logger.LogInformation("[Permissions] Getting permissions for UserId: {UserId}", userId);

            try
            {
                // 1. Get user and verify exists
                var user = await _context.TblUsers
                    .Where(u => u.Id == userId && u.IsActive == true)
                    .Select(u => new { u.Id, u.Username })
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    _logger.LogWarning("[Permissions] User {UserId} not found or inactive", userId);
                    return Ok(new ApiResponse(200, "User not found", new List<object>()));
                }

                _logger.LogInformation("[Permissions] User {Username} (Id: {UserId}) found", user.Username, userId);

                // 2. Get user groups from Tbl_UserGroup_User (many-to-many relationship)
                var userGroupsFromTable = await _context.TblUserGroupUsers
                    .Include(ugu => ugu.IdUserGroupNavigation)
                    .Where(ugu => ugu.IdUser == userId && ugu.IdUserGroupNavigation.IsActive)
                    .Select(ugu => ugu.IdUserGroup)
                    .ToListAsync();

                // 3. Also check IdUserType from Tbl_User (direct relationship)
                var userWithType = await _context.TblUsers
                    .Where(u => u.Id == userId)
                    .Select(u => u.IdUserType)
                    .FirstOrDefaultAsync();

                var userGroups = new List<int>();
                
                // Add groups from TblUserGroupUsers
                if (userGroupsFromTable.Any())
                {
                    userGroups.AddRange(userGroupsFromTable);
                    _logger.LogInformation("[Permissions] Found {Count} groups from TblUserGroupUsers for User {UserId}",
                        userGroupsFromTable.Count, userId);
                }

                // Add IdUserType if exists and not already in the list
                if (userWithType > 0 && !userGroups.Contains(userWithType))
                {
                    // Verify the group is active
                    var groupExists = await _context.TblUserGroups
                        .AnyAsync(ug => ug.Id == userWithType && ug.IsActive);
                    
                    if (groupExists)
                    {
                        userGroups.Add(userWithType);
                        _logger.LogInformation("[Permissions] Found IdUserType {IdUserType} for User {UserId}",
                            userWithType, userId);
                    }
                }

                if (!userGroups.Any())
                {
                    _logger.LogWarning("[Permissions] User {UserId} has no active groups assigned (checked TblUserGroupUsers and IdUserType)", userId);
                    return Ok(new ApiResponse(200, "No permissions found", new List<object>()));
                }

                _logger.LogInformation("[Permissions] User {UserId} belongs to {Count} active groups: {Groups}",
                    userId, userGroups.Count, string.Join(", ", userGroups));

                // 4. Get permissions from Tbl_UserGroup_Permission
                var permissions = await _context.TblUserGroupPermissions
                    .Where(p => userGroups.Contains(p.IdUserGroup))
                    .Select(p => new { userPermissionName = p.UserPermissionName })
                    .Distinct()
                    .OrderBy(p => p.userPermissionName)
                    .ToListAsync();

                _logger.LogInformation("[Permissions] Found {Count} permissions for User {UserId}",
                    permissions.Count, userId);

                return Ok(new ApiResponse(200, "Permissions retrieved successfully", permissions));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Permissions] Error getting permissions for UserId: {UserId}", userId);
                return StatusCode(500, new ApiResponse(500, $"Error retrieving permissions: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get permissions by Username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>List of permissions with userPermissionName</returns>
        [HttpGet("by-username/{username}")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            _logger.LogInformation("[Permissions] Getting permissions for Username: {Username}", username);

            try
            {
                var user = await _context.TblUsers
                    .Where(u => u.Username == username && u.IsActive == true)
                    .Select(u => new { u.Id, u.Username })
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    _logger.LogWarning("[Permissions] User '{Username}' not found or inactive", username);
                    return Ok(new ApiResponse(200, "User not found", new List<object>()));
                }

                // Get user groups from TblUserGroupUsers
                var userGroupsFromTable = await _context.TblUserGroupUsers
                    .Include(ugu => ugu.IdUserGroupNavigation)
                    .Where(ugu => ugu.IdUser == user.Id && ugu.IdUserGroupNavigation.IsActive)
                    .Select(ugu => ugu.IdUserGroup)
                    .ToListAsync();

                // Also check IdUserType from Tbl_User
                var userWithType = await _context.TblUsers
                    .Where(u => u.Id == user.Id)
                    .Select(u => u.IdUserType)
                    .FirstOrDefaultAsync();

                var userGroups = new List<int>();
                
                if (userGroupsFromTable.Any())
                {
                    userGroups.AddRange(userGroupsFromTable);
                }

                if (userWithType > 0 && !userGroups.Contains(userWithType))
                {
                    var groupExists = await _context.TblUserGroups
                        .AnyAsync(ug => ug.Id == userWithType && ug.IsActive);
                    
                    if (groupExists)
                    {
                        userGroups.Add(userWithType);
                    }
                }

                if (!userGroups.Any())
                {
                    _logger.LogWarning("[Permissions] User '{Username}' has no active groups", username);
                    return Ok(new ApiResponse(200, "No permissions found", new List<object>()));
                }

                var permissions = await _context.TblUserGroupPermissions
                    .Where(p => userGroups.Contains(p.IdUserGroup))
                    .Select(p => new { userPermissionName = p.UserPermissionName })
                    .Distinct()
                    .OrderBy(p => p.userPermissionName)
                    .ToListAsync();

                _logger.LogInformation("[Permissions] Found {Count} permissions for '{Username}'",
                    permissions.Count, username);

                return Ok(new ApiResponse(200, "Permissions retrieved successfully", permissions));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Permissions] Error getting permissions for Username: {Username}", username);
                return StatusCode(500, new ApiResponse(500, $"Error retrieving permissions: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get permissions by UserGroupId directly
        /// </summary>
        /// <param name="userGroupId">User Group ID</param>
        /// <returns>List of permissions with userPermissionName</returns>
        [HttpGet("by-group/{userGroupId}")]
        public async Task<IActionResult> GetByUserGroup(int userGroupId)
        {
            _logger.LogInformation("[Permissions] Getting permissions for UserGroup: {UserGroupId}", userGroupId);

            try
            {
                // Verify group exists and is active
                var group = await _context.TblUserGroups
                    .Where(ug => ug.Id == userGroupId && ug.IsActive)
                    .FirstOrDefaultAsync();

                if (group == null)
                {
                    _logger.LogWarning("[Permissions] UserGroup {UserGroupId} not found or inactive", userGroupId);
                    return Ok(new ApiResponse(200, "User group not found", new List<object>()));
                }

                var permissions = await _context.TblUserGroupPermissions
                    .Where(p => p.IdUserGroup == userGroupId)
                    .Select(p => new { userPermissionName = p.UserPermissionName })
                    .Distinct()
                    .OrderBy(p => p.userPermissionName)
                    .ToListAsync();

                _logger.LogInformation("[Permissions] Found {Count} permissions for UserGroup {UserGroupId}",
                    permissions.Count, userGroupId);

                return Ok(new ApiResponse(200, "Permissions retrieved successfully", permissions));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Permissions] Error getting permissions for UserGroup: {UserGroupId}", userGroupId);
                return StatusCode(500, new ApiResponse(500, $"Error retrieving permissions: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get user details including groups for debugging
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>User details with groups information</returns>
        [HttpGet("debug/user/{userId}")]
        public async Task<IActionResult> GetUserDebugInfo(int userId)
        {
            _logger.LogInformation("[Permissions] Getting debug info for UserId: {UserId}", userId);

            try
            {
                var user = await _context.TblUsers
                    .Where(u => u.Id == userId)
                    .Select(u => new
                    {
                        u.Id,
                        u.Username,
                        u.Name,
                        u.IsActive,
                        u.IdUserType
                    })
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return Ok(new ApiResponse(200, "User not found", new { }));
                }

                // Get groups from TblUserGroupUsers
                var groupsFromTable = await _context.TblUserGroupUsers
                    .Include(ugu => ugu.IdUserGroupNavigation)
                    .Where(ugu => ugu.IdUser == userId)
                    .Select(ugu => new
                    {
                        ugu.IdUserGroup,
                        GroupName = ugu.IdUserGroupNavigation.Name,
                        GroupIsActive = ugu.IdUserGroupNavigation.IsActive
                    })
                    .ToListAsync();

                // Get group from IdUserType
                TblUserGroup? groupFromType = null;
                if (user.IdUserType > 0)
                {
                    groupFromType = await _context.TblUserGroups
                        .Where(ug => ug.Id == user.IdUserType)
                        .FirstOrDefaultAsync();
                }

                var debugInfo = new
                {
                    user = new
                    {
                        user.Id,
                        user.Username,
                        user.Name,
                        user.IsActive,
                        user.IdUserType
                    },
                    groupsFromTblUserGroupUsers = groupsFromTable,
                    groupFromIdUserType = groupFromType != null ? new
                    {
                        groupFromType.Id,
                        groupFromType.Name,
                        groupFromType.IsActive
                    } : null,
                    totalActiveGroups = groupsFromTable.Count(g => g.GroupIsActive) + (groupFromType?.IsActive == true ? 1 : 0)
                };

                return Ok(new ApiResponse(200, "Debug info retrieved successfully", debugInfo));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Permissions] Error getting debug info for UserId: {UserId}", userId);
                return StatusCode(500, new ApiResponse(500, $"Error: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get permissions by Role Name (UserGroup Name)
        /// </summary>
        /// <param name="roleName">Role/UserGroup Name</param>
        /// <returns>List of permissions with userPermissionName</returns>
        [HttpGet("by-role/{roleName}")]
        public async Task<IActionResult> GetByRoleName(string roleName)
        {
            _logger.LogInformation("[Permissions] Getting permissions for Role: {RoleName}", roleName);

            try
            {
                // Find UserGroup by name
                var userGroup = await _context.TblUserGroups
                    .Where(ug => (ug.Name == roleName || ug.ForeignName == roleName) && ug.IsActive == true)
                    .Select(ug => ug.Id)
                    .FirstOrDefaultAsync();

                if (userGroup == 0)
                {
                    _logger.LogWarning("[Permissions] UserGroup '{RoleName}' not found", roleName);
                    return Ok(new ApiResponse(200, "User group not found", new List<object>()));
                }

                var permissions = await _context.TblUserGroupPermissions
                    .Where(p => p.IdUserGroup == userGroup)
                    .Select(p => new { userPermissionName = p.UserPermissionName })
                    .Distinct()
                    .OrderBy(p => p.userPermissionName)
                    .ToListAsync();

                _logger.LogInformation("[Permissions] Found {Count} permissions for Role '{RoleName}'",
                    permissions.Count, roleName);

                return Ok(new ApiResponse(200, "Permissions retrieved successfully", permissions));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Permissions] Error getting permissions for Role: {RoleName}", roleName);
                return StatusCode(500, new ApiResponse(500, $"Error retrieving permissions: {ex.Message}"));
            }
        }

        /// <summary>
        /// Create permission for a user group (Tbl_UserGroup_Permission)
        /// Composite PK: (IdUserGroup, UserPermissionName)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Administration")]
        public async Task<IActionResult> Create([FromBody] CreateUserGroupPermissionDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse(400, "Invalid request"));

            if (dto.UserGroupId <= 0 || string.IsNullOrWhiteSpace(dto.PermissionName))
                return BadRequest(new ApiResponse(400, "UserGroupId and PermissionName are required"));

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var createdBy))
                return Unauthorized(new ApiResponse(401, "Invalid user token"));

            var permissionName = dto.PermissionName.Trim();

            // verify group exists (and active)
            var groupExists = await _context.TblUserGroups.AnyAsync(g => g.Id == dto.UserGroupId && g.IsActive);
            if (!groupExists)
                return NotFound(new ApiResponse(404, "User group not found or inactive"));

            // optionally verify permission exists in Tbl_UserPermission (if you want to restrict to predefined permissions)
            var permissionExists = await _context.TblUserPermissions.AnyAsync(p => p.Name == permissionName && p.IsActive);
            if (!permissionExists)
                return BadRequest(new ApiResponse(400, $"Permission '{permissionName}' not found or inactive"));

            var exists = await _context.TblUserGroupPermissions.AnyAsync(p =>
                p.IdUserGroup == dto.UserGroupId && p.UserPermissionName == permissionName);

            if (exists)
                return Ok(new ApiResponse(200, "Permission already assigned to the group"));

            var entity = new TblUserGroupPermission
            {
                IdUserGroup = dto.UserGroupId,
                UserPermissionName = permissionName,
                IdLegalEntity = dto.LegalEntityId,
                IdCreatedBy = createdBy,
                CreatedDate = DateTime.UtcNow
            };

            _context.TblUserGroupPermissions.Add(entity);
            await _context.SaveChangesAsync();

            _permissionService.InvalidateRolePermissionsCache(dto.UserGroupId);
            _permissionService.InvalidatePermissionMatrixCache();

            return StatusCode(201, new ApiResponse(201, "Permission assigned successfully", new
            {
                userGroupId = entity.IdUserGroup,
                permissionName = entity.UserPermissionName,
                legalEntityId = entity.IdLegalEntity,
                createdBy = entity.IdCreatedBy,
                createdDate = entity.CreatedDate
            }));
        }

        /// <summary>
        /// Update permission name for a user group
        /// (implemented as delete+insert due to composite PK)
        /// </summary>
        [HttpPut("{userGroupId:int}/{permissionName}")]
        [Authorize(Roles = "Administration")]
        public async Task<IActionResult> Update(int userGroupId, string permissionName, [FromBody] UpdateUserGroupPermissionDto dto)
        {
            if (userGroupId <= 0 || string.IsNullOrWhiteSpace(permissionName))
                return BadRequest(new ApiResponse(400, "Invalid route parameters"));

            if (dto == null || string.IsNullOrWhiteSpace(dto.NewPermissionName))
                return BadRequest(new ApiResponse(400, "NewPermissionName is required"));

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var createdBy))
                return Unauthorized(new ApiResponse(401, "Invalid user token"));

            var oldName = permissionName.Trim();
            var newName = dto.NewPermissionName.Trim();

            if (string.Equals(oldName, newName, StringComparison.OrdinalIgnoreCase))
                return Ok(new ApiResponse(200, "No changes"));

            var oldRow = await _context.TblUserGroupPermissions
                .FirstOrDefaultAsync(p => p.IdUserGroup == userGroupId && p.UserPermissionName == oldName);

            if (oldRow == null)
                return NotFound(new ApiResponse(404, "Permission assignment not found"));

            // ensure target permission exists
            var permissionExists = await _context.TblUserPermissions.AnyAsync(p => p.Name == newName && p.IsActive);
            if (!permissionExists)
                return BadRequest(new ApiResponse(400, $"Permission '{newName}' not found or inactive"));

            // ensure new composite key doesn't already exist
            var newExists = await _context.TblUserGroupPermissions.AnyAsync(p =>
                p.IdUserGroup == userGroupId && p.UserPermissionName == newName);

            if (newExists)
            {
                // delete old to avoid duplicates if caller is "renaming" to one that already exists
                _context.TblUserGroupPermissions.Remove(oldRow);
                await _context.SaveChangesAsync();

                _permissionService.InvalidateRolePermissionsCache(userGroupId);
                _permissionService.InvalidatePermissionMatrixCache();

                return Ok(new ApiResponse(200, "Updated successfully"));
            }

            // delete old + add new
            _context.TblUserGroupPermissions.Remove(oldRow);

            var newRow = new TblUserGroupPermission
            {
                IdUserGroup = userGroupId,
                UserPermissionName = newName,
                IdLegalEntity = dto.LegalEntityId ?? oldRow.IdLegalEntity,
                IdCreatedBy = createdBy,
                CreatedDate = DateTime.UtcNow
            };

            _context.TblUserGroupPermissions.Add(newRow);
            await _context.SaveChangesAsync();

            _permissionService.InvalidateRolePermissionsCache(userGroupId);
            _permissionService.InvalidatePermissionMatrixCache();

            return Ok(new ApiResponse(200, "Updated successfully", new
            {
                userGroupId = newRow.IdUserGroup,
                permissionName = newRow.UserPermissionName,
                legalEntityId = newRow.IdLegalEntity
            }));
        }

        /// <summary>
        /// Delete permission from a user group (composite key)
        /// </summary>
        [HttpDelete("{userGroupId:int}/{permissionName}")]
        [Authorize(Roles = "Administration")]
        public async Task<IActionResult> Delete(int userGroupId, string permissionName)
        {
            if (userGroupId <= 0 || string.IsNullOrWhiteSpace(permissionName))
                return BadRequest(new ApiResponse(400, "Invalid route parameters"));

            var name = permissionName.Trim();
            var row = await _context.TblUserGroupPermissions
                .FirstOrDefaultAsync(p => p.IdUserGroup == userGroupId && p.UserPermissionName == name);

            if (row == null)
                return NotFound(new ApiResponse(404, "Permission assignment not found"));

            _context.TblUserGroupPermissions.Remove(row);
            await _context.SaveChangesAsync();

            _permissionService.InvalidateRolePermissionsCache(userGroupId);
            _permissionService.InvalidatePermissionMatrixCache();

            return Ok(new ApiResponse(200, "Deleted successfully"));
        }

        /// <summary>
        /// Replace all permissions for a group with the provided list (sync)
        /// </summary>
        [HttpPut("by-group/{userGroupId:int}/sync")]
        [Authorize(Roles = "Administration")]
        public async Task<IActionResult> SyncGroupPermissions(int userGroupId, [FromBody] SyncUserGroupPermissionsDto dto)
        {
            if (userGroupId <= 0)
                return BadRequest(new ApiResponse(400, "Invalid userGroupId"));

            dto ??= new SyncUserGroupPermissionsDto();

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var createdBy))
                return Unauthorized(new ApiResponse(401, "Invalid user token"));

            // verify group exists (and active)
            var groupExists = await _context.TblUserGroups.AnyAsync(g => g.Id == userGroupId && g.IsActive);
            if (!groupExists)
                return NotFound(new ApiResponse(404, "User group not found or inactive"));

            var desired = (dto.PermissionNames ?? new List<string>())
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Select(p => p.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            // ensure all desired permissions exist and are active
            if (desired.Any())
            {
                var activePermissionNames = await _context.TblUserPermissions
                    .Where(p => p.IsActive)
                    .Select(p => p.Name)
                    .ToListAsync();

                var activeSet = new HashSet<string>(activePermissionNames, StringComparer.OrdinalIgnoreCase);
                var invalid = desired.Where(p => !activeSet.Contains(p)).ToList();
                if (invalid.Any())
                    return BadRequest(new ApiResponse(400, "Some permissions are invalid/inactive", invalid));
            }

            var existing = await _context.TblUserGroupPermissions
                .Where(p => p.IdUserGroup == userGroupId)
                .ToListAsync();

            var existingSet = new HashSet<string>(existing.Select(e => e.UserPermissionName), StringComparer.OrdinalIgnoreCase);
            var desiredSet = new HashSet<string>(desired, StringComparer.OrdinalIgnoreCase);

            var toRemove = existing.Where(e => !desiredSet.Contains(e.UserPermissionName)).ToList();
            var toAdd = desired.Where(p => !existingSet.Contains(p)).ToList();

            if (toRemove.Any())
                _context.TblUserGroupPermissions.RemoveRange(toRemove);

            if (toAdd.Any())
            {
                foreach (var p in toAdd)
                {
                    _context.TblUserGroupPermissions.Add(new TblUserGroupPermission
                    {
                        IdUserGroup = userGroupId,
                        UserPermissionName = p,
                        IdLegalEntity = dto.LegalEntityId,
                        IdCreatedBy = createdBy,
                        CreatedDate = DateTime.UtcNow
                    });
                }
            }

            await _context.SaveChangesAsync();

            _permissionService.InvalidateRolePermissionsCache(userGroupId);
            _permissionService.InvalidatePermissionMatrixCache();

            return Ok(new ApiResponse(200, "Synced successfully", new
            {
                userGroupId,
                added = toAdd.Count,
                removed = toRemove.Count,
                total = desired.Count
            }));
        }
    }
}

