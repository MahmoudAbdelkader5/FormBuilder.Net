using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domian.Interfaces
{
    public interface ITableSubMenusRepository : IBaseRepository<TABLE_SUB_MENUS>
    {
        Task<IEnumerable<TABLE_SUB_MENUS>> GetByMenuIdAsync(int menuId);
        Task<IEnumerable<TABLE_SUB_MENUS>> GetActiveByMenuIdAsync(int menuId);
        Task<TABLE_SUB_MENUS?> GetByIdWithDocumentsAsync(int id);
    }
}

