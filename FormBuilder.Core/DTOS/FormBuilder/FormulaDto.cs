using FormBuilder.Application.DTOs.Formula;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.DTOS.FormBuilder
{
    public class FormulaDto
    {
        public int Id { get; set; }
        public int FormBuilderId { get; set; }
        public string FormBuilderName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ExpressionText { get; set; }
        public int? ResultFieldId { get; set; }
        public string ResultFieldName { get; set; }
        public string ResultFieldCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int VariableCount { get; set; }
        public List<FormulaVariableDto> Variables { get; set; } = new List<FormulaVariableDto>();
    }

    //public class FormulaVariableDto
    //{
    //    public int Id { get; set; }
    //    public int FormulaId { get; set; }
    //    public int FieldId { get; set; }
    //    public string FieldCode { get; set; }
    //    public string FieldName { get; set; }
    //    public string FieldType { get; set; }
    //    public string VariableName { get; set; }
    //}

    public class FormulaFieldInfoDto
    {
        public int FieldId { get; set; }
        public string FieldCode { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public string TabName { get; set; }
        public int FormBuilderId { get; set; }
        public string FormBuilderName { get; set; }
        public bool IsActive { get; set; }
    }

    public class ValidateExpressionResultDto
    {
        public bool IsValid { get; set; }
        public List<string> ValidFieldCodes { get; set; } = new List<string>();
        public List<string> InvalidFieldCodes { get; set; } = new List<string>();
        public List<FormulaFieldInfoDto> FieldDetails { get; set; } = new List<FormulaFieldInfoDto>();
    }

    public class CreateFormulaDto
    {
        [Required]
        public int FormBuilderId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Code { get; set; }

        [Required]
        public string ExpressionText { get; set; }

        public int? ResultFieldId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpdateFormulaDto
    {
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Code { get; set; }

        public string ExpressionText { get; set; }

        public int? ResultFieldId { get; set; }

        public bool? IsActive { get; set; }
    }

    public class ValidateExpressionDto
    {
        [Required]
        public string ExpressionText { get; set; }

        [Required]
        public int FormBuilderId { get; set; }
    }

    public class DuplicateFormulaDto
    {
        [Required]
        public int TargetFormBuilderId { get; set; }

        [Required]
        [StringLength(100)]
        public string NewCode { get; set; }

        [Required]
        [StringLength(200)]
        public string NewName { get; set; }
    }

    public class FormulaStatisticsDto
    {
        public int TotalFormulas { get; set; }
        public int ActiveFormulas { get; set; }
        public int InactiveFormulas { get; set; }
        public int FormulasWithResultField { get; set; }
        public int FormulasWithoutResultField { get; set; }
        public int TotalVariables { get; set; }
        public double AverageVariablesPerFormula { get; set; }
    }

    public class BatchUpdateResultDto
    {
        public int UpdatedCount { get; set; }
        public int FailedCount { get; set; }
        public List<int> FailedIds { get; set; } = new List<int>();
        public string Message { get; set; }
    }

    // For controller use
    public class BatchUpdateFormulaStatusDto
    {
        public List<int> FormulaIds { get; set; }
        public bool IsActive { get; set; }
    }
    // Add to your existing DTOs

    public class PreviewCalculationDto
    {
        public string ExpressionText { get; set; }
        public int FormBuilderId { get; set; }
        public Dictionary<string, object> FieldValues { get; set; } = new Dictionary<string, object>();
    }

    public class CalculateFormulaDto
    {
        public int FormulaId { get; set; }
        public Dictionary<string, object> FieldValues { get; set; } = new Dictionary<string, object>();
    }

    public class BatchCalculateDto
    {
        public int FormBuilderId { get; set; }
        public Dictionary<string, object> FieldValues { get; set; } = new Dictionary<string, object>();
        public List<int>? FormulaIds { get; set; }
    }

    public class CalculateExpressionDto
    {
        public string ExpressionText { get; set; } = string.Empty;
        public Dictionary<string, object> FieldValues { get; set; } = new Dictionary<string, object>();
    }

    public class CalculateAdvancedDto
    {
        public string ExpressionText { get; set; } = string.Empty;
        public Dictionary<string, object> FieldValues { get; set; } = new Dictionary<string, object>();
    }
}