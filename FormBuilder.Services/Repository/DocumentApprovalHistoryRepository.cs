using FormBuilder.Infrastructure.Data;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domain.Interfaces.Repositories;
using FormBuilder.core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Infrastructure.Repositories
{
    public class DocumentApprovalHistoryRepository : BaseRepository<DOCUMENT_APPROVAL_HISTORY>, IDocumentApprovalHistoryRepository
    {
        private readonly FormBuilderDbContext _context;

        public DocumentApprovalHistoryRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DOCUMENT_APPROVAL_HISTORY>> GetBySubmissionIdAsync(int submissionId)
        {
            return await _context.DOCUMENT_APPROVAL_HISTORY
                .Where(h => h.SubmissionId == submissionId)
                .OrderBy(h => h.ActionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<DOCUMENT_APPROVAL_HISTORY>> GetByStageIdAsync(int stageId)
        {
            return await _context.DOCUMENT_APPROVAL_HISTORY
                .Where(h => h.StageId == stageId)
                .OrderBy(h => h.ActionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<DOCUMENT_APPROVAL_HISTORY>> GetByUserIdAsync(string userId)
        {
            return await _context.DOCUMENT_APPROVAL_HISTORY
                .Where(h => h.ActionByUserId == userId)
                .OrderByDescending(h => h.ActionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<DOCUMENT_APPROVAL_HISTORY>> GetAllApprovalHistoryAsync()
        {
            return await _context.DOCUMENT_APPROVAL_HISTORY
                .Include(h => h.FORM_SUBMISSIONS)
                    .ThenInclude(s => s.DOCUMENT_TYPES)
                .Include(h => h.FORM_SUBMISSIONS)
                    .ThenInclude(s => s.FORM_BUILDER)
                .Include(h => h.APPROVAL_STAGES)
                .Where(h => h.ActionType == "Approved" || h.ActionType == "Rejected")
                .OrderByDescending(h => h.ActionDate)
                .ToListAsync();
        }

        public async Task DeleteBySubmissionIdAsync(int submissionId)
        {
            var records = await _context.DOCUMENT_APPROVAL_HISTORY
                .Where(h => h.SubmissionId == submissionId && !h.IsDeleted)
                .ToListAsync();
            
            if (records.Any())
            {
                // Soft Delete - لا نحذف فعلياً، فقط نعلّم كمحذوف
                foreach (var record in records)
                {
                    record.IsDeleted = true;
                    record.DeletedDate = DateTime.UtcNow;
                    record.IsActive = false;
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}

