using FormBuilder.Application.Dtos.Auth;
using FormBuilder.Core.Models;

public interface IUserPermissionService
{
    Task<IEnumerable<TblUserPermission>> GetAllAsync();
    Task<IEnumerable<string>> GetUserPermissionsAsync(int userId);
    Task<bool> HasPermissionAsync(int userId, string permissionName);
    Task<Dictionary<string, bool>> CheckMultiplePermissionsAsync(int userId, IEnumerable<string> permissionNames);
    Task<IEnumerable<TblUserGroupPermission>> GetRolePermissionsAsync(int roleId);
    Task<PermissionMatrixDto> GetPermissionMatrixAsync();

    void InvalidateRolePermissionsCache(int roleId);
    void InvalidatePermissionMatrixCache();
}
