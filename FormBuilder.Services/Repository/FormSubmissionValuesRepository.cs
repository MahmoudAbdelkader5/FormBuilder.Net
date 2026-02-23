using FormBuilder.Infrastructure.Data;
using FormBuilder.core;
using FormBuilder.Domain.Interfaces.Repositories;
using FormBuilder.Domian.Entitys.froms;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Infrastructure.Repositories
{
    public class FormSubmissionValuesRepository : BaseRepository<FORM_SUBMISSION_VALUES>, IFormSubmissionValuesRepository
    {
        private readonly FormBuilderDbContext _context;

        public FormSubmissionValuesRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<FORM_SUBMISSION_VALUES> GetByIdAsync(int id)
        {
            return await _context.FORM_SUBMISSION_VALUES
                .Include(fsv => fsv.FORM_SUBMISSIONS)
                .Include(fsv => fsv.FORM_FIELDS)
                .FirstOrDefaultAsync(fsv => fsv.Id == id);
        }

        public async Task<IEnumerable<FORM_SUBMISSION_VALUES>> GetBySubmissionIdAsync(int submissionId)
        {
            return await _context.FORM_SUBMISSION_VALUES
                .Include(fsv => fsv.FORM_FIELDS)
                .Where(fsv => fsv.SubmissionId == submissionId)
                .OrderBy(fsv => fsv.FORM_FIELDS.FieldOrder)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORM_SUBMISSION_VALUES>> GetByFieldIdAsync(int fieldId)
        {
            return await _context.FORM_SUBMISSION_VALUES
                .Include(fsv => fsv.FORM_SUBMISSIONS)
                .Where(fsv => fsv.FieldId == fieldId)
                .OrderByDescending(fsv => fsv.FORM_SUBMISSIONS.CreatedDate)
                .ToListAsync();
        }

        public async Task<FORM_SUBMISSION_VALUES> GetBySubmissionAndFieldAsync(int submissionId, int fieldId)
        {
            return await _context.FORM_SUBMISSION_VALUES
                .Include(fsv => fsv.FORM_FIELDS)
                .FirstOrDefaultAsync(fsv => fsv.SubmissionId == submissionId && fsv.FieldId == fieldId);
        }

        public async Task<IEnumerable<FORM_SUBMISSION_VALUES>> GetBySubmissionIdsAsync(List<int> submissionIds)
        {
            return await _context.FORM_SUBMISSION_VALUES
                .Include(fsv => fsv.FORM_FIELDS)
                .Where(fsv => submissionIds.Contains(fsv.SubmissionId))
                .ToListAsync();
        }

        public async Task<bool> DeleteBySubmissionIdAsync(int submissionId)
        {
            var values = await _context.FORM_SUBMISSION_VALUES
                .Where(fsv => fsv.SubmissionId == submissionId && !fsv.IsDeleted)
                .ToListAsync();

            if (values.Any())
            {
                // Soft Delete
                foreach (var value in values)
                {
                    value.IsDeleted = true;
                    value.DeletedDate = DateTime.UtcNow;
                    value.IsActive = false;
                }
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> ExistsBySubmissionAndFieldAsync(int submissionId, int fieldId)
        {
            return await _context.FORM_SUBMISSION_VALUES
                .AnyAsync(fsv => fsv.SubmissionId == submissionId && fsv.FieldId == fieldId && !fsv.IsDeleted);
        }

        public async Task<Dictionary<int, FORM_SUBMISSION_VALUES>> GetFieldValuesDictionaryAsync(int submissionId)
        {
            var values = await _context.FORM_SUBMISSION_VALUES
                .Where(fsv => fsv.SubmissionId == submissionId)
                .ToListAsync();

            return values.ToDictionary(v => v.FieldId, v => v);
        }
    }
}