using FormBuilder.Infrastructure.Data;
using FormBuilder.core;
using FormBuilder.Domain.Interfaces.Repositories;
using FormBuilder.Domian.Entitys.FromBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Infrastructure.Repositories
{
    public class FormGridRepository : BaseRepository<FORM_GRIDS>, IFormGridRepository
    {
        private readonly FormBuilderDbContext _context;
        private readonly ILogger<FormGridRepository>? _logger;

        public FormGridRepository(FormBuilderDbContext context, ILogger<FormGridRepository>? logger = null)
            : base(context)
        {
            _context = context;
            _logger = logger;
        }

        // Override or add GetByIdAsync to include navigation properties
        public async Task<FORM_GRIDS?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.FORM_GRIDS
                    .Include(g => g.FORM_BUILDER)
                    .Include(g => g.FORM_TABS)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting form grid by ID: {Id}", id);
                throw;
            }
        }

        // Get all with navigation properties
        public async Task<IEnumerable<FORM_GRIDS>> GetAllAsync()
        {
            try
            {
                return await _context.FORM_GRIDS
                    .Include(g => g.FORM_BUILDER)
                    .Include(g => g.FORM_TABS)
                    .Where(g => !g.IsDeleted)
                    .OrderBy(g => g.FormBuilderId)
                    .ThenBy(g => g.GridOrder)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting all form grids");
                throw;
            }
        }

        public async Task<IEnumerable<FORM_GRIDS>> GetByFormBuilderIdAsync(int formBuilderId)
        {
            try
            {
                return await _context.FORM_GRIDS
                    .Include(g => g.FORM_BUILDER)
                    .Include(g => g.FORM_TABS)
                    .Where(g => g.FormBuilderId == formBuilderId && !g.IsDeleted)
                    .OrderBy(g => g.GridOrder)
                    .ThenBy(g => g.GridName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting grids by form builder ID: {FormBuilderId}", formBuilderId);
                throw;
            }
        }

        public async Task<IEnumerable<FORM_GRIDS>> GetByTabIdAsync(int tabId)
        {
            try
            {
                return await _context.FORM_GRIDS
                    .Include(g => g.FORM_BUILDER)
                    .Include(g => g.FORM_TABS)
                    .Where(g => g.TabId == tabId && !g.IsDeleted)
                    .OrderBy(g => g.GridOrder)
                    .ThenBy(g => g.GridName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting grids by tab ID: {TabId}", tabId);
                throw;
            }
        }

        public async Task<IEnumerable<FORM_GRIDS>> GetActiveByFormBuilderIdAsync(int formBuilderId)
        {
            try
            {
                return await _context.FORM_GRIDS
                    .Include(g => g.FORM_BUILDER)
                    .Include(g => g.FORM_TABS)
                    .Where(g => g.FormBuilderId == formBuilderId && g.IsActive && !g.IsDeleted)
                    .OrderBy(g => g.GridOrder)
                    .ThenBy(g => g.GridName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting active grids by form builder ID: {FormBuilderId}", formBuilderId);
                throw;
            }
        }

        public async Task<FORM_GRIDS?> GetByGridCodeAsync(string gridCode, int formBuilderId)
        {
            try
            {
                return await _context.FORM_GRIDS
                    .Include(g => g.FORM_BUILDER)
                    .Include(g => g.FORM_TABS)
                    .FirstOrDefaultAsync(g =>
                        g.GridCode == gridCode &&
                        g.FormBuilderId == formBuilderId &&
                        !g.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting grid by code: {GridCode}, form builder: {FormBuilderId}",
                    gridCode, formBuilderId);
                throw;
            }
        }

        public async Task<bool> GridCodeExistsAsync(string gridCode, int formBuilderId, int? excludeId = null)
        {
            try
            {
                var query = _context.FORM_GRIDS
                    .Where(g => g.GridCode == gridCode && g.FormBuilderId == formBuilderId && !g.IsDeleted);

                if (excludeId.HasValue)
                {
                    query = query.Where(g => g.Id != excludeId.Value);
                }

                return await query.AnyAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error checking if grid code exists: {GridCode}, form builder: {FormBuilderId}",
                    gridCode, formBuilderId);
                throw;
            }
        }

        public async Task<int> GetNextGridOrderAsync(int formBuilderId, int? tabId = null)
        {
            try
            {
                var query = _context.FORM_GRIDS
                    .Where(g => g.FormBuilderId == formBuilderId);

                if (tabId.HasValue)
                {
                    query = query.Where(g => g.TabId == tabId.Value);
                }

                var maxOrder = await query
                    .MaxAsync(g => (int?)g.GridOrder) ?? 0;

                return maxOrder + 1;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting next grid order for form builder: {FormBuilderId}, tab: {TabId}",
                    formBuilderId, tabId);
                throw;
            }
        }

        public async Task<bool> IsActiveAsync(int id)
        {
            try
            {
                var grid = await _context.FORM_GRIDS
                    .Where(g => g.Id == id)
                    .Select(g => new { g.IsActive })
                    .FirstOrDefaultAsync();

                return grid?.IsActive ?? false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error checking if grid is active: {Id}", id);
                throw;
            }
        }
    }
}