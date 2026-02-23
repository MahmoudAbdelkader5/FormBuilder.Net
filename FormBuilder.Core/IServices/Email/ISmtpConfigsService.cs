using FormBuilder.API.Models.DTOs;
using FormBuilder.Application.DTOS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface ISmtpConfigsService
    {
        Task<ServiceResult<IEnumerable<SmtpConfigDto>>> GetAllAsync(bool includeInactive = true);
        Task<ServiceResult<SmtpConfigDto>> GetByIdAsync(int id);
        Task<ServiceResult<SmtpConfigDto>> CreateAsync(CreateSmtpConfigDto createDto);
        Task<ServiceResult<SmtpConfigDto>> UpdateAsync(int id, UpdateSmtpConfigDto updateDto);
        Task<ServiceResult<bool>> DeleteAsync(int id);
        Task<ServiceResult<bool>> ToggleActiveAsync(int id, bool isActive);
    }
}


