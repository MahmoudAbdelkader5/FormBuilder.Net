using FormBuilder.API.Models.DTOs;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.Common;
using FormBuilder.Domian.Entitys.FromBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace FormBuilder.Domain.Interfaces.Services
{
    public interface IProjectService
    {
        Task<ServiceResult<IEnumerable<ProjectDto>>> GetAllAsync(Expression<Func<PROJECTS, bool>>? filter = null);
        Task<ServiceResult<PagedResult<ProjectDto>>> GetPagedAsync(int page = 1, int pageSize = 20, Expression<Func<PROJECTS, bool>>? filter = null);
        Task<ServiceResult<ProjectDto>> GetByIdAsync(int id, bool asNoTracking = false);
        Task<ServiceResult<ProjectDto>> GetByCodeAsync(string code, bool asNoTracking = false);
        Task<ServiceResult<IEnumerable<ProjectDto>>> GetActiveAsync();
        Task<ServiceResult<ProjectDto>> CreateAsync(CreateProjectDto createDto);
        Task<ServiceResult<ProjectDto>> UpdateAsync(int id, UpdateProjectDto updateDto);
        Task<ServiceResult<bool>> DeleteAsync(int id);
        Task<ServiceResult<bool>> ToggleActiveAsync(int id, bool isActive);
        Task<ServiceResult<bool>> ExistsAsync(int id);
        Task<ServiceResult<bool>> CodeExistsAsync(string code, int? excludeId = null);
    }
}