using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.DTOS.FormRules
{
    /// <summary>
    /// Request DTO for evaluating a single rule
    /// </summary>
    public class EvaluateRuleRequestDto
    {
        /// <summary>
        /// Rule ID to evaluate
        /// </summary>
        [Required]
        public int RuleId { get; set; }

        /// <summary>
        /// Dictionary of field codes and their values
        /// </summary>
        public Dictionary<string, object> FieldValues { get; set; } = new Dictionary<string, object>();
    }
}

