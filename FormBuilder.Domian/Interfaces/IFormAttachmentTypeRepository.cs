using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Domian.Entitys.froms;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IFormAttachmentTypeRepository : IBaseRepository<FORM_ATTACHMENT_TYPES>
    {
        Task<FORM_ATTACHMENT_TYPES> GetByIdAsync(int id);
        Task<IEnumerable<FORM_ATTACHMENT_TYPES>> GetAllAsync(Expression<Func<FORM_ATTACHMENT_TYPES, object>>? include = null);
        Task<IEnumerable<FORM_ATTACHMENT_TYPES>> GetByFormBuilderIdAsync(int formBuilderId);
        Task<IEnumerable<FORM_ATTACHMENT_TYPES>> GetByAttachmentTypeIdAsync(int attachmentTypeId);
        Task<IEnumerable<FORM_ATTACHMENT_TYPES>> GetActiveAsync();
        Task<IEnumerable<FORM_ATTACHMENT_TYPES>> GetActiveByFormBuilderIdAsync(int formBuilderId);
        Task<IEnumerable<FORM_ATTACHMENT_TYPES>> GetMandatoryByFormBuilderIdAsync(int formBuilderId);
        Task<bool> ExistsAsync(int formBuilderId, int attachmentTypeId);
        Task<bool> IsActiveAsync(int id);
        Task<int> DeleteByFormBuilderIdAsync(int formBuilderId);
        Task<bool> HasMandatoryAttachmentsAsync(int formBuilderId);
    }
}