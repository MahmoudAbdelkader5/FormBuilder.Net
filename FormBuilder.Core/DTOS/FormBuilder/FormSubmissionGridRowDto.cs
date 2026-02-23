using System;
using System.ComponentModel.DataAnnotations;

namespace FormBuilder.API.DTOs
{
    public class FormSubmissionGridRowDto
    {
        public int Id { get; set; }
        public int SubmissionId { get; set; }
        public string SubmissionNumber { get; set; }
        public int GridId { get; set; }
        public string GridName { get; set; }
        public int RowIndex { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class CreateFormSubmissionGridRowDto
    {
        [Required(ErrorMessage = "SubmissionId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "SubmissionId must be greater than 0")]
        public int SubmissionId { get; set; }

        [Required(ErrorMessage = "GridId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "GridId must be greater than 0")]
        public int GridId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "RowIndex must be greater than or equal to 0")]
        public int? RowIndex { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpdateFormSubmissionGridRowDto
    {
        /// <summary>
        /// SubmissionId cannot be changed after creation. This property is ignored during updates.
        /// </summary>
        public int? SubmissionId { get; set; }

        /// <summary>
        /// GridId cannot be changed after creation. This property is ignored during updates.
        /// </summary>
        public int? GridId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "RowIndex must be greater than or equal to 0")]
        public int? RowIndex { get; set; }
        
        public bool? IsActive { get; set; }
    }

    public class FormSubmissionGridRowWithCellsDto : FormSubmissionGridRowDto
    {
        public List<FormSubmissionGridCellDto> Cells { get; set; } = new();
    }

    public class GridValidationResultDto
    {
        public bool IsValid { get; set; }
        public List<GridValidationErrorDto> Errors { get; set; } = new();
        public List<GridValidationWarningDto> Warnings { get; set; } = new();
    }

    public class GridValidationErrorDto
    {
        public string Field { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int? RowIndex { get; set; }
        public int? ColumnId { get; set; }
    }

    public class GridValidationWarningDto
    {
        public string Field { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int? RowIndex { get; set; }
    }
}