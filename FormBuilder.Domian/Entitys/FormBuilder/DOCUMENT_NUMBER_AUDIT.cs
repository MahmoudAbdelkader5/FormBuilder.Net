using formBuilder.Domian.Entitys;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormBuilder.Domian.Entitys.FormBuilder
{
    [Table("DOCUMENT_NUMBER_AUDIT")]
    public class DOCUMENT_NUMBER_AUDIT : BaseEntity
    {
        public int FormSubmissionId { get; set; }
        public int SeriesId { get; set; }
        
        [Required, StringLength(100)]
        public string GeneratedNumber { get; set; }
        
        [Required, StringLength(500)]
        public string TemplateUsed { get; set; }
        
        public DateTime GeneratedAt { get; set; }
        
        [StringLength(20)]
        public string GeneratedOn { get; set; }  // Submit/Approval
        
        [StringLength(450)]
        public string GeneratedByUserId { get; set; }
    }
}
