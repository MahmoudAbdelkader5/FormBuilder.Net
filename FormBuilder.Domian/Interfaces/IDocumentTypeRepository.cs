using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Domian.Entitys.froms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IDocumentTypeRepository : IBaseRepository<DOCUMENT_TYPES>
    {
        Task<DOCUMENT_TYPES> GetByIdAsync(int id);
        Task<DOCUMENT_TYPES> GetByCodeAsync(string code);
        Task<IEnumerable<DOCUMENT_TYPES>> GetByFormBuilderIdAsync(int formBuilderId);
        Task<IEnumerable<DOCUMENT_TYPES>> GetActiveAsync();
        Task<IEnumerable<DOCUMENT_TYPES>> GetByParentMenuIdAsync(int? parentMenuId);
        Task<bool> CodeExistsAsync(string code, int? excludeId = null);
        Task<bool> IsActiveAsync(int id);
        Task<IEnumerable<DOCUMENT_TYPES>> GetMenuItemsAsync();
    }
}