using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.froms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IFormulaVariableRepository :IBaseRepository<FORMULA_VARIABLES>
    {
        Task<IEnumerable<FORMULA_VARIABLES>> GetAllAsync();
        Task<FORMULA_VARIABLES> GetByIdAsync(int id);
        Task<bool> AnyAsync(Func<FORMULA_VARIABLES, bool> predicate);
        
    }
}
