using System.Collections.Generic;

namespace FormBuilder.Core.DTOS.Common
{
    /// <summary>
    /// Represents a single validation error for a specific field
    /// </summary>
    public class ValidationError
    {
        public string FieldName { get; set; } = string.Empty;
        public string FieldCode { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string ErrorType { get; set; } = string.Empty; // Required, Format, Range, Unique, etc.
        public object? AttemptedValue { get; set; }
        public Dictionary<string, object>? AdditionalData { get; set; }
    }

    /// <summary>
    /// Represents multiple validation errors
    /// </summary>
    public class ValidationErrorCollection
    {
        public List<ValidationError> Errors { get; set; } = new();
        public string? GeneralMessage { get; set; }
        public int ErrorCount => Errors.Count;

        public void AddError(string fieldName, string errorCode, string errorMessage, string errorType = "Validation")
        {
            Errors.Add(new ValidationError
            {
                FieldName = fieldName,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage,
                ErrorType = errorType
            });
        }

        public void AddError(ValidationError error)
        {
            Errors.Add(error);
        }

        public bool HasErrors => Errors.Count > 0;
    }

    /// <summary>
    /// Enhanced ValidationResult with support for multiple errors
    /// </summary>
    public class EnhancedValidationResult
    {
        public bool IsValid { get; }
        public ValidationErrorCollection Errors { get; }
        public string? GeneralMessage { get; }

        private EnhancedValidationResult(bool isValid, ValidationErrorCollection? errors = null, string? generalMessage = null)
        {
            IsValid = isValid;
            Errors = errors ?? new ValidationErrorCollection();
            GeneralMessage = generalMessage;
        }

        public static EnhancedValidationResult Success() => new EnhancedValidationResult(true);
        
        public static EnhancedValidationResult Failure(string message) 
        {
            var errors = new ValidationErrorCollection { GeneralMessage = message };
            return new EnhancedValidationResult(false, errors, message);
        }

        public static EnhancedValidationResult Failure(ValidationErrorCollection errors)
        {
            return new EnhancedValidationResult(false, errors, errors.GeneralMessage);
        }

        public static EnhancedValidationResult Failure(string fieldName, string errorCode, string errorMessage, string errorType = "Validation")
        {
            var errors = new ValidationErrorCollection();
            errors.AddError(fieldName, errorCode, errorMessage, errorType);
            return new EnhancedValidationResult(false, errors);
        }

        public static EnhancedValidationResult Failure(ValidationError error)
        {
            var errors = new ValidationErrorCollection();
            errors.AddError(error);
            return new EnhancedValidationResult(false, errors);
        }
    }
}

