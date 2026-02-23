using FormBuilder.API.DTOs;
using FormBuilder.Application.DTOS;
using FormBuilder.Domian.Entitys.FormBuilder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IFieldTypesService
    {
        Task<ServiceResult<IEnumerable<FieldTypeDto>>> GetAllAsync(Expression<Func<FIELD_TYPES, bool>>? filter = null);
        Task<ServiceResult<FieldTypeDto>> GetByIdAsync(int id, bool asNoTracking = false);
        Task<ServiceResult<FieldTypeDto>> GetByTypeNameAsync(string typeName);
        Task<ServiceResult<IEnumerable<FieldTypeDto>>> GetActiveAsync();
        Task<ServiceResult<FieldTypeDto>> CreateAsync(CreateFieldTypeDto createDto);
        Task<ServiceResult<FieldTypeDto>> UpdateAsync(int id, UpdateFieldTypeDto updateDto);
        Task<ServiceResult<bool>> DeleteAsync(int id);
        Task<ServiceResult<bool>> ToggleActiveAsync(int id, bool isActive);
        Task<ServiceResult<bool>> ExistsAsync(int id);
        Task<ServiceResult<bool>> TypeNameExistsAsync(string typeName, int? excludeId = null);
        Task<ServiceResult<int>> GetUsageCountAsync(int fieldTypeId);
    }
}

