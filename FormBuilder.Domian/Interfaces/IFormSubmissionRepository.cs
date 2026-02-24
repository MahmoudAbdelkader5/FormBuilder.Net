using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.froms;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IFormSubmissionsRepository : IBaseRepository<FORM_SUBMISSIONS>
    {
        Task<FORM_SUBMISSIONS> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<FORM_SUBMISSIONS> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
        Task<FORM_SUBMISSIONS> GetByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken = default);
        Task<IEnumerable<FORM_SUBMISSIONS>> GetByFormBuilderIdAsync(int formBuilderId, CancellationToken cancellationToken = default);
        Task<IEnumerable<FORM_SUBMISSIONS>> GetByDocumentTypeIdAsync(int documentTypeId, CancellationToken cancellationToken = default);
        Task<IEnumerable<FORM_SUBMISSIONS>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<FORM_SUBMISSIONS>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
        Task<IEnumerable<FORM_SUBMISSIONS>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<bool> DocumentNumberExistsAsync(string documentNumber, CancellationToken cancellationToken = default);
        Task<int> GetNextVersionAsync(int formBuilderId, CancellationToken cancellationToken = default);
        Task<IEnumerable<FORM_SUBMISSIONS>> GetSubmissionsWithDetailsAsync(CancellationToken cancellationToken = default);
        Task<bool> HasSubmissionsAsync(int formBuilderId, CancellationToken cancellationToken = default);
        Task UpdateStatusAsync(int submissionId, string status, CancellationToken cancellationToken = default);
    }
}
