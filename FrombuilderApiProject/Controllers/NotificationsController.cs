using FormBuilder.Infrastructure.Data;
using FormBuilder.Domian.Entitys.FormBuilder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FormBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly FormBuilderDbContext _db;

        public NotificationsController(FormBuilderDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// GET /api/Notifications?userId={userId}&limit=20
        /// Returns latest notifications for user (not deleted), ordered by CreatedDate desc.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string userId, [FromQuery] int limit = 20, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest(new { success = false, message = "userId is required" });

            if (limit <= 0) limit = 20;
            if (limit > 200) limit = 200;

            var baseQuery = _db.Set<NOTIFICATIONS>()
                .AsNoTracking()
                .Where(n => !n.IsDeleted && n.UserId == userId);

            // Load latest N notifications first (then filter by ALERT_RULES.NotificationType = Internal/Both)
            var raw = await baseQuery
                .OrderByDescending(n => n.CreatedDate)
                .Take(limit)
                .ToListAsync(cancellationToken);

            // For each notification, compute notificationType from ALERT_RULES (Internal/Both/Email)
            // We assume ReferenceId is SubmissionId for form-related notifications.
            var submissionIds = raw
                .Where(n => n.ReferenceId.HasValue && n.ReferenceId.Value > 0)
                .Select(n => n.ReferenceId!.Value)
                .Distinct()
                .ToList();

            var submissionDocTypes = await _db.Set<FORM_SUBMISSIONS>()
                .AsNoTracking()
                .Where(s => submissionIds.Contains(s.Id))
                .Select(s => new { s.Id, s.DocumentTypeId })
                .ToListAsync(cancellationToken);

            var docTypeBySubmissionId = submissionDocTypes.ToDictionary(x => x.Id, x => x.DocumentTypeId);

            var triggerTypes = raw
                .Select(n => n.ReferenceType)
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct()
                .ToList();

            var docTypeIds = submissionDocTypes.Select(x => x.DocumentTypeId).Distinct().ToList();

            var alertRules = await _db.Set<ALERT_RULES>()
                .AsNoTracking()
                .Where(ar =>
                    docTypeIds.Contains(ar.DocumentTypeId) &&
                    triggerTypes.Contains(ar.TriggerType) &&
                    ar.IsActive &&
                    !ar.IsDeleted)
                .Select(ar => new { ar.DocumentTypeId, ar.TriggerType, ar.NotificationType })
                .ToListAsync(cancellationToken);

            string? ResolveNotificationType(NOTIFICATIONS n)
            {
                // If no ReferenceId, check if it's an approval result trigger (fallback to Internal)
                if (!n.ReferenceId.HasValue)
                {
                    return IsApprovalResultTrigger(n.ReferenceType) ? "Internal" : null;
                }

                // Try to get DocumentTypeId from submission
                if (!docTypeBySubmissionId.TryGetValue(n.ReferenceId.Value, out var dtId))
                {
                    // Submission not found or ReferenceId mismatch - fallback to Internal for approval triggers
                    return IsApprovalResultTrigger(n.ReferenceType) ? "Internal" : null;
                }

                // Look for matching alert rule
                var rule = alertRules
                    .Where(r => r.DocumentTypeId == dtId && r.TriggerType == n.ReferenceType)
                    .OrderByDescending(r => r.NotificationType == "Both") // prefer Both if multiple
                    .FirstOrDefault();

                if (rule?.NotificationType != null) return rule.NotificationType;

                // Fallback: approval result notifications are internal by default
                return IsApprovalResultTrigger(n.ReferenceType) ? "Internal" : null;
            }

            var filtered = raw
                .Where(n => 
                {
                    var resolvedType = ResolveNotificationType(n);
                    if (resolvedType == "Internal" || resolvedType == "Both") return true;
                    // Direct fallback: always show approval result triggers as Internal
                    return IsApprovalResultTrigger(n.ReferenceType);
                })
                .Select(n => new
                {
                    entity = n,
                    notificationType = ResolveNotificationType(n) ?? 
                        (IsApprovalResultTrigger(n.ReferenceType) ? "Internal" : null) ?? "Internal"
                })
                .ToList();

            var totalCount = filtered.Count;
            var unreadCount = filtered.Count(x => !x.entity.IsRead);

            var items = filtered
                .Select(x => new
                {
                    id = x.entity.Id,
                    userId = x.entity.UserId,
                    title = x.entity.Title,
                    message = x.entity.Message,
                    type = x.entity.Type,
                    notificationType = x.notificationType, // from ALERT_RULES
                    referenceType = x.entity.ReferenceType,
                    referenceId = x.entity.ReferenceId,
                    isRead = x.entity.IsRead,
                    createdAt = x.entity.CreatedDate,
                    readAt = x.entity.ReadAt
                })
                .ToList();

            return Ok(new
            {
                success = true,
                data = new
                {
                    totalCount,
                    unreadCount,
                    notifications = items
                }
            });
        }

        /// <summary>
        /// GET /api/Notifications/unread-count?userId={userId}
        /// </summary>
        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount([FromQuery] string userId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest(new { success = false, message = "userId is required" });

            // Unread count is filtered by ALERT_RULES.NotificationType = Internal/Both
            var raw = await _db.Set<NOTIFICATIONS>()
                .AsNoTracking()
                .Where(n => !n.IsDeleted && n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedDate)
                .Take(500) // cap
                .ToListAsync(cancellationToken);

            var submissionIds = raw
                .Where(n => n.ReferenceId.HasValue && n.ReferenceId.Value > 0)
                .Select(n => n.ReferenceId!.Value)
                .Distinct()
                .ToList();

            var submissionDocTypes = await _db.Set<FORM_SUBMISSIONS>()
                .AsNoTracking()
                .Where(s => submissionIds.Contains(s.Id))
                .Select(s => new { s.Id, s.DocumentTypeId })
                .ToListAsync(cancellationToken);

            var docTypeBySubmissionId = submissionDocTypes.ToDictionary(x => x.Id, x => x.DocumentTypeId);
            var triggerTypes = raw.Select(n => n.ReferenceType).Where(t => !string.IsNullOrWhiteSpace(t)).Distinct().ToList();
            var docTypeIds = submissionDocTypes.Select(x => x.DocumentTypeId).Distinct().ToList();

        var alertRules = await _db.Set<ALERT_RULES>()
                .AsNoTracking()
                .Where(ar =>
                    docTypeIds.Contains(ar.DocumentTypeId) &&
                    triggerTypes.Contains(ar.TriggerType) &&
                    ar.IsActive &&
                    !ar.IsDeleted)
                .Select(ar => new { ar.DocumentTypeId, ar.TriggerType, ar.NotificationType })
                .ToListAsync(cancellationToken);

            bool IsInternalOrBoth(NOTIFICATIONS n)
            {
                // If no ReferenceId, check if it's an approval result trigger (fallback to Internal)
                if (!n.ReferenceId.HasValue)
                {
                    return IsApprovalResultTrigger(n.ReferenceType);
                }

                // Try to get DocumentTypeId from submission
                if (!docTypeBySubmissionId.TryGetValue(n.ReferenceId.Value, out var dtId))
                {
                    // Submission not found or ReferenceId mismatch - fallback to Internal for approval triggers
                    return IsApprovalResultTrigger(n.ReferenceType);
                }

                // Look for matching alert rule
                var rule = alertRules.FirstOrDefault(r => r.DocumentTypeId == dtId && r.TriggerType == n.ReferenceType);
                if (rule?.NotificationType == "Internal" || rule?.NotificationType == "Both") return true;
                
                // Fallback: approval result notifications are internal by default
                return IsApprovalResultTrigger(n.ReferenceType);
            }

            var unreadCount = raw.Count(IsInternalOrBoth);

            return Ok(new { success = true, data = new { unreadCount } });
        }

        /// <summary>
        /// PATCH /api/Notifications/{id}/read
        /// </summary>
        [HttpPatch("{id:int}/read")]
        public async Task<IActionResult> MarkRead([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var entity = await _db.Set<NOTIFICATIONS>()
                .FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted, cancellationToken);

            if (entity == null)
                return NotFound(new { success = false, message = "Notification not found" });

            if (!entity.IsRead)
            {
                entity.IsRead = true;
                entity.ReadAt = DateTime.UtcNow;
                entity.UpdatedDate = DateTime.UtcNow;
                await _db.SaveChangesAsync(cancellationToken);
            }

            return Ok(new { success = true });
        }

        /// <summary>
        /// PATCH /api/Notifications/read-all  body: { userId }
        /// </summary>
        [HttpPatch("read-all")]
        public async Task<IActionResult> MarkAllRead([FromBody] UserIdRequest body, CancellationToken cancellationToken = default)
        {
            if (body == null || string.IsNullOrWhiteSpace(body.UserId))
                return BadRequest(new { success = false, message = "userId is required" });

            var now = DateTime.UtcNow;
            var items = await _db.Set<NOTIFICATIONS>()
                .Where(n => !n.IsDeleted && n.UserId == body.UserId && !n.IsRead)
                .ToListAsync(cancellationToken);

            foreach (var n in items)
            {
                n.IsRead = true;
                n.ReadAt = now;
                n.UpdatedDate = now;
            }

            await _db.SaveChangesAsync(cancellationToken);
            return Ok(new { success = true, data = new { updated = items.Count } });
        }

        /// <summary>
        /// DELETE /api/Notifications/{id}
        /// Soft-delete.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var entity = await _db.Set<NOTIFICATIONS>()
                .FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted, cancellationToken);

            if (entity == null)
                return NotFound(new { success = false, message = "Notification not found" });

            entity.IsDeleted = true;
            entity.DeletedDate = DateTime.UtcNow;
            entity.IsActive = false;
            entity.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync(cancellationToken);
            return Ok(new { success = true });
        }

        /// <summary>
        /// DELETE /api/Notifications/clear-all  body: { userId }
        /// Soft-delete all notifications for user.
        /// </summary>
        [HttpDelete("clear-all")]
        public async Task<IActionResult> ClearAll([FromBody] UserIdRequest body, CancellationToken cancellationToken = default)
        {
            if (body == null || string.IsNullOrWhiteSpace(body.UserId))
                return BadRequest(new { success = false, message = "userId is required" });

            var now = DateTime.UtcNow;
            var items = await _db.Set<NOTIFICATIONS>()
                .Where(n => !n.IsDeleted && n.UserId == body.UserId)
                .ToListAsync(cancellationToken);

            foreach (var n in items)
            {
                n.IsDeleted = true;
                n.DeletedDate = now;
                n.IsActive = false;
                n.UpdatedDate = now;
            }

            await _db.SaveChangesAsync(cancellationToken);
            return Ok(new { success = true, data = new { deleted = items.Count } });
        }

        public sealed class UserIdRequest
        {
            public string UserId { get; set; } = string.Empty;
        }

        private static bool IsApprovalResultTrigger(string? triggerType)
        {
            return triggerType != null && (
                triggerType.Equals("ApprovalApproved", StringComparison.OrdinalIgnoreCase) ||
                triggerType.Equals("ApprovalRejected", StringComparison.OrdinalIgnoreCase) ||
                triggerType.Equals("ApprovalReturned", StringComparison.OrdinalIgnoreCase));
        }
    }
}


