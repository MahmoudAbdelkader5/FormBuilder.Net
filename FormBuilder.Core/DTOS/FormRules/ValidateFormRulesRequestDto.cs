using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace FormBuilder.Core.DTOS.FormRules
{
    /// <summary>
    /// Request DTO for validating form rules
    /// </summary>
    public class ValidateFormRulesRequestDto
    {
        /// <summary>
        /// Form Builder ID
        /// </summary>
        [Required]
        public int FormBuilderId { get; set; }

        /// <summary>
        /// Dictionary of field codes and their values (as JsonElement to support arrays and complex types)
        /// </summary>
        public Dictionary<string, JsonElement> FieldValues { get; set; } = new Dictionary<string, JsonElement>();
    }
}

