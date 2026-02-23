using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.DTOS.FormRules
{
    public class CreateFormRuleDto
    {
        [Required]
        public int FormBuilderId { get; set; }

        [Required]
        [StringLength(200)]
        public string RuleName { get; set; }

        /// <summary>
        /// Rule Type: "Condition" (standard field-based condition) or "StoredProcedure" (database-driven)
        /// </summary>
        [StringLength(50)]
        public string RuleType { get; set; } = "Condition"; // Default to Condition

        // Condition Fields (for RuleType = "Condition")
        [StringLength(100)]
        public string? ConditionField { get; set; }

        [StringLength(50)]
        public string? ConditionOperator { get; set; }

        [StringLength(500)]
        public string? ConditionValue { get; set; }

        [StringLength(20)]
        public string? ConditionValueType { get; set; } // "constant" or "field"

        // Stored Procedure Fields (for RuleType = "StoredProcedure")
        /// <summary>
        /// ID of the stored procedure from FORM_STORED_PROCEDURES whitelist (alternative to StoredProcedureName/Database)
        /// </summary>
        public int? StoredProcedureId { get; set; }

        [StringLength(200)]
        public string? StoredProcedureName { get; set; }

        [StringLength(100)]
        public string? StoredProcedureDatabase { get; set; }

        /// <summary>
        /// JSON mapping of Form Fields to Stored Procedure Parameters
        /// Example: {"@EmployeeId": "employeeId", "@Grade": "grade"}
        /// </summary>
        public string? ParameterMapping { get; set; }

        /// <summary>
        /// JSON mapping of Stored Procedure result to boolean/action
        /// Example: {"resultColumn": "IsValid", "trueValue": 1, "falseValue": 0}
        /// </summary>
        public string? ResultMapping { get; set; }

        // Actions as List (not JSON)
        public List<ActionDataDto>? Actions { get; set; }

        // Else Actions as List (not JSON)
        public List<ActionDataDto>? ElseActions { get; set; }

        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Execution order - lower numbers execute first
        /// </summary>
        public int? ExecutionOrder { get; set; } = 1;

        // Blocking Rules Fields (for Data-Driven Blocking)
        /// <summary>
        /// Evaluation Phase: "PreOpen" (before form creation) or "PreSubmit" (before submission)
        /// </summary>
        [StringLength(20)]
        public string? EvaluationPhase { get; set; } // PreOpen, PreSubmit

        /// <summary>
        /// Condition Source: "Database" (evaluated from DB) or "Submission" (evaluated from form values)
        /// </summary>
        [StringLength(20)]
        public string? ConditionSource { get; set; } // Database, Submission

        /// <summary>
        /// Condition Key: FieldCode (for Submission) or predefined DB key (for Database)
        /// </summary>
        [StringLength(100)]
        public string? ConditionKey { get; set; }

        /// <summary>
        /// Block Message: Message to display when rule blocks access or submission
        /// </summary>
        [StringLength(1000)]
        public string? BlockMessage { get; set; }

        /// <summary>
        /// Priority: Higher priority rules are evaluated first (for blocking rules)
        /// </summary>
        public int? Priority { get; set; }
    }

    public class UpdateFormRuleDto
    {
        [Required]
        public int FormBuilderId { get; set; }

        [Required]
        [StringLength(200)]
        public string RuleName { get; set; }

        /// <summary>
        /// Rule Type: "Condition" (standard field-based condition) or "StoredProcedure" (database-driven)
        /// </summary>
        [StringLength(50)]
        public string RuleType { get; set; } = "Condition"; // Default to Condition

        // Condition Fields (for RuleType = "Condition")
        [StringLength(100)]
        public string? ConditionField { get; set; }

        [StringLength(50)]
        public string? ConditionOperator { get; set; }

        [StringLength(500)]
        public string? ConditionValue { get; set; }

        [StringLength(20)]
        public string? ConditionValueType { get; set; } // "constant" or "field"

        // Stored Procedure Fields (for RuleType = "StoredProcedure")
        /// <summary>
        /// ID of the stored procedure from FORM_STORED_PROCEDURES whitelist (alternative to StoredProcedureName/Database)
        /// </summary>
        public int? StoredProcedureId { get; set; }

        [StringLength(200)]
        public string? StoredProcedureName { get; set; }

        [StringLength(100)]
        public string? StoredProcedureDatabase { get; set; }

        /// <summary>
        /// JSON mapping of Form Fields to Stored Procedure Parameters
        /// Example: {"@EmployeeId": "employeeId", "@Grade": "grade"}
        /// </summary>
        public string? ParameterMapping { get; set; }

        /// <summary>
        /// JSON mapping of Stored Procedure result to boolean/action
        /// Example: {"resultColumn": "IsValid", "trueValue": 1, "falseValue": 0}
        /// </summary>
        public string? ResultMapping { get; set; }

        // Actions as List (not JSON)
        public List<ActionDataDto>? Actions { get; set; }

        // Else Actions as List (not JSON)
        public List<ActionDataDto>? ElseActions { get; set; }

        public bool IsActive { get; set; }

        /// <summary>
        /// Execution order - lower numbers execute first
        /// </summary>
        public int? ExecutionOrder { get; set; }

        // Blocking Rules Fields (for Data-Driven Blocking)
        /// <summary>
        /// Evaluation Phase: "PreOpen" (before form creation) or "PreSubmit" (before submission)
        /// </summary>
        [StringLength(20)]
        public string? EvaluationPhase { get; set; } // PreOpen, PreSubmit

        /// <summary>
        /// Condition Source: "Database" (evaluated from DB) or "Submission" (evaluated from form values)
        /// </summary>
        [StringLength(20)]
        public string? ConditionSource { get; set; } // Database, Submission

        /// <summary>
        /// Condition Key: FieldCode (for Submission) or predefined DB key (for Database)
        /// </summary>
        [StringLength(100)]
        public string? ConditionKey { get; set; }

        /// <summary>
        /// Block Message: Message to display when rule blocks access or submission
        /// </summary>
        [StringLength(1000)]
        public string? BlockMessage { get; set; }

        /// <summary>
        /// Priority: Higher priority rules are evaluated first (for blocking rules)
        /// </summary>
        public int? Priority { get; set; }
    }

    public class FormRuleDto
    {
        public int Id { get; set; }
        public int FormBuilderId { get; set; }
        public string RuleName { get; set; }
        public string RuleType { get; set; } = "Condition";
        
        // Condition Fields (for RuleType = "Condition")
        public string? ConditionField { get; set; }
        public string? ConditionOperator { get; set; }
        public string? ConditionValue { get; set; }
        public string? ConditionValueType { get; set; }

        // Stored Procedure Fields (for RuleType = "StoredProcedure")
        public int? StoredProcedureId { get; set; }
        public string? StoredProcedureName { get; set; }
        public string? StoredProcedureDatabase { get; set; }
        public string? ParameterMapping { get; set; }
        public string? ResultMapping { get; set; }

        // Actions as List (not JSON)
        public List<ActionDataDto>? Actions { get; set; }

        // Else Actions as List (not JSON)
        public List<ActionDataDto>? ElseActions { get; set; }
        
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? ExecutionOrder { get; set; }
        
        // Blocking Rules Fields (for Data-Driven Blocking)
        public string? EvaluationPhase { get; set; } // PreOpen, PreSubmit
        public string? ConditionSource { get; set; } // Database, Submission
        public string? ConditionKey { get; set; }
        public string? BlockMessage { get; set; }
        public int? Priority { get; set; }
        
        public string FormName { get; set; }
        public string FormCode { get; set; }
    }
}