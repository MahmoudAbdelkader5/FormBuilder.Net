using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.froms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IFormSubmissionsRepository : IBaseRepository<FORM_SUBMISSIONS>
    {
        Task<FORM_SUBMISSIONS> GetByIdAsync(int id);
        Task<FORM_SUBMISSIONS> GetByIdWithDetailsAsync(int id);
        Task<FORM_SUBMISSIONS> GetByDocumentNumberAsync(string documentNumber);
        Task<IEnumerable<FORM_SUBMISSIONS>> GetByFormBuilderIdAsync(int formBuilderId);
        Task<IEnumerable<FORM_SUBMISSIONS>> GetByDocumentTypeIdAsync(int documentTypeId);
        Task<IEnumerable<FORM_SUBMISSIONS>> GetByUserIdAsync(string userId);
        Task<IEnumerable<FORM_SUBMISSIONS>> GetByStatusAsync(string status);
        Task<IEnumerable<FORM_SUBMISSIONS>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> DocumentNumberExistsAsync(string documentNumber);
        Task<int> GetNextVersionAsync(int formBuilderId);
        Task<IEnumerable<FORM_SUBMISSIONS>> GetSubmissionsWithDetailsAsync();
        Task<bool> HasSubmissionsAsync(int formBuilderId);
        Task UpdateStatusAsync(int submissionId, string status);
    }
}