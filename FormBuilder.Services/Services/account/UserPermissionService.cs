using FormBuilder.Core.Models;
using FormBuilder.Application.Dtos.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

public class UserPermissionService : IUserPermissionService
{
    private readonly AkhmanageItContext _context;
    private readonly IMemoryCache _cache;
    private const string CACHE_KEY_ALL_PERMISSIONS = "AllPermissions";
    private const string CACHE_KEY_USER_PERMISSIONS = "UserPermissions_{0}";
    private const string CACHE_KEY_ROLE_PERMISSIONS = "RolePermissions_{0}";
    private const string CACHE_KEY_PERMISSION_MATRIX = "PermissionMatrix";
    private static readonly TimeSpan CACHE_DURATION = TimeSpan.FromMinutes(30);

    public UserPermissionService(AkhmanageItContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    async Task<IEnumerable<TblUserPermission>> IUserPermissionService.GetAllAsync()
    {
        // محاولة جلب البيانات من Cache أولاً
        if (_cache.TryGetValue(CACHE_KEY_ALL_PERMISSIONS, out IEnumerable<TblUserPermission>? cachedPermissions))
        {
            return cachedPermissions ?? Enumerable.Empty<TblUserPermission>();
        }

        // إذا لم تكن في Cache، جلب من قاعدة البيانات
        var permissions = await _context.TblUserPermissions
                             .Where(p => p.IsActive) // اختياري: فقط النشطة
                             .AsNoTracking() // للقراءة فقط - يحسن الأداء
                             .ToListAsync();

        // حفظ في Cache
        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CACHE_DURATION,
            SlidingExpiration = TimeSpan.FromMinutes(10),
            Priority = CacheItemPriority.Normal
        };

        _cache.Set(CACHE_KEY_ALL_PERMISSIONS, permissions, cacheOptions);

        return permissions;
    }

    public async Task<IEnumerable<string>> GetUserPermissionsAsync(int userId)
    {
        var cacheKey = string.Format(CACHE_KEY_USER_PERMISSIONS, userId);
        
        // محاولة جلب من Cache
        if (_cache.TryGetValue(cacheKey, out IEnumerable<string>? cachedPermissions))
        {
            return cachedPermissions ?? Enumerable.Empty<string>();
        }

        // جلب permissions من Roles
        var user = await _context.TblUsers
            .Include(u => u.TblUserGroupUsers)
                .ThenInclude(ugu => ugu.IdUserGroupNavigation)
                    .ThenInclude(ug => ug.TblUserGroupPermissions)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);

        if (user == null)
            return Enumerable.Empty<string>();

        var rolePermissions = user.TblUserGroupUsers
            .Where(ugu => ugu.IdUserGroupNavigation.IsActive)
            .SelectMany(ugu => ugu.IdUserGroupNavigation.TblUserGroupPermissions)
            .Where(ugp => !string.IsNullOrEmpty(ugp.UserPermissionName))
            .Select(ugp => ugp.UserPermissionName)
            .Distinct()
            .ToList();

        // TODO: إضافة User Override Permissions هنا عند إضافة الجدول

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CACHE_DURATION,
            SlidingExpiration = TimeSpan.FromMinutes(10),
            Priority = CacheItemPriority.Normal
        };

        _cache.Set(cacheKey, rolePermissions, cacheOptions);

        return rolePermissions;
    }

    public async Task<bool> HasPermissionAsync(int userId, string permissionName)
    {
        if (string.IsNullOrWhiteSpace(permissionName))
            return false;

        var userPermissions = await GetUserPermissionsAsync(userId);
        return userPermissions.Contains(permissionName, StringComparer.OrdinalIgnoreCase);
    }

    public async Task<Dictionary<string, bool>> CheckMultiplePermissionsAsync(int userId, IEnumerable<string> permissionNames)
    {
        var userPermissions = await GetUserPermissionsAsync(userId);
        var userPermissionsSet = new HashSet<string>(userPermissions, StringComparer.OrdinalIgnoreCase);

        var result = new Dictionary<string, bool>();
        foreach (var permissionName in permissionNames)
        {
            if (!string.IsNullOrWhiteSpace(permissionName))
            {
                result[permissionName] = userPermissionsSet.Contains(permissionName);
            }
        }

        return result;
    }

    public async Task<IEnumerable<TblUserGroupPermission>> GetRolePermissionsAsync(int roleId)
    {
        var cacheKey = string.Format(CACHE_KEY_ROLE_PERMISSIONS, roleId);

        // محاولة جلب من Cache
        if (_cache.TryGetValue(cacheKey, out IEnumerable<TblUserGroupPermission>? cachedPermissions))
        {
            return cachedPermissions ?? Enumerable.Empty<TblUserGroupPermission>();
        }

        var permissions = await _context.TblUserGroupPermissions
            .Where(ugp => ugp.IdUserGroup == roleId)
            .AsNoTracking()
            .ToListAsync();

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CACHE_DURATION,
            SlidingExpiration = TimeSpan.FromMinutes(10),
            Priority = CacheItemPriority.Normal
        };

        _cache.Set(cacheKey, permissions, cacheOptions);

        return permissions;
    }

    public async Task<PermissionMatrixDto> GetPermissionMatrixAsync()
    {
        // محاولة جلب من Cache
        if (_cache.TryGetValue(CACHE_KEY_PERMISSION_MATRIX, out PermissionMatrixDto? cachedMatrix))
        {
            return cachedMatrix ?? new PermissionMatrixDto();
        }

        // جلب جميع Permissions
        var allPermissions = await _context.TblUserPermissions
            .Where(p => p.IsActive)
            .AsNoTracking()
            .ToListAsync();

        // جلب جميع Roles مع Permissions
        var roles = await _context.TblUserGroups
            .Include(ug => ug.TblUserGroupPermissions)
            .Where(ug => ug.IsActive)
            .AsNoTracking()
            .ToListAsync();

        var matrix = new PermissionMatrixDto
        {
            Permissions = allPermissions.Select(p => new PermissionInfoDto
            {
                Name = p.Name,
                Description = p.Description,
                ScreenName = p.ScreenName,
                IsActive = p.IsActive
            }).ToList(),

            RolePermissions = roles.Select(r => new RolePermissionDto
            {
                RoleId = r.Id,
                RoleName = r.Name,
                Permissions = r.TblUserGroupPermissions
                    .Select(ugp => ugp.UserPermissionName)
                    .Distinct()
                    .ToList()
            }).ToList()
        };

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CACHE_DURATION,
            SlidingExpiration = TimeSpan.FromMinutes(10),
            Priority = CacheItemPriority.Normal
        };

        _cache.Set(CACHE_KEY_PERMISSION_MATRIX, matrix, cacheOptions);

        return matrix;
    }

    public void InvalidateRolePermissionsCache(int roleId)
    {
        var cacheKey = string.Format(CACHE_KEY_ROLE_PERMISSIONS, roleId);
        _cache.Remove(cacheKey);
    }

    public void InvalidatePermissionMatrixCache()
    {
        _cache.Remove(CACHE_KEY_PERMISSION_MATRIX);
        _cache.Remove(CACHE_KEY_ALL_PERMISSIONS);
    }
}
