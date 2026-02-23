using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace FormBuilder.Core.DTOS.FormBuilder
{
    // ================================
    // MAIN SUBMISSION DTOs
    // ================================

    public class FormSubmissionDto
    {
        public int Id { get; set; }
        public int FormBuilderId { get; set; }
        public string? FormName { get; set; }
        public int Version { get; set; }
        public int DocumentTypeId { get; set; }
        public string? DocumentTypeName { get; set; }
        public int SeriesId { get; set; }
        public string? SeriesCode { get; set; }
        public string? DocumentNumber { get; set; }
        public string? SubmittedByUserId { get; set; }
        public string? SubmittedByUserName { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int? StageId { get; set; }  // مرحلة الموافقة الحالية
        public string SignatureStatus { get; set; } = "not_required";
        public string? DocuSignEnvelopeId { get; set; }
        public DateTime? SignedAt { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }

    public class FormSubmissionDetailDto : FormSubmissionDto
    {
        public List<FormSubmissionValueDto> FieldValues { get; set; } = new();
        public List<FormSubmissionAttachmentDto> Attachments { get; set; } = new();
        public List<FormSubmissionGridDto> GridData { get; set; } = new();
    }

    public class CreateFormSubmissionDto
    {
        [Required]
        public int FormBuilderId { get; set; }

        [Required]
        public int DocumentTypeId { get; set; }

        [Required]
        public int SeriesId { get; set; }

        // SubmittedByUserId is optional - can be provided via query parameter or will default to "public-user" in service
        public string? SubmittedByUserId { get; set; }

        public string Status { get; set; } = "Draft";
    }

    public class UpdateFormSubmissionDto
    {
        public string? DocumentNumber { get; set; }
        public string? Status { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public int? StageId { get; set; }  // مرحلة الموافقة الحالية
    }

    // ================================
    // FIELD VALUES DTOs (FORM_SUBMISSION_VALUES)
    // ================================

    public class FormSubmissionValueDto
    {
        public int Id { get; set; }
        public int SubmissionId { get; set; }
        public int FieldId { get; set; }
        public string? FieldCode { get; set; }
        public string? FieldName { get; set; }
        public string? ValueString { get; set; }
        public decimal? ValueNumber { get; set; }
        public DateTime? ValueDate { get; set; }
        public bool? ValueBool { get; set; }
        public string? ValueJson { get; set; }
    }

    public class SaveFormSubmissionValueDto
    {
        [Required]
        public int FieldId { get; set; }

        [Required]
        public string FieldCode { get; set; } = string.Empty;

        public string? ValueString { get; set; }
        public decimal? ValueNumber { get; set; }
        public DateTime? ValueDate { get; set; }
        public bool? ValueBool { get; set; }
        
        // Accept JsonElement to handle arrays and complex objects from frontend
        // Will be converted to string in the service layer
        // Using default(JsonElement) for null values (JsonElement is a struct)
        public JsonElement ValueJson { get; set; }
        
        // Helper property to get ValueJson as string (for backward compatibility)
        public string? ValueJsonString 
        { 
            get 
            {
                if (ValueJson.ValueKind == JsonValueKind.Null || ValueJson.ValueKind == JsonValueKind.Undefined)
                    return null;
                    
                return ValueJson.GetRawText();
            }
        }
    }

    public class BulkSaveFieldValuesDto
    {
        [Required]
        public int SubmissionId { get; set; }

        public List<SaveFormSubmissionValueDto> FieldValues { get; set; } = new();
    }

    // ================================
    // ATTACHMENTS DTOs (FORM_SUBMISSION_ATTACHMENTS)
    // ================================


    public class SaveFormSubmissionAttachmentDto
    {
        [Required]
        public int FieldId { get; set; }

        [Required]
        public string FieldCode { get; set; } = string.Empty;

        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public string FilePath { get; set; } = string.Empty;

        [Required]
        public long FileSize { get; set; }

        [Required]
        public string ContentType { get; set; } = string.Empty;
    }

    public class UploadAttachmentDto
    {
        [Required]
        public int SubmissionId { get; set; }

        [Required]
        public int FieldId { get; set; }

        [Required]
        public string FieldCode { get; set; } = string.Empty;
    }

    // ================================
    // GRID DATA DTOs (FORM_SUBMISSION_GRID_ROWS & FORM_SUBMISSION_GRID_CELLS)
    // ================================

    public class FormSubmissionGridDto
    {
        public int Id { get; set; }
        public int SubmissionId { get; set; }
        public int GridId { get; set; }
        public string GridName { get; set; } = string.Empty;
        public string GridCode { get; set; } = string.Empty;
        public int RowIndex { get; set; }
        public List<FormSubmissionGridCellDto> Cells { get; set; } = new();
    }

    public class FormSubmissionGridCellDto
    {
        public int Id { get; set; }
        public int RowId { get; set; }
        public int ColumnId { get; set; }
        public string ColumnName { get; set; } = string.Empty;
        public string ColumnCode { get; set; } = string.Empty;
        public string? ValueString { get; set; }
        public decimal? ValueNumber { get; set; }
        public DateTime? ValueDate { get; set; }
        public bool? ValueBool { get; set; }
        public string? ValueJson { get; set; }
    }

    public class SaveFormSubmissionGridDto
    {
        [Required]
        public int GridId { get; set; }

        [Required]
        public int RowIndex { get; set; }

        public List<SaveFormSubmissionGridCellDto> Cells { get; set; } = new();
    }

    public class SaveFormSubmissionGridCellDto
    {
        [Required]
        public int ColumnId { get; set; }

        [Required]
        public string ColumnCode { get; set; } = string.Empty;

        public string? ValueString { get; set; }
        public decimal? ValueNumber { get; set; }
        public DateTime? ValueDate { get; set; }
        public bool? ValueBool { get; set; }
        public string? ValueJson { get; set; }
    }

    public class BulkSaveGridDataDto
    {
        [Required]
        public int SubmissionId { get; set; }

        [Required]
        public int GridId { get; set; }

        public List<SaveFormSubmissionGridDto> Rows { get; set; } = new();
    }

    // ================================
    // OPERATION DTOs
    // ================================

    public class SubmitFormDto
    {
        [Required]
        public int SubmissionId { get; set; }

        // SubmittedByUserId is optional - can be provided via query parameter or extracted from auth context
        public string? SubmittedByUserId { get; set; }
    }

    public class SubmitFormResponseDto
    {
        public bool Submitted { get; set; }
        public bool SignatureRequired { get; set; }
        public string SignatureStatus { get; set; } = "not_required"; // not_required | pending | signed
        public string? SigningUrl { get; set; }
    }

    public class ChangeStatusDto
    {
        [Required]
        public int SubmissionId { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;

        public string? Comments { get; set; }
    }

    public class SaveFormSubmissionDataDto
    {
        [Required]
        public int SubmissionId { get; set; }

        public List<SaveFormSubmissionValueDto> FieldValues { get; set; } = new();
        public List<SaveFormSubmissionAttachmentDto> Attachments { get; set; } = new();
        public List<SaveFormSubmissionGridDto> GridData { get; set; } = new();
    }

    /// <summary>
    /// DTO for creating a new submission and saving all its data in one operation
    /// Used for public forms to combine Create + Save operations
    /// </summary>
    public class CreateAndSaveFormSubmissionDto
    {
        [Required]
        public int FormBuilderId { get; set; }

        [Required]
        public int DocumentTypeId { get; set; }

        [Required]
        public int SeriesId { get; set; }

        [Required]
        public string SubmittedByUserId { get; set; } = string.Empty;

        public List<SaveFormSubmissionValueDto> FieldValues { get; set; } = new();
        public List<SaveFormSubmissionAttachmentDto> Attachments { get; set; } = new();
        public List<SaveFormSubmissionGridDto> GridData { get; set; } = new();
    }

    /// <summary>
    /// DTO for creating a draft submission
    /// Accepts formBuilderId, projectId (optional - can be derived from seriesId), seriesId (optional), and submittedByUserId
    /// </summary>
    public class CreateDraftDto
    {
        [Required]
        public int FormBuilderId { get; set; }

        public int? ProjectId { get; set; }

        public int? SeriesId { get; set; }

        public string? SubmittedByUserId { get; set; }

        // Optional fields that may be sent by client but are not used by the service
        public int? DocumentTypeId { get; set; }
        public int? Version { get; set; }
    }

    // ================================
    // FILTER & SEARCH DTOs
    // ================================

    public class FormSubmissionFilterDto
    {
        public int? FormBuilderId { get; set; }
        public int? DocumentTypeId { get; set; }
        public string? Status { get; set; }
        public string? SubmittedByUserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? DocumentNumber { get; set; }
        public string? SearchText { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class FormSubmissionSearchResultDto
    {
        public List<FormSubmissionDto> Submissions { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

    // ================================
    // STATISTICS & REPORTING DTOs
    // ================================

    public class FormSubmissionStatisticsDto
    {
        public int TotalSubmissions { get; set; }
        public int DraftCount { get; set; }
        public int SubmittedCount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public int PendingApprovalCount { get; set; }
        public Dictionary<string, int> SubmissionsByDocumentType { get; set; } = new();
        public Dictionary<string, int> SubmissionsByStatus { get; set; } = new();
        public Dictionary<string, int> SubmissionsByMonth { get; set; } = new();
    }

    public class SubmissionTrendDto
    {
        public string Period { get; set; } = string.Empty; // "2024-01", "2024-02", etc.
        public int Count { get; set; }
    }

    public class UserSubmissionStatsDto
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int TotalSubmissions { get; set; }
        public int DraftCount { get; set; }
        public int SubmittedCount { get; set; }
        public int ApprovedCount { get; set; }
    }

    // ================================
    // VALIDATION & WORKFLOW DTOs
    // ================================

    public class SubmissionValidationResultDto
    {
        public bool IsValid { get; set; }
        public List<ValidationErrorDto> Errors { get; set; } = new();
        public List<ValidationWarningDto> Warnings { get; set; } = new();
    }

    public class ValidationErrorDto
    {
        public string FieldCode { get; set; } = string.Empty;
        public string FieldName { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string ErrorType { get; set; } = string.Empty; // Required, Format, Range, etc.
    }

    public class ValidationWarningDto
    {
        public string FieldCode { get; set; } = string.Empty;
        public string FieldName { get; set; } = string.Empty;
        public string WarningMessage { get; set; } = string.Empty;
    }

    public class WorkflowActionDto
    {
        [Required]
        public int SubmissionId { get; set; }

        [Required]
        public string Action { get; set; } = string.Empty; // Approve, Reject, Return, Cancel

        public string? Comments { get; set; }
        public string? PerformedByUserId { get; set; }
    }

    // ================================
    // EXPORT & IMPORT DTOs
    // ================================

    public class ExportSubmissionsDto
    {
        public FormSubmissionFilterDto? Filter { get; set; }
        public string ExportFormat { get; set; } = string.Empty; // Excel, PDF, CSV
        public List<string> IncludeFields { get; set; } = new();
    }

    public class ImportSubmissionDto
    {
        [Required]
        public int FormBuilderId { get; set; }

        [Required]
        public string FileContent { get; set; } = string.Empty; // Base64 encoded file

        [Required]
        public string FileName { get; set; } = string.Empty;

        public string? ImportedByUserId { get; set; }
    }

    public class ImportResultDto
    {
        public bool Success { get; set; }
        public int ImportedCount { get; set; }
        public int FailedCount { get; set; }
        public List<ImportErrorDto> Errors { get; set; } = new();
        public string? Message { get; set; }
    }

    public class ImportErrorDto
    {
        public int RowNumber { get; set; }
        public string FieldName { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }

    // ================================
    // HISTORY & AUDIT DTOs
    // ================================

    public class SubmissionHistoryDto
    {
        public int Id { get; set; }
        public int SubmissionId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PerformedByUserId { get; set; } = string.Empty;
        public string PerformedByUserName { get; set; } = string.Empty;
        public DateTime PerformedDate { get; set; }
        public string OldValues { get; set; } = string.Empty;
        public string NewValues { get; set; } = string.Empty;
    }

    // ================================
    // DASHBOARD & SUMMARY DTOs
    // ================================

    public class SubmissionSummaryDto
    {
        public int TodayCount { get; set; }
        public int ThisWeekCount { get; set; }
        public int ThisMonthCount { get; set; }
        public int PendingApprovalCount { get; set; }
        public int MyDraftCount { get; set; }
        public List<RecentSubmissionDto> RecentSubmissions { get; set; } = new();
    }

    public class RecentSubmissionDto
    {
        public int Id { get; set; }
        public string? DocumentNumber { get; set; }
        public string? FormName { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string? SubmittedByUserName { get; set; }
    }
}
