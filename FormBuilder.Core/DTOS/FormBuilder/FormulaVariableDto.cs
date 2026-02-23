namespace FormBuilder.Application.DTOs.Formula
{
    public class FormulaVariableDto
    {
        public int Id { get; set; }
        public string VariableName { get; set; }
        public int FormulaId { get; set; }
        public string Formulaname { get; set; } // Formula Name
        public int SourceFieldId { get; set; }
        public string SourceFieldName { get; set; }
    }

    public class FormulaVariableCreateDto
    {
        public string VariableName { get; set; }
        public int FormulaId { get; set; }
        public int SourceFieldId { get; set; }
    }

    public class FormulaVariableUpdateDto
    {
        public string VariableName { get; set; }
        public int? FormulaId { get; set; }
        public int? SourceFieldId { get; set; }
    }
}
