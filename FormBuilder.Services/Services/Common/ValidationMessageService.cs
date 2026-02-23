using Microsoft.Extensions.Localization;
using FormBuilder.Core.DTOS.Common;
using System.Collections.Generic;

namespace FormBuilder.Services.Services.Common
{
    /// <summary>
    /// Centralized service for managing validation messages with localization support
    /// </summary>
    public class ValidationMessageService
    {
        private readonly IStringLocalizer<ValidationMessageService>? _localizer;

        public ValidationMessageService(IStringLocalizer<ValidationMessageService> localizer)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// Gets localized validation message by error code
        /// </summary>
        public string GetMessage(string errorCode, params object[] args)
        {
            if (_localizer != null)
            {
                try
                {
                    var message = _localizer[errorCode, args];
                    if (message != null && !message.ResourceNotFound)
                    {
                        return message.Value;
                    }
                }
                catch
                {
                    // If localization fails, fall back to default
                }
            }

            // Fallback to default messages
            return GetDefaultMessage(errorCode, args);
        }

        /// <summary>
        /// Creates a ValidationError with localized message
        /// </summary>
        public ValidationError CreateError(
            string fieldName,
            string errorCode,
            string errorType = "Validation",
            object? attemptedValue = null,
            params object[] messageArgs)
        {
            return new ValidationError
            {
                FieldName = fieldName,
                ErrorCode = errorCode,
                ErrorMessage = GetMessage(errorCode, messageArgs),
                ErrorType = errorType,
                AttemptedValue = attemptedValue
            };
        }

        /// <summary>
        /// Default messages when localization is not available
        /// </summary>
        private string GetDefaultMessage(string errorCode, params object[] args)
        {
            return errorCode switch
            {
                // Common
                ValidationErrorCodes.Required => $"The field '{args[0]}' is required.",
                ValidationErrorCodes.InvalidFormat => $"The field '{args[0]}' has an invalid format.",
                ValidationErrorCodes.InvalidLength => $"The field '{args[0]}' must be between {args[1]} and {args[2]} characters.",
                ValidationErrorCodes.InvalidRange => $"The field '{args[0]}' must be between {args[1]} and {args[2]}.",
                ValidationErrorCodes.DuplicateValue => $"The value '{args[0]}' already exists.",
                ValidationErrorCodes.NotFound => $"The resource was not found.",
                ValidationErrorCodes.AlreadyExists => $"The resource already exists.",
                ValidationErrorCodes.InvalidReference => $"Invalid reference: {args[0]}.",
                ValidationErrorCodes.CircularReference => $"Circular reference detected: {args[0]}.",

                // FormBuilder
                ValidationErrorCodes.FormCodeExists => $"Form code '{args[0]}' already exists.",
                ValidationErrorCodes.FormCodeRequired => "Form code is required.",
                ValidationErrorCodes.FormNotFound => "Form not found.",

                // FormField
                ValidationErrorCodes.FieldCodeExists => $"Field code '{args[0]}' already exists.",
                ValidationErrorCodes.FieldCodeRequired => "Field code is required.",
                ValidationErrorCodes.FieldNameExists => $"Field name '{args[0]}' already exists in this tab.",
                ValidationErrorCodes.FieldNotFound => "Field not found.",
                ValidationErrorCodes.FieldExpressionInvalid => "Invalid expression syntax.",
                ValidationErrorCodes.FieldExpressionRequired => "Expression is required for calculated fields.",
                ValidationErrorCodes.FieldCircularReference => $"Expression cannot reference the same field '{args[0]}'. This would create a circular reference.",

                // FormTab
                ValidationErrorCodes.TabNameExists => $"Tab name '{args[0]}' already exists.",
                ValidationErrorCodes.TabNotFound => "Tab not found.",

                // FieldType
                ValidationErrorCodes.FieldTypeNotFound => "Field type not found.",
                ValidationErrorCodes.FieldTypeNameExists => $"Field type name '{args[0]}' already exists.",
                ValidationErrorCodes.FieldTypeNameRequired => "Field type name is required.",
                ValidationErrorCodes.FieldTypeDataTypeRequired => "Data type is required.",
                ValidationErrorCodes.FieldTypeInUse => $"Cannot delete field type: It is used {args[0]} time(s).",
                ValidationErrorCodes.FieldTypeInvalidDataType => $"Invalid data type. Valid types are: {args[0]}.",

                // DocumentType
                ValidationErrorCodes.DocumentTypeCodeExists => $"Document type code '{args[0]}' already exists.",
                ValidationErrorCodes.DocumentTypeNotFound => "Document type not found.",

                // DocumentSeries
                ValidationErrorCodes.SeriesCodeExists => $"Series code '{args[0]}' already exists.",
                ValidationErrorCodes.SeriesNotFound => "Document series not found.",
                ValidationErrorCodes.SeriesInUse => "Cannot delete document series: There are form submissions associated with this series.",

                // Project
                ValidationErrorCodes.ProjectCodeExists => $"Project code '{args[0]}' already exists.",
                ValidationErrorCodes.ProjectNotFound => "Project not found.",

                // FormSubmission
                ValidationErrorCodes.SubmissionNotFound => "Form submission not found.",
                ValidationErrorCodes.SubmissionInvalidStatus => $"Form submission is already {args[0]}.",
                ValidationErrorCodes.DocumentNumberExists => $"Document number '{args[0]}' already exists.",
                ValidationErrorCodes.DocumentNumberRequired => "Document number is required.",

                // Grid
                ValidationErrorCodes.GridNotFound => "Grid not found.",
                ValidationErrorCodes.GridColumnNotFound => "Grid column not found.",

                // Attachment
                ValidationErrorCodes.AttachmentNotFound => "Attachment not found.",
                ValidationErrorCodes.FileSizeExceeded => $"File size exceeds the maximum allowed size of {args[0]}.",
                ValidationErrorCodes.InvalidFileType => $"Invalid file type. Allowed types: {args[0]}.",

                // Workflow
                ValidationErrorCodes.WorkflowNotFound => "Approval workflow not found.",
                ValidationErrorCodes.StageNotFound => "Approval stage not found.",
                ValidationErrorCodes.InvalidWorkflowState => "Invalid workflow state.",

                _ => $"Validation error: {errorCode}"
            };
        }
    }
}

