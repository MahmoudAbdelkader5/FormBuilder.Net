using FormBuilder.Infrastructure.Data;
using FormBuilder.core;
using FormBuilder.Domain.Interfaces.Repositories;
using FormBuilder.Domian.Entitys.FromBuilder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Infrastructure.Repositories
{
    public class DocumentSeriesRepository : BaseRepository<DOCUMENT_SERIES>, IDocumentSeriesRepository
    {
        public FormBuilderDbContext _context { get; }

        public DocumentSeriesRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<DOCUMENT_SERIES?> GetByIdAsync(int id)
        {
            return await _context.DOCUMENT_SERIES
                .Include(ds => ds.PROJECTS)
                .FirstOrDefaultAsync(ds => ds.Id == id && !ds.IsDeleted);
        }

        public async Task<DOCUMENT_SERIES?> GetBySeriesCodeAsync(string seriesCode)
        {
            return await _context.DOCUMENT_SERIES
                .Include(ds => ds.PROJECTS)
                .FirstOrDefaultAsync(ds => ds.SeriesCode == seriesCode && ds.IsActive && !ds.IsDeleted);
        }

        public async Task<IEnumerable<DOCUMENT_SERIES>> GetByDocumentTypeIdAsync(int documentTypeId)
        {
            // DocumentType is no longer part of DOCUMENT_SERIES; keep method for backward compatibility.
            return await _context.DOCUMENT_SERIES
                .Include(ds => ds.PROJECTS)
                .Where(ds => ds.IsActive && !ds.IsDeleted)
                .OrderBy(ds => ds.SeriesCode)
                .ToListAsync();
        }

        public async Task<IEnumerable<DOCUMENT_SERIES>> GetByProjectIdAsync(int projectId)
        {
            return await _context.DOCUMENT_SERIES
                .Include(ds => ds.PROJECTS)
                .Where(ds => ds.ProjectId == projectId && ds.IsActive && !ds.IsDeleted)
                .OrderBy(ds => ds.SeriesCode)
                .ToListAsync();
        }

        public async Task<IEnumerable<DOCUMENT_SERIES>> GetActiveAsync()
        {
            return await _context.DOCUMENT_SERIES
                .Include(ds => ds.PROJECTS)
                .Where(ds => ds.IsActive && !ds.IsDeleted)
                .OrderBy(ds => ds.SeriesCode)
                .ToListAsync();
        }

        public async Task<DOCUMENT_SERIES?> GetDefaultSeriesAsync(int documentTypeId, int projectId)
        {
            // DocumentType is no longer part of DOCUMENT_SERIES; keep method signature for compatibility.
            return await _context.DOCUMENT_SERIES
                .Include(ds => ds.PROJECTS)
                .FirstOrDefaultAsync(ds => ds.ProjectId == projectId &&
                                         ds.IsDefault &&
                                         ds.IsActive &&
                                         !ds.IsDeleted);
        }

        public async Task<bool> SeriesCodeExistsAsync(string seriesCode, int? excludeId = null)
        {
            var query = _context.DOCUMENT_SERIES.Where(ds => ds.SeriesCode == seriesCode && !ds.IsDeleted);

            if (excludeId.HasValue)
            {
                query = query.Where(ds => ds.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> IsActiveAsync(int id)
        {
            return await _context.DOCUMENT_SERIES
                .AnyAsync(ds => ds.Id == id && ds.IsActive && !ds.IsDeleted);
        }

        /// <summary>
        /// Atomically gets and increments the next number for a document series.
        /// Uses database transaction with pessimistic locking (UPDLOCK) to ensure atomic operation and prevent race conditions.
        /// </summary>
        public async Task<int> GetNextNumberAsync(int seriesId)
        {
            // Use a transaction with pessimistic locking to ensure atomic increment
            // This prevents race conditions when multiple requests try to get the next number simultaneously
            var database = _context.Database;
            var ownsTransaction = database.CurrentTransaction == null;
            using var transaction = ownsTransaction
                ? await database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted)
                : null;

            try
            {
                // Use pessimistic locking (UPDLOCK, ROWLOCK) to prevent concurrent access
                // UPDLOCK: Takes an update lock that prevents other transactions from reading or modifying the row
                // ROWLOCK: Locks only the specific row, not the entire table
                var series = await _context.DOCUMENT_SERIES
                    .FromSqlRaw("SELECT * FROM DOCUMENT_SERIES WITH (UPDLOCK, ROWLOCK) WHERE Id = {0}", seriesId)
                    .FirstOrDefaultAsync();

                if (series == null)
                {
                    if (ownsTransaction && transaction != null)
                        await transaction.RollbackAsync();
                    return -1;
                }

                var nextNumber = series.NextNumber;
                series.NextNumber++;
                series.UpdatedDate = DateTime.UtcNow;
                
                _context.DOCUMENT_SERIES.Update(series);
                await _context.SaveChangesAsync();

                if (ownsTransaction && transaction != null)
                    await transaction.CommitAsync();

                return nextNumber;
            }
            catch
            {
                if (ownsTransaction && transaction != null)
                    await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Gets all active series for a document type and project combination.
        /// Used for series selection logic.
        /// </summary>
        public async Task<IEnumerable<DOCUMENT_SERIES>> GetByDocumentTypeAndProjectAsync(int documentTypeId, int projectId)
        {
            // DocumentType is no longer part of DOCUMENT_SERIES; keep method signature for compatibility.
            return await _context.DOCUMENT_SERIES
                .Include(ds => ds.PROJECTS)
                .Where(ds => ds.ProjectId == projectId &&
                             ds.IsActive &&
                             !ds.IsDeleted)
                .OrderByDescending(ds => ds.IsDefault) // Default series first
                .ThenBy(ds => ds.SeriesCode)
                .ToListAsync();
        }

        /// <summary>
        /// Selects the appropriate series for a document type and project based on the selection rules:
        /// 1. If only one series exists, it is selected automatically
        /// 2. If multiple series exist: Match by Project, Select default series
        /// </summary>
        public async Task<DOCUMENT_SERIES?> SelectSeriesForSubmissionAsync(int documentTypeId, int projectId)
        {
            var series = await GetByDocumentTypeAndProjectAsync(documentTypeId, projectId);
            var seriesList = series.ToList();

            if (!seriesList.Any())
                return null;

            // If only one series exists, select it automatically
            if (seriesList.Count == 1)
                return seriesList.First();

            // If multiple series exist: Select default series
            return seriesList.FirstOrDefault(s => s.IsDefault) ?? seriesList.First();
        }

        public async Task<bool> IsDefaultSeriesAsync(int documentTypeId, int projectId, int seriesId)
        {
            // DocumentType is no longer part of DOCUMENT_SERIES; keep method signature for compatibility.
            return await _context.DOCUMENT_SERIES
                .AnyAsync(ds => ds.Id == seriesId &&
                              ds.ProjectId == projectId &&
                              ds.IsDefault &&
                              !ds.IsDeleted);
        }
    }
}
