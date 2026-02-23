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
    public class ApprovalStageAssigneesRepository : BaseRepository<APPROVAL_STAGE_ASSIGNEES>, IApprovalStageAssigneesRepository
    {
        private readonly FormBuilderDbContext _context;

        public ApprovalStageAssigneesRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<APPROVAL_STAGE_ASSIGNEES>> GetByStageIdAsync(int stageId)
        {
            return await _context.APPROVAL_STAGE_ASSIGNEES
                .Where(a => a.StageId == stageId && a.IsActive && !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<APPROVAL_STAGE_ASSIGNEES>> GetByUserIdAsync(string userId)
        {
            return await _context.APPROVAL_STAGE_ASSIGNEES
                .Where(a => a.UserId == userId && a.IsActive && !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<APPROVAL_STAGE_ASSIGNEES>> GetByRoleIdAsync(string roleId)
        {
            return await _context.APPROVAL_STAGE_ASSIGNEES
                .Where(a => a.RoleId == roleId && a.IsActive && !a.IsDeleted)
                .ToListAsync();
        }
    }
}

