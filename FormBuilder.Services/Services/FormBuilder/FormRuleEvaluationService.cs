using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using FormBuilder.Core.DTOS.FormRules;
using FormBuilder.Core.IServices.FormBuilder;
using FormBuilder.Domain.Interfaces;
using FormBuilder.Domian.Entitys.froms;
using FormBuilder.Domian.Entitys.FormBuilder;
using formBuilder.Domian.Interfaces;

namespace FormBuilder.Services.Services.FormBuilder
{
    /// <summary>
    /// Service for evaluating form rules (conditions and actions)
    /// </summary>
    public class FormRuleEvaluationService : IFormRuleEvaluationService
    {
        private readonly ILogger<FormRuleEvaluationService>? _logger;
        private readonly StoredProcedureService? _storedProcedureService;
        private readonly IFORM_RULESRepository? _formRulesRepository;
        private readonly IunitOfwork? _unitOfWork;

        public FormRuleEvaluationService(
            ILogger<FormRuleEvaluationService>? logger = null,
            StoredProcedureService? storedProcedureService = null,
            IFORM_RULESRepository? formRulesRepository = null,
            IunitOfwork? unitOfWork = null)
        {
            _logger = logger;
            _storedProcedureService = storedProcedureService;
            _formRulesRepository = formRulesRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Evaluates a condition against form field values
        /// </summary>
        public bool EvaluateCondition(ConditionDataDto condition, Dictionary<string, object> fieldValues)
        {
            if (condition == null)
            {
                _logger?.LogWarning("Condition is null");
                return false;
            }

            if (string.IsNullOrEmpty(condition.Field))
            {
                _logger?.LogWarning("Condition field is empty");
                return false;
            }

            if (fieldValues == null)
            {
                _logger?.LogWarning("FieldValues dictionary is null");
                return false;
            }

            // Normalize field name to uppercase for case-insensitive matching
            var normalizedFieldName = condition.Field?.ToUpperInvariant() ?? string.Empty;
            
            // Try to find field with case-insensitive matching
            var matchingKey = fieldValues.Keys.FirstOrDefault(k => 
                k.Equals(normalizedFieldName, StringComparison.OrdinalIgnoreCase));
            
            if (matchingKey == null)
            {
                _logger?.LogWarning("Field '{Field}' (normalized: '{NormalizedField}') not found in form values. Available fields: {AvailableFields}",
                    condition.Field, normalizedFieldName, string.Join(", ", fieldValues.Keys));
                return false;
            }

            var fieldValue = fieldValues[matchingKey];
            object? compareValue;

            // Determine comparison value
            if (condition.ValueType == "field")
            {
                var compareField = condition.Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(compareField))
                {
                    compareField = compareField.ToUpperInvariant();
                }
                
                // Try to find field with case-insensitive matching
                var matchingCompareKey = fieldValues.Keys.FirstOrDefault(k => 
                    k.Equals(compareField, StringComparison.OrdinalIgnoreCase));
                
                if (string.IsNullOrEmpty(compareField) || matchingCompareKey == null)
                {
                    _logger?.LogWarning("Comparison field '{Field}' (normalized: '{NormalizedField}') not found in form values. Available fields: {AvailableFields}", 
                        condition.Value, compareField, string.Join(", ", fieldValues.Keys));
                    return false;
                }
                compareValue = fieldValues[matchingCompareKey];
            }
            else
            {
                compareValue = condition.Value;
            }

            // Normalize operator (convert text names to symbols)
            var normalizedOperator = NormalizeOperator(condition.Operator);
            
            // Validate operator
            var validOperators = new[] { "==", "!=", ">", "<", ">=", "<=", "contains", "isEmpty", "isNotEmpty" };
            if (string.IsNullOrEmpty(normalizedOperator) || !validOperators.Contains(normalizedOperator))
            {
                _logger?.LogWarning("Invalid operator '{Operator}' (normalized: '{NormalizedOperator}') for field '{Field}'", 
                    condition.Operator, normalizedOperator ?? "null", condition.Field);
                return false;
            }

            // Evaluate based on operator
            try
            {
                _logger?.LogDebug("Evaluating condition: Field={Field}, Operator={Operator} (normalized: {NormalizedOperator}), FieldValue={FieldValue} ({FieldValueType}), CompareValue={CompareValue} ({CompareValueType})",
                    condition.Field, condition.Operator, normalizedOperator, fieldValue, fieldValue?.GetType().Name ?? "null", 
                    compareValue, compareValue?.GetType().Name ?? "null");

                bool result;
                switch (normalizedOperator)
                {
                    case "==":
                        // Try numeric comparison first, then fallback to string comparison
                        if (IsNumeric(fieldValue) && IsNumeric(compareValue))
                        {
                            result = CompareNumeric(fieldValue, compareValue, (a, b) => Math.Abs(a - b) < 0.0001); // Use epsilon for floating point comparison
                            _logger?.LogDebug("Numeric equality comparison: FieldValue={FieldValue} == CompareValue={CompareValue} = {Result}",
                                fieldValue, compareValue, result);
                        }
                        else
                        {
                            result = CompareValues(fieldValue, compareValue, (a, b) => a?.ToString()?.Trim() == b?.ToString()?.Trim());
                            _logger?.LogDebug("String equality comparison: FieldValue={FieldValue} == CompareValue={CompareValue} = {Result}",
                                fieldValue, compareValue, result);
                        }
                        break;
                    case "!=":
                        // Try numeric comparison first, then fallback to string comparison
                        if (IsNumeric(fieldValue) && IsNumeric(compareValue))
                        {
                            result = CompareNumeric(fieldValue, compareValue, (a, b) => Math.Abs(a - b) >= 0.0001);
                            _logger?.LogDebug("Numeric inequality comparison: FieldValue={FieldValue} != CompareValue={CompareValue} = {Result}",
                                fieldValue, compareValue, result);
                        }
                        else
                        {
                            result = CompareValues(fieldValue, compareValue, (a, b) => a?.ToString()?.Trim() != b?.ToString()?.Trim());
                            _logger?.LogDebug("String inequality comparison: FieldValue={FieldValue} != CompareValue={CompareValue} = {Result}",
                                fieldValue, compareValue, result);
                        }
                        break;
                    case ">":
                        result = CompareNumeric(fieldValue, compareValue, (a, b) => a > b);
                        _logger?.LogDebug("Numeric comparison '>': FieldValue={FieldValue} > CompareValue={CompareValue} = {Result}",
                            fieldValue, compareValue, result);
                        break;
                    case "<":
                        result = CompareNumeric(fieldValue, compareValue, (a, b) => a < b);
                        _logger?.LogDebug("Numeric comparison '<': FieldValue={FieldValue} < CompareValue={CompareValue} = {Result}",
                            fieldValue, compareValue, result);
                        break;
                    case ">=":
                        result = CompareNumeric(fieldValue, compareValue, (a, b) => a >= b);
                        _logger?.LogDebug("Numeric comparison '>=': FieldValue={FieldValue} >= CompareValue={CompareValue} = {Result}",
                            fieldValue, compareValue, result);
                        break;
                    case "<=":
                        result = CompareNumeric(fieldValue, compareValue, (a, b) => a <= b);
                        _logger?.LogDebug("Numeric comparison '<=': FieldValue={FieldValue} <= CompareValue={CompareValue} = {Result}",
                            fieldValue, compareValue, result);
                        break;
                    case "contains":
                        result = fieldValue?.ToString()?.Contains(compareValue?.ToString() ?? "", StringComparison.OrdinalIgnoreCase) ?? false;
                        break;
                    case "isEmpty":
                        result = IsEmpty(fieldValue);
                        break;
                    case "isNotEmpty":
                        result = !IsEmpty(fieldValue);
                        break;
                    default:
                        _logger?.LogWarning("Unsupported operator '{Operator}' for field '{Field}'", condition.Operator, condition.Field);
                        return false;
                }

                _logger?.LogDebug("Condition evaluated: Field={Field}, Operator={Operator}, FieldValue={FieldValue}, CompareValue={CompareValue}, Result={Result}",
                    condition.Field, condition.Operator, fieldValue, compareValue, result);

                return result;
            }
            catch (FormatException ex)
            {
                _logger?.LogError(ex, "Format error evaluating condition: Field={Field}, Operator={Operator}, FieldValue={FieldValue}, CompareValue={CompareValue}",
                    condition.Field, condition.Operator, fieldValue, compareValue);
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error evaluating condition: Field={Field}, Operator={Operator}",
                    condition.Field, condition.Operator);
                return false;
            }
        }

        /// <summary>
        /// Evaluates a formula expression using field values
        /// </summary>
        public object EvaluateFormula(string expression, Dictionary<string, object> fieldValues)
        {
            return EvaluateExpression(expression, fieldValues);
        }

        /// <summary>
        /// Evaluates an expression using field values (for Compute actions)
        /// </summary>
        public object EvaluateExpression(string expression, Dictionary<string, object> fieldValues)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                _logger?.LogWarning("Expression is empty");
                return 0;
            }

            if (fieldValues == null)
            {
                _logger?.LogWarning("FieldValues dictionary is null");
                return 0;
            }

            try
            {
                // Replace field codes with their numeric values
                var formula = expression;
                var replacedFields = new List<string>();

                foreach (var kvp in fieldValues)
                {
                    try
                    {
                        var numValue = Convert.ToDouble(kvp.Value ?? 0);
                        // Replace field code in formula (with word boundaries to avoid partial matches)
                        var pattern = $@"\b{Regex.Escape(kvp.Key)}\b";
                        if (Regex.IsMatch(formula, pattern, RegexOptions.IgnoreCase))
                        {
                            formula = Regex.Replace(
                                formula,
                                pattern,
                                numValue.ToString(),
                                RegexOptions.IgnoreCase);
                            replacedFields.Add(kvp.Key);
                        }
                    }
                    catch (FormatException ex)
                    {
                        // If value cannot be converted to number, skip this field
                        _logger?.LogDebug(ex, "Field '{Field}' value '{Value}' cannot be converted to number, skipping",
                            kvp.Key, kvp.Value);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "Unexpected error processing field '{Field}' in expression", kvp.Key);
                    }
                }

                // Check if any field codes remain unreplaced (potential error)
                var remainingFieldPattern = @"\b[A-Za-z_][A-Za-z0-9_]*\b";
                var matches = Regex.Matches(formula, remainingFieldPattern);
                var unreplacedFields = matches
                    .Cast<Match>()
                    .Where(m => !double.TryParse(m.Value, out _) && 
                                !new[] { "true", "false", "null" }.Contains(m.Value.ToLower()))
                    .Select(m => m.Value)
                    .Distinct()
                    .ToList();

                if (unreplacedFields.Any())
                {
                    _logger?.LogWarning("Expression contains unreplaced field codes: {Fields}. Expression: {Expression}",
                        string.Join(", ", unreplacedFields), expression);
                }

                // Evaluate formula using DataTable.Compute (safer than eval)
                var dataTable = new DataTable();
                var result = dataTable.Compute(formula, null);
                
                if (result == null || result == DBNull.Value)
                {
                    _logger?.LogWarning("Expression evaluation returned null. Expression: {Expression}", expression);
                    return 0;
                }

                _logger?.LogDebug("Expression evaluated successfully: Expression={Expression}, Result={Result}, ReplacedFields={ReplacedFields}",
                    expression, result, string.Join(", ", replacedFields));
                
                return result;
            }
            catch (SyntaxErrorException ex)
            {
                _logger?.LogError(ex, "Syntax error in expression: {Expression}", expression);
                throw new InvalidOperationException($"Invalid expression syntax: {expression}", ex);
            }
            catch (EvaluateException ex)
            {
                _logger?.LogError(ex, "Evaluation error in expression: {Expression}", expression);
                throw new InvalidOperationException($"Expression evaluation failed: {expression}", ex);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error evaluating expression: {Expression}", expression);
                throw new InvalidOperationException($"Error evaluating expression: {expression}", ex);
            }
        }

        /// <summary>
        /// Validates actions and returns list of validation errors
        /// </summary>
        public List<string> ValidateActions(
            List<ActionDataDto>? actions,
            Dictionary<string, object> fieldValues,
            string ruleName)
        {
            var errors = new List<string>();

            if (actions == null || !actions.Any())
            {
                _logger?.LogDebug("No actions to validate for rule: {RuleName}", ruleName);
                return errors;
            }

            foreach (var action in actions)
            {
                try
                {
                    if (action.Type == "SetMandatory" && 
                        (action.Value?.ToString() == "true" || action.Value?.ToString() == "True"))
                    {
                        if (!fieldValues.ContainsKey(action.FieldCode) ||
                            IsEmpty(fieldValues[action.FieldCode]))
                        {
                            var error = $"Field '{action.FieldCode}' is required based on rule '{ruleName}'";
                            errors.Add(error);
                            _logger?.LogWarning(error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error validating action: Type={Type}, FieldCode={FieldCode}",
                        action.Type, action.FieldCode);
                }
            }

            return errors;
        }

        /// <summary>
        /// Parses RuleJson string into FormRuleDataDto
        /// </summary>
        /// <summary>
        /// Parse RuleJson (for backward compatibility)
        /// </summary>
        public FormRuleDataDto? ParseRuleJson(string ruleJson)
        {
            if (string.IsNullOrWhiteSpace(ruleJson))
            {
                _logger?.LogWarning("RuleJson is empty");
                return null;
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var ruleData = JsonSerializer.Deserialize<FormRuleDataDto>(ruleJson, options);
                
                if (ruleData == null)
                {
                    _logger?.LogWarning("Failed to deserialize RuleJson");
                    return null;
                }

                return ruleData;
            }
            catch (JsonException ex)
            {
                _logger?.LogError(ex, "Error parsing RuleJson: {RuleJson}", ruleJson);
                return null;
            }
        }

        /// <summary>
        /// Build FormRuleDataDto from separate fields (new approach)
        /// </summary>
        public FormRuleDataDto? BuildRuleDataFromFields(
            string? conditionField,
            string? conditionOperator,
            string? conditionValue,
            string? conditionValueType,
            string? actionsJson,
            string? elseActionsJson)
        {
            if (string.IsNullOrWhiteSpace(conditionField) || string.IsNullOrWhiteSpace(conditionOperator))
            {
                _logger?.LogWarning("Condition Field or Operator is empty");
                return null;
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var ruleData = new FormRuleDataDto
                {
                    Condition = new ConditionDataDto
                    {
                        Field = conditionField,
                        Operator = conditionOperator,
                        Value = conditionValue,
                        ValueType = conditionValueType ?? "constant"
                    }
                };

                // Parse Actions
                if (!string.IsNullOrWhiteSpace(actionsJson))
                {
                    ruleData.Actions = JsonSerializer.Deserialize<List<ActionDataDto>>(actionsJson, options);
                }

                // Parse ElseActions
                if (!string.IsNullOrWhiteSpace(elseActionsJson))
                {
                    ruleData.ElseActions = JsonSerializer.Deserialize<List<ActionDataDto>>(elseActionsJson, options);
                }

                return ruleData;
            }
            catch (JsonException ex)
            {
                _logger?.LogError(ex, "Error building rule data from fields");
                return null;
            }
        }

        // Private helper methods

        private bool CompareValues(object? a, object? b, Func<object?, object?, bool> comparer)
        {
            try
            {
                return comparer(a, b);
            }
            catch
            {
                return false;
            }
        }

        private bool CompareNumeric(object? a, object? b, Func<double, double, bool> comparer)
        {
            try
            {
                // Handle null values
                if (a == null && b == null) return false;
                if (a == null) a = 0;
                if (b == null) b = 0;

                // Convert to double, handling various numeric types and string representations
                double numA;
                double numB;

                // Handle JsonElement (from System.Text.Json)
                if (a.GetType().Name == "JsonElement")
                {
                    var jsonElement = (System.Text.Json.JsonElement)a;
                    if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.Number)
                    {
                        if (jsonElement.TryGetDouble(out double jsonDouble))
                            numA = jsonDouble;
                        else if (jsonElement.TryGetInt64(out long jsonLong))
                            numA = jsonLong;
                        else if (jsonElement.TryGetInt32(out int jsonInt))
                            numA = jsonInt;
                        else
                            numA = jsonElement.GetDouble();
                    }
                    else if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.String)
                    {
                        numA = double.Parse(jsonElement.GetString() ?? "0");
                    }
                    else
                    {
                        numA = Convert.ToDouble(a);
                    }
                }
                else if (a is double dA)
                    numA = dA;
                else if (a is float fA)
                    numA = fA;
                else if (a is decimal decA)
                    numA = (double)decA;
                else if (a is int iA)
                    numA = iA;
                else if (a is long lA)
                    numA = lA;
                else if (a is short sA)
                    numA = sA;
                else if (a is byte byteA)
                    numA = byteA;
                else if (a is string strA && double.TryParse(strA, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double parsedA))
                    numA = parsedA;
                else
                    numA = Convert.ToDouble(a);

                // Handle JsonElement for b
                if (b.GetType().Name == "JsonElement")
                {
                    var jsonElement = (System.Text.Json.JsonElement)b;
                    if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.Number)
                    {
                        if (jsonElement.TryGetDouble(out double jsonDouble))
                            numB = jsonDouble;
                        else if (jsonElement.TryGetInt64(out long jsonLong))
                            numB = jsonLong;
                        else if (jsonElement.TryGetInt32(out int jsonInt))
                            numB = jsonInt;
                        else
                            numB = jsonElement.GetDouble();
                    }
                    else if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.String)
                    {
                        numB = double.Parse(jsonElement.GetString() ?? "0");
                    }
                    else
                    {
                        numB = Convert.ToDouble(b);
                    }
                }
                else if (b is double dB)
                    numB = dB;
                else if (b is float fB)
                    numB = fB;
                else if (b is decimal decB)
                    numB = (double)decB;
                else if (b is int iB)
                    numB = iB;
                else if (b is long lB)
                    numB = lB;
                else if (b is short sB)
                    numB = sB;
                else if (b is byte byteB)
                    numB = byteB;
                else if (b is string strB && double.TryParse(strB, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double parsedB))
                    numB = parsedB;
                else
                    numB = Convert.ToDouble(b);

                var result = comparer(numA, numB);
                _logger?.LogDebug("CompareNumeric: {ValueA} ({TypeA}) vs {ValueB} ({TypeB}) = {Result}. Comparison: {NumA} vs {NumB}", 
                    a, a?.GetType().Name ?? "null", b, b?.GetType().Name ?? "null", result, numA, numB);
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Error in CompareNumeric: ValueA={ValueA} ({TypeA}), ValueB={ValueB} ({TypeB})", 
                    a, a?.GetType().Name ?? "null", b, b?.GetType().Name ?? "null");
                return false;
            }
        }

        /// <summary>
        /// Normalizes operator names to symbols (e.g., "GreaterThan" -> ">")
        /// </summary>
        private string NormalizeOperator(string? operatorValue)
        {
            if (string.IsNullOrWhiteSpace(operatorValue))
                return "=="; // Default operator

            var normalized = operatorValue.Trim();

            // Map text operator names to symbols
            // Note: Using StringComparer.OrdinalIgnoreCase means "GT" and "Gt" are the same key
            var operatorMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                // Greater than operators
                { "GreaterThan", ">" },
                { "Greater", ">" },
                { "GT", ">" },
                
                // Less than operators
                { "LessThan", "<" },
                { "Less", "<" },
                { "LT", "<" },
                
                // Greater than or equal operators
                { "GreaterThanOrEqual", ">=" },
                { "GreaterOrEqual", ">=" },
                { "GTE", ">=" },
                { "GE", ">=" },
                
                // Less than or equal operators
                { "LessThanOrEqual", "<=" },
                { "LessOrEqual", "<=" },
                { "LTE", "<=" },
                { "LE", "<=" },
                
                // Equal operators
                { "Equal", "==" },
                { "Equals", "==" },
                { "EQ", "==" },
                { "EqualTo", "==" },
                { "IsEqual", "==" },
                { "IsEqualTo", "==" },
                
                // Not equal operators
                { "NotEqual", "!=" },
                { "NotEquals", "!=" },
                { "NE", "!=" },
                { "NEQ", "!=" },
                { "NotEqualTo", "!=" },
                { "IsNotEqual", "!=" },
                { "IsNotEqualTo", "!=" },
                
                // Contains operators
                { "Contains", "contains" },
                { "Contain", "contains" },
                { "Has", "contains" },
                
                // Empty operators
                { "IsEmpty", "isEmpty" },
                { "IsNullOrEmpty", "isEmpty" },
                { "Empty", "isEmpty" },
                { "Null", "isEmpty" },
                { "IsNull", "isEmpty" },
                
                // Not empty operators
                { "IsNotEmpty", "isNotEmpty" },
                { "IsNotNullOrEmpty", "isNotEmpty" },
                { "NotEmpty", "isNotEmpty" },
                { "NotNull", "isNotEmpty" },
                { "IsNotNull", "isNotEmpty" },
                { "HasValue", "isNotEmpty" }
            };

            if (operatorMap.TryGetValue(normalized, out var mappedOperator))
            {
                _logger?.LogDebug("Operator '{Operator}' normalized to '{NormalizedOperator}'", operatorValue, mappedOperator);
                return mappedOperator;
            }

            // If not found in map, return as-is (might already be a symbol)
            return normalized;
        }

        private bool IsEmpty(object? value)
        {
            if (value == null)
                return true;

            var stringValue = value.ToString();
            return string.IsNullOrWhiteSpace(stringValue) ||
                   stringValue == "null" ||
                   stringValue == "";
        }

        /// <summary>
        /// Checks if a value is numeric (int, long, double, float, decimal, or numeric string)
        /// </summary>
        private bool IsNumeric(object? value)
        {
            if (value == null)
                return false;

            if (value is int || value is long || value is double || value is float || value is decimal ||
                value is short || value is byte || value is uint || value is ulong || value is ushort)
                return true;

            if (value is string str)
            {
                return double.TryParse(str, System.Globalization.NumberStyles.Any, 
                    System.Globalization.CultureInfo.InvariantCulture, out _);
            }

            return false;
        }

        /// <summary>
        /// Evaluates a Stored Procedure based rule by ID (from Whitelist)
        /// </summary>
        public async Task<bool> EvaluateStoredProcedureByIdAsync(
            int storedProcedureId,
            string? parameterMapping,
            string? resultMapping,
            Dictionary<string, object> fieldValues)
        {
            if (_storedProcedureService == null)
            {
                _logger?.LogError("StoredProcedureService is not available");
                throw new InvalidOperationException("StoredProcedureService is not configured");
            }

            if (storedProcedureId <= 0)
            {
                _logger?.LogWarning("Invalid stored procedure ID: {StoredProcedureId}", storedProcedureId);
                return false;
            }

            if (fieldValues == null)
            {
                _logger?.LogWarning("Field values dictionary is null");
                return false;
            }

            try
            {
                // Build parameters from form field values and parameter mapping
                var parameters = _storedProcedureService.BuildParameters(fieldValues, parameterMapping);

                // Execute stored procedure by ID (uses whitelist)
                var result = await _storedProcedureService.ExecuteStoredProcedureByIdAsync(
                    storedProcedureId,
                    parameters,
                    resultMapping);

                _logger?.LogDebug("Stored procedure (ID: {StoredProcedureId}) returned {Result}", storedProcedureId, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error evaluating stored procedure (ID: {StoredProcedureId})", storedProcedureId);
                return false;
            }
        }

        /// <summary>
        /// Evaluates a Stored Procedure based rule
        /// </summary>
        public async Task<bool> EvaluateStoredProcedureAsync(
            string procedureName,
            string databaseName,
            string? parameterMapping,
            string? resultMapping,
            Dictionary<string, object> fieldValues)
        {
            if (_storedProcedureService == null)
            {
                _logger?.LogError("StoredProcedureService is not available");
                throw new InvalidOperationException("StoredProcedureService is not configured");
            }

            if (string.IsNullOrWhiteSpace(procedureName))
            {
                _logger?.LogWarning("Stored procedure name is empty");
                return false;
            }

            if (string.IsNullOrWhiteSpace(databaseName))
            {
                _logger?.LogWarning("Database name is empty");
                return false;
            }

            if (fieldValues == null)
            {
                _logger?.LogWarning("Field values dictionary is null");
                return false;
            }

            try
            {
                // Build parameters from form field values and parameter mapping
                var parameters = _storedProcedureService.BuildParameters(fieldValues, parameterMapping);

                // Execute stored procedure (with whitelist check by default, but allow backward compatibility)
                var result = await _storedProcedureService.ExecuteStoredProcedureAsync(
                    procedureName,
                    databaseName,
                    parameters,
                    resultMapping,
                    skipWhitelistCheck: false); // Check whitelist by default

                _logger?.LogDebug("Stored procedure '{ProcedureName}' returned {Result}", procedureName, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error evaluating stored procedure '{ProcedureName}'", procedureName);
                return false;
            }
        }

        /// <summary>
        /// Evaluates blocking rules for a form at a specific evaluation phase
        /// </summary>
        public async Task<BlockingRuleResultDto> EvaluateBlockingRulesAsync(
            int formBuilderId,
            string evaluationPhase,
            int? submissionId = null,
            Dictionary<string, object>? fieldValues = null,
            bool includeDebugInfo = false)
        {
            var result = new BlockingRuleResultDto
            {
                IsBlocked = false,
                BlockMessage = null,
                MatchedRuleId = null,
                MatchedRuleName = null
            };

            var debugInfo = includeDebugInfo ? new BlockingRuleDebugInfo
            {
                Rules = new List<RuleEvaluationInfo>()
            } : null;

            if (_formRulesRepository == null)
            {
                _logger?.LogWarning("FORM_RULESRepository is not available. Cannot evaluate blocking rules.");
                return result;
            }

            try
            {
                // 1. Load all active blocking rules for the form
                var allRules = await _formRulesRepository.GetActiveRulesByFormIdAsync(formBuilderId);
                
                if (debugInfo != null)
                {
                    debugInfo.TotalActiveRules = allRules.Count();
                }
                
                _logger?.LogDebug("Loaded {Count} active rules for FormId={FormId}. Rules: {Rules}", 
                    allRules.Count(), formBuilderId, 
                    string.Join(", ", allRules.Select(r => $"Id={r.Id}, Name={r.RuleName}, Phase={r.EvaluationPhase}, Source={r.ConditionSource}")));
                
                // 2. Filter rules by evaluation phase and add filtered rules to debug info
                var blockingRules = new List<FORM_RULES>();
                var filteredRules = new List<FORM_RULES>();

                foreach (var rule in allRules)
                {
                    bool hasPhase = !string.IsNullOrWhiteSpace(rule.EvaluationPhase);
                    bool phaseMatches = hasPhase && rule.EvaluationPhase.Equals(evaluationPhase, StringComparison.OrdinalIgnoreCase);
                    bool hasSource = !string.IsNullOrWhiteSpace(rule.ConditionSource);

                    if (hasPhase && phaseMatches && hasSource)
                    {
                        blockingRules.Add(rule);
                    }
                    else
                    {
                        filteredRules.Add(rule);
                        
                        // Add filtered rule to debug info with reason
                        if (debugInfo != null)
                        {
                            var filteredRuleInfo = new RuleEvaluationInfo
                            {
                                RuleId = rule.Id,
                                RuleName = rule.RuleName,
                                EvaluationPhase = rule.EvaluationPhase,
                                ConditionSource = rule.ConditionSource,
                                ConditionField = rule.ConditionField ?? rule.ConditionKey,
                                ConditionOperator = rule.ConditionOperator,
                                ConditionValue = rule.ConditionValue,
                                ConditionMet = false,
                                ErrorMessage = BuildFilterReason(hasPhase, phaseMatches, hasSource, rule.EvaluationPhase, evaluationPhase)
                            };
                            
                            if (debugInfo.Rules == null)
                            {
                                debugInfo.Rules = new List<RuleEvaluationInfo>();
                            }
                            
                            debugInfo.Rules.Add(filteredRuleInfo);
                            _logger?.LogDebug("Rule {RuleId} ({RuleName}) filtered out: {Reason}", 
                                rule.Id, rule.RuleName ?? "unnamed", filteredRuleInfo.ErrorMessage);
                        }
                    }
                }

                // Sort blocking rules by priority and execution order
                blockingRules = blockingRules
                    .OrderByDescending(r => r.Priority ?? 0) // Higher priority first
                    .ThenBy(r => r.ExecutionOrder ?? 1)
                    .ToList();

                if (debugInfo != null)
                {
                    debugInfo.RulesEvaluated = blockingRules.Count;
                }

                if (!blockingRules.Any())
                {
                    var withoutPhase = allRules.Count(r => string.IsNullOrWhiteSpace(r.EvaluationPhase));
                    var withoutSource = allRules.Count(r => string.IsNullOrWhiteSpace(r.ConditionSource));
                    
                    _logger?.LogWarning("No blocking rules found for FormId={FormId}, Phase={Phase}. Total active rules: {TotalRules}. " +
                        "Rules without EvaluationPhase: {WithoutPhase}, Rules without ConditionSource: {WithoutSource}", 
                        formBuilderId, evaluationPhase, allRules.Count(), withoutPhase, withoutSource);
                    
                    if (debugInfo != null)
                    {
                        debugInfo.NoRulesReason = $"No blocking rules found. Total active rules: {allRules.Count()}. " +
                            $"Rules without EvaluationPhase: {withoutPhase}. Rules without ConditionSource: {withoutSource}.";
                        result.DebugInfo = debugInfo;
                    }
                    
                    return result;
                }

                _logger?.LogInformation("Evaluating {Count} blocking rules for FormId={FormId}, Phase={Phase}. Rules: {RuleIds}", 
                    blockingRules.Count, formBuilderId, evaluationPhase, string.Join(", ", blockingRules.Select(r => $"{r.Id}({r.RuleName})")));

                // 3. Evaluate each rule
                foreach (var rule in blockingRules)
                {
                    var ruleInfo = debugInfo != null ? new RuleEvaluationInfo
                    {
                        RuleId = rule.Id,
                        RuleName = rule.RuleName,
                        EvaluationPhase = rule.EvaluationPhase,
                        ConditionSource = rule.ConditionSource,
                        ConditionField = rule.ConditionField ?? rule.ConditionKey,
                        OriginalOperator = rule.ConditionOperator,
                        ConditionOperator = null, // Will be set after normalization
                        ConditionValue = rule.ConditionValue
                    } : null;

                    try
                    {
                        _logger?.LogDebug("Processing blocking rule {RuleId}: Name={RuleName}, Phase={Phase}, Source={Source}, ConditionKey={ConditionKey}, ConditionField={ConditionField}, Operator={Operator}, Value={Value}",
                            rule.Id, rule.RuleName ?? "unnamed", rule.EvaluationPhase ?? "null", rule.ConditionSource ?? "null",
                            rule.ConditionKey ?? "null", rule.ConditionField ?? "null", rule.ConditionOperator ?? "null", rule.ConditionValue ?? "null");

                        bool conditionMet = false;

                        // 4. Resolve condition values based on ConditionSource
                        if (rule.ConditionSource.Equals("Database", StringComparison.OrdinalIgnoreCase))
                        {
                            // Database-based rules: Evaluate using ConditionKey from database
                            // This would typically use a stored procedure or direct DB query
                            // For now, we'll support StoredProcedure rule type
                            if (rule.RuleType != null && rule.RuleType.Equals("StoredProcedure", StringComparison.OrdinalIgnoreCase))
                            {
                                if (rule.StoredProcedureId.HasValue && rule.StoredProcedureId.Value > 0)
                                {
                                    try
                                    {
                                        // Use stored procedure with field values
                                        var dbFieldValues = fieldValues ?? new Dictionary<string, object>();
                                        Dictionary<string, object?>? spParametersForDebug = null;
                                        StoredProcedureService.StoredProcedureExecutionDebug? spExecDebug = null;
                                        if (includeDebugInfo && _storedProcedureService != null)
                                        {
                                            try
                                            {
                                                spParametersForDebug = _storedProcedureService
                                                    .BuildParameters(dbFieldValues, rule.ParameterMapping)
                                                    .ToDictionary(kvp => kvp.Key, kvp => (object?)kvp.Value);
                                            }
                                            catch (Exception mapEx)
                                            {
                                                _logger?.LogWarning(mapEx, "Error building stored procedure parameters for debug. RuleId={RuleId}", rule.Id);
                                            }
                                        }
                                        
                                        _logger?.LogInformation("Evaluating SP Rule {RuleId}: SP ID={SPId}, ParameterMapping={ParameterMapping}, FieldValues={FieldValues}",
                                            rule.Id, rule.StoredProcedureId.Value, rule.ParameterMapping ?? "null",
                                            string.Join(", ", dbFieldValues.Select(kvp => $"{kvp.Key}={kvp.Value}")));
                                        
                                        if (includeDebugInfo && _storedProcedureService != null)
                                        {
                                            // execute with raw output capture for troubleshooting mapping/output issues
                                            _logger?.LogInformation("Using DEBUG mode for SP execution. RuleId={RuleId}, SPId={SPId}", rule.Id, rule.StoredProcedureId.Value);
                                            try
                                            {
                                                var spParams = _storedProcedureService.BuildParameters(dbFieldValues, rule.ParameterMapping);
                                                _logger?.LogDebug("Built SP parameters: {Parameters}", string.Join(", ", spParams.Select(kvp => $"{kvp.Key}={kvp.Value}")));
                                                
                                                spExecDebug = await _storedProcedureService.ExecuteStoredProcedureByIdWithDebugAsync(
                                                    rule.StoredProcedureId.Value,
                                                    spParams,
                                                    rule.ResultMapping);
                                                
                                                conditionMet = spExecDebug.Result;
                                                _logger?.LogInformation("DEBUG SP execution succeeded. Result={Result}, SelectedParam={SelectedParam}, OutputValues={OutputValues}", 
                                                    conditionMet, spExecDebug.SelectedResultParam, 
                                                    spExecDebug.OutputValues != null ? string.Join(", ", spExecDebug.OutputValues.Select(kvp => $"{kvp.Key}={kvp.Value}")) : "null");
                                            }
                                            catch (Exception exDbg)
                                            {
                                                _logger?.LogError(exDbg, "DEBUG SP execution failed; falling back to normal evaluation. RuleId={RuleId}, Error={Error}", 
                                                    rule.Id, exDbg.Message);
                                                
                                                // Store error in debug info even if debug execution failed
                                                if (ruleInfo != null)
                                                {
                                                    ruleInfo.ErrorMessage = $"DEBUG execution failed: {exDbg.Message}. Falling back to normal evaluation.";
                                                }
                                                
                                                conditionMet = await EvaluateStoredProcedureByIdAsync(
                                                    rule.StoredProcedureId.Value,
                                                    rule.ParameterMapping,
                                                    rule.ResultMapping,
                                                    dbFieldValues);
                                            }
                                        }
                                        else
                                        {
                                            _logger?.LogDebug("Using NORMAL mode for SP execution (includeDebugInfo={IncludeDebug}, ServiceAvailable={ServiceAvailable}). RuleId={RuleId}", 
                                                includeDebugInfo, _storedProcedureService != null, rule.Id);
                                            conditionMet = await EvaluateStoredProcedureByIdAsync(
                                                rule.StoredProcedureId.Value,
                                                rule.ParameterMapping,
                                                rule.ResultMapping,
                                                dbFieldValues);
                                        }
                                        
                                        _logger?.LogInformation("SP Rule {RuleId} evaluation result: ConditionMet={ConditionMet}", rule.Id, conditionMet);
                                        
                                        // Update ruleInfo for debug
                                        if (ruleInfo != null)
                                        {
                                            ruleInfo.ConditionMet = conditionMet;
                                            ruleInfo.ConditionOperator = "StoredProcedure";
                                            ruleInfo.ConditionValue = $"SP ID: {rule.StoredProcedureId.Value}";
                                            ruleInfo.StoredProcedureId = rule.StoredProcedureId.Value;
                                            ruleInfo.StoredProcedureName = rule.StoredProcedureName;
                                            ruleInfo.StoredProcedureDatabase = rule.StoredProcedureDatabase;
                                            ruleInfo.ParameterMapping = rule.ParameterMapping;
                                            ruleInfo.ResultMapping = rule.ResultMapping;
                                            ruleInfo.StoredProcedureParameters = spParametersForDebug;
                                            if (spExecDebug != null)
                                            {
                                                ruleInfo.StoredProcedureSelectedResultParam = spExecDebug.SelectedResultParam;
                                                ruleInfo.StoredProcedureOutputValues = spExecDebug.OutputValues;
                                                ruleInfo.StoredProcedureReturnValue = spExecDebug.ReturnValue;
                                                ruleInfo.StoredProcedureFirstRow = spExecDebug.FirstRow;
                                            }
                                            if (conditionMet)
                                            {
                                                ruleInfo.EvaluationResult = $"Stored procedure (ID: {rule.StoredProcedureId.Value}) returned: condition met";
                                            }
                                            else
                                            {
                                                ruleInfo.EvaluationResult = $"Stored procedure (ID: {rule.StoredProcedureId.Value}) returned: condition not met";
                                            }
                                            _logger?.LogDebug("SP Rule {RuleId} evaluation completed: ConditionMet={ConditionMet}", rule.Id, conditionMet);
                                        }
                                    }
                                    catch (Exception spEx)
                                    {
                                        _logger?.LogError(spEx, "Error evaluating SP Rule {RuleId} with StoredProcedureId {SPId}", 
                                            rule.Id, rule.StoredProcedureId.Value);
                                        conditionMet = false;
                                        if (ruleInfo != null)
                                        {
                                            // Provide detailed error message for debugging
                                            var errorDetails = spEx.Message;
                                            if (spEx.InnerException != null)
                                            {
                                                errorDetails += $" | Inner: {spEx.InnerException.Message}";
                                            }
                                            // Check if SP doesn't exist
                                            if (spEx.Message.Contains("Could not find stored procedure") || 
                                                spEx.Message.Contains("Invalid object name") ||
                                                spEx.Message.Contains("does not exist"))
                                            {
                                                errorDetails = $"Stored procedure '{rule.StoredProcedureName ?? "unknown"}' not found in database '{rule.StoredProcedureDatabase ?? "unknown"}'. Please create it first. Original error: {spEx.Message}";
                                            }
                                            ruleInfo.ErrorMessage = $"SP evaluation error: {errorDetails}";
                                            ruleInfo.ConditionMet = false;
                                            ruleInfo.ConditionOperator = "StoredProcedure";
                                            ruleInfo.ConditionValue = $"SP ID: {rule.StoredProcedureId.Value}";
                                            ruleInfo.StoredProcedureId = rule.StoredProcedureId.Value;
                                            ruleInfo.StoredProcedureName = rule.StoredProcedureName;
                                            ruleInfo.StoredProcedureDatabase = rule.StoredProcedureDatabase;
                                            ruleInfo.ParameterMapping = rule.ParameterMapping;
                                            ruleInfo.ResultMapping = rule.ResultMapping;
                                        }
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(rule.StoredProcedureName) &&
                                         !string.IsNullOrWhiteSpace(rule.StoredProcedureDatabase))
                                {
                                    var dbFieldValues = fieldValues ?? new Dictionary<string, object>();
                                    Dictionary<string, object?>? spParametersForDebug = null;
                                    if (includeDebugInfo && _storedProcedureService != null)
                                    {
                                        try
                                        {
                                            spParametersForDebug = _storedProcedureService
                                                .BuildParameters(dbFieldValues, rule.ParameterMapping)
                                                .ToDictionary(kvp => kvp.Key, kvp => (object?)kvp.Value);
                                        }
                                        catch (Exception mapEx)
                                        {
                                            _logger?.LogWarning(mapEx, "Error building stored procedure parameters for debug. RuleId={RuleId}", rule.Id);
                                        }
                                    }
                                    conditionMet = await EvaluateStoredProcedureAsync(
                                        rule.StoredProcedureName,
                                        rule.StoredProcedureDatabase,
                                        rule.ParameterMapping,
                                        rule.ResultMapping,
                                        dbFieldValues);
                                    
                                    // Update ruleInfo for debug
                                    if (ruleInfo != null)
                                    {
                                        ruleInfo.ConditionMet = conditionMet;
                                        ruleInfo.ConditionOperator = "StoredProcedure";
                                        ruleInfo.ConditionValue = $"SP: {rule.StoredProcedureName}";
                                        ruleInfo.StoredProcedureId = rule.StoredProcedureId;
                                        ruleInfo.StoredProcedureName = rule.StoredProcedureName;
                                        ruleInfo.StoredProcedureDatabase = rule.StoredProcedureDatabase;
                                        ruleInfo.ParameterMapping = rule.ParameterMapping;
                                        ruleInfo.ResultMapping = rule.ResultMapping;
                                        ruleInfo.StoredProcedureParameters = spParametersForDebug;
                                        if (conditionMet)
                                        {
                                            ruleInfo.EvaluationResult = $"Stored procedure '{rule.StoredProcedureName}' returned: condition met";
                                        }
                                        else
                                        {
                                            ruleInfo.EvaluationResult = $"Stored procedure '{rule.StoredProcedureName}' returned: condition not met";
                                        }
                                    }
                                }
                                else
                                {
                                    _logger?.LogWarning("Database-based blocking rule {RuleId} requires StoredProcedureId or StoredProcedureName/Database", rule.Id);
                                    if (ruleInfo != null)
                                    {
                                        ruleInfo.ErrorMessage = "StoredProcedureId or StoredProcedureName/Database is required";
                                        ruleInfo.ConditionMet = false;
                                    }
                                }
                            }
                            else
                            {
                                // For Database-based rules without stored procedure, 
                                // we would need to query the database using ConditionKey
                                _logger?.LogWarning("Database-based blocking rule {RuleId} requires StoredProcedure type", rule.Id);
                                if (ruleInfo != null)
                                {
                                    ruleInfo.ErrorMessage = "Database-based rules require StoredProcedure type";
                                    ruleInfo.ConditionMet = false;
                                }
                            }
                        }
                        else if (rule.ConditionSource.Equals("Submission", StringComparison.OrdinalIgnoreCase))
                        {
                            // Submission-based rules: Evaluate using field values
                            if (fieldValues == null || !fieldValues.Any())
                            {
                                _logger?.LogWarning("Submission-based blocking rule {RuleId} requires field values", rule.Id);
                                if (ruleInfo != null)
                                {
                                    ruleInfo.ErrorMessage = "Field values are required but not provided";
                                    ruleInfo.ConditionMet = false;
                                    debugInfo?.Rules?.Add(ruleInfo);
                                }
                                continue;
                            }

                            // Use ConditionKey or ConditionField
                            var conditionField = !string.IsNullOrWhiteSpace(rule.ConditionKey) 
                                ? rule.ConditionKey 
                                : rule.ConditionField;

                            // Normalize field name to uppercase for consistency
                            if (!string.IsNullOrWhiteSpace(conditionField))
                            {
                                conditionField = conditionField.ToUpperInvariant();
                            }

                            _logger?.LogDebug("Rule {RuleId}: ConditionKey='{ConditionKey}', ConditionField='{ConditionField}', SelectedField='{SelectedField}' (normalized)",
                                rule.Id, rule.ConditionKey ?? "null", rule.ConditionField ?? "null", conditionField ?? "null");

                            if (string.IsNullOrWhiteSpace(conditionField))
                            {
                                _logger?.LogWarning("Blocking rule {RuleId} has no ConditionKey or ConditionField. ConditionKey='{ConditionKey}', ConditionField='{ConditionField}'", 
                                    rule.Id, rule.ConditionKey ?? "null", rule.ConditionField ?? "null");
                                if (ruleInfo != null)
                                {
                                    ruleInfo.ErrorMessage = $"No ConditionKey or ConditionField specified. ConditionKey='{rule.ConditionKey ?? "null"}', ConditionField='{rule.ConditionField ?? "null"}'";
                                    ruleInfo.ConditionMet = false;
                                    debugInfo?.Rules?.Add(ruleInfo);
                                }
                                continue;
                            }

                            // Normalize operator before building condition
                            var normalizedOperator = NormalizeOperator(rule.ConditionOperator);
                            
                            // Update ruleInfo with normalized operator
                            if (ruleInfo != null)
                            {
                                ruleInfo.ConditionOperator = normalizedOperator ?? "==";
                                ruleInfo.OriginalOperator = rule.ConditionOperator; // Keep original for debugging
                                ruleInfo.ConditionField = conditionField;
                            }
                            
                            // Build condition from rule fields
                            var condition = new ConditionDataDto
                            {
                                Field = conditionField,
                                Operator = normalizedOperator ?? "==",
                                Value = rule.ConditionValue,
                                ValueType = rule.ConditionValueType ?? "constant"
                            };
                            
                            _logger?.LogDebug("Rule {RuleId}: Original operator '{OriginalOperator}' normalized to '{NormalizedOperator}'",
                                rule.Id, rule.ConditionOperator ?? "null", normalizedOperator);

                            _logger?.LogInformation("Evaluating Submission-based blocking rule {RuleId} ({RuleName}): Field={Field}, Operator={Operator} (original: {OriginalOperator}), Value={Value}, ValueType={ValueType}, FieldValues={FieldValues}",
                                rule.Id, rule.RuleName ?? "unnamed", conditionField, condition.Operator, rule.ConditionOperator ?? "null", condition.Value ?? "null", condition.ValueType,
                                string.Join(", ", fieldValues.Select(kvp => $"{kvp.Key}={kvp.Value}({kvp.Value?.GetType().Name ?? "null"})")));

                            conditionMet = EvaluateCondition(condition, fieldValues);
                            
                            _logger?.LogInformation("Blocking rule {RuleId} ({RuleName}) evaluation result: {Result}. Condition: {Field} {Operator} {Value}", 
                                rule.Id, rule.RuleName ?? "unnamed", conditionMet, conditionField, condition.Operator, condition.Value ?? "null");
                            
                            if (ruleInfo != null)
                            {
                                ruleInfo.ConditionMet = conditionMet;
                                if (conditionMet)
                                {
                                    ruleInfo.EvaluationResult = $"Condition met: {conditionField} {condition.Operator} {condition.Value}";
                                }
                                else
                                {
                                    var fieldValueStr = fieldValues?.ContainsKey(conditionField) == true 
                                        ? fieldValues[conditionField]?.ToString() ?? "null"
                                        : "field not found";
                                    ruleInfo.EvaluationResult = $"Condition not met: {conditionField} ({fieldValueStr}) {condition.Operator} {condition.Value}";
                                }
                            }
                        }
                        else
                        {
                            _logger?.LogWarning("Blocking rule {RuleId} has unknown ConditionSource: '{ConditionSource}'", rule.Id, rule.ConditionSource ?? "null");
                            if (ruleInfo != null)
                            {
                                ruleInfo.ErrorMessage = $"Unknown ConditionSource: {rule.ConditionSource}";
                            }
                        }

                        // Add ruleInfo to debug info (always, even if condition not met or error occurred)
                        if (ruleInfo != null && debugInfo != null)
                        {
                            // Check if ruleInfo is already added (avoid duplicates)
                            if (!debugInfo.Rules.Any(r => r.RuleId == ruleInfo.RuleId))
                            {
                                debugInfo.Rules.Add(ruleInfo);
                                _logger?.LogInformation("Added rule {RuleId} ({RuleName}) to debug info. ConditionMet={ConditionMet}, Operator={Operator}",
                                    ruleInfo.RuleId, ruleInfo.RuleName ?? "unnamed", ruleInfo.ConditionMet, ruleInfo.ConditionOperator ?? "null");
                            }
                            else
                            {
                                _logger?.LogWarning("Rule {RuleId} already exists in debug info, skipping duplicate", ruleInfo.RuleId);
                            }
                        }
                        else if (ruleInfo != null && debugInfo == null)
                        {
                            _logger?.LogWarning("RuleInfo for rule {RuleId} exists but debugInfo is null", ruleInfo.RuleId);
                        }
                        else if (ruleInfo == null && debugInfo != null)
                        {
                            _logger?.LogWarning("DebugInfo exists but ruleInfo is null for rule {RuleId}", rule.Id);
                        }

                        // 5. If condition is met, block and return
                        if (conditionMet)
                        {
                            // Ensure ruleInfo is added to debug info before returning
                            if (ruleInfo != null && debugInfo != null && !debugInfo.Rules.Contains(ruleInfo))
                            {
                                debugInfo.Rules.Add(ruleInfo);
                            }
                            
                            result.IsBlocked = true;
                            result.BlockMessage = rule.BlockMessage ?? 
                                $"Form access is blocked by rule '{rule.RuleName}'";
                            result.MatchedRuleId = rule.Id;
                            result.MatchedRuleName = rule.RuleName;

                            _logger?.LogWarning("Blocking rule matched: RuleId={RuleId}, RuleName={RuleName}, Phase={Phase}, FormId={FormId}",
                                rule.Id, rule.RuleName, evaluationPhase, formBuilderId);
                            
                            // Log to audit table
                            await LogBlockingRuleEvaluationAsync(
                                formBuilderId,
                                submissionId,
                                evaluationPhase,
                                rule.Id,
                                rule.RuleName,
                                true,
                                result.BlockMessage,
                                null);
                            
                            if (debugInfo != null)
                            {
                                result.DebugInfo = debugInfo;
                            }
                            
                            // Return immediately with highest priority rule
                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "Error evaluating blocking rule {RuleId}: {RuleName}", 
                            rule.Id, rule.RuleName);
                        
                        // Add rule info with error message for debugging
                        if (ruleInfo != null)
                        {
                            ruleInfo.ErrorMessage = $"Error: {ex.Message}";
                            ruleInfo.ConditionMet = false;
                            debugInfo?.Rules?.Add(ruleInfo);
                        }
                        
                        // Continue with other rules - don't fail entire evaluation
                    }
                }

                // Log evaluation result (even if not blocked) for audit purposes
                await LogBlockingRuleEvaluationAsync(
                    formBuilderId,
                    submissionId,
                    evaluationPhase,
                    null,
                    null,
                    false,
                    null,
                    null);

                if (debugInfo != null)
                {
                    result.DebugInfo = debugInfo;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error evaluating blocking rules for FormId={FormId}, Phase={Phase}", 
                    formBuilderId, evaluationPhase);
                
                // Log error to audit table
                await LogBlockingRuleEvaluationAsync(
                    formBuilderId,
                    submissionId,
                    evaluationPhase,
                    null,
                    null,
                    false,
                    $"Error: {ex.Message}",
                    null);
                
                // Return unblocked on error to avoid blocking legitimate access
                return result;
            }
        }

        /// <summary>
        /// Logs blocking rule evaluation to audit table
        /// </summary>
        private async Task LogBlockingRuleEvaluationAsync(
            int formBuilderId,
            int? submissionId,
            string evaluationPhase,
            int? ruleId,
            string? ruleName,
            bool isBlocked,
            string? blockMessage,
            string? userId)
        {
            try
            {
                if (_unitOfWork == null)
                {
                    _logger?.LogDebug("UnitOfWork not available. Skipping audit log.");
                    return;
                }

                var auditLog = new BLOCKING_RULE_AUDIT_LOG
                {
                    FormBuilderId = formBuilderId,
                    SubmissionId = submissionId,
                    EvaluationPhase = evaluationPhase,
                    RuleId = ruleId,
                    RuleName = ruleName,
                    IsBlocked = isBlocked,
                    BlockMessage = blockMessage,
                    UserId = userId,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false
                };

                var repository = _unitOfWork.Repositary<BLOCKING_RULE_AUDIT_LOG>();
                repository.Add(auditLog);
                await _unitOfWork.CompleteAsyn();

                _logger?.LogDebug("Blocking rule evaluation logged: FormId={FormId}, Phase={Phase}, IsBlocked={IsBlocked}",
                    formBuilderId, evaluationPhase, isBlocked);
            }
            catch (Exception ex)
            {
                // Don't fail the main operation if logging fails
                _logger?.LogError(ex, "Error logging blocking rule evaluation to audit table");
            }
        }

        /// <summary>
        /// Builds a reason message explaining why a rule was filtered out
        /// </summary>
        private string BuildFilterReason(bool hasPhase, bool phaseMatches, bool hasSource, string? rulePhase, string requestedPhase)
        {
            var reasons = new List<string>();

            if (!hasPhase)
            {
                reasons.Add("missing EvaluationPhase");
            }
            else if (!phaseMatches)
            {
                reasons.Add($"EvaluationPhase '{rulePhase}' does not match requested phase '{requestedPhase}'");
            }

            if (!hasSource)
            {
                reasons.Add("missing ConditionSource");
            }

            if (reasons.Count == 0)
            {
                return "Rule was filtered out (unknown reason)";
            }

            return $"Rule filtered out: {string.Join(", ", reasons)}";
        }
    }
}

