using FormBuilder.API.Models;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Application.DTOs.Formula;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IFormulaVariableService
    {
        Task<ApiResponse> GetAllAsync();
        Task<ApiResponse> GetByIdAsync(int id);
        Task<ApiResponse> CreateAsync(FormulaVariableCreateDto dto);
        Task<ApiResponse> UpdateAsync(int id, FormulaVariableUpdateDto dto);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse> ExistsAsync(int id);
    }
}
