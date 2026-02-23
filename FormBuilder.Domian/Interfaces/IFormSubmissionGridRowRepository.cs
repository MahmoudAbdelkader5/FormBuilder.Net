using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IFormSubmissionGridRowRepository : IBaseRepository<FORM_SUBMISSION_GRID_ROWS>
    {
        // Inherited from IBaseRepository but explicitly declared for clarity
        Task<FORM_SUBMISSION_GRID_ROWS> GetByIdAsync(int id);
        Task<IEnumerable<FORM_SUBMISSION_GRID_ROWS>> GetAllAsync();

        // Custom methods
        Task<IEnumerable<FORM_SUBMISSION_GRID_ROWS>> GetBySubmissionIdAsync(int submissionId);
        Task<IEnumerable<FORM_SUBMISSION_GRID_ROWS>> GetByGridIdAsync(int gridId);
        Task<IEnumerable<FORM_SUBMISSION_GRID_ROWS>> GetBySubmissionAndGridAsync(int submissionId, int gridId);
        Task<IEnumerable<FORM_SUBMISSION_GRID_ROWS>> GetActiveRowsAsync(int submissionId, int gridId);
        Task<int> GetNextRowIndexAsync(int submissionId, int gridId);
        Task<bool> RowExistsAsync(int submissionId, int gridId, int rowIndex);
        Task<int> GetRowCountBySubmissionAsync(int submissionId);
        Task<int> GetRowCountByGridAsync(int gridId);
        Task<IEnumerable<FORM_SUBMISSION_GRID_ROWS>> GetByFormBuilderIdAsync(int formBuilderId);
    }
}