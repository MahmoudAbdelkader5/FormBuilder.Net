using FormBuilder.Infrastructure.Data;
using FormBuilder.core;
using FormBuilder.Domain.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace FormBuilder.Infrastructure.Repositories
{
    public class FieldTypesRepository : BaseRepository<FIELD_TYPES>, IFieldTypesRepository
    {
        public FormBuilderDbContext _context { get; }

        public FieldTypesRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<FIELD_TYPES?> GetByIdAsync(int id)
        {
            return await _context.Set<FIELD_TYPES>()
                .FirstOrDefaultAsync(ft => ft.Id == id && !ft.IsDeleted);
        }

        public async Task<IEnumerable<FIELD_TYPES>> GetActiveAsync()
        {
            return await _context.Set<FIELD_TYPES>()
                .Where(ft => ft.IsActive && !ft.IsDeleted)
                .OrderBy(ft => ft.TypeName)
                .ToListAsync();
        }

        public async Task<FIELD_TYPES?> GetByTypeNameAsync(string typeName)
        {
            return await _context.Set<FIELD_TYPES>()
                .FirstOrDefaultAsync(ft => ft.TypeName == typeName && !ft.IsDeleted);
        }

        public async Task<bool> TypeNameExistsAsync(string typeName, int? excludeId = null)
        {
            var query = _context.Set<FIELD_TYPES>()
                .Where(ft => ft.TypeName == typeName && !ft.IsDeleted);

            if (excludeId.HasValue)
            {
                query = query.Where(ft => ft.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<int> GetUsageCountAsync(int fieldTypeId)
        {
            var fieldsCount = await _context.FORM_FIELDS
                .CountAsync(f => f.FieldTypeId == fieldTypeId && !f.IsDeleted);

            var columnsCount = await _context.FORM_GRID_COLUMNS
                .CountAsync(c => c.FieldTypeId == fieldTypeId && !c.IsDeleted);

            return fieldsCount + columnsCount;
        }
    }
}

