using FormBuilder.API.Models.DTOs;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using FormBuilder.Domian.Entitys.FromBuilder;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IDocumentTypeService
    {
        Task<ServiceResult<IEnumerable<DocumentTypeDto>>> GetAllAsync(Expression<System.Func<DOCUMENT_TYPES, bool>>? filter = null);
        Task<ServiceResult<PagedResult<DocumentTypeDto>>> GetPagedAsync(int page = 1, int pageSize = 20, Expression<System.Func<DOCUMENT_TYPES, bool>>? filter = null);
        Task<ServiceResult<DocumentTypeDto>> GetByIdAsync(int id, bool asNoTracking = false);
        Task<ServiceResult<DocumentTypeDto>> GetByCodeAsync(string code, bool asNoTracking = false);
        Task<ServiceResult<IEnumerable<DocumentTypeDto>>> GetByFormBuilderIdAsync(int formBuilderId, bool asNoTracking = false);
        Task<ServiceResult<IEnumerable<DocumentTypeDto>>> GetActiveAsync();
        Task<ServiceResult<IEnumerable<DocumentTypeDto>>> GetByParentMenuIdAsync(int? parentMenuId, bool asNoTracking = false);
        Task<ServiceResult<DocumentTypeDto>> CreateAsync(CreateDocumentTypeDto createDto);
        Task<ServiceResult<DocumentTypeDto>> UpdateAsync(int id, UpdateDocumentTypeDto updateDto);
        Task<ServiceResult<bool>> DeleteAsync(int id);
        Task<ServiceResult<bool>> ToggleActiveAsync(int id, bool isActive);
        Task<ServiceResult<bool>> ExistsAsync(int id);
        Task<ServiceResult<bool>> CodeExistsAsync(string code, int? excludeId = null);
    }
}