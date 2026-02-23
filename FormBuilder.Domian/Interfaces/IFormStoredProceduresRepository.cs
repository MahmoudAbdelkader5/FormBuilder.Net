using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domian.Interfaces
{
    public interface IFormStoredProceduresRepository : IBaseRepository<FORM_STORED_PROCEDURES>
    {
        Task<IEnumerable<FORM_STORED_PROCEDURES>> GetActiveAsync();
        Task<IEnumerable<FORM_STORED_PROCEDURES>> GetByUsageTypeAsync(string? usageType);
        Task<IEnumerable<FORM_STORED_PROCEDURES>> GetByDatabaseAsync(string databaseName);
        Task<FORM_STORED_PROCEDURES?> GetByDatabaseSchemaAndProcedureAsync(string databaseName, string schemaName, string procedureName);
    }
}

