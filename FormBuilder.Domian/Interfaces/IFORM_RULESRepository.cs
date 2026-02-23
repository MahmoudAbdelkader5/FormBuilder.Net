using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.froms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces
{
    public interface IFORM_RULESRepository : IBaseRepository<FORM_RULES>
    {
        // Get all rules for a specific form

        // Get active rules for a specific form
        Task<IEnumerable<FORM_RULES>> GetActiveRulesByFormIdAsync(int formBuilderId);

        // Get rule by name and form
        Task<FORM_RULES> GetRuleByNameAsync(int formBuilderId, string ruleName);

        // Check if rule name is unique within a form
        Task<bool> IsRuleNameUniqueAsync(int formBuilderId, string ruleName, int? ignoreId = null);

        // Get rules with form details
        Task<IEnumerable<FORM_RULES>> GetRulesWithFormDetailsAsync(int formBuilderId);

        // Bulk update rules status
        Task<bool> UpdateRulesStatusAsync(int formBuilderId, bool isActive);
    }
}