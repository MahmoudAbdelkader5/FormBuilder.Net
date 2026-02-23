using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Core.IServices.FormBuilder
{
    public interface IUserQueriesService
    {
        Task<ApiResponse> GetAllAsync(string userId);
        Task<ApiResponse> GetByDatabaseAsync(string userId, string databaseName);
        Task<ApiResponse> GetByIdAsync(int id, string userId);
        Task<ApiResponse> CreateAsync(CreateUserQueryDto createDto, string userId);
        Task<ApiResponse> UpdateAsync(int id, UpdateUserQueryDto updateDto, string userId);
        Task<ApiResponse> DeleteAsync(int id, string userId);
        Task<ApiResponse> SoftDeleteAsync(int id, string userId);
    }
}

