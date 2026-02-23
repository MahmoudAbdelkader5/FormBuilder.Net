using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FromBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IDocumentSeriesRepository : IBaseRepository<DOCUMENT_SERIES>
    {
        Task<DOCUMENT_SERIES> GetByIdAsync(int id);
        Task<DOCUMENT_SERIES> GetBySeriesCodeAsync(string seriesCode);
        Task<IEnumerable<DOCUMENT_SERIES>> GetByDocumentTypeIdAsync(int documentTypeId);
        Task<IEnumerable<DOCUMENT_SERIES>> GetByProjectIdAsync(int projectId);
        Task<IEnumerable<DOCUMENT_SERIES>> GetActiveAsync();
        Task<DOCUMENT_SERIES> GetDefaultSeriesAsync(int documentTypeId, int projectId);
        Task<bool> SeriesCodeExistsAsync(string seriesCode, int? excludeId = null);
        Task<bool> IsActiveAsync(int id);
        Task<int> GetNextNumberAsync(int seriesId);
        Task<bool> IsDefaultSeriesAsync(int documentTypeId, int projectId, int seriesId);
        Task<IEnumerable<DOCUMENT_SERIES>> GetByDocumentTypeAndProjectAsync(int documentTypeId, int projectId);
        Task<DOCUMENT_SERIES?> SelectSeriesForSubmissionAsync(int documentTypeId, int projectId);
    }
}