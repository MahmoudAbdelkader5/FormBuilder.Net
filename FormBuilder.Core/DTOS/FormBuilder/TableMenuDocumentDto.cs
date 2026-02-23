using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.DTOS.FormBuilder
{
    public class TableMenuDocumentDto
    {
        public int Id { get; set; }
        public int DocumentTypeId { get; set; }
        public string? DocumentTypeName { get; set; }
        public string? DocumentTypeCode { get; set; }
        public int? MenuId { get; set; }
        public string? MenuName { get; set; }
        public int? SubMenuId { get; set; }
        public string? SubMenuName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedByUserId { get; set; }
    }

    public class CreateTableMenuDocumentDto
    {
        [Required]
        public int DocumentTypeId { get; set; }

        public int? MenuId { get; set; } // Required if SubMenuId is null

        public int? SubMenuId { get; set; } // Required if MenuId is null

        public bool IsActive { get; set; } = true;
    }

    public class UpdateTableMenuDocumentDto
    {
        public int? MenuId { get; set; }

        public int? SubMenuId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    // DTO for dashboard view (includes nested structure)
    public class DashboardMenuDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ForeignName { get; set; }
        public string MenuCode { get; set; } = string.Empty;
        public List<DashboardSubMenuDto>? SubMenus { get; set; }
        public List<DashboardDocumentDto>? Documents { get; set; } // Direct documents (not in sub menu)
    }

    public class DashboardSubMenuDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ForeignName { get; set; }
        public List<DashboardDocumentDto>? Documents { get; set; }
    }

    public class DashboardDocumentDto
    {
        public int Id { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; } = string.Empty;
        public string? DocumentTypeCode { get; set; }
    }
}

