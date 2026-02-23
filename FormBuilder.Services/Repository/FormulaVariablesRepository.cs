using FormBuilder.Infrastructure.Data;
using FormBuilder.core;
using FormBuilder.Domain.Interfaces.Repositories;
using FormBuilder.Domian.Entitys.froms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FormBuilder.Infrastructure.Repositories
{
    public class FormulaVariablesRepository : BaseRepository<FORMULA_VARIABLES>, IFormulaVariableRepository
    {
        public FormBuilderDbContext _context { get; }

        public FormulaVariablesRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        // Override BaseRepository methods to include relationships
        public async Task<FORMULA_VARIABLES?> GetByIdAsync(int id)
        {
            return await _context.Set<FORMULA_VARIABLES>()
                .Include(fv => fv.FORMULAS)
                .Include(fv => fv.FORM_FIELDS)
                .FirstOrDefaultAsync(fv => fv.Id == id && fv.IsActive);
        }

        public async Task<IEnumerable<FORMULA_VARIABLES>> GetAllAsync()
        {
            return await _context.Set<FORMULA_VARIABLES>()
                .Include(fv => fv.FORMULAS)
                .Include(fv => fv.FORM_FIELDS)
                .Where(fv => fv.IsActive)
                .OrderBy(fv => fv.FormulaId)
                .ThenBy(fv => fv.VariableName)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORMULA_VARIABLES>> GetAllAsync(
            Expression<Func<FORMULA_VARIABLES, bool>>? filter = null,
            params Expression<Func<FORMULA_VARIABLES, object>>[] includes)
        {
            var query = _context.Set<FORMULA_VARIABLES>().AsQueryable();

            // Apply includes
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            else
            {
                // Default includes
                query = query
                    .Include(fv => fv.FORMULAS)
                    .Include(fv => fv.FORM_FIELDS)
;
            }

            // Apply filter if provided
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Always filter by IsActive
            query = query.Where(fv => fv.IsActive);

            return await query
                .OrderBy(fv => fv.FormulaId)
                .ThenBy(fv => fv.VariableName)
                .ToListAsync();
        }

        public new async Task<FORMULA_VARIABLES?> SingleOrDefaultAsync(
            Expression<Func<FORMULA_VARIABLES, bool>> filter,
            bool asNoTracking = false,
            params Expression<Func<FORMULA_VARIABLES, object>>[] includes)
        {
            var query = _context.Set<FORMULA_VARIABLES>().AsQueryable();

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            // Apply includes
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            else
            {
                // Default includes
                query = query
                    .Include(fv => fv.FORMULAS)
                    .Include(fv => fv.FORM_FIELDS)
;
            }

            return await query
                .Where(fv => fv.IsActive)
                .SingleOrDefaultAsync(filter);
        }

        public new async Task<int> CountAsync(Expression<Func<FORMULA_VARIABLES, bool>>? filter = null)
        {
            var query = _context.Set<FORMULA_VARIABLES>()
                .Where(fv => fv.IsActive);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }

        public new async Task<bool> AnyAsync(Expression<Func<FORMULA_VARIABLES, bool>>? filter = null)
        {
            var query = _context.Set<FORMULA_VARIABLES>()
                .Where(fv => fv.IsActive);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.AnyAsync();
        }

        // Custom queries for FORMULA_VARIABLES entity
        public async Task<FORMULA_VARIABLES?> GetByVariableNameAsync(string variableName, int formulaId, params Expression<Func<FORMULA_VARIABLES, object>>[] includes)
        {
            var query = _context.Set<FORMULA_VARIABLES>().AsQueryable();

            // Apply includes
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            else
            {
                // Default includes
                query = query
                    .Include(fv => fv.FORMULAS)
                    .Include(fv => fv.FORM_FIELDS)
;
            }

            return await query
                .Where(fv => fv.FormulaId == formulaId &&
                            fv.VariableName.ToLower() == variableName.ToLower() &&
                            fv.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<FORMULA_VARIABLES>> GetByFormulaIdAsync(int formulaId, params Expression<Func<FORMULA_VARIABLES, object>>[] includes)
        {
            var query = _context.Set<FORMULA_VARIABLES>().AsQueryable();

            // Apply includes
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            else
            {
                // Default includes
                query = query
                    .Include(fv => fv.FORMULAS)
                    .Include(fv => fv.FORM_FIELDS)
;
            }

            return await query
                .Where(fv => fv.FormulaId == formulaId && fv.IsActive)
                .OrderBy(fv => fv.VariableName)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORMULA_VARIABLES>> GetActiveByFormulaIdAsync(int formulaId, params Expression<Func<FORMULA_VARIABLES, object>>[] includes)
        {
            var query = _context.Set<FORMULA_VARIABLES>().AsQueryable();

            // Apply includes
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            else
            {
                // Default includes
                query = query
                    .Include(fv => fv.FORMULAS)
                    .Include(fv => fv.FORM_FIELDS)
;
            }

            return await query
                .Where(fv => fv.FormulaId == formulaId && fv.IsActive)
                .OrderBy(fv => fv.VariableName)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORMULA_VARIABLES>> GetBySourceFieldIdAsync(int sourceFieldId, params Expression<Func<FORMULA_VARIABLES, object>>[] includes)
        {
            var query = _context.Set<FORMULA_VARIABLES>().AsQueryable();

            // Apply includes
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            else
            {
                // Default includes
                query = query
                    .Include(fv => fv.FORMULAS)
                    .Include(fv => fv.FORM_FIELDS)
;
            }

            return await query
                .Where(fv => fv.SourceFieldId == sourceFieldId && fv.IsActive)
                .OrderBy(fv => fv.VariableName)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORMULA_VARIABLES>> GetVariablesWithoutSourceFieldAsync(int formulaId, params Expression<Func<FORMULA_VARIABLES, object>>[] includes)
        {
            var query = _context.Set<FORMULA_VARIABLES>().AsQueryable();

            // Apply includes
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            else
            {
                // Default includes
                query = query
                    .Include(fv => fv.FORMULAS)
                    .Include(fv => fv.FORM_FIELDS)
;
            }

            return await query
                .Where(fv => fv.FormulaId == formulaId &&
                            fv.SourceFieldId == 0 &&
                            fv.IsActive)
                .OrderBy(fv => fv.VariableName)
                .ToListAsync();
        }

        public async Task<bool> VariableNameExistsAsync(string variableName, int formulaId, int? excludeId = null)
        {
            var query = _context.Set<FORMULA_VARIABLES>()
                .Where(fv => fv.FormulaId == formulaId &&
                            fv.VariableName.ToLower() == variableName.ToLower() &&
                            fv.IsActive && !fv.IsDeleted);

            if (excludeId.HasValue)
            {
                query = query.Where(fv => fv.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> HasActiveVariablesAsync(int formulaId)
        {
            return await _context.Set<FORMULA_VARIABLES>()
                .Where(fv => fv.FormulaId == formulaId && fv.IsActive)
                .AnyAsync();
        }

        public async Task<IEnumerable<FORMULA_VARIABLES>> GetVariablesWithDetailsAsync(int formulaId = 0)
        {
            var query = _context.Set<FORMULA_VARIABLES>()
                .Include(fv => fv.FORMULAS)
                    .ThenInclude(f => f.FORM_BUILDER)
                .Include(fv => fv.FORM_FIELDS)
                .Include(fv => fv.FORM_FIELDS)
                    .ThenInclude(ff => ff.FORM_TABS)
                .Where(fv => fv.IsActive);

            if (formulaId > 0)
            {
                query = query.Where(fv => fv.FormulaId == formulaId);
            }

            return await query
                .OrderBy(fv => fv.FormulaId)
                .ThenBy(fv => fv.VariableName)
                .ToListAsync();
        }

        public async Task<FORMULA_VARIABLES?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Set<FORMULA_VARIABLES>()
                .Include(fv => fv.FORMULAS)
                    .ThenInclude(f => f.FORM_BUILDER)
                .Include(fv => fv.FORM_FIELDS)
                .Include(fv => fv.FORM_FIELDS)
                    .ThenInclude(ff => ff.FORM_TABS)
                        .ThenInclude(t => t.FORM_BUILDER)
                .FirstOrDefaultAsync(fv => fv.Id == id && fv.IsActive);
        }

        public async Task<bool> IsActiveAsync(int id)
        {
            return await _context.Set<FORMULA_VARIABLES>()
                .Where(fv => fv.Id == id && fv.IsActive)
                .AnyAsync();
        }

        // Validation methods
        public async Task<bool> IsVariableValidForFormulaAsync(int formulaId, string variableName, int sourceFieldId)
        {
            // Check if formula exists and is active
            var formulaExists = await _context.Set<FORMULAS>()
                .AnyAsync(f => f.Id == formulaId && f.IsActive);

            if (!formulaExists)
                return false;

            // Check if source field exists and belongs to the same form builder
            var formula = await _context.Set<FORMULAS>()
                .FirstOrDefaultAsync(f => f.Id == formulaId);

            if (formula == null)
                return false;

            var fieldBelongsToForm = await _context.Set<FORM_FIELDS>()
                .Include(f => f.FORM_TABS)
                .AnyAsync(f => f.Id == sourceFieldId &&
                              f.FORM_TABS.FormBuilderId == formula.FormBuilderId &&
                              f.IsActive);

            if (!fieldBelongsToForm)
                return false;

            return true;
        }

        public async Task<IEnumerable<string>> GetVariableNamesInFormulaAsync(int formulaId)
        {
            return await _context.Set<FORMULA_VARIABLES>()
                .Where(fv => fv.FormulaId == formulaId && fv.IsActive)
                .Select(fv => fv.VariableName)
                .OrderBy(name => name)
                .ToListAsync();
        }

        // Field relationship helper methods
        public async Task<int?> GetFormulaIdFromVariableIdAsync(int variableId)
        {
            var variable = await _context.Set<FORMULA_VARIABLES>()
                .FirstOrDefaultAsync(fv => fv.Id == variableId && fv.IsActive);

            return variable?.FormulaId;
        }

        public async Task<int?> GetFormulaIdFromVariableNameAsync(string variableName, int formBuilderId)
        {
            var variable = await _context.Set<FORMULA_VARIABLES>()
                .Include(fv => fv.FORMULAS)
                .FirstOrDefaultAsync(fv => fv.VariableName.ToLower() == variableName.ToLower() &&
                                         fv.FORMULAS.FormBuilderId == formBuilderId &&
                                         fv.IsActive);

            return variable?.FORMULAS?.Id;
        }

        public async Task<bool> VariableBelongsToFormulaAsync(int variableId, int formulaId)
        {
            return await _context.Set<FORMULA_VARIABLES>()
                .AnyAsync(fv => fv.Id == variableId &&
                               fv.FormulaId == formulaId &&
                               fv.IsActive && !fv.IsDeleted);
        }

        public async Task<bool> VariableNameBelongsToFormulaAsync(string variableName, int formulaId)
        {
            return await _context.Set<FORMULA_VARIABLES>()
                .AnyAsync(fv => fv.VariableName.ToLower() == variableName.ToLower() &&
                               fv.FormulaId == formulaId &&
                               fv.IsActive && !fv.IsDeleted);
        }

        public async Task<IEnumerable<FORM_FIELDS>> GetSourceFieldsByFormulaIdAsync(int formulaId)
        {
            var fields = await _context.Set<FORMULA_VARIABLES>()
                .Include(fv => fv.FORM_FIELDS)
                .Include(fv => fv.FORM_FIELDS)
                    .ThenInclude(ff => ff.FORM_TABS)
                .Where(fv => fv.FormulaId == formulaId && fv.IsActive && fv.SourceFieldId > 0)
                .Select(fv => fv.FORM_FIELDS)
                .Distinct()
                .ToListAsync();

            return fields;
        }

        // Statistics and counts
        public async Task<IEnumerable<FORMULA_VARIABLES>> GetFormulaVariablesWithDetailsAsync(int formulaId)
        {
            return await _context.Set<FORMULA_VARIABLES>()
                .Include(fv => fv.FORMULAS)
                .Include(fv => fv.FORM_FIELDS)
                .Include(fv => fv.FORM_FIELDS)
                    .ThenInclude(ff => ff.FORM_TABS)
                .Where(fv => fv.FormulaId == formulaId && fv.IsActive)
                .OrderBy(fv => fv.VariableName)
                .ToListAsync();
        }

        public async Task<int> CountFormulaVariablesAsync(int formulaId)
        {
            return await _context.Set<FORMULA_VARIABLES>()
                .Where(fv => fv.FormulaId == formulaId && fv.IsActive)
                .CountAsync();
        }

        public async Task<bool> HasFormulaVariablesAsync(int formulaId)
        {
            return await _context.Set<FORMULA_VARIABLES>()
                .Where(fv => fv.FormulaId == formulaId && fv.IsActive)
                .AnyAsync();
        }

        public async Task<int> CountActiveByFormulaIdAsync(int formulaId)
        {
            return await _context.Set<FORMULA_VARIABLES>()
                .Where(fv => fv.FormulaId == formulaId && fv.IsActive)
                .CountAsync();
        }

        public async Task<int> CountInactiveByFormulaIdAsync(int formulaId)
        {
            return await _context.Set<FORMULA_VARIABLES>()
                .Where(fv => fv.FormulaId == formulaId && !fv.IsActive)
                .CountAsync();
        }

        // Search and filter methods
        public async Task<IEnumerable<FORMULA_VARIABLES>> SearchVariablesAsync(int formulaId, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<FORMULA_VARIABLES>();

            return await _context.Set<FORMULA_VARIABLES>()
                .Include(fv => fv.FORMULAS)
                .Include(fv => fv.FORM_FIELDS)
                .Include(fv => fv.FORM_FIELDS)
                    .ThenInclude(ff => ff.FORM_TABS)
                .Where(fv => fv.FormulaId == formulaId &&
                            fv.IsActive &&
                            (fv.VariableName.Contains(searchTerm) ||
                             fv.FORM_FIELDS.FieldName.Contains(searchTerm) ||
                             fv.FORM_FIELDS.FieldCode.Contains(searchTerm)))
                .OrderBy(fv => fv.VariableName)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORMULA_VARIABLES>> GetByFieldTypeAsync(string fieldType, int formulaId)
        {
            if (string.IsNullOrWhiteSpace(fieldType))
                return new List<FORMULA_VARIABLES>();

            return await _context.Set<FORMULA_VARIABLES>()
                .Include(fv => fv.FORMULAS)
                .Include(fv => fv.FORM_FIELDS)
                .Where(fv => fv.FormulaId == formulaId &&
                            fv.IsActive)
                .OrderBy(fv => fv.VariableName)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORMULA_VARIABLES>> GetByFormBuilderIdAsync(int formBuilderId)
        {
            return await _context.Set<FORMULA_VARIABLES>()
                .Include(fv => fv.FORMULAS)
                .Include(fv => fv.FORM_FIELDS)
                    .ThenInclude(ff => ff.FORM_TABS)
                .Where(fv => fv.FORMULAS.FormBuilderId == formBuilderId &&
                            fv.FORM_FIELDS.FORM_TABS.FormBuilderId == formBuilderId &&
                            fv.IsActive)
                .OrderBy(fv => fv.VariableName)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORMULA_VARIABLES>> GetInactiveByFormulaIdAsync(int formulaId)
        {
            return await _context.Set<FORMULA_VARIABLES>()
                .Include(fv => fv.FORMULAS)
                .Include(fv => fv.FORM_FIELDS)
                .Where(fv => fv.FormulaId == formulaId && !fv.IsActive)
                .OrderBy(fv => fv.VariableName)
                .ToListAsync();
        }

        // Bulk operations
        public async Task<Dictionary<int, int>> GetVariableCountByFormulaAsync(List<int> formulaIds)
        {
            return await _context.Set<FORMULA_VARIABLES>()
                .Where(fv => formulaIds.Contains(fv.FormulaId) && fv.IsActive)
                .GroupBy(fv => fv.FormulaId)
                .Select(g => new { FormulaId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.FormulaId, x => x.Count);
        }

        public async Task<IEnumerable<FORMULA_VARIABLES>> GetVariablesWithFormulaDetailsAsync(int formulaId)
        {
            return await _context.Set<FORMULA_VARIABLES>()
                .Include(fv => fv.FORMULAS)
                    .ThenInclude(f => f.FORM_BUILDER)
                .Include(fv => fv.FORM_FIELDS)
                .Include(fv => fv.FORM_FIELDS)
                    .ThenInclude(ff => ff.FORM_TABS)
                        .ThenInclude(t => t.FORM_BUILDER)
                .Where(fv => fv.FormulaId == formulaId && fv.IsActive)
                .OrderBy(fv => fv.VariableName)
                .ToListAsync();
        }

        // Source field validation
        public async Task<bool> IsSourceFieldUsedAsync(int fieldId)
        {
            return await _context.Set<FORMULA_VARIABLES>()
                .Where(fv => fv.SourceFieldId == fieldId && fv.IsActive)
                .AnyAsync();
        }

        public async Task<bool> IsSourceFieldUsedInOtherFormulasAsync(int fieldId, int excludeFormulaId)
        {
            return await _context.Set<FORMULA_VARIABLES>()
                .Where(fv => fv.SourceFieldId == fieldId &&
                            fv.FormulaId != excludeFormulaId &&
                            fv.IsActive)
                .AnyAsync();
        }

        // Variable mapping helper
        public async Task<Dictionary<string, FORM_FIELDS>> GetVariableMappingsForFormulaAsync(int formulaId)
        {
            var variables = await _context.Set<FORMULA_VARIABLES>()
                .Include(fv => fv.FORM_FIELDS)
                .Where(fv => fv.FormulaId == formulaId && fv.IsActive && fv.SourceFieldId > 0)
                .ToListAsync();

            return variables.ToDictionary(
                v => v.VariableName,
                v => v.FORM_FIELDS);
        }

        public Task<bool> AnyAsync(Func<FORMULA_VARIABLES, bool> predicate)
        {
            return Task.FromResult(_context.Set<FORMULA_VARIABLES>().AsEnumerable().Any(predicate));
        }
    }
}