using FormBuilder.API.Models;
using FormBuilder.API.Models;
using FormBuilder.API.DTOs;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IFormSubmissionGridCellService
    {
        // CRUD الأساسية الكاملة
        Task<ApiResponse> GetAllAsync();
        Task<ApiResponse> GetByIdAsync(int id);
        Task<ApiResponse> CreateAsync(CreateFormSubmissionGridCellDto createDto);
        Task<ApiResponse> UpdateAsync(int id, UpdateFormSubmissionGridCellDto updateDto);
        Task<ApiResponse> DeleteAsync(int id);

        // عمليات ضرورية فقط (3 عمليات)
        Task<ApiResponse> GetByRowIdAsync(int rowId);
        Task<ApiResponse> GetByRowAndColumnAsync(int rowId, int columnId);
        Task<ApiResponse> DeleteByRowIdAsync(int rowId);

        
    }
}