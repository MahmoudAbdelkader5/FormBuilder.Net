using System;

namespace FormBuilder.API.DTOs
{
    public class FormGridDto
    {
        public int Id { get; set; }
        public int FormBuilderId { get; set; }
        public string FormBuilderName { get; set; } = string.Empty;
        public string GridName { get; set; } = string.Empty;
        public string GridCode { get; set; } = string.Empty;
        public int? TabId { get; set; }
        public string TabName { get; set; } = string.Empty;
        public int GridOrder { get; set; }
        public bool IsActive { get; set; }
        public int? MinRows { get; set; }
        public int? MaxRows { get; set; }
        public string? GridRulesJson { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class CreateFormGridDto
    {
        public int FormBuilderId { get; set; }
        public string GridName { get; set; } = string.Empty;
        public string GridCode { get; set; } = string.Empty;
        public int? TabId { get; set; }
        public int? GridOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public int? MinRows { get; set; }
        public int? MaxRows { get; set; }
        public string? GridRulesJson { get; set; }
    }

    public class UpdateFormGridDto
    {
        public string GridName { get; set; } = string.Empty;
        public string GridCode { get; set; } = string.Empty;
        public int? TabId { get; set; }
        public int? GridOrder { get; set; }
        public bool? IsActive { get; set; }
        public int? MinRows { get; set; }
        public int? MaxRows { get; set; }
        public string? GridRulesJson { get; set; }
    }

    public class CopyFormGridDto
    {
        public int SourceGridId { get; set; }
        public int TargetFormBuilderId { get; set; }
        public string? NewGridName { get; set; }
        public string? NewGridCode { get; set; }
        public int? TargetTabId { get; set; }
        public bool CopyColumns { get; set; } = true;
    }
}