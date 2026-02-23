namespace FormBuilder.Core.DTOS.FormRules
{
    /// <summary>
    /// Result of evaluating blocking rules
    /// </summary>
    public class BlockingRuleResultDto
    {
        /// <summary>
        /// Whether the form/document is blocked
        /// </summary>
        public bool IsBlocked { get; set; }

        /// <summary>
        /// Block message to display to the user
        /// </summary>
        public string? BlockMessage { get; set; }

        /// <summary>
        /// ID of the rule that caused the block (if any)
        /// </summary>
        public int? MatchedRuleId { get; set; }

        /// <summary>
        /// Name of the rule that caused the block (if any)
        /// </summary>
        public string? MatchedRuleName { get; set; }

        /// <summary>
        /// Debug information about rules evaluated (for development/testing)
        /// </summary>
        public BlockingRuleDebugInfo? DebugInfo { get; set; }
    }

    /// <summary>
    /// Debug information for blocking rules evaluation
    /// </summary>
    public class BlockingRuleDebugInfo
    {
        /// <summary>
        /// Total number of active rules found for the form
        /// </summary>
        public int TotalActiveRules { get; set; }

        /// <summary>
        /// Number of blocking rules that matched the evaluation phase
        /// </summary>
        public int RulesEvaluated { get; set; }

        /// <summary>
        /// List of rules that were evaluated
        /// </summary>
        public List<RuleEvaluationInfo>? Rules { get; set; }

        /// <summary>
        /// Summary of why no rules matched (if applicable)
        /// </summary>
        public string? NoRulesReason { get; set; }
    }

    /// <summary>
    /// Information about a single rule evaluation
    /// </summary>
    public class RuleEvaluationInfo
    {
        public int RuleId { get; set; }
        public string? RuleName { get; set; }
        public string? EvaluationPhase { get; set; }
        public string? ConditionSource { get; set; }
        public string? ConditionField { get; set; }
        public string? ConditionOperator { get; set; }
        public string? OriginalOperator { get; set; }
        public string? ConditionValue { get; set; }
        public bool ConditionMet { get; set; }
        public string? EvaluationResult { get; set; }
        public string? ErrorMessage { get; set; }

        // Extra debug-only fields (populated when includeDebugInfo=true)
        public int? StoredProcedureId { get; set; }
        public string? StoredProcedureName { get; set; }
        public string? StoredProcedureDatabase { get; set; }
        public string? ParameterMapping { get; set; }
        public string? ResultMapping { get; set; }
        public Dictionary<string, object?>? StoredProcedureParameters { get; set; }
        public string? StoredProcedureSelectedResultParam { get; set; }
        public Dictionary<string, object?>? StoredProcedureOutputValues { get; set; }
        public object? StoredProcedureReturnValue { get; set; }
        public Dictionary<string, object?>? StoredProcedureFirstRow { get; set; }
    }
}

