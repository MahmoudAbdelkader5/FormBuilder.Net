using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Domian.Entitys.froms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IProjectRepository : IBaseRepository<PROJECTS>
    {
        Task<PROJECTS> GetByIdAsync(int id);
        Task<PROJECTS> GetByCodeAsync(string code);
        Task<IEnumerable<PROJECTS>> GetActiveAsync();
        Task<bool> CodeExistsAsync(string code, int? excludeId = null);
        Task<bool> IsActiveAsync(int id);
    }
}