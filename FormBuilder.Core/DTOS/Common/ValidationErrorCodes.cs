namespace FormBuilder.Core.DTOS.Common
{
    /// <summary>
    /// Centralized validation error codes for the entire application
    /// </summary>
    public static class ValidationErrorCodes
    {
        // Common validation errors
        public const string Required = "VALIDATION_REQUIRED";
        public const string InvalidFormat = "VALIDATION_INVALID_FORMAT";
        public const string InvalidLength = "VALIDATION_INVALID_LENGTH";
        public const string InvalidRange = "VALIDATION_INVALID_RANGE";
        public const string InvalidValue = "VALIDATION_INVALID_VALUE";
        public const string DuplicateValue = "VALIDATION_DUPLICATE_VALUE";
        public const string NotFound = "VALIDATION_NOT_FOUND";
        public const string AlreadyExists = "VALIDATION_ALREADY_EXISTS";
        public const string InvalidReference = "VALIDATION_INVALID_REFERENCE";
        public const string CircularReference = "VALIDATION_CIRCULAR_REFERENCE";

        // FormBuilder specific
        public const string FormCodeExists = "FORMBUILDER_CODE_EXISTS";
        public const string FormCodeRequired = "FORMBUILDER_CODE_REQUIRED";
        public const string FormNotFound = "FORMBUILDER_NOT_FOUND";

        // FormField specific
        public const string FieldCodeExists = "FIELDFIELD_CODE_EXISTS";
        public const string FieldCodeRequired = "FIELDFIELD_CODE_REQUIRED";
        public const string FieldNameExists = "FIELDFIELD_NAME_EXISTS";
        public const string FieldNotFound = "FIELDFIELD_NOT_FOUND";
        public const string FieldExpressionInvalid = "FIELDFIELD_EXPRESSION_INVALID";
        public const string FieldExpressionRequired = "FIELDFIELD_EXPRESSION_REQUIRED";
        public const string FieldCircularReference = "FIELDFIELD_CIRCULAR_REFERENCE";

        // FieldType specific
        public const string FieldTypeNotFound = "FIELDTYPE_NOT_FOUND";
        public const string FieldTypeNameExists = "FIELDTYPE_NAME_EXISTS";
        public const string FieldTypeNameRequired = "FIELDTYPE_NAME_REQUIRED";
        public const string FieldTypeDataTypeRequired = "FIELDTYPE_DATATYPE_REQUIRED";
        public const string FieldTypeInUse = "FIELDTYPE_IN_USE";
        public const string FieldTypeInvalidDataType = "FIELDTYPE_INVALID_DATATYPE";

        // FormTab specific
        public const string TabNameExists = "TAB_NAME_EXISTS";
        public const string TabNotFound = "TAB_NOT_FOUND";

        // DocumentType specific
        public const string DocumentTypeCodeExists = "DOCUMENTTYPE_CODE_EXISTS";
        public const string DocumentTypeNotFound = "DOCUMENTTYPE_NOT_FOUND";

        // DocumentSeries specific
        public const string SeriesCodeExists = "SERIES_CODE_EXISTS";
        public const string SeriesNotFound = "SERIES_NOT_FOUND";
        public const string SeriesInUse = "SERIES_IN_USE";

        // Project specific
        public const string ProjectCodeExists = "PROJECT_CODE_EXISTS";
        public const string ProjectNotFound = "PROJECT_NOT_FOUND";

        // FormSubmission specific
        public const string SubmissionNotFound = "SUBMISSION_NOT_FOUND";
        public const string SubmissionInvalidStatus = "SUBMISSION_INVALID_STATUS";
        public const string DocumentNumberExists = "SUBMISSION_DOCUMENT_NUMBER_EXISTS";
        public const string DocumentNumberRequired = "SUBMISSION_DOCUMENT_NUMBER_REQUIRED";

        // Grid specific
        public const string GridNotFound = "GRID_NOT_FOUND";
        public const string GridColumnNotFound = "GRID_COLUMN_NOT_FOUND";

        // Attachment specific
        public const string AttachmentNotFound = "ATTACHMENT_NOT_FOUND";
        public const string FileSizeExceeded = "ATTACHMENT_FILE_SIZE_EXCEEDED";
        public const string InvalidFileType = "ATTACHMENT_INVALID_FILE_TYPE";

        // Workflow specific
        public const string WorkflowNotFound = "WORKFLOW_NOT_FOUND";
        public const string StageNotFound = "STAGE_NOT_FOUND";
        public const string InvalidWorkflowState = "WORKFLOW_INVALID_STATE";
    }
}

