using FormBuilder.Infrastructure.Data;
using FormBuilder.core;
using FormBuilder.Domain.Interfaces.Repositories;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Domian.Entitys.froms;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Infrastructure.Repositories
{
    public class FormSubmissionAttachmentsRepository : BaseRepository<FORM_SUBMISSION_ATTACHMENTS>, IFormSubmissionAttachmentsRepository
    {
        private readonly FormBuilderDbContext _context;

        public FormSubmissionAttachmentsRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<FORM_SUBMISSION_ATTACHMENTS> GetByIdAsync(int id)
        {
            return await _context.FORM_SUBMISSION_ATTACHMENTS
                .Include(fsa => fsa.FORM_SUBMISSIONS)
                .Include(fsa => fsa.FORM_FIELDS)
                .FirstOrDefaultAsync(fsa => fsa.Id == id);
        }

        public async Task<IEnumerable<FORM_SUBMISSION_ATTACHMENTS>> GetBySubmissionIdAsync(int submissionId)
        {
            return await _context.FORM_SUBMISSION_ATTACHMENTS
                .Include(fsa => fsa.FORM_FIELDS)
                .Where(fsa => fsa.SubmissionId == submissionId)
                .OrderByDescending(fsa => fsa.UploadedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORM_SUBMISSION_ATTACHMENTS>> GetByFieldIdAsync(int fieldId)
        {
            return await _context.FORM_SUBMISSION_ATTACHMENTS
                .Include(fsa => fsa.FORM_SUBMISSIONS)
                .Where(fsa => fsa.FieldId == fieldId)
                .OrderByDescending(fsa => fsa.UploadedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORM_SUBMISSION_ATTACHMENTS>> GetBySubmissionAndFieldAsync(int submissionId, int fieldId)
        {
            return await _context.FORM_SUBMISSION_ATTACHMENTS
                .Include(fsa => fsa.FORM_FIELDS)
                .Where(fsa => fsa.SubmissionId == submissionId && fsa.FieldId == fieldId)
                .OrderByDescending(fsa => fsa.UploadedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORM_SUBMISSION_ATTACHMENTS>> GetBySubmissionIdsAsync(List<int> submissionIds)
        {
            return await _context.FORM_SUBMISSION_ATTACHMENTS
                .Include(fsa => fsa.FORM_FIELDS)
                .Where(fsa => submissionIds.Contains(fsa.SubmissionId))
                .ToListAsync();
        }

        public async Task<bool> DeleteBySubmissionIdAsync(int submissionId)
        {
            var attachments = await _context.FORM_SUBMISSION_ATTACHMENTS
                .Where(fsa => fsa.SubmissionId == submissionId && !fsa.IsDeleted)
                .ToListAsync();

            if (attachments.Any())
            {
                // Soft Delete
                foreach (var attachment in attachments)
                {
                    attachment.IsDeleted = true;
                    attachment.DeletedDate = DateTime.UtcNow;
                    attachment.IsActive = false;
                }
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteBySubmissionAndFieldAsync(int submissionId, int fieldId)
        {
            var attachments = await _context.FORM_SUBMISSION_ATTACHMENTS
                .Where(fsa => fsa.SubmissionId == submissionId && fsa.FieldId == fieldId && !fsa.IsDeleted)
                .ToListAsync();

            if (attachments.Any())
            {
                // Soft Delete
                foreach (var attachment in attachments)
                {
                    attachment.IsDeleted = true;
                    attachment.DeletedDate = DateTime.UtcNow;
                    attachment.IsActive = false;
                }
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<long> GetTotalSizeBySubmissionAsync(int submissionId)
        {
            return await _context.FORM_SUBMISSION_ATTACHMENTS
                .Where(fsa => fsa.SubmissionId == submissionId)
                .SumAsync(fsa => fsa.FileSize);
        }

        public async Task<int> GetCountBySubmissionAsync(int submissionId)
        {
            return await _context.FORM_SUBMISSION_ATTACHMENTS
                .CountAsync(fsa => fsa.SubmissionId == submissionId);
        }

        public async Task<bool> FileNameExistsAsync(int submissionId, string fileName)
        {
            return await _context.FORM_SUBMISSION_ATTACHMENTS
                .AnyAsync(fsa => fsa.SubmissionId == submissionId && fsa.FileName == fileName && !fsa.IsDeleted);
        }
    }
}