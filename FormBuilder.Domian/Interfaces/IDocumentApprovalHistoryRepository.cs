using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IDocumentApprovalHistoryRepository : IBaseRepository<DOCUMENT_APPROVAL_HISTORY>
    {
        Task<IEnumerable<DOCUMENT_APPROVAL_HISTORY>> GetBySubmissionIdAsync(int submissionId);
        Task<IEnumerable<DOCUMENT_APPROVAL_HISTORY>> GetByStageIdAsync(int stageId);
        Task<IEnumerable<DOCUMENT_APPROVAL_HISTORY>> GetByUserIdAsync(string userId);
        Task<IEnumerable<DOCUMENT_APPROVAL_HISTORY>> GetAllApprovalHistoryAsync();
        Task DeleteBySubmissionIdAsync(int submissionId);
    }
}

