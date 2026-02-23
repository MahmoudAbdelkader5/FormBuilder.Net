using Microsoft.AspNetCore.Mvc.ModelBinding;
using FormBuilder.Core.DTOS.Common;

namespace FormBuilder.API.Extensions
{
    /// <summary>
    /// Extension methods for ModelStateDictionary to convert to ValidationErrorCollection
    /// </summary>
    public static class ModelStateExtensions
    {
        /// <summary>
        /// Converts ModelState errors to ValidationErrorCollection
        /// </summary>
        public static ValidationErrorCollection ToValidationErrors(this ModelStateDictionary modelState)
        {
            var errors = new ValidationErrorCollection();

            foreach (var modelStateEntry in modelState)
            {
                var fieldName = modelStateEntry.Key;
                var fieldErrors = modelStateEntry.Value.Errors;

                foreach (var error in fieldErrors)
                {
                    var errorCode = DetermineErrorCode(error);
                    var errorType = DetermineErrorType(error);

                    errors.AddError(
                        fieldName,
                        errorCode,
                        error.ErrorMessage ?? $"Validation error for field '{fieldName}'.",
                        errorType
                    );
                }
            }

            return errors;
        }

        private static string DetermineErrorCode(ModelError error)
        {
            if (error.ErrorMessage?.Contains("required") == true || 
                error.ErrorMessage?.Contains("مطلوب") == true)
            {
                return ValidationErrorCodes.Required;
            }

            if (error.ErrorMessage?.Contains("format") == true ||
                error.ErrorMessage?.Contains("تنسيق") == true)
            {
                return ValidationErrorCodes.InvalidFormat;
            }

            if (error.ErrorMessage?.Contains("length") == true ||
                error.ErrorMessage?.Contains("طول") == true)
            {
                return ValidationErrorCodes.InvalidLength;
            }

            if (error.ErrorMessage?.Contains("range") == true ||
                error.ErrorMessage?.Contains("نطاق") == true)
            {
                return ValidationErrorCodes.InvalidRange;
            }

            return ValidationErrorCodes.InvalidValue;
        }

        private static string DetermineErrorType(ModelError error)
        {
            if (error.ErrorMessage?.Contains("required") == true)
                return "Required";

            if (error.ErrorMessage?.Contains("format") == true)
                return "Format";

            if (error.ErrorMessage?.Contains("length") == true)
                return "Length";

            if (error.ErrorMessage?.Contains("range") == true)
                return "Range";

            return "Validation";
        }
    }
}

