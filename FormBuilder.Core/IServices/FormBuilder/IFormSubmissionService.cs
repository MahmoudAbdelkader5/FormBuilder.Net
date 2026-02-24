using FormBuilder.API.Models;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Core.DTOS.FormBuilder;
using System.Threading;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IFormSubmissionsService
    {
        Task<ApiResponse> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ApiResponse> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
        Task<ApiResponse> GetByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken = default);
        Task<ApiResponse> GetByFormBuilderIdAsync(int formBuilderId, CancellationToken cancellationToken = default);
        Task<ApiResponse> GetByDocumentTypeIdAsync(int documentTypeId, CancellationToken cancellationToken = default);
        Task<ApiResponse> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<ApiResponse> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
        Task<ApiResponse> GetDraftAsync(int formBuilderId, int projectId, string submittedByUserId, CancellationToken cancellationToken = default);
        Task<ApiResponse> GetOrCreateDraftAsync(int formBuilderId, int projectId, string submittedByUserId, int? seriesId = null, CancellationToken cancellationToken = default);
        Task<ApiResponse> CreateAsync(CreateFormSubmissionDto createDto, CancellationToken cancellationToken = default);
        Task<ApiResponse> CreateDraftAsync(int formBuilderId, int projectId, string submittedByUserId, int? seriesId = null, CancellationToken cancellationToken = default);
        Task<ApiResponse> CreateDraftAsync(CreateDraftDto createDraftDto, CancellationToken cancellationToken = default);
        Task<ApiResponse> UpdateAsync(int id, UpdateFormSubmissionDto updateDto, CancellationToken cancellationToken = default);
        Task<ApiResponse> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<ApiResponse> SubmitAsync(SubmitFormDto submitDto, CancellationToken cancellationToken = default);
        Task<ApiResponse> GetSigningUrlAsync(int submissionId, string requestedByUserId, CancellationToken cancellationToken = default);
        Task<ApiResponse> UpdateStatusAsync(int id, string status, CancellationToken cancellationToken = default);
        Task<ApiResponse> ApproveSubmissionAsync(ApproveSubmissionDto dto, CancellationToken cancellationToken = default);
        Task<ApiResponse> RejectSubmissionAsync(RejectSubmissionDto dto, CancellationToken cancellationToken = default);
        Task<ApiResponse> ExistsAsync(int id, CancellationToken cancellationToken = default);
        Task<ApiResponse> SaveFormSubmissionDataAsync(SaveFormSubmissionDataDto saveDto, string? resolvedUserId = null, CancellationToken cancellationToken = default);
    }
}
