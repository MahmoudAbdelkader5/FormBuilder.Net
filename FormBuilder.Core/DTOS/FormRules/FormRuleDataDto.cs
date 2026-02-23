using System.Collections.Generic;

namespace FormBuilder.Core.DTOS.FormRules
{
    /// <summary>
    /// Represents the complete rule structure (parsed from RuleJson)
    /// </summary>
    public class FormRuleDataDto
    {
        /// <summary>
        /// Condition (IF part)
        /// </summary>
        public ConditionDataDto? Condition { get; set; }

        /// <summary>
        /// Actions to execute when condition is true (THEN part)
        /// </summary>
        public List<ActionDataDto>? Actions { get; set; }

        /// <summary>
        /// Actions to execute when condition is false (ELSE part)
        /// </summary>
        public List<ActionDataDto>? ElseActions { get; set; }
    }
}

