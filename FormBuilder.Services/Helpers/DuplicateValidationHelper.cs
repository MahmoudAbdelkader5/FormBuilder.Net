using Microsoft.Extensions.Logging;
using System;

namespace FormBuilder.Services.Helpers
{
    /// <summary>
    /// Helper class for handling duplicate validation warnings and errors
    /// </summary>
    public static class DuplicateValidationHelper
    {
        /// <summary>
        /// Logs a warning when a duplicate value is detected
        /// </summary>
        public static void LogDuplicateWarning(ILogger logger, string entityType, string fieldName, string fieldValue, string scope = null)
        {
            var scopeMessage = string.IsNullOrWhiteSpace(scope) ? "" : $" in {scope}";
            var warningMessage = $"⚠️ Duplicate {fieldName} detected: '{fieldValue}' for {entityType}{scopeMessage}";
            
            logger?.LogWarning(warningMessage);
            Console.WriteLine($"[DUPLICATE WARNING] {warningMessage}");
        }

        /// <summary>
        /// Creates a formatted error message for duplicate values
        /// </summary>
        public static string FormatDuplicateErrorMessage(string entityType, string fieldName, string fieldValue, string scope = null)
        {
            var scopeMessage = string.IsNullOrWhiteSpace(scope) ? "" : $" {scope}";
            return $"{entityType} {fieldName} '{fieldValue}' is already in use{scopeMessage}. Please choose a different {fieldName}.";
        }

        /// <summary>
        /// Creates a formatted warning message for duplicate values
        /// </summary>
        public static string FormatDuplicateWarningMessage(string entityType, string fieldName, string fieldValue, string scope = null)
        {
            var scopeMessage = string.IsNullOrWhiteSpace(scope) ? "" : $" in {scope}";
            return $"⚠️ Warning: {fieldName} '{fieldValue}' already exists for {entityType}{scopeMessage}. This may cause conflicts.";
        }

        /// <summary>
        /// Logs duplicate detection with full context
        /// </summary>
        public static void LogDuplicateDetection(ILogger logger, string entityType, string fieldName, string fieldValue, 
            int? existingId = null, string scope = null, bool isDeleted = false)
        {
            var scopeMessage = string.IsNullOrWhiteSpace(scope) ? "" : $" in {scope}";
            var deletedMessage = isDeleted ? " (soft-deleted)" : "";
            var idMessage = existingId.HasValue ? $" (ID: {existingId.Value})" : "";
            
            var logMessage = $"🔍 Duplicate {fieldName} detected: '{fieldValue}' for {entityType}{scopeMessage}{idMessage}{deletedMessage}";
            
            logger?.LogWarning(logMessage);
            Console.WriteLine($"[DUPLICATE DETECTION] {logMessage}");
        }
    }
}

