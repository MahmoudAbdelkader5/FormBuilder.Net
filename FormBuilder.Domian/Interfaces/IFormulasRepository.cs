using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.froms;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IFormulasRepository : IBaseRepository<FORMULAS>
    {

        // Custom queries for FORMULAS entity
        Task<FORMULAS> GetByCodeAsync(string code, int formBuilderId, params Expression<Func<FORMULAS, object>>[] includes);
        Task<IEnumerable<FORMULAS>> GetByFormBuilderAsync(int formBuilderId, params Expression<Func<FORMULAS, object>>[] includes);
        Task<IEnumerable<FORMULAS>> GetActiveByFormBuilderAsync(int formBuilderId, params Expression<Func<FORMULAS, object>>[] includes);
        Task<IEnumerable<FORMULAS>> GetByResultFieldAsync(int? resultFieldId, params Expression<Func<FORMULAS, object>>[] includes);
        Task<IEnumerable<FORMULAS>> GetFormulasWithoutResultFieldAsync(int formBuilderId, params Expression<Func<FORMULAS, object>>[] includes);
        Task<bool> CodeExistsAsync(string code, int formBuilderId, int? excludeId = null);
        Task<bool> HasActiveFormulasAsync(int formBuilderId);
        Task<IEnumerable<FORMULAS>> GetFormulasWithDetailsAsync(int formBuilderId = 0);
        Task<FORMULAS> GetByIdWithDetailsAsync(int id);
        Task<bool> IsActiveAsync(int id);

        // Validation methods
        Task<bool> IsExpressionValidForFormAsync(string expressionText, int formBuilderId);
        Task<IEnumerable<string>> GetReferencedFieldCodesInExpressionAsync(string expressionText);

        // Field relationship helper methods
        Task<int?> GetFormIdFromFieldIdAsync(int fieldId);
        Task<int?> GetFormIdFromFieldCodeAsync(string fieldCode, int formBuilderId);
        Task<bool> FieldBelongsToFormBuilderAsync(int fieldId, int formBuilderId);
        Task<bool> FieldCodeBelongsToFormBuilderAsync(string fieldCode, int formBuilderId);
        Task<IEnumerable<FORM_FIELDS>> GetFieldsByFormBuilderAsync(int formBuilderId);

        // FORMULA_VARIABLES helper methods
        Task<IEnumerable<FORMULA_VARIABLES>> GetFormulaVariablesWithDetailsAsync(int formulaId);
        Task<int> CountFormulaVariablesAsync(int formulaId);
        Task<bool> HasFormulaVariablesAsync(int formulaId);

        // Formula calculation helper
        Task<Dictionary<string, FORM_FIELDS>> GetFieldMappingsFromExpressionAsync(string expressionText, int formBuilderId);

        // Search method
        Task<IEnumerable<FORMULAS>> SearchFormulasAsync(string searchTerm, int formBuilderId);
    }
}