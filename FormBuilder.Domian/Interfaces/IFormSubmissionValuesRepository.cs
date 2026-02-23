using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.froms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IFormSubmissionValuesRepository : IBaseRepository<FORM_SUBMISSION_VALUES>
    {
        Task<FORM_SUBMISSION_VALUES> GetByIdAsync(int id);
        Task<IEnumerable<FORM_SUBMISSION_VALUES>> GetBySubmissionIdAsync(int submissionId);
        Task<IEnumerable<FORM_SUBMISSION_VALUES>> GetByFieldIdAsync(int fieldId);
        Task<FORM_SUBMISSION_VALUES> GetBySubmissionAndFieldAsync(int submissionId, int fieldId);
        Task<IEnumerable<FORM_SUBMISSION_VALUES>> GetBySubmissionIdsAsync(List<int> submissionIds);
        Task<bool> DeleteBySubmissionIdAsync(int submissionId);
        Task<bool> ExistsBySubmissionAndFieldAsync(int submissionId, int fieldId);
        Task<Dictionary<int, FORM_SUBMISSION_VALUES>> GetFieldValuesDictionaryAsync(int submissionId);
    }
}