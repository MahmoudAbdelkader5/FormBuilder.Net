using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace FormBuilder.API.Attributes
{
    /// <summary>
    /// Attribute للتحقق من وجود permission معين للمستخدم
    /// يمكن استخدامه من JWT Claims أو من PermissionService
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RequirePermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _permissionName;

        public RequirePermissionAttribute(string permissionName)
        {
            _permissionName = permissionName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // الحصول على UserId من Claims
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // الحصول على PermissionService من DI
            var permissionService = context.HttpContext.RequestServices
                .GetService(typeof(IUserPermissionService)) as IUserPermissionService;

            if (permissionService == null)
            {
                // إذا لم يكن Service متاح، نتحقق من Role فقط
                return;
            }

            // التحقق من Permission من PermissionService (مع Cache)
            var hasPermission = permissionService.HasPermissionAsync(userId, _permissionName).GetAwaiter().GetResult();

            if (!hasPermission)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
