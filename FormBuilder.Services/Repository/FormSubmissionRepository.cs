using FormBuilder.Infrastructure.Data;
using FormBuilder.core;
using FormBuilder.Domain.Interfaces.Repositories;
using FormBuilder.Domian.Entitys.froms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Infrastructure.Repositories
{
    public class FormSubmissionsRepository : BaseRepository<FORM_SUBMISSIONS>, IFormSubmissionsRepository
    {
        private readonly FormBuilderDbContext _context;

        public FormSubmissionsRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<FORM_SUBMISSIONS?> GetByIdAsync(int id)
        {
            return await _context.FORM_SUBMISSIONS
                .Include(fs => fs.FORM_BUILDER)
                .Include(fs => fs.DOCUMENT_TYPES)
                .Include(fs => fs.DOCUMENT_SERIES)
                .AsNoTracking()
                .FirstOrDefaultAsync(fs => fs.Id == id);
        }

        public async Task<FORM_SUBMISSIONS?> GetByIdWithDetailsAsync(int id)
        {
            var submission = await _context.FORM_SUBMISSIONS
                .Include(fs => fs.FORM_BUILDER)
                .Include(fs => fs.DOCUMENT_TYPES)
                .Include(fs => fs.DOCUMENT_SERIES)
                .Include(fs => fs.FORM_SUBMISSION_VALUES)
                    .ThenInclude(fv => fv.FORM_FIELDS)
                .Include(fs => fs.FORM_SUBMISSION_ATTACHMENTS)
                    .ThenInclude(fa => fa.FORM_FIELDS)
                .Include(fs => fs.FORM_SUBMISSION_GRID_ROWS.Where(gr => !gr.IsDeleted))
                    .ThenInclude(gr => gr.FORM_GRIDS)
                // Note: Cells are loaded separately in Service layer to avoid tracking conflicts
                // and to handle AsNoTracking() limitations with filtered collections
                .AsNoTracking()
                .FirstOrDefaultAsync(fs => fs.Id == id && !fs.IsDeleted);

            // Note: Cells are loaded separately in Service layer due to AsNoTracking() limitations
            // The Include chain above will attempt to load cells, but if it fails due to filtering,
            // the Service layer will load them explicitly using DbContext
            
            return submission;
        }

        public async Task<FORM_SUBMISSIONS?> GetByDocumentNumberAsync(string documentNumber)
        {
            return await _context.FORM_SUBMISSIONS
                .Include(fs => fs.FORM_BUILDER)
                .Include(fs => fs.DOCUMENT_TYPES)
                .Include(fs => fs.DOCUMENT_SERIES)
                .FirstOrDefaultAsync(fs => fs.DocumentNumber == documentNumber && !fs.IsDeleted);
        }

        public async Task<IEnumerable<FORM_SUBMISSIONS>> GetByFormBuilderIdAsync(int formBuilderId)
        {
            return await _context.FORM_SUBMISSIONS
                .Include(fs => fs.DOCUMENT_TYPES)
                .Include(fs => fs.DOCUMENT_SERIES)
                .Where(fs => fs.FormBuilderId == formBuilderId)
                .OrderByDescending(fs => fs.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORM_SUBMISSIONS>> GetByDocumentTypeIdAsync(int documentTypeId)
        {
            return await _context.FORM_SUBMISSIONS
                .Include(fs => fs.FORM_BUILDER)
                .Include(fs => fs.DOCUMENT_SERIES)
                .Where(fs => fs.DocumentTypeId == documentTypeId)
                .OrderByDescending(fs => fs.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORM_SUBMISSIONS>> GetByUserIdAsync(string userId)
        {
            return await _context.FORM_SUBMISSIONS
                .Include(fs => fs.FORM_BUILDER)
                .Include(fs => fs.DOCUMENT_TYPES)
                .Include(fs => fs.DOCUMENT_SERIES)
                .Where(fs => fs.SubmittedByUserId == userId)
                .OrderByDescending(fs => fs.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORM_SUBMISSIONS>> GetByStatusAsync(string status)
        {
            return await _context.FORM_SUBMISSIONS
                .Include(fs => fs.FORM_BUILDER)
                .Include(fs => fs.DOCUMENT_TYPES)
                .Include(fs => fs.DOCUMENT_SERIES)
                .Where(fs => fs.Status == status)
                .OrderByDescending(fs => fs.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORM_SUBMISSIONS>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.FORM_SUBMISSIONS
                .Include(fs => fs.FORM_BUILDER)
                .Include(fs => fs.DOCUMENT_TYPES)
                .Include(fs => fs.DOCUMENT_SERIES)
                .Where(fs => fs.CreatedDate >= startDate && fs.CreatedDate <= endDate)
                .OrderByDescending(fs => fs.CreatedDate)
                .ToListAsync();
        }

        public async Task<bool> DocumentNumberExistsAsync(string documentNumber)
        {
            return await _context.FORM_SUBMISSIONS
                .AnyAsync(fs => fs.DocumentNumber == documentNumber && !fs.IsDeleted);
        }

        public async Task<int> GetNextVersionAsync(int formBuilderId)
        {
            var currentVersion = await _context.FORM_SUBMISSIONS
                .Where(fs => fs.FormBuilderId == formBuilderId && !fs.IsDeleted)
                .MaxAsync(fs => (int?)fs.Version) ?? 0;

            return currentVersion + 1;
        }

        public async Task<IEnumerable<FORM_SUBMISSIONS>> GetSubmissionsWithDetailsAsync()
        {
            return await _context.FORM_SUBMISSIONS
                .Include(fs => fs.FORM_BUILDER)
                .Include(fs => fs.DOCUMENT_TYPES)
                .Include(fs => fs.DOCUMENT_SERIES)
                .OrderByDescending(fs => fs.CreatedDate)
                .ToListAsync();
        }

        public async Task<bool> HasSubmissionsAsync(int formBuilderId)
        {
            return await _context.FORM_SUBMISSIONS
                .AnyAsync(fs => fs.FormBuilderId == formBuilderId && !fs.IsDeleted);
        }

        public async Task UpdateStatusAsync(int submissionId, string status)
        {
            var submission = await _context.FORM_SUBMISSIONS.FindAsync(submissionId);
            if (submission != null)
            {
                submission.Status = status;
                submission.UpdatedDate = DateTime.UtcNow;
                _context.FORM_SUBMISSIONS.Update(submission);
                await _context.SaveChangesAsync();
            }
        }
    }
}