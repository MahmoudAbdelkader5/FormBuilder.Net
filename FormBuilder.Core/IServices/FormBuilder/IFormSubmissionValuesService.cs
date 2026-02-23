using FormBuilder.API.Models;
using FormBuilder.API.Models;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Core.DTOS.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IFormSubmissionValuesService
    {
        Task<ApiResponse> GetAllAsync();
        Task<ApiResponse> GetByIdAsync(int id);
        Task<ApiResponse> GetBySubmissionIdAsync(int submissionId);
        Task<ApiResponse> GetByFieldIdAsync(int fieldId);
        Task<ApiResponse> GetBySubmissionAndFieldAsync(int submissionId, int fieldId);
        Task<ApiResponse> CreateAsync(CreateFormSubmissionValueDto createDto);
        Task<ApiResponse> CreateBulkAsync(BulkFormSubmissionValuesDto bulkDto);
        Task<ApiResponse> UpdateAsync(int id, UpdateFormSubmissionValueDto updateDto);
        Task<ApiResponse> UpdateByFieldAsync(int submissionId, int fieldId, UpdateFormSubmissionValueDto updateDto);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse> DeleteBySubmissionIdAsync(int submissionId);
        Task<ApiResponse> ExistsAsync(int id);
    }
}