using FormBuilder.API.Models;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Core.DTOS.FormBuilder;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IAttachmentTypeService
    {
        Task<ApiResponse> GetAllAsync();
        Task<ApiResponse> GetByIdAsync(int id);
        Task<ApiResponse> GetByCodeAsync(string code);
        Task<ApiResponse> GetActiveAsync();
        Task<ApiResponse> CreateAsync(CreateAttachmentTypeDto createDto);
        Task<ApiResponse> UpdateAsync(int id, UpdateAttachmentTypeDto updateDto);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse> ToggleActiveAsync(int id, bool isActive);
        Task<ApiResponse> ExistsAsync(int id);
    }
}