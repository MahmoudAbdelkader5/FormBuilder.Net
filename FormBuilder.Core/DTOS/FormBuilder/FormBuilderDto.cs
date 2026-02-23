using System;
using System.Collections.Generic;
using FormBuilder.Core.DTOS.FormTabs;

namespace FormBuilder.Core.DTOS.FormBuilder
{
    public class FormBuilderDto
    {
        public int Id { get; set; }
        public string FormName { get; set; }
        public string? ForeignFormName { get; set; }
        public string FormCode { get; set; }
        public string Description { get; set; }
        public string? ForeignDescription { get; set; }
        public int Version { get; set; }
        public bool IsPublished { get; set; }
        public bool? IsSapEnabled { get; set; }
        public string? SapExecutionMode { get; set; }
        public bool IsActive { get; set; }

        // Auditing Fields
        public string CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Public view: tabs & fields (for anonymous access)
        public List<FormTabDto> Tabs { get; set; } = new List<FormTabDto>();
    }
}
