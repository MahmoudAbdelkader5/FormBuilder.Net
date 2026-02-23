using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domian.Entitys.FromBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IFormGridColumnRepository : IBaseRepository<FORM_GRID_COLUMNS>
    {
        // Inherited from IBaseRepository but explicitly declared for clarity
        Task<FORM_GRID_COLUMNS> GetByIdAsync(int id);
        Task<IEnumerable<FORM_GRID_COLUMNS>> GetAllAsync();

        // Custom methods
        Task<IEnumerable<FORM_GRID_COLUMNS>> GetByGridIdAsync(int gridId);
        Task<IEnumerable<FORM_GRID_COLUMNS>> GetActiveByGridIdAsync(int gridId);
        Task<FORM_GRID_COLUMNS> GetByColumnCodeAsync(string columnCode, int gridId);
        Task<bool> ColumnCodeExistsAsync(string columnCode, int gridId, int? excludeId = null);
        Task<int> GetNextColumnOrderAsync(int gridId);
        Task<bool> IsActiveAsync(int id);
        Task<IEnumerable<FORM_GRID_COLUMNS>> GetByFormBuilderIdAsync(int formBuilderId);
    }
}