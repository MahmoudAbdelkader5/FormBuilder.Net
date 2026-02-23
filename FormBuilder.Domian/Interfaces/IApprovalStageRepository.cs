using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IApprovalStageRepository : IBaseRepository<APPROVAL_STAGES>
    {
        Task<IEnumerable<APPROVAL_STAGES>> GetAllAsync();
        Task<APPROVAL_STAGES> GetByIdAsync(int id);
        Task<bool> AnyAsync(int id);
    }
}
