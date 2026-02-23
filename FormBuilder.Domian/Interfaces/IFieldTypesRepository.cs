using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces
{
    public interface IFieldTypesRepository : IBaseRepository<FIELD_TYPES>
    {
        Task<FIELD_TYPES?> GetByIdAsync(int id);
        Task<IEnumerable<FIELD_TYPES>> GetActiveAsync();
        Task<FIELD_TYPES?> GetByTypeNameAsync(string typeName);
        Task<bool> TypeNameExistsAsync(string typeName, int? excludeId = null);
        Task<int> GetUsageCountAsync(int fieldTypeId);
    }
}

