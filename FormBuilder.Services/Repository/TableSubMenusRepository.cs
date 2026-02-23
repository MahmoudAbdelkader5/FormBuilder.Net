using FormBuilder.Domian.Interfaces;
using FormBuilder.Infrastructure.Data;
using FormBuilder.Domian.Entitys.FormBuilder;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormBuilder.core;

namespace FormBuilder.Services.Repository
{
    public class TableSubMenusRepository : BaseRepository<TABLE_SUB_MENUS>, ITableSubMenusRepository
    {
        private readonly FormBuilderDbContext _context;

        public TableSubMenusRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TABLE_SUB_MENUS>> GetByMenuIdAsync(int menuId)
        {
            return await _context.TABLE_SUB_MENUS
                .Where(sm => sm.MenuId == menuId && !sm.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<TABLE_SUB_MENUS>> GetActiveByMenuIdAsync(int menuId)
        {
            return await _context.TABLE_SUB_MENUS
                .Where(sm => sm.MenuId == menuId && sm.IsActive && !sm.IsDeleted)
                .ToListAsync();
        }

        public async Task<TABLE_SUB_MENUS?> GetByIdWithDocumentsAsync(int id)
        {
            return await _context.TABLE_SUB_MENUS
                .Include(sm => sm.MenuDocuments.Where(md => md.IsActive && !md.IsDeleted))
                    .ThenInclude(md => md.DocumentType)
                .FirstOrDefaultAsync(sm => sm.Id == id && !sm.IsDeleted);
        }

    }
}

