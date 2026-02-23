using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces
{
    public interface IGridColumnOptionsRepository : IBaseRepository<GRID_COLUMN_OPTIONS>
    {
        Task<IEnumerable<GRID_COLUMN_OPTIONS>> GetByColumnIdAsync(int columnId);
        Task<IEnumerable<GRID_COLUMN_OPTIONS>> GetActiveByColumnIdAsync(int columnId);
        Task<GRID_COLUMN_OPTIONS?> GetDefaultOptionAsync(int columnId);
        Task<bool> ColumnHasOptionsAsync(int columnId);
        Task<int> GetOptionsCountAsync(int columnId);
    }
}

