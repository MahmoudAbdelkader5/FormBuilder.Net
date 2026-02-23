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
    public class FormSubmissionGridRowRepository : BaseRepository<FORM_SUBMISSION_GRID_ROWS>, IFormSubmissionGridRowRepository
    {
        private readonly FormBuilderDbContext _context;

        public FormSubmissionGridRowRepository(FormBuilderDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<FORM_SUBMISSION_GRID_ROWS?> GetByIdAsync(int id)
        {
            return await _context.FORM_SUBMISSION_GRID_ROWS
                .Include(r => r.FORM_SUBMISSIONS)
                .Include(r => r.FORM_GRIDS)
                    .ThenInclude(g => g.FORM_BUILDER)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<FORM_SUBMISSION_GRID_ROWS>> GetAllAsync()
        {
            return await _context.FORM_SUBMISSION_GRID_ROWS
                .Include(r => r.FORM_SUBMISSIONS)
                .Include(r => r.FORM_GRIDS)
                    .ThenInclude(g => g.FORM_BUILDER)
                .AsNoTracking()
                .OrderBy(r => r.SubmissionId)
                .ThenBy(r => r.GridId)
                .ThenBy(r => r.RowIndex)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORM_SUBMISSION_GRID_ROWS>> GetBySubmissionIdAsync(int submissionId)
        {
            return await _context.FORM_SUBMISSION_GRID_ROWS
                .Include(r => r.FORM_SUBMISSIONS)
                .Include(r => r.FORM_GRIDS)
                    .ThenInclude(g => g.FORM_BUILDER)
                .AsNoTracking()
                .Where(r => r.SubmissionId == submissionId)
                .OrderBy(r => r.GridId)
                .ThenBy(r => r.RowIndex)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORM_SUBMISSION_GRID_ROWS>> GetByGridIdAsync(int gridId)
        {
            return await _context.FORM_SUBMISSION_GRID_ROWS
                .Include(r => r.FORM_SUBMISSIONS)
                .Include(r => r.FORM_GRIDS)
                    .ThenInclude(g => g.FORM_BUILDER)
                .AsNoTracking()
                .Where(r => r.GridId == gridId)
                .OrderBy(r => r.SubmissionId)
                .ThenBy(r => r.RowIndex)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORM_SUBMISSION_GRID_ROWS>> GetBySubmissionAndGridAsync(int submissionId, int gridId)
        {
            return await _context.FORM_SUBMISSION_GRID_ROWS
                .Include(r => r.FORM_SUBMISSIONS)
                .Include(r => r.FORM_GRIDS)
                    .ThenInclude(g => g.FORM_BUILDER)
                .AsNoTracking()
                .Where(r => r.SubmissionId == submissionId && r.GridId == gridId)
                .OrderBy(r => r.RowIndex)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORM_SUBMISSION_GRID_ROWS>> GetActiveRowsAsync(int submissionId, int gridId)
        {
            return await _context.FORM_SUBMISSION_GRID_ROWS
                .Include(r => r.FORM_SUBMISSIONS)
                .Include(r => r.FORM_GRIDS)
                    .ThenInclude(g => g.FORM_BUILDER)
                .AsNoTracking()
                .Where(r => r.SubmissionId == submissionId &&
                            r.GridId == gridId &&
                            r.IsActive)
                .OrderBy(r => r.RowIndex)
                .ToListAsync();
        }

        public async Task<int> GetNextRowIndexAsync(int submissionId, int gridId)
        {
            var maxIndex = await _context.FORM_SUBMISSION_GRID_ROWS
                .AsNoTracking()
                .Where(r => r.SubmissionId == submissionId && r.GridId == gridId)
                .MaxAsync(r => (int?)r.RowIndex) ?? -1;

            return maxIndex + 1;
        }

        public async Task<bool> RowExistsAsync(int submissionId, int gridId, int rowIndex)
        {
            return await _context.FORM_SUBMISSION_GRID_ROWS
                .AsNoTracking()
                .AnyAsync(r => r.SubmissionId == submissionId &&
                               r.GridId == gridId &&
                               r.RowIndex == rowIndex);
        }

        public async Task<int> GetRowCountBySubmissionAsync(int submissionId)
        {
            return await _context.FORM_SUBMISSION_GRID_ROWS
                .AsNoTracking()
                .CountAsync(r => r.SubmissionId == submissionId && r.IsActive);
        }

        public async Task<int> GetRowCountByGridAsync(int gridId)
        {
            return await _context.FORM_SUBMISSION_GRID_ROWS
                .AsNoTracking()
                .CountAsync(r => r.GridId == gridId && r.IsActive);
        }

        public async Task<IEnumerable<FORM_SUBMISSION_GRID_ROWS>> GetByFormBuilderIdAsync(int formBuilderId)
        {
            return await _context.FORM_SUBMISSION_GRID_ROWS
                .Include(r => r.FORM_SUBMISSIONS)
                .Include(r => r.FORM_GRIDS)
                    .ThenInclude(g => g.FORM_BUILDER)
                .AsNoTracking()
                .Where(r => r.FORM_GRIDS != null &&
                            r.FORM_GRIDS.FORM_BUILDER != null &&
                            r.FORM_GRIDS.FORM_BUILDER.Id == formBuilderId)
                .OrderBy(r => r.SubmissionId)
                .ThenBy(r => r.GridId)
                .ThenBy(r => r.RowIndex)
                .ToListAsync();
        }
    }
}