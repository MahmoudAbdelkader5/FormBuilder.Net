using FormBuilder.API.DTOs;
using FormBuilder.API.Models;
using FormBuilder.Application.DTOS;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;
using CreateGridColumnOptionDto = FormBuilder.API.DTOs.CreateGridColumnOptionDto;
using UpdateGridColumnOptionDto = FormBuilder.API.DTOs.UpdateGridColumnOptionDto;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IGridColumnOptionsService
    {
        Task<ApiResponse> GetAllAsync();
        Task<ApiResponse> GetByIdAsync(int id);
        Task<ApiResponse> GetByColumnIdAsync(int columnId);
        Task<ApiResponse> GetActiveByColumnIdAsync(int columnId);
        Task<ApiResponse> CreateAsync(CreateGridColumnOptionDto createDto);
        Task<ApiResponse> CreateBulkAsync(List<CreateGridColumnOptionDto> createDtos);
        Task<ApiResponse> UpdateAsync(int id, UpdateGridColumnOptionDto updateDto);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse> SoftDeleteAsync(int id);
        Task<ApiResponse> ToggleActiveAsync(int id, bool isActive);
        Task<ApiResponse> ExistsAsync(int id);
        Task<ApiResponse> GetDefaultOptionAsync(int columnId);
        Task<ApiResponse> GetOptionsCountAsync(int columnId);
        Task<ApiResponse> ColumnHasOptionsAsync(int columnId);
    }
}

