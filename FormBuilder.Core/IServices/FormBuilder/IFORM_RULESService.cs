using FormBuilder.Core.DTOS.FormRules;
using FormBuilder.Domian.Entitys.froms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Services.Services
{
    public interface IFORM_RULESService
    {
        // Basic CRUD Operations
        Task<FORM_RULES> CreateRuleAsync(CreateFormRuleDto rule);
        Task<FORM_RULES> GetRuleByIdAsync(int id);
        Task<IEnumerable<FormRuleDto>> GetAllRulesAsync(); // ✅ تصحيح اسم الدالة
        Task<bool> UpdateRuleAsync(UpdateFormRuleDto rule ,int id);
        Task<bool> DeleteRuleAsync(int id);
        Task<bool> RestoreRuleAsync(int id);

        // Validation
        Task<bool> IsRuleNameUniqueAsync(int formBuilderId, string ruleName, int? ignoreId = null);

        // Utility
        Task<bool> RuleExistsAsync(int id);

        // Get active rules by form ID
        Task<IEnumerable<FormRuleDto>> GetActiveRulesByFormIdAsync(int formBuilderId);
    }
}