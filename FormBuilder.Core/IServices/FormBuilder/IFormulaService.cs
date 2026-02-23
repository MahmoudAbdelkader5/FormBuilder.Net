using FormBuilder.Application.DTOS;
using FormBuilder.Application.DTOs.Formula;
using FormBuilder.Core.DTOS.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IFormulaService
    {
        #region Basic CRUD Operations
        Task<ServiceResult<IEnumerable<FormulaDto>>> GetAllAsync(int formBuilderId);
        Task<ServiceResult<FormulaDto>> GetByIdAsync(int id);
        Task<ServiceResult<FormulaDto>> GetByCodeAsync(string code, int formBuilderId);
        Task<ServiceResult<FormulaDto>> CreateAsync(CreateFormulaDto createDto);
        Task<ServiceResult<FormulaDto>> UpdateAsync(int id, UpdateFormulaDto updateDto);
        Task<ServiceResult<bool>> DeleteAsync(int id);
        Task<ServiceResult<int>> DeleteByFormBuilderIdAsync(int formBuilderId);
        #endregion

        #region Status Management
        Task<ServiceResult<bool>> ToggleActiveAsync(int id, bool isActive);
        Task<ServiceResult<IEnumerable<FormulaDto>>> GetActiveAsync(int formBuilderId);
        Task<ServiceResult<IEnumerable<FormulaDto>>> GetInactiveAsync(int formBuilderId);
        #endregion

        #region Query Operations
        Task<ServiceResult<IEnumerable<FormulaDto>>> GetByFormBuilderAsync(int formBuilderId);
        Task<ServiceResult<IEnumerable<FormulaDto>>> GetByResultFieldAsync(int? resultFieldId);
        Task<ServiceResult<IEnumerable<FormulaDto>>> GetFormulasWithoutResultFieldAsync(int formBuilderId);
        Task<ServiceResult<IEnumerable<FormulaDto>>> GetFormulasWithDetailsAsync(int formBuilderId = 0);
        Task<ServiceResult<FormulaDto>> GetByIdWithDetailsAsync(int id);
        Task<ServiceResult<IEnumerable<FormulaDto>>> SearchFormulasAsync(string searchTerm, int formBuilderId);
        #endregion

        #region Validation Operations
        Task<ServiceResult<ValidateExpressionResultDto>> ValidateExpressionAsync(ValidateExpressionDto validationDto);
        Task<ServiceResult<bool>> CodeExistsAsync(string code, int formBuilderId, int? excludeId = null);
        Task<ServiceResult<bool>> IsActiveAsync(int id);
        Task<ServiceResult<bool>> HasActiveFormulasAsync(int formBuilderId);
        Task<ServiceResult<bool>> IsExpressionValidForFormAsync(string expressionText, int formBuilderId);
        #endregion

        #region Utility Operations
        Task<ServiceResult<IEnumerable<string>>> GetReferencedFieldCodesAsync(int formulaId);
        Task<ServiceResult<int>> CountFormulasAsync(int formBuilderId);
        Task<ServiceResult<FormulaDto>> UpdateFormulaExpressionAsync(int id, string expressionText);
        Task<ServiceResult<IEnumerable<string>>> GetFieldCodesForFormAsync(int formBuilderId);
        Task<ServiceResult<ValidateExpressionResultDto>> ValidateExpressionWithDetailsAsync(ValidateExpressionDto validationDto);
        #endregion

        #region FORMULA_VARIABLES Operations
        Task<ServiceResult<IEnumerable<FormulaVariableDto>>> GetFormulaVariablesAsync(int formulaId);
        Task<ServiceResult<int>> CountVariablesAsync(int formulaId);
        Task<ServiceResult<bool>> HasVariablesAsync(int formulaId);
        #endregion

        #region Additional Formula Operations
        Task<ServiceResult<IEnumerable<FormulaDto>>> GetFormulasByFieldAsync(int fieldId);
        Task<ServiceResult<object>> GetFormulaStatisticsAsync(int formBuilderId);
        Task<ServiceResult<int>> BatchUpdateFormulaStatusAsync(List<int> formulaIds, bool isActive);
        Task<ServiceResult<FormulaDto>> DuplicateFormulaAsync(int sourceFormulaId, DuplicateFormulaDto duplicateDto);
        #endregion

        #region Formula Calculation Operations
        Task<ServiceResult<object>> CalculateFormulaAsync(int formulaId, Dictionary<string, object> fieldValues = null);
        Task<ServiceResult<object>> CalculateExpressionAsync(string expressionText, Dictionary<string, object> fieldValues);
        Task<ServiceResult<Dictionary<int, object>>> BatchCalculateFormulasAsync(int formBuilderId, Dictionary<string, object> fieldValues);
        Task<ServiceResult<object>> PreviewCalculationAsync(PreviewCalculationDto previewDto);
        Task<ServiceResult<object>> TestFormulaWithSampleDataAsync(int formulaId);
        Task<ServiceResult<object>> CalculateWithAllOperationsAsync(string expressionText, Dictionary<string, object> fieldValues);
        Task<ServiceResult<object>> SafeCalculateExpressionAsync(string expressionText, Dictionary<string, object> fieldValues);
        #endregion
    }
}