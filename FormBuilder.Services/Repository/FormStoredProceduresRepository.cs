using FormBuilder.Domian.Interfaces;
using FormBuilder.Infrastructure.Data;
using FormBuilder.Domian.Entitys.FormBuilder;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormBuilder.core;

namespace FormBuilder.Services.Repository
{
    public class FormStoredProceduresRepository : BaseRepository<FORM_STORED_PROCEDURES>, IFormStoredProceduresRepository
    {
        private readonly FormBuilderDbContext _context;

        public FormStoredProceduresRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FORM_STORED_PROCEDURES>> GetActiveAsync()
        {
            return await _context.FORM_STORED_PROCEDURES
                .Where(sp => sp.IsActive && !sp.IsDeleted)
                .OrderBy(sp => sp.ExecutionOrder)
                .ThenBy(sp => sp.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORM_STORED_PROCEDURES>> GetByUsageTypeAsync(string? usageType)
        {
            var query = _context.FORM_STORED_PROCEDURES
                .Where(sp => sp.IsActive && !sp.IsDeleted);

            if (!string.IsNullOrWhiteSpace(usageType))
            {
                query = query.Where(sp => sp.UsageType == usageType);
            }

            return await query
                .OrderBy(sp => sp.ExecutionOrder)
                .ThenBy(sp => sp.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORM_STORED_PROCEDURES>> GetByDatabaseAsync(string databaseName)
        {
            return await _context.FORM_STORED_PROCEDURES
                .Where(sp => sp.DatabaseName == databaseName && 
                           sp.IsActive && 
                           !sp.IsDeleted)
                .OrderBy(sp => sp.ExecutionOrder)
                .ThenBy(sp => sp.Title)
                .ToListAsync();
        }

        public async Task<FORM_STORED_PROCEDURES?> GetByDatabaseSchemaAndProcedureAsync(
            string databaseName, 
            string schemaName, 
            string procedureName)
        {
            return await _context.FORM_STORED_PROCEDURES
                .FirstOrDefaultAsync(sp => 
                    sp.DatabaseName == databaseName &&
                    sp.SchemaName == schemaName &&
                    sp.ProcedureName == procedureName &&
                    !sp.IsDeleted);
        }
    }
}

