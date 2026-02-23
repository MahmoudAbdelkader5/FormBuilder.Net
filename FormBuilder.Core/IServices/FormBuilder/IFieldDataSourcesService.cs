using FormBuilder.API.Models;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Core.DTOS.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;
using CreateFieldDataSourceDto = FormBuilder.API.Models.CreateFieldDataSourceDto;
using UpdateFieldDataSourceDto = FormBuilder.API.Models.UpdateFieldDataSourceDto;

namespace FormBuilder.Core.IServices.FormBuilder
{
    public interface IFieldDataSourcesService
    {
        // Add this method
        Task<ApiResponse> GetAllAsync();

        // Existing methods
        Task<ApiResponse> GetByFieldIdAsync(int fieldId);
        Task<ApiResponse> GetActiveByFieldIdAsync(int fieldId);
        Task<ApiResponse> GetByIdAsync(int id);
        Task<ApiResponse> CreateAsync(CreateFieldDataSourceDto createDto);
        Task<ApiResponse> CreateBulkAsync(List<CreateFieldDataSourceDto> createDtos);
        Task<ApiResponse> UpdateAsync(int id, UpdateFieldDataSourceDto updateDto);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse> SoftDeleteAsync(int id);
        Task<ApiResponse> GetByFieldIdAndTypeAsync(int fieldId, string sourceType);
        Task<ApiResponse> GetDataSourcesCountAsync(int fieldId);

        // New methods for field options
        Task<ApiResponse> GetFieldOptionsAsync(int fieldId, Dictionary<string, object>? context = null, string? requestBodyJson = null);
        Task<ApiResponse> PreviewDataSourceAsync(PreviewDataSourceRequestDto request);
        Task<ApiResponse> InspectApiAsync(InspectApiRequestDto request);
        Task<ApiResponse> GetAvailableLookupTablesAsync(string? database = null);
        Task<ApiResponse> GetLookupTableColumnsAsync(string tableName, string? database = null);
    }
}