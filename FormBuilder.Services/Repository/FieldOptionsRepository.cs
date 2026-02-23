using FormBuilder.Infrastructure.Data;
using FormBuilder.core;
using FormBuilder.Domain.Interfaces;
using FormBuilder.Domian.Entitys.froms;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Infrastructure.Repositories
{
    public class FieldOptionsRepository : BaseRepository<FIELD_OPTIONS>, IFieldOptionsRepository
    {
        public FormBuilderDbContext _context { get; }

        public FieldOptionsRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FIELD_OPTIONS>> GetByFieldIdAsync(int fieldId)
        {
            return await _context.FIELD_OPTIONS
                .Where(fo => fo.FieldId == fieldId && !fo.IsDeleted)
                .OrderBy(fo => fo.OptionOrder)
                .ThenBy(fo => fo.OptionText)
                .ToListAsync();
        }

        public async Task<IEnumerable<FIELD_OPTIONS>> GetActiveByFieldIdAsync(int fieldId)
        {
            return await _context.FIELD_OPTIONS
                .Where(fo => fo.FieldId == fieldId && fo.IsActive && !fo.IsDeleted)
                .OrderBy(fo => fo.OptionOrder)
                .ThenBy(fo => fo.OptionText)
                .ToListAsync();
        }

        public async Task<FIELD_OPTIONS> GetDefaultOptionAsync(int fieldId)
        {
            return await _context.FIELD_OPTIONS
                .FirstOrDefaultAsync(fo => fo.FieldId == fieldId && fo.IsDefault && fo.IsActive && !fo.IsDeleted);
        }

        public async Task<bool> FieldHasOptionsAsync(int fieldId)
        {
            return await _context.FIELD_OPTIONS
                .AnyAsync(fo => fo.FieldId == fieldId && fo.IsActive && !fo.IsDeleted);
        }

        public async Task<int> GetOptionsCountAsync(int fieldId)
        {
            return await _context.FIELD_OPTIONS
                .CountAsync(fo => fo.FieldId == fieldId && fo.IsActive && !fo.IsDeleted);
        }

        // Note: GetByIdAsync and ExistsAsync are inherited from BaseRepository
        // No need to override them unless custom logic is required
    }
}