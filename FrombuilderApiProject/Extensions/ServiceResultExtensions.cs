using FormBuilder.Application.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace FormBuilder.API.Extensions
{
    public static class ServiceResultExtensions
    {
        public static IActionResult ToActionResult<T>(this ServiceResult<T> result)
        {
            if (result == null)
            {
                return new StatusCodeResult(500);
            }

            if (result.Success)
            {
                return new ObjectResult(result.Data) { StatusCode = result.StatusCode };
            }

            return new ObjectResult(new { error = result.ErrorMessage })
            {
                StatusCode = result.StatusCode
            };
        }
    }
}

