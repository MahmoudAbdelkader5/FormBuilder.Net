using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FormBuilder.API.Filters
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check if any parameter is [FromForm] with IFormFile (either direct or in DTO)
            var formParameters = context.MethodInfo.GetParameters()
                .Where(p => p.GetCustomAttributes(typeof(Microsoft.AspNetCore.Mvc.FromFormAttribute), false).Any())
                .ToList();

            if (!formParameters.Any())
                return;

            // Check if any form parameter contains IFormFile
            bool hasFileUpload = formParameters.Any(p =>
            {
                var paramType = p.ParameterType;
                
                // Direct IFormFile parameter
                if (paramType == typeof(IFormFile) || paramType == typeof(IFormFile[]))
                    return true;
                
                // List<IFormFile>
                if (paramType.IsGenericType && 
                    paramType.GetGenericTypeDefinition() == typeof(List<>) &&
                    paramType.GetGenericArguments()[0] == typeof(IFormFile))
                    return true;
                
                // DTO with IFormFile property
                var properties = paramType.GetProperties();
                return properties.Any(prop => 
                    prop.PropertyType == typeof(IFormFile) ||
                    prop.PropertyType == typeof(IFormFile[]) ||
                    (prop.PropertyType.IsGenericType && 
                     prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>) &&
                     prop.PropertyType.GetGenericArguments()[0] == typeof(IFormFile)));
            });

            if (!hasFileUpload)
                return;

            // For DTOs with [FromForm], Swashbuckle should handle it automatically
            // But we ensure the schema is correct for file properties
            foreach (var formParam in formParameters)
            {
                var paramType = formParam.ParameterType;
                
                // If it's a DTO, ensure file properties are correctly mapped
                if (paramType != typeof(IFormFile) && 
                    paramType != typeof(IFormFile[]) &&
                    !(paramType.IsGenericType && 
                      paramType.GetGenericTypeDefinition() == typeof(List<>) &&
                      paramType.GetGenericArguments()[0] == typeof(IFormFile)))
                {
                    // This is a DTO - Swashbuckle should handle it, but we can enhance it
                    if (operation.RequestBody?.Content != null &&
                        operation.RequestBody.Content.ContainsKey("multipart/form-data"))
                    {
                        var schema = operation.RequestBody.Content["multipart/form-data"].Schema;
                        if (schema?.Properties != null)
                        {
                            // Ensure IFormFile properties are marked as binary
                            foreach (var prop in paramType.GetProperties())
                            {
                                if (prop.PropertyType == typeof(IFormFile))
                                {
                                    if (schema.Properties.ContainsKey(prop.Name))
                                    {
                                        schema.Properties[prop.Name].Type = "string";
                                        schema.Properties[prop.Name].Format = "binary";
                                    }
                                }
                                else if (prop.PropertyType == typeof(IFormFile[]) ||
                                        (prop.PropertyType.IsGenericType && 
                                         prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>) &&
                                         prop.PropertyType.GetGenericArguments()[0] == typeof(IFormFile)))
                                {
                                    if (schema.Properties.ContainsKey(prop.Name))
                                    {
                                        schema.Properties[prop.Name].Type = "array";
                                        schema.Properties[prop.Name].Items = new OpenApiSchema 
                                        { 
                                            Type = "string", 
                                            Format = "binary" 
                                        };
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

