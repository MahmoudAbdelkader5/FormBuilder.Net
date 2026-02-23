using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Core.IServices.FormBuilder
{
    public interface ITableMenusService
    {
        // Menu Operations
        Task<ApiResponse> GetAllMenusAsync();
        Task<ApiResponse> GetMenuByIdAsync(int id);
        Task<ApiResponse> GetMenuByCodeAsync(string menuCode);
        Task<ApiResponse> CreateMenuAsync(CreateTableMenuDto createDto);
        Task<ApiResponse> UpdateMenuAsync(int id, UpdateTableMenuDto updateDto);
        Task<ApiResponse> DeleteMenuAsync(int id);
        Task<ApiResponse> SoftDeleteMenuAsync(int id);

        // Sub Menu Operations
        Task<ApiResponse> GetAllSubMenusAsync();
        Task<ApiResponse> GetSubMenusByMenuIdAsync(int menuId);
        Task<ApiResponse> GetSubMenuByIdAsync(int id);
        Task<ApiResponse> CreateSubMenuAsync(CreateTableSubMenuDto createDto);
        Task<ApiResponse> UpdateSubMenuAsync(int id, UpdateTableSubMenuDto updateDto);
        Task<ApiResponse> DeleteSubMenuAsync(int id);
        Task<ApiResponse> SoftDeleteSubMenuAsync(int id);

        // Menu Document Operations
        Task<ApiResponse> GetAllMenuDocumentsAsync();
        Task<ApiResponse> GetMenuDocumentsByMenuIdAsync(int menuId);
        Task<ApiResponse> GetMenuDocumentsBySubMenuIdAsync(int subMenuId);
        Task<ApiResponse> GetMenuDocumentByIdAsync(int id);
        Task<ApiResponse> CreateMenuDocumentAsync(CreateTableMenuDocumentDto createDto);
        Task<ApiResponse> UpdateMenuDocumentAsync(int id, UpdateTableMenuDocumentDto updateDto);
        Task<ApiResponse> DeleteMenuDocumentAsync(int id);
        Task<ApiResponse> SoftDeleteMenuDocumentAsync(int id);

        // Dashboard Operations (for end users)
        Task<ApiResponse> GetDashboardMenusAsync(List<string>? userPermissions = null);
        Task<ApiResponse> GetDashboardMenuByIdAsync(int menuId, List<string>? userPermissions = null);
    }
}

