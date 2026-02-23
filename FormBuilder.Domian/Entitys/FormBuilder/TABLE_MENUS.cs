using formBuilder.Domian.Entitys;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormBuilder.Domian.Entitys.FormBuilder
{
    [Table("TABLE_MENUS")]
    public class TABLE_MENUS : BaseEntity
    {
        [Required]
        [StringLength(200)]
        [Column("Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        [Column("ForeignName")]
        public string? ForeignName { get; set; }

        [Required]
        [StringLength(100)]
        [Column("MenuCode")]
        public string MenuCode { get; set; } = string.Empty;

        [Required]
        [Column("IsActive")]
        public new bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<TABLE_SUB_MENUS> SubMenus { get; set; } = new List<TABLE_SUB_MENUS>();
        public virtual ICollection<TABLE_MENU_DOCUMENTS> MenuDocuments { get; set; } = new List<TABLE_MENU_DOCUMENTS>();
    }
}

