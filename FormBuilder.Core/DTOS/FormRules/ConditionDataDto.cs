namespace FormBuilder.Core.DTOS.FormRules
{
    /// <summary>
    /// Represents a condition in a form rule (IF part)
    /// </summary>
    public class ConditionDataDto
    {
        /// <summary>
        /// Field code to evaluate
        /// </summary>
        public string Field { get; set; } = string.Empty;

        /// <summary>
        /// Operator: ==, !=, >, <, >=, <=, contains, isEmpty, isNotEmpty
        /// </summary>
        public string Operator { get; set; } = "==";

        /// <summary>
        /// Value to compare against
        /// </summary>
        public object? Value { get; set; }

        /// <summary>
        /// Type of value: "constant" or "field"
        /// </summary>
        public string ValueType { get; set; } = "constant";
    }
}

