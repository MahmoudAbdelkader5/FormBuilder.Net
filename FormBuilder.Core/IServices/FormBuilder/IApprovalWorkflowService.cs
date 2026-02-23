using FormBuilder.API.Models;

using FormBuilder.Application.DTOs.ApprovalWorkflow;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IApprovalWorkflowService
    {
        Task<ApiResponse> GetAllAsync();
        Task<ApiResponse> GetByIdAsync(int id);
        Task<ApiResponse> GetByNameAsync(string name);
        Task<ApiResponse> GetActiveAsync();
        Task<ApiResponse> CreateAsync(ApprovalWorkflowCreateDto dto);
        Task<ApiResponse> UpdateAsync(int id, ApprovalWorkflowUpdateDto dto);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse> ToggleActiveAsync(int id, bool isActive);
        Task<ApiResponse> ExistsAsync(int id);
        Task<ApiResponse> NameExistsAsync(string name, int? excludeId = null);
        Task<ApiResponse> EnsureDefaultStageAsync(int workflowId);
        Task<ApiResponse> FixAllWorkflowsWithoutStagesAsync();
    }
}
