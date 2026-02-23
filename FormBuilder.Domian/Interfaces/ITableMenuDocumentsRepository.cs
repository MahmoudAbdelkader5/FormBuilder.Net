using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domian.Interfaces
{
    public interface ITableMenuDocumentsRepository : IBaseRepository<TABLE_MENU_DOCUMENTS>
    {
        Task<IEnumerable<TABLE_MENU_DOCUMENTS>> GetByMenuIdAsync(int menuId);
        Task<IEnumerable<TABLE_MENU_DOCUMENTS>> GetBySubMenuIdAsync(int subMenuId);
        Task<IEnumerable<TABLE_MENU_DOCUMENTS>> GetByDocumentTypeIdAsync(int documentTypeId);
        Task<IEnumerable<TABLE_MENU_DOCUMENTS>> GetActiveByMenuIdAsync(int menuId);
        Task<IEnumerable<TABLE_MENU_DOCUMENTS>> GetActiveBySubMenuIdAsync(int subMenuId);
    }
}

