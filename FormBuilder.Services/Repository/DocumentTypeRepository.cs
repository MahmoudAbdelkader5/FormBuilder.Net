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
    public class DocumentTypeRepository : BaseRepository<DOCUMENT_TYPES>, IDocumentTypeRepository
    {
        public FormBuilderDbContext _context { get; }

        public DocumentTypeRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<DOCUMENT_TYPES?> GetByIdAsync(int id)
        {
            return await _context.DOCUMENT_TYPES
                .Include(dt => dt.FORM_BUILDER)
                .Include(dt => dt.ParentMenu)
                .Include(dt => dt.ApprovalWorkflow)
                .FirstOrDefaultAsync(dt => dt.Id == id && !dt.IsDeleted);
        }

        public async Task<DOCUMENT_TYPES?> GetByCodeAsync(string code)
        {
            return await _context.DOCUMENT_TYPES
                .Include(dt => dt.FORM_BUILDER)
                .FirstOrDefaultAsync(dt => dt.Code == code && dt.IsActive && !dt.IsDeleted);
        }

        public async Task<IEnumerable<DOCUMENT_TYPES>> GetByFormBuilderIdAsync(int formBuilderId)
        {
            return await _context.DOCUMENT_TYPES
                .Include(dt => dt.FORM_BUILDER)
                .Where(dt => dt.FormBuilderId == formBuilderId && !dt.IsDeleted)
                .OrderBy(dt => dt.MenuOrder)
                .ThenBy(dt => dt.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<DOCUMENT_TYPES>> GetActiveAsync()
        {
            return await _context.DOCUMENT_TYPES
                .Include(dt => dt.FORM_BUILDER)
                .Include(dt => dt.ParentMenu)
                .Where(dt => dt.IsActive && !dt.IsDeleted)
                .OrderBy(dt => dt.MenuOrder)
                .ThenBy(dt => dt.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<DOCUMENT_TYPES>> GetByParentMenuIdAsync(int? parentMenuId)
        {
            return await _context.DOCUMENT_TYPES
                .Include(dt => dt.FORM_BUILDER)
                .Where(dt => dt.ParentMenuId == parentMenuId && dt.IsActive && !dt.IsDeleted)
                .OrderBy(dt => dt.MenuOrder)
                .ThenBy(dt => dt.Name)
                .ToListAsync();
        }

        public async Task<bool> CodeExistsAsync(string code, int? excludeId = null)
        {
            var query = _context.DOCUMENT_TYPES.Where(dt => dt.Code == code && !dt.IsDeleted);

            if (excludeId.HasValue)
            {
                query = query.Where(dt => dt.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> IsActiveAsync(int id)
        {
            return await _context.DOCUMENT_TYPES
                .AnyAsync(dt => dt.Id == id && dt.IsActive);
        }

        public async Task<IEnumerable<DOCUMENT_TYPES>> GetMenuItemsAsync()
        {
            return await _context.DOCUMENT_TYPES
                .Include(dt => dt.FORM_BUILDER)
                .Include(dt => dt.Children)
                .Where(dt => dt.IsActive && !dt.IsDeleted)
                .OrderBy(dt => dt.MenuOrder)
                .ThenBy(dt => dt.Name)
                .ToListAsync();
        }

        // Override GetAllAsync to exclude deleted records
        public override async Task<ICollection<DOCUMENT_TYPES>> GetAllAsync(System.Linq.Expressions.Expression<Func<DOCUMENT_TYPES, bool>>? filter = null, params System.Linq.Expressions.Expression<Func<DOCUMENT_TYPES, object>>[] includes)
        {
            // Start with base query excluding deleted records
            var query = _context.DOCUMENT_TYPES
                .Where(dt => !dt.IsDeleted)
                .AsQueryable();
            
            // Apply custom filter if provided
            if (filter != null)
            {
                query = query.Where(filter);
            }
            
            // Apply includes
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            
            // Use AsNoTracking for read operations
            query = query.AsNoTracking();
            
            return await query.ToListAsync();
        }
    }
}