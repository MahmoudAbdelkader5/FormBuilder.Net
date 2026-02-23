using formBuilder.Domian.Entitys;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormBuilder.Domian.Entitys.FormBuilder
{
    [Table("TABLE_MENU_DOCUMENT_PERMISSIONS")]
    public class TABLE_MENU_DOCUMENT_PERMISSIONS : BaseEntity
    {
        [Required]
        [ForeignKey("TABLE_MENU_DOCUMENTS")]
        [Column("MenuDocumentId")]
        public int MenuDocumentId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("PermissionName")]
        public string PermissionName { get; set; } = string.Empty; // Role name or permission identifier

        [Column("IsActive")]
        public new bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual TABLE_MENU_DOCUMENTS MenuDocument { get; set; } = null!;
    }
}

