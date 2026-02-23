using formBuilder.Domian.Entitys;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormBuilder.Domian.Entitys.FormBuilder
{
    /// <summary>
    /// Internal (in-app) notification entity.
    /// Stored in FormBuilder database.
    /// </summary>
    [Table("NOTIFICATIONS")]
    public class NOTIFICATIONS : BaseEntity
    {
        [Required, StringLength(450)]
        public string UserId { get; set; } = string.Empty; // username or numeric string (matches your system ids)

        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Visual severity (Info/Warning/Error/Success)
        /// </summary>
        [Required, StringLength(20)]
        public string Type { get; set; } = "Info";

        [StringLength(50)]
        public string ReferenceType { get; set; } = string.Empty; // e.g. ApprovalRequired

        public int? ReferenceId { get; set; } // e.g. SubmissionId

        public bool IsRead { get; set; } = false;

        public DateTime? ReadAt { get; set; }
    }
}


