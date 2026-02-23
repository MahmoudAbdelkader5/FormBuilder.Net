using formBuilder.Domian.Entitys;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormBuilder.Domian.Entitys.FormBuilder
{
    [Table("TABLE_SUB_MENU_PERMISSIONS")]
    public class TABLE_SUB_MENU_PERMISSIONS : BaseEntity
    {
        [Required]
        [ForeignKey("TABLE_SUB_MENUS")]
        [Column("SubMenuId")]
        public int SubMenuId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("PermissionName")]
        public string PermissionName { get; set; } = string.Empty; // Role name or permission identifier

        [Column("IsActive")]
        public new bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual TABLE_SUB_MENUS SubMenu { get; set; } = null!;
    }
}

