using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IAlertRuleRepository : IBaseRepository<ALERT_RULES>
    {
        Task<ALERT_RULES> GetByIdAsync(int id);
        Task<IEnumerable<ALERT_RULES>> GetByDocumentTypeIdAsync(int documentTypeId);
        Task<IEnumerable<ALERT_RULES>> GetByTriggerTypeAsync(string triggerType);
        Task<IEnumerable<ALERT_RULES>> GetActiveByDocumentTypeAndTriggerAsync(int documentTypeId, string triggerType);
        Task<IEnumerable<ALERT_RULES>> GetActiveAsync();
        Task<bool> RuleNameExistsAsync(int documentTypeId, string ruleName, int? excludeId = null);
    }
}

