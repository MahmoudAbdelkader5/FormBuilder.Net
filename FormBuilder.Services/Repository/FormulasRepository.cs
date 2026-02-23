using FormBuilder.Infrastructure.Data;
using FormBuilder.core;
using FormBuilder.Domain.Interfaces.Repositories;
using FormBuilder.Domian.Entitys.froms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormBuilder.Infrastructure.Repositories
{
    public class FormulasRepository : BaseRepository<FORMULAS>, IFormulasRepository
    {
        public FormBuilderDbContext _context { get; }

        public FormulasRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        // ====================
        // FORMULA_VARIABLES helper methods implementation
        // ====================

        public async Task<IEnumerable<FORMULA_VARIABLES>> GetFormulaVariablesWithDetailsAsync(int formulaId)
        {
            return await _context.FORMULA_VARIABLES
                .Include(fv => fv.FORMULAS)
                .Include(fv => fv.FORM_FIELDS)
                    .ThenInclude(ff => ff.FORM_TABS)
                    .ThenInclude(ft => ft.FORM_BUILDER)
                .Include(fv => fv.FORM_FIELDS)
                .Where(fv => fv.FormulaId == formulaId)
                .OrderBy(fv => fv.Id)
                .ToListAsync();
        }

        public async Task<int> CountFormulaVariablesAsync(int formulaId)
        {
            return await _context.FORMULA_VARIABLES
                .CountAsync(fv => fv.FormulaId == formulaId);
        }

        public async Task<bool> HasFormulaVariablesAsync(int formulaId)
        {
            return await _context.FORMULA_VARIABLES
                .AnyAsync(fv => fv.FormulaId == formulaId);
        }

        public async Task<IEnumerable<FORM_FIELDS>> GetFieldsByFormBuilderAsync(int formBuilderId)
        {
            return await _context.FORM_FIELDS
                .Include(f => f.FORM_TABS)
                .Where(f => f.FORM_TABS.FormBuilderId == formBuilderId && f.IsActive)
                .OrderBy(f => f.FieldOrder)
                .ToListAsync();
        }

        // ====================
        // Field relationship helper methods implementation
        // ====================

        public async Task<int?> GetFormIdFromFieldIdAsync(int fieldId)
        {
            var field = await _context.FORM_FIELDS
                .Include(f => f.FORM_TABS)
                .FirstOrDefaultAsync(f => f.Id == fieldId);

            return field?.FORM_TABS?.FormBuilderId;
        }

        public async Task<int?> GetFormIdFromFieldCodeAsync(string fieldCode, int formBuilderId)
        {
            var field = await _context.FORM_FIELDS
                .Include(f => f.FORM_TABS)
                .FirstOrDefaultAsync(f => f.FieldCode == fieldCode &&
                                         f.FORM_TABS.FormBuilderId == formBuilderId);

            return field?.FORM_TABS?.FormBuilderId;
        }

        public async Task<bool> FieldBelongsToFormBuilderAsync(int fieldId, int formBuilderId)
        {
            var formId = await GetFormIdFromFieldIdAsync(fieldId);
            return formId.HasValue && formId.Value == formBuilderId;
        }

        public async Task<bool> FieldCodeBelongsToFormBuilderAsync(string fieldCode, int formBuilderId)
        {
            var formId = await GetFormIdFromFieldCodeAsync(fieldCode, formBuilderId);
            return formId.HasValue && formId.Value == formBuilderId;
        }

        // ====================
        // Formula calculation helper implementation
        // ====================

        public async Task<Dictionary<string, FORM_FIELDS>> GetFieldMappingsFromExpressionAsync(string expressionText, int formBuilderId)
        {
            var fieldCodes = ExtractFieldCodesFromExpression(expressionText);
            var fieldMappings = new Dictionary<string, FORM_FIELDS>();

            if (!fieldCodes.Any())
                return fieldMappings;

            var fields = await _context.FORM_FIELDS
                .Include(f => f.FORM_TABS)
                .Where(f => f.FORM_TABS.FormBuilderId == formBuilderId &&
                           fieldCodes.Contains(f.FieldCode) &&
                           f.IsActive)
                .ToListAsync();

            foreach (var field in fields)
            {
                if (!fieldMappings.ContainsKey(field.FieldCode))
                {
                    fieldMappings[field.FieldCode] = field;
                }
            }

            return fieldMappings;
        }

        // باقي الدوال تبقى كما هي...
        // [كل الدوال السابقة تبقى هنا بدون تغيير]
        // ====================
        // Override BaseRepository methods
        // ====================

        public async Task<FORMULAS> GetByIdAsync(int id)
        {
            return await _context.FORMULAS
                .Include(f => f.FORM_BUILDER)
                .Include(f => f.RESULT_FIELD)
                    .ThenInclude(rf => rf.FORM_TABS)
                    .ThenInclude(ft => ft.FORM_BUILDER)
                .Include(f => f.RESULT_FIELD)
                .Include(f => f.FORMULA_VARIABLES)
                    .ThenInclude(fv => fv.FORM_FIELDS)
                        .ThenInclude(ff => ff.FORM_TABS)
                        .ThenInclude(ft => ft.FORM_BUILDER)
                .Include(f => f.FORMULA_VARIABLES)
                    .ThenInclude(fv => fv.FORM_FIELDS)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<FORMULAS>> GetAllAsync()
        {
            return await _context.FORMULAS
                .Include(f => f.FORM_BUILDER)
                .Include(f => f.RESULT_FIELD)
                .Include(f => f.FORMULA_VARIABLES)
                .OrderBy(f => f.FormBuilderId)
                .ThenBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORMULAS>> GetAllAsync(Expression<Func<FORMULAS, bool>>? filter = null, params Expression<Func<FORMULAS, object>>[] includes)
        {
            var query = _context.FORMULAS.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            query = ApplyIncludes(query, includes);

            return await query.ToListAsync();
        }

        public new async Task<FORMULAS> SingleOrDefaultAsync(Expression<Func<FORMULAS, bool>> filter, bool asNoTracking = false, params Expression<Func<FORMULAS, object>>[] includes)
        {
            var query = _context.FORMULAS.AsQueryable();

            query = ApplyIncludes(query, includes);

            if (asNoTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(filter);
        }

        public new async Task<int> CountAsync(Expression<Func<FORMULAS, bool>>? filter = null)
        {
            var query = _context.FORMULAS.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync();
        }

        public new async Task<bool> AnyAsync(Expression<Func<FORMULAS, bool>>? filter = null)
        {
            var query = _context.FORMULAS.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            return await query.AnyAsync();
        }

        // ====================
        // Custom methods for FORMULAS
        // ====================

        public async Task<FORMULAS> GetByCodeAsync(string code, int formBuilderId, params Expression<Func<FORMULAS, object>>[] includes)
        {
            var query = _context.FORMULAS.AsQueryable();

            query = ApplyIncludes(query, includes);

            return await query.FirstOrDefaultAsync(f => f.Code == code && f.FormBuilderId == formBuilderId);
        }

        public async Task<IEnumerable<FORMULAS>> GetByFormBuilderAsync(int formBuilderId, params Expression<Func<FORMULAS, object>>[] includes)
        {
            var query = _context.FORMULAS
                .Where(f => f.FormBuilderId == formBuilderId);

            query = ApplyIncludes(query, includes);

            return await query
                .OrderBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORMULAS>> GetActiveByFormBuilderAsync(int formBuilderId, params Expression<Func<FORMULAS, object>>[] includes)
        {
            var query = _context.FORMULAS
                .Where(f => f.FormBuilderId == formBuilderId && f.IsActive);

            query = ApplyIncludes(query, includes);

            return await query
                .OrderBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORMULAS>> GetByResultFieldAsync(int? resultFieldId, params Expression<Func<FORMULAS, object>>[] includes)
        {
            var query = _context.FORMULAS.AsQueryable();

            if (resultFieldId.HasValue)
            {
                query = query.Where(f => f.ResultFieldId == resultFieldId.Value);
            }
            else
            {
                query = query.Where(f => f.ResultFieldId == null);
            }

            query = ApplyIncludes(query, includes);

            return await query
                .OrderBy(f => f.FormBuilderId)
                .ThenBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<FORMULAS>> GetFormulasWithoutResultFieldAsync(int formBuilderId, params Expression<Func<FORMULAS, object>>[] includes)
        {
            var query = _context.FORMULAS
                .Where(f => f.FormBuilderId == formBuilderId && f.ResultFieldId == null);

            query = ApplyIncludes(query, includes);

            return await query
                .OrderBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<bool> CodeExistsAsync(string code, int formBuilderId, int? excludeId = null)
        {
            var query = _context.FORMULAS
                .Where(f => f.Code == code && f.FormBuilderId == formBuilderId && !f.IsDeleted);

            if (excludeId.HasValue)
            {
                query = query.Where(f => f.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> HasActiveFormulasAsync(int formBuilderId)
        {
            return await _context.FORMULAS
                .AnyAsync(f => f.FormBuilderId == formBuilderId && f.IsActive);
        }

        public async Task<IEnumerable<FORMULAS>> GetFormulasWithDetailsAsync(int formBuilderId = 0)
        {
            var query = _context.FORMULAS
                .Include(f => f.FORM_BUILDER)
                .Include(f => f.RESULT_FIELD)
                    .ThenInclude(rf => rf.FORM_TABS)
                    .ThenInclude(ft => ft.FORM_BUILDER)
                .Include(f => f.RESULT_FIELD)
                .Include(f => f.FORMULA_VARIABLES)
                    .ThenInclude(fv => fv.FORM_FIELDS)
                        .ThenInclude(ff => ff.FORM_TABS)
                        .ThenInclude(ft => ft.FORM_BUILDER)
                .Include(f => f.FORMULA_VARIABLES)
                    .ThenInclude(fv => fv.FORM_FIELDS)
                .AsQueryable();

            if (formBuilderId > 0)
            {
                query = query.Where(f => f.FormBuilderId == formBuilderId);
            }

            return await query
                .OrderBy(f => f.FormBuilderId)
                .ThenBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<FORMULAS> GetByIdWithDetailsAsync(int id)
        {
            return await _context.FORMULAS
                .Include(f => f.FORM_BUILDER)
                .Include(f => f.RESULT_FIELD)
                    .ThenInclude(rf => rf.FORM_TABS)
                    .ThenInclude(ft => ft.FORM_BUILDER)
                .Include(f => f.RESULT_FIELD)
                .Include(f => f.FORMULA_VARIABLES)
                    .ThenInclude(fv => fv.FORM_FIELDS)
                        .ThenInclude(ff => ff.FORM_TABS)
                        .ThenInclude(ft => ft.FORM_BUILDER)
                .Include(f => f.FORMULA_VARIABLES)
                    .ThenInclude(fv => fv.FORM_FIELDS)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<bool> IsActiveAsync(int id)
        {
            return await _context.FORMULAS
                .AnyAsync(f => f.Id == id && f.IsActive);
        }

        // ====================
        // Validation methods
        // ====================

        public async Task<bool> IsExpressionValidForFormAsync(string expressionText, int formBuilderId)
        {
            var fieldCodes = ExtractFieldCodesFromExpression(expressionText);

            if (!fieldCodes.Any())
                return false;

            var existingFields = await _context.FORM_FIELDS
                .Include(f => f.FORM_TABS)
                .Where(f => f.FORM_TABS.FormBuilderId == formBuilderId &&
                           fieldCodes.Contains(f.FieldCode) &&
                           f.IsActive)
                .Select(f => f.FieldCode)
                .ToListAsync();

            return fieldCodes.All(code => existingFields.Contains(code));
        }

        public async Task<IEnumerable<string>> GetReferencedFieldCodesInExpressionAsync(string expressionText)
        {
            var fieldCodes = ExtractFieldCodesFromExpression(expressionText);
            return await Task.FromResult(fieldCodes.Distinct().ToList());
        }

        // ====================
        // Helper methods
        // ====================

        private List<string> ExtractFieldCodesFromExpression(string expressionText)
        {
            var fieldCodes = new List<string>();

            if (string.IsNullOrWhiteSpace(expressionText))
                return fieldCodes;

            var matches = Regex.Matches(expressionText, @"[\[{]([^\]}]+)[\]}]");

            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    fieldCodes.Add(match.Groups[1].Value.Trim());
                }
            }

            return fieldCodes.Distinct().ToList();
        }

        private IQueryable<FORMULAS> ApplyIncludes(IQueryable<FORMULAS> query, params Expression<Func<FORMULAS, object>>[] includes)
        {
            if (includes == null || !includes.Any())
                return query;

            foreach (var include in includes)
                query = query.Include(include);

            return query;
        }

        // ====================
        // Additional helper methods
        // ====================

        public async Task<int> CountByFormBuilderAsync(int formBuilderId)
        {
            return await _context.FORMULAS
                .CountAsync(f => f.FormBuilderId == formBuilderId);
        }

        public async Task<IEnumerable<FORMULAS>> SearchFormulasAsync(string searchTerm, int formBuilderId)
        {
            return await _context.FORMULAS
                .Include(f => f.FORM_BUILDER)
                .Include(f => f.RESULT_FIELD)
                .Where(f => f.FormBuilderId == formBuilderId &&
                           (f.Name.Contains(searchTerm) ||
                            f.Code.Contains(searchTerm) ||
                            f.ExpressionText.Contains(searchTerm)))
                .OrderBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<int> DeleteByFormBuilderAsync(int formBuilderId)
        {
            var formulas = await _context.FORMULAS
                .Where(f => f.FormBuilderId == formBuilderId && !f.IsDeleted)
                .ToListAsync();

            // Soft Delete
            foreach (var formula in formulas)
            {
                formula.IsDeleted = true;
                formula.DeletedDate = DateTime.UtcNow;
                formula.IsActive = false;
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateFormulaExpressionAsync(int id, string expressionText)
        {
            var formula = await _context.FORMULAS.FindAsync(id);
            if (formula == null)
                return false;

            formula.ExpressionText = expressionText;
            formula.UpdatedDate = DateTime.UtcNow;

            _context.FORMULAS.Update(formula);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}