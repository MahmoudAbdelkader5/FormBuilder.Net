using FormBuilder.API.Models;
using FormBuilder.Core.DTOS.FormBuilder;
using System.Threading.Tasks;

namespace FormBuilder.Core.IServices.FormBuilder
{
    public interface IFormStoredProceduresService
    {
        Task<ApiResponse> GetAllAsync();
        Task<ApiResponse> GetByIdAsync(int id);
        Task<ApiResponse> GetByUsageTypeAsync(string? usageType);
        Task<ApiResponse> GetByDatabaseAsync(string databaseName);
        Task<ApiResponse> CreateAsync(CreateStoredProcedureDto createDto, string userId);
        Task<ApiResponse> UpdateAsync(int id, UpdateStoredProcedureDto updateDto, string userId);
        Task<ApiResponse> DeleteAsync(int id, string userId);
        Task<ApiResponse> SoftDeleteAsync(int id, string userId);
        Task<ApiResponse> ValidateStoredProcedureAsync(int storedProcedureId);
    }
}

