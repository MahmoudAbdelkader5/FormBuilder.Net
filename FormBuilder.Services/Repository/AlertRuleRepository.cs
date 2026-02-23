using FormBuilder.Infrastructure.Data;
using FormBuilder.core;
using FormBuilder.Domain.Interfaces.Repositories;
using FormBuilder.Domian.Entitys.FormBuilder;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Infrastructure.Repositories
{
    public class AlertRuleRepository : BaseRepository<ALERT_RULES>, IAlertRuleRepository
    {
        private readonly FormBuilderDbContext _context;

        public AlertRuleRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ALERT_RULES> GetByIdAsync(int id)
        {
            return await _context.ALERT_RULES
                .Include(ar => ar.DOCUMENT_TYPES)
                .Include(ar => ar.EMAIL_TEMPLATES)
                .FirstOrDefaultAsync(ar => ar.Id == id && !ar.IsDeleted);
        }

        public async Task<IEnumerable<ALERT_RULES>> GetByDocumentTypeIdAsync(int documentTypeId)
        {
            return await _context.ALERT_RULES
                .Include(ar => ar.DOCUMENT_TYPES)
                .Include(ar => ar.EMAIL_TEMPLATES)
                .Where(ar => ar.DocumentTypeId == documentTypeId && !ar.IsDeleted)
                .OrderBy(ar => ar.RuleName)
                .ToListAsync();
        }

        public async Task<IEnumerable<ALERT_RULES>> GetByTriggerTypeAsync(string triggerType)
        {
            return await _context.ALERT_RULES
                .Include(ar => ar.DOCUMENT_TYPES)
                .Include(ar => ar.EMAIL_TEMPLATES)
                .Where(ar => ar.TriggerType == triggerType && !ar.IsDeleted)
                .OrderBy(ar => ar.RuleName)
                .ToListAsync();
        }

        public async Task<IEnumerable<ALERT_RULES>> GetActiveByDocumentTypeAndTriggerAsync(int documentTypeId, string triggerType)
        {
            return await _context.ALERT_RULES
                .Include(ar => ar.DOCUMENT_TYPES)
                .Include(ar => ar.EMAIL_TEMPLATES)
                .Where(ar => ar.DocumentTypeId == documentTypeId 
                    && ar.TriggerType == triggerType 
                    && ar.IsActive 
                    && !ar.IsDeleted)
                .OrderBy(ar => ar.RuleName)
                .ToListAsync();
        }

        public async Task<IEnumerable<ALERT_RULES>> GetActiveAsync()
        {
            return await _context.ALERT_RULES
                .Include(ar => ar.DOCUMENT_TYPES)
                .Include(ar => ar.EMAIL_TEMPLATES)
                .Where(ar => ar.IsActive && !ar.IsDeleted)
                .OrderBy(ar => ar.DocumentTypeId)
                .ThenBy(ar => ar.TriggerType)
                .ThenBy(ar => ar.RuleName)
                .ToListAsync();
        }

        public async Task<bool> RuleNameExistsAsync(int documentTypeId, string ruleName, int? excludeId = null)
        {
            var query = _context.ALERT_RULES
                .Where(ar => ar.DocumentTypeId == documentTypeId 
                    && ar.RuleName == ruleName 
                    && !ar.IsDeleted);

            if (excludeId.HasValue)
            {
                query = query.Where(ar => ar.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}

