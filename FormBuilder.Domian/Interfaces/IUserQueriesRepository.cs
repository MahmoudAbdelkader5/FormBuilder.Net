using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domian.Interfaces
{
    public interface IUserQueriesRepository : IBaseRepository<USER_QUERIES>
    {
        Task<IEnumerable<USER_QUERIES>> GetByUserIdAsync(string userId);
        Task<IEnumerable<USER_QUERIES>> GetByUserIdAndDatabaseAsync(string userId, string databaseName);
    }
}

