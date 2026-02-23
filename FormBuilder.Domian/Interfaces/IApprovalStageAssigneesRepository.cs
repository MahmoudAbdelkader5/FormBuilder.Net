using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IApprovalStageAssigneesRepository : IBaseRepository<APPROVAL_STAGE_ASSIGNEES>
    {
        Task<IEnumerable<APPROVAL_STAGE_ASSIGNEES>> GetByStageIdAsync(int stageId);
        Task<IEnumerable<APPROVAL_STAGE_ASSIGNEES>> GetByUserIdAsync(string userId);
        Task<IEnumerable<APPROVAL_STAGE_ASSIGNEES>> GetByRoleIdAsync(string roleId);
    }
}

