using FormBuilder.API.Models;

using FormBuilder.Application.DTOs.ApprovalWorkflow;
using System.Threading;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IApprovalWorkflowService
    {
        Task<ApiResponse> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ApiResponse> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<ApiResponse> GetActiveAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse> CreateAsync(ApprovalWorkflowCreateDto dto, CancellationToken cancellationToken = default);
        Task<ApiResponse> UpdateAsync(int id, ApprovalWorkflowUpdateDto dto, CancellationToken cancellationToken = default);
        Task<ApiResponse> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<ApiResponse> ToggleActiveAsync(int id, bool isActive, CancellationToken cancellationToken = default);
        Task<ApiResponse> ExistsAsync(int id, CancellationToken cancellationToken = default);
        Task<ApiResponse> NameExistsAsync(string name, int? excludeId = null, CancellationToken cancellationToken = default);
        Task<ApiResponse> EnsureDefaultStageAsync(int workflowId, CancellationToken cancellationToken = default);
        Task<ApiResponse> FixAllWorkflowsWithoutStagesAsync(CancellationToken cancellationToken = default);
    }
}
