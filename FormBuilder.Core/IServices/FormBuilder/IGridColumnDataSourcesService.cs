using FormBuilder.API.Models;
using FormBuilder.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IGridColumnDataSourcesService
    {
        Task<ApiResponse> GetAllAsync();
        Task<ApiResponse> GetByIdAsync(int id);
        Task<ApiResponse> GetByColumnIdAsync(int columnId);
        Task<ApiResponse> GetActiveByColumnIdAsync(int columnId);
        Task<ApiResponse> GetByColumnIdAndTypeAsync(int columnId, string sourceType);
        Task<ApiResponse> CreateAsync(CreateGridColumnDataSourceDto createDto);
        Task<ApiResponse> UpdateAsync(int id, UpdateGridColumnDataSourceDto updateDto);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse> ToggleActiveAsync(int id, bool isActive);
        Task<ApiResponse> ExistsAsync(int id);
        Task<ApiResponse> ColumnHasDataSourcesAsync(int columnId);
        Task<ApiResponse> GetDataSourcesCountAsync(int columnId);
        Task<ApiResponse> GetColumnOptionsAsync(int columnId, Dictionary<string, object>? context = null, string? requestBodyJson = null);
    }
}

