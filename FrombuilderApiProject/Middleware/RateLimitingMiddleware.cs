using FormBuilder.Core.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Net;

namespace FormBuilder.API.Middleware
{
    /// <summary>
    /// Middleware للتحكم في معدل الطلبات (Rate Limiting)
    /// يدعم:
    /// - Global Rate Limit
    /// - Endpoint-specific Rate Limits
    /// - IP Whitelist/Blacklist
    /// - Different limits for authenticated vs anonymous users
    /// </summary>
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimitingMiddleware> _logger;
        private readonly RateLimitingOptions _options;
        private readonly ConcurrentDictionary<string, RateLimitInfo> _requestCounts = new();
        private readonly Timer _cleanupTimer;

        public RateLimitingMiddleware(
            RequestDelegate next,
            ILogger<RateLimitingMiddleware> logger,
            IOptions<RateLimitingOptions> options)
        {
            _next = next;
            _logger = logger;
            _options = options.Value;

            // تنظيف دوري كل 5 دقائق
            _cleanupTimer = new Timer(CleanupOldEntries, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // التحقق من تفعيل Rate Limiting
            if (!_options.Enabled)
            {
                await _next(context);
                return;
            }

            // تخطي OPTIONS requests (CORS preflight requests)
            if (context.Request.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            var path = context.Request.Path.Value ?? "";
            var pathLower = path.ToLower();

            // تخطي Rate Limiting للمسارات المحددة
            if (_options.BypassPaths.Any(bp => pathLower.Contains(bp.ToLower())))
            {
                await _next(context);
                return;
            }

            var clientId = GetClientIdentifier(context);
            var clientIp = GetClientIpAddress(context);

            // التحقق من Blacklist
            if (_options.Blacklist.Contains(clientIp, StringComparer.OrdinalIgnoreCase))
            {
                _logger.LogWarning("Blocked request from blacklisted IP: {IP}", clientIp);
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync("Access denied");
                return;
            }

            // تخطي Rate Limiting للـ Whitelist
            if (_options.Whitelist.Contains(clientIp, StringComparer.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            // تحديد Rate Limit للـ Endpoint المحدد أو Global
            var endpointLimit = GetEndpointLimit(path);
            var maxRequests = endpointLimit.MaxRequests;
            var timeWindow = TimeSpan.FromMinutes(endpointLimit.TimeWindowMinutes);

            var now = DateTime.UtcNow;
            var cacheKey = $"{clientId}:{pathLower}";

            // التحقق من Rate Limit
            var rateLimitInfo = _requestCounts.GetOrAdd(cacheKey, _ => new RateLimitInfo 
            { 
                FirstRequest = now,
                MaxRequests = maxRequests,
                TimeWindow = timeWindow
            });

            // تحديث MaxRequests و TimeWindow إذا تغيرت
            rateLimitInfo.MaxRequests = maxRequests;
            rateLimitInfo.TimeWindow = timeWindow;

            // إعادة تعيين النافذة الزمنية إذا انتهت
            if (now - rateLimitInfo.FirstRequest > timeWindow)
            {
                rateLimitInfo.FirstRequest = now;
                rateLimitInfo.RequestCount = 0;
            }

            rateLimitInfo.RequestCount++;

            if (rateLimitInfo.RequestCount > maxRequests)
            {
                _logger.LogWarning(
                    "Rate limit exceeded - Client: {ClientId}, IP: {IP}, Path: {Path}, Requests: {Count}/{Max}, Window: {Window}min",
                    clientId, clientIp, path, rateLimitInfo.RequestCount, maxRequests, endpointLimit.TimeWindowMinutes);

                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                context.Response.ContentType = "application/json";

                var retryAfter = (int)(timeWindow.TotalSeconds - (now - rateLimitInfo.FirstRequest).TotalSeconds);
                if (retryAfter < 0) retryAfter = (int)timeWindow.TotalSeconds;

                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                {
                    error = "Too many requests",
                    message = $"Rate limit exceeded. Maximum {maxRequests} requests per {endpointLimit.TimeWindowMinutes} minute(s).",
                    retryAfter = retryAfter,
                    limit = maxRequests,
                    windowMinutes = endpointLimit.TimeWindowMinutes
                }));

                return;
            }

            // إضافة Headers للـ Rate Limit
            var remaining = Math.Max(0, maxRequests - rateLimitInfo.RequestCount);
            var resetTime = rateLimitInfo.FirstRequest.Add(timeWindow);

            context.Response.Headers.Append("X-RateLimit-Limit", maxRequests.ToString());
            context.Response.Headers.Append("X-RateLimit-Remaining", remaining.ToString());
            context.Response.Headers.Append("X-RateLimit-Reset", resetTime.ToUniversalTime().ToString("R"));
            context.Response.Headers.Append("X-RateLimit-Used", rateLimitInfo.RequestCount.ToString());

            await _next(context);
        }

        private string GetClientIdentifier(HttpContext context)
        {
            // استخدام User ID إذا كان authenticated (أفضل للتتبع)
            var userId = context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                return $"user_{userId}";
            }

            // استخدام IP Address للـ anonymous users
            return GetClientIpAddress(context);
        }

        private string GetClientIpAddress(HttpContext context)
        {
            // التحقق من X-Forwarded-For header (للـ proxies)
            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                var ips = forwardedFor.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (ips.Length > 0)
                {
                    return ips[0];
                }
            }

            // استخدام RemoteIpAddress
            return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }

        private RateLimitConfig GetEndpointLimit(string path)
        {
            // البحث عن endpoint limit محدد
            var endpointLimit = _options.EndpointLimits
                .FirstOrDefault(kvp => path.StartsWith(kvp.Key, StringComparison.OrdinalIgnoreCase));

            if (endpointLimit.Value != null)
            {
                return endpointLimit.Value;
            }

            // استخدام Global Limit
            return _options.GlobalLimit;
        }

        private void CleanupOldEntries(object? state)
        {
            var now = DateTime.UtcNow;
            var maxAge = TimeSpan.FromMinutes(10); // تنظيف entries أقدم من 10 دقائق

            var keysToRemove = _requestCounts
                .Where(kvp => now - kvp.Value.FirstRequest > maxAge)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in keysToRemove)
            {
                _requestCounts.TryRemove(key, out _);
            }

            if (keysToRemove.Count > 0)
            {
                _logger.LogDebug("Cleaned up {Count} old rate limit entries", keysToRemove.Count);
            }
        }

        private class RateLimitInfo
        {
            public DateTime FirstRequest { get; set; }
            public int RequestCount { get; set; }
            public int MaxRequests { get; set; }
            public TimeSpan TimeWindow { get; set; }
        }
    }

    public static class RateLimitingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RateLimitingMiddleware>();
        }
    }
}
