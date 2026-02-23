using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domian.Interfaces
{
    public interface ITableMenusRepository : IBaseRepository<TABLE_MENUS>
    {
        Task<IEnumerable<TABLE_MENUS>> GetAllActiveAsync();
        Task<TABLE_MENUS?> GetByIdWithSubMenusAsync(int id);
        Task<TABLE_MENUS?> GetByIdWithDocumentsAsync(int id);
        Task<TABLE_MENUS?> GetByMenuCodeAsync(string menuCode);
    }
}

