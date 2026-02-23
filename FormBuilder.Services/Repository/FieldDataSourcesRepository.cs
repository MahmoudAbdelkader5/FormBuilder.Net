using DocumentFormat.OpenXml.InkML;
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
    public class FieldDataSourcesRepository : BaseRepository<FIELD_DATA_SOURCES>, IFieldDataSourcesRepository
    {
        public FormBuilderDbContext _context { get; }

        public FieldDataSourcesRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FIELD_DATA_SOURCES>> GetByFieldIdAsync(int fieldId)
        {
            return await _context.FIELD_DATA_SOURCES
                .Where(fds => fds.FieldId == fieldId)
                .OrderBy(fds => fds.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<FIELD_DATA_SOURCES>> GetActiveByFieldIdAsync(int fieldId)
        {
            return await _context.FIELD_DATA_SOURCES
                .Where(fds => fds.FieldId == fieldId && fds.IsActive)
                .OrderBy(fds => fds.Id)
                .ToListAsync();
        }

        public async Task<FIELD_DATA_SOURCES> GetByFieldIdAsync(int fieldId, string sourceType)
        {
            // Case-insensitive comparison for SourceType
            if (string.IsNullOrWhiteSpace(sourceType))
                return null;

            return await _context.FIELD_DATA_SOURCES
                .FirstOrDefaultAsync(fds => fds.FieldId == fieldId 
                    && fds.SourceType.ToUpper() == sourceType.ToUpper() 
                    && fds.IsActive);
        }

        public async Task<bool> FieldHasDataSourcesAsync(int fieldId)
        {
            return await _context.FIELD_DATA_SOURCES
                .AnyAsync(fds => fds.FieldId == fieldId && fds.IsActive);
        }

        public async Task<int> GetDataSourcesCountAsync(int fieldId)
        {
            return await _context.FIELD_DATA_SOURCES
                .CountAsync(fds => fds.FieldId == fieldId && fds.IsActive);
        }
        public async Task<FIELD_DATA_SOURCES> GetByIdAsync(int id)
        {
            return await _context.FIELD_DATA_SOURCES
                .Include(fds => fds.FORM_FIELDS)
                .AsNoTracking()
                .FirstOrDefaultAsync(fds => fds.Id == id);
        }
    }
}