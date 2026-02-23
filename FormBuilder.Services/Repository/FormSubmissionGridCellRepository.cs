using FormBuilder.Infrastructure.Data;
using FormBuilder.core;
using FormBuilder.Domain.Interfaces.Repositories;
using FormBuilder.Domian.Entitys.FormBuilder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Infrastructure.Repositories
{
    public class FormSubmissionGridCellRepository : BaseRepository<FORM_SUBMISSION_GRID_CELLS>, IFormSubmissionGridCellRepository
    {
        private readonly FormBuilderDbContext _context;

        public FormSubmissionGridCellRepository(FormBuilderDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<FORM_SUBMISSION_GRID_CELLS> GetByIdAsync(int id)
        {
            return await _context.FORM_SUBMISSION_GRID_CELLS
                .Include(c => c.FORM_SUBMISSION_GRID_ROWS)
                    .ThenInclude(r => r.FORM_SUBMISSIONS)
                .Include(c => c.FORM_SUBMISSION_GRID_ROWS)
                    .ThenInclude(r => r.FORM_GRIDS)
                .Include(c => c.FORM_GRID_COLUMNS)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<FORM_SUBMISSION_GRID_CELLS>> GetAllAsync()
        {
            return await _context.FORM_SUBMISSION_GRID_CELLS
                .Include(c => c.FORM_SUBMISSION_GRID_ROWS)
                    .ThenInclude(r => r.FORM_SUBMISSIONS)
                .Include(c => c.FORM_SUBMISSION_GRID_ROWS)
                    .ThenInclude(r => r.FORM_GRIDS)
                .Include(c => c.FORM_GRID_COLUMNS)
                .AsNoTracking()
                .OrderBy(c => c.RowId)
                .ThenBy(c => c.ColumnId)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORM_SUBMISSION_GRID_CELLS>> GetByRowIdAsync(int rowId)
        {
            return await _context.FORM_SUBMISSION_GRID_CELLS
                .Include(c => c.FORM_SUBMISSION_GRID_ROWS)
                    .ThenInclude(r => r.FORM_SUBMISSIONS)
                .Include(c => c.FORM_SUBMISSION_GRID_ROWS)
                    .ThenInclude(r => r.FORM_GRIDS)
                .Include(c => c.FORM_GRID_COLUMNS)
                .AsNoTracking()
                .Where(c => c.RowId == rowId && !c.IsDeleted)
                .OrderBy(c => c.ColumnId)
                .ToListAsync();
        }

        public async Task<FORM_SUBMISSION_GRID_CELLS> GetByRowAndColumnAsync(int rowId, int columnId)
        {
            return await _context.FORM_SUBMISSION_GRID_CELLS
                .Include(c => c.FORM_SUBMISSION_GRID_ROWS)
                    .ThenInclude(r => r.FORM_SUBMISSIONS)
                .Include(c => c.FORM_SUBMISSION_GRID_ROWS)
                    .ThenInclude(r => r.FORM_GRIDS)
                .Include(c => c.FORM_GRID_COLUMNS)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.RowId == rowId && c.ColumnId == columnId);
        }

        public async Task<bool> CellExistsAsync(int rowId, int columnId)
        {
            return await _context.FORM_SUBMISSION_GRID_CELLS
                .AnyAsync(c => c.RowId == rowId && c.ColumnId == columnId && !c.IsDeleted);
        }

        public async Task<int> DeleteByRowIdAsync(int rowId)
        {
            var cells = await _context.FORM_SUBMISSION_GRID_CELLS
                .Where(c => c.RowId == rowId && !c.IsDeleted)
                .ToListAsync();

            // Soft Delete
            foreach (var cell in cells)
            {
                cell.IsDeleted = true;
                cell.DeletedDate = DateTime.UtcNow;
                cell.IsActive = false;
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteBySubmissionIdAsync(int submissionId)
        {
            // Get all cells for this submission through their rows
            var cells = await _context.FORM_SUBMISSION_GRID_CELLS
                .Include(c => c.FORM_SUBMISSION_GRID_ROWS)
                .Where(c => c.FORM_SUBMISSION_GRID_ROWS.SubmissionId == submissionId && !c.IsDeleted)
                .ToListAsync();

            // Soft Delete
            foreach (var cell in cells)
            {
                cell.IsDeleted = true;
                cell.DeletedDate = DateTime.UtcNow;
                cell.IsActive = false;
            }
            return await _context.SaveChangesAsync();
        }
    }
}