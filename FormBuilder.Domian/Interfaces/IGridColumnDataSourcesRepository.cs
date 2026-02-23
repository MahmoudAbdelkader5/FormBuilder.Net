using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces
{
    public interface IGridColumnDataSourcesRepository : IBaseRepository<GRID_COLUMN_DATA_SOURCES>
    {
        Task<IEnumerable<GRID_COLUMN_DATA_SOURCES>> GetByColumnIdAsync(int columnId);
        Task<IEnumerable<GRID_COLUMN_DATA_SOURCES>> GetActiveByColumnIdAsync(int columnId);
        Task<GRID_COLUMN_DATA_SOURCES?> GetByColumnIdAsync(int columnId, string sourceType);
        Task<bool> ColumnHasDataSourcesAsync(int columnId);
        Task<int> GetDataSourcesCountAsync(int columnId);
        Task<GRID_COLUMN_DATA_SOURCES?> GetByIdAsync(int id);
    }
}

