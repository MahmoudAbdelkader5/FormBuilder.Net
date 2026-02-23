using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Domian.Entitys.froms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IFormSubmissionAttachmentsRepository : IBaseRepository<FORM_SUBMISSION_ATTACHMENTS>
    {
        Task<FORM_SUBMISSION_ATTACHMENTS> GetByIdAsync(int id);
        Task<IEnumerable<FORM_SUBMISSION_ATTACHMENTS>> GetBySubmissionIdAsync(int submissionId);
        Task<IEnumerable<FORM_SUBMISSION_ATTACHMENTS>> GetByFieldIdAsync(int fieldId);
        Task<IEnumerable<FORM_SUBMISSION_ATTACHMENTS>> GetBySubmissionAndFieldAsync(int submissionId, int fieldId);
        Task<IEnumerable<FORM_SUBMISSION_ATTACHMENTS>> GetBySubmissionIdsAsync(List<int> submissionIds);
        Task<bool> DeleteBySubmissionIdAsync(int submissionId);
        Task<bool> DeleteBySubmissionAndFieldAsync(int submissionId, int fieldId);
        Task<long> GetTotalSizeBySubmissionAsync(int submissionId);
        Task<int> GetCountBySubmissionAsync(int submissionId);
        Task<bool> FileNameExistsAsync(int submissionId, string fileName);
    }
}