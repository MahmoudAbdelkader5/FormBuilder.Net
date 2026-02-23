using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Domian.Entitys.froms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IAttachmentTypeRepository : IBaseRepository<ATTACHMENT_TYPES>
    {
        Task<ATTACHMENT_TYPES> GetByCodeAsync(string code);
        Task<IEnumerable<ATTACHMENT_TYPES>> GetActiveAsync();
        Task<bool> CodeExistsAsync(string code, int? excludeId = null);
        Task<bool> IsActiveAsync(int id);
        Task<ATTACHMENT_TYPES> GetByIdAsync(int id);

    }
}