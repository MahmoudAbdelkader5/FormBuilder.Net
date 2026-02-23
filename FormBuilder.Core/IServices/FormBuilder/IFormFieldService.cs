using FormBuilder.Core.DTOS;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Core.DTOS.FormFields;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.API.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CreateFormFieldDto = FormBuilder.Core.DTOS.FormFields.CreateFormFieldDto;
using UpdateFormFieldDto = FormBuilder.API.Models.UpdateFormFieldDto;
using FormBuilder.Application.DTOS;

namespace FormBuilder.Core.IServices.FormBuilder
{
    public interface IFormFieldService
    {
        // Base CRUD operations
        Task<ServiceResult<IEnumerable<FormFieldDto>>> GetAllAsync(Expression<Func<FORM_FIELDS, bool>>? filter = null);
        Task<ServiceResult<FormFieldDto>> GetByIdAsync(int id, bool asNoTracking = false);
        Task<ServiceResult<FormFieldDto>> CreateAsync(CreateFormFieldDto dto);
        Task<ServiceResult<FormFieldDto>> UpdateAsync(int id, UpdateFormFieldDto dto);
        Task<ServiceResult<bool>> DeleteAsync(int id);

        // Custom operations specific to FormField
        Task<ServiceResult<IEnumerable<FormFieldDto>>> GetActiveAsync();
        Task<ServiceResult<IEnumerable<FormFieldDto>>> GetByTabIdAsync(int tabId);
        Task<ServiceResult<IEnumerable<FormFieldDto>>> GetByFormIdAsync(int formBuilderId);
        Task<ServiceResult<IEnumerable<FormFieldDto>>> GetMandatoryFieldsAsync(int tabId);
        Task<ServiceResult<IEnumerable<FormFieldDto>>> GetVisibleFieldsAsync(int tabId);
        Task<ServiceResult<FormFieldDto>> GetByFieldCodeAsync(string fieldCode);
        Task<ServiceResult<IEnumerable<FormFieldDto>>> GetEditableFieldsAsync(int tabId);
        Task<ServiceResult<IEnumerable<FormFieldDto>>> GetFieldsByGridIdAsync(int gridId);
        Task<ServiceResult<bool>> SoftDeleteAsync(int id);
        Task<ServiceResult<FormFieldDto>> RestoreAsync(int id);

        // Validation
        Task<bool> IsFieldCodeUniqueAsync(string fieldCode, int? ignoreId = null);
        Task<bool> IsFieldNameUniqueAsync(string fieldName, int? ignoreId = null, int? tabId = null);

        // Utility
        Task<bool> ExistsAsync(int id);
        Task<ServiceResult<int>> GetUsageCountAsync(int fieldId);
        Task<ServiceResult<int>> GetFieldsCountByTabAsync(int tabId);
        Task<ServiceResult<int>> GetFieldsCountByFormAsync(int formBuilderId);
    }
}