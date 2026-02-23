using FormBuilder.API.Models;
using FormBuilder.Core.DTOS.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IAlertRuleService
    {
        Task<ApiResponse> GetAllAsync();
        Task<ApiResponse> GetByIdAsync(int id);
        Task<ApiResponse> GetByDocumentTypeIdAsync(int documentTypeId);
        Task<ApiResponse> GetByTriggerTypeAsync(string triggerType);
        Task<ApiResponse> GetActiveByDocumentTypeAndTriggerAsync(int documentTypeId, string triggerType);
        Task<ApiResponse> CreateAsync(CreateAlertRuleDto createDto);
        Task<ApiResponse> UpdateAsync(int id, UpdateAlertRuleDto updateDto);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse> ActivateAsync(int id);
        Task<ApiResponse> DeactivateAsync(int id);
    }
}

