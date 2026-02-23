using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Application.DTOS
{
    // Models/ServiceResult.cs
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }


        public ServiceResult(bool success, T data, string errorMessage = "", int statusCode = 200)
        {
            Success = success;
            Data = data;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }

        // Helper methods for common scenarios
        public static ServiceResult<T> Ok(T data)
        {
            return new ServiceResult<T>(true, data, "", 200);
        }

        public static ServiceResult<T> NotFound(string message = "Resource not found")
        {
            return new ServiceResult<T>(false, default(T), message, 404);
        }

        public static ServiceResult<T> BadRequest(string message = "Bad request")
        {
            return new ServiceResult<T>(false, default(T), message, 400);
        }

        public static ServiceResult<T> Error(string message = "An error occurred", int statusCode = 500)
        {
            return new ServiceResult<T>(false, default(T), message, statusCode);
        }

        public static ServiceResult<T> Unauthorized(string message = "Unauthorized")
        {
            return new ServiceResult<T>(false, default(T), message, 401);
        }
    }

    // Non-generic version for void operations
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }

        public ServiceResult()
        {
        }

        public ServiceResult(bool success, string errorMessage = "", int statusCode = 200)
        {
            Success = success;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }

        // Helper methods
        public static ServiceResult Ok()
        {
            return new ServiceResult(true, "", 200);
        }

        public static ServiceResult NotFound(string message = "Resource not found")
        {
            return new ServiceResult(false, message, 404);
        }

        public static ServiceResult BadRequest(string message = "Bad request")
        {
            return new ServiceResult(false, message, 400);
        }

        public static ServiceResult Error(string message = "An error occurred", int statusCode = 500)
        {
            return new ServiceResult(false, message, statusCode);
        }

        public static ServiceResult Unauthorized(string message = "Unauthorized")
        {
            return new ServiceResult(false, message, 401);
        }

        // Convert from generic to non-generic
        public static ServiceResult FromGeneric<T>(ServiceResult<T> genericResult)
        {
            return new ServiceResult(genericResult.Success, genericResult.ErrorMessage, genericResult.StatusCode);
        }
    }
}
