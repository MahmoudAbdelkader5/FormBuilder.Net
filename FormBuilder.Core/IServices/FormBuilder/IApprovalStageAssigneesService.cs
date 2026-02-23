using FormBuilder.API.Models;
using FormBuilder.Application.DTOs.ApprovalWorkflow;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IApprovalStageAssigneesService
    {
        Task<ApiResponse> GetByStageIdAsync(int stageId);
        Task<ApiResponse> GetByIdAsync(int id);
        Task<ApiResponse> CreateAsync(ApprovalStageAssigneesCreateDto dto);
        Task<ApiResponse> UpdateAsync(int id, ApprovalStageAssigneesUpdateDto dto);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse> BulkUpdateAsync(StageAssigneesBulkDto dto);
        Task<ApiResponse> UpdateMissingRoleIdsAsync();
    }
}

