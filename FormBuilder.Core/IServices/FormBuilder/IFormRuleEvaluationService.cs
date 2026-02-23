using System.Collections.Generic;
using FormBuilder.Core.DTOS.FormRules;

namespace FormBuilder.Core.IServices.FormBuilder
{
    /// <summary>
    /// Service interface for evaluating form rules
    /// </summary>
    public interface IFormRuleEvaluationService
    {
        /// <summary>
        /// Evaluates a condition against form field values
        /// </summary>
        bool EvaluateCondition(ConditionDataDto condition, Dictionary<string, object> fieldValues);

        /// <summary>
        /// Evaluates a formula expression using field values
        /// </summary>
        object EvaluateFormula(string expression, Dictionary<string, object> fieldValues);

        /// <summary>
        /// Evaluates an expression (alias for EvaluateFormula for consistency)
        /// </summary>
        object EvaluateExpression(string expression, Dictionary<string, object> fieldValues);

        /// <summary>
        /// Validates actions and returns list of validation errors
        /// </summary>
        List<string> ValidateActions(
            List<ActionDataDto>? actions,
            Dictionary<string, object> fieldValues,
            string ruleName);

        /// <summary>
        /// Parses RuleJson string into FormRuleDataDto (for backward compatibility)
        /// </summary>
        FormRuleDataDto? ParseRuleJson(string ruleJson);

        /// <summary>
        /// Builds FormRuleDataDto from separate fields (new approach)
        /// </summary>
        FormRuleDataDto? BuildRuleDataFromFields(
            string? conditionField,
            string? conditionOperator,
            string? conditionValue,
            string? conditionValueType,
            string? actionsJson,
            string? elseActionsJson);

        /// <summary>
        /// Evaluates a Stored Procedure based rule by ID (from Whitelist)
        /// </summary>
        /// <param name="storedProcedureId">ID of the stored procedure from FORM_STORED_PROCEDURES</param>
        /// <param name="parameterMapping">JSON mapping of form fields to procedure parameters (optional, uses default from whitelist if not provided)</param>
        /// <param name="resultMapping">JSON mapping for interpreting the result (optional, uses default from whitelist if not provided)</param>
        /// <param name="fieldValues">Current form field values</param>
        /// <returns>Boolean result indicating if the condition is true</returns>
        Task<bool> EvaluateStoredProcedureByIdAsync(
            int storedProcedureId,
            string? parameterMapping,
            string? resultMapping,
            Dictionary<string, object> fieldValues);

        /// <summary>
        /// Evaluates a Stored Procedure based rule
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure</param>
        /// <param name="databaseName">Database name: "FormBuilder" or "AKHManageIT"</param>
        /// <param name="parameterMapping">JSON mapping of form fields to procedure parameters</param>
        /// <param name="resultMapping">JSON mapping for interpreting the result</param>
        /// <param name="fieldValues">Current form field values</param>
        /// <returns>Boolean result indicating if the condition is true</returns>
        Task<bool> EvaluateStoredProcedureAsync(
            string procedureName,
            string databaseName,
            string? parameterMapping,
            string? resultMapping,
            Dictionary<string, object> fieldValues);

        /// <summary>
        /// Evaluates blocking rules for a form at a specific evaluation phase
        /// </summary>
        /// <param name="formBuilderId">ID of the form</param>
        /// <param name="evaluationPhase">Evaluation phase: "PreOpen" or "PreSubmit"</param>
        /// <param name="submissionId">Optional submission ID (for Pre-Submit evaluation)</param>
        /// <param name="fieldValues">Optional field values (for Pre-Submit evaluation)</param>
        /// <param name="includeDebugInfo">Whether to include debug information in the result (for development/testing)</param>
        /// <returns>Blocking rule result indicating if form is blocked</returns>
        Task<BlockingRuleResultDto> EvaluateBlockingRulesAsync(
            int formBuilderId,
            string evaluationPhase,
            int? submissionId = null,
            Dictionary<string, object>? fieldValues = null,
            bool includeDebugInfo = false);
    }
}

