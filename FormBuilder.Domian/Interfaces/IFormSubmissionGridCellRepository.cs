using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IFormSubmissionGridCellRepository : IBaseRepository<FORM_SUBMISSION_GRID_CELLS>
    {
        Task<FORM_SUBMISSION_GRID_CELLS> GetByIdAsync(int id);
        Task<IEnumerable<FORM_SUBMISSION_GRID_CELLS>> GetAllAsync();

        // العمليات الضرورية فقط
        Task<IEnumerable<FORM_SUBMISSION_GRID_CELLS>> GetByRowIdAsync(int rowId);
        Task<FORM_SUBMISSION_GRID_CELLS> GetByRowAndColumnAsync(int rowId, int columnId);
        Task<bool> CellExistsAsync(int rowId, int columnId);
        Task<int> DeleteByRowIdAsync(int rowId);
        Task<int> DeleteBySubmissionIdAsync(int submissionId);
    }
}