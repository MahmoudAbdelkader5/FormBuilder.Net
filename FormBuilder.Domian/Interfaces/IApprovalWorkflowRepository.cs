using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FromBuilder;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IApprovalWorkflowRepository : IBaseRepository<APPROVAL_WORKFLOWS>
    {
        Task<APPROVAL_WORKFLOWS> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<APPROVAL_WORKFLOWS> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<IEnumerable<APPROVAL_WORKFLOWS>> GetActiveAsync(CancellationToken cancellationToken = default);
        Task<bool> NameExistsAsync(string name, int? excludeId = null, CancellationToken cancellationToken = default);
        Task<bool> IsActiveAsync(int id, CancellationToken cancellationToken = default);
        Task<APPROVAL_WORKFLOWS> GetActiveWorkflowByDocumentTypeIdAsync(int documentTypeId, CancellationToken cancellationToken = default);
    }
}
