using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.DTOS.FormBuilder
{
    public class TableMenuDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ForeignName { get; set; }
        public string MenuCode { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedByUserId { get; set; }

        // Navigation properties
        public List<TableSubMenuDto>? SubMenus { get; set; }
        public List<TableMenuDocumentDto>? MenuDocuments { get; set; }
    }

    public class CreateTableMenuDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? ForeignName { get; set; }

        [Required]
        [StringLength(100)]
        public string MenuCode { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }

    public class UpdateTableMenuDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? ForeignName { get; set; }

        public bool IsActive { get; set; } = true;
    }
}

