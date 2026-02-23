using FormBuilder.API.Models;
using FormBuilder.API.DTOs;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IFormSubmissionGridRowService
    {
        Task<ApiResponse> GetAllAsync();
        Task<ApiResponse> GetByIdAsync(int id);
        Task<ApiResponse> GetBySubmissionIdAsync(int submissionId);
        Task<ApiResponse> GetByGridIdAsync(int gridId);
        Task<ApiResponse> GetBySubmissionAndGridAsync(int submissionId, int gridId);
        Task<ApiResponse> GetActiveRowsAsync(int submissionId, int gridId);
        Task<ApiResponse> CreateAsync(CreateFormSubmissionGridRowDto createDto);
        Task<ApiResponse> CreateMultipleAsync(List<CreateFormSubmissionGridRowDto> createDtos);
        Task<ApiResponse> UpdateAsync(int id, UpdateFormSubmissionGridRowDto updateDto);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse> DeleteBySubmissionAndGridAsync(int submissionId, int gridId);
        Task<ApiResponse> ToggleActiveAsync(int id, bool isActive);
        Task<ApiResponse> ExistsAsync(int id);
        Task<ApiResponse> RowExistsAsync(int submissionId, int gridId, int rowIndex);
        Task<ApiResponse> GetNextRowIndexAsync(int submissionId, int gridId);
        Task<ApiResponse> GetRowCountBySubmissionAsync(int submissionId);
        Task<ApiResponse> GetRowCountByGridAsync(int gridId);
        Task<ApiResponse> GetByFormBuilderIdAsync(int formBuilderId);
        Task<ApiResponse> ReorderRowsAsync(int submissionId, int gridId);
        Task<ApiResponse> SaveBulkGridDataAsync(BulkSaveGridDataDto bulkDto);
        Task<ApiResponse> GetCompleteGridDataAsync(int submissionId, int gridId);
        Task<ApiResponse> ValidateGridDataAsync(BulkSaveGridDataDto bulkDto);
    }
}