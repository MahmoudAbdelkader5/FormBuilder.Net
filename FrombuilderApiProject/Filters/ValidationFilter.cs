using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using FormBuilder.Core.DTOS.Common;
using FormBuilder.API.Models;
using System.Linq;
using System;
using System.Reflection;

namespace FormBuilder.API.Filters
{
    /// <summary>
    /// Action filter that automatically validates ModelState and returns formatted validation errors
    /// </summary>
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var httpMethod = context.HttpContext.Request.Method;
            
            // Skip validation for DELETE and GET requests (they typically don't have request body)
            if (httpMethod == "DELETE" || httpMethod == "GET")
            {
                return;
            }

            // IMPORTANT: Remove errors for ignored fields FIRST, before any other processing
            // This ensures these errors are removed before any validation checks
            var ignoredFields = new[] { "RoleId", "UserId", "SubmittedByUserId" };
            foreach (var fieldName in ignoredFields)
            {
                // Remove ALL keys that contain this field name (case-insensitive)
                var keysToRemove = context.ModelState.Keys
                    .Where(k => k.IndexOf(fieldName, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
                
                foreach (var keyToRemove in keysToRemove)
                {
                    context.ModelState.Remove(keyToRemove);
                }
            }

            // Check if request has body content
            var hasBody = context.HttpContext.Request.ContentLength.HasValue && context.HttpContext.Request.ContentLength.Value > 0;
            
            // Check if request has query parameters (alternative to body)
            var hasQueryParams = context.HttpContext.Request.Query.Count > 0;

            // If no body content but has query parameters, remove ModelState errors for [FromBody] parameters
            // This allows endpoints to accept query parameters as alternative to body
            // Also handle case where body exists but is missing fields that are in query parameters
            if (hasQueryParams)
            {
                var query = context.HttpContext.Request.Query;
                var queryKeys = query.Keys.ToList();
                
                var actionDescriptor = context.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
                if (actionDescriptor != null)
                {
                    var parameters = actionDescriptor.MethodInfo.GetParameters();
                    foreach (var param in parameters)
                    {
                        if (param.GetCustomAttributes(typeof(Microsoft.AspNetCore.Mvc.FromBodyAttribute), false).Any())
                        {
                            var paramName = param.Name;
                            
                            // If no body, remove all errors for this [FromBody] parameter
                            if (!hasBody)
                            {
                                if (context.ModelState.ContainsKey(paramName))
                                {
                                    context.ModelState.Remove(paramName);
                                }
                                
                                // Remove all nested property errors
                                var keysToRemove = context.ModelState.Keys
                                    .Where(k => k.StartsWith(paramName + ".", StringComparison.OrdinalIgnoreCase) ||
                                               k.StartsWith(paramName + "[", StringComparison.OrdinalIgnoreCase))
                                    .ToList();
                                
                                foreach (var keyToRemove in keysToRemove)
                                {
                                    context.ModelState.Remove(keyToRemove);
                                }
                            }
                            
                            // If body exists, remove errors for fields that are available in query parameters
                            // This allows partial body with query parameters to fill in missing fields
                            if (param.ParameterType != null && !param.ParameterType.IsPrimitive && param.ParameterType != typeof(string) && !param.ParameterType.IsValueType)
                            {
                                var properties = param.ParameterType.GetProperties();
                                
                                foreach (var prop in properties)
                                {
                                    // Check if this property is in query parameters (case-insensitive)
                                    var propName = prop.Name;
                                    var propNameCamel = propName.Length > 1 
                                        ? char.ToLowerInvariant(propName[0]) + propName.Substring(1)
                                        : propName.ToLowerInvariant();
                                    
                                    var isInQuery = queryKeys.Any(qk => 
                                        string.Equals(qk, propName, StringComparison.OrdinalIgnoreCase) ||
                                        string.Equals(qk, propNameCamel, StringComparison.OrdinalIgnoreCase));
                                    
                                    if (isInQuery)
                                    {
                                        // Remove ModelState errors for ALL keys that contain this property name (case-insensitive)
                                        // This handles: "SubmittedByUserId", "createDraftDto.SubmittedByUserId", etc.
                                        var keysToRemove = context.ModelState.Keys
                                            .Where(k => k.IndexOf(propName, StringComparison.OrdinalIgnoreCase) >= 0)
                                            .ToList();
                                        
                                        foreach (var keyToRemove in keysToRemove)
                                        {
                                            context.ModelState.Remove(keyToRemove);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // If query parameters are present, also ignore fields that can be provided via query params
            // This handles cases where body is partial and missing fields are in query parameters
            if (hasQueryParams)
            {
                var query = context.HttpContext.Request.Query;
                var queryKeys = query.Keys.Select(k => k.ToLowerInvariant()).ToList();
                
                // Check all ModelState keys and remove errors for fields that are in query parameters
                var modelStateKeys = context.ModelState.Keys.ToList();
                foreach (var modelStateKey in modelStateKeys)
                {
                    // Extract the property name from the ModelState key
                    // Handle formats like: "SubmittedByUserId", "createDraftDto.SubmittedByUserId", etc.
                    var propertyName = modelStateKey;
                    if (modelStateKey.Contains("."))
                    {
                        propertyName = modelStateKey.Split('.').Last();
                    }
                    else if (modelStateKey.Contains("["))
                    {
                        var start = modelStateKey.IndexOf("[");
                        var end = modelStateKey.IndexOf("]");
                        if (start >= 0 && end > start)
                        {
                            propertyName = modelStateKey.Substring(start + 1, end - start - 1);
                        }
                    }
                    
                    // Check if this property (or its camelCase version) is in query parameters
                    var propertyNameLower = propertyName.ToLowerInvariant();
                    var propertyNameCamel = propertyName.Length > 1 
                        ? char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1).ToLowerInvariant()
                        : propertyNameLower;
                    
                    if (queryKeys.Contains(propertyNameLower) || queryKeys.Contains(propertyNameCamel))
                    {
                        context.ModelState.Remove(modelStateKey);
                    }
                }
            }

            // Only validate ModelState for POST, PUT, PATCH requests
            // After removing ignored fields
            if (!context.ModelState.IsValid)
            {
                var errors = new ValidationErrorCollection();
                
                foreach (var modelStateEntry in context.ModelState)
                {
                    var fieldName = modelStateEntry.Key;
                    var fieldErrors = modelStateEntry.Value.Errors;

                    foreach (var error in fieldErrors)
                    {
                        var errorCode = DetermineErrorCode(error, modelStateEntry.Value);
                        var errorType = DetermineErrorType(error, modelStateEntry.Value);
                        
                        errors.AddError(
                            fieldName,
                            errorCode,
                            error.ErrorMessage ?? GetDefaultErrorMessage(fieldName, errorCode),
                            errorType
                        );
                    }
                }

                // Only return error if there are errors
                if (errors.HasErrors)
                {
                    var response = new ApiResponse(400, "Validation failed", new
                    {
                        errors = errors.Errors,
                        message = "One or more validation errors occurred.",
                        errorCount = errors.ErrorCount
                    });

                    context.Result = new BadRequestObjectResult(response);
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No action needed after execution
        }

        private string DetermineErrorCode(ModelError error, ModelStateEntry entry)
        {
            // Try to extract error code from error message or use default
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

        private string DetermineErrorType(ModelError error, ModelStateEntry entry)
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

        private string GetDefaultErrorMessage(string fieldName, string errorCode)
        {
            return errorCode switch
            {
                ValidationErrorCodes.Required => $"The field '{fieldName}' is required.",
                ValidationErrorCodes.InvalidFormat => $"The field '{fieldName}' has an invalid format.",
                ValidationErrorCodes.InvalidLength => $"The field '{fieldName}' has an invalid length.",
                ValidationErrorCodes.InvalidRange => $"The field '{fieldName}' is out of range.",
                _ => $"Validation error for field '{fieldName}'."
            };
        }
    }
}

