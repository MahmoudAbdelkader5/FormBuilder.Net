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
    public class AttachmentTypeRepository : BaseRepository<ATTACHMENT_TYPES>, IAttachmentTypeRepository
    {
        public FormBuilderDbContext _context { get; }

        public AttachmentTypeRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ATTACHMENT_TYPES?> GetByCodeAsync(string code)
        {
            return await _context.ATTACHMENT_TYPES
                .AsNoTracking()
                .FirstOrDefaultAsync(at => at.Code == code && at.IsActive);
        }

        public async Task<IEnumerable<ATTACHMENT_TYPES>> GetActiveAsync()
        {
            return await _context.ATTACHMENT_TYPES
                .AsNoTracking()
                .Where(at => at.IsActive)
                .OrderBy(at => at.Name)
                .ToListAsync();
        }

        public async Task<bool> CodeExistsAsync(string code, int? excludeId = null)
        {
            var query = _context.ATTACHMENT_TYPES
                .AsNoTracking()
                .Where(at => at.Code == code && !at.IsDeleted);

            if (excludeId.HasValue)
            {
                query = query.Where(at => at.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> IsActiveAsync(int id)
        {
            return await _context.ATTACHMENT_TYPES
                .AsNoTracking()
                .AnyAsync(at => at.Id == id && at.IsActive);
        }

        public Task<ATTACHMENT_TYPES?> GetByIdAsync(int id)
        {
            return _context.ATTACHMENT_TYPES
                .AsNoTracking()
                .FirstOrDefaultAsync(at => at.Id == id);
        }
    }
}