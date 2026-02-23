using FormBuilder.Core.Models;

namespace FormBuilder.Application.Dtos.Auth
{
    public class PermissionMatrixDto
    {
        public List<PermissionInfoDto> Permissions { get; set; } = new();
        public List<RolePermissionDto> RolePermissions { get; set; } = new();
    }

    public class PermissionInfoDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ScreenName { get; set; }
        public bool IsActive { get; set; }
    }

    public class RolePermissionDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public List<string> Permissions { get; set; } = new();
    }

    public class UserPermissionDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public List<string> RolePermissions { get; set; } = new();
        public List<string> OverridePermissions { get; set; } = new();
        public List<string> AllPermissions { get; set; } = new();
    }

    public class CheckPermissionRequestDto
    {
        public string PermissionName { get; set; } = string.Empty;
    }

    public class CheckPermissionsRequestDto
    {
        public List<string> PermissionNames { get; set; } = new();
    }

    public class CreatePermissionRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ScreenName { get; set; }
        public int? LegalEntityId { get; set; }
    }
}
