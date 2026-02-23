using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.DTOS.FormRules
{
    /// <summary>
    /// Request DTO for evaluating blocking rules
    /// </summary>
    public class EvaluateBlockingRulesRequestDto
    {
        [Required]
        public int FormBuilderId { get; set; }

        [Required]
        [RegularExpression("PreOpen|PreSubmit", ErrorMessage = "EvaluationPhase must be either 'PreOpen' or 'PreSubmit'")]
        public string EvaluationPhase { get; set; } = string.Empty; // PreOpen or PreSubmit

        /// <summary>
        /// Submission ID (required for PreSubmit, optional for PreOpen)
        /// </summary>
        public int? SubmissionId { get; set; }

        /// <summary>
        /// Field values dictionary (for PreSubmit evaluation)
        /// </summary>
        public Dictionary<string, object>? FieldValues { get; set; }
    }
}

