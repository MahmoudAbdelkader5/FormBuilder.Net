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
    public class GridColumnDataSourcesRepository : BaseRepository<GRID_COLUMN_DATA_SOURCES>, IGridColumnDataSourcesRepository
    {
        public FormBuilderDbContext _context { get; }

        public GridColumnDataSourcesRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GRID_COLUMN_DATA_SOURCES>> GetByColumnIdAsync(int columnId)
        {
            return await _context.GRID_COLUMN_DATA_SOURCES
                .Include(gcds => gcds.FORM_GRID_COLUMNS)
                    .ThenInclude(fgc => fgc.FORM_GRIDS)
                .Where(gcds => gcds.ColumnId == columnId)
                .OrderBy(gcds => gcds.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<GRID_COLUMN_DATA_SOURCES>> GetActiveByColumnIdAsync(int columnId)
        {
            return await _context.GRID_COLUMN_DATA_SOURCES
                .Include(gcds => gcds.FORM_GRID_COLUMNS)
                    .ThenInclude(fgc => fgc.FORM_GRIDS)
                .Where(gcds => gcds.ColumnId == columnId && gcds.IsActive)
                .OrderBy(gcds => gcds.Id)
                .ToListAsync();
        }

        public async Task<GRID_COLUMN_DATA_SOURCES?> GetByColumnIdAsync(int columnId, string sourceType)
        {
            return await _context.GRID_COLUMN_DATA_SOURCES
                .Include(gcds => gcds.FORM_GRID_COLUMNS)
                    .ThenInclude(fgc => fgc.FORM_GRIDS)
                .AsNoTracking()
                .FirstOrDefaultAsync(gcds => gcds.ColumnId == columnId && 
                                             gcds.SourceType == sourceType && 
                                             gcds.IsActive);
        }

        public async Task<bool> ColumnHasDataSourcesAsync(int columnId)
        {
            return await _context.GRID_COLUMN_DATA_SOURCES
                .AnyAsync(gcds => gcds.ColumnId == columnId && gcds.IsActive);
        }

        public async Task<int> GetDataSourcesCountAsync(int columnId)
        {
            return await _context.GRID_COLUMN_DATA_SOURCES
                .CountAsync(gcds => gcds.ColumnId == columnId && gcds.IsActive);
        }

        public async Task<GRID_COLUMN_DATA_SOURCES?> GetByIdAsync(int id)
        {
            return await _context.GRID_COLUMN_DATA_SOURCES
                .Include(gcds => gcds.FORM_GRID_COLUMNS)
                    .ThenInclude(fgc => fgc.FORM_GRIDS)
                        .ThenInclude(fg => fg.FORM_BUILDER)
                .AsNoTracking()
                .FirstOrDefaultAsync(gcds => gcds.Id == id);
        }
    }
}

