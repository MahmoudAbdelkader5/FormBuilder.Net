namespace FormBuilder.API.Models.DTOs
{
    public class DocumentTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? FormBuilderId { get; set; }
        public string MenuCaption { get; set; }
        public int MenuOrder { get; set; }
        public int? ParentMenuId { get; set; }
        public bool IsActive { get; set; }

        // Approval workflow
        public int? ApprovalWorkflowId { get; set; }
        public string ApprovalWorkflowName { get; set; }

        // Navigation properties
        public string FormBuilderName { get; set; }
        public string ParentMenuName { get; set; }
    }

    public class CreateDocumentTypeDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int? FormBuilderId { get; set; }
        public string MenuCaption { get; set; }
        public int MenuOrder { get; set; } = 0;
        public int? ParentMenuId { get; set; }
        public bool IsActive { get; set; } = true;
        public int? ApprovalWorkflowId { get; set; }
    }

    public class UpdateDocumentTypeDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int? FormBuilderId { get; set; }
        public string MenuCaption { get; set; }
        public int? MenuOrder { get; set; }
        public int? ParentMenuId { get; set; }
        public bool? IsActive { get; set; }
        public int? ApprovalWorkflowId { get; set; }
    }

    
}