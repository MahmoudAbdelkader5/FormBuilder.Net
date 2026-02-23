using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FormBuilder.API.Filters
{
    /// <summary>
    /// Custom model validator provider to skip validation for optional fields
    /// </summary>
    public class CustomModelValidatorProvider : IModelValidatorProvider
    {
        public void CreateValidators(ModelValidatorProviderContext context)
        {
            // This provider can be used to customize validation behavior
            // Currently, validation is handled by FluentValidation and ValidationFilter
            // This class exists to satisfy the reference in Program.cs
        }
    }
}

