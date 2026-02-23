using formBuilder.Domian.Interfaces;
using FormBuilder.Infrastructure.Data;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.core;
using FormBuilder.Domian.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Services.Repository
{
    public class FormBuilderRepository
        : BaseRepository<FORM_BUILDER>, IFormBuilderRepository
    {
        private readonly FormBuilderDbContext _context;

        public FormBuilderRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsFormCodeExistsAsync(string formCode, int? excludeId = null)
        {
            return await _context.FORM_BUILDER
                .AnyAsync(f => f.FormCode == formCode &&
                               !f.IsDeleted &&
                               (!excludeId.HasValue || f.Id != excludeId));
        }

        public async Task<FORM_BUILDER?> GetFormWithTabsAndFieldsByCodeAsync(string formCode)
        {
            if (string.IsNullOrWhiteSpace(formCode))
            {
                return null;
            }

            var normalizedCode = formCode.Trim();

            return await _context.FORM_BUILDER
                .AsNoTracking()
                .Include(f => f.FORM_TABS.Where(t => t.IsActive && !t.IsDeleted))
                    .ThenInclude(t => t.FORM_FIELDS.Where(ff => ff.IsActive && !ff.IsDeleted))
                .Include(f => f.FORM_TABS.Where(t => t.IsActive && !t.IsDeleted))
                    .ThenInclude(t => t.FORM_FIELDS.Where(ff => ff.IsActive && !ff.IsDeleted))
                        .ThenInclude(ff => ff.FIELD_OPTIONS.Where(fo => !fo.IsDeleted))
                .Include(f => f.FORM_TABS.Where(t => t.IsActive && !t.IsDeleted))
                    .ThenInclude(t => t.FORM_FIELDS.Where(ff => ff.IsActive && !ff.IsDeleted))
                        .ThenInclude(ff => ff.FIELD_DATA_SOURCES.Where(fds => fds.IsActive && !fds.IsDeleted))
                .FirstOrDefaultAsync(f => f.FormCode == normalizedCode && f.IsActive && f.IsPublished && !f.IsDeleted);
        }

        public async Task<FORM_BUILDER?> GetFormWithAllDataForDuplicateAsync(int id)
        {
            return await _context.FORM_BUILDER
                .Include(f => f.FORM_TABS)
                    .ThenInclude(t => t.FORM_FIELDS)
                        .ThenInclude(ff => ff.FIELD_OPTIONS)
                .Include(f => f.FORM_TABS)
                    .ThenInclude(t => t.FORM_FIELDS)
                        .ThenInclude(ff => ff.FIELD_DATA_SOURCES)
                .Include(f => f.FORMULAS)
                    .ThenInclude(formula => formula.FORMULA_VARIABLES)
                .Include(f => f.FORM_RULES)
                    .ThenInclude(rule => rule.FORM_RULE_ACTIONS)
                .Include(f => f.FORM_GRIDS)
                    .ThenInclude(g => g.FORM_GRID_COLUMNS)
                        .ThenInclude(c => c.GRID_COLUMN_OPTIONS)
                .Include(f => f.FORM_GRIDS)
                    .ThenInclude(g => g.FORM_GRID_COLUMNS)
                        .ThenInclude(c => c.GRID_COLUMN_DATA_SOURCES)
                .Include(f => f.FORM_ATTACHMENT_TYPES)
                .Include(f => f.FORM_BUTTONS)
                .Include(f => f.FORM_VALIDATION_RULES)
                .FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
