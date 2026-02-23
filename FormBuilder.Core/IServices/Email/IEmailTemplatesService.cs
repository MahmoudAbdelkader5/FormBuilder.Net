using FormBuilder.API.Models.DTOs;
using FormBuilder.Application.DTOS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IEmailTemplatesService
    {
        Task<ServiceResult<IEnumerable<EmailTemplateDto>>> GetAllAsync(int? documentTypeId = null, bool includeInactive = true);
        Task<ServiceResult<EmailTemplateDto>> GetByIdAsync(int id);
        Task<ServiceResult<EmailTemplateDto>> CreateAsync(CreateEmailTemplateDto createDto);
        Task<ServiceResult<EmailTemplateDto>> UpdateAsync(int id, UpdateEmailTemplateDto updateDto);
        Task<ServiceResult<bool>> DeleteAsync(int id);
        Task<ServiceResult<bool>> ToggleActiveAsync(int id, bool isActive);
    }
}


