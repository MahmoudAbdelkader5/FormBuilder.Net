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
    public class TableMenuDocumentsRepository : BaseRepository<TABLE_MENU_DOCUMENTS>, ITableMenuDocumentsRepository
    {
        private readonly FormBuilderDbContext _context;

        public TableMenuDocumentsRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TABLE_MENU_DOCUMENTS>> GetByMenuIdAsync(int menuId)
        {
            return await _context.TABLE_MENU_DOCUMENTS
                .Include(md => md.DocumentType)
                .Where(md => md.MenuId == menuId && md.SubMenuId == null && !md.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<TABLE_MENU_DOCUMENTS>> GetBySubMenuIdAsync(int subMenuId)
        {
            return await _context.TABLE_MENU_DOCUMENTS
                .Include(md => md.DocumentType)
                .Where(md => md.SubMenuId == subMenuId && !md.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<TABLE_MENU_DOCUMENTS>> GetByDocumentTypeIdAsync(int documentTypeId)
        {
            return await _context.TABLE_MENU_DOCUMENTS
                .Include(md => md.Menu)
                .Include(md => md.SubMenu)
                .Where(md => md.DocumentTypeId == documentTypeId && !md.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<TABLE_MENU_DOCUMENTS>> GetActiveByMenuIdAsync(int menuId)
        {
            return await _context.TABLE_MENU_DOCUMENTS
                .Include(md => md.DocumentType)
                .Where(md => md.MenuId == menuId && md.SubMenuId == null && md.IsActive && !md.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<TABLE_MENU_DOCUMENTS>> GetActiveBySubMenuIdAsync(int subMenuId)
        {
            return await _context.TABLE_MENU_DOCUMENTS
                .Include(md => md.DocumentType)
                .Where(md => md.SubMenuId == subMenuId && md.IsActive && !md.IsDeleted)
                .ToListAsync();
        }
    }
}

