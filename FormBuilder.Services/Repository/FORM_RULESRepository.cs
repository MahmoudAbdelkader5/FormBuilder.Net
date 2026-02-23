using FormBuilder.Infrastructure.Data;
using FormBuilder.core;
using FormBuilder.Domain.Interfaces;
using FormBuilder.Domian.Entitys.froms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Services.Repository
{
    public class FORM_RULESRepository : BaseRepository<FORM_RULES>, IFORM_RULESRepository
    {
        private readonly FormBuilderDbContext _context;

        public FORM_RULESRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<FORM_RULES>> GetRulesByFormIdAsync(int formBuilderId)
        {
            return await _context.FORM_RULES
                .Where(r => r.FormBuilderId == formBuilderId && !r.IsDeleted)
                .OrderBy(r => r.RuleName)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORM_RULES>> GetActiveRulesByFormIdAsync(int formBuilderId)
        {
            return await _context.FORM_RULES
                .Where(r => r.FormBuilderId == formBuilderId && r.IsActive && !r.IsDeleted)
                .OrderBy(r => r.RuleName)
                .ToListAsync();
        }

        public async Task<FORM_RULES> GetRuleByNameAsync(int formBuilderId, string ruleName)
        {
            return await _context.FORM_RULES
                .FirstOrDefaultAsync(r => r.FormBuilderId == formBuilderId && r.RuleName == ruleName && !r.IsDeleted);
        }

        public async Task<bool> IsRuleNameUniqueAsync(int formBuilderId, string ruleName, int? ignoreId = null)
        {
            if (string.IsNullOrWhiteSpace(ruleName))
                return false;

            return !await _context.FORM_RULES
                .AnyAsync(r => r.FormBuilderId == formBuilderId &&
                              r.RuleName == ruleName.Trim() &&
                              !r.IsDeleted &&
                              (!ignoreId.HasValue || r.Id != ignoreId.Value));
        }

        public async Task<IEnumerable<FORM_RULES>> GetRulesWithFormDetailsAsync(int formBuilderId)
        {
            return await _context.FORM_RULES
                .Where(r => r.FormBuilderId == formBuilderId && !r.IsDeleted)
                .Include(r => r.FORM_BUILDER)
                .OrderBy(r => r.RuleName)
                .ToListAsync();
        }

        public async Task<bool> UpdateRulesStatusAsync(int formBuilderId, bool isActive)
        {
            var rules = await _context.FORM_RULES
                .Where(r => r.FormBuilderId == formBuilderId && !r.IsDeleted)
                .ToListAsync();

            if (!rules.Any())
                return false;

            foreach (var rule in rules)
            {
                rule.IsActive = isActive;
            }

            _context.FORM_RULES.UpdateRange(rules);
            return await _context.SaveChangesAsync() > 0;
        }

        // Additional utility methods
        public async Task<int> GetRulesCountByFormAsync(int formBuilderId)
        {
            return await _context.FORM_RULES
                .CountAsync(r => r.FormBuilderId == formBuilderId && !r.IsDeleted);
        }

        public async Task<int> GetActiveRulesCountByFormAsync(int formBuilderId)
        {
            return await _context.FORM_RULES
                .CountAsync(r => r.FormBuilderId == formBuilderId && r.IsActive && !r.IsDeleted);
        }

        public async Task<bool> DeleteRulesByFormIdAsync(int formBuilderId)
        {
            var rules = await _context.FORM_RULES
                .Where(r => r.FormBuilderId == formBuilderId && !r.IsDeleted)
                .ToListAsync();

            if (!rules.Any())
                return false;

            // Use soft delete instead of hard delete
            foreach (var rule in rules)
            {
                rule.IsDeleted = true;
                rule.DeletedDate = DateTime.UtcNow;
                rule.UpdatedDate = DateTime.UtcNow;
            }

            _context.FORM_RULES.UpdateRange(rules);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}