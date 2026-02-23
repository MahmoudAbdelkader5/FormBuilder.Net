using FormBuilder.Infrastructure.Data;
using FormBuilder.core;
using FormBuilder.Domain.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Infrastructure.Repositories
{
    public class GridColumnOptionsRepository : BaseRepository<GRID_COLUMN_OPTIONS>, IGridColumnOptionsRepository
    {
        public FormBuilderDbContext _context { get; }

        public GridColumnOptionsRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GRID_COLUMN_OPTIONS>> GetByColumnIdAsync(int columnId)
        {
            return await _context.GRID_COLUMN_OPTIONS
                .Include(gco => gco.FORM_GRID_COLUMNS)
                    .ThenInclude(fgc => fgc.FORM_GRIDS)
                .Where(gco => gco.ColumnId == columnId)
                .OrderBy(gco => gco.OptionOrder)
                .ThenBy(gco => gco.OptionText)
                .ToListAsync();
        }

        public async Task<IEnumerable<GRID_COLUMN_OPTIONS>> GetActiveByColumnIdAsync(int columnId)
        {
            return await _context.GRID_COLUMN_OPTIONS
                .Include(gco => gco.FORM_GRID_COLUMNS)
                    .ThenInclude(fgc => fgc.FORM_GRIDS)
                .Where(gco => gco.ColumnId == columnId && gco.IsActive && !gco.IsDeleted)
                .OrderBy(gco => gco.OptionOrder)
                .ThenBy(gco => gco.OptionText)
                .ToListAsync();
        }

        public async Task<GRID_COLUMN_OPTIONS?> GetDefaultOptionAsync(int columnId)
        {
            return await _context.GRID_COLUMN_OPTIONS
                .Include(gco => gco.FORM_GRID_COLUMNS)
                .AsNoTracking()
                .FirstOrDefaultAsync(gco => gco.ColumnId == columnId && gco.IsDefault && gco.IsActive);
        }

        public async Task<bool> ColumnHasOptionsAsync(int columnId)
        {
            return await _context.GRID_COLUMN_OPTIONS
                .AnyAsync(gco => gco.ColumnId == columnId && gco.IsActive);
        }

        public async Task<int> GetOptionsCountAsync(int columnId)
        {
            return await _context.GRID_COLUMN_OPTIONS
                .CountAsync(gco => gco.ColumnId == columnId && gco.IsActive);
        }
    }
}

