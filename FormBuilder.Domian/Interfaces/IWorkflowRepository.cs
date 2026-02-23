using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Domian.Entitys.froms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IWorkflowRepository : IBaseRepository<APPROVAL_WORKFLOWS>
    {
        Task<APPROVAL_WORKFLOWS> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<APPROVAL_WORKFLOWS>> GetByDocumentTypeIdAsync(int documentTypeId);
        Task<IEnumerable<APPROVAL_WORKFLOWS>> GetActiveAsync();
        Task<IEnumerable<APPROVAL_WORKFLOWS>> GetActiveByDocumentTypeIdAsync(int documentTypeId);
        Task<bool> NameExistsAsync(string name, int? excludeId = null);
        Task<bool> IsNameUniqueForDocumentTypeAsync(string name, int documentTypeId, int? excludeId = null);
        Task<bool> HasWorkflowsAsync(int documentTypeId);
        Task<bool> HasActiveWorkflowsAsync(int documentTypeId);
        Task<int> CountByDocumentTypeIdAsync(int documentTypeId);
        Task<int> CountActiveByDocumentTypeIdAsync(int documentTypeId);
        Task<IEnumerable<APPROVAL_WORKFLOWS>> SearchAsync(string searchTerm, int? documentTypeId = null);
        Task<IEnumerable<APPROVAL_WORKFLOWS>> GetByDocumentTypeIdsAsync(List<int> documentTypeIds);
        Task<APPROVAL_WORKFLOWS> GetDefaultWorkflowForDocumentTypeAsync(int documentTypeId);
        Task<bool> IsDefaultWorkflowAsync(int workflowId);
        Task SetAsDefaultWorkflowAsync(int workflowId);
        Task RemoveDefaultStatusFromOtherWorkflowsAsync(int documentTypeId, int excludeWorkflowId);
        Task<IEnumerable<APPROVAL_WORKFLOWS>> GetWorkflowsWithStepsAsync(int? documentTypeId = null);
        Task<APPROVAL_WORKFLOWS> GetWorkflowWithStepsAsync(int workflowId);
        Task<bool> IsWorkflowUsedAsync(int workflowId);
        Task<IEnumerable<APPROVAL_WORKFLOWS>> GetByStatusAsync(bool isActive, int? documentTypeId = null);
        Task<IEnumerable<string>> GetWorkflowNamesByDocumentTypeIdAsync(int documentTypeId);
        Task<Dictionary<int, int>> GetWorkflowCountByDocumentTypesAsync(List<int> documentTypeIds);
    }
}