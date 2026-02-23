using formBuilder.Domian.Entitys;
using FormBuilder.Domian.Entitys.FormBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Domian.Entitys.froms
{
    [Table("FORM_RULES")]
    public class FORM_RULES :BaseEntity
    {
    

        [ForeignKey("FORM_BUILDER")]
        public int FormBuilderId { get; set; }
        public virtual FORM_BUILDER FORM_BUILDER { get; set; }

        [Required, StringLength(200)]
        public string RuleName { get; set; }

        /// <summary>
        /// Rule Type: "Condition" (standard field-based condition) or "StoredProcedure" (database-driven)
        /// </summary>
        [Required, StringLength(50)]
        public string RuleType { get; set; } = "Condition"; // Default to Condition for backward compatibility

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
        /// Reference to FORM_STORED_PROCEDURES (Whitelist) - preferred method
        /// </summary>
        public int? StoredProcedureId { get; set; }
        public virtual FORM_STORED_PROCEDURES? FORM_STORED_PROCEDURES { get; set; }

        /// <summary>
        /// Stored Procedure Name (e.g., "sp_CheckEmployeeGrade") - kept for backward compatibility
        /// </summary>
        [StringLength(200)]
        public string? StoredProcedureName { get; set; }

        /// <summary>
        /// Database Name where the Stored Procedure exists (FormBuilder or AKHManageIT) - kept for backward compatibility
        /// </summary>
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
        /// or {"resultColumn": "Result", "trueValue": "true", "falseValue": "false"}
        /// </summary>
        public string? ResultMapping { get; set; }

        // Navigation property for Actions (stored in separate table)
        public virtual ICollection<FORM_RULE_ACTIONS> FORM_RULE_ACTIONS { get; set; }

        // Keep RuleJson for backward compatibility (nullable)
        public string? RuleJson { get; set; }

        public new bool IsActive { get; set; }

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
        /// Can use ConditionField for Submission-based rules
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

        // Constructor
        public FORM_RULES()
        {
            FORM_RULE_ACTIONS = new HashSet<FORM_RULE_ACTIONS>();
        }
    }
}
