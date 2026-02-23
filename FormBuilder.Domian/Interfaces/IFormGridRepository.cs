using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Domian.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IFormGridRepository : IBaseRepository<FORM_GRIDS>
    {
        // Explicitly include these methods from IBaseRepository if needed
        Task<FORM_GRIDS> GetByIdAsync(int id);
        Task<IEnumerable<FORM_GRIDS>> GetAllAsync();

        // Custom methods
        Task<IEnumerable<FORM_GRIDS>> GetByFormBuilderIdAsync(int formBuilderId);
        Task<IEnumerable<FORM_GRIDS>> GetByTabIdAsync(int tabId);
        Task<IEnumerable<FORM_GRIDS>> GetActiveByFormBuilderIdAsync(int formBuilderId);
        Task<FORM_GRIDS> GetByGridCodeAsync(string gridCode, int formBuilderId);
        Task<bool> GridCodeExistsAsync(string gridCode, int formBuilderId, int? excludeId = null);
        Task<int> GetNextGridOrderAsync(int formBuilderId, int? tabId = null);
        Task<bool> IsActiveAsync(int id);
    }
}