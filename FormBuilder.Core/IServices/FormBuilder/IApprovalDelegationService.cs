using FormBuilder.API.Models;
using FormBuilder.Application.DTOs.ApprovalWorkflow;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IApprovalDelegationService
    {
        Task<ApiResponse> GetAllAsync(string fromUserId = null);
        Task<ApiResponse> GetByIdAsync(int id);
        Task<ApiResponse> CreateAsync(ApprovalDelegationCreateDto dto);
        Task<ApiResponse> UpdateAsync(int id, ApprovalDelegationUpdateDto dto);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse> GetActiveDelegationsAsync(string userId);
        
        /// <summary>
        /// Get active delegations by ToUserId (the delegated user)
        /// </summary>
        Task<ApiResponse> GetActiveDelegationsByToUserIdAsync(string toUserId);
        
        /// <summary>
        /// Get all delegations by ToUserId (including inactive ones) - for debugging
        /// </summary>
        Task<ApiResponse> GetAllDelegationsByToUserIdAsync(string toUserId);
        
        /// <summary>
        /// Resolves the delegated approver based on priority: Document -> Workflow -> Global
        /// Returns the delegated user ID if found, null otherwise
        /// </summary>
        Task<string?> ResolveDelegatedApproverAsync(
            string originalApproverId, 
            int? workflowId, 
            int? submissionId);
    }
}

