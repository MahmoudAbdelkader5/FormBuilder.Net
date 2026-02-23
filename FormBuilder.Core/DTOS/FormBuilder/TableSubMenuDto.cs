using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.DTOS.FormBuilder
{
    public class TableSubMenuDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ForeignName { get; set; }
        public int MenuId { get; set; }
        public string? MenuName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedByUserId { get; set; }

        // Navigation properties
        public List<TableMenuDocumentDto>? MenuDocuments { get; set; }
    }

    public class CreateTableSubMenuDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? ForeignName { get; set; }

        [Required]
        public int MenuId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpdateTableSubMenuDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? ForeignName { get; set; }

        public bool IsActive { get; set; } = true;
    }
}

