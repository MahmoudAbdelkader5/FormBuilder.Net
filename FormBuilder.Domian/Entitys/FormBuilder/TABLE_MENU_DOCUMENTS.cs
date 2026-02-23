using formBuilder.Domian.Entitys;
using FormBuilder.Domian.Entitys.FromBuilder;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormBuilder.Domian.Entitys.FormBuilder
{
    [Table("TABLE_MENU_DOCUMENTS")]
    public class TABLE_MENU_DOCUMENTS : BaseEntity
    {
        [Required]
        [ForeignKey("DOCUMENT_TYPES")]
        [Column("DocumentTypeId")]
        public int DocumentTypeId { get; set; }

        [ForeignKey("TABLE_MENUS")]
        [Column("MenuId")]
        public int? MenuId { get; set; }

        [ForeignKey("TABLE_SUB_MENUS")]
        [Column("SubMenuId")]
        public int? SubMenuId { get; set; }

        [Required]
        [Column("IsActive")]
        public new bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual DOCUMENT_TYPES DocumentType { get; set; } = null!;
        public virtual TABLE_MENUS? Menu { get; set; }
        public virtual TABLE_SUB_MENUS? SubMenu { get; set; }
    }
}

