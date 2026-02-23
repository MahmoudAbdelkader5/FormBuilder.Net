using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using FormBuilder.Core.DTOS.FormTabs;
using FormBuilder.Domian.Entitys.FormBuilder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FormBuilder.Services.Services
{
    public interface IFormTabService
    {
        Task<ServiceResult<IEnumerable<FormTabDto>>> GetAllAsync(Expression<Func<FORM_TABS, bool>>? filter = null);
        Task<ServiceResult<PagedResult<FormTabDto>>> GetPagedAsync(int page = 1, int pageSize = 20, Expression<Func<FORM_TABS, bool>>? filter = null);
        Task<ServiceResult<FormTabDto>> GetByIdAsync(int id, bool asNoTracking = false);
        Task<ServiceResult<FormTabDto>> GetByCodeAsync(string tabCode, bool asNoTracking = false);
        Task<ServiceResult<IEnumerable<FormTabDto>>> GetByFormIdAsync(int formBuilderId);
        Task<ServiceResult<IEnumerable<FormTabDto>>> GetAllDeletedAsync();
        Task<ServiceResult<PagedResult<FormTabDto>>> GetPagedDeletedAsync(int page = 1, int pageSize = 20);
        Task<ServiceResult<IEnumerable<FormTabDto>>> GetAllIncludingDeletedAsync(Expression<Func<FORM_TABS, bool>>? filter = null);
        Task<ServiceResult<FormTabDto>> CreateAsync(CreateFormTabDto dto);
        Task<ServiceResult<FormTabDto>> UpdateAsync(int id, UpdateFormTabDto dto);
        Task<ServiceResult<bool>> DeleteAsync(int id);
        Task<ServiceResult<bool>> ToggleActiveAsync(int id, bool isActive);
        Task<ServiceResult<FormTabDto>> RestoreAsync(int id);
        Task<ServiceResult<bool>> ExistsAsync(int id);
        Task<ServiceResult<bool>> CodeExistsAsync(int formBuilderId, string tabCode, int? excludeId = null);
    }
}