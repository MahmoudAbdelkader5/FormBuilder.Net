using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domian.Entitys.FormBuilder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IFormBuilderService
    {
        Task<ServiceResult<FormBuilderDto>> CreateAsync(CreateFormBuilderDto dto);
        Task<ServiceResult<FormBuilderDto>> UpdateAsync(int id, UpdateFormBuilderDto dto);
        Task<ServiceResult<bool>> DeleteAsync(int id);
        Task<ServiceResult<FormBuilderDto>> GetByIdAsync(int id, bool asNoTracking = false);
        Task<ServiceResult<FormBuilderDto>> GetByCodeAsync(string formCode, bool asNoTracking = false);
        Task<ServiceResult<IEnumerable<FormBuilderDto>>> GetAllAsync(Expression<Func<FORM_BUILDER, bool>>? filter = null);
        Task<ServiceResult<PagedResult<FormBuilderDto>>> GetPagedAsync(int page = 1, int pageSize = 20);
        Task<ServiceResult<bool>> IsFormCodeExistsAsync(string formCode, int? excludeId = null);
        Task<ServiceResult<FormBuilderDto>> DuplicateAsync(int id, string? newFormCode = null, string? newFormName = null);
        Task<ServiceResult<FormBuilderDto>> ToggleActiveAsync(int id, bool isActive);
        Task<ServiceResult<FormBuilderDto>> RestoreAsync(int id);
    }
}
