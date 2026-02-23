using FormBuilder.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace FormBuilder.Core.Models;

public class RoleService : IRoleService
{
    private readonly AkhmanageItContext _context;
    private readonly IMemoryCache _cache;
    private const string CACHE_KEY_ALL_ROLES = "AllRoles";
    private static readonly TimeSpan CACHE_DURATION = TimeSpan.FromMinutes(30);

    public RoleService(AkhmanageItContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<IEnumerable<TblUserGroup>> GetAllRolesAsync()
    {
        // محاولة جلب البيانات من Cache أولاً
        if (_cache.TryGetValue(CACHE_KEY_ALL_ROLES, out IEnumerable<TblUserGroup>? cachedRoles))
        {
            return cachedRoles ?? Enumerable.Empty<TblUserGroup>();
        }

        // إذا لم تكن في Cache، جلب من قاعدة البيانات
        var roles = await _context.TblUserGroups
                             .Where(r => r.IsActive == true)
                             .AsNoTracking() // للقراءة فقط - يحسن الأداء
                             .ToListAsync();

        // حفظ في Cache
        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CACHE_DURATION,
            SlidingExpiration = TimeSpan.FromMinutes(10),
            Priority = CacheItemPriority.Normal
        };

        _cache.Set(CACHE_KEY_ALL_ROLES, roles, cacheOptions);

        return roles;
    }
}
