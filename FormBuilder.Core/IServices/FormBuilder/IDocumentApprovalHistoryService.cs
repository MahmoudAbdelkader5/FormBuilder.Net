using FormBuilder.API.Models;
using FormBuilder.Application.DTOs.ApprovalWorkflow;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IDocumentApprovalHistoryService
    {
        Task<ApiResponse> GetBySubmissionIdAsync(int submissionId);
        Task<ApiResponse> GetByStageIdAsync(int stageId);
        Task<ApiResponse> GetByUserIdAsync(string userId);
        Task<ApiResponse> GetAllApprovalHistoryAsync();
        Task<ApiResponse> CreateAsync(DocumentApprovalHistoryCreateDto dto);
        Task<ApiResponse> DeleteBySubmissionIdAsync(int submissionId);
    }
}

