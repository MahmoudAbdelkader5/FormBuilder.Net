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
    public class ApprovalStageRepository : BaseRepository<APPROVAL_STAGES>, IApprovalStageRepository
    {
        private readonly FormBuilderDbContext _context;

        public ApprovalStageRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<APPROVAL_STAGES>> GetAllAsync()
        {
            return await _context.APPROVAL_STAGES
                .Where(s => !s.IsDeleted)
                .Include(s => s.APPROVAL_WORKFLOWS)
                .ToListAsync();
        }

        public async Task<APPROVAL_STAGES> GetByIdAsync(int id)
        {
            return await _context.APPROVAL_STAGES
                .Where(s => s.Id == id && !s.IsDeleted)
                .Include(s => s.APPROVAL_WORKFLOWS)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _context.APPROVAL_STAGES.AnyAsync(s => s.Id == id && !s.IsDeleted);
        }
    }
}
