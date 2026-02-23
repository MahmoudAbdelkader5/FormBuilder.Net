using FormBuilder.API.Models;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Core.DTOS.FormBuilder;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IFormSubmissionsService
    {
        Task<ApiResponse> GetAllAsync();
        Task<ApiResponse> GetByIdAsync(int id);
        Task<ApiResponse> GetByIdWithDetailsAsync(int id);
        Task<ApiResponse> GetByDocumentNumberAsync(string documentNumber);
        Task<ApiResponse> GetByFormBuilderIdAsync(int formBuilderId);
        Task<ApiResponse> GetByDocumentTypeIdAsync(int documentTypeId);
        Task<ApiResponse> GetByUserIdAsync(string userId);
        Task<ApiResponse> GetByStatusAsync(string status);
        Task<ApiResponse> GetDraftAsync(int formBuilderId, int projectId, string submittedByUserId);
        Task<ApiResponse> GetOrCreateDraftAsync(int formBuilderId, int projectId, string submittedByUserId, int? seriesId = null);
        Task<ApiResponse> CreateAsync(CreateFormSubmissionDto createDto);
        Task<ApiResponse> CreateDraftAsync(int formBuilderId, int projectId, string submittedByUserId, int? seriesId = null);
        Task<ApiResponse> CreateDraftAsync(CreateDraftDto createDraftDto);
        Task<ApiResponse> UpdateAsync(int id, UpdateFormSubmissionDto updateDto);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse> SubmitAsync(SubmitFormDto submitDto);
        Task<ApiResponse> GetSigningUrlAsync(int submissionId, string requestedByUserId);
        Task<ApiResponse> UpdateStatusAsync(int id, string status);
        Task<ApiResponse> ApproveSubmissionAsync(ApproveSubmissionDto dto);
        Task<ApiResponse> RejectSubmissionAsync(RejectSubmissionDto dto);
        Task<ApiResponse> ExistsAsync(int id);
        Task<ApiResponse> SaveFormSubmissionDataAsync(SaveFormSubmissionDataDto saveDto, string? resolvedUserId = null);
    }
}
