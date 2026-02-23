using FormBuilder.API.Models;
using FormBuilder.API.Models;
using FormBuilder.API.DTOs;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IFormGridColumnService
    {
        Task<ApiResponse> GetAllAsync();
        Task<ApiResponse> GetByIdAsync(int id);
        Task<ApiResponse> GetByGridIdAsync(int gridId);
        Task<ApiResponse> GetActiveByGridIdAsync(int gridId);
        Task<ApiResponse> GetByColumnCodeAsync(string columnCode, int gridId);
        Task<ApiResponse> GetByFormBuilderIdAsync(int formBuilderId);
        Task<ApiResponse> CreateAsync(CreateFormGridColumnDto createDto);
        Task<ApiResponse> UpdateAsync(int id, UpdateFormGridColumnDto updateDto);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse> SoftDeleteAsync(int id);
        Task<ApiResponse> ToggleActiveAsync(int id, bool isActive);
        Task<ApiResponse> ExistsAsync(int id);
        Task<ApiResponse> ColumnCodeExistsAsync(string columnCode, int gridId, int? excludeId = null);
        Task<ApiResponse> GetNextColumnOrderAsync(int gridId);
    }
}