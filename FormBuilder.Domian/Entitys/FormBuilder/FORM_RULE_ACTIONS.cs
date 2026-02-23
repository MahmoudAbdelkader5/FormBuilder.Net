using formBuilder.Domian.Entitys;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormBuilder.Domian.Entitys.froms
{
    [Table("FORM_RULE_ACTIONS")]
    public class FORM_RULE_ACTIONS : BaseEntity
    {
        [ForeignKey("FORM_RULES")]
        public int RuleId { get; set; }
        public virtual FORM_RULES FORM_RULES { get; set; }

        /// <summary>
        /// Action Type: SetVisible, SetReadOnly, SetMandatory, SetDefault, ClearValue, Compute, Block
        /// Block: Blocks form access or submission (for blocking rules)
        /// </summary>
        [Required, StringLength(50)]
        public string ActionType { get; set; } // SetVisible, SetReadOnly, SetMandatory, SetDefault, ClearValue, Compute, Block

        /// <summary>
        /// Field Code: Required for most actions, optional for Block action
        /// </summary>
        [StringLength(100)]
        public string? FieldCode { get; set; }

        [StringLength(500)]
        public string? Value { get; set; } // For SetVisible, SetReadOnly, SetMandatory, SetDefault

        [StringLength(1000)]
        public string? Expression { get; set; } // For Compute action

        [Required]
        public bool IsElseAction { get; set; } = false; // false = Action, true = ElseAction

        public int ActionOrder { get; set; } = 1; // Order of execution

        public new bool IsActive { get; set; } = true;
    }
}

