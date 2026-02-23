using FormBuilder.Infrastructure.Data;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domain.Interfaces.Repositories;
using FormBuilder.core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Infrastructure.Repositories
{
    public class ApprovalDelegationRepository : BaseRepository<APPROVAL_DELEGATIONS>, IApprovalDelegationRepository
    {
        private readonly FormBuilderDbContext _context;

        public ApprovalDelegationRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<APPROVAL_DELEGATIONS>> GetActiveDelegationsByFromUserIdAsync(string fromUserId)
        {
            var now = DateTime.UtcNow;
            
            // Query with all required filters:
            // - IsActive = true
            // - IsDeleted = false
            // - StartDate <= now
            // - EndDate >= now
            // - FromUserId matches (handles both string and int if database column is int)
            var query = _context.APPROVAL_DELEGATIONS
                .Where(d => d.IsActive 
                    && !d.IsDeleted
                    && d.StartDate <= now 
                    && d.EndDate >= now
                    && d.FromUserId == fromUserId);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<APPROVAL_DELEGATIONS>> GetActiveDelegationsByToUserIdAsync(string toUserId)
        {
            // Handle type conversion: if toUserId is numeric string and DB column is int
            // Use Raw SQL to ensure proper type conversion and use GETDATE() for accurate date comparison
            if (int.TryParse(toUserId, out int toUserIdInt))
            {
                // Use Raw SQL with parameterized query to handle int type in database
                // Use GETDATE() directly in SQL to match SQL Server's date comparison
                var sql = @"
                    SELECT * FROM APPROVAL_DELEGATIONS 
                    WHERE ToUserId = {0}
                    AND IsActive = 1
                    AND IsDeleted = 0
                    AND StartDate <= GETDATE()
                    AND EndDate >= GETDATE()";
                
                return await _context.APPROVAL_DELEGATIONS
                    .FromSqlRaw(sql, toUserIdInt)
                    .ToListAsync();
            }
            else
            {
                // Use LINQ for string comparison
                var now = DateTime.UtcNow;
                var query = _context.APPROVAL_DELEGATIONS
                    .Where(d => d.IsActive 
                        && !d.IsDeleted
                        && d.StartDate <= now 
                        && d.EndDate >= now
                        && d.ToUserId == toUserId);

                return await query.ToListAsync();
            }
        }

        public async Task<APPROVAL_DELEGATIONS?> GetActiveDelegationAsync(string fromUserId, DateTime checkDate)
        {
            return await _context.APPROVAL_DELEGATIONS
                .Where(d => d.FromUserId == fromUserId 
                    && d.IsActive 
                    && !d.IsDeleted
                    && d.StartDate <= checkDate 
                    && d.EndDate >= checkDate)
                .OrderByDescending(d => d.StartDate)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> HasActiveDelegationAsync(string fromUserId, DateTime checkDate)
        {
            return await _context.APPROVAL_DELEGATIONS
                .AnyAsync(d => d.FromUserId == fromUserId 
                    && d.IsActive 
                    && !d.IsDeleted
                    && d.StartDate <= checkDate 
                    && d.EndDate >= checkDate);
        }

        public async Task<APPROVAL_DELEGATIONS?> GetActiveDelegationByScopeAsync(
            string fromUserId, 
            string scopeType, 
            int? scopeId, 
            DateTime checkDate)
        {
            // Handle type conversion: if fromUserId is numeric string and DB column is int
            // Use Raw SQL to ensure proper type conversion and use GETDATE() for accurate date comparison
            if (int.TryParse(fromUserId, out int fromUserIdInt))
            {
                // Use Raw SQL with parameterized query to handle int type in database
                // Use GETDATE() directly in SQL to match SQL Server's date comparison
                string sql;
                if (scopeId.HasValue)
                {
                    sql = @"
                        SELECT TOP 1 * FROM APPROVAL_DELEGATIONS 
                        WHERE FromUserId = {0}
                        AND ScopeType = {1}
                        AND ScopeId = {2}
                        AND IsActive = 1
                        AND IsDeleted = 0
                        AND StartDate <= GETDATE()
                        AND EndDate >= GETDATE()
                        ORDER BY StartDate DESC";
                    
                    return await _context.APPROVAL_DELEGATIONS
                        .FromSqlRaw(sql, fromUserIdInt, scopeType, scopeId.Value)
                        .FirstOrDefaultAsync();
                }
                else
                {
                    sql = @"
                        SELECT TOP 1 * FROM APPROVAL_DELEGATIONS 
                        WHERE FromUserId = {0}
                        AND ScopeType = {1}
                        AND ScopeId IS NULL
                        AND IsActive = 1
                        AND IsDeleted = 0
                        AND StartDate <= GETDATE()
                        AND EndDate >= GETDATE()
                        ORDER BY StartDate DESC";
                    
                    return await _context.APPROVAL_DELEGATIONS
                        .FromSqlRaw(sql, fromUserIdInt, scopeType)
                        .FirstOrDefaultAsync();
                }
            }
            else
            {
                // Use LINQ for string comparison
                return await _context.APPROVAL_DELEGATIONS
                    .Where(d => d.FromUserId == fromUserId
                        && d.ScopeType == scopeType
                        && (scopeId == null ? d.ScopeId == null : d.ScopeId == scopeId)
                        && d.IsActive
                        && !d.IsDeleted
                        && d.StartDate <= checkDate
                        && d.EndDate >= checkDate)
                    .OrderByDescending(d => d.StartDate)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<bool> HasActiveDelegationForScopeAsync(
            string fromUserId, 
            string scopeType, 
            int? scopeId, 
            DateTime checkDate)
        {
            return await _context.APPROVAL_DELEGATIONS
                .AnyAsync(d => d.FromUserId == fromUserId
                    && d.ScopeType == scopeType
                    && (scopeId == null ? d.ScopeId == null : d.ScopeId == scopeId)
                    && d.IsActive
                    && !d.IsDeleted
                    && d.StartDate <= checkDate
                    && d.EndDate >= checkDate);
        }

        public async Task<IEnumerable<APPROVAL_DELEGATIONS>> GetAllDelegationsByToUserIdAsync(string toUserId)
        {
            // Get all delegations (including inactive and deleted) for debugging
            // Handle type conversion: if toUserId is numeric string and DB column is int
            if (int.TryParse(toUserId, out int toUserIdInt))
            {
                // Use Raw SQL with parameterized query to handle int type in database
                var sql = @"SELECT * FROM APPROVAL_DELEGATIONS WHERE ToUserId = {0}";
                
                return await _context.APPROVAL_DELEGATIONS
                    .FromSqlRaw(sql, toUserIdInt)
                    .ToListAsync();
            }
            else
            {
                // Use LINQ for string comparison
                return await _context.APPROVAL_DELEGATIONS
                    .Where(d => d.ToUserId == toUserId)
                    .ToListAsync();
            }
        }
    }
}

