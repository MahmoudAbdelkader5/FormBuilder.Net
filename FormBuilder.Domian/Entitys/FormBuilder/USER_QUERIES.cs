using formBuilder.Domian.Entitys;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormBuilder.Domian.Entitys.FormBuilder
{
    [Table("USER_QUERIES")]
    public class USER_QUERIES : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string QueryName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string DatabaseName { get; set; } = string.Empty;

        [Required]
        public string Query { get; set; } = string.Empty;

        [Required]
        [StringLength(450)]
        public string UserId { get; set; } = string.Empty;
    }
}

