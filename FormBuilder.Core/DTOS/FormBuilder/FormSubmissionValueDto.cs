using System;
using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Core.DTOS.FormBuilder
{
   

    public class CreateFormSubmissionValueDto
    {
        [Required]
        public int SubmissionId { get; set; }

        [Required]
        public int FieldId { get; set; }

        [Required, StringLength(100)]
        public string FieldCode { get; set; }

        public string ValueString { get; set; }
        public decimal? ValueNumber { get; set; }
        public DateTime? ValueDate { get; set; }
        public bool? ValueBool { get; set; }
        public string ValueJson { get; set; }
    }

    public class UpdateFormSubmissionValueDto
    {
        public string ValueString { get; set; }
        public decimal? ValueNumber { get; set; }
        public DateTime? ValueDate { get; set; }
        public bool? ValueBool { get; set; }
        public string ValueJson { get; set; }
    }

    public class BulkFormSubmissionValuesDto
    {
        [Required]
        public int SubmissionId { get; set; }

        public List<CreateFormSubmissionValueDto> Values { get; set; } = new();
    }
}