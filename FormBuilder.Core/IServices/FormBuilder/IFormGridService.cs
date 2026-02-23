using FormBuilder.API.Models;
using FormBuilder.API.DTOs;


namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IFormGridService
    {
        Task<ApiResponse> GetAllAsync();
        Task<ApiResponse> GetByIdAsync(int id);
        Task<ApiResponse> GetByFormBuilderIdAsync(int formBuilderId);
        Task<ApiResponse> GetByTabIdAsync(int tabId);
        Task<ApiResponse> GetActiveByFormBuilderIdAsync(int formBuilderId);
        Task<ApiResponse> GetByGridCodeAsync(string gridCode, int formBuilderId);
        Task<ApiResponse> CreateAsync(CreateFormGridDto createDto);
        Task<ApiResponse> UpdateAsync(int id, UpdateFormGridDto updateDto);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse> ToggleActiveAsync(int id, bool isActive);
        Task<ApiResponse> ExistsAsync(int id);
        Task<ApiResponse> GridCodeExistsAsync(string gridCode, int formBuilderId, int? excludeId = null);
        Task<ApiResponse> GetNextGridOrderAsync(int formBuilderId, int? tabId = null);
        Task<ApiResponse> CopyAsync(CopyFormGridDto copyDto);
    }
}