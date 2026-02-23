using FormBuilder.API.Models;
using FormBuilder.Application.DTOs.ApprovalWorkflow;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IApprovalWorkflowRuntimeService
    {
        Task<ApiResponse> ActivateStageForSubmissionAsync(int submissionId);
        Task<ApiResponse> ResolveApproversForStageAsync(int stageId);
        Task<ApiResponse> CheckDelegationAsync(string userId);
        Task<ApiResponse> ProcessApprovalActionAsync(ApprovalActionDto dto);
        Task<ApiResponse> RequestStageSignatureAsync(RequestStageSignatureDto dto);
        Task<ApiResponse> GetApprovalInboxAsync(string userId);
        Task<ApiResponse> GetInboxDebugInfoAsync(string userId);
        Task<List<string>> ResolveUsersFromRolesAsync(List<string> roleIds);
    }
}
