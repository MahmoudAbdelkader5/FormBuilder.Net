using FormBuilder.API.Models;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Core.DTOS.FormBuilder;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IDocumentSeriesService
    {
        Task<ApiResponse> GetAllAsync();
        Task<ApiResponse> GetByIdAsync(int id);
        Task<ApiResponse> GetBySeriesCodeAsync(string seriesCode);
        Task<ApiResponse> GetByDocumentTypeIdAsync(int documentTypeId);
        Task<ApiResponse> GetByProjectIdAsync(int projectId);
        Task<ApiResponse> GetActiveAsync();
        Task<ApiResponse> GetDefaultSeriesAsync(int documentTypeId, int projectId);
        Task<ApiResponse> CreateAsync(CreateDocumentSeriesDto createDto);
        Task<ApiResponse> UpdateAsync(int id, UpdateDocumentSeriesDto updateDto);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse> ToggleActiveAsync(int id, bool isActive);
        Task<ApiResponse> SetAsDefaultAsync(int id);
        Task<ApiResponse> GetNextNumberAsync(int seriesId);
        Task<ApiResponse> ExistsAsync(int id);
    }
}