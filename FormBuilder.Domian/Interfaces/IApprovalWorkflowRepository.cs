using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FromBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IApprovalWorkflowRepository : IBaseRepository<APPROVAL_WORKFLOWS>
    {
        Task<APPROVAL_WORKFLOWS> GetByIdAsync(int id);
        Task<APPROVAL_WORKFLOWS> GetByNameAsync(string name);
        Task<IEnumerable<APPROVAL_WORKFLOWS>> GetActiveAsync();
        Task<bool> NameExistsAsync(string name, int? excludeId = null);
        Task<bool> IsActiveAsync(int id);
        Task<APPROVAL_WORKFLOWS> GetActiveWorkflowByDocumentTypeIdAsync(int documentTypeId);
    }
}
