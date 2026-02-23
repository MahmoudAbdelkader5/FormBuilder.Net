using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.froms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces
{
    public interface IFieldOptionsRepository : IBaseRepository<FIELD_OPTIONS>
    {
        Task<IEnumerable<FIELD_OPTIONS>> GetByFieldIdAsync(int fieldId);
        Task<IEnumerable<FIELD_OPTIONS>> GetActiveByFieldIdAsync(int fieldId);
        Task<FIELD_OPTIONS> GetDefaultOptionAsync(int fieldId);
        Task<bool> FieldHasOptionsAsync(int fieldId);
        Task<int> GetOptionsCountAsync(int fieldId);
        // Note: GetByIdAsync and ExistsAsync are inherited from IBaseRepository<FIELD_OPTIONS>
    }
}