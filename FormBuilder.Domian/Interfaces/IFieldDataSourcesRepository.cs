using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.froms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces
{
    public interface IFieldDataSourcesRepository : IBaseRepository<FIELD_DATA_SOURCES>
    {
        Task<IEnumerable<FIELD_DATA_SOURCES>> GetByFieldIdAsync(int fieldId);
        Task<IEnumerable<FIELD_DATA_SOURCES>> GetActiveByFieldIdAsync(int fieldId);
        Task<FIELD_DATA_SOURCES> GetByFieldIdAsync(int fieldId, string sourceType);
        Task<bool> FieldHasDataSourcesAsync(int fieldId);
        Task<int> GetDataSourcesCountAsync(int fieldId);
        Task<FIELD_DATA_SOURCES> GetByIdAsync(int id);
    }
}