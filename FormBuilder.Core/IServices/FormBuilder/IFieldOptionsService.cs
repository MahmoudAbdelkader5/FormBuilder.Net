using FormBuilder.Application.DTOS;
using FormBuilder.API.Models;
using FormBuilder.Domian.Entitys.froms;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CreateFieldOptionDto = FormBuilder.API.Models.CreateFieldOptionDto;
using UpdateFieldOptionDto = FormBuilder.API.Models.UpdateFieldOptionDto;

namespace FormBuilder.Core.IServices.FormBuilder
{
    public interface IFieldOptionsService
    {
        // Base CRUD operations
        Task<ServiceResult<IEnumerable<FieldOptionDto>>> GetAllAsync(Expression<Func<FIELD_OPTIONS, bool>>? filter = null);
        Task<ServiceResult<FieldOptionDto>> GetByIdAsync(int id, bool asNoTracking = false);
        Task<ServiceResult<FieldOptionDto>> CreateAsync(CreateFieldOptionDto createDto);
        Task<ServiceResult<FieldOptionDto>> UpdateAsync(int id, UpdateFieldOptionDto updateDto);
        Task<ServiceResult<bool>> DeleteAsync(int id);

        // Custom operations specific to FieldOptions
        Task<ServiceResult<IEnumerable<FieldOptionDto>>> GetByFieldIdAsync(int fieldId);
        Task<ServiceResult<IEnumerable<FieldOptionDto>>> GetActiveByFieldIdAsync(int fieldId);
        Task<ServiceResult<IEnumerable<FieldOptionDto>>> CreateBulkAsync(List<CreateFieldOptionDto> createDtos);
        Task<ServiceResult<bool>> SoftDeleteAsync(int id, string? deletedByUserId = null);
        Task<ServiceResult<FieldOptionDto>> GetDefaultOptionAsync(int fieldId);
        Task<ServiceResult<int>> GetOptionsCountAsync(int fieldId);
        Task<bool> FieldHasOptionsAsync(int fieldId);
    }
}