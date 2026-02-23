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
    public class TableMenusRepository : BaseRepository<TABLE_MENUS>, ITableMenusRepository
    {
        private readonly FormBuilderDbContext _context;

        public TableMenusRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TABLE_MENUS>> GetAllActiveAsync()
        {
            return await _context.TABLE_MENUS
                .Where(m => m.IsActive && !m.IsDeleted)
                .ToListAsync();
        }

        public async Task<TABLE_MENUS?> GetByIdWithSubMenusAsync(int id)
        {
            return await _context.TABLE_MENUS
                .Include(m => m.SubMenus.Where(sm => sm.IsActive && !sm.IsDeleted))
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
        }

        public async Task<TABLE_MENUS?> GetByIdWithDocumentsAsync(int id)
        {
            return await _context.TABLE_MENUS
                .Include(m => m.MenuDocuments.Where(md => md.IsActive && !md.IsDeleted))
                    .ThenInclude(md => md.DocumentType)
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
        }

        public async Task<TABLE_MENUS?> GetByMenuCodeAsync(string menuCode)
        {
            return await _context.TABLE_MENUS
                .FirstOrDefaultAsync(m => m.MenuCode == menuCode && !m.IsDeleted);
        }

    }
}

