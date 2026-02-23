using FormBuilder.API.Models;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Application.DTOs.ApprovalWorkflow;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IApprovalStageService
    {
        Task<ApiResponse> GetAllAsync(int workflowId);
        Task<ApiResponse> GetByIdAsync(int id);
        Task<ApiResponse> CreateAsync(ApprovalStageCreateDto dto);
        Task<ApiResponse> UpdateAsync(int id, ApprovalStageUpdateDto dto);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse> ToggleActiveAsync(int id, bool isActive);
        Task<ApiResponse> GetFormFieldsByWorkflowIdAsync(int workflowId);
    }
}
